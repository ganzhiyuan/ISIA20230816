using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ISIA.UI.BASE
{
    public static class Tools
    {
        public static void BindingControlWithArgs<T>(TAP.UIControls.BasicControls.TPanel panel, T args)
        {
            const System.Reflection.BindingFlags flags = System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.IgnoreCase;

            PropertyInfo[] objFieldNames = typeof(T).GetProperties(flags);

            foreach (PropertyInfo item in objFieldNames)
            {
                string tmptxt = string.Format("txt_{0}", item.Name);
                string tmpcbo = string.Format("cbo_{0}", item.Name);
                string tmplbl = string.Format("lbl_{0}", item.Name);
                string tmpchk = string.Format("chk_{0}", item.Name);
                string tmpdate = string.Format("date_{0}", item.Name);

                if (panel.Controls.Find(tmptxt, true).Length > 0)
                {
                    if (panel.Controls.Find(tmptxt, true)[0] is TAP.UIControls.BasicControlsDEV.TTextBox||panel.Controls.Find(tmptxt, true)[0] is TAP.UIControls.BasicControlsDEV.TMemoEdit)
                    {
                        panel.Controls.Find(tmptxt, true)[0].Text = item.GetValue(args) == null ? "" : item.GetValue(args).ToString();
                    }
                }
               
                if (panel.Controls.Find(tmpcbo, true).Length > 0)
                {
                    if (panel.Controls.Find(tmpcbo, true)[0] is TAP.UIControls.BasicControlsDEV.TComboBox)
                    {
                        TAP.UIControls.BasicControlsDEV.TComboBox comboBox = ((TAP.UIControls.BasicControlsDEV.TComboBox)panel.Controls.Find(tmpcbo, true)[0]);
                        if (comboBox.Properties.Items.Count == 0) {
                            comboBox.Setting();
                        }
                        if (comboBox.Properties.Items.Contains(item.GetValue(args)))
                        {
                            comboBox.SelectedItem = item.GetValue(args)==null?"": item.GetValue(args).ToString();
                        }
                        else
                        {
                            comboBox.Text = "";
                        }
                    }
                }
                if (panel.Controls.Find(tmplbl, true).Length > 0)
                {
                    if (panel.Controls.Find(tmplbl, true)[0] is TAP.UIControls.BasicControlsDEV.TLabel)
                    {
                        if (item.GetValue(args) == null) { continue; }
                        panel.Controls.Find(tmplbl, true)[0].Text = item.GetValue(args).ToString();
                    }
                }
                if (panel.Controls.Find(tmpdate, true).Length > 0)
                {
                    if (panel.Controls.Find(tmpdate, true)[0] is DevExpress.XtraEditors.TimeEdit)
                    {
                        ((DevExpress.XtraEditors.TimeEdit)panel.Controls.Find(tmpdate, true)[0]).EditValue = item.GetValue(args) == null ? "" : item.GetValue(args).ToString();
                    }
                }
                if (panel.Controls.Find(tmpchk, true).Length > 0)
                {
                    if (panel.Controls.Find(tmpchk, true)[0] is TAP.UIControls.BasicControlsDEV.TCheckBox)
                    {
                        string value=   item.GetValue(args) == null ? "" : item.GetValue(args).ToString();
                        ((TAP.UIControls.BasicControlsDEV.TCheckBox)panel.Controls.Find(tmpchk, true)[0]).Checked = value.ToUpper() == "Y" ? true : false;
                    }
                }
            }
        }

        public static T BindingArgsWithControl<T>(TAP.UIControls.BasicControls.TPanel panel, T args)
        {
            const System.Reflection.BindingFlags flags = System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.IgnoreCase;

            PropertyInfo[] objFieldNames = typeof(T).GetProperties(flags);

            foreach (PropertyInfo item in objFieldNames)
            {
                string tmptxt = string.Format("txt_{0}", item.Name);
                string tmpcbo = string.Format("cbo_{0}", item.Name);
                string tmpdate = string.Format("date_{0}", item.Name);
                string tmpchk = string.Format("chk_{0}", item.Name);

                if (panel.Controls.Find(tmptxt, true).Length > 0)
                {
                    if (panel.Controls.Find(tmptxt, true)[0] is TAP.UIControls.BasicControlsDEV.TTextBox||panel.Controls.Find(tmptxt, true)[0] is TAP.UIControls.BasicControlsDEV.TMemoEdit)
                    {
                        if (item.PropertyType.Name.Equals("Int32"))
                        {
                            if (!string.IsNullOrEmpty(panel.Controls.Find(tmptxt, true)[0].Text))
                            {
                                item.SetValue(args, Convert.ToInt32(panel.Controls.Find(tmptxt, true)[0].Text));
                            }
                        }
                        else
                        {
                            item.SetValue(args, panel.Controls.Find(tmptxt, true)[0].Text);
                        }
                    } 
                }
                if (panel.Controls.Find(tmpcbo, true).Length > 0)
                {
                    if (panel.Controls.Find(tmpcbo, true)[0] is TAP.UIControls.BasicControlsDEV.TComboBox)
                    {
                        string value = ((TAP.UIControls.BasicControlsDEV.TComboBox)panel.Controls.Find(tmpcbo, true)[0]).SelectedItem == null ? "" : ((TAP.UIControls.BasicControlsDEV.TComboBox)panel.Controls.Find(tmpcbo, true)[0]).SelectedItem.ToString();
                        item.SetValue(args, value);
                    }
                }
                if (panel.Controls.Find(tmpdate, true).Length > 0)
                {
                    if (panel.Controls.Find(tmpdate, true)[0] is DevExpress.XtraEditors.TimeEdit)
                    {
                        string dateValue = ((DevExpress.XtraEditors.TimeEdit)panel.Controls.Find(tmpdate, true)[0]).Text;
                        if (dateValue.Length == 5)
                        {
                            item.SetValue(args, dateValue.Substring(0, 2) + dateValue.Substring(3, 2));
                        }
                        else if (dateValue.Length == 4)
                        {
                            item.SetValue(args, 0 + dateValue.Substring(0, 1) + dateValue.Substring(2, 2));
                        }
                    }

                }
                if (panel.Controls.Find(tmpchk, true).Length > 0)
                {
                    if (panel.Controls.Find(tmpchk, true)[0] is TAP.UIControls.BasicControlsDEV.TCheckBox)
                    {
                        item.SetValue(args, ((TAP.UIControls.BasicControlsDEV.TCheckBox)panel.Controls.Find(tmpchk, true)[0]).Checked == true ? "Y" : "N");
                    }
                }
            }

            return args;
        }
    }
}
