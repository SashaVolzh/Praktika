using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using WpfApp1.Models;
using WpfApp1.ViewModel.Interfaces;

namespace WpfApp1.ViewModel.Classes
{
    internal class DownloadsFiles : IDownloadsFiles
    {
        static public List<File> listOfFiles { get; set; }
        static public void SetupFiles(object? sender, EventArgs e, ListBox listBox)
        {
            if(listOfFiles is not null) listOfFiles.Clear();
            listBox.Items.Clear();
            ComboBox comboBox = (ComboBox)sender;
            var db=Connection.CreateConnection();
            listOfFiles = db.Query<File>("SELECT dbo.EstimateFiles.* FROM dbo.ContractDraft2File " +
                    "Join dbo.EstimateFiles ON dbo.ContractDraft2File.FileID=dbo.EstimateFiles.ID " +
                    "Join dbo.ContractDraft On dbo.ContractDraft.DraftID=dbo.ContractDraft2File.DraftID Where ContractName=@Name", new { Name = comboBox?.SelectedValue }).ToList();
            db.DropConnection();
            foreach (File item in listOfFiles)
            {
                listBox.Items.Add(item.FileName);
            }
            listBox.SelectedIndex = 0;
            listBox.UpdateLayout();
        }
    }
}
