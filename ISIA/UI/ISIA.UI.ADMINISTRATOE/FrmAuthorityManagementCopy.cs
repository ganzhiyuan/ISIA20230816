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

namespace ISIA.UI.ADMINISTRATOE
{
    public partial class FrmAuthorityManagementCopy : UIBase
    {
        public FrmAuthorityManagementCopy()
        {
            InitializeComponent();
            InitializeGroupList();
            InitializeUIGroup();
            bs = new BizDataClient("ISIA.BIZ.ADMINISTRATOE.DLL", "ISIA.BIZ.ADMINISTRATOE.AuthorityManagement");
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
                TreeNode treeNode = new TreeNode(dt.Rows[i]["NAME"].ToString(), GetGroupMember(dt.Rows[i]["NAME"].ToString()));
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

        private TreeNode[] GetGroupMember(string groupname)
        {            
            DataTable dt=groupMemberModel.LoadModelDataList(retVal);
            DataRow[] drs = dt.Select("USERGROUP= '"+groupname+"'");

            DataTable dtNew = dt.Clone();
            foreach (DataRow dr in drs)
            {
                dtNew.ImportRow(dr);
            }
            
            //string sql = string.Format("select NAME from TAPUTGROUPMEMBER where USERGROUP='{0}' AND ISALIVE='YES'", groupname);
            //DataSet ds = DEC.SelectDataSet(sql, "");
            List<TreeNode> treeNodes = new List<TreeNode>();
            List<string> itmenames = new List<string>();
            for (int i = 0; i < dtNew.Rows.Count; i++)
            {
                itmenames.Add(dtNew.Rows[i]["NAME"].ToString());
                TreeNode treeNode = new TreeNode(dtNew.Rows[i]["NAME"].ToString());
                treeNode.Name = dtNew.Rows[i]["NAME"].ToString();
                treeNode.Text = dtNew.Rows[i]["NAME"].ToString();
                treeNode.ImageIndex = 0;
                treeNodes.Add(treeNode);
            }
            allgroupitems[groupname] = itmenames;
            TreeNode[] treeNodess = treeNodes.ToArray<TreeNode>();
            return treeNodess;
        }

        private void InitializeUIGroup()
        {
            DataTable dt = uiBasicModel.LoadModelDataList(retVal);
            DataRow[] drs = dt.Select("MDI='" + MDI + "'");
            DataTable dtNew = dt.Clone();
            foreach (DataRow dr in drs)
            {
                dtNew.ImportRow(dr);
            }

            DataView dv = dtNew.DefaultView;
            dtTemporary = dv.ToTable(true, "MAINMENU");

            //string sql = string.Format("select distinct(MAINMENU) MAINMENU from TAPSTBUI  WHERE MDI='{0}'AND ISALIVE='YES' ", MDI);
            //DataSet ds = DEC.SelectDataSet(sql, "");
            //uids = ds;
            List<TreeNode> treeNodes = new List<TreeNode>();

            for (int i = 0; i < dtTemporary.Rows.Count; i++)
            {
                alluiitems.Add(dtTemporary.Rows[i]["MAINMENU"].ToString(), null);
                TreeNode treeNode = new TreeNode(dtTemporary.Rows[i]["MAINMENU"].ToString(), GetUIGroupName(dtTemporary.Rows[i]["MAINMENU"].ToString()));
                treeNode.Name = dtTemporary.Rows[i]["MAINMENU"].ToString();
                treeNode.Text = dtTemporary.Rows[i]["MAINMENU"].ToString();
                treeNode.Tag = dtTemporary.Rows[i]["MAINMENU"].ToString();

                treeNodes.Add(treeNode);
            }
            TreeNode[] treeNodess = treeNodes.ToArray<TreeNode>();

            UiGrouptw.Nodes.AddRange(treeNodess);
            UiGrouptw.CheckBoxes = true;


            List<treeListInfo> listData = new List<treeListInfo>();
            //先取出来父级加入集合
            List<treeListInfo> listP = GetList<treeListInfo>(dtTemporary);
            for (int i = 0; i < listP.Count; i++)
            {
                listP[i].NAME = listP[i].MAINMENU;
                listP[i].CID = i+1;
                listData.Add(listP[i]);
            }

            List<treeListInfo> list = GetList<treeListInfo>(dt);

            int m = listP.Count+1;
            foreach (var item in listP)
            {
                var itemList = list.Where(x => x.MAINMENU == item.MAINMENU).ToList();
                foreach (var ite in itemList)
                {
                    ite.CID = m++;
                    ite.PID = item.CID;
                    listData.Add(ite);
                }
            }
            treeList1.KeyFieldName = "CID";
            treeList1.ParentFieldName = "PID";
            treeList1.DataSource = listData;
            treeList1.Columns[0].Visible = false;
            treeList1.ExpandAll();
        }

        public class treeListInfo
        {
            public string MAINMENU { get; set; }
            public string NAME { get; set; }

            //public string SUBMENU { get; set; }

            public int CID { get; set; }
            public int PID { get; set; }

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
            return list.Count ==0  ? null : list;
        }
        public DataSet ConvertToDataSet<T>(IList<T> list)
        {
            if (list == null || list.Count <= 0)
            {
                return null;
            }

            DataSet ds = new DataSet();
            DataTable dt = new DataTable(typeof(T).Name);
            DataColumn column;
            DataRow row;

            System.Reflection.PropertyInfo[] myPropertyInfo = typeof(T).GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);

            foreach (T t in list)
            {
                if (t == null)
                {
                    continue;
                }

                row = dt.NewRow();

                for (int i = 0, j = myPropertyInfo.Length; i < j; i++)
                {
                    System.Reflection.PropertyInfo pi = myPropertyInfo[i];

                    string name = pi.Name;

                    if (dt.Columns[name] == null)
                    {
                        column = new DataColumn(name, pi.PropertyType);
                        dt.Columns.Add(column);
                    }

                    row[name] = pi.GetValue(t, null);
                }

                dt.Rows.Add(row);
            }

            ds.Tables.Add(dt);

            return ds;
        }
        private TreeNode[] GetUIGroupName(string mainmenuname)
        {
            DataTable dt = uiBasicModel.LoadModelDataList(retVal);

            DataRow[] drs = dt.Select("MDI='" + MDI + "' AND MAINMENU='"+ mainmenuname + "'");
            DataTable dtNew = dt.Clone();
            foreach (DataRow dr in drs)
            {
                dtNew.ImportRow(dr);
            }

            //string sql = string.Format("select distinct(DISPLAYNAME) DISPLAYNAME from TAPSTBUI where MAINMENU='{0}'AND MDI='{1}' AND ISALIVE='YES'", mainenuname, MDI);
            //DataSet ds = DEC.SelectDataSet(sql, "");
            List<TreeNode> treeNodes = new List<TreeNode>();
            List<string> itemnames = new List<string>();

            for (int i = 0; i < dtNew.Rows.Count; i++)
            {
                itemnames.Add(dtNew.Rows[i]["DISPLAYNAME"].ToString());
                TreeNode treeNode = new TreeNode(dtNew.Rows[i]["DISPLAYNAME"].ToString());
                treeNode.Name = dtNew.Rows[i]["DISPLAYNAME"].ToString();
                treeNode.Text = dtNew.Rows[i]["DISPLAYNAME"].ToString();
                treeNodes.Add(treeNode);
            }
            alluiitems[mainmenuname] = itemnames;

            TreeNode[] treeNodess = treeNodes.ToArray<TreeNode>();
            return treeNodess;
        }

