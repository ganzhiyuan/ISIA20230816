using DevExpress.XtraEditors.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISIA.COMMON
{
    public class RepHelper


    {

        public RepositoryItem GetRepItem(string rep, string editname)
        {

            if (rep == "RepositoryItemComboBox")
            {
                return NewRepCbo(editname);
            }
            else {
                return NewRepCbo(editname);
            }


            
        }

        /// <summary>
        /// 获取一个内置的日期控件
        /// </summary>
        /// <param name="format">日期格式(d/g/G...)</param>
        /// <returns></returns>
        private RepositoryItemComboBox NewRepCbo(string editname)
        {

            if (editname == "CUSTOM05")
            {
                var rep = new RepositoryItemComboBox();
                //rep.ReadOnly = true;
                rep.Name = "CUSTOM05";
                rep.Items.Add("aaaa5");
                rep.Items.Add("cccc5");
                rep.Items.Add("bbbbb5");
                rep.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
                return rep;
            }
            else if (editname == "CUSTOM06")
            {
                var rep = new RepositoryItemComboBox();
                //rep.ReadOnly = true;
                rep.Name = "CUSTOM06";
                rep.Items.Add("aaaa6");
                rep.Items.Add("cccc6");
                rep.Items.Add("bbbbb6");
                rep.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
                return rep;
            }
            else {
                var rep = new RepositoryItemComboBox();
                //rep.ReadOnly = true;
                rep.Items.Add("aaaa");
                rep.Items.Add("cccc");
                rep.Items.Add("bbbbb");
                return rep;
            }
            
        }

    }
}
