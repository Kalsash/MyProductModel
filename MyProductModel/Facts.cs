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
