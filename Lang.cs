using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace HackLoader
{
    class Lang
    {
        public static string GetLang() {
            CultureInfo ci = CultureInfo.InstalledUICulture;
            if (ci.Name.Contains("ru"))
            {
                return "ru";
            }
            else {
                return "en";
            }

        }
        private static string GetJson() {
            string str;
            using (StreamReader strr = new StreamReader(HttpWebRequest.Create(@"http://timoxa5651.siteme.org/hackloader/json.php?lang=" + GetLang()).GetResponse().GetResponseStream()))
                str = System.Text.RegularExpressions.Regex.Unescape(strr.ReadToEnd());
            return str;
        }
        public static dynamic jsonDe = JsonConvert.DeserializeObject(GetJson());
    }

}
