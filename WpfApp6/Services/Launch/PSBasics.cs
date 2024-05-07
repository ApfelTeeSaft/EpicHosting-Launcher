using LauncherApplicationV2.Services;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using WpfApp6.Services.Launch;

namespace LauncherApplicationV2.Services
{
    public static class PSBasics
    {
        public static Process _FortniteProcess;

        public static int Start(string PATH, string args, string Email, string Exchange)
        {
            if (Exchange == null)
            {
                MessageBox.Show("Sorry, make sure you put your Lunar Login");
                Application.Current.Shutdown();
            }

            if (File.Exists(Path.Combine(PATH, "FortniteGame\\Binaries\\Win64\\", "FortniteClient-Win64-Shipping.exe")))
            {
                if (_FortniteProcess != null && !_FortniteProcess.HasExited)
                {
                    // Fortnite process is already running, return the existing process ID
                    return _FortniteProcess.Id;
                }

                _FortniteProcess = new Process()
                {
                    StartInfo = new ProcessStartInfo()
                    {
                        Arguments = $"-AUTH_LOGIN={Email} -AUTH_PASSWORD={Exchange} -AUTH_TYPE=epic " + args,
                        FileName = Path.Combine(PATH, "FortniteGame\\Binaries\\Win64\\", "FortniteClient-Win64-Shipping.exe")
                    },
                    EnableRaisingEvents = true
                };

                _FortniteProcess.Exited += new EventHandler(OnFortniteExit);
                _FortniteProcess.Start();

                return _FortniteProcess?.Id ?? -1; // Return the process ID if available, otherwise -1
            }

            return -1; // Return -1 if the file does not exist
        }

        public static void OnFortniteExit(object sender, EventArgs e)
        {
            Process fortniteProcess = _FortniteProcess;
            if (fortniteProcess != null && fortniteProcess.HasExited)
            {
                _FortniteProcess = null;
            }
            FakeAC._FNLauncherProcess?.Kill();
            FakeAC._FNAntiCheatProcess?.Kill();
        }
    }
}