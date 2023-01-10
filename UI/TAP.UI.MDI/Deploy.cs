using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.IO;
using System.Text;

using TAP.Base;
using TAP.Base.Configuration;
using TAP.FTP;

namespace TAP.UI.MDI
{
    /// <summary>
    /// This class downloads last files from deploy server
    /// </summary>
    public class Deploy
    {
        #region Fiedls

        private string _ftpHost;
        private string _ftpRoot;
        private string _userName;
        private string _password;

        private string _localDir;
        private string _appName;
        private string _appDirectory;

        private FTPclient _ftpClient;

        #endregion

        public void DeployFiles()
        {
            #region Code

            try
            {
                this.Initialize();
                //this.DownloadFXFiles();
                //this.DownloadApplicationFiles();

                return;
            }
            catch(System.Exception ex)
            {
                throw ex;
            }

            #endregion
        }

        private void Initialize()
        {
            #region Code

            try
            {

                //_localDir = Directory.GetParent(FormRibbon_ISEM._ExecutableDirectory.Trim('\\')).FullName;
                _localDir = Directory.GetCurrentDirectory();
                _ftpHost = ConfigurationManager.Instance.RemoteDeploySection.ClientDeployInfo.HostName;
                _ftpRoot = ConfigurationManager.Instance.RemoteDeploySection.ClientDeployInfo.RootDirectory;
                _userName = ConfigurationManager.Instance.RemoteDeploySection.ClientDeployInfo.UserName;
                _password = ConfigurationManager.Instance.RemoteDeploySection.ClientDeployInfo.Password;
                _appName = ConfigurationManager.Instance.AppSection.MDIName;
                _appDirectory = TAP.Base.Configuration.ConfigurationManager.Instance.AppSection.AppDirectory;

                _ftpClient = new FTPclient(_ftpHost, _userName, _password);
                bool flag = this.CheckFXVersion(_appDirectory);
                if (flag)
                {
                    //this.DownloadFXFiles();
                    this.DownloadApplicationFiles();
                }
                else
                {
                    this.DownloadApplicationFiles();
                }
                return;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

            #endregion
        }

        private void DownloadFXFiles()
        {
            #region Code

            try
            {
                    System.Windows.Forms.MessageBox.Show("FX was updated. System starts deploy last FX.");

                    if (File.Exists(Path.Combine(_localDir, "FX", "Deployer.exe")))
                    {
                        Process.Start(Path.Combine(_localDir, "FX", "Deployer.exe"));
                        foreach (Process item in Process.GetProcessesByName("ISEM"))
                        {
                            item.Kill();
                        }
                    }
                    else
                    {
                        System.Windows.Forms.MessageBox.Show("Deployer does not exist. System can not contiue to deploy.");
                        return;
                    }
                return;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

            #endregion
        }

        private void DownloadApplicationFiles()
        {
            #region Code

            try
            {
                //Donwload DLL
                this.DownloadFiles(Path.Combine(_appDirectory));

                return;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

            #endregion
        }
        private void DownloadFiles(string directory, string directory2)
        {
            #region Code

            FTPdirectory ftpDir = null;
            string fileLine = string.Empty;

            try
            {
                if (!Directory.Exists(Path.Combine(_localDir, directory2)))
                    Directory.CreateDirectory(Path.Combine(_localDir, directory2));

                ftpDir = _ftpClient.ListDirectoryDetail(_ftpRoot + "/" + directory.Replace("\\", "/") + directory2 + "/");

                for (int i = 0; i < ftpDir.Count; i++)
                {
                    try
                    {
                        if (ftpDir[i].FileType == FTPfileInfo.DirectoryEntryTypes.Directory)
                        {
                            this.DownloadFiles(directory + "\\",  directory2 + "\\" + ftpDir[i].Filename); //하위 디렉토리 일경우 넣는거 재귀로 안됨.
                            continue;
                        }

                        if (System.IO.File.Exists(Path.Combine(_localDir, directory2, ftpDir[i].Filename)))
                        {
                            System.IO.FileInfo tmpFile = new FileInfo(Path.Combine(_localDir, directory2,ftpDir[i].Filename));

                            if (tmpFile.LastWriteTime >= ftpDir[i].FileDateTime)
                                continue;
                        }

                        this._ftpClient.Download(ftpDir[i], Path.Combine(_localDir, directory2, ftpDir[i].Filename), true);
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
        private void DownloadFiles(string directory)
        {
            #region Code

            FTPdirectory ftpDir = null;
            string fileLine = string.Empty;

            try
            {
                if (!Directory.Exists(Path.Combine(_localDir)))
                    Directory.CreateDirectory(Path.Combine(_localDir));

                ftpDir = _ftpClient.ListDirectoryDetail(_ftpRoot + "/" + directory.Replace("\\", "/") + "/");

                for (int i = 0; i < ftpDir.Count; i++)
                {
                    try
                    {
                        if (ftpDir[i].FileType == FTPfileInfo.DirectoryEntryTypes.Directory)
                        {
                            this.DownloadFiles(directory + "\\", ftpDir[i].Filename); //하위 디렉토리 일경우 넣는거 재귀로 안됨.
                            continue;
                        }

                        if (System.IO.File.Exists(Path.Combine(_localDir, ftpDir[i].Filename)))
                        {
                            System.IO.FileInfo tmpFile = new FileInfo(Path.Combine(_localDir, ftpDir[i].Filename));

                            if (tmpFile.LastWriteTime >= ftpDir[i].FileDateTime)
                                continue;
                        }

                        this._ftpClient.Download(ftpDir[i], Path.Combine(_localDir, ftpDir[i].Filename), true);
                    }
                    catch(System.Exception ex)
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

        private bool CheckFXVersion(string directory)
        {
            #region Code

            FTPdirectory ftpDir = null;
            string fileLine = string.Empty;

            try
            {
                if (!Directory.Exists(Path.Combine(_localDir)))
                    Directory.CreateDirectory(Path.Combine(_localDir));

                ftpDir = _ftpClient.ListDirectoryDetail(_ftpRoot + "/" + directory.Replace("\\", "/") + "/");

                for (int i = 0; i < ftpDir.Count; i++)
                {
                    try
                    {
                        if (ftpDir[i].FileType == FTPfileInfo.DirectoryEntryTypes.Directory)
                        {
                            this.CheckFXVersion(directory + "\\" + ftpDir[i].Filename);
                            continue;
                        }

                        if (ftpDir[i].Filename.StartsWith("ISEM.exe"))
                         continue;

                        if (System.IO.File.Exists(Path.Combine(_localDir, ftpDir[i].Filename)))
                        {
                            System.IO.FileInfo tmpFile = new FileInfo(Path.Combine(_localDir, ftpDir[i].Filename));

                            if (tmpFile.LastWriteTime < ftpDir[i].FileDateTime)
                                return true;
                        }
                        else
                        {
                            return true;
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

                return false;
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
    }
}
