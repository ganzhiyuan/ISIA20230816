using ISIA.INTERFACE.ARGUMENTSPACK;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using TAP;
using TAP.Data.Client;
using TAP.Fressage;
using TAP.Models.UIBasic;
using TAP.Models.User;
using TAP.UI;
using ISIA.UI.BASE;
using DevExpress.XtraTreeList;
using DevExpress.XtraTreeList.Nodes;
using DevExpress.XtraRichEdit.Model;
using ISIA.COMMON;

namespace ISIA.UI.ADMINISTRATOE
{
    public partial class FrmAuthorityManagement : UIBase
    {
        public FrmAuthorityManagement()
        {
            InitializeComponent();
            InitializeGroupList();
            InitializeUIGroup();
            bs = new BizDataClient("ISIA.BIZ.ADMINISTRATOE.DLL", "ISIA.BIZ.ADMINISTRATOE.AuthorityManagement");

            int temp = Convert.ToInt32(panelControl2.Height * 0.48);
            flowLayoutPanel2.Height = temp;
        }

        #region Feild     
        ComboBoxControl CBC = new ComboBoxControl();
        BizDataClient bs = null;
        CommonArgsPack args = new CommonArgsPack();
        //DataSet uids = new DataSet();
        DataSet checkuids = new DataSet();
        String addname = "";
        EnumAuthorityOwnerType addmembertype;
        List<Image> images = new List<Image>();
        String MDI = TAP.Base.Configuration.ConfigurationManager.Instance.AppSection.MDIName;

        Dictionary<string, List<string>> allgroupitems = new Dictionary<string, List<string>>();
        Dictionary<string, List<string>> alluiitems = new Dictionary<string, List<string>>();
        List<string> checkgroupitems = new List<string>();
        List<string> checkuiitems = new List<string>();
        string findgroupstr = "";
        string finduistr = "";
        int nowgroupindex = 0;
        int nowuiindex = 0;

        UserModel userModel = new UserModel();
        UserGroupModel userGroupModel = new UserGroupModel();
        GroupMemberModel groupMemberModel = new GroupMemberModel();
        ArgumentPack retVal = new ArgumentPack();
        UserDefaultInfo userDefaultInfo = new UserDefaultInfo();
        UIBasicModel uiBasicModel = new UIBasicModel();
        UIBasicDefaultInfo uiBasicDefaultInfo = new UIBasicDefaultInfo();
        UIAuthorityBasicModel uiAuthorityBasicModel = new UIAuthorityBasicModel();
        DataTable dtTemporary = new DataTable();
        string region = TAP.Base.Configuration.ConfigurationManager.Instance.EnvironmentSection.Region;

        List<treeListInfo> listDataAll = new List<treeListInfo>();

        List<string> listCID = new List<string>();
        TreeListHitInfo downHitInfo = null;
        #endregion

        #region Method

        private void InitializeGroupList()
        {
            DataTable dt=userGroupModel.LoadModelDataList(retVal);

            //string sql = string.Format("select NAME from TAPUTGROUP WHERE ISALIVE='YES'");
            //DataSet ds = DEC.SelectDataSet(sql, "");
            List<TreeNode> treeNodes = new List<TreeNode>();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                allgroupitems.Add(dt.Rows[i]["NAME"].ToString(), null);
                //TreeNode treeNode = new TreeNode(dt.Rows[i]["NAME"].ToString(), GetGroupMember(dt.Rows[i]["NAME"].ToString()));
                TreeNode treeNode = new TreeNode();
                if (dt.Rows[i]["NAME"].ToString() == "ADMIN")
                {
                    treeNode.ImageIndex = 1;
                }
                else if (dt.Rows[i]["NAME"].ToString() == "GUEST")
                {
                    treeNode.ImageIndex = 3;
                }
                else if (dt.Rows[i]["NAME"].ToString() == "MANAGER")
                {
                    treeNode.ImageIndex = 2;
                }
                else
                {
                    treeNode.ImageIndex = 4;
                }
                treeNode.Name = dt.Rows[i]["NAME"].ToString();
                treeNode.Text = dt.Rows[i]["NAME"].ToString();
                treeNodes.Add(treeNode);
            }
            TreeNode[] treeNodess = treeNodes.ToArray<TreeNode>();
            GroupListtw.Nodes.AddRange(treeNodess);         
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

            listDataAll= GetBindSource(dt);

