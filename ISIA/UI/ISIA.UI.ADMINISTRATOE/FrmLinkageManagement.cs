using DevExpress.XtraTreeList;
using DevExpress.XtraTreeList.Nodes;
using ISIA.INTERFACE.ARGUMENTSPACK;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TAP;
using TAP.Data.Client;
using TAP.Fressage;
using TAP.Models.UIBasic;
using TAP.UI;

namespace ISIA.UI.ADMINISTRATOE
{
    public partial class FrmLinkageManagement : UIBase
    {

        BizDataClient bs = null;
        UIBasicModel uiBasicModel = new UIBasicModel();

        ArgumentPack retVal = new ArgumentPack();
        String MDI = TAP.Base.Configuration.ConfigurationManager.Instance.AppSection.MDIName;
        DataTable dtTemporary = new DataTable();
        List<treeListInfo> listDataAll = new List<treeListInfo>();

        List<string> listCID = new List<string>();
        string _groupSelected = string.Empty;


        public FrmLinkageManagement()
        {
            InitializeComponent();
            bs = new BizDataClient("ISIA.BIZ.ADMINISTRATOE.DLL", "ISIA.BIZ.ADMINISTRATOE.LinkageManagement");
            UIListData();
            InitializeUIGroup();
        }


        private void UIListData()
        {
            GroupListtw.Nodes.Clear();
            DataTable dt = bs.ExecuteDataSet("GetCtLinkageParent").Tables[0];

            //string sql = string.Format("select NAME from TAPUTGROUP WHERE ISALIVE='YES'");
            //DataSet ds = DEC.SelectDataSet(sql, "");
            List<TreeNode> treeNodes = new List<TreeNode>();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                //TreeNode treeNode = new TreeNode(dt.Rows[i]["NAME"].ToString(), GetGroupMember(dt.Rows[i]["NAME"].ToString()));
                TreeNode treeNode = new TreeNode();
                //if (dt.Rows[i]["NAME"].ToString() == "ADMIN")
                //{
                //    treeNode.ImageIndex = 1;
                //}
                //else if (dt.Rows[i]["NAME"].ToString() == "GUEST")
                //{
                //    treeNode.ImageIndex = 3;
                //}
                //else if (dt.Rows[i]["NAME"].ToString() == "MANAGER")
                //{
                //    treeNode.ImageIndex = 2;
                //}
                //else
                //{
                //    treeNode.ImageIndex = 4;
                //}
                treeNode.Name = dt.Rows[i]["GROUPNAME"].ToString();
                treeNode.Text = dt.Rows[i]["GROUPNAME"].ToString();
                treeNodes.Add(treeNode);
            }
            TreeNode[] treeNodess = treeNodes.ToArray<TreeNode>();
            GroupListtw.Nodes.AddRange(treeNodess);
        }

        public List<T> GetList<T>(DataTable table)
        {
            List<T> list = new List<T>();
            T t = default(T);
            PropertyInfo[] propertypes = null;
            string tempName = string.Empty;
            foreach (DataRow row in table.Rows)
            {
                t = Activator.CreateInstance<T>();
                propertypes = t.GetType().GetProperties();
                foreach (PropertyInfo pro in propertypes)
                {
                    tempName = pro.Name;
                    if (table.Columns.Contains(tempName))
                    {
                        object value = row[tempName];
                        if (!value.ToString().Equals(""))
                        {
                            pro.SetValue(t, value, null);
                        }
                    }
                }
                list.Add(t);
            }
            return list.Count == 0 ? null : list;
        }

