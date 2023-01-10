using ISIA.INTERFACE.ARGUMENTSPACK;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using TAP;
using TAP.Data.Client;
using TAP.Models.User;
using TAP.UI;
using ISIA.UI.BASE;

namespace ISIA.UI.ADMINISTRATOE
{
    public partial class WorkGroupAdd : Form
    {
        public WorkGroupAdd()
        {
            InitializeComponent();
            InitializeLanguage();
            bs = new BizDataClient("ISIA.BIZ.ADMINISTRATOE.dll", "ISIA.BIZ.ADMINISTRATOE.ShiftManagement");
        }
        #region Field
        ComboBoxControl ComboBoxControl = new ComboBoxControl();
        BizDataClient bs = null;
        DataSet rawdt = new DataSet();

        //ResponseArgsPack args = new ResponseArgsPack();
        public int selectIndex = 0;
        private bool bMainEQP = true;
        private string intervalOption = "LDEM"; //L ast D ay of E very M onth.   First Day Of Every Month, Custom Interval Day
        #endregion

        #region Event     
        private void simpleButton3_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion
        private void InitializeLanguage()
        {
            FormSetLanguage formSetLanguage = new FormSetLanguage();
            List<DevExpress.XtraEditors.LabelControl> labels = new List<DevExpress.XtraEditors.LabelControl>();
            labels.Add(labelControl1);
            labels.Add(labelControl5);
            labels.Add(lblShift);
            formSetLanguage.SetLanguage_LabelControl(labels);
            List<DevExpress.XtraEditors.SimpleButton> buttons = new List<DevExpress.XtraEditors.SimpleButton>();
            buttons.Add(btnSave);
            buttons.Add(btnCancel);
            formSetLanguage.SetLanguage_SimpleButton(buttons);
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                string addGroupName = txtGroupName.Text;
                string addDESCRIPTION = txtDESCRIPTION.Text;
                string addShift = txtShift.Text;

                WorkGroupArgsPack workGroupArgs = new WorkGroupArgsPack();

                workGroupArgs.Region = InfoBase._USER_INFO.Region;
                workGroupArgs.Name = addGroupName;
                workGroupArgs.Shift = addShift;
                workGroupArgs.Description = addDESCRIPTION;
                workGroupArgs.InsertTime = DateTime.Now.ToString("yyyyMMddHHmmssff");
                workGroupArgs.InsertUser = InfoBase._USER_INFO.Name;
                workGroupArgs.IsAlive = "YES";

                int SaveCount=bs.ExecuteModify("InsertWorkGroup", workGroupArgs.getPack());
                
                if (SaveCount == 0)
                {
                    TAPMsgBox.Instance.ShowMessage(this.Text, EnumMsgType.ERROR, "Insert failure..");
                }
                else
                {
                    TAPMsgBox.Instance.ShowMessage(this.Text, EnumMsgType.WARNING, "Insert complete..");
                    this.Close();
                }
            }
            catch (System.Exception ex)
            {
                TAP.UI.TAPMsgBox.Instance.ShowMessage(this.Text, EnumMsgType.ERROR, ex.ToString());
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
