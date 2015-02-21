using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using Microsoft.VisualBasic.FileIO;
using Microsoft.Win32;

namespace Date_Stamp
{
    class DateStamp
    {
        private static string treePath = @"Software\Classes\*\shell";
        private static string keyName = "datestamp";

        public static void install()
        {
            try
            {
                string assemblyLocation = System.Reflection.Assembly.GetEntryAssembly().Location;
                string commandLine = string.Format("{0} /copy \"%1\"", assemblyLocation);

                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(treePath, true).CreateSubKey(keyName))
                {
                    key.SetValue("", Strings.MakeDatedCopy);
                    key.CreateSubKey("command").SetValue("", commandLine);
                }
                MessageBox.Show(Strings.Success, Strings.Install, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, Strings.Install, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public static void uninstall()
        {
            try
            {
                Registry.CurrentUser.OpenSubKey(treePath, true).DeleteSubKeyTree(keyName);
                MessageBox.Show(Strings.Success, Strings.Uninstall, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, Strings.Uninstall, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public static void copy(string sourcePath)
        {
            Regex rgx = new Regex("(?=\\.[^.\\\\]+$)");
            string destinationPath = rgx.Replace(sourcePath, DateTime.Now.ToString("_ddMMyy"));
            try
            {
                FileSystem.CopyFile(sourcePath, destinationPath, UIOption.AllDialogs);
            }
            catch { }
        }
    }
}