        private List<treeListInfo> GetBindSource(DataTable dt)
        {
            //定义绑定LIST，下标值
            List<treeListInfo> listData = new List<treeListInfo>();
            int count = 1;

            //取出来一二级，一级MAINMENU 二级 NAME  三级（UI表  父级-SUBMENU 对应二级NAME字段）
            DataClient tmpDataClient = new DataClient();
            string tmpMainMenuSql = "SELECT * FROM TAPSTBSUBMENU WHERE ISALIVE = 'YES'  ORDER BY SEQUENCES";
            DataTable retVal1 = tmpDataClient.SelectData(tmpMainMenuSql, "SUBMENU").Tables[0];
            List<treeListInfo> listMENU = GetList<treeListInfo>(retVal1);
            //取出一级名称
            List<string> listStr = listMENU.Select(x => x.MAINMENU).Distinct().ToList();
            foreach (var item in listStr)
            {
                treeListInfo info = new treeListInfo();
                info.CID = count++;
                info.MAINMENU = item;
                info.typ = "0";
                listData.Add(info);
            }
            //根据一级取出二级名称加入list
            List<treeListInfo> listName = GetList<treeListInfo>(retVal1);
            foreach (var item in listName)
            {
                treeListInfo info = new treeListInfo();
                info.CID = count++;
                info.MAINMENU = item.NAME;
                info.typ = "1";
                info.DISPLAYNAME = item.DISPLAYNAME;
                info.ASSEMBLYFILENAME = item.ASSEMBLYFILENAME;
                var pItem = listData.FirstOrDefault(x => x.MAINMENU == item.MAINMENU);
                if (pItem != null)
                {
                    info.PID = pItem.CID;
                }
                listData.Add(info);
            }
            //取出三级名称加入list
            List<treeListInfo> listChild = GetList<treeListInfo>(dt);
            foreach (var item in listChild)
            {
                treeListInfo info = new treeListInfo();
                info.CID = count++;
                info.MAINMENU = item.NAME;
                info.SUBMENU = item.SUBMENU;
                info.DISPLAYNAME = item.DISPLAYNAME;
                info.ASSEMBLYFILENAME = item.ASSEMBLYFILENAME;
                info.UI = "0";
                info.typ = "2";
                var pItem = listData.FirstOrDefault(x => x.MAINMENU == item.SUBMENU && x.PID != 0);
                if (pItem != null)
                    info.PID = pItem.CID;
                listData.Add(info);
            }
            return listData;
        }

        private void InitializeUIGroup()
        {
            DataTable dt = uiBasicModel.LoadModelDataList(retVal); //当前所有菜单
            DataRow[] drs = dt.Select("MDI='" + MDI + "'");
            DataTable dtNew = dt.Clone();
            foreach (DataRow dr in drs)
            {
                dtNew.ImportRow(dr);
            }

            DataView dv = dtNew.DefaultView;
            dtTemporary = dv.ToTable(true, "MAINMENU");

            listDataAll = GetBindSource(dt);

            treeList1.KeyFieldName = "CID";
            treeList1.ParentFieldName = "PID";
            treeList1.DataSource = listDataAll;
            if (listDataAll != null)
            {
                treeList1.Columns[0].Visible = false;
                treeList1.Columns[2].Visible = false;
                treeList1.Columns[3].Visible = false;
                treeList1.Columns[4].Visible = false;
                treeList1.Columns[5].Visible = false;
                treeList1.Columns[6].Visible = false;
                treeList1.Columns[7].Visible = false;
            }

            treeList1.ExpandAll();

            //treeList2.KeyFieldName = "CID";
            //treeList2.ParentFieldName = "PID";
            //var listTe = GetBindSourceUser("");
            //treeList2.DataSource = listTe;
            //if (listTe != null)
            //{
            //    treeList2.Columns[0].Visible = false;
            //    treeList2.Columns[2].Visible = false;
            //    treeList2.Columns[3].Visible = false;
            //    treeList2.Columns[4].Visible = false;
            //}
            //treeList2.ExpandAll();
        }


        private void SetCheckedChildNodes(TreeListNode node, CheckState check)
        {
            for (int i = 0; i < node.Nodes.Count; i++)
            {
                node.Nodes[i].CheckState = check;
                SetCheckedChildNodes(node.Nodes[i], check);
            }
        }
        private void SetCheckedParentNodes(TreeListNode node, CheckState check)
        {
            if (node.ParentNode != null)
            {
                bool b = false;
                CheckState state;
                for (int i = 0; i < node.ParentNode.Nodes.Count; i++)
                {
                    state = (CheckState)node.ParentNode.Nodes[i].CheckState;
                    if (!check.Equals(state))
                    {
                        b = !b;
                        break;
                    }
                }
                node.ParentNode.CheckState = b ? CheckState.Indeterminate : check;
                SetCheckedParentNodes(node.ParentNode, check);
            }
        }

