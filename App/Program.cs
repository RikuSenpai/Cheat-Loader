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

        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            if (!Web.CheckForInternetConnection())
            {
                MessageBox.Show("No connection");
                Environment.Exit(0);
            }
            AppDomain currentDomain = AppDomain.CurrentDomain;
            currentDomain.UnhandledException += new UnhandledExceptionEventHandler(OnCrash);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
            
        }
        static void OnCrash(object sender, UnhandledExceptionEventArgs args)
        {
            Exception e = (Exception)args.ExceptionObject;
            Dictionary<string, string> dic = new Dictionary<string, string>(6);
            dic.Add("IsTerminating", args.IsTerminating.ToString());
            dic.Add("Type", e.GetType().FullName);
            dic.Add("Steam", Helper.IsProcess("Steam").ToString());
            dic.Add("csgo", Helper.IsProcess("csgo").ToString());
            var name = (from x in new ManagementObjectSearcher("SELECT Caption FROM Win32_OperatingSystem").Get().Cast<ManagementObject>()
                        select x.GetPropertyValue("Caption")).FirstOrDefault();
            name = name != null ? name.ToString() : "Unknown";
            dic.Add("winver", name.ToString());
            CultureInfo ci = CultureInfo.InstalledUICulture;
            dic.Add("lang", ci.Name);
            string json = JsonConvert.SerializeObject(dic);
            Console.WriteLine(json);
        }
    }
}
