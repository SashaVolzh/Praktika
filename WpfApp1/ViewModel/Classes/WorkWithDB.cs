
namespace WpfApp1.ViewModel.Classes;

internal class WorkWithDB : IWorkWithDB
{

    //Сохранение в БД
     static public void SaveIntoDB(object sender, RoutedEventArgs e, ComboBox comboBox, ListBox listbox)
    {
        if (comboBox.Text == "") return;

        var db = Connection.CreateConnection();
        //Получение файлов из БД
        var DBFiles = db.Query<Models.File>("SELECT dbo.EstimateFiles.* FROM dbo.ContractDraft2File " +
                "Join dbo.EstimateFiles ON dbo.ContractDraft2File.FileID=dbo.EstimateFiles.ID " +
                "Join dbo.ContractDraft On dbo.ContractDraft.DraftID=dbo.ContractDraft2File.DraftID Where ContractName=@Name", new { Name = comboBox?.SelectedValue }).ToList();
        // Получение GUID договора из БД 
        var draftId = db.Query<Guid>("SELECT DraftID FROM dbo.ContractDraft Where ContractName=@Name", new { Name = comboBox?.SelectedValue }).ToList().Last();
        //Цикл по проверке обновления файлов 
        for (int i = 0; i< DBFiles.Count; i++)
        {
            if (DownloadsFiles.listOfFiles.Any(x => x.FileName.Equals(DBFiles[i].FileName)))
            {
                int index = DownloadsFiles.listOfFiles.FindIndex(x => x.FileName.Equals(DBFiles[i].FileName));
                if(!DownloadsFiles.listOfFiles[index].FileData.ByteEquals(DBFiles[i].FileData))
                {
                    string sqlSend = "Insert Into dbo.EstimateFiles (FileSize, FileName, FileExtension, FileData, ID, UploadedBy, UploadedAt) VALUES (@FileSize, @FileName, @FileExtension, @FileData, default, @UploadedBy, @UploadedAt)";
                    db.Execute(sqlSend, new { FileSize = DownloadsFiles.listOfFiles[index].FileSize, FileName = DownloadsFiles.listOfFiles[index].FileName, FileExtension= DownloadsFiles.listOfFiles[index].FileExtension, FileData = DownloadsFiles.listOfFiles[index].FileData, UploadedBy = DownloadsFiles.listOfFiles[index].UploadedBy, UploadedAt = DownloadsFiles.listOfFiles[index].UploadedAt});
                    var fileId = db.Query<Guid>("SELECT Top(1) ID from dbo.EstimateFiles Where FileName=@FileName Order By UploadedAt DESC", new { FileName = DownloadsFiles.listOfFiles[index].FileName }).ToList();
                    sqlSend = "Insert into dbo.ContractDraft2File (DraftID, FileID) VALUES (@DraftID, @FileID)";
                    db.Execute(sqlSend, new { DraftID = draftId, FileID = fileId });
                    sqlSend = "Delete From dbo.ContractDraft2File WHERE DraftID=@DraftID and FileID= @FileID";
                    db.Execute(sqlSend, new { DraftID = draftId, FileID = DBFiles[i].ID });
                    sqlSend = "Insert Into dbo.History Values (default, @ChangeDate, @ChangeType, @ChangeBy, @DraftId, @OldFileID, @NewFileID)";
                    db.Execute(sqlSend, new {ChangeDate = DateTime.Now, ChangeType = Status.Обновление.ToString(), ChangeBy = "Лыганов Алексей", DraftId = draftId, OldFileID = DBFiles[i].ID, NewFileID = fileId });
                    
                }
                DBFiles.RemoveAt(i);
                i--;
                DownloadsFiles.listOfFiles.RemoveAt(index);
            }
        }
        //Цикл по удалению связей между договором и файлом из БД 
        foreach (var file in DBFiles)
        {
            string sqlSend = "DELETE FROM dbo.ContractDraft2File WHERE FileID=@FileID";
            db.Execute( sqlSend, new { FileID = file.ID });
            sqlSend = "Insert Into dbo.History (HistoryId, ChangeDate, ChangeType, ChangeBy, DraftId, OldFileID) Values (default, @ChangeDate, @ChangeType, @ChangeBy, @DraftId, @OldFileID)";
            db.Execute(sqlSend, new { ChangeDate = DateTime.Now, ChangeType = Status.Удаление.ToString(), ChangeBy = "Лыганов Алексей", DraftId = draftId, OldFileID= file.ID });
        }
        //Цикл по добавлению файла в БД
        foreach (var file in DownloadsFiles.listOfFiles)
        {
            string sqlSend = "Insert into dbo.EstimateFiles (FileSize, FileName, FileExtension, FileData, ID, UploadedBy, UploadedAt) VALUES (@FileSize, @FileName, @FileExtension, @FileData, default, @UploadedBy, @UploadedAt)";
            db.Execute(sqlSend, new { FileSize = file.FileSize, FileName = file.FileName, FileExtension = file.FileExtension, FileData = file.FileData, UploadedBy = "Лыганов Алексей", UploadedAt = file.UploadedAt });
            var fileId = db.Query<Guid>("SELECT ID from dbo.EstimateFiles Where FileName=@FileName", new { FileName = file.FileName }).ToList().Last();
            sqlSend = "Insert into dbo.ContractDraft2File (DraftID, FileID) VALUES (@DraftID, @FileID)";
            db.Execute(sqlSend, new { DraftID = draftId, FileID = fileId});
            sqlSend = "Insert Into dbo.History (HistoryId, ChangeDate, ChangeType, ChangeBy, DraftId, NewFileID) Values (default, @ChangeDate, @ChangeType, @ChangeBy, @DraftId, @NewFileID)";
            db.Execute(sqlSend, new { ChangeDate = DateTime.Now, ChangeType = Status.Добавление.ToString(), ChangeBy = "Лыганов Алексей", DraftId = draftId, NewFileID=fileId });
        }
        DownloadsFiles.SetupFiles(comboBox, e, listbox);
        db.DropConnection();
    }
}

