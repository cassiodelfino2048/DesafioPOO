using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DesafioPOO.Models
{
    public class Casa : Imovel
    {
        public Casa(string endereco, int numero, Proprietario proprietario) : base(endereco, numero, proprietario) { }
        public override string EstaAlugado()
        {
            return Alugado ? "A casa esta alugada." : "A casa está disponível";
        }
        
    }
}