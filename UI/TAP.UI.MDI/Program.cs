using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace TAP.UI.MDI
{
    /// <summary>
    /// This is entry point
    /// </summary>
    static class Program
    {
        /// <summary>
        /// Entry point
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new FormLauncher());
           // Application.Run(new Form1());
            Application.Run(new FormRibbon_ISEM());

            //Application.Run(new NoDbForm());
            //Application.Run(new FormLauncher()); 
        }
    }
}