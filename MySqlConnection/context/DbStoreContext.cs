using MySql.Data.MySqlClient;

namespace MySqlConnectiondemo.context
{
    public class DbStoreContext
    {
        public string ConnectionString { get; set; }

        public DbStoreContext(string connectionString)
        {
            this.ConnectionString = connectionString;
        }

        //private MySqlConnection GetConnection()
        //{
        //    return new MySqlConnection(ConnectionString);
        //}
    }
}
