
namespace WpfApp1.ViewModel.Classes;

static internal class Connection
{
    public static IDbConnection? ConnectionName { get; private set; } = default;
    /// <summary>
    /// Подключение к БД. Возвращает connection 
    /// </summary>
    static public IDbConnection CreateConnection()
    {
        string connectionString = "Data Source=DESKTOP-CU79IVQ\\SQLEXPRESS;Initial Catalog=DB_Prakt;TrustServerCertificate=true;Integrated Security=true";
        ConnectionName = new SqlConnection(connectionString);
        ConnectionName.Open();
        return ConnectionName;
    }
    /// <summary>
    /// Отключение от БД
    /// </summary>
    static public void DropConnection(this IDbConnection db)
    {
        ConnectionName = db;
        if (db != null) 
        {
            db.Close();
            db.Dispose();
        }
    }
}
