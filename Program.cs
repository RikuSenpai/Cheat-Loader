using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace HackLoader
{
    
    static class Program
    {

        [STAThread]
        static void Main()
        {
            if (Lang.GetLang() == "en")
            {
                Form2.IsEn = true;
            }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
            
        }
    }
}
