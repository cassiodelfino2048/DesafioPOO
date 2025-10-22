using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DesafioPOO.Models
{    
    
    public abstract class Imovel
    {
        
        protected string Endereco;
        protected int Numero;
        protected bool Alugado;
        protected int Id;
        protected int Proprietario_id;
        public Proprietario Proprietario { get; protected set; }



        public Imovel(int id, string endereco, int numero, int proprietario_id)
        {
            Id = id;
            Proprietario_id = proprietario_id;
            Endereco = endereco;
            Numero = numero;
            Alugado = false;
        }
        public Imovel(string endereco, int numero, int proprietario_id)
        {
            Id = 0;
            Proprietario_id = proprietario_id;
            Endereco = endereco;
            Numero = numero;            
            Alugado = false;
        }

        // Cria um encapsulameto para os atributos do imóvel
        public string GetEndereco() => Endereco;
        public int GetNumero() => Numero;
        public bool GetAlugado() => Alugado;
        public int GetId() => Id;
        public void SetAlugado(bool valor)
        {
            Alugado = valor;
            if (Alugado)
            {
                Console.WriteLine("Esta muito alugado");
            }
        }

        // Cria informações sobre o proprietario para exibição depois
        public string ContatoProprietario() => $"Proprietário: {Proprietario.Nome}, Tel: {Proprietario.Telefone}";

        // Calcular Aluguel
        public int CalcularAluguel(int valorMensal, int meses) => valorMensal * meses;
        public abstract string EstaAlugado();

    }
}