using ISIA.INTERFACE.ARGUMENTSPACK;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TAP.Data.Client;
using TAP.Fressage;
using TAP.UI;
using ISIA.UI.BASE;

namespace ISIA.UI.ADMINISTRATOE
{
    public delegate void SetWorkUserModify(List<string> users);
    public partial class UserWorkFlowModify : Form
    {
        public event SetWorkUserModify SettblUser;
        BizDataClient bs = null;
        public UserWorkFlowModify()
        {
            InitializeComponent();
            bs = new BizDataClient("ISIA.BIZ.ADMINISTRATOE.dll", "ISIA.BIZ.ADMINISTRATOE.UserWorkFlow");
        }

        public UserWorkFlowModify(List<string> lstTemp)
        {
            InitializeComponent();
            InitializeLanguage();
            InitializeSerectUser(lstTemp);
            bs = new BizDataClient("ISIA.BIZ.ADMINISTRATOE.dll", "ISIA.BIZ.ADMINISTRATOE.UserWorkFlow");
        }
        private void InitializeLanguage()
        {
            FormSetLanguage formSetLanguage = new FormSetLanguage();
            List<DevExpress.XtraEditors.LabelControl> labels = new List<DevExpress.XtraEditors.LabelControl>();
            labels.Add(lblDep);
            labels.Add(lblPos);
            labels.Add(tLabel1);
            labels.Add(lblSelectUser);
            labels.Add(lblUserList);
            formSetLanguage.SetLanguage_LabelControl(labels);
            List<DevExpress.XtraEditors.SimpleButton> buttons = new List<DevExpress.XtraEditors.SimpleButton>();
            buttons.Add(btnAddAdmin);
            buttons.Add(btnAddEng);
            buttons.Add(btnAddOper);
            buttons.Add(btnApply);
            buttons.Add(btnClose);
            buttons.Add(btnSerach);
            formSetLanguage.SetLanguage_SimpleButton(buttons);
        }
        private void InitializeSerectUser(List<string> values)
        {
            for (int i = 0; i < values.Count; i++)
            {
                tListBoxX.Items.Add(values[i]);
            }

            tListBoxX.Refresh();
        }   

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void DeleteRight_Click(object sender, EventArgs e)
        {
            try
            {
                ArrayList ar = new ArrayList();

                foreach (object item in tListBoxX.SelectedItems)
                {
                    ar.Add(item);
                }

                foreach (object item in ar)
                {
                    tListBoxX.Items.Remove(item);
                }
            }
            catch (System.Exception ex)
            {

            }
        }
        
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            
            List<string> users = new List<string>();

            for (int i = 0; i < tListBoxX.Items.Count; i++)
            {
                users.Add(tListBoxX.Items[i].ToString());

            }
            SettblUser(users);
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnSerach_Click(object sender, EventArgs e)
        {
            WorkFlowArgsPack args = new WorkFlowArgsPack();
            DataTable tmpdt = new DataTable();
            try
            {
                args.Facility = cboPos.Text;
                args.Department = cboDep.Text;

                tmpdt = bs.ExecuteDataTable("GetUserList", args.getPack());
                
                foreach(DataRow dr in tmpdt.Rows)
                {
                    string strTemp = string.Format("{0}_{1}", dr["NAME"].ToString(), dr["USERNAME"].ToString());

                    tListBoxY.Items.Add(strTemp);
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        private void addAdminToolStripMenuItem_Click(object sender, EventArgs e)
        {   
            foreach(object item in  tListBoxY.SelectedItems)
            {
                string tempStr = string.Format("{0}_{1}", "A", item.ToString());

                if(!tListBoxX.Items.Contains(tempStr))
                {
                    tListBoxX.Items.Add(tempStr);
                }
            }
        }

        private void addOperatorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                ArrayList ar = new ArrayList();

                foreach (object item in tListBoxX.SelectedItems)
                {
                    ar.Add(item);
                }

                foreach (object item in ar)
                {
                    tListBoxX.Items.Remove(item);
                }
            }
            catch(System.Exception ex)
            {

            }
            //foreach (object item in tListBoxY.SelectedItems)
            //{
            //    string tempStr = string.Format("{0}_{1}", "O", item.ToString());

            //    if (!tListBoxX.Items.Contains(tempStr))
            //    {
            //        tListBoxX.Items.Add(tempStr);
            //    }
            //}
        }

        private void addEngineerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (object item in tListBoxY.SelectedItems)
            {
                //string tempStr = string.Format("{0}_{1}", "E", item.ToString());

                string tempStr = item.ToString();

                if (!tListBoxX.Items.Contains(tempStr))
                {
                    tListBoxX.Items.Add(tempStr);
                }
            }
        }
    }

}
