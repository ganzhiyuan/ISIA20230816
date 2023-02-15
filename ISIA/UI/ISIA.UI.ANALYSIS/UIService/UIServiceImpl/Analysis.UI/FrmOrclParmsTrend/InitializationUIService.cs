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
    public class InitializationUIService : CommonUIService<FrmOrclParmsTrendChart, object, AwrArgsPack>
    {

        public InitializationUIService(FrmOrclParmsTrendChart frm, object args, AwrArgsPack argsPack) : base(frm, args, argsPack)
        {

        }

        public override object ConvertData(object data)
        {
            return base.ConvertData(data);
        }

        public override void DisplayData(FrmOrclParmsTrendChart frm, object data)
        {
            DataSet ds = (DataSet)data;
            DataTable dt=ds.Tables[0];
            foreach( DataRow dr in dt.Rows)
            {
                frm.comboBoxDBName.Properties.Items.Add(dr["name"]);

            }
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override object GetData(AwrArgsPack args)
        {
            DataSet ds = Bs.ExecuteDataSet("GetDBName", args.getPack());
            return ds;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override AwrArgsPack HandleArugument(FrmOrclParmsTrendChart frm)
        {
            return new AwrArgsPack();
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}