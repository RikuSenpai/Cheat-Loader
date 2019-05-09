using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using Bleak;
using Microsoft.Win32;
using System.Threading;

namespace HackLoader
{
    public partial class Form2 : Form
    {
        public static string workDir = Environment.ExpandEnvironmentVariables("%AppData%\\Files");
        private static readonly string[] workFiles = Createfiles();
        public static string[] Createfiles()
        {
            string[] workFiles = Directory.GetFiles(workDir);
            string dlls = workDir + "\\dlls.zip";
            string data = workDir + "\\data.txt";
            string[] work = new string[workFiles.Length - 2];
            int i = 0;
            foreach (string file in workFiles)
            {
                if(file != dlls)
                {
                   if(file != data)
                    {
                        work[i] = file;
                        i += 1;
                    }
                   
                }
            }
            return work;
        }  
        public void CheckCSGO()
        {
                Process[] pname = Process.GetProcessesByName("csgo");
                if (pname.Length > 0)
                {
                    label2.ForeColor = Color.Green;
                }
                else
                {
                    label2.ForeColor = Color.Red;
                }
        }
        public Form2()
        {
            InitializeComponent();
            
        }
        private void Form2_Load(object sender, EventArgs e)
        {
            foreach (string file in workFiles)
            {
                string result = Path.GetFileName(file);
                result = result.Replace(".dll", String.Empty);
                comboBox1.Items.Add(result);
            }
            checkBox1.Checked = true;
            CheckCSGO();
        }
        private bool Injectt(string name)
        {
            CheckCSGO();
            string DllName = "\\" +name + ".dll";
            if (label2.ForeColor == Color.Green){
                try
                {
                    Random rn = new Random();
                    int value = rn.Next(1, 4);
                    var injector = new Injector();
                    if (value == 1)
                    {
                        injector.ManualMap("csgo", workDir + DllName);
                    }
                    else if(value == 2)
                    {
                        injector.CreateRemoteThread("csgo", workDir + DllName);
                    }
                    else if(value == 3)
                    {
                        injector.RtlCreateUserThread("csgo", workDir + DllName);
                    }
                    else
                    {
                        injector.CreateRemoteThread("csgo", workDir + DllName);
                    }
                    injector.EraseDllHeaders("csgo", workDir + DllName);
                    label1.Text = "OK";
                    return true;
                }
                catch
                {
                    label1.Text = "Ошибка инжекта...";
                    return false;
                }
            }
            return false;
        }
        private void Loadcfg(string name)
        {
            string droppath;
            bool need = false;
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
            else if (name == "M0ne0N")
            {
                droppath = @"C:\M0ne0N Free\";
                Directory.CreateDirectory(droppath);
            }
            else if (name == "1tapgang")
            {
                droppath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\1tapgang\\";
                Directory.CreateDirectory(droppath);
            }
            else if (name == "ferrum")
            {
                droppath = @"C:\ferrum\";
                Directory.CreateDirectory(droppath);
            }
            else if (name == "lumihook(gladiatorcheatz)")
            {
                droppath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\gladiatorcheatz\\";
                Directory.CreateDirectory(droppath);
                MessageBox.Show("При закрытии консоли ксго тоже закроется..");
            }
            else if(name == "publish.cc")
            {
                droppath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\PUBLISH\\";
                Directory.CreateDirectory(droppath);
            }
            else if(name == "iccluded")
            {
                droppath = @"C:\Iccluded\";
                Directory.CreateDirectory(droppath);
            }
            else if(name == "xy0")
            {
                droppath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\xyo\\";
                Directory.CreateDirectory(droppath);
            }
            else if(name == "postal")
            {
                droppath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\postal_cheat\\";
                Directory.CreateDirectory(droppath);
            }
            else
            {
                MessageBox.Show("Конфиг не найден:( Скачайте новую версию <3");
                return;
            }
            string[] needFiles = Directory.GetFiles(workDir + "\\cfg\\" + name + "\\");
            foreach(string fil in needFiles)
            {
                string[] gotFile = Directory.GetFiles(droppath, Path.GetFileName(fil));
                if (gotFile.Length == 0)
                {
                    need = true;
                }
            }
            if (!need)
            {
                return;
            }
            foreach(string fi in needFiles)
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
            foreach(string file in needFiles)
            {
                File.Copy(file, droppath+Path.GetFileName(file), true);
            }

        }
        private void Label3_Click(object sender, EventArgs e)
        {
            Directory.Delete(workDir, true);
            Environment.Exit(0);
        }
        private void Button1_Click(object sender, EventArgs e)
        {
            CheckCSGO();
            string DllName = "\\" + comboBox1.Text + ".dll";
            if (!File.Exists(workDir + DllName))
            {
                label1.Text = "Файл не найден..";
                return;
            }
            Process[] pname = Process.GetProcessesByName("csgo");
            if (pname.Length > 0)
            {
                if (checkBox2.Checked == true)
                {
                    Loadcfg(comboBox1.Text);
                }
                try
                {
                    bool st = Injectt(comboBox1.Text);
                    if (checkBox1.Checked == true && st)
                    {
                        Environment.Exit(0);
                    }
                }
                catch { }
                return;
            }
            else
            {
                label2.ForeColor = Color.Red;
                label1.Text = "Где CSGO?";
            }
            
        }

        private void Label4_Click(object sender, EventArgs e)
        {
            Form3 f3 = new Form3();
            f3.ShowDialog();
        }
    }
}
