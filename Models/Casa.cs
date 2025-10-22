using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DesafioPOO.Models
{
    public class Casa : Imovel
    {

        public Casa(int id, string endereco, int numero, int proprietario_id) : base(id, endereco, numero, proprietario_id) { }
        public Casa(string endereco, int numero, int proprietario_id) : base(endereco, numero, proprietario_id) { }
        public override string EstaAlugado()
        {
            return Alugado ? "A casa esta alugada." : "A casa está disponível";
        }
        
    }
}