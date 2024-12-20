﻿using LauncherApplicationV2.Services;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Reflection;
using System.Threading;
using System.Windows;
using WpfApp6.Services;
using WpfApp6.Services.Launch;
using WpfApp6;
using SevenZip.Compression.LZ;
using System.Threading.Tasks;

namespace WpfApp6.Pages
{
    /// <summary>
    /// Interaction logic for Home.xaml
    /// </summary>
    public partial class Home
    {
        public Home()
        {
            InitializeComponent();
        }

        public class DataUtility
        {
            private static readonly string FileName = "data.json";

            public static void SaveData(string email, string password, string path)
            {
                var data = new DataModel { Email = email, Password = password, Path = path };
                string jsonData = JsonConvert.SerializeObject(data);

                string localAppDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                string LauncherFolderPath = Path.Combine(localAppDataPath, "Launcher");

                Directory.CreateDirectory(LauncherFolderPath);

                string filePath = Path.Combine(LauncherFolderPath, FileName);

                File.WriteAllText(filePath, jsonData);
            }

            public static DataModel LoadData()
            {
                string localAppDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                string LauncherFolderPath = Path.Combine(localAppDataPath, "Launcher");
                string filePath = Path.Combine(LauncherFolderPath, FileName);

                if (File.Exists(filePath))
                {
                    try
                    {
                        string jsonData = File.ReadAllText(filePath);
                        return JsonConvert.DeserializeObject<DataModel>(jsonData);
                    }
                    catch (Exception ex)
                    {
                        // Handle deserialization error
                        Console.WriteLine($"Error deserializing data: {ex.Message}");
                    }
                }

                return null;
            }

            public class DataModel
            {
                public string Email { get; set; }
                public string Password { get; set; }
                public string Path { get; set; }
            }
        }

        private void Launch_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Window mainWindow = Window.GetWindow(this);
                string path69 = UpdateINI.ReadValue("Auth", "Path");
                if (path69 != "NONE")
                {
                    string exeFilePath = System.IO.Path.Join(path69, "FortniteGame\\Binaries\\Win64\\FortniteClient-Win64-Shipping.exe");

                    // Check if the file exists and its version matches
                    if (File.Exists(exeFilePath))
                    {
                        if (UpdateINI.ReadValue("Auth", "Email") == "NONE" || UpdateINI.ReadValue("Auth", "Password") == "NONE")
                        {
                            MessageBox.Show("Please Add Your Launcher Info In Settings");
                            return;
                        }

                        WebClient OMG = new WebClient();
                        string dllPath = Path.Combine(path69, "Engine\\Binaries\\ThirdParty\\NVIDIA\\NVaftermath\\Win64", "Cobalt.dll");
                        string consolePath = Path.Combine(path69, "Engine\\Binaries\\ThirdParty\\NVIDIA\\NVaftermath\\Win64", "Console.dll");
                        string memoryPath = Path.Combine(path69, "Engine\\Binaries\\ThirdParty\\NVIDIA\\NVaftermath\\Win64", "MemFixer.dll");
                        OMG.DownloadFile("https://apfelteesaft.com/cdn/epiccobalt", Path.Combine(path69, "Engine\\Binaries\\ThirdParty\\NVIDIA\\NVaftermath\\Win64", "Cobalt.dll")); //replace with your curl
                        OMG.DownloadFile("https://apfelteesaft.com/cdn/epicconsole", Path.Combine(path69, "Engine\\Binaries\\ThirdParty\\NVIDIA\\NVaftermath\\Win64", "Console.dll")); //replace with your curl
                        OMG.DownloadFile("https://apfelteesaft.com/cdn/MemoryLeakFixer", Path.Combine(path69, "Engine\\Binaries\\ThirdParty\\NVIDIA\\NVaftermath\\Win64", "MemFixer.dll")); //replace with your curl

                        mainWindow.WindowState = WindowState.Minimized;

                        int processId = PSBasics.Start(path69, "-epicapp=Fortnite -epicenv=Prod -epiclocale=en-us -epicportal -noeac -fromfl=be -fltoken=h1cdhchd10150221h130eB56 -skippatchcheck", UpdateINI.ReadValue("Auth", "Email"), UpdateINI.ReadValue("Auth", "Password"));
                        Thread.Sleep(10000);
                        Injector.Inject(processId, dllPath);
                        Injector.Inject(processId, memoryPath);
                        Thread.Sleep(15000);
                        Injector.Inject(processId, consolePath);

                        FakeAC.Start(path69, "FortniteClient-Win64-Shipping_BE.exe", $"-epicapp=Fortnite -epicenv=Prod -epiclocale=en-us -epicportal -noeac -fromfl=be -fltoken=h1cdhchd10150221h130eB56 -skippatchcheck", "r");
                        FakeAC.Start(path69, "FortniteLauncher.exe", $"-epicapp=Fortnite -epicenv=Prod -epiclocale=en-us -epicportal -noeac -fromfl=be -fltoken=h1cdhchd10150221h130eB56 -skippatchcheck", "dsf");

                        PSBasics._FortniteProcess.WaitForExit();

                        try
                        {
                            FakeAC._FNLauncherProcess.Close();
                            FakeAC._FNAntiCheatProcess.Close();
                            // File.Delete(Path.Combine(path69, "FortniteGame\\Content\\Paks", "pakchunk6004-WindowsClient.pak")); //delete custom pak
                            // File.Delete(Path.Combine(path69, "FortniteGame\\Content\\Paks", "pakchunk6004-WindowsClient.sig")); //delete sig file
                        }
                        catch (Exception ex)
                        {
                            mainWindow.WindowState = WindowState.Normal;
                            MessageBox.Show("There has been an error closing");
                        }
                    }
                    else
                    {
                        mainWindow.WindowState = WindowState.Normal;
                        MessageBox.Show("Error!");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("UNKNOWN ERROR");
            }
        }

        static bool IsFileVersionMatch(string filePath, Version expectedVersion)
        {
            try
            {
                FileVersionInfo fileVersionInfo = FileVersionInfo.GetVersionInfo(filePath);

                int fileMajorPart = fileVersionInfo.FileMajorPart;
                int fileMinorPart = fileVersionInfo.FileMinorPart;
                int fileBuildPart = fileVersionInfo.FileBuildPart;
                int filePrivatePart = fileVersionInfo.FilePrivatePart;

                return fileMajorPart == expectedVersion.Major &&
                       fileMinorPart == expectedVersion.Minor &&
                       fileBuildPart == expectedVersion.Build &&
                       filePrivatePart == expectedVersion.Revision;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
                return false;
            }
        }
    }
}