        private void Information(string name)
        {
            ArgumentPack tempPack = new ArgumentPack();

            tempPack.AddArgument("NAME", typeof(string), name);

            DataTable dt = userModel.LoadModelDataList(tempPack);
            
            //string sql = String.Format("select * from TAPUTUSERS where NAME='{0}'", name);
            //DataSet ds = DEC.SelectDataSet(sql, "");
            //DataTable dt = ds.Tables[0];
            if (dt.Rows.Count>0) {
                txtDEPARTMENT.Text = dt.Rows[0]["DEPARTMENT"].ToString().Trim();
                txtPOSITION.Text = dt.Rows[0]["POSITION"].ToString().Trim();
                txtUSERNAME.Text = dt.Rows[0]["USERNAME"].ToString().Trim();
                txtCONTACTNO.Text = dt.Rows[0]["CONTACTNO"].ToString().Trim();
                txtMOBILENO.Text = dt.Rows[0]["MOBILENO"].ToString().Trim();
                txtMAILADDRESS.Text = dt.Rows[0]["MAILADDRESS"].ToString().Trim();
                txtDESCRIPTION.Text = dt.Rows[0]["DESCRIPTION"].ToString().Trim();
            }
            else
            {
                txtDEPARTMENT.Text = "";
                txtPOSITION.Text = "";
                txtUSERNAME.Text = "";
                txtCONTACTNO.Text = "";
                txtMOBILENO.Text = "";
                txtMAILADDRESS.Text = "";
                txtDESCRIPTION.Text = "";
            }
        }

