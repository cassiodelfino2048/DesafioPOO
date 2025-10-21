using MySql.Data.MySqlClient;

namespace DesafioPOO.Models
{
    public class DataBase
    {
        private string connectionString = "server=localhost;port=3306;database=corretora;uid=root;pwd=root";

        public void SalvarImovel(Imovel imovel)
        {
            using (var conexao = new MySqlConnection(connectionString))
            {
                conexao.Open();

                using (var trans = conexao.BeginTransaction())
                {
                    try
                    {
                        // Tentar achar proprietario existente pelo CPF
                        long idProprietario = 0;
                        using (var cmdCheck = new MySqlCommand("SELECT id FROM proprietarios WHERE cpf = @cpf LIMIT 1;", conexao, trans))
                        {
                            cmdCheck.Parameters.AddWithValue("@cpf", imovel.Proprietario.CPF);
                            var scalar = cmdCheck.ExecuteScalar();
                            if (scalar != null && scalar != DBNull.Value)
                            {
                                idProprietario = Convert.ToInt64(scalar);
                            }                                
                        }

                        // Se não existir, inserir proprietário e recuperar o id inserido
                        if (idProprietario == 0)
                        {
                            using (var cmdProp = new MySqlCommand("INSERT INTO proprietarios (nome, telefone, cpf) VALUES (@nome, @telefone, @cpf);",
                                conexao, trans))
                            {
                                cmdProp.Parameters.AddWithValue("@nome", imovel.Proprietario.Nome);
                                cmdProp.Parameters.AddWithValue("@telefone", imovel.Proprietario.Telefone);
                                cmdProp.Parameters.AddWithValue("@cpf", imovel.Proprietario.CPF);
                                cmdProp.ExecuteNonQuery();

                                // Tenta pegar id pelo LastInsertedId do comando (funciona com MySql.Data)
                                idProprietario = cmdProp.LastInsertedId;
                                if (idProprietario == 0) // fallback seguro
                                {
                                    using (var cmdId = new MySqlCommand("SELECT LAST_INSERT_ID();", conexao, trans))
                                    {
                                        idProprietario = Convert.ToInt64(cmdId.ExecuteScalar());
                                    }
                                }
                            }
                        }

                        // Debug: escreva no console (ou log) para confirmar o id
                        Console.WriteLine($"DEBUG: proprietario_id = {idProprietario}");

                        // 3) Inserir imóvel usando o id do proprietário (garante vínculo)
                        using (var cmdImovel = new MySqlCommand(@"
                    INSERT INTO imoveis
                      (tipo, endereco, numero, alugado, nome_proprietario, telefone, cpf, proprietario_id)
                    VALUES
                      (@tipo, @endereco, @numero, @alugado, @nome, @telefone, @cpf, @propId);",
                            conexao, trans))
                        {
                            cmdImovel.Parameters.AddWithValue("@tipo", imovel is Casa ? "Casa" : "Apartamento");
                            cmdImovel.Parameters.AddWithValue("@endereco", imovel.GetEndereco());
                            cmdImovel.Parameters.AddWithValue("@numero", imovel.GetNumero());
                            // converta para int se sua coluna for TINYINT(1)
                            cmdImovel.Parameters.AddWithValue("@alugado", imovel.GetAlugado());
                            cmdImovel.Parameters.AddWithValue("@nome", imovel.Proprietario.Nome);
                            cmdImovel.Parameters.AddWithValue("@telefone", imovel.Proprietario.Telefone);
                            cmdImovel.Parameters.AddWithValue("@cpf", imovel.Proprietario.CPF);
                            cmdImovel.Parameters.AddWithValue("@propId", idProprietario);

                            cmdImovel.ExecuteNonQuery();
                        }

                        trans.Commit();
                    }
                    catch (Exception ex)
                    {
                        try { trans.Rollback(); } catch { /* ignore rollback errors */ }
                        Console.WriteLine("Erro ao salvar imóvel: " + ex.Message);
                        throw; // rethrow para quem chamou decidir o que fazer
                    }
                }

                conexao.Close();
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
        public void AtualizarStatus(int id, bool alugado)
        {
        using (var conexao = new MySqlConnection(connectionString))
        {
        conexao.Open();

        using (var transacao = conexao.BeginTransaction())
        {
            string query = "UPDATE imoveis SET alugado = @alugado WHERE id = @id";

            using (var cmd = new MySqlCommand(query, conexao, transacao))
            {
                cmd.Parameters.AddWithValue("@alugado", alugado ? 1 : 0);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }

            // Confirma as alterações
            transacao.Commit();
        }

        conexao.Close();
    }
    }
}

}

