using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using TEST.UI.CHARTTEST;

namespace TEST.UI
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new EffectTrace());
            //Application.Run(new TotalCorrelation());
            //Application.Run(new LowYieldReport());
            //Application.Run(new MapSpec());
            //Application.Run(new SampleMap_New());
            Application.Run(new ChartTest());
            //Application.Run(new SampleChart());
            //Application.Run(new Form2());
        }
    }
}
