using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UFSC.Trabalho02
{
    [Serializable]
    public class QuestionNode : Node
    {
        public QuestionNode(string caracteristica)
        {
            Caracteristica = caracteristica;
        }

        public string Caracteristica { get; set; }

        public Node eh { get; set; }

        public Node nEh { get; set; }
    }
}
