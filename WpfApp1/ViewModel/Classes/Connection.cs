using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics.Arm;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.ViewModel.Classes
{
    static internal class Connection
    {
        public static IDbConnection? ConnectionName { get; private set; } = default;

        static public IDbConnection CreateConnection()
        {
            string connectionString = "Data Source=DESKTOP-CU79IVQ\\SQLEXPRESS;Initial Catalog=DB_Prakt;TrustServerCertificate=true;Integrated Security=true";
            ConnectionName = new SqlConnection(connectionString);
            ConnectionName.Open();
            return ConnectionName;
        }

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
}
