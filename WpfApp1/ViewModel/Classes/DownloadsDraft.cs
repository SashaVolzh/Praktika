
namespace WpfApp1.ViewModel.Classes;

internal class DownloadsDraft : IDownloadsDraft
{
    /// <summary>
    /// Подгружает договоры в форму
    /// </summary>
    static public void SetupDrafts(object? sender, EventArgs e)
    {
        if (sender is  ComboBox comboBox) 
        {
            comboBox.Items.Clear();
            List<Draft> list;
            using (var db = Connection.CreateConnection())
            {
                list = db.Query<Draft>("SELECT * FROM dbo.ContractDraft").ToList();
            }
            foreach (Draft item in list)
            {
                comboBox.Items.Add(item.ContractName);
            }
        }
    }
}