        private void ShowDescription(string item)
        {
            txtDESCRIPTION.Text = "DESCRIPTION: ";

            DataTable dt = userGroupModel.LoadModelDataList(retVal);
            DataRow[] drs = dt.Select("NAME='" + item + "'");
            DataTable dtNew = dt.Clone();
            foreach (DataRow dr in drs)
            {
                dtNew.ImportRow(dr);
            }

            //string sql = string.Format("select DESCRIPTION from TAPUTGROUP where NAME ='{0}' ", item);
            //DataSet ds = DEC.SelectDataSet(sql, "");
            
            txtDESCRIPTION.Text = dtNew.Rows[0]["DESCRIPTION"].ToString();
            txtDEPARTMENT.Text = "";
            txtPOSITION.Text = "";
            txtUSERNAME.Text = "";
            txtCONTACTNO.Text = "";
            txtMOBILENO.Text = "";
            txtMAILADDRESS.Text = "";
        }

        public void SetCheckGroupItems(string strgroup)
        {
            for (int i = 0; i < allgroupitems.Count; i++)
            {
                var element = allgroupitems.ElementAt(i);
                string Key = element.Key;
                if (Key.ToUpper().Contains(strgroup.ToUpper()))
                {
                    checkgroupitems.Add(Key);
                }
                List<string> Value = element.Value;
                for (int j = 0; j < Value.Count; j++)
                {
                    if (Value[j].ToUpper().Contains(strgroup.ToUpper()))
                    {
                        checkgroupitems.Add(Key + "-" + Value[j]);
                    }
                }
            }
        }

        public void SetFindCheckGroup(int index)
        {
            string value = checkgroupitems[index];
            if (value.Contains("-"))
            {
                string[] str = value.Split('-');
                foreach (TreeNode node in GroupListtw.Nodes)
                {
                    if (node.Name.ToUpper() == str[0].ToUpper())
                    {
                        foreach (TreeNode node1 in node.Nodes)
                        {
                            if (node1.Name.ToUpper() == str[1].ToUpper())
                            {
                                GroupListtw.SelectedNode = node1;//选中
                                node1.Expand();
                            }
                        }
                    }
                }
            }
            else
            {
                foreach (TreeNode node in GroupListtw.Nodes)
                {
                    if (node.Name.ToUpper() == value.ToUpper())
                    {
                        GroupListtw.SelectedNode = node;//选中
                        node.Expand();
                    }
                }
            }
        }

