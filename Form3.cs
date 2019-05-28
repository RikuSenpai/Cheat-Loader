using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Collections;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text;

namespace HackLoader
{
    public partial class Form3 : Form
    {
        public static string workDir = Environment.ExpandEnvironmentVariables("%AppData%\\Files");
        public Form3()
        {
            InitializeComponent();
        }

        private void RichTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void Form3_Load(object sender, EventArgs e)
        {
            string lan = Lang.GetLang();
            string[] lines;
            if (lan == "ru")
            {
               lines = File.ReadAllLines(workDir + "\\rudata.txt", Encoding.Default);
            }
            else
            {
                lines = File.ReadAllLines(workDir + "\\endata.txt", Encoding.Default);
            }
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < lines.Length; i++)
            {
                sb.AppendFormat("{0}", lines[i]);
                sb.AppendLine();
            }
            richTextBox1.Text = sb.ToString();
        }
    }
}
