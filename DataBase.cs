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
                    cmd.Parameters.AddWithValue("@alugado", imovel.GetAlugado() ? 1 : 0);
                    cmd.Parameters.AddWithValue("@nome", imovel.Proprietario.Nome);
                    cmd.Parameters.AddWithValue("@telefone", imovel.Proprietario.Telefone);
                    cmd.Parameters.AddWithValue("@cpf", imovel.Proprietario.CPF);

                    cmd.ExecuteNonQuery();
                }
            }
        }


        public List<Imovel> LerImoveis()
        {

            var lista = new List<Imovel>();

            using (var conexao = new MySqlConnection(connectionString))
            {
                conexao.Open();
                string query = "SELECT * FROM imoveis";

                using (var cmd = new MySqlCommand(query, conexao))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string tipo = reader.GetString("tipo");
                        string endereco = reader.GetString("endereco");
                        int numero = reader.GetInt32("numero");
                        bool alugado = reader.GetBoolean("alugado");
                        string nome = reader.GetString("nome_proprietario");
                        string telefone = reader.GetString("telefone");
                        string cpf = reader.GetString("cpf");

                        var proprietario = new Proprietario(nome, telefone, cpf);
                        Imovel imovel;

                        if (tipo == "casa")
                            imovel = new Casa(endereco, numero, proprietario);
                        else
                            imovel = new Apartamento(endereco, numero, proprietario);

                        imovel.SetAlugado(alugado);
                        lista.Add(imovel);
                    }
                }
            }

            return lista;
        }

        public void DeletarImovelDoBanco(Imovel imovel)
        {
            using (var conexao = new MySqlConnection(connectionString))
            {
                conexao.Open();
                string query = "DELETE FROM imoveis WHERE endereco = @endereco AND numero = @numero";

                using (var cmd = new MySqlCommand(query, conexao))
                {
                    cmd.Parameters.AddWithValue("@endereco", imovel.GetEndereco());
                    cmd.Parameters.AddWithValue("@numero", imovel.GetNumero());

                    cmd.ExecuteNonQuery();
                }

            }
        }
    }
}
