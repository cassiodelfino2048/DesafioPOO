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

        // Encapsulamento (Rquisito 2)
        public Proprietario(string nome, string telefone, string cpf)
        {
            Nome = nome;
            Telefone = telefone;
            CPF = cpf;
        }
    }
    public abstract class Imovel
    {
        protected string Endereco;
        protected int Numero;
        protected bool Alugado;
        protected Proprietario Proprietario;

        public Imovel(string endereco, int numero, Proprietario proprietario)
        {
            Endereco = endereco;
            Numero = numero;
            Proprietario = proprietario;
            Alugado = false;
        }

        // Cria um encapsulameto para os atributos do imóvel
        public string GetEndereco() => Endereco;
        public int GetNumero() => Numero;
        public bool GetAlugado() => Alugado;
        public void SetAlugado(bool valor) => Alugado = valor;

        // Cria informações sobre o proprietario para exibição depois
        public string ContatoProprietario() => $"Proprietário: {Proprietario.Nome}, Tel: {Proprietario.Telefone}";

        // Calcular Aluguel
        public int CalcularAluguel(int valorMensal, int meses) => valorMensal * meses;
        public abstract string EstaAlugado();

    }
}