        public void SetCheckUIItems(string strui)
        {
            for (int i = 0; i < alluiitems.Count; i++)
            {
                var element = alluiitems.ElementAt(i);
                string Key = element.Key;
                if (Key.ToUpper().Contains(strui.ToUpper()))
                {
                    checkuiitems.Add(Key);
                }
                List<string> Value = element.Value;
                for (int j = 0; j < Value.Count; j++)
                {
                    if (Value[j].ToUpper().Contains(strui.ToUpper()))
                    {
                        checkuiitems.Add(Key + "-" + Value[j]);
                    }
                }
            }
        }

        public void SetFindCheckUI(int index)
        {
            string value = checkuiitems[index];
            if (value.Contains("-"))
            {
                string[] str = value.Split('-');
                foreach (TreeNode node in UiGrouptw.Nodes)
                {
                    if (node.Name.ToUpper() == str[0].ToUpper())
                    {
                        foreach (TreeNode node1 in node.Nodes)
                        {
                            if (node1.Name.ToUpper() == str[1].ToUpper())
                            {
                                UiGrouptw.SelectedNode = node1;//选中
                                node1.Expand();
                            }
                        }
                    }
                }
            }
            else
            {
                foreach (TreeNode node in UiGrouptw.Nodes)
                {
                    if (node.Name.ToUpper() == value.ToUpper())
                    {
                        UiGrouptw.SelectedNode = node;//选中
                        node.Expand();
                    }
                }
            }
        }
        #endregion

