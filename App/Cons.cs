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
    public partial class Cons
    {
        readonly static internal string workDir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\HackLoader\\";
        internal readonly static string ver = "2.2";
        readonly static internal string link = @"http://timoxa5651.siteme.org/hackloader/v2.0.1/"; //Link
        readonly static internal string handler = @"http://timoxa5651.siteme.org/hackloader/v2.0.1/json.php"; //Handler
        public static string json = Web.Get(handler); //Json data
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

        public static void Load()
        {
            Console.Title = "Hack-Loader v"+ver+". Last version: v"+Json.data.version;
            int count = 0;
            foreach (var process in Process.GetProcesses().Where(d => d.ProcessName.ToLower().StartsWith("hack-loader"))) //Close other processes
            {
                if(process.Id != Process.GetCurrentProcess().Id)
                {
                    count += 1;
                    process.Kill();
                }
            }
            System.Console.ForegroundColor = ConsoleColor.Green;
            System.Console.WriteLine("Найдено "+ count + " копий процесса...закрыли");
            bool DllsOk = false;
            try
            {
                if (File.Exists(workDir + "dlls.zip"))
                {
                    if (Helper.IsLast(Helper.GetMd5(workDir + "dlls.zip")))
                    {
                        System.Console.WriteLine("Готово! Последняя версия");
                        DllsOk = true;
                    }
                    else
                    {
                        System.Console.WriteLine("Нашел обновление...");
                        if (Web.DownloadFile(link + "dlls.zip", workDir + "dlls.zip"))
                        {
                            System.Console.WriteLine("Готово!");
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
                    System.Console.WriteLine("Загружаю файлы...");
                    Directory.CreateDirectory(workDir);
                    if (Web.DownloadFile(link+"dlls.zip", workDir + "dlls.zip"))
                    {
                        System.Console.WriteLine("Готово!");
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
                System.Console.ForegroundColor = ConsoleColor.Red;
                System.Console.WriteLine("Ошибка скачивания!");
            }
            catch
            {
                System.Console.ForegroundColor = ConsoleColor.Red;
                System.Console.WriteLine("Ошибка:(");
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
            if (!Helper.CountCheck() && DllsOk)
            {
                System.Console.ForegroundColor = ConsoleColor.Red;
                System.Console.WriteLine("Возможно разраб даун, перезапусти меня...");
                Directory.Delete(workDir, true);
                Environment.Exit(0);
            }
            System.Threading.Thread.Sleep(500);
            KeyMenu();

        }
        private static bool AskForCfg(string name)
        {
            string cfg = Path.GetFileName(name).Replace(".dll", "");
            int vac = -1;
            int unt = -1;
            foreach (Json.Rage arr in Json.Main.rage)
            {
                if (arr.name == cfg)
                {
                    vac = Convert.ToInt16(arr.vac);
                    unt = Convert.ToInt16(arr.untrusted);
                }
            }
            if(vac == -1 || unt == -1)
            {
                foreach (Json.Legit arr in Json.Main.legit)
                {
                    if (arr.name == cfg)
                    {
                        vac = Convert.ToInt16(arr.vac);
                        unt = Convert.ToInt16(arr.untrusted);
                    }
                }
            }
            switch (vac)
            {
                case 0:
                    System.Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("VAC статус: Undetected");
                    break;
                case 1:
                    System.Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("VAC статус: Use at own risk");
                    break;
                case 2:
                    System.Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("VAC статус: Detected");
                    break;
            }
            switch (unt)
            {
                case 0:
                    System.Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Untrusted статус: Undetected");
                    break;
                case 1:
                    System.Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("Untrusted статус: Use at own risk");
                    break;
                case 2:
                    System.Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Untrusted статус: Detected");
                    break;
            }

            System.Console.ForegroundColor = ConsoleColor.Cyan;
            System.Console.WriteLine("Загрузить кфг?(ЗАМЕНИТ СТАРЫЙ)");
            System.Console.WriteLine("1 - Да, любое другое - нет");
            string key = System.Console.ReadLine();
            if(key == "1")
            {
                return true;
            }
            else
            {
                return false;
            }

        }
        public static void KeyMenu()
        {
            int key = 0;
            System.Console.Clear();
            System.Console.ForegroundColor = ConsoleColor.Cyan;
            System.Console.WriteLine("Я очень рад что все загрузилось:)");
            System.Console.WriteLine("Всего RAGE читов : "+ Json.Main.rage.Count+ " ,а LEGIT читов: "+ Json.Main.legit.Count);
            System.Console.WriteLine("Введи 1: Rage/HvH софт");
            System.Console.WriteLine("Введи 2: Legit софт");
            while (key == 0)
            {
                try
                {
                    key = Convert.ToInt16(System.Console.ReadLine().ToString());
                    switch (key){
                        case 1:
                            string k = RageMenu();
                            string res = InjectDll(k, AskForCfg(k));
                            if(res != "OK")
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                            }
                            else
                            {
                                Console.ForegroundColor = ConsoleColor.Green;
                            }
                            System.Console.WriteLine("Результат: " + res);
                            System.Console.ForegroundColor = ConsoleColor.Cyan;
                            break;
                        case 2:
                            string kk = LegitMenu();
                            string ress = InjectDll(kk, AskForCfg(kk));
                            if (ress != "OK")
                            {
                                System.Console.ForegroundColor = ConsoleColor.Red;
                            }
                            System.Console.WriteLine("Результат: " + ress);
                            System.Console.ForegroundColor = ConsoleColor.Cyan;
                            break;
                        default:
                            System.Console.ForegroundColor = ConsoleColor.Red;
                            System.Console.WriteLine("Введи еще раз");
                            System.Console.ForegroundColor = ConsoleColor.Cyan;
                            key = 0;
                            break;
                    }
                }
                catch (FormatException)
                {
                    System.Console.ForegroundColor = ConsoleColor.Red;
                    System.Console.WriteLine("Только цифры!");
                    System.Console.ForegroundColor = ConsoleColor.Cyan;
                }
            }
            
        }
        internal static string LegitMenu()
        {
            Dictionary<int, string> cheats = new Dictionary<int, string>(Convert.ToInt16(Json.data.count));
            int num = 0;
            int key = 0;
            string path = "";
            foreach (Json.Legit name in Json.Main.legit)
            {
                num += 1;
                cheats.Add(num, name.name);
                System.Console.WriteLine(num + ": " + name.name);
            }
            while (key == 0)
            {
                try
                {
                    key = Convert.ToInt16(System.Console.ReadLine().ToString());
                }
                catch
                {
                    System.Console.ForegroundColor = ConsoleColor.Red;
                    System.Console.WriteLine("Только цифры!");
                    System.Console.ForegroundColor = ConsoleColor.Cyan;
                    continue;
                }
                if (cheats.ContainsKey(key))
                {
                    path = workDir + cheats[key] + ".dll";
                }
                else
                {
                    System.Console.ForegroundColor = ConsoleColor.Red;
                    System.Console.WriteLine("Неверный ввод!");
                    System.Console.ForegroundColor = ConsoleColor.Cyan;
                    key = 0;
                }
            }
            return path;
        }
        internal static string RageMenu()
        {
            Dictionary<int, string> cheats = new Dictionary<int, string>(Convert.ToInt16(Json.data.count));
            int num = 0;
            int key = 0;
            string path = "";
            foreach(Json.Rage name in Json.Main.rage)
            {
                num += 1;
                cheats.Add(num, name.name);
                System.Console.WriteLine(num +  ": "+ name.name);
            }
            while (key == 0)
            {
                try
                {
                    key = Convert.ToInt16(System.Console.ReadLine().ToString());
                }
                catch
                {
                    System.Console.ForegroundColor = ConsoleColor.Red;
                    System.Console.WriteLine("Только цифры!");
                    System.Console.ForegroundColor = ConsoleColor.Cyan;
                    continue;
                }
                if (cheats.ContainsKey(key))
                {
                    path = workDir + cheats[key] + ".dll";
                }
                else
                {
                    System.Console.ForegroundColor = ConsoleColor.Red;
                    System.Console.WriteLine("Неверный ввод!");
                    System.Console.ForegroundColor = ConsoleColor.Cyan;
                    key = 0;
                }
            }
            return path;
            
        }
        private static string InjectDll(string path, bool cfg)
        {
            string res = "Unknown Error";
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
                res = CSGO.InjecttSafe(path, cfg); // Inject
            }
            else
            {
                res = CSGO.InjecttSafe(path, cfg); // Inject
            }
            return res;
        } // Inject button
    }
}
