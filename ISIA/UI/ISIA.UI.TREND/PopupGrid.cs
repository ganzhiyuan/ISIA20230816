using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TAP.Models.UIBasic;

using System.Reflection;
using System.IO;

namespace ISIA.UI.TREND
{

    public partial class PopupGrid : DevExpress.XtraEditors.XtraForm
    {


        public DataTable _DataTable { get; set; }
        string _SeleSeriesName;
        string _ValueName;
        public bool linkage  { get; set; }

        public PopupGrid()
        {
            InitializeComponent();
            gridViewStyle();
        }

        public PopupGrid(DataTable dt, string SeleSeriesName, string valueName)
        {
            InitializeComponent();
            gridViewStyle();
            gridControl1.DataSource = dt;
            _DataTable = dt;
            _SeleSeriesName = SeleSeriesName;
            _ValueName = valueName;
            this.gridView1.BestFitColumns();
        }


        public PopupGrid(DataTable dt)
        {
            InitializeComponent();
            gridViewStyle();
            gridControl1.DataSource = dt;
            _DataTable = dt;
           
            this.gridView1.BestFitColumns();
        }

        private void gridViewStyle()
        {
            //this.gridView1.OptionsSelection.MultiSelect = true;

            //this.gridView1.OptionsSelection.MultiSelectMode = DevExpress.XtraGrid.Views.Grid.GridMultiSelectMode.CheckBoxRowSelect;
            //?????
            //this.gridView1.OptionsSelection.CheckBoxSelectorColumnWidth = 1;
            //this.gridView1.OptionsSelection.ShowCheckBoxSelectorInColumnHeader = DevExpress.Utils.DefaultBoolean.True;
            //gridView1.OptionsSelection.CheckBoxSelectorColumnWidth = 30;
            //gridView1.OptionsSelection.MultiSelect = true;
            //gridView1.OptionsSelection.MultiSelectMode = DevExpress.XtraGrid.Views.Grid.GridMultiSelectMode.CheckBoxRowSelect;
            //gridView1.OptionsSelection.ShowCheckBoxSelectorInColumnHeader = DevExpress.Utils.DefaultBoolean.True;
            ////?????????
            this.gridView1.OptionsBehavior.Editable = false;
            //?????
            this.gridView1.OptionsView.ColumnAutoWidth = false;
            //?????????
            this.gridView1.BestFitColumns();
            //????????
            this.gridView1.OptionsCustomization.AllowColumnMoving = false;
            //????????
            this.gridView1.OptionsCustomization.AllowSort = true;
            //?????????
            this.gridView1.OptionsCustomization.AllowColumnResizing = true;
            //??????????
            //this.gridView1.Columns[0].OptionsColumn.AllowEdit = false;
            //this.gridView1.Columns[1].OptionsColumn.AllowEdit = false;

        }

        private void boxPlotToolStripMenuItem_Click(object sender, EventArgs e)
        {
            /*PopupBoxplot pg = new PopupBoxplot((DataTable)gridControl1.DataSource, _SeleSeriesName, _ValueName);
            pg.Location = this.Location;
            pg.ShowDialog(this);
            pg.Dispose();*/
        }

        private void defectMapAnalysisToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //????? ?? ???..
            //defectmapanalysis ??..

            //OpenUI("");

            
            this.Close();
            
        }

