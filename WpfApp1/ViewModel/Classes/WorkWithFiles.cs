using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.ViewModel.Interfaces;
using Microsoft.Win32;
using System.Diagnostics;
using System.IO;
using Microsoft.IdentityModel.Tokens;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WpfApp1.ViewModel.Classes
{
    class WorkWithFiles : IWorkWithFiles
    {
        static public void AddFile(object sender, RoutedEventArgs e, ListBox space)
        {
            var openFileDialog = new OpenFileDialog();
            openFileDialog.Filter= "PDF Files (*.pdf)|*.pdf|Word Documents (*.docx)|*.docx";
            if (openFileDialog.ShowDialog() == true)
            {
                string selectedFilePath = openFileDialog.FileName;
                Models.File file = new Models.File();
                var fileInfo=new FileInfo(selectedFilePath);
                file.FileName = fileInfo.Name;
                file.FileSize = (int)fileInfo.Length;
                file.FileExtension = fileInfo.Extension;
                file.FileData = File.ReadAllBytes(selectedFilePath);
                file.UploadedBy = "Лыганов Алексей";
                file.UploadedAt = DateTime.Now;
                DownloadsFiles.listOfFiles.Add(file);
                space.Items.Add(file.FileName);
            }
        }

        static public void DownloadOnPC(object sender, RoutedEventArgs e)
        {
            if (DownloadsFiles.listOfFiles.IsNullOrEmpty()) { return; }
            foreach (Models.File item in DownloadsFiles.listOfFiles)
            {
                var data_document = item.FileData;
                string downloadFolder = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) +
           "\\Downloads";
              string  filePath = Path.Combine(downloadFolder, item.FileName);
                File.Create(filePath).Close();
                File.WriteAllBytes(filePath, data_document);
            }
            Process.Start("explorer.exe", Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) +
           "\\Downloads");
        }

        static public void SaveFile(object sender, RoutedEventArgs e, ListBox listBox)
        {
            if (DownloadsFiles.listOfFiles.IsNullOrEmpty()) { return; }
            Models.File item = DownloadsFiles.listOfFiles.ElementAt(listBox.SelectedIndex);
            var data_document = item.FileData;
            string downloadFolder = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Downloads";
            string filePath = Path.Combine(downloadFolder, item.FileName);
            File.Create(filePath).Close();
            File.WriteAllBytes(filePath, data_document);
            Process.Start("explorer.exe", Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) +
           "\\Downloads");
        }

        static public void DeleteFile(object sender, RoutedEventArgs e, ListBox listBox)
        {
            if (DownloadsFiles.listOfFiles.IsNullOrEmpty()) { return; }
            DownloadsFiles.listOfFiles.RemoveAt(listBox.SelectedIndex);
            listBox.Items.RemoveAt(listBox.SelectedIndex);
        }
    }
}
