using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DesafioPOO.Models
{
    public class Apartamento : Imovel
    {

        public Apartamento(int id, string endereco, int numero, int proprietario_id) : base(id, endereco, numero, proprietario_id) { }
        public Apartamento(string endereco, int numero, int proprietario_id) : base(endereco, numero, proprietario_id) { }

        public override string EstaAlugado()
        {
            return Alugado ? $"O apartamento de número {Numero} está alugado."
            : $"O apartamento de número {Numero} está disponível.";        
        }
    }
}