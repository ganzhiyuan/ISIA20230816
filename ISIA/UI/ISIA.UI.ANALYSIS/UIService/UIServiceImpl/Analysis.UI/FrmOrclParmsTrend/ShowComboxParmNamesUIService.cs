﻿using ISIA.INTERFACE.ARGUMENTSPACK;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UIHelper;

namespace ISIA.UI.ANALYSIS.UIHelper.UIServiceImpl.Analysis.UI.FrmOrclParmsTrend
{
    public class ShowComboxParmNamesUIService : CommonUIService<FrmOrclParmsTrendChart, object, AwrArgsPack>
    {

        public ShowComboxParmNamesUIService(FrmOrclParmsTrendChart frm, object args, AwrArgsPack argsPack) : base(frm, args, argsPack)
        {

        }

        public override object ConvertData(object data)
        {
            return base.ConvertData(data);
        }

        public override void DisplayData(FrmOrclParmsTrendChart frm, object data)
        {
            frm.tCheckComboBoxParmNames.Properties.Items.Clear();
            DataSet ds = (DataSet)data;
            DataTable dt = ds.Tables[0];
            foreach (DataRow dr in dt.Rows)
            {
                frm.tCheckComboBoxParmNames.Properties.Items.Add(dr["parametername"]);
            }
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override object GetData(AwrArgsPack args)
        {
            DataSet ds = Bs.ExecuteDataSet("GetParmNameByType", args.getPack());
            return ds;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override AwrArgsPack HandleArugument(FrmOrclParmsTrendChart frm)
        {
            AwrArgsPack awrArgsPack = new AwrArgsPack();
            awrArgsPack.ParamType = frm.tCheckComboBoxParmType.EditValue.ToString();
            return awrArgsPack;
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}
