using ISIA.INTERFACE.ARGUMENTSPACK;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TAP.Data.Client;
using TAP.UIControls.BasicControlsDEV;


namespace ISIA.UI.MANAGEMENT
{
    public delegate void SPECdele(Spec delespec);

    public partial class FrmParameterspecdata : Form
    {

        public Spec getspec = new Spec();
        BizDataClient bs = null;
        AwrCommonArgsPack args = null;

        public FrmParameterspecdata()
        {
            InitializeComponent();
        }

        public event SPECdele MYDELE;

        public FrmParameterspecdata(Spec spec)
        {
            InitializeComponent();
            bs = new BizDataClient("ISIA.BIZ.MANAGEMENT.DLL", "ISIA.BIZ.MANAGEMENT.ParameterSpecManagement");
            textDbName.Text = spec.DBNAME;
            textINSTANCE_NUMBER.Text = spec.INSTANCE_NUMBER;
            textPARAMETERNAME.Text = spec.PARAMETERNAME;
            textRULENAME.Text = spec.RULENAME;
            //spMeasurementDay.EditValue = null;

            spCONTROLUPPERLIMIT.EditValue = string.IsNullOrEmpty(spec.CONTROLUPPERLIMIT) == true ? null: spec.CONTROLUPPERLIMIT;
            spCONTROLLOWERLIMIT.EditValue = string.IsNullOrEmpty(spec.CONTROLLOWERLIMIT) == true ? null : spec.CONTROLLOWERLIMIT;
            spstdvalue.EditValue =  string.IsNullOrEmpty(spec.STD_VALUE) == true ? null : spec.STD_VALUE;
            sptarget.EditValue = string.IsNullOrEmpty(spec.TARGET) == true ? null : spec.TARGET;

            args = new AwrCommonArgsPack();
            args.ParameterName = spec.PARAMETERNAME;
            args.ParameterId = spec.PARAMETERID;
            
            args.DbId = spec.DBID;
            args.DbName = spec.DBNAME;
            args.InstanceNumber = spec.INSTANCE_NUMBER;
            args.ParameterName = spec.PARAMETERNAME;
            
        }

        private void tbOK_Click(object sender, EventArgs e)
        {
            
            getspec.CONTROLLOWERLIMIT = spCONTROLUPPERLIMIT.Text.ToString();
            getspec.CONTROLUPPERLIMIT = spCONTROLLOWERLIMIT.Text.ToString();
            getspec.TARGET = sptarget.Text.ToString();
            getspec.STD_VALUE = spstdvalue.Text.ToString();

            if (MYDELE != null)
            {
                MYDELE(getspec);
                this.Close();
            }


        }

        private void tbcalc_Click(object sender, EventArgs e)
        {
            DataTable dtparatype = bs.ExecuteDataTable("GetParameterType", args.getPack());

            if (dtparatype.Rows.Count == 0)
            {
                spCONTROLUPPERLIMIT.EditValue = null;
                spCONTROLLOWERLIMIT.EditValue = null;
                sptarget.EditValue = null;
                spstdvalue.EditValue = null;
                return;
            }

            args.ParameterType = dtparatype.Rows[0]["PARAMETERTYPE"].ToString();

            if (string.IsNullOrEmpty(spMeasurementDay.Text))
            {
                spMeasurementDay.BackColor = Color.Orange;
                return;
            }
            double days = Convert.ToDouble(string.Format("-{0}", spMeasurementDay.Text.ToString()));
            args.EndTimeKey = DateTime.Now.ToString();
            args.StartTimeKey = DateTime.Now.AddDays(days).ToString();

            DataSet dtclac = bs.ExecuteDataSet("GetParaValue", args.getPack());
            /*foreach (DataRow dataRow in dtclac.Tables[0].Rows)
            {
                if (string.IsNullOrEmpty(dataRow["N_VALUE"].ToString()))
                {
                    dtclac.Tables[0].Rows.Remove(dataRow);
                }
            }*/

            for (int i = dtclac.Tables[0].Rows.Count - 1; i >= 0; i--)
            {
                DataRow dataRow = dtclac.Tables[0].Rows[i];
                if (string.IsNullOrEmpty(dataRow["N_VALUE"].ToString()))
                {
                    dtclac.Tables[0].Rows.RemoveAt(i);
                }
            }



            var max =  dtclac.Tables[0].AsEnumerable().Max(x =>Convert.ToDouble(x["N_VALUE"]));
            var min =  dtclac.Tables[0].AsEnumerable().Min(x => Convert.ToDouble(x["N_VALUE"]));
            var avg = dtclac.Tables[0].AsEnumerable().Average(x => Convert.ToDouble(x["N_VALUE"]));
            var sum = dtclac.Tables[0].AsEnumerable().Sum(x => Convert.ToDouble(x["N_VALUE"]));
            double sumOfSquaredDifferences = 0;
            /*double stdDev = 0;
            double sqrSum = 0;
            int count = dtclac.Tables[0].Rows.Count;*/


            /*// 计算每个值与平均值之差的平方和
            foreach (DataRow row in dtclac.Tables[0].Rows)
            {
                double value = Convert.ToDouble(row["N_VALUE"]);
                sqrSum += Math.Pow(value - avg, 2);
            }



            // 计算标准偏差
            if (count > 1)
            {
                stdDev = Math.Sqrt(sqrSum / (count - 1));
                stdDev = stdDev / Math.Sqrt(count);
            }*/

            foreach (DataRow row in dtclac.Tables[0].Rows)
            {
                double value = Convert.ToDouble(row["N_VALUE"]);
                double difference = value - avg;
                sumOfSquaredDifferences += difference * difference;
            }

            double stdDev =  Math.Sqrt(sumOfSquaredDifferences / (dtclac.Tables[0].Rows.Count - 1));
             

            spCONTROLUPPERLIMIT.EditValue = Math.Round(max, 2);
            spCONTROLLOWERLIMIT.EditValue = Math.Round(min, 2);
            sptarget.EditValue = Math.Round(avg, 2);
            spstdvalue.EditValue = Math.Round(stdDev, 2);

        }

        private void tbclose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