        private void OpenUI(string menu)
        {
            MainMenuBasicModel tmpMainMenu = null;
            UIBasicModel tmpUI = new UIBasicModel();

            try
            {
                string mdiName = "ISFA";
                string mainMenu = "ANALYSIS";


                Assembly a = null;
                Type tmpType = null;
                object tmpObject = null;
                Form tmpNewForm = null;

                a = Assembly.LoadFile(Path.Combine("ISFA.UI.ANALYSIS.dll"));
                
                tmpType = a.GetType("ISFA.UI.ANALYSIS.FrmDefectMapAnalysis");
                //??
                tmpObject = Activator.CreateInstance(tmpType);

                //??
                tmpNewForm = (Form)tmpObject;
                tmpNewForm.MdiParent = this.Owner;
                tmpNewForm.FormBorderStyle = FormBorderStyle.None;
                tmpNewForm.Dock = DockStyle.Fill;

                tmpNewForm.Name = this.MakeUITitle("ANALYSIS", "Defect Map Analysis");
                ((TAP.UI.UIBase)tmpNewForm).UIInformation = tmpUI;
                //((TAP.UI.UIBase)tmpNewForm).TabControl = this.tabMDIList;
                //((TAP.UI.UIBase)tmpNewForm).UITitle = this.MakeUITitle(InfoBase._MDI_INFO[this._mdiName].MainMenus[mainMenu].DisplayName, tmpUI.DisplayName);
                ((TAP.UI.UIBase)tmpNewForm).UITitle = this.MakeUITitle("ANALYSIS", "Defect Map Analysis");

                //TAP.Remoting.Caller.CallerInfo.UserID = InfoBase._USER_INFO.UserName;
                //TAP.Remoting.Caller.CallerInfo.ClientPort = TAP.Base.Configuration.ConfigurationManager.Instance.RemoteAdapterSection.LocalPort;
                //TAP.Remoting.Caller.CallerInfo.MDIName = this._mdiName;
                //TAP.Remoting.Caller.CallerInfo.FunctionName = tmpUI.AssemblyName;

                tmpNewForm.Show();

                //this.xtraTabbedMdiManager1.Pages[this.xtraTabbedMdiManager1.Pages.Count - 1].Text = this._conveter.ConvertPhrase(tmpUI.DisplayName);
                //this.xtraTabbedMdiManager1.Pages[this.xtraTabbedMdiManager1.Pages.Count - 1].ShowCloseButton = DevExpress.Utils.DefaultBoolean.False;


                //AgumentPack Data ??? ??
                /*ArgumentPack tmpPack = new ArgumentPack();
                tmpPack.AddArgument("_dataTable", typeof(DataTable), _DataTable);
                ((TAP.UI.UIBase)tmpNewForm).ExecuteCommand(tmpPack);*/

                return;
            }
            catch (System.Exception ex)
            {
            }
        }

        private string MakeUITitle(string mainMenu, string menu)
        {
            #region Code

            string retVal = string.Empty;

            try
            {
                //retVal += "[" + TAP.Base.Configuration.ConfigurationManager.Instance.EnvironmentSection.Region + " / " + TAP.Base.Configuration.ConfigurationManager.Instance.AppSection.Facility + "]";
                //retVal += " ";
                //retVal += "<" + TAP.Base.Configuration.ConfigurationManager.Instance.AppSection.ProductName + "> ? ";
                retVal += "<" + mainMenu + "> ? ";
                retVal += "<" + menu + ">";

                retVal += "<" + ">";

                //retVal += "[" + TAP.Base.Configuration.ConfigurationManager.Instance.EnvironmentSection.Region + " / " + TAP.Base.Configuration.ConfigurationManager.Instance.AppSection.Facility + "]";
                //retVal += " ";
                //retVal += "<" + TAP.Base.Configuration.ConfigurationManager.Instance.AppSection.Apps[this._appName].SubApps[this._mdiName].DisplayName + "> ? ";
                //retVal += "<" + this._conveter.ConvertPhrase(mainMenu) + "> ? ";
                //retVal += "<" + this._conveter.ConvertPhrase(menu) + ">";

                return retVal;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

            #endregion
        }

        private string MakeUITitle(UIBasicModel uiModel)
        {
            #region Code

            string retVal = string.Empty;

            try
            {
                //retVal += "[" + TAP.Base.Configuration.ConfigurationManager.Instance.EnvironmentSection.Region + " / " + TAP.Base.Configuration.ConfigurationManager.Instance.AppSection.Facility + "]";
                //retVal += " ";
                //retVal += "<" + TAP.Base.Configuration.ConfigurationManager.Instance.AppSection.ProductName + "> ? ";
                retVal += "<" + uiModel.MainMenu + "> ? ";
                retVal += "<" + uiModel.Name + "> _ ";
                retVal += "<" + ">";

                //retVal += "[" + TAP.Base.Configuration.ConfigurationManager.Instance.EnvironmentSection.Region + " / " + TAP.Base.Configuration.ConfigurationManager.Instance.AppSection.Facility + "]";
                //retVal += " ";
                //retVal += "<" + TAP.Base.Configuration.ConfigurationManager.Instance.AppSection.Apps[this._appName].SubApps[this._mdiName].DisplayName + "> ? ";
                //retVal += "<" + this._conveter.ConvertPhrase(mainMenu) + "> ? ";
                //retVal += "<" + this._conveter.ConvertPhrase(menu) + ">";

                return retVal;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

            #endregion
        }

    }
}
