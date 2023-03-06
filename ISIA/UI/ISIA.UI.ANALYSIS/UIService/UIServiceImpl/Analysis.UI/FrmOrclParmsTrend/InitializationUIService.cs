using ISIA.INTERFACE.ARGUMENTSPACK;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using UIHelper;

namespace ISIA.UI.ANALYSIS.UIHelper.UIServiceImpl.Analysis.UI.FrmOrclParmsTrend
{
    public class InitializationUIService : CommonUIService<FrmOrclParmsTrendChart, object, AwrArgsPack>
    {
        int _ClusteringMaxNum = 20;
        public InitializationUIService(FrmOrclParmsTrendChart frm, object args, AwrArgsPack argsPack) : base(frm, args, argsPack)
        {

        }

        public int ClusteringMaxNum { get => _ClusteringMaxNum; set => _ClusteringMaxNum = value; }

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
            for (int i = 1; i <= ClusteringMaxNum; i++)
            {
                frm.tCheckComboBoxClusteringCnt.Properties.Items.Add(i);
            }
            //init date period 
            frm.dateStart.DateTime = Convert.ToDateTime(DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd") + " " + EventArgPack.StartTime);
            frm.dateEnd.DateTime = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd") + " " + EventArgPack.EndTime);
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public static string TIME_SELECTION = "A";

        public override object GetData(AwrArgsPack args)
        {
            DataSet ds = Bs.ExecuteDataSet("GetDBName", args.getPack());
            XmlDocument doc = new XmlDocument();
            doc.Load(@".\ISIA.config");
            XmlNodeList nodeList = doc.SelectNodes("configuration/TAP.ISIA.Configuration/WX/Shift");
            foreach (XmlNode node in nodeList)
            {
                EventArgPack.StartTime = node[TIME_SELECTION].Attributes["StartTime"].Value;
                EventArgPack.EndTime = node[TIME_SELECTION].Attributes["EndTime"].Value;
            }
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
