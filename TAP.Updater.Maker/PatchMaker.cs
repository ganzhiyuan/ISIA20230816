using DevExpress.Utils.Extensions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using TAP.UPDATER;
using TAP.UPDATER.FileSystem;
using TAP.UPDATER.Hash;
using TAP.UPDATER.Resources.Configs;
using TAP.UPDATER.Resources.TextResources;

namespace TAP.Updater.Maker
{
    public partial class PatchMaker : DevExpress.XtraEditors.XtraForm
    {
        private static ConcurrentDictionary<string, FileMetadata> LocalMetadata;

        private static IHasher Hasher;

        private static string _FileName = "patchlist.txt";

        public PatchMaker()
        {
            InitializeComponent();
            Hasher = new Md5Hasher();
        }

        private void textEdit1_Click(object sender, EventArgs e)
        {
            if (xtraFolderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                string[] filePaths = Directory.GetFiles(xtraFolderBrowserDialog1.SelectedPath, "*.*", SearchOption.AllDirectories);
                lblFolder.Text = xtraFolderBrowserDialog1.SelectedPath+"\\";

                ConcurrentDictionary<string, FileMetadata> metadata = new ConcurrentDictionary<string, FileMetadata>(filePaths.Length, Math.Max(1, Environment.ProcessorCount / 2));
                string replacePath = lblFolder.Text;
                Parallel.ForEach(filePaths, (currentPath) =>
                {
                    using (FileStream stream = File.OpenRead(currentPath))
                    {
                        string temp = currentPath.Replace(replacePath, "");

                        // The bigger the files to hash the bigger the speedup!
                        metadata[temp] = new FileMetadata(temp, Hasher.GeneratedHashFromStream(stream));
                    }
                });


                if(metadata.Count < 1)
                {
                    return;
                }

                LocalMetadata = metadata;

                StringBuilder stringBuilder = new StringBuilder();

                stringBuilder.AppendLine(txtAddress.Text);

                foreach (string file in LocalMetadata.Keys)
                {
                    stringBuilder.AppendLine(file);
                    stringBuilder.AppendLine(LocalMetadata[file].Hash);
                }
                memoEdit1.Text = stringBuilder.ToString();
                lblCount.Text = "File Count: " + LocalMetadata.Count.ToString();
            }


        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            xtraSaveFileDialog1.FileName = txtOutputFile.Text;
            xtraSaveFileDialog1.InitialDirectory = Directory.GetCurrentDirectory();

            if (xtraSaveFileDialog1.ShowDialog() == DialogResult.OK)
            {

                System.IO.File.WriteAllText(xtraSaveFileDialog1.FileName, memoEdit1.Text);

            }
        }
    }
}
