using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Net;
using System.Diagnostics;
using System.IO.Compression;
using System.Security.Cryptography;

namespace HackLoader
{
    public partial class Form1 : Form
    {
        static string CalculateMD5(string filename)
        {
            using (var md5 = MD5.Create())
            {
                using (var stream = File.OpenRead(filename))
                {
                    var hash = md5.ComputeHash(stream);
                    return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
                }
            }
        }
        public static void Clear()
        {
            string[] workFiles = Directory.GetFiles(workDir);
            string dlls = workDir + "\\dlls.zip";
            int i = 0;
            try
            {
                Directory.Delete(workDir + "\\cfg", true);
            }
            catch { }
            foreach (string file in workFiles)
            {
                if (file != dlls)
                {
                    try
                    {
                        File.Delete(file);
                    }
                    catch
                    {
                        MessageBox.Show("Выгрузите старый чит прежде чем загружать новый!");
                        Environment.Exit(0);
                    }
                    i += 1;
                }
            }
        }
        public static string workDir = Environment.ExpandEnvironmentVariables("%AppData%\\Files");
        public Form1()
        {
            InitializeComponent();
            Directory.CreateDirectory(workDir);
        }
        WebClient webClient;
        readonly Stopwatch sw = new Stopwatch();
        public void DownloadFile(string urlAddress, string location)
        {
            using (webClient = new WebClient())
            {
                webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(Completed);
                webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(ProgressChanged);

                Uri URL = new Uri(urlAddress);

                sw.Start();

                try
                {
                    webClient.DownloadFileAsync(URL, location);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
        private void ProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            progressBar.Value = e.ProgressPercentage;
            labelPerc.Text = e.ProgressPercentage.ToString() + "%";
        }
        private void Completed(object sender, AsyncCompletedEventArgs e)
        {
            sw.Reset();
            label1.Text = "Успешно!";
            Clear();
            ZipFile.ExtractToDirectory(workDir + "\\dlls.zip", workDir);
            Application.Restart();
            return;
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            if (!File.Exists(workDir + "\\dlls.zip"))
            {
                try
                {
                    DownloadFile("http://timoxa5651.siteme.org/hackloader/dlls.zip", workDir + "\\dlls.zip");
                }
                catch { }
            }
            else
            {
                string lastmd5;
                using (StreamReader strr = new StreamReader(HttpWebRequest.Create(@"http://timoxa5651.siteme.org/hackloader/md5.txt").GetResponse().GetResponseStream()))
                lastmd5 = strr.ReadToEnd();
                string md5 = CalculateMD5(workDir + "\\dlls.zip");
                if (lastmd5 != md5)
                {
                    MessageBox.Show(Lang.jsonDe.updateNeeded);
                    Directory.Delete(workDir, true);
                    Application.Exit();
                    return;
                }
                Clear();
                ZipFile.ExtractToDirectory(workDir + "\\dlls.zip", workDir);
                Form2 sistema = new Form2();
                sistema.ShowDialog();
                Application.Exit();
                
            }
        }
    }
}
