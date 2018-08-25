using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FAESInstaller
{
    static class Program
    {
        
        [STAThread]
        static void Main(string[] args)
        {

            for (int i = 0; i < args.Length; i++)
            {
                args[i].ToLower();
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FAESInstaller());
        }
    }
}
