using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Management;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Hack_Loader2
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            try
            {
                AppDomain.CurrentDomain.UnhandledException += (sender, e)
                => OnCrash(e.ExceptionObject);
                Application.ThreadException += (sender, e)
                => Crash(e.Exception);

                if (!Web.CheckForInternetConnection())
                {
                    MessageBox.Show("No connection/Server down");
                    Environment.Exit(0);
                }
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new Form1());

            }
            catch(Exception e)
            {
                Crash(e);
            }
            
        }
        internal static void Crash(Exception ex, string message = "")
        {
            try
            {
                Dictionary<string, string> dic = new Dictionary<string, string>(9);
                dic.Add("Type", ex.GetType().FullName);
                dic.Add("source", ex.Message);
                dic.Add("Steam", Helper.IsProcess("Steam").ToString());
                dic.Add("csgo", Helper.IsProcess("csgo").ToString());
                var name = (from x in new ManagementObjectSearcher("SELECT Caption FROM Win32_OperatingSystem").Get().Cast<ManagementObject>()
                            select x.GetPropertyValue("Caption")).FirstOrDefault();
                name = name != null ? name.ToString() : "Unknown";
                dic.Add("winver", name.ToString());
                CultureInfo ci = CultureInfo.InstalledUICulture;
                dic.Add("lang", ci.Name);
                List<string> lst = Helper.ListInstalledAntivirusProducts();
                string avl = "";
                foreach (string av in lst)
                {
                    avl += av + " ";
                }
                dic.Add("Antivirus", avl);
                if(message != "")
                {
                    dic.Add("Custom", message);
                }
                string json = JsonConvert.SerializeObject(dic);
                Web.Get(Form1.link + "json.php?mode=crash&data=" + json);
                if(message != "")
                {
                    MessageBox.Show("Oops, error. Data sent. Message: "+message);
                }
                else
                {
                    MessageBox.Show("Oops, error. Data sent.");
                }
            }
            catch
            {
                MessageBox.Show("Oops, error. Data wasnot sent");
            }
        }
        static void OnCrash(object exceptionObject)
        {
            var huh = exceptionObject as Exception;
            Crash(huh);
        }
    }
}
