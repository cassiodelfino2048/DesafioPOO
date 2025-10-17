using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DesafioPOO.Models
{
    public class Apartamento : Imovel
    {
        public Apartamento(string endereco, int numero, Proprietario proprietario) : base(endereco, numero, proprietario) { }

        public override string EstaAlugado()
        {
            return Alugado ? $"O apartamento de número {Numero} está alugado."
            : $"O apartamento de número {Numero} está disponível.";        
        }
    }
}