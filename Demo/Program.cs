using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Demo
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
            //Application.Run(new JacobiForm());
            //Application.Run(new SobolevWalshPartSumExample());
            //Application.Run(new TchebForm());
            Application.Run(new OdeSpectralLegendre());
        }
    }
}
