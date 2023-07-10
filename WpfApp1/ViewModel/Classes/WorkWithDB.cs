
using System.Collections.Generic;
using System.Windows.Input;
using WpfApp1.Models;

namespace WpfApp1.ViewModel.Classes;

internal class WorkWithDB : IWorkWithDB
{
    /// <summary>
    /// Сохранение изменений в БД
    /// </summary>
    static public void SaveIntoDB(object sender, RoutedEventArgs e, ComboBox comboBox, ListBox listbox)
    {
        if (comboBox.Text == "") return;
        List<Models.File> DBFiles;
        Guid draftId;
        using (var db = Connection.CreateConnection())
        {
            //Получение файлов из БД
            DBFiles = db.Query<Models.File>("SELECT dbo.EstimateFiles.* FROM dbo.ContractDraft2File " +
                "Join dbo.EstimateFiles ON dbo.ContractDraft2File.FileID=dbo.EstimateFiles.ID " +
                "Join dbo.ContractDraft On dbo.ContractDraft.DraftID=dbo.ContractDraft2File.DraftID Where ContractName=@Name", new { Name = comboBox?.SelectedValue }).ToList();
            draftId = db.Query<Guid>("SELECT DraftID FROM dbo.ContractDraft Where ContractName=@Name", new { Name = comboBox?.SelectedValue }).Last();
        }
        UpdateRecords(DBFiles, DownloadsFiles.listOfFiles, draftId);
        if (DBFiles.Count != 0)
            DeleteRecords(DBFiles, draftId);
        if (DownloadsFiles.listOfFiles.Count != 0)
            InsertRecords(DownloadsFiles.listOfFiles, draftId);
        DownloadsFiles.SetupFiles(comboBox, e, listbox);
    }
    /// <summary>
    /// Обновление записей в БД
    /// </summary>
    static private void UpdateRecords(List<Models.File> DBFiles, List<Models.File> filelist, Guid draftId)
    {
        using (var db = Connection.CreateConnection())
        {
            for (int i = 0; i < DBFiles.Count; i++)
            {
                if (filelist.Any(x => x.FileName.Equals(DBFiles[i].FileName)))
                {
                    int index = filelist.FindIndex(x => x.FileName.Equals(DBFiles[i].FileName));
                    if (!filelist[index].FileData.ByteEquals(DBFiles[i].FileData))
                    {
                        string sqlSend = "Insert Into dbo.EstimateFiles (FileSize, FileName, FileExtension, FileData, ID, UploadedBy, UploadedAt) VALUES (@FileSize, @FileName, @FileExtension, @FileData, default, @UploadedBy, @UploadedAt)";
                        db.Execute(sqlSend, new { FileSize = filelist[index].FileSize, FileName = filelist[index].FileName, FileExtension = filelist[index].FileExtension, FileData = filelist[index].FileData, UploadedBy = filelist[index].UploadedBy, UploadedAt = filelist[index].UploadedAt });
                        var fileId = db.Query<Guid>("SELECT Top(1) ID from dbo.EstimateFiles Where FileName=@FileName Order By UploadedAt DESC", new { FileName = filelist[index].FileName }).ToList();
                        sqlSend = "Insert into dbo.ContractDraft2File (DraftID, FileID) VALUES (@DraftID, @FileID)";
                        db.Execute(sqlSend, new { DraftID = draftId, FileID = fileId });
                        sqlSend = "Delete From dbo.ContractDraft2File WHERE DraftID=@DraftID and FileID= @FileID";
                        db.Execute(sqlSend, new { DraftID = draftId, FileID = DBFiles[i].ID });
                        sqlSend = "Insert Into dbo.History Values (default, @ChangeDate, @ChangeType, @ChangeBy, @DraftId, @OldFileID, @NewFileID)";
                        db.Execute(sqlSend, new { ChangeDate = DateTime.Now, ChangeType = (int)Status.Update, ChangeBy = "Лыганов Алексей", DraftId = draftId, OldFileID = DBFiles[i].ID, NewFileID = fileId });

                    }
                    DBFiles.RemoveAt(i);
                    i--;
                    filelist.RemoveAt(index);
                }
            }
        }
    }
    /// <summary>
    /// Удаление записей из БД
    /// </summary>
    static private void DeleteRecords(List<Models.File> DBFiles, Guid draftId)
    {
        using (var db = Connection.CreateConnection())
        {
            foreach (var file in DBFiles)
            {
                string sqlSend = "DELETE FROM dbo.ContractDraft2File WHERE FileID=@FileID";
                db.Execute(sqlSend, new { FileID = file.ID });
                sqlSend = "Insert Into dbo.History (HistoryId, ChangeDate, ChangeType, ChangeBy, DraftId, OldFileID) Values (default, @ChangeDate, @ChangeType, @ChangeBy, @DraftId, @OldFileID)";
                db.Execute(sqlSend, new { ChangeDate = DateTime.Now, ChangeType = (int)Status.Delete, ChangeBy = "Лыганов Алексей", DraftId = draftId, OldFileID = file.ID });
            }
        }
    }
    /// <summary>
    /// Вставка записей в БД 
    /// </summary>
    static private void InsertRecords(List<Models.File> filelist, Guid draftId)
    {
        using (var db = Connection.CreateConnection())
        {
            foreach (var file in filelist)
            {
                string sqlSend = "Insert into dbo.EstimateFiles (FileSize, FileName, FileExtension, FileData, ID, UploadedBy, UploadedAt) VALUES (@FileSize, @FileName, @FileExtension, @FileData, default, @UploadedBy, @UploadedAt)";
                db.Execute(sqlSend, new { FileSize = file.FileSize, FileName = file.FileName, FileExtension = file.FileExtension, FileData = file.FileData, UploadedBy = "Лыганов Алексей", UploadedAt = file.UploadedAt });
                var fileId = db.Query<Guid>("SELECT ID from dbo.EstimateFiles Where FileName=@FileName", new { FileName = file.FileName }).Last();
                sqlSend = "Insert into dbo.ContractDraft2File (DraftID, FileID) VALUES (@DraftID, @FileID)";
                db.Execute(sqlSend, new { DraftID = draftId, FileID = fileId });
                sqlSend = "Insert Into dbo.History (HistoryId, ChangeDate, ChangeType, ChangeBy, DraftId, NewFileID) Values (default, @ChangeDate, @ChangeType, @ChangeBy, @DraftId, @NewFileID)";
                db.Execute(sqlSend, new { ChangeDate = DateTime.Now, ChangeType = (int)Status.Insert, ChangeBy = "Лыганов Алексей", DraftId = draftId, NewFileID = fileId });
            }
        }
    }
}

