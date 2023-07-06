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
        }

        private void SetupFiles(object sender, RoutedEventArgs e)
        {
            ViewModel.Classes.DownloadsFiles.SetupFiles(sender, e, Files);
        }

        private void AddFile(object sender, RoutedEventArgs e)
        {
            ViewModel.Classes.WorkWithFiles.AddFile(sender, e, Files);
        }
    }
}
