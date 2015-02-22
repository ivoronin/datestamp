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
        public static void Install()
        {
            try
            {
                string assemblyLocation = System.Reflection.Assembly.GetEntryAssembly().Location;
                string productName = Application.ProductName;
                string commandLine = string.Format("{0} /copy \"%1\"", assemblyLocation);

                using (RegistryKey HKCUSoftware = Registry.CurrentUser.OpenSubKey(@"Software", true))
                {
                    /* Configure Exporer context menu item */
                    using (RegistryKey key = HKCUSoftware.CreateSubKey(@"Classes\*\shell").CreateSubKey(productName))
                    {
                        key.SetValue("", Strings.MakeDatedCopy);
                        key.CreateSubKey("command").SetValue("", commandLine);
                    }

                    /* Configure default settings */
                    using (RegistryKey key = HKCUSoftware.CreateSubKey(productName))
                    {
                        key.SetValue(Strings.DateStampFormatKey, Strings.DefaultDateStampFormat);
                        key.SetValue(Strings.FileNameRegexKey, Strings.DefaultFileNameRegex);
                    }
                }
                MessageBox.Show(Strings.Success, Strings.Install, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, Strings.Install, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public static void Uninstall()
        {
            try
            {
                string productName = Application.ProductName;

                using (RegistryKey HKCUSoftware = Registry.CurrentUser.OpenSubKey(@"Software", true))
                {
                    HKCUSoftware.OpenSubKey(@"Classes\*\shell", true).DeleteSubKeyTree(productName);
                    HKCUSoftware.DeleteSubKeyTree(productName);
                }
                MessageBox.Show(Strings.Success, Strings.Uninstall, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, Strings.Uninstall, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public static void Copy(string sourcePath)
        {
            string dateStampFormat;
            string fileNameRegex;
            string destinationPath;
            string productName = Application.ProductName;

            try
            {
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software").OpenSubKey(productName))
                {
                    dateStampFormat = (string)key.GetValue(Strings.DateStampFormatKey, Strings.DefaultDateStampFormat);
                    fileNameRegex = (string)key.GetValue(Strings.FileNameRegexKey, Strings.DefaultFileNameRegex);
                }
            }
            catch
            {
                dateStampFormat = Strings.DefaultDateStampFormat;
                fileNameRegex = Strings.DefaultFileNameRegex;
            }

            destinationPath = Regex.Replace(sourcePath, fileNameRegex, DateTime.Now.ToString(dateStampFormat));

            try
            {
                FileSystem.CopyFile(sourcePath, destinationPath, UIOption.AllDialogs);
            }
            catch { }
        }
    }
}