        private List<LinkInfo> GetBindSourceUI(string groupName)
        {
            DataClient tmpDataClient = new DataClient();
            string UserUISql = string.Format("select GROUPNAME,UI,TAGETUI,TAGETUINAME,PARAMETERLIST,GROUPID  from tapctlinkage where GROUPNAME='{0}' and isalive = 'YES' and TAGETUI!='0'", groupName);
            DataTable UserUIList = tmpDataClient.SelectData(UserUISql, "tapctlinkage").Tables[0];
            List<LinkInfo> linkList = GetList<LinkInfo>(UserUIList);
            if (linkList == null)
            {
                return null;
            }

            List<string> listTm = linkList.Select(x => x.TAGETUI).ToList();
            List<LinkInfo> list = linkList.Where(x => listTm.Contains(x.TAGETUI) || x.TAGETUI != "0").ToList();
            return list;
        }
        private void GetCheckNodeID(TreeListNode parentNode, TreeList treeList)
        {
            if (parentNode.Nodes.Count == 0)
            {
                return;
            }
            foreach (TreeListNode item in parentNode.Nodes)
            {
                if (item.CheckState == CheckState.Checked)
                {
                    var drRow = treeList.GetDataRecordByNode(item);
                    if (drRow != null)
                    {
                        int selId = ((treeListInfo)drRow).CID;
                        listCID.Add(selId.ToString());
                    }
                }
                GetCheckNodeID(item, treeList);
            }
        }
        private void GetBindSourceAll(List<LinkInfo> list, TreeList treeList)
        {

            treeList.DataSource = null;
            treeList.DataSource = listDataAll;
            treeList.ExpandAll();
            if (list == null)
            {

                return;
            }
            List<string> listTm = list.Select(x => x.TAGETUINAME).ToList();
            foreach (TreeListNode node in treeList.Nodes)
            {
                foreach (TreeListNode item in node.Nodes)
                {
                    foreach (TreeListNode child in item.Nodes)
                    {
                        var drRow = treeList.GetDataRecordByNode(child);
                        if (drRow != null)
                        {
                            string menuName = ((treeListInfo)drRow).DISPLAYNAME;
                            if (listTm.Contains(menuName))
                            {
                                child.CheckState = CheckState.Checked;
                                SetCheckedParentNodes(child, child.CheckState);
                            }

                        }
                    }

                }
            }

        }

        private void treeList1_CustomDrawNodeImages(object sender, DevExpress.XtraTreeList.CustomDrawNodeImagesEventArgs e)
        {
            if (e.Node.GetValue("typ") != null)
                e.SelectImageIndex = int.Parse(e.Node.GetValue("typ").ToString());
            if (e.Node.HasChildren)
                if (e.Node.Expanded)
                    e.SelectImageIndex = 3;
        }


        private void treeList1_AfterCheckNode(object sender, NodeEventArgs e)
        {
            SetCheckedChildNodes(e.Node, e.Node.CheckState);
            SetCheckedParentNodes(e.Node, e.Node.CheckState);
        }

        private void treeList1_BeforeCheckNode(object sender, CheckNodeEventArgs e)
        {
            e.State = (e.PrevState == CheckState.Checked ? CheckState.Unchecked : CheckState.Checked);
        }

