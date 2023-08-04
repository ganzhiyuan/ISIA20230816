using TAP.UPDATER.Downloaders;
using TAP.UPDATER.ExceptionHandler;
using TAP.UPDATER.ExceptionHandler.Exceptions;
using TAP.UPDATER.FileSystem;
using TAP.UPDATER.Hash;
using TAP.UPDATER.Resources;
using TAP.UPDATER.Resources.Configs;
using TAP.UPDATER.Resources.TextResources;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace TAP.UPDATER.Engine {
    /// <summary>
    /// A pipeline-action-based engine class, with all its core logic behind the Auto-Updater.
    /// </summary>
    public class UpdateCheckEngine : IUpdateCheckEngine
    {
        /// <summary>
        /// The LocalMetadata stores the metadata obtained from the local files.
        /// It is stored in a thread-safe structure since it is going to be accessed concurrently.
        /// </summary>
        private static ConcurrentDictionary<string, FileMetadata> LocalMetadata;

        /// <summary>
        /// The ServerMetadata stores the metadata obtained from the server.
        /// </summary>
        private static Dictionary<string, FileMetadata> ServerMetadata;

        /// <summary>
        /// The <c>BackgroundWorker</c> instance, used to contact the UI Thread.
        /// </summary>
        private static BackgroundWorker BW;

        /// <summary>
        /// The <c>IDownloader</c>, used to download resources.
        /// </summary>
        private static IDownloader Downloader;

        /// <summary>
        /// The <c>IHasher</c>, used to compute any hash operation whenever appropriate.
        /// </summary>
        private static IHasher Hasher;

        /// <summary>
        /// The url to the actual server directory with the files.
        /// </summary>
        private static string PatchDirectory;

        private static string PatchAddress;

        /// <summary>
        /// A pipeline containing the steps to follow in order to try to guarantee a successful patch.
        /// Every action-step knows its relative order.
        /// </summary>
        private static readonly Tuple<Action<int>, string>[] Pipeline = {
                new Tuple<Action<int>, string>(GenerateServerMetadata,
                PatcherEngineResources.PARSING_SERVER_METADATA)
                //,
                //new Tuple<Action<int>, string>(DownloadMissingContent,
                //PatcherEngineResources.CHECKING_MISSING_CONTENT),
                //new Tuple<Action<int>, string>(DownloadOutdatedContent,
                //PatcherEngineResources.CHECKING_OUTDATED_CONTENT)
            };

        /// <summary>
        /// Initializes a new instance of the <c>PatcherEngine</c> class with the specified <c>BackgroundWorker</c>.
        /// </summary>
        public UpdateCheckEngine(string address) {
            BW = new BackgroundWorker();
            Hasher = new Md5Hasher();
            Downloader = new HttpClientDownloader(BW, Hasher);
            PatchAddress = address;
        }

        /// <summary>
        /// Performs the required steps in order to try to fully patch the client.
        /// The time it takes for it to patch is measured through a <c>Stopwatch</c>.
        /// Every current step's description is outputted to the respective <c>Label</c> in the UI Thread through the BackgroundWorker.
        /// </summary>
        bool IUpdateCheckEngine.Check() {
            bool bResult = false;
            Debug.WriteLine("Start - Check files");
            for (int i = 0; i < Pipeline.Length; i++) {
                Pipeline[i].Item1.Invoke(i);                
            }
            Debug.WriteLine("End = Check files");


            Debug.WriteLine("Start - CalculateMissingContent");
            List<string> missingFiles = CalculateMissingContent();
            Debug.WriteLine(missingFiles);
            if (missingFiles.Count > 0)
            {
                foreach (string temp in missingFiles)
                {
                    Debug.WriteLine(string.Format("Find MissingContent - {0}", temp));
                }
                return true;
            }
            Debug.WriteLine("End - CalculateMissingContent");

            Debug.WriteLine("Start - GenerateLocalMetadata");
            GenerateLocalMetadata();
            Debug.WriteLine("End - GenerateLocalMetadata");
            Debug.WriteLine("Start - CalculateOutdatedContent");
            List<string> OutDataFiles = CalculateOutdatedContent();
            Debug.WriteLine(OutDataFiles);
            if (OutDataFiles.Count > 0)
            {
                foreach (string temp in OutDataFiles)
                {
                    Debug.WriteLine(string.Format("Find OutdatedContent - {0}", temp));
                }
                return true;
            }
            Debug.WriteLine("End - CalculateOutdatedContent");
            return bResult;
        }

        /// <summary>
        /// Invokes the <c>FileSystemExplorer</c> to try to download and write a list of specific files (content) to the disk.
        /// isMissingContent is true if the method is invoked as a mean to download missing content, or false if to download outdated content.
        /// After the downloads and writes are completed, the local metadata is refreshed.
        /// </summary>
        private static void DownloadContent(int step, List<string> content, bool isMissingContent) {            
            for (int i = 0; i < content.Count; i++) {
                // The expected hash of the file to be downloaded is already saved in the ServerMetadata.
                FileSystemExplorer.FetchFile(Downloader, content[i], PatchDirectory + content[i], isMissingContent, ServerMetadata[content[i]].Hash);                
            }
            // Give time to AntiVirus for it to delete or tamper any of the recently downloaded files.
            Thread.Sleep(EngineConfigs.MS_TO_WAIT_FOR_AV_FALSE_POSITIVES);
            if (!isMissingContent)
                FileSystemExplorer.ApplyUpdate(content);
            GenerateLocalMetadata();
        }

        /// <summary>
        /// This method downloads the server's metadata file into memory.
        /// It then parses it, and stores its content in the ServerMetadata global variable.
        /// </summary>
        private static void GenerateServerMetadata(int step) {
            Debug.WriteLine("Start - GenerateServerMetadata");
            Debug.WriteLine(string.Format("PatchAddress - {0}", EngineConfigs.PATCH_METADATA));
            string[] metadataByLine = Encoding.Default.GetString(Downloader.DefaultDownloadDataToMemory(PatchAddress)).Trim().Split(new[] { "\n" }, StringSplitOptions.None);
            // Assume that the first line of the server's metadata file is the url to the actual server directory with the files.
            PatchDirectory = metadataByLine[0].Trim();
            ServerMetadata = new Dictionary<string, FileMetadata>((metadataByLine.Length - 1) / 2);
            // Every odd line number represents a file name, and every even line number its hash.
            for (int i = 1; i < metadataByLine.Length; i += 2)
                ServerMetadata[metadataByLine[i].Trim()] = new FileMetadata(metadataByLine[i].Trim(), metadataByLine[i + 1].Trim());

            Debug.WriteLine("End - GenerateServerMetadata");
        }

        /// <summary>
        /// Returns a list containing the server files' names that are not present locally.
        /// </summary>
        private static List<string> CalculateMissingContent() {
            List<string> missingFiles = new List<string>();
            foreach (string file in ServerMetadata.Keys)
                if (!FileSystemExplorer.FileExists(file))
                    missingFiles.Add(file);
            return missingFiles;
        }

        /// <summary>
        /// Returns a list containing the server files' names which their hash differs from the respective local files' hash.
        /// </summary>
        private static List<string> CalculateOutdatedContent() {
            List<string> outdatedFiles = new List<string>();
            foreach (KeyValuePair<string, FileMetadata> entry in ServerMetadata)
                if (!entry.Value.Hash.Equals(LocalMetadata[entry.Key].Hash))
                    outdatedFiles.Add(entry.Key);
            return outdatedFiles;
        }

        /// <summary>
        /// Asks the FileSystemExplorer for a fresh copy of the local metadata performed in a concurrent way.
        /// </summary>
        private static void GenerateLocalMetadata() {
            // The local metadata refreshed is based only on the contents available in the server metadata.
            // Do it with a concurrency level equal to half the number of processors.
            LocalMetadata = FileSystemExplorer.GenerateLocalMetadata(ServerMetadata.Keys.ToArray(), Hasher, Math.Max(1, Environment.ProcessorCount / 2));
        }

        /// <summary>
        /// Invokes <c>CalculateMissingContent()</c> and then downloads the received list of missing files.
        /// </summary>
        private static void DownloadMissingContent(int step) {
            DownloadContent(step, CalculateMissingContent(), true);
        }

        /// <summary>
        /// Invokes <c>CalculateOutdatedContent()</c> and then downloads the received list of outdated files.
        /// </summary>
        private static void DownloadOutdatedContent(int step) {
            DownloadContent(step, CalculateOutdatedContent(), false);
        }

        /// <summary>
        /// Calculates the current progress percentage, based on the received step.
        /// </summary>
        private static int GetCurrentStepProgress(int step) {
            return Convert.ToInt32(step / (float)Pipeline.Length * 100);
        }

        /// <summary>
        /// Informs the UI Thread that a specific type of download (missing content or outdated content) is taking progress, if that's the case.
        /// </summary>
        private static void LogDownloadingEvent(int nResources, bool isMissingContent) {
            if (nResources == 0)
                return;
            string message = isMissingContent ? PatcherEngineResources.DOWNLOADING_MISSING_CONTENT :
                PatcherEngineResources.DOWNLOADING_OUTDATED_CONTENT;            
        }

        /// <summary>
        /// Checks if there are still any missing or outdated content after successfully applying the patch,
        /// throwing a new <c>FileNotFoundException</c> or <c>DataTamperedException</c> respectively if that's the case.
        /// </summary>
        private static void PerformLastSanityChecks() {
            // Give time to AntiVirus for it to delete or tamper any of the recently downloaded files.
            Thread.Sleep(EngineConfigs.MS_TO_WAIT_FOR_AV_FALSE_POSITIVES);
            if (CalculateOutdatedContent().Count != 0)
                Handler.Handle(new DataTamperedException());
            if (CalculateMissingContent().Count != 0)
                Handler.Handle(new FileNotFoundException());
        }

        /// <summary>
        /// Invokes <c>PerformLastSanityChecks()</c>, performing some last second sanity check, and stops the Stopwatch timer informing the UI Thread that the patch is completed.
        /// </summary>
        private static void Finish(Stopwatch sw) {
            PerformLastSanityChecks();
            sw.Stop();            
        }
    }
}