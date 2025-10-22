using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DesafioPOO.Models
{
    public class Proprietario // Criei a classe proprietário
    {
        
        public string Nome { get; set; }
        public string Telefone { get; set; }
        public string CPF { get; set; }
        public int Id { get; set; }

        // Encapsulamento (Rquisito 2)
        public Proprietario(int id, string nome, string telefone, string cpf)
        {
            Id = id;
            Nome = nome;
            Telefone = telefone;
            CPF = cpf;
        }
        public Proprietario(string nome, string telefone, string cpf)
        {
            Id = 0;
            Nome = nome;
            Telefone = telefone;
            CPF = cpf;
        }
    }
}
