using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using WpfApp1.Models;
using WpfApp1.ViewModel.Interfaces;

namespace WpfApp1.ViewModel.Classes
{
    internal class DownloadsDraft : IDownloadsDraft
    {
        static public void SetupDrafts(object? sender, EventArgs e)
        {
            if (sender is  ComboBox comboBox) 
            {
                comboBox.Items.Clear();
                var db=Connection.CreateConnection();
                var list=db.Query<Draft>("SELECT * FROM dbo.ContractDraft").ToList();
                db.DropConnection();
                foreach (Draft item in list)
                {
                    comboBox.Items.Add(item.ContractName);
                }
            }
        }
    }
}
