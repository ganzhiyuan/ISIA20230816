﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISIA.UI.ANALYSIS;
namespace UIHelper.UIServiceImpl.Analysis.UI.FrmOrclParmsTrend
{
    public class SerchUiService : CommonUIService<FrmOrclParmsTrendChart, object>
    {

        public SerchUiService(FrmOrclParmsTrendChart frm, object args)
        {
            this.Frm = frm;
            this.Args = args;
        }

        public override object ConvertData(object data)
        {
            return base.ConvertData(data);
        }

        public override void DisplayData(FrmOrclParmsTrendChart frm, object data)
        {
            base.DisplayData(frm, data);
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override object GetData(object args)
        {
            return base.GetData(args);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override void HandleArugument(FrmOrclParmsTrendChart frm)
        {
            base.HandleArugument(frm);
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}