            treeList1.KeyFieldName = "CID";
            treeList1.ParentFieldName = "PID";
            treeList1.DataSource = listDataAll;
            var a =DataTableExtend.ConvertToDataSet<treeListInfo>(listDataAll);
            if (listDataAll!=null)
            {
                treeList1.Columns[0].Visible = false;
                treeList1.Columns[1].Visible = false;
                treeList1.Columns[3].Visible = false;
                treeList1.Columns[4].Visible = false;
            }
            
            treeList1.ExpandAll();

            treeList2.KeyFieldName = "CID";
            treeList2.ParentFieldName = "PID";
            var listTe= GetBindSourceUser("");
            treeList2.DataSource = listTe;
            if (listTe!=null)
            {
                treeList2.Columns[0].Visible = false;
                treeList2.Columns[1].Visible = false;
                treeList2.Columns[3].Visible = false;
                treeList2.Columns[4].Visible = false;
            }
            treeList2.ExpandAll();
        }

        private List<treeListInfo> GetBindSourceUser(string groupID="ADMIN")
        {
            CommonArgsPack arguments = new CommonArgsPack();
            arguments.GroupName = groupID;
            DataSet UserUIList = bs.ExecuteDataSet("GetAuthority", arguments.getPack());
            List<treeListInfo> UserAllUI = DataTableExtend.GetList<treeListInfo>(UserUIList.Tables[0]);
            if (UserAllUI==null)
            {
                return null;
            }
            List<string> listTm = UserAllUI.Select(x => x.UI).ToList();
            List<treeListInfo> listP = listDataAll.Where(x => listTm.Contains(x.NAME)||x.UI!="0").ToList();

            var parentIDList = listP.Where(x => x.PID == 0).Select(x => x.CID).ToList();//一级的CID
           

            


            //var temp = listP.Where(x => !parentIDList.Contains(x.PID)&&x.UI=="0").ToList();//取出没有子集的二级菜单
            //foreach (var item in temp)
            //{
            //    listP.Remove(item);
            //}

            

            return listP;
        }

        private List<treeListInfo> GetBindSourceAll(List<treeListInfo> list,TreeList treeList)
        {

            treeList.DataSource = null;
            treeList.DataSource = listDataAll;
            treeList.ExpandAll();
            if (list==null)
            {
               
                return null;
            }
            List<string> listTm = list.Where(x => x.UI == "0").Select(x => x.NAME).ToList();
            foreach (TreeListNode node in treeList.Nodes)
            {
                foreach (TreeListNode item in node.Nodes)
                {
                    foreach (TreeListNode child in item.Nodes)
                    {
                        var drRow = treeList.GetDataRecordByNode(child);
                        if (drRow != null)
                        {
                            string menuName = ((ISIA.UI.ADMINISTRATOE.FrmAuthorityManagement.treeListInfo)drRow).NAME;
                            if (listTm.Contains(menuName))
                            {
                                child.CheckState = CheckState.Checked;
                                SetCheckedParentNodes(child, child.CheckState);
                            }

                        }
                    }
                    
                }
            }
            


            List<treeListInfo> listP = listDataAll.Where(x => !listTm.Contains(x.MAINMENU) || x.UI != "0").ToList();

            var parentIDList = listP.Where(x => x.PID == 0).Select(x=>x.CID).ToList();
            var temp = listP.Where(x => !parentIDList.Contains(x.PID)&&x.UI=="0").ToList();//取出没有子集的一级二级菜单
            //foreach (var item in temp)
            //{
            //    //listP.Remove(item);
            //}
            //foreach (var item in parentIDList)
            //{
            //    var parent2IDList = listP.Where(x => x.PID == item && x.UI != "0").Select(x => x.CID).ToList();//二级的CID
            //    foreach (var item2 in parent2IDList)//2级CID有没有PID决定是否有子集
            //    {
            //        var tempChild = listP.Where(x => parentIDList.Contains(x.PID) && x.UI == "0").ToList();//取出没有子集的二级菜单
            //        if (!tempChild.Any())
            //        {
            //            listP.Remove(listP.FirstOrDefault(x => x.CID == item2));
            //        }
            //    }
            //}
            return listP;
        }

