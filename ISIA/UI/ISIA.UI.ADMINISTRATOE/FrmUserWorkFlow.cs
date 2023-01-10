using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using TAP.UI;
using System.Collections;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraEditors;
using TAP.Data.Client;
using TAP;
using TAP.UIControls;
using TAP.UIControls.BasicControlsDEV;
using TAP.Models.Codes;
using ISIA.INTERFACE.ARGUMENTSPACK;
using ISIA.UI.BASE;

namespace ISIA.UI.ADMINISTRATOE
{
    public partial class FrmUserWorkFlow : DockUIBase1T1
    {
        public FrmUserWorkFlow()
        {
            InitializeComponent();
            bs = new BizDataClient("ISIA.BIZ.ADMINISTRATOE.dll", "ISIA.BIZ.ADMINISTRATOE.UserWorkFlow");                      
            TListBoxAdd();
        }


        #region Field
        List<int> focusedRowHandles = new List<int>();
        ComboBoxControl ComboBoxControl = new ComboBoxControl();
        BizDataClient bs = null;
        List<IUIControl> OptionControls = new List<IUIControl>();
        List<IUIControl> AddOptionControls = new List<IUIControl>();
        DataSet rawdt = new DataSet();
        
        //ResponseArgsPack args = new ResponseArgsPack();
        List<string> UserList = new List<string>();
        public int selectIndex = 0;
        
        #endregion

        #region Event

        public void SetUserModify(List<string> listTemp)
        {
            UserList = listTemp;
        }

        #endregion

        #region Initialize

