using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace MyProductModel
{
    public class Facts
    {
        public List<string> FactsList { get; }
        public HashSet<string> F1 = new HashSet<string>();
        public HashSet<string> F2 = new HashSet<string>();
        public HashSet<string> F3 = new HashSet<string>();
        public HashSet<string> F4 = new HashSet<string>();
        public HashSet<string> FF = new HashSet<string>();
        public HashSet<string> NotF = new HashSet<string>();

        public Facts(string filePath)
        {
            FactsList = LoadFactsFromFile(filePath);
        }

        private List<string> LoadFactsFromFile(string filePath)
        {
            List<string> facts = new List<string>();

            using (StreamReader reader = new StreamReader(filePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    line = line.Replace(" ", "");
                    string[] factsSplit = line.Split(',');
                    facts.AddRange(factsSplit);
                    facts.Remove("");
                }
            }

            return facts;
        }
        public bool IsFact(string t)
        {
            if (FactsList.Contains(t))
            {
                return true;
            }
            return false;
        }
        public void AddFact(string fact)
        {
            if (!IsFact(fact))
            {
                FactsList.Add(fact);
            }
        }
        public void GroupFacts(HashSet<string> f1, HashSet<string> f2,
            HashSet<string> f3, HashSet<string> f4, HashSet<string> ff)
            {
            F1 = f1;
            F2 = f2; 
            F3 = f3;
            F4 = f4;
            FF= ff;
            }

        public void PrintFacts()
        {
            Console.WriteLine("Facts: ");
            foreach (string fact in FactsList)
            {
                Console.WriteLine(fact);
            }
            Console.WriteLine();
        }
    }
}
