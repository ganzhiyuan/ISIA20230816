using ISIA.INTERFACE.ARGUMENTSPACK;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UIService;

namespace ISIA.UI.ANALYSIS.UIService.UIServiceImpl.ANALYSIS.UI.ParameterClusteringAnalysis
{
    public class InitializationComboxParmTypeUIService : CommonUIService<FrmParameterClusteringAnalysis, object, AwrArgsPack>
    {

        public InitializationComboxParmTypeUIService(FrmParameterClusteringAnalysis frm, object args, AwrArgsPack argsPack) : base(frm, args, argsPack)
        {

        }

        public override object ConvertData(object data)
        {
            return base.ConvertData(data);
        }

        public override void DisplayData(FrmParameterClusteringAnalysis frm, object data)
        {
            DataSet ds = (DataSet)data;
            DataTable dt=ds.Tables[0];
            foreach( DataRow dr in dt.Rows)
            {
                frm.cmbParameterType.Properties.Items.Add(dr["parametertype"]);

            }
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override object GetData(AwrArgsPack args)
        {
            DataSet ds = Bs.ExecuteDataSet("GetParmType", args.getPack());
            return ds;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override AwrArgsPack HandleArugument(FrmParameterClusteringAnalysis frm)
        {
            return new AwrArgsPack();
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}