        protected override void Initialize()
        {
            #region Initialize

            try
            {
                base.Initialize();
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

            #endregion
        }

        #endregion

        #region Method

        
        private void TListBoxAdd()
        {
            AddOptionControls.Clear();
            AddOptionControls.Add(tblWorkABM);
            AddOptionControls.Add(tblWorkADaily);
        }


        public DataSet LoadEquipmentList()
        {
            WorkFlowArgsPack args = new WorkFlowArgsPack();

            try
            {
                args.Facility = cboFab.Text;
                args.Line = cboLine.Text;
                args.Area = cboArea.Text;
                //args.Bay = cboBay.Text;
                args.EquipmentType = cboEqpType.Text;
                //args.MainEquipment = cboMainEQP.Text;
                args.EqpGroup = cboGroup.Text;
                args.Model = cboModel.Text;
                //args.

                if(rboEquipment.Checked)                
                    rawdt.Tables.Add(bs.ExecuteDataTable("GetEQ", args.getPack()));
                else
                    rawdt.Tables.Add(bs.ExecuteDataTable("GetGroupEQPList", args.getPack()));

            }
            catch(System.Exception ex)
            {
                throw ex;
            }
            
            return rawdt;
        }

        public string UpdateUserWorkFlow()
        {            
            try
            {
                WorkFlowArgsPack args = new WorkFlowArgsPack();
                ArgumentPack argumentPack = new ArgumentPack();
                string count = null;
                
                if (rboEquipment.Checked)
                {
                    if (tlbEQP.SelectedItem == null)
                    {
                        TAP.UI.TAPMsgBox.Instance.ShowMessage(this.Text, EnumMsgType.ERROR, "Not Selected Equipment.");
                        return null;
                    }

                    DataTable dtEqp = new DataTable();
                    dtEqp = ((DataTable)tlbEQP.DataSource).Copy();
                    dtEqp.DefaultView.RowFilter = string.Format("NAME = '{0}'", tlbEQP.Text);

                    DataTable tmpdt = dtEqp.DefaultView.ToTable();

                    if (tmpdt == null)
                    {
                        TAP.UI.TAPMsgBox.Instance.ShowMessage(this.Text, EnumMsgType.ERROR, "Not Found Infomation.");
                        return null;
                    }

                    args.Region = TAP.Base.Configuration.ConfigurationManager.Instance.EnvironmentSection.Region;
                    args.Facility = tmpdt.Rows[0]["FACILITY"].ToString();
                    args.Line = tmpdt.Rows[0]["LINE"].ToString();
                    args.Area = tmpdt.Rows[0]["AREA"].ToString();
                    //args.Bay = tmpdt.Rows[0]["BAY"].ToString();


                    //args.MainEquipment = tmpdt.Rows[0]["MAINEQUIPMENT"].ToString();
                    args.Equipment = tlbEQP.Text;


                    int deleteCount = bs.ExecuteModify("DeleteUserWorkFlow", args.getPack());

                    for (int i = 0; i < AddOptionControls.Count; i++)
                    {
                        //args.ShiftName = AddOptionControls[i].ControlID.Substring(7, 1); //Shift Name A,B,C

                        if (AddOptionControls[i].ControlID.Substring(8, 1) == "D") args.PmSchedule = "PM";
                        else if (AddOptionControls[i].ControlID.Substring(8, 1) == "W") args.PmSchedule = "WEEKLY";
                        else if (AddOptionControls[i].ControlID.Substring(8, 1) == "2") args.PmSchedule = "2WEEKLY";
                        else if (AddOptionControls[i].ControlID.Substring(8, 1) == "M") args.PmSchedule = "MONTHLY";
                        else if (AddOptionControls[i].ControlID.Substring(8, 1) == "Q") args.PmSchedule = "QUARTERLY";
                        else if (AddOptionControls[i].ControlID.Substring(8, 1) == "Y") args.PmSchedule = "YEARLY";
                        else if (AddOptionControls[i].ControlID.Substring(8, 1) == "B") args.PmSchedule = "BM";

                        for (int j = 0; j < ((TListBox)AddOptionControls[i]).Items.Count; j++)
                        {
                            string[] strTemp = ((TListBox)AddOptionControls[i]).Items[j].ToString().Split('_');
                            //if (strTemp[0] == "A") strTemp[0] = "ADMIN";
                            //else if (strTemp[0] == "E") strTemp[0] = "ENGINEER";
                            //else if (strTemp[0] == "O") strTemp[0] = "OPERATOR";

                            //args.WorkerRole = strTemp[0];
                            args.Worker = strTemp[0];

                            args.WorkerRole = "ENGINEER";
                            //args.Worker = ((TListBox)AddOptionControls[i]).Items[j].ToString();

                            int saveCount = bs.ExecuteModify("InsertUserWorkFlow", args.getPack());
                            count += saveCount.ToString();
                        }
                    }
                }
                else
                {

                    DataTable dtEqp = new DataTable();
                    dtEqp = ((DataTable)tlbEQP.DataSource).Copy();

                    if (dtEqp.Rows.Count < 1)
                    {
                        TAP.UI.TAPMsgBox.Instance.ShowMessage(this.Text, EnumMsgType.ERROR, "Don't have any equipment..");
                        return null;
                    }

                    foreach (DataRow dr in dtEqp.Rows)
                    {
                        args.Region = TAP.Base.Configuration.ConfigurationManager.Instance.EnvironmentSection.Region;
                        args.Facility = dr["FACILITY"].ToString();
                        args.Line = dr["LINE"].ToString();
                        args.Area = dr["AREA"].ToString();
                        //args.Bay = dr["BAY"].ToString();

                        args.MainEquipment = dr["MAINEQUIPMENT"].ToString();
                        args.Equipment = dr["NAME"].ToString();

                        int deleteCount = bs.ExecuteModify("DeleteUserWorkFlow", args.getPack());

                        for (int i = 0; i < AddOptionControls.Count; i++)
                        {
                            //args.ShiftName = AddOptionControls[i].ControlID.Substring(7, 1); //Shift Name A,B,C

                            if (AddOptionControls[i].ControlID.Substring(8, 1) == "D") args.PmSchedule = "PM";
                            else if (AddOptionControls[i].ControlID.Substring(8, 1) == "W") args.PmSchedule = "WEEKLY";
                            else if (AddOptionControls[i].ControlID.Substring(8, 1) == "2") args.PmSchedule = "2WEEKLY";
                            else if (AddOptionControls[i].ControlID.Substring(8, 1) == "M") args.PmSchedule = "MONTHLY";
                            else if (AddOptionControls[i].ControlID.Substring(8, 1) == "Q") args.PmSchedule = "QUARTERLY";
                            else if (AddOptionControls[i].ControlID.Substring(8, 1) == "Y") args.PmSchedule = "YEARLY";
                            else if (AddOptionControls[i].ControlID.Substring(8, 1) == "B") args.PmSchedule = "BM";

                            for (int j = 0; j < ((TListBox)AddOptionControls[i]).Items.Count; j++)
                            {
                                string[] strTemp = ((TListBox)AddOptionControls[i]).Items[j].ToString().Split('_');
                                //if (strTemp[0] == "A") strTemp[0] = "ADMIN";
                                //else if (strTemp[0] == "E") strTemp[0] = "ENGINEER";
                                //else if (strTemp[0] == "O") strTemp[0] = "OPERATOR";

                                //args.WorkerRole = strTemp[0];
                                args.Worker = strTemp[0];

                                args.WorkerRole = "ENGINEER";

                                int saveCount = bs.ExecuteModify("InsertUserWorkFlow", args.getPack());
                                count += saveCount;
                            }
                        }
                    }

                }
                return count.ToString();
            }
            catch (System.Exception ex)
            {
                TAP.UI.TAPMsgBox.Instance.ShowMessage("Update", EnumMsgType.ERROR, "Update Error.");
                return ex.ToString();
            }
        }
        public void UpdateComplete(string str)
        {
            try
            {
                TAP.UI.TAPMsgBox.Instance.ShowMessage(this.Text, EnumMsgType.INFORMATION, string.Format("{0} update Complete.",str.Length));
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        public void DisplayData(DataSet dataSet)
        {
            try
            {
                tlbEQP.DisplayMember = "NAME";
                tlbEQP.ValueMember = "NAME";
                tlbEQP.DataSource = dataSet.Tables[0];

                if (rboGroup.Checked)
                {
                    GetWorkFlow();
                }

            }
            catch(System.Exception ex)
            {
                throw ex;
            }
        }

        // Add
        private void btnSave_Click(object sender, EventArgs e)
        {
            ComboBoxControl.SetCrossLang(this._translator);           
        }

        //Select ??
        private void btnSelect_Click(object sender, EventArgs e)
        {
            //ArgumentPack argumentPack = new ArgumentPack();

            //argumentPack.AddArgument("FACILITY", typeof(string), cboFab.Text);
            //argumentPack.AddArgument("LINE", typeof(string), cboLine.Text);
            //argumentPack.AddArgument("AREA", typeof(string), cboArea.Text);
            //argumentPack.AddArgument("MAINEQUIPMENT", typeof(string), cboMainEQP.Text);
            //Model 사용시
            //argumentPack.AddArgument("MODEL", typeof(string), cboModel.Text);
            rawdt.Clear();
            rawdt.Tables.Clear();
            base.BeginAsyncCall("LoadEquipmentList", "DisplayData", EnumDataObject.DATASET);

        }       

        //Update ??
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult dialog = TAP.UI.TAPMsgBox.Instance.ShowMessage(this.Text, EnumMsgType.CONFIRM, "Do you really update data?");
              
                if (dialog.ToString() == "Yes")
                {
                    base.BeginAsyncCall("UpdateUserWorkFlow", "UpdateComplete", EnumDataObject.STRING);
                }
            }
            catch(System.Exception ex)
            {
                throw ex;
            }
        }
        
        private void btnCancel_Click(object sender, EventArgs e)
        {
            base.CancelAsync();
        }

        #endregion

        private void tlbEQP_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (rboEquipment.Checked)
                {
                    GetWorkFlow();
                }
            }
            catch(System.Exception ex)
            {
                throw ex;
            }
            
        }

