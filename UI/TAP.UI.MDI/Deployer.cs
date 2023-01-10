using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAP.Base.Configuration;
using TAP.FTP;

namespace TAP.UI.MDI
{
    class Deployer
    {
        //public static void Main(string[] args)
        //{
        //    //获得进程中Isem.exe的路径
        //    path = GetPath();
        //    if (!string.IsNullOrEmpty(path))
        //    {
        //        KillProcess("ISEM");

        //        Initialize();

        //        OpenISEM(path);

        //    }
        //    //Console.ReadKey();
        //}
        public static void Start() {
            path = GetPath();
            if (!string.IsNullOrEmpty(path))
            {
                KillProcess("ISEM");

                Initialize();

                OpenISEM(path);

            }

        }


        #region Fiedls
        private  static string _ftpHost;
        private  static string _ftpRoot;
        private  static string _userName;
        private  static string _password;
        private  static string _localDir;
        private  static string _appName;
        private  static string _appDirectory;
        private  static FTPclient _ftpClient;
        private  static string path ;
        #endregion

        #region
        public static void KillProcess(string processName)
        {
            // System.Diagnostics.Process myproc = new System.Diagnostics.Process();
            //得到所有打开的进程

            foreach (Process thisproc in Process.GetProcessesByName(processName))
            {
                try
                {
                    thisproc.Kill();
                    Console.WriteLine($"已杀掉{processName}进程！！！");
                }
                catch (Win32Exception e)
                {
                    Console.WriteLine(e.Message.ToString());
                }
                catch (InvalidOperationException e)
                {
                    Console.WriteLine(e.Message.ToString());
                }


            }
        }

        public static void OpenISEM(string EXEName)
        {

            try
            {
                System.Diagnostics.Process.Start(EXEName);//打开cmd.exe可执行文件
            }
            catch (System.Exception e)
            {
                Console.WriteLine(e.Message.ToString());
            }
                        
        }

        public static void Initialize()
        {
            #region Code
            try
            {
                //_localDir = Directory.GetParent(FormRibbon_ISEM._ExecutableDirectory.Trim('\\')).FullName;
                //E:\Project\ISEM\ISEM\_bin\FX\ISEM.exe
                //_localDir = "E:\\Project\\ISEM\\ISEM\\_bin";

                _localDir = path.Substring(0, path.Length - 9);
                _ftpHost = ConfigurationManager.Instance.RemoteDeploySection.ClientDeployInfo.HostName;
                _ftpRoot = ConfigurationManager.Instance.RemoteDeploySection.ClientDeployInfo.RootDirectory;
                _userName = ConfigurationManager.Instance.RemoteDeploySection.ClientDeployInfo.UserName;
                _password = ConfigurationManager.Instance.RemoteDeploySection.ClientDeployInfo.Password;
                _appName = ConfigurationManager.Instance.AppSection.MDIName;
                _appDirectory = TAP.Base.Configuration.ConfigurationManager.Instance.AppSection.AppDirectory;


                _ftpClient = new FTPclient(_ftpHost, _userName, _password);

                DownloadFiles(Path.Combine(_appDirectory));

                return;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

            #endregion
        }

        private static void DownloadFiles(string directory)
        {
            #region Code

            FTPdirectory ftpDir = null;
            string fileLine = string.Empty;

            try
            {
                if (!Directory.Exists(Path.Combine(_localDir, directory)))
                    Directory.CreateDirectory(Path.Combine(_localDir, directory));

                ftpDir = _ftpClient.ListDirectoryDetail(_ftpRoot + "/" + directory.Replace("\\", "/") + "/");



                for (int i = 0; i < ftpDir.Count; i++)
                {
                    try
                    {
                        if (ftpDir[i].FileType == FTPfileInfo.DirectoryEntryTypes.Directory)
                        {
                            DownloadFiles(directory + "\\" + ftpDir[i].Filename);
                            continue;
                        }
                        if (ftpDir[i].Filename.Contains("TEST.txt"))
                        {
                            if (System.IO.File.Exists(Path.Combine(_localDir, directory, ftpDir[i].Filename)))
                            {
                                System.IO.FileInfo tmpFile = new FileInfo(Path.Combine(_localDir, directory, ftpDir[i].Filename));

                                if (tmpFile.LastWriteTime >= ftpDir[i].FileDateTime)
                                    continue;
                            }

                            _ftpClient.Download(ftpDir[i], Path.Combine(_localDir, directory, ftpDir[i].Filename), true);
                        }
                       
                    }
                    catch (System.Exception ex)
                    {
                        if (TAP.Base.Configuration.ConfigurationManager.Instance.RemoteDeploySection.ClientDeployInfo.IgnoreExcetion == false)
                            throw ex;
                        else
                            continue;
                    }
                }

                return;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            finally
            {
                //
            }

            #endregion
        }

        private static void DownloadISEMEXE(string directory)
        {
            #region Code

            FTPdirectory ftpDir = null;
            string fileLine = string.Empty;

            try
            {
                if (!Directory.Exists(Path.Combine(_localDir, directory)))
                    Directory.CreateDirectory(Path.Combine(_localDir, directory));

                ftpDir = _ftpClient.ListDirectoryDetail(_ftpRoot + "/" + directory.Replace("\\", "/") + "/");



                for (int i = 0; i < ftpDir.Count; i++)
                {
                    try
                    {
                        if (ftpDir[i].FileType == FTPfileInfo.DirectoryEntryTypes.Directory)
                        {
                            DownloadFiles(directory + "\\" + ftpDir[i].Filename);
                            continue;
                        }

                        if (System.IO.File.Exists(Path.Combine(_localDir, directory, ftpDir[i].Filename)))
                        {
                            System.IO.FileInfo tmpFile = new FileInfo(Path.Combine(_localDir, directory, ftpDir[i].Filename));

                            if (tmpFile.LastWriteTime >= ftpDir[i].FileDateTime)
                                continue;
                        }

                        _ftpClient.Download(ftpDir[i], Path.Combine(_localDir, directory, ftpDir[i].Filename), true);
                    }
                    catch (System.Exception ex)
                    {
                        if (TAP.Base.Configuration.ConfigurationManager.Instance.RemoteDeploySection.ClientDeployInfo.IgnoreExcetion == false)
                            throw ex;
                        else
                            continue;
                    }
                }

                return;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            finally
            {
                //
            }

            #endregion
        }

        public static string GetPath() {

            Process[] ps = Process.GetProcessesByName("ISEM");
            foreach (Process p in ps)
            {
                path = p.MainModule.FileName.ToString();

            }

            return path;

        }

        #endregion



    }
}
