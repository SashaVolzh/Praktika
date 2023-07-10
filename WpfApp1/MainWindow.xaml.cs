

namespace WpfApp1;

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
        ViewModel.Classes.WorkWithFiles.AddFile(sender, e, Files, Drafts);
    }
    private void OpenContexMenu(object sender, RoutedEventArgs e)
    {
        if (Files.SelectedIndex == -1) return;
        ContextMenu contextMenu = Files.ContextMenu;
        if (contextMenu != null)
        {
            contextMenu.PlacementTarget = Files;
            contextMenu.IsOpen = true;
        }
    }
    private void SaveIntoDB(object sender, RoutedEventArgs e)
    {
        ViewModel.Classes.WorkWithDB.SaveIntoDB(sender, e, Drafts, Files);
    }
    private void SaveFile(object sender, RoutedEventArgs e)
    {
        ViewModel.Classes.WorkWithFiles.SaveFile(sender, e, Files,Drafts);
    }
    private void DeleteFile(object sender, RoutedEventArgs e)
    {
        ViewModel.Classes.WorkWithFiles.DeleteFile(sender, e, Files, Drafts);
    }

}