        private void GetWorkFlow()
        {
            WorkFlowArgsPack args = new WorkFlowArgsPack();
            try
            {
                DataTable dtEqp = new DataTable();
                dtEqp = ((DataTable)tlbEQP.DataSource).Copy();
                dtEqp.DefaultView.RowFilter = string.Format("NAME = '{0}'", tlbEQP.Text);

                DataTable tmpdt = dtEqp.DefaultView.ToTable();

                if (tmpdt.Rows.Count < 1)
                {
                    //TAP.UI.TAPMsgBox.Instance.ShowMessage(this.Text, EnumMsgType.ERROR, "Not Found Infomation.");
                    return;
                }

                args.Region = TAP.Base.Configuration.ConfigurationManager.Instance.EnvironmentSection.Region;
                args.Facility = tmpdt.Rows[0]["FACILITY"].ToString();
                args.Line = tmpdt.Rows[0]["LINE"].ToString();
                args.Area = tmpdt.Rows[0]["AREA"].ToString();
                
                //args.Bay = tmpdt.Rows[0]["BAY"].ToString();

                args.MainEquipment = tmpdt.Rows[0]["MAINEQUIPMENT"].ToString();
                args.Equipment = tlbEQP.Text;


                DataTable workdt = bs.ExecuteDataTable("GetUserWorkFlow", args.getPack());

                clearTListBox();

                foreach (DataRow dr in workdt.Rows)
                {
                    AddListBoxItem(dr);
                }
            }
            catch(System.Exception ex)
            {
                throw ex;
            }
        }


