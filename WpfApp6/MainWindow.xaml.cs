﻿using ModernWpf.Controls;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using System.Windows.Media;
using WpfApp6.Pages;
using ModernWpf.Media.Animation;
using System.Net.NetworkInformation;
using System.IO;
using WpfApp6.Services;
using Launcher.Windows;

namespace WpfApp6
{
    public partial class MainWindow : Window
    {
        // fakeLogin fakeLogin = new fakeLogin();
        Home home = new Home();
        Settings settings = new Settings();
        Downloader download;
        Loading loading = new Loading();
        AnnouncementsPage announcementsPage = new AnnouncementsPage(); // Add AnnouncementsPage instance
        DownloadStateService downloadStateService = new DownloadStateService(); // Create a singleton instance of DownloadStateService

        public MainWindow()
        {
            // fakeLogin.Activate();
            InitializeComponent();

            // Create a singleton instance of DownloadStateService and add it to App resources
            // NavView.Background = new SolidColorBrush(Colors.Blue);
            downloadStateService = new DownloadStateService();
            App.Current.Resources["DownloadStateService"] = downloadStateService;

            ContentFrame.Navigate(loading);

            // Check server status and Version
            
            CheckServerStatusAndVersion();
        }

        private void NavView_Loaded(object sender, RoutedEventArgs e)
        {
            ContentFrame.Navigate(home);
        }

        private bool CheckInternetConnection()
        {
            try
            {
                Ping ping = new Ping();
                PingReply reply = ping.Send("www.google.com", 3000);
                return reply != null && reply.Status == IPStatus.Success;
            }
            catch
            {
                return false;
            }
        }

        private void NavView_SelectionChanged(ModernWpf.Controls.NavigationView sender, ModernWpf.Controls.NavigationViewSelectionChangedEventArgs args)
        {
            if (args.IsSettingsSelected)
            {
                ContentFrame.Navigate(settings);
            }
            else
            {
                NavigationViewItem item = args.SelectedItem as NavigationViewItem;

                if (item != null)
                {
                    if (item.Tag != null)
                    {
                        if (item.Tag.ToString() == "Home")
                        {
                            ContentFrame.Navigate(home);
                        }
                        else if (item.Tag.ToString() == "Downloader")
                        {
                            // Provide the existing singleton instance of DownloadStateService
                            download = new Downloader();
                            ContentFrame.Navigate(download);
                        }
                        else if (item.Tag.ToString() == "Announcements")
                        {
                            ContentFrame.Navigate(announcementsPage);
                        }
                    }
                }
            }
        }

        private async void CheckServerStatusAndVersion()
        {
            try
            {

                // Check version
                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage response = await client.GetAsync("http://26.158.133.248:5000/api/check"); //replace with your actual server ip
                    HttpResponseMessage versionResponse = await client.GetAsync("http://26.158.133.248:5000/api/version");

                    if (response.IsSuccessStatusCode)
                    {
                        string jsonString = await versionResponse.Content.ReadAsStringAsync();

                        // Parse the JSON string
                        var json = System.Text.Json.JsonDocument.Parse(jsonString);
                        var version = json.RootElement.GetProperty("version").GetString();

                        if (version.Equals("0.3", StringComparison.InvariantCulture))
                        {
                            // Continue with the rest of the code
                            ContentFrame.Navigate(home);
                        }
                        else
                        {
                            MessageBox.Show($"Please Update The Launcher to {version}\nVisit the Discord!");
                            Close();
                        }
                    }
                    else
                    {
                        MessageBox.Show($"Please connect the the epichosting radmin server!\r\nUser : epichosting\r\nPassword : epichosting \r\n", "Radmin not Detected! ", MessageBoxButton.OK, MessageBoxImage.Error);
                        Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Please connect the the epichosting radmin server!\r\nUser : epichosting\r\nPassword : epichosting \r\n", "Radmin not Detected! ", MessageBoxButton.OK, MessageBoxImage.Error);
                Close();
            }
        }

        private async Task CheckUrlEndpoint(string url)
        {
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();

                    if (content.Equals("Denied", StringComparison.OrdinalIgnoreCase))
                    {
                        MessageBox.Show("Server Maintenance", "Access Denied");
                        Close();
                    }
                    // No need to check for "Allowed" explicitly, as it will continue with the execution
                }
                else
                {
                    MessageBox.Show($"Failed to check the URL. Status code: {response.StatusCode}");
                    Close();
                }
            }
        }

        private void ContentFrame_NavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page."); // Never TBH
        }
    }
}