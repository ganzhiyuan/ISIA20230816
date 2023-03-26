using DevExpress.XtraEditors.Controls;
using ISIA.INTERFACE.ARGUMENTSPACK;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using TAP;
using TAP.Data.Client;
using TAP.Fressage;
using TAP.Models.User;
using TAP.UI;
using ISIA.UI.BASE;

namespace ISIA.UI.ADMINISTRATOE
{
    public partial class FrmGroupUserManagement : UIBase
    {
        public FrmGroupUserManagement()
        {
            InitializeComponent();
            bs = new BizDataClient("ISIA.BIZ.ADMINISTRATOE.DLL", "ISIA.BIZ.ADMINISTRATOE.GroupUserManagement");
            InitializeGroupList();
            InitializecboUserColumn();
            InitializeUsergrid();
            GridviewStyle();

            int temp = Convert.ToInt32(panelControl1.Height * 0.48);
            flowLayoutPanel2.Height = temp;
        }

        #region Feild            
        string addGroupname = "";
        bool valid = true;
        BizDataClient bs = null;
        DataTable groupDt = null;
        DataTable groupMemberDt = null;
        DataTable userDt = null;
        #endregion

        #region Method
        private void InitializecboUserColumn()
        {
            
        }

        private void InitializeGroupList()
        {
            imageListBoxControl1.Items.Clear();

            DataTable dt = bs.ExecuteDataTable("GetGroup");
            groupDt = dt;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmGroupUserManagement));
            List<ImageListBoxItem> imageListBoxItems = new List<ImageListBoxItem>();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DevExpress.XtraEditors.Controls.ImageListBoxItemImageOptions imageListBoxItemImageOptions = new DevExpress.XtraEditors.Controls.ImageListBoxItemImageOptions();
                imageListBoxItemImageOptions.ImageIndex = i;
                if (dt.Rows[i]["NAME"].ToString() == "ADMIN")
                {
                    imageListBoxItemImageOptions.Image = Properties.Resources.user_gray;
                }
                else if (dt.Rows[i]["NAME"].ToString() == "GUEST")
                {
                    imageListBoxItemImageOptions.Image = Properties.Resources.user_orange;
                }
                else if (dt.Rows[i]["NAME"].ToString() == "MANAGER")
                {
                    imageListBoxItemImageOptions.Image = Properties.Resources.user_green;
                }
                else
                {
                    imageListBoxItemImageOptions.Image = Properties.Resources.user;
                }
                ImageListBoxItem imageListBoxItem = new ImageListBoxItem(dt.Rows[i]["NAME"].ToString(), "", imageListBoxItemImageOptions, null);
                imageListBoxItems.Add(imageListBoxItem);
            }
            ImageListBoxItem[] imageListBoxItems1 = imageListBoxItems.ToArray<ImageListBoxItem>();
            imageListBoxControl1.Items.AddRange(imageListBoxItems1);
        }

        private void RefreshUsergroupgrid(string item)
        {
            gridControl1.DataBindings.Clear();
            CommonArgsPack argsPack = new CommonArgsPack();
            argsPack.GroupName = item;
            DataTable dt = bs.ExecuteDataTable("GetGroupMember", argsPack.getPack());
            groupMemberDt = dt;
            List<string> ids = new List<string>();
            for (int i = 0; i < groupMemberDt.Rows.Count; i++)
            {
                ids.Add(groupMemberDt.Rows[i]["NAME"].ToString());
            }
            gridControl1.DataSource = groupMemberDt;
            GetUsergridById(ids);
            GridviewStyle();

        }


        private void GetUsergridById(List<string> ids = null)
        {
            gridView2.ClearSelection();
            if (ids == null || ids.Count == 0)
            {
                return;
            }
            gridControl2.DataBindings.Clear();

            // DataTable dt = userModel.LoadModelDataList(retVal);
            DataTable dt = userDt;
            gridControl2.DataSource = dt;
            this.gridView2.Columns["LASTEVENTCOMMENT"].Visible = false;
            this.gridView2.Columns["LASTEVENT"].Visible = false;
            this.gridView2.Columns["LASTEVENTFLAG"].Visible = false;
            this.gridView2.Columns["LASTEVENTTIME"].Visible = false;
            this.gridView2.Columns["LASTEVENTCODE"].Visible = false;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                for (int j = 0; j < ids.Count; j++)
                {
                    if (dt.Rows[i]["NAME"].ToString().ToUpper() == ids[j].ToUpper())
                    {
                        gridView2.SelectRow(i);
                    }
                }
            }
            GridviewStyle();
        }

        private void InitializeUsergrid()
        {
            gridControl2.DataBindings.Clear();
            //DataTable dt = userModel.LoadModelDataList(retVal);
            userDt = bs.ExecuteDataTable("GetUsers");
            DataTable dt = userDt;
            gridControl2.DataSource = dt;
            this.gridView2.Columns["LASTEVENTCOMMENT"].Visible = false;
            this.gridView2.Columns["LASTEVENT"].Visible = false;
            this.gridView2.Columns["LASTEVENTFLAG"].Visible = false;
            this.gridView2.Columns["LASTEVENTTIME"].Visible = false;
            this.gridView2.Columns["LASTEVENTCODE"].Visible = false;
        }

        private void ShowDescription(string item)
        {
            txtDescription.Text = "DESCRIPTION: ";
            groupDt.DefaultView.RowFilter = String.Format("NAME ='{0}'", item);
            DataTable dt = groupDt.DefaultView.ToTable();
            groupDt.DefaultView.RowFilter = String.Empty;
            List<string> vs = new List<string>();
            if (dt.Rows.Count == 0)
            {
                return;
            }
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                vs.Add(dt.Rows[i]["DESCRIPTION"].ToString());
            }
            String desctiption = "";
            for (int i = 0; i < vs.Count; i++)
            {
                if (vs[i].Replace(" ", "") == "")
                {

                }
                else
                {
                    desctiption += vs[i] + ",";
                }
            }
            if (desctiption == "" || desctiption == null)
            {
                txtDescription.Text = "DESCRIPTION: ";
                return;
            }
            else
            {
                txtDescription.Text = "DESCRIPTION: " + desctiption.Substring(0, desctiption.Length - 1);
            }
        }

        private void GridviewStyle()
        {
            //显示滚动条
            this.gridView1.OptionsView.ColumnAutoWidth = false;
            //列表宽度自适应内容
            this.gridView1.BestFitColumns();
            gridView1.Appearance.HorzLine.BackColor = Color.White;
            gridView1.Appearance.VertLine.BackColor = Color.White;
            //不可编辑
            gridView1.OptionsBehavior.Editable = false;

            //显示滚动条
            this.gridView2.OptionsView.ColumnAutoWidth = false;
            //列表宽度自适应内容
            this.gridView2.BestFitColumns();
            //显示复选框
            gridView2.OptionsSelection.MultiSelectMode = DevExpress.XtraGrid.Views.Grid.GridMultiSelectMode.CheckBoxRowSelect;
            gridView2.OptionsSelection.CheckBoxSelectorColumnWidth = 30;
            gridView2.OptionsSelection.MultiSelect = true;
            gridView2.OptionsSelection.ShowCheckBoxSelectorInColumnHeader = DevExpress.Utils.DefaultBoolean.True;
            gridView2.Appearance.HorzLine.BackColor = Color.White;
            gridView2.Appearance.VertLine.BackColor = Color.White;
            //不可编辑
            gridView2.OptionsBehavior.Editable = false;
            gridView2.Columns[0].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
        }
        #endregion

        #region Event

        private void imageListBoxControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (imageListBoxControl1.SelectedItem == null)
            {
                return;
            }
            string item = imageListBoxControl1.SelectedItem.ToString();
            addGroupname = item;
            RefreshUsergroupgrid(item);
            ShowDescription(item);
        }

        private void navBarControl1_Resize(object sender, EventArgs e)
        {
            // navBarGroupControlContainer2.Height = splitContainerControl1.Panel1.Height - navBarGroupControlContainer1.Height;
        }

        //Delete Group List Value 
        private void Delete_Click(object sender, EventArgs e)
        {
            if (imageListBoxControl1.SelectedItem == null)
            {
                return;
            }
            string item = imageListBoxControl1.SelectedItem.ToString();

            string tmpMessage = _translator.ConvertGeneralTemplate(EnumVerbs.DELETE, EnumGeneralTemplateType.CONFIRM, "");
            DialogResult dialog = TAP.UI.TAPMsgBox.Instance.ShowMessage(Text, EnumMsgType.CONFIRM, tmpMessage);
            if (dialog.ToString() == "Yes")
            {
                try
                {
                    CommonArgsPack argsPack = new CommonArgsPack();
                    argsPack.Name = item;
                    argsPack.GroupName = item;
                    bs.ExecuteModify("DeleteGroupAndGroupmember", argsPack.getPack());

                    RefreshUsergroupgrid(addGroupname);
                    InitializeGroupList();
                    txtDescription.Text = "";
                }
                catch (System.Exception ex)
                {
                    TAP.UI.TAPMsgBox.Instance.ShowMessage(this.Text, EnumMsgType.ERROR, ex.ToString());
                }
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            GroupListAdd groupListAdd = new GroupListAdd();
            groupListAdd.ShowDialog();
            InitializeGroupList();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (imageListBoxControl1.SelectedItem == null)
                {
                    TAPMsgBox.Instance.ShowMessage(this.Text, EnumMsgType.WARNING, "Please choose  Group List..");

                    return;
                }
                string selectitem = imageListBoxControl1.SelectedItem.ToString();

                groupDt.DefaultView.RowFilter = String.Format("NAME ='{0}'", selectitem);
                DataTable dt = groupDt.DefaultView.ToTable();
                groupDt.DefaultView.RowFilter = String.Empty;
                GroupListUpdate update = new GroupListUpdate();
                DataSet dataSet = new DataSet();
                dataSet.Tables.Add(dt);
                update.Receipt(dataSet);
                update.ShowDialog();
                InitializeGroupList();
            }
            catch (System.Exception ex)
            {
                TAP.UI.TAPMsgBox.Instance.ShowMessage(this.Text, EnumMsgType.ERROR, ex.ToString());
            }
        }

        //Delete User Group List Value
        private void Delete1_Click(object sender, EventArgs e)
        {
            if (imageListBoxControl1.SelectedItem == null)
            {
                return;
            }
            string item = imageListBoxControl1.SelectedItem.ToString();

            string tmpMessage = _translator.ConvertGeneralTemplate(EnumVerbs.DELETE, EnumGeneralTemplateType.CONFIRM, "");
            DialogResult dialog = TAP.UI.TAPMsgBox.Instance.ShowMessage(Text, EnumMsgType.CONFIRM, tmpMessage);
            if (dialog.ToString() == "Yes")
            {
                List<int> selectRowNum = new List<int>();
                try
                {
                    if (gridView1.SelectedRowsCount <= 0)
                        return;
                    foreach (int rowhandel in gridView1.GetSelectedRows())
                    {
                        selectRowNum.Add(rowhandel);
                    }
                    List<CommonArgsPack> argsPacks = new List<CommonArgsPack>();
                    for (int i = 0; i < selectRowNum.Count; i++)
                    {
                        CommonArgsPack args = new CommonArgsPack();
                        DataRow tmpRow = gridView1.GetDataRow(selectRowNum[i]);

                        string USERGROUP = tmpRow["USERGROUP"].ToString();
                        string NAME = tmpRow["NAME"].ToString();
                        string REGION = tmpRow["REGION"].ToString();
                        string FACILITY = tmpRow["FACILITY"].ToString();
                        args.Name = NAME;
                        args.GroupName = USERGROUP;
                        args.Region = REGION;
                        args.Facility = FACILITY;

                        argsPacks.Add(args);
                    }

                    ArgumentPack tmpap = new ArgumentPack();
                    tmpap.AddArgument("arguments", typeof(List<CommonArgsPack>), argsPacks);
                    bs.ExecuteModify("DeleteGroupmember", tmpap);

                    RefreshUsergroupgrid(item);
                }
                catch (System.Exception ex)
                {
                    TAP.UI.TAPMsgBox.Instance.ShowMessage(this.Text, EnumMsgType.ERROR, ex.ToString());
                }
            }
        }

        private void btnUserAdd_Click(object sender, EventArgs e)
        {
            UserAdd userAdd = new UserAdd();
            userAdd.ShowDialog();
            InitializeUsergrid();
        }

        private void btnUserUpdate_Click(object sender, EventArgs e)
        {
            List<int> selectRowNum = new List<int>();
            if (gridView2.SelectedRowsCount == 1)
            {
                foreach (int rowhandel in gridView2.GetSelectedRows())
                {
                    selectRowNum.Add(rowhandel);
                }
            }
            else
            {
                TAPMsgBox.Instance.ShowMessage(this.Text, EnumMsgType.WARNING, "Please choose one line");
                return;
            }
            DataRow tmpRow = gridView2.GetDataRow(selectRowNum[0]);

            UserUpdate update = new UserUpdate();
            update.Receipt(tmpRow);
            update.ShowDialog();
            InitializeUsergrid();
            gridView2.RefreshData();
        }
        //user list
        private void btnUserDelete_Click(object sender, EventArgs e)
        {
            string tmpMessage = _translator.ConvertGeneralTemplate(EnumVerbs.DELETE, EnumGeneralTemplateType.CONFIRM, "");
            DialogResult dialog = TAP.UI.TAPMsgBox.Instance.ShowMessage(Text, EnumMsgType.CONFIRM, tmpMessage);
            if (dialog.ToString() == "Yes")
            {
                List<int> selectRowNum = new List<int>();
                try
                {
                    if (gridView2.SelectedRowsCount <= 0)
                        return;

                    foreach (int rowhandel in gridView2.GetSelectedRows())
                    {
                        selectRowNum.Add(rowhandel);
                    }
                    List<CommonArgsPack> argsPacks = new List<CommonArgsPack>();
                    for (int i = 0; i < selectRowNum.Count; i++)
                    {
                        CommonArgsPack args = new CommonArgsPack();
                        DataRow tmpRow = gridView2.GetDataRow(selectRowNum[i]);
                        string user = tmpRow["NAME"].ToString();
                        string region = tmpRow["REGION"].ToString();
                        string facility = tmpRow["FACILITY"].ToString();

                        args.Name = user;
                        args.Region = region;
                        args.Facility = facility;
                        argsPacks.Add(args);
                    }
                    ArgumentPack argsPack = new ArgumentPack();
                    argsPack.AddArgument("arguments", typeof(List<CommonArgsPack>), argsPacks);
                    int delcount = bs.ExecuteModify("DeleteUsers", argsPack);
                    if (delcount == 0)
                    {
                        TAPMsgBox.Instance.ShowMessage(this.Text, EnumMsgType.WARNING, "Delete User failure..");
                    }
                    else
                    {
                        TAPMsgBox.Instance.ShowMessage(this.Text, EnumMsgType.WARNING, "Delete User complete..");
                    }
                    InitializeUsergrid();
                }
                catch (System.Exception ex)
                {
                    TAP.UI.TAPMsgBox.Instance.ShowMessage(this.Text, EnumMsgType.ERROR, ex.ToString());
                }
            }
        }

        private void btnGroup_Click(object sender, EventArgs e)
        {
            
        }

        private void txtFilter_KeyPress(object sender, KeyPressEventArgs e)
        {
            valid = true;
            if (e.KeyChar.ToString().Trim().Contains("'") || e.KeyChar.ToString().Trim().Contains("\"") || e.KeyChar.ToString().Trim().Contains("‘")
                || e.KeyChar.ToString().Trim().Contains("’") || e.KeyChar.ToString().Trim().Contains("“") || e.KeyChar.ToString().Trim().Contains("”"))
            {
                valid = false;
            }
        }

        private void tButton2_Click(object sender, EventArgs e)
        {
            if (addGroupname == "")
            {
                TAPMsgBox.Instance.ShowMessage(this.Text, EnumMsgType.WARNING, "Please choose Group List..");
                return;
            }
            int SaveCount = 0;
            List<int> selectRowNum = new List<int>();
            try
            {
                if (gridView2.SelectedRowsCount <= 0)
                    return;

                foreach (int rowhandel in gridView2.GetSelectedRows())
                {
                    selectRowNum.Add(rowhandel);
                }
                List<string> ids = new List<string>();

                List<CommonArgsPack> updateArgsPacks = new List<CommonArgsPack>();
                List<CommonArgsPack> addArgsPacks = new List<CommonArgsPack>();
                for (int i = 0; i < selectRowNum.Count; i++)
                {
                    DataRow tmpRow = gridView2.GetDataRow(selectRowNum[i]); ;

                    ids.Add(tmpRow["NAME"].ToString());

                    string name = tmpRow["NAME"].ToString();
                    string region = tmpRow["REGION"].ToString();
                    string facility = tmpRow["FACILITY"].ToString();

                    DataTable dt = userDt;
                    DataRow[] drs = dt.Select("NAME='" + name + "' AND FACILITY= '" + facility + "' AND REGION='" + region + "'");

                    DataTable dtNew = dt.Clone();
                    foreach (DataRow dr in drs)
                    {
                        dtNew.ImportRow(dr);
                    }
                    string addUSERGROUP = addGroupname;
                    string addName = dtNew.Rows[0]["NAME"].ToString();
                    string addREGION = dtNew.Rows[0]["REGION"].ToString();
                    string addFACILITY = dtNew.Rows[0]["FACILITY"].ToString();
                    string addDEPARTMENT = dtNew.Rows[0]["DEPARTMENT"].ToString();
                    string addPOSITION = dtNew.Rows[0]["POSITION"].ToString();
                    string addUSERNAME = dtNew.Rows[0]["USERNAME"].ToString();
                    string addUSERLASTNAME = dtNew.Rows[0]["USERLASTNAME"].ToString();
                    string addCURRENTMODEL = dtNew.Rows[0]["CURRENTMODEL"].ToString();
                    string addDESCRIPTION = dtNew.Rows[0]["DESCRIPTION"].ToString();
                    string addLASTEVENTCOMMENT = dtNew.Rows[0]["LASTEVENTCOMMENT"].ToString();
                    string addLASTEVENT = dtNew.Rows[0]["LASTEVENT"].ToString();
                    string addLASTEVENTFLAG = dtNew.Rows[0]["LASTEVENTFLAG"].ToString();
                    string addLASTEVENTTIME = dtNew.Rows[0]["LASTEVENTTIME"].ToString();
                    string addLASTEVENTCODE = dtNew.Rows[0]["LASTEVENTCODE"].ToString();
                    string addLASTJOBCODE = dtNew.Rows[0]["LASTJOBCODE"].ToString();
                    string addINSERTTIME = dtNew.Rows[0]["INSERTTIME"].ToString();
                    //string addUPDATETIME = dtNew.Rows[0]["UPDATETIME"].ToString();
                    string addINSERTUSER = dtNew.Rows[0]["INSERTUSER"].ToString();
                    //string addUPDATEUSER = dtNew.Rows[0]["UPDATEUSER"].ToString();
                    string addSEQUENCES = dtNew.Rows[0]["SEQUENCES"].ToString();
                    string addISALIVE = dtNew.Rows[0]["ISALIVE"].ToString();
                    string addMODELLEVELS = dtNew.Rows[0]["MODELLEVELS"].ToString();
                    string addLANGUAGE = dtNew.Rows[0]["LANGUAGE"].ToString();

                    CommonArgsPack args1 = new CommonArgsPack();
                    args1.GroupName = addGroupname;
                    args1.Name = addName;
                    args1.Region = addREGION;
                    args1.Facility = addFACILITY;
                    DataTable dtGroupMember = bs.ExecuteDataTable("GetGroupMemberByUser", args1.getPack());
                    DataRow[] drsGroupMember = dtGroupMember.Select("USERGROUP='" + addGroupname + "' AND NAME='" + addName + "' AND FACILITY= '" + addFACILITY + "' AND REGION='" + addREGION + "'");
                    CommonArgsPack args = new CommonArgsPack();
                    args.Name = addName;
                    args.Region = addREGION;
                    args.Facility = addFACILITY;
                    args.Department = addDEPARTMENT;
                    args.Position = addPOSITION;
                    args.UserName = addUSERNAME;
                    args.UserLastName = addUSERLASTNAME;
                    args.CurrentModel = addCURRENTMODEL;
                    args.Description = addDESCRIPTION;
                    args.LastEventComment = addLASTEVENTCOMMENT;
                    args.LastEvent = addLASTEVENT;
                    args.LastEventFlag = addLASTEVENTFLAG;
                    args.LastEventTime = addLASTEVENTTIME;
                    args.LastEventCode = addLASTEVENTCODE;
                    args.LastJobCode = addLASTJOBCODE;
                    args.IsAlive = addISALIVE;
                    args.GroupName = addUSERGROUP;
                    args.Sequences = addSEQUENCES;
                    args.Modellevels = addMODELLEVELS;
                    if (string.IsNullOrEmpty(addMODELLEVELS))
                    {
                        args.Modellevels = "0";
                    }
                    else { args.Modellevels = addMODELLEVELS; }
                    args.Language = addLANGUAGE;

                    if (drsGroupMember.Length > 0)
                    {//update
                        args.InsertUser = addINSERTUSER;
                        args.InsertTime = addINSERTTIME;
                        args.UpdateUser = InfoBase._USER_INFO.UserName;
                        args.UpdateTime = DateTime.Now.ToString("yyyyMMddHHmmss");
                        updateArgsPacks.Add(args);
                    }
                    else
                    {//add
                        args.InsertUser = InfoBase._USER_INFO.UserName;
                        args.InsertTime = DateTime.Now.ToString("yyyyMMddHHmmss");
                        args.UpdateUser = "";
                        args.UpdateTime = "";
                        addArgsPacks.Add(args);
                    }
                }
                ArgumentPack addtmpap = new ArgumentPack();
                addtmpap.ClearArguments();
                addtmpap.AddArgument("arguments", typeof(List<CommonArgsPack>), addArgsPacks);
                int savecount = bs.ExecuteModify("SaveGroupmember", addtmpap);
                ArgumentPack updatetmpap = new ArgumentPack();
                updatetmpap.ClearArguments();
                updatetmpap.AddArgument("arguments", typeof(List<CommonArgsPack>), updateArgsPacks);
                int updatecount = bs.ExecuteModify("UpdateGroupmember", updatetmpap);

                SaveCount = savecount + updatecount;
                if (SaveCount == 0)
                {
                    TAPMsgBox.Instance.ShowMessage(this.Text, EnumMsgType.WARNING, "Group failure..");
                }
                else
                {
                    TAPMsgBox.Instance.ShowMessage(this.Text, EnumMsgType.WARNING, "Group complete..");
                }
                RefreshUsergroupgrid(addGroupname);
                GetUsergridById(ids);
            }
            catch (System.Exception ex)
            {
                TAP.UI.TAPMsgBox.Instance.ShowMessage(this.Text, EnumMsgType.ERROR, ex.ToString());
            }
        }

        private void tButton1_Click(object sender, EventArgs e)
        {
            if (imageListBoxControl1.SelectedItem == null)
            {
                return;
            }
            string item = imageListBoxControl1.SelectedItem.ToString();

            string tmpMessage = _translator.ConvertGeneralTemplate(EnumVerbs.DELETE, EnumGeneralTemplateType.CONFIRM, "");
            DialogResult dialog = TAP.UI.TAPMsgBox.Instance.ShowMessage(Text, EnumMsgType.CONFIRM, tmpMessage);
            if (dialog.ToString() == "Yes")
            {
                List<int> selectRowNum = new List<int>();
                try
                {
                    if (gridView1.SelectedRowsCount <= 0)
                        return;
                    foreach (int rowhandel in gridView1.GetSelectedRows())
                    {
                        selectRowNum.Add(rowhandel);
                    }
                    List<CommonArgsPack> argsPacks = new List<CommonArgsPack>();
                    for (int i = 0; i < selectRowNum.Count; i++)
                    {
                        CommonArgsPack args = new CommonArgsPack();
                        DataRow tmpRow = gridView1.GetDataRow(selectRowNum[i]);

                        string USERGROUP = tmpRow["USERGROUP"].ToString();
                        string NAME = tmpRow["NAME"].ToString();
                        string REGION = tmpRow["REGION"].ToString();
                        string FACILITY = tmpRow["FACILITY"].ToString();
                        args.Name = NAME;
                        args.GroupName = USERGROUP;
                        args.Region = REGION;
                        args.Facility = FACILITY;

                        argsPacks.Add(args);
                    }

                    ArgumentPack tmpap = new ArgumentPack();
                    tmpap.AddArgument("arguments", typeof(List<CommonArgsPack>), argsPacks);
                    bs.ExecuteModify("DeleteGroupmember", tmpap);

                    RefreshUsergroupgrid(item);
                }
                catch (System.Exception ex)
                {
                    TAP.UI.TAPMsgBox.Instance.ShowMessage(this.Text, EnumMsgType.ERROR, ex.ToString());
                }
            }
        }

        private void gridView2_DoubleClick(object sender, EventArgs e)
        {
            //string nodeName = gridView2.GetRowCellValue(gridView2.FocusedRowHandle, "nodeName").ToString();
            DataRow tmpRow = gridView2.GetDataRow(gridView2.FocusedRowHandle);
            if (tmpRow==null)
            {
                return;
            }

            UserUpdate update = new UserUpdate();
            update.Receipt(tmpRow);
            update.ShowDialog();
            InitializeUsergrid();
            gridView2.RefreshData();
        }

        #endregion
    }
}
