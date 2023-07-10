

namespace WpfApp1.ViewModel.Classes;

internal class DownloadsFiles : IDownloadsFiles
{
    /// <summary>
    /// Хранит список файлов в форме
    /// </summary>
    static public List<Models.File> listOfFiles { get; set; }= new List<Models.File>();
    /// <summary>
    /// Подгружает файлы в форму
    /// </summary>
    static public void SetupFiles(object? sender, EventArgs e, ListBox listBox)
    {
        if(listOfFiles is not null) listOfFiles.Clear();
        listBox.Items.Clear();
        ComboBox? comboBox = sender as ComboBox;
        var db=Connection.CreateConnection();
        listOfFiles = db.Query<Models.File>("SELECT dbo.EstimateFiles.* FROM dbo.ContractDraft2File " +
                "Join dbo.EstimateFiles ON dbo.ContractDraft2File.FileID=dbo.EstimateFiles.ID " +
                "Join dbo.ContractDraft On dbo.ContractDraft.DraftID=dbo.ContractDraft2File.DraftID Where ContractName=@Name", new { Name = comboBox?.SelectedValue }).ToList();
        db.DropConnection();
        foreach (Models.File item in listOfFiles)
        {
            listBox.Items.Add(item.FileName);
        }
        listBox.SelectedIndex = 0;
        listBox.UpdateLayout();
    }
}
