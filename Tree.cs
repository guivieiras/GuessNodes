using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UFSC.Trabalho02
{
    [Serializable]
    public class Tree
    {

        public QuestionNode Root { get; set; }
        public QuestionNode AddQuestion(QuestionNode pai, bool eh, string caracteristica)
        {
            var qn = new QuestionNode(caracteristica);

            if (pai == null && Root == null)
                Root = qn;
            else if (pai == null && Root != null)
                throw new Exception("Arvore já tem root, pai nulo invalido");
            else
            {
                if (eh)
                    pai.eh = qn;
                else
                    pai.nEh = qn;


            }
                
            return qn;
        }
        public AnimalNode AddAnimal(QuestionNode caracteristica, bool eh, string nome)
        {
            var animal = new AnimalNode(nome);
            if (eh)
                caracteristica.eh = animal;
            else
                caracteristica.nEh = animal;

            return animal;
        }

        public QuestionNode GetQuestionNode (string caracteristica)
        {
            return GetInOrder().First(l => l.Caracteristica == caracteristica);
        }

        public IEnumerable<QuestionNode> GetInOrder()
        {
            return null;
        }
    }
}
