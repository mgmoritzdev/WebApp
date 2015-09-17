using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Web;
using System.Windows.Forms;

namespace WebApp4.Helpers
{
    public class FileHelpers
    {
        public static string selectDirectory(string currentSelectedDir = "")
        {
            FolderBrowserDialog fbd = null;
            string selectedDir = currentSelectedDir;
            try
            {
                var t = new Thread((ThreadStart)(() =>
                {
                    fbd = new FolderBrowserDialog();
                    fbd.RootFolder = System.Environment.SpecialFolder.Desktop;
                    fbd.SelectedPath = currentSelectedDir;
                    fbd.ShowNewFolderButton = true;
                    DialogResult result = fbd.ShowDialog();
                    if (result == DialogResult.OK && !fbd.SelectedPath.Equals(""))
                        selectedDir = fbd.SelectedPath;
                }));

                t.SetApartmentState(ApartmentState.STA);
                t.Start();
                t.Join();
            }
            catch (Exception e)
            {
                Debug.WriteLine("Quebrou + " + e.ToString());
            }

            return selectedDir;
        }
    }
}