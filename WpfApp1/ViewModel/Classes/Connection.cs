
namespace WpfApp1.ViewModel.Classes;

static internal class Connection
{
    public static IDbConnection? ConnectionName { get; private set; } = default;

    // Подключение к БД. Возвращает connection 
    static public IDbConnection CreateConnection()
    {
        string connectionString = "Data Source=DESKTOP-BED4UI8\\SQLEXPRESS;Initial Catalog=DB_Prakt;TrustServerCertificate=true;Integrated Security=true";
        ConnectionName = new SqlConnection(connectionString);
        ConnectionName.Open();
        return ConnectionName;
    }

    // Отключение от БД
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
