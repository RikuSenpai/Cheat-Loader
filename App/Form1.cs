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
        // ОБНОВИТЬ ВЕБ ЧАСТЬ!!! ДОБАВЛЕН MERCURY
        readonly static internal string workDir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\HackLoader\\";
        readonly static string ver = "2.1.3";
        readonly static internal string link = @"http://srv159232.hoster-test.ru/"; //Link
        readonly static internal string handler = @"http://srv159232.hoster-test.ru/json.php"; //Handler
        public static string json = Web.Get(handler); //Json data
        public Form1()
        {
            Json.Deserialize(); //Prepare json
            InitializeComponent();
            Nodes(); //Visualize cheats
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
            foreach (var process in Process.GetProcesses().Where(d => d.ProcessName.ToLower().StartsWith("hack-loader"))) //Close other processes
            {
                if(process.Id != Process.GetCurrentProcess().Id)
                {
                    process.Kill();
                }
            }
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

                }
                else //Download
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
                button2.Enabled = true;
                label4.Visible = false;
                label5.Visible = false;
                return;
            } //if not cheat
            else
            {
                checkBox1.Enabled = true;
                button2.Enabled = false;
            }
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
            if (Helper.IsProcess("csgo")) //If csgo opened
            {
                try
                {
                    Process[] proc = Process.GetProcessesByName("csgo");
                    foreach(Process pro in proc)
                    {
                        pro.Kill(); //Close it
                    }
                }
                catch { }
                label1.Text = CSGO.InjecttSafe(main.SelectedNode.Name, checkBox1.Checked); // Inject
            }
            else
            {
                label1.Text = CSGO.InjecttSafe(main.SelectedNode.Name, checkBox1.Checked); // Inject
            }
            if(label1.Text == "OK") //Check status
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
        private void Button2_Click(object sender, EventArgs e) // Custom dll button
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog()) //Open dialog
            {
                openFileDialog.InitialDirectory = "c:\\";
                openFileDialog.Filter = "DLL Files (*.dll)|*.dll|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 1;
                openFileDialog.RestoreDirectory = true;
                string filePath ="";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    filePath = openFileDialog.FileName.ToString();
                }
                if (!filePath.EndsWith(".dll"))
                {
                    label1.Text = "Not a dll";
                    label1.ForeColor = Color.Red;
                    return;
                }
                checkBox1.Enabled = false;
                try
                {
                    File.Copy(filePath, workDir + Path.GetFileName(filePath), false); //Copy dll to workdir
                }
                catch
                {
                    label1.Text = "Copy err";
                    label1.ForeColor = Color.Red;
                    checkBox1.Enabled = true;
                    return;
                }

                if (Helper.IsProcess("csgo"))
                {
                    try
                    {
                        Process[] proc = Process.GetProcessesByName("csgo");
                        foreach (Process pro in proc)
                        {
                            pro.Kill();
                        }
                    }
                    catch { }
                    label1.Text = CSGO.InjecttSafe(Path.GetFileNameWithoutExtension(filePath), checkBox1.Checked);
                    File.Delete(workDir + Path.GetFileName(filePath)); //Delete dll from workdir
                }
                else
                {
                    label1.Text = CSGO.InjecttSafe(Path.GetFileNameWithoutExtension(filePath), checkBox1.Checked);
                    File.Delete(workDir + Path.GetFileName(filePath));
                }
                if (label1.Text == "OK")
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
            }
        } 
    }
}
