using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using WpfApp1.ViewModel.Interfaces;
using WpfApp1.Models;
using System.Collections;
using WpfApp1.ViewModel.Enum;

namespace WpfApp1.ViewModel.Classes
{
    internal class WorkWithDB : IWorkWithDB
    {
         static public void SaveIntoDB(object sender, RoutedEventArgs e, ComboBox comboBox)
        {
            if (comboBox.Text == "") return;

            var db = Connection.CreateConnection();
            var DBFiles = db.Query<File>("SELECT dbo.EstimateFiles.* FROM dbo.ContractDraft2File " +
                    "Join dbo.EstimateFiles ON dbo.ContractDraft2File.FileID=dbo.EstimateFiles.ID " +
                    "Join dbo.ContractDraft On dbo.ContractDraft.DraftID=dbo.ContractDraft2File.DraftID Where ContractName=@Name", new { Name = comboBox?.SelectedValue }).ToList();
            for (int i = 0; i< DBFiles.Count; i++)
            {
                if (DownloadsFiles.listOfFiles.Any(x => x.FileName.Equals(DBFiles[i].FileName)))
                {
                    int index = DownloadsFiles.listOfFiles.FindIndex(x => x.FileName.Equals(DBFiles[i].FileName));
                    if(!DownloadsFiles.listOfFiles[index].FileData.ByteEquals(DBFiles[i].FileData))
                    {
                        string sqlSend = "Update dbo.EstimateFiles Set FileData=@FileData, FileSize=@FileSize, UploadedBy=@UploadedBy, UploadedAt=@UploadedAt Where ID=@ID";
                        db.Execute(sqlSend, new { FileData = DownloadsFiles.listOfFiles[index].FileData, FileSize = DownloadsFiles.listOfFiles[index].FileSize, UploadedBy = DownloadsFiles.listOfFiles[index].UploadedBy, UploadedAt = DownloadsFiles.listOfFiles[index].UploadedAt, ID = DBFiles[i].ID });
                        sqlSend = "Insert Into dbo.History Values (default, @ChangeDate, @ChangeType, @ChangeBy, @DraftId)";
                        db.Execute(sqlSend, new {ChangeDate = DateTime.Now, ChangeType = Status.Обновление.ToString(), ChangeBy = "Лыганов Алексей", DraftId = DBFiles[i].ID });
                        
                    }
                    DBFiles.RemoveAt(i);
                    i--;
                    DownloadsFiles.listOfFiles.RemoveAt(index);
                }
            }
            var draftId = db.Query<Guid>("SELECT DraftID FROM dbo.ContractDraft Where ContractName=@Name", new { Name = comboBox?.SelectedValue }).ToList();
            foreach (var file in DBFiles)
            {
                string sqlSend = "DELETE FROM dbo.ContractDraft2File WHERE FileID=@FileID";
                db.Execute( sqlSend, new { FileID = file.ID });
                sqlSend = "Insert Into dbo.History (HistoryId, ChangeDate, ChangeType, ChangeBy, DraftId) Values (default, @ChangeDate, @ChangeType, @ChangeBy, @DraftId)";
                db.Execute(sqlSend, new { ChangeDate = DateTime.Now, ChangeType = Status.Удаление.ToString(), ChangeBy = "Лыганов Алексей", DraftId = draftId });
            }
            foreach(var file in DownloadsFiles.listOfFiles)
            {
                string sqlSend = "Insert into dbo.EstimateFiles (FileSize, FileName, FileExtension, FileData, ID, UploadedBy, UploadedAt) VALUES (@FileSize, @FileName, @FileExtension, @FileData, default, @UploadedBy, @UploadedAt)";
                db.Execute(sqlSend, new { FileSize = file.FileSize, FileName = file.FileName, FileExtension = file.FileExtension, FileData = file.FileData, UploadedBy = "Лыганов Алексей", UploadedAt = file.UploadedAt });
                var fileId = db.Query<Guid>("SELECT ID from dbo.EstimateFiles Where FileName=@FileName", new { FileName = file.FileName }).ToList();
                sqlSend = "Insert into dbo.ContractDraft2File (DraftID, FileID) VALUES (@DraftID, @FileID)";
                db.Execute(sqlSend, new { DraftID = draftId, FileID = fileId});
                sqlSend = "Insert Into dbo.History (HistoryId, ChangeDate, ChangeType, ChangeBy, DraftId) Values (default, @ChangeDate, @ChangeType, @ChangeBy, @DraftId)";
                db.Execute(sqlSend, new { ChangeDate = DateTime.Now, ChangeType = Status.Добавление.ToString(), ChangeBy = "Лыганов Алексей", DraftId = draftId });
            }
            db.DropConnection();
        }
    }
   
}