        private void UiList_MouseDown(object sender, MouseEventArgs e)
        {
            if ((sender as TreeView) != null)
            {
                TreeView treeView = (TreeView)sender;
                treeView.SelectedNode = treeView.GetNodeAt(e.X, e.Y);
                if (treeView.SelectedNode != null)
                {
                    treeList2.DataSource = null;
                    var listTe = GetBindSourceUI(treeView.SelectedNode.Text);
                    _groupSelected = treeView.SelectedNode.Text;
                    treeList2.DataSource = listTe;
                    GetBindSourceAll(listTe, treeList1);
                    if (listTe != null)
                    {
                        treeList2.Columns[0].Visible = false;
                        treeList2.Columns[1].Visible = false;
                        treeList2.Columns[2].Visible = false;
                        treeList2.Columns[3].Visible = false;
                        //treeList2.Columns[4].Visible = false;
                        treeList2.Columns[5].Visible = false;
                        treeList2.Columns[6].Visible = false;
                        treeList2.Columns[7].Visible = false;
                        treeList2.Columns[8].Visible = false;
                        treeList2.Columns[9].Visible = false;
                        treeList2.Columns[10].Visible = false;
                        treeList2.Columns[11].Visible = false;
                        treeList2.Columns[12].Visible = false;
                    }
                    treeList2.ExpandAll();
                    return;
                }

                //BindSourceChange(treeView);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            FrmLinkageEdit frm = new FrmLinkageEdit();
            frm.ShowDialog();
            DataClient tmpDataClient = new DataClient();
            string tmpSql = string.Format("select *  from tapctlinkage where GROUPNAME='{0}'  and TAGETUI='0'",
            frm.args.GroupName);
            DataTable dt1 = tmpDataClient.SelectData(tmpSql, "tapctlinkage").Tables[0];
            if (dt1 == null)
            {
                return;
            }

            if (dt1.Rows.Count == 0)
            {
                bs.ExecuteModify("SaveUIGroup", frm.args.getPack());
            }
            
            UIListData();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            int count = 0;
            FrmLinkageEdit frm = new FrmLinkageEdit(_groupSelected);
            frm.ShowDialog();
            count = SetEdit(frm);
        }

        private int SetEdit(FrmLinkageEdit frm)
        {
            int count;
            if (frm.isUpdate == 0)
            {
                count = bs.ExecuteModify("UpdateUser", frm.args.getPack());
            }
            else
            {

                count = bs.ExecuteModify("SaveUIGroup", frm.args.getPack());
            }
            if (count == 0)
            {
                TAPMsgBox.Instance.ShowMessage(this.Text, EnumMsgType.WARNING, "Group failure..");
            }
            else
            {
                TAPMsgBox.Instance.ShowMessage(this.Text, EnumMsgType.WARNING, "Group complete..");
            }
            UIListData();
            return count;
        }

        private void tButton2_Click(object sender, EventArgs e)
        {
            if (GroupListtw.SelectedNode == null)
            {
                TAP.UI.TAPMsgBox.Instance.ShowMessage(Text, EnumMsgType.WARNING, "Please choose userGroup.");
                return;
            }
            listCID = new List<string>();
            if (treeList1.Nodes.Count > 0)
            {
                string groupName = GroupListtw.SelectedNode.Text;
                foreach (TreeListNode root in treeList1.Nodes)
                {
                    GetCheckNodeID(root, treeList1);
                }
                if (listCID.Count == 0)
                {
                    TAP.UI.TAPMsgBox.Instance.ShowMessage(Text, EnumMsgType.WARNING, "Please choose UI.");
                    return;
                }
                var list = listDataAll.Where(x => listCID.Contains(x.CID.ToString()) && x.UI == "0").ToList();

                DataClient tmpDataClient = new DataClient();
                string tmpMainMenuSql = string.Format("select GROUPID,GROUPNAME,TAGETUINAME,PARAMETERLIST,DESCRIPTION,ISALIVE  from tapctlinkage where GROUPNAME='{0}' and isalive = 'YES' and TAGETUI='0'", groupName);
                DataTable retVal1 = tmpDataClient.SelectData(tmpMainMenuSql, "tapctlinkage").Tables[0];
                foreach (var item in list)
                {
                    DataTable dt = bs.ExecuteDataSet("GetCtLinkageByMaxID").Tables[0];
                    CommonArgsPack args = new CommonArgsPack();
                    args.UserID = dt.Rows[0][0].ToString();
                    args.Custom01 = retVal1.Rows[0]["GROUPID"].ToString();
                    args.Custom02 = item.ASSEMBLYFILENAME;
                    args.EqpGroup = (Convert.ToInt32(dt.Rows[0][0].ToString()) + 1).ToString();
                    args.GroupName = groupName;
                    args.IsAlive = "YES";
                    args.MessageName = item.DISPLAYNAME;
                    args.PartName = "0";

                    tmpDataClient = new DataClient();
                    string tmpSql=string.Format("select *  from tapctlinkage where GROUPNAME='{0}' and TAGETUINAME='{1}' and TAGETUI='{2}'",
                    args.GroupName, args.MessageName, args.Custom01);
                    DataTable dt1 = tmpDataClient.SelectData(tmpSql, "tapctlinkage").Tables[0];

                    if (dt1==null||dt1.Rows.Count==0)
                    {
                        bs.ExecuteModify("SaveUIGroup", args.getPack());
                    }
                }
                treeList2.DataSource = null;

                var listTe = GetBindSourceUI(_groupSelected);
                treeList2.DataSource = listTe;
                if (listTe != null)
                {
                    treeList2.Columns[0].Visible = false;
                    treeList2.Columns[1].Visible = false;
                    treeList2.Columns[2].Visible = false;
                    treeList2.Columns[3].Visible = false;
                    //treeList2.Columns[4].Visible = false;
                    treeList2.Columns[5].Visible = false;
                    treeList2.Columns[6].Visible = false;
                    treeList2.Columns[7].Visible = false;
                    treeList2.Columns[8].Visible = false;
                    treeList2.Columns[9].Visible = false;
                    treeList2.Columns[10].Visible = false;
                    treeList2.Columns[11].Visible = false;
                    treeList2.Columns[12].Visible = false;
                }
                treeList2.ExpandAll();
            }
        }

        private void tButton1_Click(object sender, EventArgs e)
        {
            listCID = new List<string>();
            if (treeList2.Nodes.Count > 0)
            {
                CommonArgsPack args = new CommonArgsPack();
                string name = GroupListtw.SelectedNode.Text;
                foreach (TreeListNode root in treeList2.Nodes)
                {
                    if (root.CheckState == CheckState.Checked)
                    {
                        var drRow = treeList2.GetDataRecordByNode(root);
                        if (drRow != null)
                        {
                            string selId = ((TAP.UI.LinkInfo)drRow).GROUPID.ToString();
                            listCID.Add(selId);
                        }
                    }
                }
                if (listCID.Count == 0)
                {
                    TAP.UI.TAPMsgBox.Instance.ShowMessage(Text, EnumMsgType.WARNING, "Please choose UserUI.");
                    return;
                }

                var list = listDataAll.Where(x => listCID.Contains(x.CID.ToString())).ToList();

                string tmpMessage = _translator.ConvertGeneralTemplate(EnumVerbs.UPDATE, EnumGeneralTemplateType.CONFIRM, "");
                DialogResult dialog = TAP.UI.TAPMsgBox.Instance.ShowMessage(Text, EnumMsgType.CONFIRM, tmpMessage);
                if (dialog.ToString() == "Yes")
                {
                    foreach (var item in listCID)
                    {
                        args.Name = item;
                        int DeleteCount = bs.ExecuteModify("DeleteLinkAge", args.getPack());
                    }

                }

                //var list2 = GetBindSourceUser(addname);


                treeList2.DataSource = null;

                var listTe = GetBindSourceUI(name);
                _groupSelected = name;
                treeList2.DataSource = listTe;
                if (listTe != null)
                {
                    treeList2.Columns[0].Visible = false;
                    treeList2.Columns[1].Visible = false;
                    treeList2.Columns[2].Visible = false;
                    treeList2.Columns[3].Visible = false;
                    //treeList2.Columns[4].Visible = false;
                    treeList2.Columns[5].Visible = false;
                    treeList2.Columns[6].Visible = false;
                    treeList2.Columns[7].Visible = false;
                    treeList2.Columns[8].Visible = false;
                    treeList2.Columns[9].Visible = false;
                    treeList2.Columns[10].Visible = false;
                    treeList2.Columns[11].Visible = false;
                    treeList2.Columns[12].Visible = false;
                }
                treeList2.ExpandAll();
            }
        }

        private void treeList2_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            DevExpress.XtraTreeList.TreeList treeList = sender as DevExpress.XtraTreeList.TreeList;
            if (treeList != null && treeList.Selection.Count == 1)
            {
                string groupid = treeList.FocusedNode.GetValue("GROUPID").ToString();

                FrmLinkageEdit frm = new FrmLinkageEdit(Convert.ToInt32(groupid));
                frm.ShowDialog();
                SetEdit(frm);
            }
        }

