using DevExpress.XtraBars.Docking;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace ISIA.UI.BASE.DockPanelTemp
{
    class DockPanleOption
    {
        public void DockPanelDefDock(DockPanel dockPanel)
        {
            ExeConfigurationFileMap map = new ExeConfigurationFileMap();
            try
            {
                string path = System.Environment.CurrentDirectory;               
                //config文件名字
                map.ExeConfigFilename = path + @"\ISFA.exe.config";
                Configuration config = ConfigurationManager.OpenMappedExeConfiguration(map, ConfigurationUserLevel.None);
                string defuctdock = "";
                string TabbedDocument = "";
                string fill = "";
                string floatOnDblClick = "";
                string HideButton = "";
                string CloseButton = "";
                string MaximizeButton = "";
                string Fill = "";
                string Top = "";
                string Bottom = "";
                string Float = "";

                if (config.AppSettings.Settings["DefaultDock"] != null)
                {
                     defuctdock = config.AppSettings.Settings["DefaultDock"].Value.ToString();
                }
                if (config.AppSettings.Settings["AllowDockAsTabbedDocument"] != null)
                {
                     TabbedDocument = config.AppSettings.Settings["AllowDockAsTabbedDocument"].Value.ToString();
                }
                if (config.AppSettings.Settings["AllowDockFill"] != null)
                {
                     fill = config.AppSettings.Settings["AllowDockFill"].Value.ToString();
                }
                if (config.AppSettings.Settings["FloatOnDblClick"] != null)
                {
                     floatOnDblClick = config.AppSettings.Settings["FloatOnDblClick"].Value.ToString();
                }
                if (config.AppSettings.Settings["ShowAutoHideButton"] != null)
                {
                     HideButton = config.AppSettings.Settings["ShowAutoHideButton"].Value.ToString();
                }
                if (config.AppSettings.Settings["ShowCloseButton"] !=null)
                {
                     CloseButton = config.AppSettings.Settings["ShowCloseButton"].Value.ToString();
                }
                if (config.AppSettings.Settings["ShowMaximizeButton"] !=null)
                {
                     MaximizeButton = config.AppSettings.Settings["ShowMaximizeButton"].Value.ToString();
                }
                if (config.AppSettings.Settings["AllowDockFill"] !=null)
                {
                     Fill = config.AppSettings.Settings["AllowDockFill"].Value.ToString();
                }
                if (config.AppSettings.Settings["AllowDockTop"] !=null)
                {
                     Top = config.AppSettings.Settings["AllowDockTop"].Value.ToString();
                }
                if (config.AppSettings.Settings["AllowDockBottom"] != null)
                {
                     Bottom = config.AppSettings.Settings["AllowDockBottom"].Value.ToString();
                }
                if (config.AppSettings.Settings["AllowFloating"] != null)
                {
                     Float = config.AppSettings.Settings["AllowFloating"].Value.ToString();
                }              
                if (defuctdock == "")
                {
                       dockPanel.Dock = DevExpress.XtraBars.Docking.DockingStyle.Left;
                }
                else
                {
                    if (defuctdock == "left")
                    {
                       dockPanel.Dock = DevExpress.XtraBars.Docking.DockingStyle.Left;
                    }
                    else if (defuctdock == "right")
                    {
                       dockPanel.Dock = DevExpress.XtraBars.Docking.DockingStyle.Right;
                    }
                }
                if (TabbedDocument == "false")
                    dockPanel.Options.AllowDockAsTabbedDocument = false;
                if (floatOnDblClick == "false")
                    dockPanel.Options.FloatOnDblClick = false;
                if (HideButton == "false")
                    dockPanel.Options.ShowAutoHideButton = false;
                if (CloseButton == "false")
                    dockPanel.Options.ShowCloseButton = false;
                if (MaximizeButton == "false")
                    dockPanel.Options.ShowMaximizeButton = false;
                if (Fill == "false")
                    dockPanel.Options.AllowDockFill = false;
                if (Top == "false")
                    dockPanel.Options.AllowDockTop = false;
                if (Bottom == "false")
                    dockPanel.Options.AllowDockBottom = false;
                if (Float == "false")
                    dockPanel.Options.AllowFloating = false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
