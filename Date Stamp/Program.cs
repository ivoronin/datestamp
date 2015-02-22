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
            if (args.Length == 1 && args[0] == "/install")
                DateStamp.Install();
            if (args.Length == 1 && args[0] == "/uninstall")
                DateStamp.Uninstall();
            if (args.Length == 2 && args[0] == "/copy")
                DateStamp.Copy(args[1]);
        }
    }
}