        private void AddListBoxItem(DataRow row)
        {
            try
            {                
                if (row["PMSCHEDULE"].ToString().Equals("PM"))
                {
                    tblWorkADaily.Items.Add(string.Format("{0}_{1}", row["WORKER"].ToString(), row["USERNAME"].ToString()));
                    //tblWorkADaily.Items.Add(row["WORKER"].ToString());
                }                
                else if(row["PMSCHEDULE"].ToString().Equals("BM"))
                {
                    tblWorkABM.Items.Add(string.Format("{0}_{1}", row["WORKER"].ToString(), row["USERNAME"].ToString()));
                    //tblWorkABM.Items.Add(row["WORKER"].ToString());
                }             
            }
            catch(System.Exception ex)
            {
                throw ex;
            }
        }

        private void clearTListBox()
        {
            tblWorkABM.Items.Clear();
            tblWorkADaily.Items.Clear();
        }

        private void tblbox_DoubleClick(object sender, EventArgs e)
        {
            
            List<string> tmplist = new List<string>();
            UserList.Clear();

            for (int i =0; i < ((TListBox)sender).Items.Count; i++)
            {
                tmplist.Add(((TListBox)sender).Items[i].ToString());
            }

            UserWorkFlowModify frmUser = new UserWorkFlowModify(tmplist);
            frmUser.SettblUser += SetUserModify;
            if(DialogResult.OK == frmUser.ShowDialog())
            { 
                if (UserList.Count > 0)
                {
                    ((TListBox)sender).Items.Clear();

                    for(int i = 0; i < UserList.Count; i++)
                    {
                        ((TListBox)sender).Items.Add(UserList[i].ToString());
                    }

                    ((TListBox)sender).Refresh();
                }
            else
                ((TListBox)sender).Items.Clear();
            }
        }

        private void rboGroup_CheckedChanged(object sender, EventArgs e)
        {
            if(rboGroup.Checked)
            {
                OptionControls.Clear();

                cboFab.Text = "";
                cboLine.Text = "";
                cboArea.Text = "";
                cboBay.Text = "";
                cboMainEQP.Text = "";
                cboGroup.Text = "";

                lcilblLine.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                lcicboLine.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                lcilblArea.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                lcicboArea.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                lcilblBay.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                lcicboBay.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                lcilblMainEqp.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                lcicboMainEqp.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                lcilblFab.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                lcicboFab.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                lcilblEqpType.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                lcicboEqpType.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;


                //lciCModel.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                //lciModel.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                lcilblGroup.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                lcicboGroup.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;

                //OptionControls.Add(cboFab);
                //OptionControls.Add(cboModel);
                //OptionControls.Add(cboGroup);
            }
        }

        private void rboEquipment_CheckedChanged(object sender, EventArgs e)
        {
            if (rboEquipment.Checked)
            {
                OptionControls.Clear();

                cboFab.Text = "";
                cboLine.Text = "";
                cboArea.Text = "";
                cboBay.Text = "";
                cboMainEQP.Text = "";
                cboGroup.Text = "";

                lcilblLine.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                lcicboLine.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                lcilblArea.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                lcicboArea.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                //lcilblBay.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                //lcicboBay.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                //lcilblMainEqp.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                //lcicboMainEqp.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                lcilblFab.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                lcicboFab.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                lcilblEqpType.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                lcicboEqpType.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;

                lcilblGroup.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                lcicboGroup.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;

                //OptionControls.Add(cboFab);
                //OptionControls.Add(cboLine);
                //OptionControls.Add(cboArea);
                //OptionControls.Add(cboBay);

            }
        }
    }
}
