using DevExpress.XtraEditors;
using ISIA.INTERFACE.ARGUMENTSPACK;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TAP.Data.Client;
using TAP.UI;
using ISIA.COMMON;
using ISIA.UI.BASE;

namespace ISIA.UI.ADMINISTRATOE
{
    public partial class FrmUserMailManagement : DockUIBase1T1
    {
        public FrmUserMailManagement()
        {
            InitializeComponent();
            bs = new BizDataClient("ISIA.BIZ.ADMINISTRATOE.DLL", "ISIA.BIZ.ADMINISTRATOE.UserMailManagement");
            //OptionControls.Add(cboFacility);
            //OptionControls.Add(cboMailGroup);
        }

        #region Feild

        ComboBoxControl ComboBoxControl = new ComboBoxControl();
        DataSet ds = new DataSet();
        string groupname = "";
        BizDataClient bs = null;
        CommonArgsPack args = new CommonArgsPack();
        DataRow[] RightListDr = null;
        ////List<TAP.UIControls.IUIControl> OptionControls = new List<TAP.UIControls.IUIControl>();
        #endregion

        #region Method

        private void btnSelect_Click(object sender, EventArgs e)
        {
            ComboBoxControl.SetCrossLang(this._translator);

            if (base.ValidateUserInput(this.layoutControl1))
            {
                base.BeginAsyncCall("LoadData", "DisplayData", EnumDataObject.DATASET);
            }

        }
        public DataSet LoadData()
        {
            try
            {
                if (ds.Tables.Count > 0)
                {
                    ds.Clear();
                    ds.Tables.Clear();
                }
                groupname = cboMailGroup.Text;
                args.GroupName = groupname;
                ds = bs.ExecuteDataSet("GetGroupUser", args.getPack());

                return ds;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        //DisplayData  gridview显示数据
        public void DisplayData(DataSet dataSet)
        {
            if (dataSet == null)
            {
                return;
            }
            //显示数据
            LeftList.Items.Clear();
            if (dataSet.Tables["GROUPUSER"].Rows.Count > 0)
            {
                foreach (DataRow dr in dataSet.Tables["GROUPUSER"].Rows)
                {
                    LeftList.Items.Add(string.Format("{0} ({1})", dr["USERID"].ToString(), dr["USERNAME"].ToString()));
                }
            }
            // RightList.Items.Clear();
            //if (dataSet.Tables["USER"].Rows.Count > 0)
            //{
            //    foreach (DataRow dr in dataSet.Tables["USER"].Rows)
            //    {
            //        RightList.Items.Add(string.Format("{0} ({1})", dr["NAME"].ToString(), dr["USERNAME"].ToString()));
            //    }
            //}

            cboDep.changeUpdateStatus(true);
            cboPos.changeUpdateStatus(true);
            cboDep.Text = null;
            cboPos.Text = null;
        }
        private void SearchRightList(string Filter = "")
        {

            if (ds.Tables.Count > 0)
            {
                string Pos = cboPos.Text;
                string department = cboDep.Text;
                StringBuilder tmpsql = new StringBuilder();
                tmpsql.Append("1=1 ");
                if (!string.IsNullOrEmpty(Pos))
                {
                    tmpsql.AppendFormat("AND POSITION IN({0})", Utils.MakeSqlQueryIn(Pos, ','));
                }
                if (!string.IsNullOrEmpty(department))
                {
                    tmpsql.AppendFormat("AND DEPARTMENT IN({0})", Utils.MakeSqlQueryIn(department, ','));
                }
                DataRow[] dataRows = ds.Tables["USER"].Select(tmpsql.ToString());
                RightListDr = dataRows;
                RightList.Items.Clear();
                if (dataRows.Length > 0)
                {
                    //for (int i = 0; i < dataRows.Length; i++)
                    //{
                    //    RightList.Items.Add(dataRows[i]["NAME"].ToString());
                    //}
                    foreach (DataRow dr in dataRows)
                    {
                        if (!chkFilter.Checked)
                        {
                            RightList.Items.Add(string.Format("{0} ({1})", dr["NAME"].ToString(), dr["USERNAME"].ToString()));
                        }
                        else
                        {
                            if ((dr["NAME"].ToString().Contains(Filter) || dr["USERNAME"].ToString().Contains(Filter)) && chkFilter.Checked)
                            {
                                RightList.Items.Add(string.Format("{0} ({1})", dr["NAME"].ToString(), dr["USERNAME"].ToString()));
                            }

                        }
                    }
                }
            }
        }
        private void SaveData()
        {
            try
            {
                List<string> delGoupname = new List<string>();
                List<string> addGoupname = new List<string>();
                //del
                //for (int i = 0; i < ds.Tables["GROUPUSER"].Rows.Count; i++)
                //{
                //    string name = ds.Tables["GROUPUSER"].Rows[i]["USERNAME"].ToString();
                //    if (!LeftList.Items.Contains(name))
                //    {
                //        delGoupname.Add(name);
                //    }
                //}

                foreach (DataRow dr in ds.Tables["GROUPUSER"].Rows)
                {
                    string item = string.Format("{0} ({1})", dr["USERID"].ToString(), dr["USERNAME"].ToString());
                    if (!LeftList.Items.Contains(item))
                    {
                        delGoupname.Add(dr["USERID"].ToString());
                    }
                }
                //add
                for (int i = 0; i < LeftList.Items.Count; i++)
                {
                    string name = LeftList.Items[i].ToString().Split(' ')[0];
                    DataRow[] dataRows = ds.Tables["GROUPUSER"].Select("USERID" + "='" + name + "'");
                    if (dataRows.Count() == 0)
                    {
                        addGoupname.Add(name);
                    }
                }
                for (int i = 0; i < delGoupname.Count; i++)
                {
                    args.GroupName = groupname;
                    args.UserName = delGoupname[i].ToString();
                    bs.ExecuteModify("DeleteGroupMember", args.getPack());
                }
                for (int i = 0; i < addGoupname.Count; i++)
                {
                    DataRow[] dataRows = ds.Tables["USER"].Select("NAME" + "='" + addGoupname[i] + "'");

                    args.GroupName = groupname;
                    args.Region = dataRows[0]["REGION"].ToString();
                    args.Facility = dataRows[0]["FACILITY"].ToString();
                    args.Department = dataRows[0]["DEPARTMENT"].ToString();
                    args.Position = dataRows[0]["POSITION"].ToString();
                    args.UserID = addGoupname[i];
                    args.UserName = dataRows[0]["USERNAME"].ToString();
                    args.InsertTime = DateTime.Now.ToString("yyyyMMddHHmmss");
                    args.InsertUser = TAP.UI.InfoBase._USER_INFO.Name;
                    bs.ExecuteModify("SaveGroupMember", args.getPack());

                }
                base.BeginAsyncCall("LoadData", "DisplayData", EnumDataObject.DATASET);
                TAPMsgBox.Instance.ShowMessage(this.Text, EnumMsgType.WARNING, " complete..");

            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        #endregion

        #region InitializeComboBox
        //private ArrayList GetItemList(DataTable dt)
        //{
        //    ArrayList array = new ArrayList();

        //    foreach (DataRow dr in dt.Rows)
        //    {
        //        array.Add(dr[0].ToString());
        //    }

        //    return array;
        //}


        #endregion

        #region Event Handlers
        //下拉框变色
        private void btnColumnRight_Click(object sender, EventArgs e)
        { //DELETE
            int SelectItems = LeftList.SelectedItems.Count;
            DialogResult dialog = DialogResult.None;
            if (SelectItems <= 0)
            {
                TAPMsgBox.Instance.ShowMessage(this.Text, EnumMsgType.WARNING, "Please choose one PMSchedule.");
                return;
            }
            else if (SelectItems > 0)
            {
                dialog = TAPMsgBox.Instance.ShowMessage(this.Text, EnumMsgType.CONFIRM, "Are you sure you want to delete these  " + SelectItems + " items");

            }
            if (dialog == DialogResult.No)
            {
                return;
            }
            if (LeftList.SelectedItems.Count > 0)
            {
                for (int i = 0; i < SelectItems; i++)
                {
                    LeftList.Items.Remove(LeftList.SelectedItems[0]);
                }
            }
            string txtcboDep = cboDep.Text;
            string txtcboPos = cboPos.Text;
            SaveData();
            cboDep.Text = txtcboDep;
            cboPos.Text = txtcboPos;
            btnSaerch_Click(null, null);
        }

        private void btnColumnLeft_Click(object sender, EventArgs e)
        {
            int SelectItems = RightList.SelectedItems.Count;
            DialogResult dialog = DialogResult.None;
            if (SelectItems <= 0)
            {
                TAPMsgBox.Instance.ShowMessage(this.Text, EnumMsgType.WARNING, "Please choose one PMSchedule.");
                return;
            }
            else if (SelectItems > 0)
            {
                dialog = TAPMsgBox.Instance.ShowMessage(this.Text, EnumMsgType.CONFIRM, "Are you sure you want to ADD these  " + SelectItems + " items");
            }
            if (dialog == DialogResult.No)
            {
                return;
            }

            //ADD
            if (RightList.SelectedIndex != -1)
            {
                for (int i = 0; i < RightList.SelectedItems.Count; i++)
                {
                    string temp = RightList.SelectedItems[i].ToString();
                    //left

                    if (!LeftList.Items.Contains(temp))
                    {
                        LeftList.Items.Add(temp);
                    }
                }
            }
            else
            {
                return;
            }
            string txtcboDep = cboDep.Text;
            string txtcboPos = cboPos.Text;
            SaveData();
            cboDep.Text = txtcboDep;
            cboPos.Text = txtcboPos;
            btnSaerch_Click(null, null);

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                bool isName = cboMailGroup.Properties.Items.Contains(txtName.Text);
                if (isName)
                {
                    TAPMsgBox.Instance.ShowMessage(this.Text, EnumMsgType.WARNING, "Added already exists..");
                    return;
                }
                else
                {
                    args.Name = txtName.Text;
                    args.Description = txtDESCRIPTION.Text;
                    args.InsertTime = DateTime.Now.ToString("yyyyMMddHHmmss");
                    args.InsertUser = TAP.UI.InfoBase._USER_INFO.Name;
                    int AddCount = bs.ExecuteModify("SaveMailGroup", args.getPack());
                    if (AddCount == 0)
                    {
                        TAPMsgBox.Instance.ShowMessage(this.Text, EnumMsgType.WARNING, "Insert failure..");
                    }
                    else
                    {
                        TAPMsgBox.Instance.ShowMessage(this.Text, EnumMsgType.WARNING, "Insert complete..");
                    }
                    cboMailGroup.changeUpdateStatus(true);
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void btnSaerch_Click(object sender, EventArgs e)
        {
            SearchRightList(txtFilter.Text);
        }

        private void cboMailGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(cboMailGroup.Text))
            {
                args.GroupName = cboMailGroup.Text;
                ds = bs.ExecuteDataSet("GetDescription", args.getPack());
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    MemDESCRIPTION.Text = ds.Tables[0].Rows[0]["DESCRIPTION"].ToString();
                }
            }
        }
        private void chkFilter_CheckedChanged(object sender, EventArgs e)
        {
            if (chkFilter.Checked)
            {
                if (RightListDr == null)
                {
                    return;
                }
                else
                {
                    SearchRightList(txtFilter.Text);
                }
            }
            else
            {
                if (RightListDr == null)
                {
                    return;
                }
                else
                {
                    SearchRightList();
                }
            }
        }
        private void txtFilter_EditValueChanged(object sender, EventArgs e)
        {
            if (chkFilter.Checked)
            {
                if (RightListDr == null)
                {
                    return;
                }
                else
                {
                    SearchRightList(txtFilter.Text);
                }
            }
            else
            {
                return;
            }
        }


        #endregion


    }
}
