using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bonus12
{
  
    class Program
    {
        static Dictionary<String, HashSet<String>> getAllProductions(String grammar)
        {
            var splitArr = grammar.Split();
            var result = new Dictionary<String, HashSet<String>>();
            foreach (var str in splitArr)
            {
                var rightParts = str.Substring(3);
                var leftPart = str.Substring(0, 1);
                result.Add(leftPart, new HashSet<String>());
                foreach (var rightPart in rightParts.Split('|'))
                    result[leftPart].Add(rightPart);
            }
            return result;
        }

        static void removeChainFromDictionary(Dictionary<String, HashSet<String>> productions)
        {
            foreach (var p in productions)
            {
                p.Value.RemoveWhere((rightPart) => rightPart.Length == 1 && char.IsUpper(rightPart[0]));
                
            }
        }

        static Dictionary<String, HashSet<String>> getN(Dictionary<String, HashSet<String>> productions)
        {
            var n = new Dictionary<String, HashSet<String>>();
            foreach (var p in productions)
            {
                n.Add(p.Key, new HashSet<String>() { p.Key });
            }
            foreach (var p in n)
            {
                p.Value.Add(p.Key);
                while (true)
                {
                    var sz = p.Value.Count;
                    foreach(var pr in productions.Keys.Where((k)=>productions[k].Any((s)=>p.Value.Contains(s))))
                    {
                        p.Value.Add(pr);
                        
                    }
                    if (sz==p.Value.Count)
                        break;

                }
            }
            return n;
        }

        static void addNewProductions(Dictionary<String, HashSet<String>> productions, Dictionary<String, HashSet<String>> n)
        {
            foreach (var p in productions)
            {
                foreach (var rightPart in p.Value)
                {
                    foreach(var p1 in n[p.Key]){
                        productions[p1].Add(rightPart);
                        
                    }

                }
            }
        }

        static String getResultString(Dictionary<String, HashSet<String>> productions)
        {
            String result = "";
            foreach(var p in productions)
            {
                result +=p.Key+"->"+ String.Join("|", p.Value.ToArray())+" ";
            }

            return result;
        }

        


        static String removeChainProductions(String grammar)
        {
            var productions = getAllProductions(grammar);
            var n = getN(productions);
            removeChainFromDictionary(productions);
            addNewProductions(productions, n);
            return getResultString(productions);
        }


        static void Main(string[] args)
        {
            Console.WriteLine("Введите название файла с описанием гамматики:");
            var fileName = Console.ReadLine();
            if (File.Exists(fileName))
            {
                try
                {
                    var grammar = File.ReadAllText(fileName);
                    Console.WriteLine("Считано: " + grammar);
                    Console.WriteLine("Удалили цепочные символы: " + removeChainProductions(grammar));

                }
                catch 
                {
                    Console.WriteLine("Возникла ошибка выполнения. Проверьте правильность исходной грамматики");
                }
               
            }
            else
            {
                Console.WriteLine("Имя файла введено с ошибкой");
            }
            Console.WriteLine("Нажмите любую клавишу");
            Console.ReadKey();
        }
    }
}
