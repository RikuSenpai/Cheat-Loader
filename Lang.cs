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
        public static string GetLang()
        {
            CultureInfo ci = CultureInfo.InstalledUICulture;
            if (ci.Name.Contains("ru"))
            {
                return "ru";
            }
            else
            {
                return "en";
            }

        }
        public static Dictionary<string, string> lang = new Dictionary<string, string>
        {
            {"loading", "Downloading files..."},
            {"unload", "Unload first cheat before starting another"},
            {"success", "Success!"},
            {"updateNeeded", "New update! Open me again"},
            {"injErr", "Inject failed.. Method: "},
            {"nocfg", "No cfg found, download latest version"},
            {"notfound", "Cant find that hack"},
            {"nocsgo", "No CS:GO found"},
            {"selectcheat", "Select cheat"},
            {"deleteall", "Delete all files"},
            {"closeafter", "Close after inject"},
            {"loadcfg", "Load cfg(!if you dont have!)"},
            {"click", "Click!"},
        };
    }
}