        private List<treeListInfo> GetBindSource(DataTable dt)
        {
            //定义绑定LIST，下标值
            List<treeListInfo> listData = new List<treeListInfo>();
            int count = 1;

            //取出来一二级，一级MAINMENU 二级 NAME  三级（UI表  父级-SUBMENU 对应二级NAME字段）
            DataClient tmpDataClient = new DataClient();
            string tmpMainMenuSql = "SELECT * FROM tapstbmainmenu WHERE ISALIVE = 'YES'  ORDER BY SEQUENCES";
            DataTable retValMain = tmpDataClient.SelectData(tmpMainMenuSql, "SUBMENU").Tables[0];

            //string tmpSubMenuSql = "SELECT * FROM TAPSTBSUBMENU WHERE ISALIVE = 'YES'  ORDER BY SEQUENCES";
            //DataTable retVal1 = tmpDataClient.SelectData(tmpSubMenuSql, "SUBMENU").Tables[0];

            DataTable retVal1 = bs.ExecuteDataSet("GetSubMenu").Tables[0];
            List<treeListInfo> listMENU = DataTableExtend.GetList<treeListInfo>(retValMain);
            //取出一级名称
            List<string> listStr = listMENU.Select(x => x.NAME).Distinct().ToList();
            foreach (var item in listStr)
            {
                treeListInfo info = new treeListInfo();
                info.CID = count++;
                info.MAINMENU = item;
                info.NAME = item;
                info.typ = "0";
                listData.Add(info);
            }
            //根据一级取出二级名称加入list
            List<treeListInfo> listName = DataTableExtend.GetList<treeListInfo>(retVal1);
            foreach (var item in listName)
            {
                treeListInfo info = new treeListInfo();
                info.CID = count++;
                info.MAINMENU = item.MAINMENU;
                info.NAME = item.NAME;
                info.typ = "1";
                var pItem = listData.FirstOrDefault(x => x.NAME == item.MAINMENU);
                if (pItem!=null)
                {
                    info.PID = pItem.CID;
                }
                listData.Add(info);
            }
            //取出三级名称加入list
            List<treeListInfo> listChild = DataTableExtend.GetList<treeListInfo>(dt);
            foreach (var item in listChild)
            {
                treeListInfo info = new treeListInfo();
                info.CID = count++;
                info.MAINMENU = item.MAINMENU;
                info.SUBMENU = item.SUBMENU;
                info.NAME = item.NAME;
                info.UI = "0";
                info.typ = "2";
                var pItem = listData.FirstOrDefault(x => x.NAME == item.SUBMENU&&x.PID!=0);
                if (pItem != null)
                    info.PID = pItem.CID;
                listData.Add(info);
            }
            return listData;
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

        }


        private void BindSourceChange(TreeView treeView)
        {
            var list = GetBindSourceUser(treeView.SelectedNode.Text);
            treeList2.DataSource = list;
            if (list != null)
            {
                treeList2.Columns[0].Visible = false;
                treeList2.Columns[1].Visible = false;
                treeList2.Columns[3].Visible = false;
                treeList2.Columns[4].Visible = false;

            }
            treeList2.ExpandAll();
            GetBindSourceAll(list, treeList1);

        }


        //取消节点选中状态之后，取消所有父节点的选中状态
        private void setParentNodeCheckedState(TreeNode currNode, bool state)
        {
            TreeNode parentNode = currNode.Parent;
            parentNode.Checked = state;
            if (currNode.Parent.Parent != null)
            {
                setParentNodeCheckedState(currNode.Parent, state);
            }
        }

