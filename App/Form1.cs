using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Hack_Loader2
{
    public partial class Form1 : Form
    {
        readonly static internal string workDir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\HackLoader\\";
        readonly static string ver = "2.1";
        readonly static internal string link = "http://timoxa5651.siteme.org/hackloader/v2.0.1/";
        public static string json = Web.Get(link + "json.php");
        public Form1()
        {
            Json.Deserialize();
            InitializeComponent();
            Nodes();
        }
        public static void Clean()
        {
            string[] files = Directory.GetFiles(workDir, "*.dll", SearchOption.TopDirectoryOnly);
            foreach (string str in files)
            {
                try
                {
                    File.Delete(str);
                }
                catch
                {
                    MessageBox.Show("Delete err. Выгрузите старый чит сначала!");
                    Environment.Exit(0);
                }
            }
            try
            {
                Directory.Delete(workDir + "cfg", true);
            }
            catch (DirectoryNotFoundException)
            {

            }
            catch
            {
                MessageBox.Show("Delete err. Возможно мешает антивирус");
                Environment.Exit(0);
            }
        } //Clear files

        private void Form1_Load(object sender, EventArgs e)
        {
            checkBox2.Checked = true;
            bool DllsOk = false;
            try
            {
                if (File.Exists(workDir + "dlls.zip"))
                {
                    if (Helper.IsLast(Helper.GetMd5(workDir + "dlls.zip")))
                    {
                        DllsOk = true;
                    }
                    else
                    {
                        if (Web.DownloadFile(link + "dlls.zip", workDir + "dlls.zip"))
                        {
                            DllsOk = true;
                        }
                        else
                        {
                            DllsOk = false;
                        }
                    }

                } //Load
                else
                {
                    Directory.CreateDirectory(workDir);
                    if (Web.DownloadFile(link+"dlls.zip", workDir + "dlls.zip"))
                    {
                        DllsOk = true;
                    }
                    else
                    {
                        DllsOk = false;
                    }

                } //Load
            }
            catch (WebException)
            {
                MessageBox.Show("Cant connect/download");
            }
            catch(Exception ex)
            {
                MessageBox.Show("Error on 1st check" + "\n" + ex);
            } //Download

            try
            {
                Clean();
            }
            catch(Exception exx)
            {
                Program.Crash(exx, "Clean err");
            }
            try
            {
                ZipFile.ExtractToDirectory(workDir + "\\dlls.zip", workDir);
            }
            catch(Exception ex)
            {
                Program.Crash(ex, "Cant extract dlls");
                Environment.Exit(0);
            }


            // Second check
            bool rd2 = Helper.CountCheck();
            if (!rd2 && DllsOk)
            {
                MessageBox.Show("Error on 2nd check, try again");
                Directory.Delete(workDir, true);
                Environment.Exit(0);
            }

        }

        private void Main_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (main.SelectedNode.Parent == null)
            {
                button1.Enabled = false;
                label4.Visible = false;
                label5.Visible = false;
                return;
            } //if not cheat
            label4.Visible = true;
            label5.Visible = true;
            int vac = -1;
            int unt = -1;
            foreach (Json.Rage arr in Json.Main.rage)
            {
                if(arr.name == e.Node.Name)
                {
                    vac = Convert.ToInt16(arr.vac);
                    unt = Convert.ToInt16(arr.untrusted);
                }
            } //rage
            if(vac == -1)
            {
                foreach (Json.Legit arr in Json.Main.legit)
                {
                    if (arr.name == e.Node.Name)
                    {
                        vac = Convert.ToInt16(arr.vac);
                        unt = Convert.ToInt16(arr.untrusted);
                    }
                }

            } //legit
            if(vac == 0)
            {
                label4.Text = "Undetected";
                label4.ForeColor = Color.Green;
            }
            else if(vac == 2)
            {
                label4.Text = "Detected";
                label4.ForeColor = Color.Red;
            }
            else
            {
                label4.Text = "Unknown";
                label4.ForeColor = Color.Yellow;
            }
            if (unt == 0)
            {
                label5.Text = "Undetected";
                label5.ForeColor = Color.Green;
            }
            else if (unt == 2)
            {
                label5.Text = "Detected";
                label5.ForeColor = Color.Red;
            }
            else
            {
                label5.Text = "Unknown";
                label5.ForeColor = Color.Yellow;
            }
            button1.Enabled = true;
            
        } //After cheat select

        private void Button1_Click(object sender, EventArgs e)
        {
            if (!File.Exists(workDir + main.SelectedNode.Name + ".dll"))
            {
                MessageBox.Show("Длл не найден... Что-то не так");
                return;
            }
            if (Helper.IsProcess("csgo"))
            {
                try
                {
                    Process[] proc = Process.GetProcessesByName("csgo");
                    foreach(Process pro in proc)
                    {
                        pro.Kill();
                    }
                }
                catch { }
                label1.Text = CSGO.InjecttSafe(main.SelectedNode.Name, checkBox1.Checked);
            }
            else
            {
                label1.Text = CSGO.InjecttSafe(main.SelectedNode.Name, checkBox1.Checked);
            }
            if(label1.Text == "OK")
            {
                if (checkBox2.Checked)
                {
                    Environment.Exit(0);
                }
                label1.ForeColor = Color.Green;
            }
            else
            {
                label1.ForeColor = Color.Red;
            }
        } // Inject button
    }
}
