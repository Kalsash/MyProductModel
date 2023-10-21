using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyProductModel
{
    internal class Productions
    {
        public Dictionary<string, Production> ProductionDict{ get; } = new Dictionary<string, Production>();

        public Productions(string filePath)
        {
            LoadRules(filePath);
        }

        public void LoadRules(string filePath)
        {
            using (StreamReader rules = new StreamReader(filePath))
            {
                string line;
                while ((line = rules.ReadLine()) != null)
                {
                    line = line.Replace(" ", "");
                    int ind = line.IndexOf('=');
                    string key = line.Substring(0, ind);
                    string val = line.Substring(ind + 2);
                    if (val.EndsWith("\n"))
                    {
                        val = val.Substring(0, val.Length - 1);
                    }
                    string[] keySplit = key.Split(',');
                    AddProd(val, keySplit);
                }
            }
        }

        public void AddProd(string name, string[] con)
        {
            if (ProductionDict.ContainsKey(name))
            {
                ProductionDict[name].AddConclusion(con);
            }
            else
            {
                ProductionDict.Add(name, new Production(name, con));
            }
           
        }
        public void ProdPrint()
        {
            foreach (Production p in ProductionDict.Values) 
            { p.Print(); }
            Console.WriteLine();
        }

    }
}
