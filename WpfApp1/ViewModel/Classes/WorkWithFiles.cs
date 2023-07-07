

namespace WpfApp1.ViewModel.Classes;

class WorkWithFiles : IWorkWithFiles
{

    //Добавление файла в форму из ПК
    static public void AddFile(object sender, RoutedEventArgs e, ListBox space, ComboBox comboBox)
    {
        if (comboBox.SelectedIndex == -1) return;
        var openFileDialog = new OpenFileDialog();
        openFileDialog.Filter= "PDF Files (*.pdf)|*.pdf|Word Documents (*.docx)|*.docx|(*.doc)|*.doc";
        if (openFileDialog.ShowDialog() == true)
        {
            string selectedFilePath = openFileDialog.FileName;
            Models.File file = new Models.File();
            var fileInfo=new FileInfo(selectedFilePath);
            file.FileName = fileInfo.Name;
            file.FileSize = (int)fileInfo.Length;
            file.FileExtension = fileInfo.Extension;
            file.FileData = System.IO.File.ReadAllBytes(selectedFilePath);
            file.UploadedBy = "Лыганов Алексей";
            file.UploadedAt = DateTime.Now;
            DownloadsFiles.listOfFiles.Add(file);
            space.Items.Add(file.FileName);
        }
    }

    //Скачивание всех файлов из формы на ПК
    static public void DownloadOnPC(object sender, RoutedEventArgs e)
    {
        if (DownloadsFiles.listOfFiles.IsNullOrEmpty()) { return; }
        foreach (Models.File item in DownloadsFiles.listOfFiles)
        {
            var data_document = item.FileData;
            string downloadFolder = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) +
       "\\Downloads";
          string  filePath = Path.Combine(downloadFolder, item.FileName);
            System.IO.File.Create(filePath).Close();
            System.IO.File.WriteAllBytes(filePath, data_document);
        }
        Process.Start("explorer.exe", Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) +
       "\\Downloads");
    }

    // Сохранение одного файла на ПК
    static public void SaveFile(object sender, RoutedEventArgs e, ListBox listBox,ComboBox comboBox)
    {
        if (DownloadsFiles.listOfFiles.IsNullOrEmpty() || comboBox.SelectedIndex==-1) { return; }
        Models.File item = DownloadsFiles.listOfFiles.ElementAt(listBox.SelectedIndex);
        var data_document = item.FileData;
        string downloadFolder = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Downloads";
        string filePath = Path.Combine(downloadFolder, item.FileName);
        System.IO.File.Create(filePath).Close();
        System.IO.File.WriteAllBytes(filePath, data_document);
        Process.Start("explorer.exe", Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) +
       "\\Downloads");
    }

    // Удаление одного файла из формы
    static public void DeleteFile(object sender, RoutedEventArgs e, ListBox listBox, ComboBox comboBox)
    {
        if (DownloadsFiles.listOfFiles.IsNullOrEmpty() || comboBox.SelectedIndex == -1) { return; }
        DownloadsFiles.listOfFiles.RemoveAt(listBox.SelectedIndex);
        listBox.Items.RemoveAt(listBox.SelectedIndex);
    }
}
