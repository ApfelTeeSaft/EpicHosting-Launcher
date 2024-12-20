﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WindowsAPICodePack.Dialogs;
using WpfApp6.Services;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace WpfApp6.Pages
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings : UserControl
    {
        public Settings()
        {
            InitializeComponent();
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            //Regex r = new Regex(@"^[a-zA-Z@]+$");
            //if (!r.IsMatch(e.Text))
            //e.Handled = true;
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            CommonOpenFileDialog commonOpenFileDialog = new CommonOpenFileDialog();
            commonOpenFileDialog.IsFolderPicker = true;
            commonOpenFileDialog.Title = "Select A Fortnite Build";
            commonOpenFileDialog.Multiselect = false;
            CommonFileDialogResult commonFileDialogResult = commonOpenFileDialog.ShowDialog();


            bool flag = commonFileDialogResult == CommonFileDialogResult.Ok;
            if (flag)
            {
                if (File.Exists(System.IO.Path.Join(commonOpenFileDialog.FileName, "FortniteGame\\Binaries\\Win64\\FortniteClient-Win64-Shipping.exe")))
                {
                    this.PathBox.Text = commonOpenFileDialog.FileName;
                }
                else
                {
                    MessageBox.Show("Please make sure that your the folder contains FortniteGame and Engine In");

                }
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            string email = EmailBox.Text;
            string password = PasswordBox.Password;
            string path = PathBox.Text;
            UpdateINI.WriteToConfig("Auth", "Path", PathBox.Text);
            var data = new
            {
                Email = email,
                Password = password,
                Path = path
            };
            string jsonData = JsonConvert.SerializeObject(data);
            string localAppDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string LauncherFolderPath = System.IO.Path.Combine(localAppDataPath, "Launcher");
            Directory.CreateDirectory(LauncherFolderPath);
            string filePath = System.IO.Path.Combine(LauncherFolderPath, "data.json");
            File.WriteAllText(filePath, jsonData);
            MessageBox.Show("Save Successful, you don't have to enter your login creds again!");
        }

        private void PathBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateINI.WriteToConfig("Auth", "Path", PathBox.Text); // Updates Live OMG!
        }
    }
}