        #region Event
        private void GroupListtw_MouseDown(object sender, MouseEventArgs e)
        {
            if ((sender as TreeView) != null)
            {
                TreeView treeView = (TreeView)sender;
                treeView.SelectedNode = treeView.GetNodeAt(e.X, e.Y);
                if (treeView.SelectedNode == null)
                {
                    return;
                }
                foreach (TreeNode node in UiGrouptw.Nodes)
                {
                    //取消节点选中状态之后，取消该节点所有子节点选中状态
                    setChildNodeCheckedState(node, false);
                    node.Checked = false;
                    //如果节点存在父节点，取消父节点的选中状态
                    if (node.Parent != null)
                    {
                        setParentNodeCheckedState(node, false);
                    }
                }

                DataSet dsUIName = bs.ExecuteDataSet("GetUIName");

                if (treeView.SelectedNode.Parent != null)
                {
                    string name = treeView.SelectedNode.Text;
                    addmembertype =EnumAuthorityOwnerType.USER ;
                    addname = name;

                    DataTable dt = uiAuthorityBasicModel.LoadModelDataList(retVal);
                    DataRow[] drs = dt.Select("MEMBERTYPE='USER' AND NAME='"+ name + "' AND MDI='"+ MDI + "'");

                    DataTable dtNew = dt.Clone();
                    foreach (DataRow dr in drs)
                    {
                        dtNew.ImportRow(dr);
                    }

                    //string sql = string.Format("select distinct(MAINMENU) MAINMENU from TAPSTBUIAUTHORITY t where MEMBERTYPE='USER' and NAME='{0}'AND MDI='{1}'AND ISALIVE='YES'", name, MDI);
                    //DataSet ds = DEC.SelectDataSet(sql, "");

                    List<int> uiindexs = new List<int>();

                    foreach (TreeNode node in UiGrouptw.Nodes)
                    {
                        for (int j = 0; j < dtNew.Rows.Count; j++)
                        {
                            //比较父级节点
                            if (dtNew.Rows[j]["MAINMENU"].ToString().Replace(" ", "").ToUpper() == node.Name.ToString().Replace(" ", "").ToUpper())
                            {
                                foreach (TreeNode node1 in node.Nodes)
                                {
                                    //比较子级节点                                   
                                    DataRow[] drs1 = dt.Select("MEMBERTYPE='"+ addmembertype + "' AND NAME='" + name + "' AND MDI='" + MDI + "' AND MAINMENU='"+ dtNew.Rows[j]["MAINMENU"].ToString() + "'");

                                    DataTable dtNew1 = dt.Clone();
                                    foreach (DataRow dr in drs1)
                                    {
                                        dtNew1.ImportRow(dr);
                                    }

                                    //string allchildnode = string.Format("select distinct(UI) UI from TAPSTBUIAUTHORITY where MAINMENU='{0}' and MEMBERTYPE='{1}' and NAME='{2}'AND MDI='{3}' AND ISALIVE='YES'", ds.Tables[0].Rows[j]["MAINMENU"].ToString(), addmembertype, name, MDI);
                                    //DataSet allchildnodeds = DEC.SelectDataSet(allchildnode, "");
                                    for (int o = 0; o < dtNew1.Rows.Count; o++)
                                    {
                                        DataTable dtUiName = new DataTable();
                                        string sqlWhere = " NAME IN('" + dtNew1.Rows[o]["UI"].ToString() + "')";
                                        DataRow[] drs2 = dsUIName.Tables[0].Select(sqlWhere);
                                        dtUiName = dsUIName.Tables[0].Clone();
                                        foreach (DataRow row in drs2)
                                        {
                                            dtUiName.ImportRow(row);
                                        }

                                        if (dtUiName.Rows.Count == 0 || dtUiName == null) return;

                                        if (node1.Name.ToString().Replace(" ", "").ToUpper() == dtUiName.Rows[0]["DISPLAYNAME"].ToString().Replace(" ", "").ToUpper())
                                        {
                                            node1.Checked = true;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    Information(addname);
                }
                else
                {
                    {
                        string name = treeView.SelectedNode.Text;
                        addname = name;
                        addmembertype = EnumAuthorityOwnerType.USERGROUP;

                        DataTable dt = uiAuthorityBasicModel.LoadModelDataList(retVal);
                        DataRow[] drs = dt.Select("MEMBERTYPE='USERGROUP' AND NAME='" + name + "' AND MDI='" + MDI + "'");

                        DataTable dtNew = dt.Clone();
                        foreach (DataRow dr in drs)
                        {
                            dtNew.ImportRow(dr);
                        }

                        //string sql = string.Format("select distinct(MAINMENU) MAINMENU from TAPSTBUIAUTHORITY t where MEMBERTYPE='USERGROUP' and NAME='{0}'AND MDI='{1}'AND ISALIVE='YES'", name, MDI);
                        //DataSet ds = DEC.SelectDataSet(sql, "");

                        List<int> uiindexs = new List<int>();

                        foreach (TreeNode node in UiGrouptw.Nodes)
                        {
                            for (int j = 0; j < dtNew.Rows.Count; j++)
                            {
                                //比较父级节点
                                if (dtNew.Rows[j]["MAINMENU"].ToString().Replace(" ", "").ToUpper() == node.Name.ToString().Replace(" ", "").ToUpper())
                                {
                                    foreach (TreeNode node1 in node.Nodes)
                                    {
                                        //比较子级节点
                                        DataRow[] drs1 = dt.Select("MEMBERTYPE='" + addmembertype + "' AND NAME='" + name + "' AND MDI='" + MDI + "' AND MAINMENU='" + dtNew.Rows[j]["MAINMENU"].ToString() + "'");

                                        DataTable dtNew1 = dt.Clone();
                                        foreach (DataRow dr in drs1)
                                        {
                                            dtNew1.ImportRow(dr);
                                        }

                                        //string allchildnode = string.Format("select DISTINCT(UI) UI  from TAPSTBUIAUTHORITY where MAINMENU='{0}'and  NAME='{1}' and MEMBERTYPE='{2}' AND MDI='{3}' AND ISALIVE='YES'", ds.Tables[0].Rows[j]["MAINMENU"].ToString(), name, addmembertype, MDI);
                                        //DataSet allchildnodeds = DEC.SelectDataSet(allchildnode, "");
                                        for (int o = 0; o < dtNew1.Rows.Count; o++)
                                        {
                                            DataTable dtUiName = new DataTable();
                                            string sqlWhere = " NAME IN('" + dtNew1.Rows[o]["UI"].ToString() + "')";
                                            DataRow[] drs2 = dsUIName.Tables[0].Select(sqlWhere);
                                            dtUiName = dsUIName.Tables[0].Clone();
                                            foreach (DataRow row in drs2)
                                            {
                                                dtUiName.ImportRow(row);
                                            }

                                            if (dtUiName.Rows.Count == 0 || dtUiName == null) return;

                                            if (node1.Name.ToString().Replace(" ", "").ToUpper() == dtUiName.Rows[0]["DISPLAYNAME"].ToString().Replace(" ", "").ToUpper())
                                            {
                                                node1.Checked = true;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    ShowDescription(addname);
                }
            }
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

        private void UiGrouptw_AfterCheck(object sender, TreeViewEventArgs e)
        {
            if (e.Action == TreeViewAction.ByMouse)
            {
                if (e.Node.Checked == true)
                {
                    //选中节点之后，选中该节点所有的子节点
                    setChildNodeCheckedState(e.Node, true);
                }
                else if (e.Node.Checked == false)
                {
                    //取消节点选中状态之后，取消该节点所有子节点选中状态
                    setChildNodeCheckedState(e.Node, false);
                    //如果节点存在父节点，取消父节点的选中状态
                    if (e.Node.Parent != null)
                    {
                        setParentNodeCheckedState(e.Node, false);
                    }
                }
            }
            if (e.Node != null)
            {
                if (e.Node.Parent != null)
                {
                    if (e.Node.Parent.Nodes.Count != 0)
                    {
                        int a = 0;
                        foreach (TreeNode node1 in e.Node.Parent.Nodes)
                        {
                            if (node1.Checked == true)
                            {
                                a += 1;
                            }
                        }
                        if (a == e.Node.Parent.Nodes.Count)
                        {
                            setParentNodeCheckedState(e.Node, true);
                        }
                    }
                }
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            Dictionary<string, List<string>> myDictionary = new Dictionary<string, List<string>>();

            //获取选中的项
            foreach (TreeNode node in UiGrouptw.Nodes)
            {
                //父级
                if (node.Checked == true)
                {
                    if (node.Nodes.Count > 0)
                    {
                        List<string> vs = new List<string>();
                        foreach (TreeNode node1 in node.Nodes)
                        {
                            vs.Add(node1.Name);
                        }
                        myDictionary.Add(node.Name, vs);
                    }
                }
                else
                {
                    if (node.Nodes.Count > 0)
                    {
                        List<string> vs = new List<string>();
                        foreach (TreeNode node1 in node.Nodes)
                        {
                            if (node1.Checked == true)
                            {
                                vs.Add(node1.Name);
                            }
                        }
                        if (vs.Count != 0)
                        {
                            myDictionary.Add(node.Name, vs);
                        }
                    }
                }
            }

            if (addname == "" || addname == null)
            {
                TAPMsgBox.Instance.ShowMessage(this.Text, EnumMsgType.CONFIRM, "Please choose Group List..");
                return;
            }

            List<string> updatesql = new List<string>();
            
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
                DataTable dtUI = new DataTable();
                dtUI.Columns.Add("MAINMENU",typeof(string));
                dtUI.Columns.Add("UI", typeof(string));
                DataSet dsUIName = bs.ExecuteDataSet("GetUIName");

                foreach (KeyValuePair<string, List<string>> item in myDictionary)
                {
                    for (int i = 0; i < item.Value.Count; i++)
                    {
                        Console.WriteLine(item.Key + "-----" + item.Value[i]);
                        try
                        {
                            DataTable dtUiName = new DataTable();
                            string sqlWhere = " DISPLAYNAME IN('" + item.Value[i] + "')";
                            DataRow[] drs = dsUIName.Tables[0].Select(sqlWhere);
                            dtUiName = dsUIName.Tables[0].Clone();
                            foreach (DataRow row in drs)
                            {
                                dtUiName.ImportRow(row);
                            }

                            dtUI.Rows.Add(item.Key, dtUiName.Rows[0]["NAME"].ToString());
                            
                            //uiBasicDefaultInfo.MDI = MDI;
                            //uiBasicDefaultInfo.MainMenu = item.Key;
                            //uiBasicDefaultInfo.UI = item.Value[i].Replace(" ", "");
                            //uiBasicDefaultInfo.Container = "BASED";
                            //uiBasicDefaultInfo.UIFunction = "SAVE";
                            //uiBasicDefaultInfo.UIAuthority = addname;

                            //uiBasicDefaultInfo.Region = region;
                            //uiBasicDefaultInfo.Facility = "T1";

                            //uiAuthorityBasicModel = new UIAuthorityBasicModel(uiBasicDefaultInfo);
                            
                            //uiAuthorityBasicModel.CommandType = EnumCommandType.COMMAND;
                            //uiAuthorityBasicModel.MemberType = addmembertype;
                            //uiAuthorityBasicModel.InsertUser = InfoBase._USER_INFO.UserName;
                           
                            //SaveCount =uiAuthorityBasicModel.Save(InfoBase._USER_INFO.UserName);                            
                        }
                        catch (System.Exception ex)
                        {
                            TAP.UI.TAPMsgBox.Instance.ShowMessage(this.Text, EnumMsgType.ERROR, ex.ToString());
                        }
                    }
                }

                args.Dt = dtUI;
                args.UserID = addname;
                args.MDI = MDI;
                args.Region = region;
                args.MemberType = addmembertype;
                args.InsertUser= InfoBase._USER_INFO.UserName;
                SaveCount = bs.ExecuteModify("SaveAuthority", args.getPack());

                args.Facility = "ALL";
                int UpdateCount = bs.ExecuteModify("UpdateAuthority", args.getPack());

                if (SaveCount > 0)
                {
                    TAPMsgBox.Instance.ShowMessage(this.Text, EnumMsgType.INFORMATION, "Update completed.");
                }
            }
            else
            {
                return;
            }
        }

        //Group Next
        private void btnGroupNext_Click(object sender, EventArgs e)
        {
            string strgroup = txtGroup.Text;
            if (findgroupstr.ToUpper() != strgroup.ToUpper())
            {
                checkgroupitems = new List<string>();
                findgroupstr = strgroup;
                SetCheckGroupItems(strgroup);
                nowgroupindex = 0;
            }
            else
            {
                nowgroupindex = nowgroupindex + 1;
                if (nowgroupindex >= checkgroupitems.Count)
                {
                    nowgroupindex = 0;
                }
            }
            if (checkgroupitems.Count < 1)
            {
                return;
            }
            if (nowgroupindex < checkgroupitems.Count)
            {
                SetFindCheckGroup(nowgroupindex);
            }
        }

        //UI next
        private void btnUINext_Click(object sender, EventArgs e)
        {
            string strui = txtUI.Text;
            if (finduistr.ToUpper() != strui.ToUpper())
            {
                checkuiitems = new List<string>();
                finduistr = strui;
                SetCheckUIItems(strui);
                nowuiindex = 0;
            }
            else
            {
                nowuiindex = nowuiindex + 1;
                if (nowuiindex >= checkuiitems.Count)
                {
                    nowuiindex = 0;
                }
            }
            if (checkuiitems.Count < 1)
            {
                return;
            }
            if (nowuiindex < checkuiitems.Count)
            {
                SetFindCheckUI(nowuiindex);
            }
        }
        #endregion
    }
}
