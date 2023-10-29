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
        public Dictionary<int, Production> ProductionDict { get; } = new Dictionary<int, Production>();
        int k = 1;
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
                    if (line == "")
                        break;
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
        public int GetByName(string s, int k)
        {

            foreach (var item in ProductionDict)
            {
                if (item.Value.Name == s)
                {
                    if (k == 0)
                        return item.Key;
                    k--;
                }
            }
            return 0;
        }
        public int GetByName(string s)
        {

            foreach (var item in ProductionDict)
            {
                if (item.Value.Name == s)
                {

                    return item.Key;
                }
            }
            return 0;
        }
        public void AddProd(string name, string[] con)
        {
            if (ProductionDict.ContainsKey(k))
            {
                ProductionDict[k].AddConclusion(con);
            }
            else
            {
                ProductionDict.Add(k++, new Production(name, con));
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
