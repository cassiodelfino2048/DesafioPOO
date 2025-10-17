using MySql.Data.MySqlClient;

namespace DesafioPOO.Models
{
    public class DataBase
    {
        private string connectionString = "server=localhost;user=root;password=root;database=corretora";

        public void SalvarImovel(Imovel imovel)
        {
            using (var conexao = new MySqlConnection(connectionString))
            {
                conexao.Open();
                string query = "INSERT INTO imoveis (tipo, endereco, numero, alugado, nome_proprietario, telefone, cpf) " +
                               "VALUES (@tipo, @endereco, @numero, @alugado, @nome, @telefone, @cpf)";

                using (var cmd = new MySqlCommand(query, conexao))
                {
                    cmd.Parameters.AddWithValue("@tipo", imovel is Casa ? "casa" : "apartamento");
                    cmd.Parameters.AddWithValue("@endereco", imovel.GetEndereco());
                    cmd.Parameters.AddWithValue("@numero", imovel.GetNumero());
                    cmd.Parameters.AddWithValue("@alugado", imovel.GetAlugado());
                    cmd.Parameters.AddWithValue("@nome", imovel.Proprietario.Nome);
                    cmd.Parameters.AddWithValue("@telefone", imovel.Proprietario.Telefone);
                    cmd.Parameters.AddWithValue("@cpf", imovel.Proprietario.CPF);

                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}