        //选中节点之后，选中节点的所有子节点
        private void setChildNodeCheckedState(TreeNode currNode, bool state)
        {
            TreeNodeCollection nodes = currNode.Nodes;
            if (nodes.Count > 0)
            {
                foreach (TreeNode tn in nodes)
                {
                    tn.Checked = state;
                    setChildNodeCheckedState(tn, state);
                }
            }
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
                        int selId = ((ISIA.UI.ADMINISTRATOE.FrmAuthorityManagement.treeListInfo)drRow).CID;
                        listCID.Add(selId.ToString());
                    }
                }
                GetCheckNodeID(item, treeList);
            }
        }

        #endregion

        #region Event
        private void GroupListtw_MouseDown(object sender, MouseEventArgs e)
        {
            if ((sender as TreeView) != null)
            {
                addmembertype = EnumAuthorityOwnerType.USERGROUP;
                TreeView treeView = (TreeView)sender;
                treeView.SelectedNode = treeView.GetNodeAt(e.X, e.Y);
                if (treeView.SelectedNode == null)
                {
                    treeList2.DataSource = null;

                    GetBindSourceAll(null, treeList1);
                    return;
                }

                BindSourceChange(treeView);
            }
        }




        #region 拖拽-暂时不用
        private void treeList1_MouseMove(object sender, MouseEventArgs e)
        {
            //TreeList treelist = sender as TreeList;
            //if (e.Button == MouseButtons.Left && downHitInfo != null)
            //{
            //    if (treeList1.Selection.Count == 0)
            //        return;
            //    Size dragSize = SystemInformation.DragSize;
            //    Rectangle dragRect = new Rectangle(new Point(downHitInfo.MousePoint.X - dragSize.Width / 2,
            //        downHitInfo.MousePoint.Y - dragSize.Height /2 ), dragSize);

            //    if (!dragRect.Contains(new Point(e.X, e.Y)))
            //    {
            //        List<TreeListNode> node = new List<TreeListNode>();
            //        foreach (TreeListNode n in treeList1.Selection)
            //        {
            //            node.Add(n);
            //        }
            //        treelist.DoDragDrop(downHitInfo.Node, DragDropEffects.Move);
            //        downHitInfo = null;
            //        DevExpress.Utils.DXMouseEventArgs.GetMouseArgs(e).Handled = true;
            //    }
            //}
        }

        private void treeList1_MouseDown(object sender, MouseEventArgs e)
        {
            //TreeList treelist = sender as TreeList;
            //downHitInfo = null;
            //TreeListHitInfo hitInfo = treelist.CalcHitInfo(new Point(e.X, e.Y));

            //if (Control.ModifierKeys != Keys.None) return;
            //if (e.Button == MouseButtons.Left)
            //{
            //    downHitInfo = hitInfo;
            //}
        }

        private void treeList2_DragOver(object sender, DragEventArgs e)
        {
            //TreeList treelist = sender as TreeList;
            //if (treelist != null)
            //{
            //    e.Effect = DragDropEffects.Move;
            //}
        }

        private void treeList2_DragEnter(object sender, DragEventArgs e)
        {
            //e.Effect = DragDropEffects.Move;
        }

        private void treeList2_DragDrop(object sender, DragEventArgs e)
        {
            //List<TreeListNode> nodes = e.Data.GetData(typeof(List<TreeListNode>)) as List<TreeListNode>;
            //TreeList grid = sender as TreeList;
            //DataTable table = grid.DataSource as DataTable;

            //if (nodes != null && nodes.Count >0  && table != null)
            //{
            //    foreach (TreeListNode node in nodes)
            //    {
            //        treeList1.Nodes.Add(node);
            //    }
            //}
        }

        #endregion

        private void treeList1_AfterCheckNode(object sender, NodeEventArgs e)
        {
            SetCheckedChildNodes(e.Node, e.Node.CheckState);
            SetCheckedParentNodes(e.Node, e.Node.CheckState);
        }

        private void treeList1_BeforeCheckNode(object sender, CheckNodeEventArgs e)
        {
            e.State = (e.PrevState == CheckState.Checked ? CheckState.Unchecked : CheckState.Checked);
        }

        private void tButton2_Click(object sender, EventArgs e)
        {
            if (GroupListtw.SelectedNode==null)
            {
                TAP.UI.TAPMsgBox.Instance.ShowMessage(Text, EnumMsgType.WARNING, "Please choose userGroup.");
                return;
            }
            listCID = new List<string>();
            if (treeList1.Nodes.Count>0)
            {
                string name = GroupListtw.SelectedNode.Text;
                addname = name;
                foreach (TreeListNode root in treeList1.Nodes)
                {
                    GetCheckNodeID(root,treeList1);
                }
                if (listCID.Count==0)
                {
                    TAP.UI.TAPMsgBox.Instance.ShowMessage(Text, EnumMsgType.WARNING, "Please choose UI.");
                    return;
                }
                var list = listDataAll.Where(x => listCID.Contains(x.CID.ToString())&&x.UI=="0").ToList();

                DataTable dtUI = new DataTable();
                dtUI.Columns.Add("MAINMENU", typeof(string));
                dtUI.Columns.Add("NAME", typeof(string));
                dtUI.Columns.Add("UI", typeof(string));
                foreach (var item in list)
                {
                    dtUI.Rows.Add(item.MAINMENU, item.SUBMENU, item.NAME);
                }
                string tmpMessage = _translator.ConvertGeneralTemplate(EnumVerbs.UPDATE, EnumGeneralTemplateType.CONFIRM, "");
                DialogResult dialog = TAP.UI.TAPMsgBox.Instance.ShowMessage(Text, EnumMsgType.CONFIRM, tmpMessage);
                if (dialog.ToString() == "Yes")
                {
                    args.Name = addname;
                    args.MemberType = addmembertype;
                    args.MDI = MDI;

                    int DeleteCount = bs.ExecuteModify("DeleteAuthority", args.getPack());

                    //DEC.ModifyData(string.Format("DELETE FROM TAPSTBUIAUTHORITY WHERE NAME ='{0}' AND MEMBERTYPE='{1}' AND MDI='{2}'AND ISALIVE='YES'", addname, addmembertype, MDI));
                    int SaveCount = 0;


                    args.Dt = dtUI;
                    args.UserID = addname;
                    args.MDI = MDI;
                    args.Region = region;
                    args.MemberType = addmembertype;
                    args.InsertUser = InfoBase._USER_INFO.UserName;
                    SaveCount = bs.ExecuteModify("SaveAuthority", args.getPack());

                    args.Facility = "T1";
                    int UpdateCount = bs.ExecuteModify("UpdateAuthority", args.getPack());

                    if (SaveCount > 0)
                    {
                        TAPMsgBox.Instance.ShowMessage(this.Text, EnumMsgType.INFORMATION, "Update completed.");
                    }
                    var listTe = GetBindSourceUser(addname);
                    treeList2.DataSource = listTe;
                    if (listTe != null)
                    {
                        treeList2.Columns[0].Visible = false;
                        treeList2.Columns[1].Visible = false;
                        treeList2.Columns[3].Visible = false;
                        treeList2.Columns[4].Visible = false;
                    }
                    treeList2.ExpandAll();
                }
                else
                {
                    return;
                }
            }
        }


        private void tButton1_Click(object sender, EventArgs e)
        {
            listCID = new List<string>();
            if (treeList2.Nodes.Count > 0)
            {
                string name = GroupListtw.SelectedNode.Text;
                addname = name;
                foreach (TreeListNode root in treeList2.Nodes)
                {
                    GetCheckNodeID(root, treeList2);
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
                    foreach (var item in list)
                    {
                        args.Name = addname;
                        args.MemberType = addmembertype;
                        args.MDI = MDI;
                        args.Custom01 = item.NAME;
                        int DeleteCount = bs.ExecuteModify("DeleteAuthority", args.getPack());
                    }
                    
                }
                //GroupListtw.MouseDown+= GroupListtw_MouseDown;

                var list2 = GetBindSourceUser(addname);


                treeList2.DataSource = null;
                treeList2.DataSource = list2;
                if (list2 != null)
                {
                    treeList2.Columns[0].Visible = false;
                    treeList2.Columns[1].Visible = false;
                    treeList2.Columns[3].Visible = false;
                    treeList2.Columns[4].Visible = false;

                }
                treeList2.ExpandAll();
                GetBindSourceAll(list2, treeList1);
            }
            
        }
        private void treeList2_AfterCheckNode(object sender, NodeEventArgs e)
        {

            SetCheckedChildNodes(e.Node, e.Node.CheckState);
            SetCheckedParentNodes(e.Node, e.Node.CheckState);
        }

        private void treeList2_BeforeCheckNode(object sender, CheckNodeEventArgs e)
        {

            e.State = (e.PrevState == CheckState.Checked ? CheckState.Unchecked : CheckState.Checked);
        }

        private void treeList1_CustomDrawNodeImages(object sender, CustomDrawNodeImagesEventArgs e)
        {
            if (e.Node.GetValue("typ") != null)
                e.SelectImageIndex = int.Parse(e.Node.GetValue("typ").ToString());
            if (e.Node.HasChildren)
                if (e.Node.Expanded)
                    e.SelectImageIndex = 3;
        }

        private void treeList2_CustomDrawNodeImages(object sender, CustomDrawNodeImagesEventArgs e)
        {
            if (e.Node.GetValue("typ") != null)
                e.SelectImageIndex = int.Parse(e.Node.GetValue("typ").ToString());

            if (e.Node.HasChildren)
                if (e.Node.Expanded)
                    e.SelectImageIndex = 3;
        }



        #endregion
    }
}
