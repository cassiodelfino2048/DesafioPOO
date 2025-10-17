using MySql.Data.MySqlClient;

public class Database
{
    private string connectionString = "Server=localhost;Database=corretora;User=root;Password=senha;";

    public MySqlConnection GetConnection()
    {
        return new MySqlConnection(connectionString);
    }
}