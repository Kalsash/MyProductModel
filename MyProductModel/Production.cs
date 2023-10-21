using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyProductModel
{
    internal class Production
    {
        public string Name { get; }
        public List<string[]> Conclusions { get; } = new List<string[]>();


        public Production(string name, string[] con)
        {
            Name = name;
            Conclusions.Add(con);
        }
        public void AddConclusion(string[] con)
        {
            Conclusions.Add(con);
        }
        public void Print()
        {
            Console.WriteLine($"Production Name: {Name}");
            Console.WriteLine("Conclusions:");
            foreach (string[] conclusion in Conclusions)
            {
                Console.WriteLine(string.Join(", ", conclusion));
            }

        }
    }
}
