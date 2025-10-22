using MySql.Data.MySqlClient;

namespace DesafioPOO.Models
{
    public class DataBase
    {
        private string connectionString = "server=localhost;port=3306;database=corretora;uid=root;pwd=root";

        public void SalvarProprietario(Proprietario proprietario)
        {
            using (var conexao = new MySqlConnection(connectionString))
            {
                conexao.Open();
                using (var trans = conexao.BeginTransaction())
                {
                    using (var cmdProp = new MySqlCommand("INSERT INTO proprietarios (nome, telefone, cpf) VALUES (@nome, @telefone, @cpf);",
                                conexao, trans))
                    {
                        cmdProp.Parameters.AddWithValue("@nome", proprietario.Nome);
                        cmdProp.Parameters.AddWithValue("@telefone", proprietario.Telefone);
                        cmdProp.Parameters.AddWithValue("@cpf", proprietario.CPF);
                        cmdProp.ExecuteNonQuery();
                    }
                }
            }
        }

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
                        /*long idProprietario = 0;
                        using (var cmdCheck = new MySqlCommand("SELECT id FROM proprietarios WHERE cpf = @cpf LIMIT 1;", conexao, trans))
                        {
                            // cmdCheck.Parameters.AddWithValue("@cpf", imovel.Proprietario.CPF);
                            var scalar = cmdCheck.ExecuteScalar();
                            if (scalar != null && scalar != DBNull.Value)
                            {
                                idProprietario = Convert.ToInt64(scalar);
                            }                                
                        }*/

                        // Se não existir, inserir proprietário e recuperar o id inserido
                        
                            using (var cmdProp = new MySqlCommand("INSERT INTO proprietarios (nome, telefone, cpf) VALUES (@nome, @telefone, @cpf);",
                                conexao, trans))
                            {
                                cmdProp.Parameters.AddWithValue("@nome", imovel.Proprietario.Nome);
                                cmdProp.Parameters.AddWithValue("@telefone", imovel.Proprietario.Telefone);
                                cmdProp.Parameters.AddWithValue("@cpf", imovel.Proprietario.CPF);
                                cmdProp.ExecuteNonQuery();

                                // Tenta pegar id pelo LastInsertedId do comando (funciona com MySql.Data)
                                var idProprietario = cmdProp.LastInsertedId;
                                if (idProprietario == 0) // fallback seguro
                                {
                                    using (var cmdId = new MySqlCommand("SELECT LAST_INSERT_ID();", conexao, trans))
                                    {
                                        idProprietario = Convert.ToInt64(cmdId.ExecuteScalar());
                                    }
                                }
                            }
                        

                        // 3) Inserir imóvel usando o id do proprietário (garante vínculo)
                        using (var cmdImovel = new MySqlCommand(@"INSERT INTO imoveis(tipo, endereco, numero, alugado, nome_proprietario, telefone, 
                        cpf, proprietario_id)VALUES (@tipo, @endereco, @numero, @alugado, @nome, @telefone, @cpf, @proprietarioId);",
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
                            cmdImovel.Parameters.AddWithValue("@proprietarioId", imovel.Proprietario.Id);

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
                        int id = reader.GetInt16("id");
                        int proprietario_id = reader.IsDBNull(reader.GetOrdinal("proprietario_id")) ? 0 : reader.GetInt32("proprietario_id");


                        if (tipo == "casa")
                        {
                            var imovel = new Casa(id, endereco, numero, proprietario_id);
                            lista.Add(imovel);
                        }
                        else
                        {
                            var imovel = new Apartamento(id, endereco, numero, proprietario_id);
                            lista.Add(imovel);
                        }

                        // imovel.SetAlugado(alugado);
                        // lista.Add(imovel);
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

