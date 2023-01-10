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
using System.Threading.Tasks;
using System.Windows.Forms;
using TAP.Data.Client;
using TAP.Fressage;
using TAP.UI;
using static System.Windows.Forms.ListBox;
using ISIA.UI.BASE;

namespace ISIA.UI.ADMINISTRATOE
{
    public delegate void SetUserModify(List<string> users);
    public partial class WorkUser : Form
    {
        public event SetUserModify SettblUser;
        BizDataClient bs = null;
        public WorkUser()
        {
            InitializeComponent();
            bs = new BizDataClient("ISIA.BIZ.ADMINISTRATOE.dll", "ISIA.BIZ.ADMINISTRATOE.UserWorkFlow");
        }

        public WorkUser(List<string> lstTemp)
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
            labels.Add(lblFab);
            labels.Add(lblSelectUser);
            labels.Add(lblUserList);
            formSetLanguage.SetLanguage_LabelControl(labels);
            List<DevExpress.XtraEditors.SimpleButton> buttons = new List<DevExpress.XtraEditors.SimpleButton>();
            buttons.Add(btnApply);
            buttons.Add(btnAdd);
            buttons.Add(btnDelete);
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
            if (tListBoxX.SelectedItem == null)
            {
                return;
            }

            foreach (object item in tListBoxX.SelectedItems)
            {
                tListBoxX.Items.Remove(item);
            }

            //int indexs = tListBoxX.SelectedIndex;

            //tListBoxX.Items.RemoveAt(indexs);

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

            this.Close();
        }

        private void btnSerach_Click(object sender, EventArgs e)
        {
            WorkFlowArgsPack args = new WorkFlowArgsPack();
            DataTable tmpdt = new DataTable();
            try
            {
                args.Facility = cboFab.Text;
                args.Department = cboDep.Text;

                tmpdt = bs.ExecuteDataTable("GetUserList", args.getPack());
                
                foreach(DataRow dr in tmpdt.Rows)
                {
                    string strTemp = string.Format("{0}({1})", dr["NAME"].ToString(), dr["USERNAME"].ToString());
                
                    tListBoxY.Items.Add(strTemp);
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
        
        private void btnAdd_Click(object sender, EventArgs e)
        {
            foreach (object item in tListBoxY.SelectedItems)
            {               
                if (!tListBoxX.Items.Contains(item))
                {
                    tListBoxX.Items.Add(item);
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (tListBoxX.SelectedItem == null)
            {
                return;
            }

            foreach (object item in tListBoxX.SelectedItems)
            {
                tListBoxX.Items.Remove(item);
            }
        }
    }

}
