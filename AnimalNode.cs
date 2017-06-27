using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UFSC.Trabalho02
{
    [Serializable]
    public class AnimalNode : Node
    {
        public AnimalNode(string nome)
        {
            Nome = nome;
        }

        public string Nome { get; set; }
    }
}
