using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;

namespace Hack_Loader2
{
    class Helper
    {
        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        public static bool IsLast(string md5)
        {
            string res = Json.data.md5;
            if(md5 == res)
            {
                return true;
            }
            else
            {
                return false;
            }
            
        }

        public static string GetMd5(string path)
        {
            using (var md5 = MD5.Create())
            {
                using (var stream = File.OpenRead(path))
                {
                    var hash= md5.ComputeHash(stream);
                    return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
                }
            }
        }

        public static bool CountCheck()
        {
            string[] files = Directory.GetFiles(Form1.workDir, "*.dll", SearchOption.TopDirectoryOnly);
            if(files.Length == Convert.ToInt16(Json.data.count))
            {
                return true;
            }
            return false;
        }

        public static bool IsProcess(string name)
        {
            var target = Process.GetProcessesByName(name).FirstOrDefault();
            if (target == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        
    }
    class Web
    {
        public static string Get(string url)
        {
            string str;
            using (StreamReader strr = new StreamReader(HttpWebRequest.Create(url).GetResponse().GetResponseStream()))
                str = strr.ReadToEnd();
            return str;
        }
        public static bool DownloadFile(string urlAddress, string location)
        {
            WebClient webClient;
            using (webClient = new WebClient())
            {

                Uri URL = new Uri(urlAddress);
                try
                {
                    webClient.DownloadFile(URL, location);
                }
                catch (Exception)
                {
                    return false;
                }
                if (File.Exists(location))
                {
                    return true;
                }
                return false;
            }
        }
    }
    class Json
    {

        public static Main data;
        internal static void Deserialize()
        {
            data = JsonConvert.DeserializeObject<Main>(Form1.json);
        }
#pragma warning disable IDE1006 // Стили именования
        internal class Rage
        {
            public string name { get; set; }
            public string vac { get; set; }

            public string untrusted { get; set; }

        }
        internal class Legit
        {
            public string name { get; set; }
            public string vac { get; set; }
            public string untrusted { get; set; }
        }
        internal class Main
        {
            public string md5 { get; set; }
            public string count { get; set; }
            public string version{ get; set; }
            [JsonProperty("rage")]
            public static List<Rage> rage { get; set; }
            [JsonProperty("legit")]
            public static List<Legit> legit { get; set; }
        }
#pragma warning restore IDE1006 // Стили именования
    }
    class CSGO
    {
        internal static string GetCfgPath(string name)
        {
            string droppath;
            if (name == "Eternity.cc")
            {
                RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Valve\\Steam\\");
                droppath = registryKey.GetValue("steampath").ToString();
                droppath += "\\steamapps\\common\\Counter-Strike Global Offensive\\";
                Directory.CreateDirectory(droppath);
            }
            else if (name == "pphud")
            {
                droppath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\PPHUD Free\\";
                Directory.CreateDirectory(droppath);
            }
            else if (name == "Interception")
            {
                droppath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\Interception\\";
                Directory.CreateDirectory(droppath);
            }
            else if (name == "1tapgang")
            {
                droppath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\1tapgang\\";
                Directory.CreateDirectory(droppath);
            }
            else if (name == "stickrpg")
            {
                droppath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\stickrpg\\";
                Directory.CreateDirectory(droppath);
            }
            else if (name == "ferrum")
            {
                droppath = @"C:\ferrum\";
                Directory.CreateDirectory(droppath);
            }
            else if (name == "M0ne0N")
            {
                droppath = @"C:\M0ne0N Free\";
                Directory.CreateDirectory(droppath);
            }
            else if (name == "Interium")
            {
                droppath = @"C:\Interium\Cfg\";
                Directory.CreateDirectory(droppath);
            }
            else if (name == "samoware")
            {
                droppath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\samoware\\";
                Directory.CreateDirectory(droppath);
            }
            else if (name == "xy0")
            {
                droppath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\xyo\\";
                Directory.CreateDirectory(droppath);
            }
            else
            {
                return "null";
            }
            return droppath;
        }
        internal static bool Loadcfg(string name)
        {
            bool need = false;
            string droppath = GetCfgPath(name);
            if (droppath == "null") //no cfg
            {
                DialogResult dialogResult = MessageBox.Show("No cfg found. Inject?/ Не удалось загрузить кфг. Продолжить?", "Warning", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.No)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            string[] needFiles = Directory.GetFiles(Form1.workDir + "\\cfg\\" + name + "\\");
            foreach (string fil in needFiles)
            {
                string[] gotFile = Directory.GetFiles(droppath, Path.GetFileName(fil));
                if (gotFile.Length == 0)
                {
                    need = true;
                }
            }
            if (!need)
            {
                return true;
            }
            foreach (string fi in needFiles)
            {
                string[] gotFile = Directory.GetFiles(droppath, Path.GetFileName(fi));
                try
                {
                    if (File.Exists(gotFile[0]))
                    {
                        File.Delete(gotFile[0]);
                    }

                }
                catch { }
            }
            Directory.CreateDirectory(droppath);
            foreach (string file in needFiles)
            {
                File.Copy(file, droppath + Path.GetFileName(file), true);
            }
            return true;

        }
        private static string InjSafe(string name)
        {
            if (!Helper.IsProcess("Steam"))
            {
                return "Open Steam first";
            }
            RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Valve\\Steam\\");
            string steampath = registryKey.GetValue("SteamPath").ToString() + "/";
            try
            {
                File.Move(steampath + "crashhandler.dll", steampath+ Helper.RandomString(11)+".dll");
            }
            catch {
                return "Rename err";
            }
            try
            {
                File.Copy(Form1.workDir + name + ".dll", steampath + "crashhandler.dll", true);
            }
            catch(Exception e)
            {
                MessageBox.Show(e.ToString());
                return "Copy err";
            }
            Process.Start(registryKey.GetValue("SteamExe").ToString(), "steam://rungameid/730");
            return "OK";
        }
        internal static string InjecttSafe(string name, bool cfg = false)
        {
            try
            {
                Web.Get("http://timoxa5651.siteme.org/hackloader/v2.0.1/json.php?mode=cheat&data=" + name);
            }
            catch { }
            if (cfg)
            {
                if (CSGO.Loadcfg(name))
                {
                    return InjSafe(name);
                }
                return "Cancelled";
            }
            try
            {
                return InjSafe(name);
            }
            catch
            {
                return "Error";
            }
        }
    }
}
