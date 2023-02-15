using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using TAP.Fressage;
using TAP.UI;
using TAP.UIControls.BasicControlsDEV;
using EnumMsgType = TAP.UI.EnumMsgType;

namespace ISIA.UI.BASE
{
    public class ComboBoxControl
    {
        /// <summary>
        /// Converter for cross-language
        /// </summary>
        private NeutralConverter _converter;

        /// <summary>
        /// Translator of cross-language
        /// </summary>
        private TemplateConverter _translator;
        private DevExpress.XtraNavBar.NavBarControl navBarControl1;
        Dictionary<string, List<string>> strchk = new Dictionary<string, List<string>>();
        public void SetCrossLang(TemplateConverter Nc)
        {
            _translator = Nc;
        }
        //初始化combobox
        
      

        public void SetComboBoxColor(TCheckComboBox  box)
        {
            if (box.Text.Equals(""))
            {
                box.BackColor = Color.SeaShell;
            }
            else
            {

                //box.Properties.AutoComplete = true;//自动搜索筛选
                //box.Properties.ImmediatePopup = true;//显示下拉列表
                                                     //单击编辑框 显示下拉列表
                box.Properties.ShowDropDown = DevExpress.XtraEditors.Controls.ShowDropDown.SingleClick;
                //下拉列表默认显示多少行 在显示滚动条
                box.Properties.DropDownRows = 6;
                //ComboBoxEdit是否允许编辑
                box.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;

                // cboFab.ShowPopup();
                box.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(243)))), ((int)(((byte)(247)))));
            }

        }
        //判断用户是否输入的是数字
        public bool IsInt(string value)
        {
            return Regex.IsMatch(value, @"^[+-]?\d*[.]?\d*$");
        }
        //combobox check function
        int i = 0;
        public int ComboBoxCheckValue(string Text,Control fatherControl)
        {
            i = 0;
            int res = ComboBoxCheck(Text, fatherControl);
            return res;
        }
        public int ComboBoxCheck(string Text, Control fatherControl)
        {
            Control.ControlCollection sonControls = fatherControl.Controls;
            foreach (Control control in sonControls)
            {
                if (control is DevExpress.XtraEditors.ComboBoxEdit)
                {
                    if (control.BackColor == Color.SeaShell)
                    {
                        //弹出消息 游标focus
                        if (i==1)
                        {
                            return i;
                        }
                        string name = control.Name.ToString().Remove(0, 3);
                        string tmpMessage = "";
                        string Lang = TAP.UI.InfoBase._USER_INFO.Language;
                        if (Lang.Equals("CN"))
                        {
                            tmpMessage = "请输入" + name;
                        }
                        else
                        {
                            tmpMessage = _translator.ConvertGeneralTemplate(EnumVerbs.INPUT, EnumGeneralTemplateType.ORDER, name);
                        }
                       
                        TAP.UI.TAPMsgBox.Instance.ShowMessage(Text, EnumMsgType.WARNING, tmpMessage);
                        control.Focus();
                        i = 1;
                        return i;
                    }
                }
                if (control.Controls != null)
                {
                    ComboBoxCheck(Text, control);
                }
            }
            return i;
        }
        public DialogResult IsUpdate(string Text)
        {
            string tmpMessage = _translator.ConvertGeneralTemplate(EnumVerbs.UPDATE, EnumGeneralTemplateType.CONFIRM, "");
            DialogResult dialog = TAP.UI.TAPMsgBox.Instance.ShowMessage(Text, EnumMsgType.CONFIRM, tmpMessage);
            return dialog;
        }
        public DialogResult IsDelete(string Text)
        {
            string tmpMessage = _translator.ConvertGeneralTemplate(EnumVerbs.DELETE, EnumGeneralTemplateType.CONFIRM, "");
            DialogResult dialog = TAP.UI.TAPMsgBox.Instance.ShowMessage(Text, EnumMsgType.CONFIRM, tmpMessage);
            return dialog;
        }
        public DialogResult IsInsert(string Text)
        {
            string tmpMessage = _translator.ConvertGeneralTemplate(EnumVerbs.INSERT, EnumGeneralTemplateType.CONFIRM, "");
            DialogResult dialog = TAP.UI.TAPMsgBox.Instance.ShowMessage(Text, EnumMsgType.CONFIRM, tmpMessage);
            return dialog;
        }

        public DialogResult IsYes(string Text, EnumVerbs en)
        {
            string tmpMessage = _translator.ConvertGeneralTemplate(en, EnumGeneralTemplateType.CONFIRM, "");
            DialogResult dialog = TAP.UI.TAPMsgBox.Instance.ShowMessage(Text, EnumMsgType.CONFIRM, tmpMessage);
            return dialog;
        }

        public bool CheckComboBoxValue(string Text, List<TAP.UIControls.IUIControl> lsc)
        {
            bool flag = true;
            foreach (TAP.UIControls.IUIControl con in lsc)
            {
                if (con.IsRequired)
                {
                    if (con is TComboBox)
                    {
                        if (string.IsNullOrEmpty(((TComboBox)con).Text.ToString().Trim()))
                        {
                            flag = false;
                            string name = ((TComboBox)con).Name.ToString().Remove(0, 3);
                            string tmpMessage = "";
                            string Lang = TAP.UI.InfoBase._USER_INFO.Language;
                            if (Lang.Equals("CN"))
                            {
                                tmpMessage = "请输入" + name;
                            }
                            else
                            {
                                tmpMessage = _translator.ConvertGeneralTemplate(EnumVerbs.INPUT, EnumGeneralTemplateType.ORDER, name);
                            }

                            TAP.UI.TAPMsgBox.Instance.ShowMessage(Text, EnumMsgType.WARNING, tmpMessage);
                        }
                    }
                    if (con is TCheckComboBox)
                    {
                        if (string.IsNullOrEmpty(((TCheckComboBox)con).Text.ToString().Trim()))
                        {
                            flag = false;
                            string name = ((TCheckComboBox)con).Name.ToString().Remove(0, 3);
                            string tmpMessage = "";
                            string Lang = TAP.UI.InfoBase._USER_INFO.Language;
                            if (Lang.Equals("CN"))
                            {
                                tmpMessage = "请输入" + name;
                            }
                            else
                            {
                                tmpMessage = _translator.ConvertGeneralTemplate(EnumVerbs.INPUT, EnumGeneralTemplateType.ORDER, name);
                            }

                            TAP.UI.TAPMsgBox.Instance.ShowMessage(Text, EnumMsgType.WARNING, tmpMessage);
                        }
                    }
                }
            }
            return flag;
        }

        public DataTable ComboBoxLink(Control fatherControl)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("ComboName", typeof(string));
            dt.Columns.Add("ComboText", typeof(string));
            dt = ComboBoxLinkValue(dt, fatherControl);
            return dt;
        }
        public DataTable ComboBoxLinkValue(DataTable dt, Control fatherControl)
        {           
            Control.ControlCollection sonControls = fatherControl.Controls;
            Boolean DateCheck = false;
            foreach (Control control in sonControls)
            {
                if (control.Controls != null)
                {
                    ComboBoxLinkValue(dt,control);
                }

                if (control is DevExpress.XtraEditors.ComboBoxEdit || control is DevExpress.XtraEditors.DateEdit || control is DevExpress.XtraEditors.CheckedComboBoxEdit || control is System.Windows.Forms.CheckBox)
                {
                    if (control.Text != "" )
                    {
                        if (control is DevExpress.XtraEditors.CheckedComboBoxEdit)
                        {
                            string ColumnName = control.Name;
                            string RowValue = Convert.ToString(((CheckedComboBoxEdit)control).Properties.GetCheckedItems());
                            dt.Rows.Add(ColumnName, RowValue);
                        }
                        else if (control is System.Windows.Forms.CheckBox)
                        {
                            if (control.Name == "chkdate")
                            {
                                if (((CheckBox)control).Checked)
                                {
                                    DateCheck = true;
                                   
                                }
                            }
                            if (((CheckBox)control).Checked)
                            {
                                string ColumnName = control.Name;
                                string RowValue = "true";
                                dt.Rows.Add(ColumnName, RowValue);
                            }
                            
                        }
                        else
                        {
                            if (control is DevExpress.XtraEditors.DateEdit)
                            {
                                if (DateCheck == false)
                                {
                                    continue;
                                }
                            }
                            string ColumnName = control.Name;
                            string RowValue = control.Text;

                            dt.Rows.Add(ColumnName, RowValue);
                        }
                        
                    }
                }
                
            }
            return dt;
        }
        public void AfterComboBoxLinkValue(DataTable dt, Control fatherControl)
        {
            Control.ControlCollection sonControls = fatherControl.Controls;
            foreach (Control control in sonControls)
            {
                if (control.Controls != null)
                {
                    AfterComboBoxLinkValue(dt,control);
                }

                if (control is DevExpress.XtraEditors.ComboBoxEdit || control is DevExpress.XtraEditors.DateEdit || control is DevExpress.XtraEditors.CheckedComboBoxEdit || control is System.Windows.Forms.CheckBox)
                {
                    
                    if (dt.Rows.Count == 0)
                    {
                        return;
                    }
                    for (int i = 0;i<dt.Rows.Count;i++)
                    {
                        if (control.Name.Equals(dt.Rows[i]["ComboName"]))
                        {
                            if (control is DevExpress.XtraEditors.ComboBoxEdit)
                            {
                                SelectedComboBox((ComboBoxEdit)control, dt.Rows[i]["ComboText"].ToString());
                            }
                            else if (control is DevExpress.XtraEditors.CheckedComboBoxEdit)
                            {
                                if (!dt.Rows[i]["ComboText"].ToString().Equals(""))
                                {
                                    foreach (CheckedListBoxItem item in ((CheckedComboBoxEdit)control).Properties.Items)
                                    {
                                        if (dt.Rows[i]["ComboText"].ToString().Contains(item.Value.ToString()))
                                        {
                                            item.CheckState = CheckState.Checked;

                                        }
                                    }
                                    
                                    control.Text = dt.Rows[i]["ComboText"].ToString();
                                }       
                            }
                            else if (control is System.Windows.Forms.CheckBox)
                            {
                                if (dt.Rows[i]["ComboText"].ToString().Equals("true"))
                                {
                                    ((CheckBox)control).Checked = true;
                                }
                            }
                            else
                            {
                                control.Text = dt.Rows[i]["ComboText"].ToString();
                            }
                        }
                    }
                }

            }
        }
        private void ChkM_Click_ChangeValue(object sender, EventArgs e)
        {
            ComboBoxEdit comboBox = (ComboBoxEdit)sender;
            string cbote = "";
            if (comboBox.SelectedIndex >= 0)
            {
                cbote = comboBox.Properties.Items[comboBox.SelectedIndex].ToString()+"/";

            }
            else { cbote = ""; return; }
            string chkname = "chk" + comboBox.Name.ToString();
            Control col = this.navBarControl1.Controls.Find(chkname, true)[0];
            CheckBox check = col as CheckBox;
            string cbotext = "";
            if (check.CheckState == CheckState.Checked)
            {
                List<string> vs = new List<string>();
                strchk.TryGetValue(comboBox.Name.ToString(), out vs);
                if (vs.Contains(cbote))
                {
                    for (int i = 0; i < vs.Count; i++)
                    {
                        if (vs[i] != "")
                        {
                            cbotext += vs[i];
                        }
                    }
                    comboBox.Text = cbotext;
                    return;
                }
                if (cbote != "/") { 
                vs.Add(cbote );
                }
                vs.Distinct();
                vs.Sort();
                strchk.Remove(comboBox.Name.ToString());
                strchk.Add(comboBox.Name.ToString(), vs);
                for (int i = 0; i < vs.Count; i++)
                {
                    if (vs[i] != "")
                    {
                        cbotext += vs[i];

                    }

                }
                comboBox.Text = cbotext;

            }
            else
            {
                cbotext = "";
            }


        }
        public void ChkM_Click(object sender, EventArgs e, DevExpress.XtraNavBar.NavBarControl navBarControl)
        {
            navBarControl1 = navBarControl;
            CheckBox check = (CheckBox)sender;
            string cboname = check.Name.ToString().Substring(3);
            Control col = this.navBarControl1.Controls.Find(cboname, true)[0];
            ComboBoxEdit comboBox = col as ComboBoxEdit;
            if (check.CheckState == CheckState.Checked)
            {

                List<string> vs = new List<string>();
                string cbote = "";
                if (comboBox.Text != "")
                {
                    cbote = comboBox.Text + "/";
                }
                else {
                    cbote = comboBox.Text;
                }
                vs.Add(cbote);
                strchk.Remove(cboname);
                strchk.Add(cboname, vs);
                comboBox.SelectedIndexChanged += new System.EventHandler(this.ChkM_Click_ChangeValue);
            }
            else
            {
                comboBox.SelectedIndex = 0;
            }
        }
        public void SelectedComboBox(ComboBoxEdit ComboBox, string str)
        {
            for (int i = 0; i < ComboBox.Properties.Items.Count; i++)
            {
                if (ComboBox.Properties.Items[i].ToString() == str)
                {
                    ComboBox.SelectedIndex = i;
                    break;
                }
            }
        }

        public void SelectedComboBox(CheckedComboBoxEdit ComboBox, string str)
        {
            if (str == "" )
            {
                ComboBox.CheckAll();
                return;
            }
            foreach (CheckedListBoxItem item in ComboBox.Properties.Items)
            {
                if (str.Contains(item.Value.ToString()))
                {
                    item.CheckState = CheckState.Checked;
                }
            }
        }
    }
}
