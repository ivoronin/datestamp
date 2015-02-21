using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Date_Stamp
{
    static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            if (args.Length == 0)
                DateStamp.install();
            else
            {
                if (args.Length == 1 && args[0] == "/install")
                    DateStamp.install();
                if (args.Length == 1 && args[0] == "/uninstall")
                    DateStamp.uninstall();
                if (args.Length == 2 && args[0] == "/copy")
                    DateStamp.copy(args[1]);
            }
        }
    }
}
