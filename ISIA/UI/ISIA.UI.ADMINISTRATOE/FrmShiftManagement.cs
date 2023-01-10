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
    public partial class FrmShiftManagement : DockUIBase1T1
    {
        public FrmShiftManagement()
        {
            InitializeComponent();
            bs = new BizDataClient("ISIA.BIZ.ADMINISTRATOE.dll", "ISIA.BIZ.ADMINISTRATOE.ShiftManagement");
            SetShift();
            SetWorkGroup();
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
        private string intervalOption = "LDEM"; //L ast D ay of E very M onth.   First Day Of Every Month, Custom Interval Day
        
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
        
        private void SetShift()
        {
            try
            {
                ShiftArgsPack shiftArgs = new ShiftArgsPack();
                
                DataTable shiftDt = bs.ExecuteDataTable("GetShift", shiftArgs.getPack());

                if (shiftDt.Rows.Count < 1)
                {
                    return;
                }
              
                nbxGroup.Text = shiftDt.Rows[0]["GROUPCOUNT"].ToString();
                nbxShift.Text = shiftDt.Rows[0]["SHIFTCOUNT"].ToString();
                //dtpDate.Text = shiftDt.Rows[0]["SHIFTSTARTDATE"].ToString();
                dtpDate.DateTime = DateTime.ParseExact(shiftDt.Rows[0]["SHIFTSTARTDATE"].ToString(), "yyyy-MM-dd", System.Globalization.CultureInfo.CurrentCulture);
                dtpTime.Text = shiftDt.Rows[0]["SHIFTSTARTTIME"].ToString();
                nbxDate.Text = shiftDt.Rows[0]["SHIFTINTERVALDAY"].ToString();

                if(shiftDt.Rows[0]["SHIFTINTERVALOPTION"].ToString().Contains("LDEM"))
                {
                    rbtnLast.Checked = true;
                }
                if (shiftDt.Rows[0]["SHIFTINTERVALOPTION"].ToString().Contains("FDEM"))
                {
                    rbtnFirst.Checked = true;
                }
                if (shiftDt.Rows[0]["SHIFTINTERVALOPTION"].ToString().Contains("CID"))
                {
                    rbtnCustom.Checked = true;
                }
                
            }
            catch (System.Exception ex)
            {
                return;
            }
        }


       

        private void SetWorkGroup()
        {
            try
            {
                
                WorkGroupArgsPack workGroupArgs = new WorkGroupArgsPack();

                DataTable WorkGroupDt = bs.ExecuteDataTable("GetWorkGroup", workGroupArgs.getPack());

                if(WorkGroupDt.Rows.Count < 1)
                {
                    return;
                }

                gridControl1.DataSource = WorkGroupDt;
                

            }
            catch(System.Exception ex)
            {
                return;
            }

        }
        #endregion

        #region Event
        // Add
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                ShiftArgsPack shiftArgs = new ShiftArgsPack();

                //interlock 

                shiftArgs.Region = this.UIInformation.Region;
                shiftArgs.ShiftCount = int.Parse(nbxShift.Text);
                shiftArgs.GroupCount = int.Parse(nbxGroup.Text);
                shiftArgs.ShiftStartDate = dtpDate.Text.Replace("-", "");
                shiftArgs.ShiftStartTime = dtpTime.Text.Replace(":", "");
                shiftArgs.ShiftIntervalOption = intervalOption;
                shiftArgs.ShiftIntervalTime = 24 / int.Parse(nbxShift.Text);
                shiftArgs.ShiftIntervalDay = int.Parse(nbxDate.Text);
                shiftArgs.InsertTime = DateTime.Now.ToString("yyyyMMddHHmmssff");
                shiftArgs.InsertUser = InfoBase._USER_INFO.Name;

                bs.ExecuteModify("DeleteShift", shiftArgs.getPack());
                bs.ExecuteModify("InsertShift", shiftArgs.getPack());


            }
            catch(System.Exception ex)
            {
                TAP.UI.TAPMsgBox.Instance.ShowMessage(this.Text, EnumMsgType.ERROR, "Failed to Save.");
                return;
            }
           
        }       


        private void rbtnCustom_CheckedChanged(object sender, EventArgs e)
        {
            if (rbtnCustom.Checked)
            {
                nbxDate.Enabled = true;
                intervalOption = "CID";
            }
            else
            {
                nbxDate.Enabled = false;
            }
        }

        private void rbtnLast_CheckedChanged(object sender, EventArgs e)
        {
            if(rbtnLast.Checked)
            {
                intervalOption = "LDEM";
            }

        }

        private void rbtnFirst_CheckedChanged(object sender, EventArgs e)
        {
            if(rbtnFirst.Checked)
            {
                intervalOption = "FDEM";
            }
        }

        private void btnGroupAdd_Click(object sender, EventArgs e)
        {
            WorkGroupAdd groupListAdd = new WorkGroupAdd();
            groupListAdd.ShowDialog();
            SetWorkGroup();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            WorkFlowArgsPack args = new WorkFlowArgsPack();
            DataTable tmpdt = new DataTable();
            BizDataClient bs2 = new BizDataClient("ISIA.BIZ.ADMINISTRATOE.dll", "ISIA.BIZ.ADMINISTRATOE.UserWorkFlow");
            tlbUser.Items.Clear();
            try
            {
                args.Position = cboPos.Text;
                args.Department = cboDep.Text;

                tmpdt = bs2.ExecuteDataTable("GetUserList", args.getPack());

                foreach (DataRow dr in tmpdt.Rows)
                {
                    string strTemp = string.Format("{0}_{1}", dr["NAME"].ToString(), dr["USERNAME"].ToString());
                    tlbUser.Items.Add(strTemp);
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        private void clbWorkGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            WorkGroupArgsPack args = new WorkGroupArgsPack();
            DataTable tmpdt = new DataTable();
            try
            {
                if (((TCheckedListBox)sender).SelectedIndex < 0)
                    return;
                tlbGroupUser.Items.Clear();
                args.Name = ((TCheckedListBox)sender).SelectedItem.ToString();
                args.Region = InfoBase._USER_INFO.Region;

                tmpdt = bs.ExecuteDataTable("GetWorkGroupUser", args.getPack());

                foreach (DataRow dr in tmpdt.Rows)
                {
                    string strTemp = string.Format("{0}_{1}", dr["USERID"].ToString(), dr["USERNAME"].ToString());
                    tlbGroupUser.Items.Add(strTemp);
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (grdWork.SelectedRowsCount < 1)
                return;

            int[] temphandel = grdWork.GetSelectedRows();

            object objtemp = grdWork.GetRow(grdWork.GetSelectedRows()[0]);

            DataRowView dr = (DataRowView)objtemp;

            string insertTime = DateTime.Now.ToString("yyyyMMddHHmmssff");

            foreach (object item in tlbUser.SelectedItems)
            {               
                if (!tlbGroupUser.Items.Contains(item))
                {
                    tlbGroupUser.Items.Add(item);

                    WorkGroupArgsPack args = new WorkGroupArgsPack();

                    args.Region = UIInformation.Region;
                    args.Name = dr["NAME"].ToString();
                    string[] strTemp = item.ToString().Split('_');                    
                    args.UserId = strTemp[0];
                    args.InsertTime = insertTime;
                    args.InsertUser = InfoBase._USER_INFO.Name;
                    args.IsAlive = "YES";

                    int saveCount = bs.ExecuteModify("InsertWorkGroupUser", args.getPack());
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (grdWork.SelectedRowsCount < 1)
                return;

            int[] temphandel = grdWork.GetSelectedRows();

            object objtemp = grdWork.GetRow(grdWork.GetSelectedRows()[0]);

            DataRowView dr = (DataRowView)objtemp;

            ArrayList arrayList = new ArrayList();

            foreach (object item in tlbGroupUser.SelectedItems)
            {
                WorkGroupArgsPack args = new WorkGroupArgsPack();
                
                args.Region = UIInformation.Region;
                args.Name = dr["NAME"].ToString();
                string[] strTemp = item.ToString().Split('_');
                args.UserId = strTemp[0];
                int deleteCount = bs.ExecuteModify("DeleteWorkGroupUser", args.getPack());
                arrayList.Add(item);
            }
            for(int i = 0; i < arrayList.Count; i++)
            {
                tlbGroupUser.Items.Remove(arrayList[i]);
            }
        }

        private void btnUP_Click(object sender, EventArgs e)
        {
            WorkGroupArgsPack args = new WorkGroupArgsPack();
            DataTable tmpdt = new DataTable();
            try
            {
                //if (clbWorkGroup.SelectedIndex == 0)
                    return;


                //args.Name = clbWorkGroup.SelectedItem.ToString();
                args.Region = this.UIInformation.Region;
                //args.Sequences = clbWorkGroup.SelectedIndex.ToString();

                tmpdt = bs.ExecuteDataTable("UpdateWorkGroupSeqUp", args.getPack());

                SetWorkGroup();
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        private void btnDown_Click(object sender, EventArgs e)
        {
            WorkGroupArgsPack args = new WorkGroupArgsPack();
            DataTable tmpdt = new DataTable();
            try
            {
                if (((TCheckedListBox)sender).SelectedIndex == 0)
                    return;


                args.Name = ((TCheckedListBox)sender).SelectedItem.ToString();
                args.Region = this.UIInformation.Region;
                args.Sequences = ((TCheckedListBox)sender).SelectedIndex.ToString();

                tmpdt = bs.ExecuteDataTable("UpdateWorkGroupSeqDown", args.getPack());

                SetWorkGroup();
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        private void btnDeleteGroup_Click(object sender, EventArgs e)
        {
            WorkGroupArgsPack args = new WorkGroupArgsPack();
            DataTable tmpdt = new DataTable();
            try
            {
                if (grdWork.SelectedRowsCount < 1)
                    return;

                int[] temphandel = grdWork.GetSelectedRows();

                object objtemp = grdWork.GetRow(grdWork.GetSelectedRows()[0]);

                DataRowView dr = (DataRowView)objtemp;                

                args.Name = dr["NAME"].ToString();
                args.Region = this.UIInformation.Region;
                args.Sequences = dr["SEQUENCES"].ToString();
                int Count = bs.ExecuteModify("DeleteWorkGroup", args.getPack());

                SetWorkGroup();
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
        #endregion
        
        private void grdWork_RowClick(object sender, RowClickEventArgs e)
        {
            WorkGroupArgsPack args = new WorkGroupArgsPack();
            DataTable tmpdt = new DataTable();
            
            try
            {
                if (selectIndex == e.RowHandle)
                    return;

                object objtemp = grdWork.GetRow(e.RowHandle);

                DataRowView drview = (DataRowView)objtemp;

                tlbGroupUser.Items.Clear();
                args.Name = drview["NAME"].ToString();
                args.Region = InfoBase._USER_INFO.Region;

                tmpdt = bs.ExecuteDataTable("GetWorkGroupUser", args.getPack());

                foreach (DataRow dr in tmpdt.Rows)
                {
                    string strTemp = string.Format("{0}_{1}", dr["USERID"].ToString(), dr["USERNAME"].ToString());
                    tlbGroupUser.Items.Add(strTemp);
                }
                selectIndex = e.RowHandle;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
    }
}
