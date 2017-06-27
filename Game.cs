using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Soap;
using System.Text;
using System.Threading.Tasks;

namespace UFSC.Trabalho02
{
    public class Game
    {

        public Tree tree = new Tree();

        public void StartGame()
        {
            if (tree.Root == null)
            {
                Console.WriteLine("Por favor, inicialize a árvore.");
                Console.Write("Animal: ");
                var animal = Console.ReadLine();
                Console.Write("Uma caracteristica sua: ");
                var caract = Console.ReadLine();

                var qn = tree.AddQuestion(null, false, caract);
                tree.AddAnimal(qn, true, animal);
                Console.Clear();
            }

            var tuple = RecursiveAsker(tree.Root);
            Console.Clear();
            ShowResult(tuple);
            ShowMainScreen();
        }

        public void ShowMainScreen()
        {
            Console.WriteLine("1. Jogar");
            Console.WriteLine("2. Salvar jogo atual");
            Console.WriteLine("3. Carregar jogo");
            Console.WriteLine("4. Fechar jogo");

            var response = Console.ReadLine();
            Console.Clear();
            switch (response)
            {
                case "1":
                    StartGame();
                    break;
                case "2":
                    SaveGame();
                    ShowMainScreen();
                    break;
                case "3":
                    LoadGame();
                    ShowMainScreen();
                    break;
                case "4":
                    break;
                default:
                    Console.WriteLine("Não entendi.");
                    ShowMainScreen();
                    break;
            }

        }

        public void SaveGame()
        {
            Console.Write("Nome do save: ");
            var saveName = Console.ReadLine();

            if (File.Exists($"{saveName}.xml"))
            {
                Console.Write("Esse save já existe, deseja sobreescrever? ");
                var response = Console.ReadLine();

                if (response == "n")
                    return;
                if (response != "s")
                { 
                    Console.WriteLine("Não entendi.");
                    SaveGame();
                    return;
                }
            }
            

            Stream stream = File.Create($"{saveName}.xml");
            SoapFormatter formatter = new SoapFormatter();

            formatter.Serialize(stream, tree);
            stream.Close();

            Console.WriteLine("Jogo salvo com sucesso!");
        }

        public void LoadGame()
        {
            Console.Write("Caminho do save: ");
            var saveName = Console.ReadLine();

            if (!File.Exists($"{saveName}.xml"))
            {
                Console.WriteLine("Erro, save game não encontrado.");
                return;
            }

            Stream stream = File.OpenRead($"{saveName}.xml");
            SoapFormatter formatter = new SoapFormatter();

            tree = (Tree)formatter.Deserialize(stream);
            stream.Close();

            Console.WriteLine("Save game carregado com sucesso!");
        }

        public void ShowResult(Tuple<QuestionNode, AnimalNode> tuple)
        {
            //Caso seja nulo o item encontrado, ocorre ao adicionar o segundo item na arvore
            if (tuple.Item2 == null)
            {
                ShowNewQA(tuple.Item1, tuple.Item2);
                return;
            }
            Console.Write($"Voce estava pensando em {tuple.Item2.Nome}? ");
            var resposta = Console.ReadLine();

            if (resposta == "s")
                Console.WriteLine(";)");
            else if (resposta == "n")
                ShowNewQA(tuple.Item1, tuple.Item2);
            else
            {
                Console.WriteLine("Não entendi");
                ShowResult(tuple);
            }
        }

        public Tuple<QuestionNode,AnimalNode> RecursiveAsker(QuestionNode question)
        {
            var response = ShowQuestion(question);
            //Caso o usuario responder sim
            if (response)
            {
                //Caso o tipo da resposa seja uma folha: um animal.
                if (question.eh.GetType() == typeof(AnimalNode))
                {
                    //Retorna uma tuple com a ultima pergunta e o animal encontrado.
                    return new Tuple<QuestionNode, AnimalNode>(question, (AnimalNode)question.eh);
                }
                //Caso contrario, faz a próxima pergunta.
                else return RecursiveAsker((QuestionNode)question.eh);
            }
            //Caso responda não
            else if (!response)
            {
                if (question.nEh == null || question.nEh.GetType() == typeof(AnimalNode))
                {
                    return new Tuple<QuestionNode, AnimalNode>(question, (AnimalNode)question.nEh);
                }
                else return RecursiveAsker((QuestionNode)question.nEh);
            }
            throw new Exception("Unexpected error");
        }

        public bool ShowQuestion(QuestionNode question)
        {
            Console.Write($"O animal {question.Caracteristica}? ");
            string resposta = Console.ReadLine();

            if (resposta == "s") return true;
            if (resposta == "n") return false;

            Console.WriteLine("Não entendi, responda novamente.");
            return ShowQuestion(question);
        }

        //É chamado caso não encontre o animal no qual o jogador pensou
        public void ShowNewQA(QuestionNode lastQuestion, AnimalNode foundAnimal)
        {
            //Ocorre apenas no primeiro animal na esquerda do root.
            if (foundAnimal == null)
            {
                Console.Write("Não conheço esse animal, escreva o nome do animal no qual você pensou: ");
                string animal = Console.ReadLine();

                tree.AddAnimal(lastQuestion, false, animal);
            }
            else
            {
                Console.Write("Escreva uma caracteristica que esse animal tenha e que o outro não tenha: ");
                string caract = Console.ReadLine();
                Console.Write("O nome do animal: ");
                string animal = Console.ReadLine();

                bool eh;
                if (foundAnimal == lastQuestion.eh)
                    eh = true;
                else
                    eh = false;

                var questionNode = tree.AddQuestion(lastQuestion, eh, caract);

                tree.AddAnimal(questionNode, true, animal);
                questionNode.nEh = foundAnimal;
            }
        }

    }
}
