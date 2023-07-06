using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using WpfApp1.Models;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Drafts.Loaded += ViewModel.Classes.DownloadsDraft.SetupDrafts;
            Drafts.SelectionChanged += SetupFiles;
            AddFiles.Click += AddFile;
            DownloadFiles.Click += ViewModel.Classes.WorkWithFiles.DownloadOnPC;
            Files.MouseRightButtonDown += OpenContexMenu;
            SaveItem.Click += SaveFile;
            DeleteItem.Click += DeleteFile;
            SaveDB.Click += SaveIntoDB;
        }

        private void SetupFiles(object sender, RoutedEventArgs e)
        {
            ViewModel.Classes.DownloadsFiles.SetupFiles(sender, e, Files);
        }

        private void AddFile(object sender, RoutedEventArgs e)
        {
            ViewModel.Classes.WorkWithFiles.AddFile(sender, e, Files);
        }
        private void OpenContexMenu(object sender, RoutedEventArgs e)
        {
            if (Files.SelectedItem == null) return;
            ContextMenu contextMenu = Files.ContextMenu;
            if (contextMenu != null)
            {
                contextMenu.PlacementTarget = Files;
                contextMenu.IsOpen = true;
            }
        }
        private void SaveIntoDB(object sender, RoutedEventArgs e)
        {
            ViewModel.Classes.WorkWithDB.SaveIntoDB(sender, e, Drafts);
        }
        private void SaveFile(object sender, RoutedEventArgs e)
        {
            ViewModel.Classes.WorkWithFiles.SaveFile(sender, e, Files);
        }
        private void DeleteFile(object sender, RoutedEventArgs e)
        {
            ViewModel.Classes.WorkWithFiles.DeleteFile(sender, e, Files);
        }

    }
}