        private void btnUIGroupDel_Click(object sender, EventArgs e)
        {
            if (GroupListtw.SelectedNode == null)
            {
                TAP.UI.TAPMsgBox.Instance.ShowMessage(Text, EnumMsgType.WARNING, "Please choose userGroup.");
                return;
            }
            string tmpMessage = _translator.ConvertGeneralTemplate(EnumVerbs.DELETE, EnumGeneralTemplateType.CONFIRM, "");
            DialogResult dialog = TAP.UI.TAPMsgBox.Instance.ShowMessage(Text, EnumMsgType.CONFIRM, tmpMessage);
            if (dialog.ToString() == "Yes")
            {
                CommonArgsPack args = new CommonArgsPack();
                args.GroupName = GroupListtw.SelectedNode.Text;
                int DeleteCount = bs.ExecuteModify("DeleteLinkAge", args.getPack());
            }
            UIListData();
        }
    }
    public class treeListInfo
    {
        public string typ { get; set; }
        public string MAINMENU { get; set; }
        public string NAME { get; set; }

        public string SUBMENU { get; set; }

        public string UI { get; set; }
        public int CID { get; set; }
        public int PID { get; set; }
        /// <summary>
        /// 显示名字
        /// </summary>
        public string DISPLAYNAME { get; set; }
        /// <summary>
        /// DLL名字
        /// </summary>
        public string ASSEMBLYFILENAME { get; set; }
        public string GROUPID { get; set; }

    }

}
