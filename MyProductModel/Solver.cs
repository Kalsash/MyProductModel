using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace MyProductModel
{
    internal class Solver
    {

        Facts facts { get; set; }
        Productions rules { get; set; }

        TaskManager tasks { get; set; }

        HashSet<string> Path = new HashSet<string>();

        HashSet<string> Logs = new HashSet<string>();

        HashSet<string> Result = new HashSet<string>();

        public Solver(Facts f, Productions ps, TaskManager tm) 
        { 
            facts = f;
            rules = ps;
            tasks = tm;
        }
        public void PrintInfo()
        {
            facts.PrintFacts();
            rules.ProdPrint();
            tasks.PrintTasks();
        }
        public void FixPath() 
        {
            HashSet<string> Def= new HashSet<string>();
            HashSet<string> Use = new HashSet<string>();
            HashSet<string> Del = new HashSet<string>();
            foreach (var s in Path)
            {
                var line = s.Replace(", =>", " =>");

                line = line.Replace(" ", "");
                int ind = line.IndexOf('=');
                string key = line.Substring(0, ind);
                string val = line.Substring(ind + 2);
                if (val.EndsWith("\n"))
                {
                    val = val.Substring(0, val.Length - 1);
                }
                string[] keySplit = key.Split(',');
                Use.Add(val);
                foreach (var x in keySplit)
                {
                    Def.Add(x);
                    
                }
            }
            foreach (var x in Use)
            {
                if (!Def.Contains(x) && !tasks.Tasks.Contains(x))
                {
                    Del.Add(x);
                }
            }
            HashSet<string> DelStr = new HashSet<string>();
            foreach (var s in Path)
            {
                var line = s.Replace(", =>", " =>");

                line = line.Replace(" ", "");
                int ind = line.IndexOf('=');
                string key = line.Substring(0, ind);
                string val = line.Substring(ind + 2);
                if (val.EndsWith("\n"))
                {
                    val = val.Substring(0, val.Length - 1);
                }
                if (Del.Contains(val))
                    DelStr.Add(s);
            }
            foreach(var x in DelStr)
                Path.Remove(x);
        }
        public void PrintPath() 
        {
            var cnt = Path.Count;
            FixPath();
            while (cnt != Path.Count)
            {
                cnt = Path.Count;
                FixPath();
            }
            var temp = "";
            foreach (var item in Logs)
            {
                temp += item + "\n";
            }
            temp += "\n";
            foreach (string item in Path)
            {
                Console.WriteLine(item.Replace(", =>", " =>"));
                temp += item.Replace(", =>", " =>") + "\n"; 
            }
            temp += "\n"+ "Found " + Result.Count + " Laptops: " + "\n";
            foreach (var item in Result)
            {
                temp += item + "\n";
            }
            // Запись строки в файл
            File.WriteAllText("../../result.txt", temp);
            Path.Clear();
            Console.WriteLine();
        }
        public int GetNewFact(Production p)
        {
            if (!facts.IsFact(p.Name))
            {
                var s = "";
                bool flag = true;
                foreach (var clist in p.Conclusions)
                {
                    s = "";
                    foreach (var c in clist)
                    {
                        s += c+ ", ";
                        if (!facts.IsFact(c))
                        {
                            flag = false;
                            break;
                        }                       
                    }
                    if (flag)
                    {
                        facts.AddFact(p.Name);
                        Path.Add(s + "=> " + p.Name);
                        return 1;

                    }
                }
            }
            return 0;
        }

        
        public bool ReverseSolve(string t)
        {
            if (!rules.ProductionDict.ContainsKey(t) || facts.FactsList.Count == 0)
                return false;
            var r = rules.ProductionDict[t];
            Queue<(string, string)> q = new Queue<(string, string)>();
            HashSet<string> Visited = new HashSet<string>();
            
            var path = t;
            q.Enqueue((t, path));
            while (q.Count != 0)
            {
                var tup = q.Dequeue();
                var s = tup.Item1;
                var prod = rules.ProductionDict[s];
                var pt = tup.Item2;

                if (facts.IsFact(s))
                {
                    Path.Add(pt);
                    return true;
                }
                foreach (var con in prod.Conclusions)
                {
                    foreach (var c in con)
                    {
                        if (!Visited.Contains(c))
                        {
                            Visited.Add(c);
                            q.Enqueue((c, pt + "," + c));
                        }
                    }
                }
            }
            

            return false; 
        }
        public bool ForwardingSolve()
        {
            //rules.ProdPrint();
            if (facts.FactsList.Count == 0)
                return false;
            int k = 1;
            while (k != 0)
            {
                k = 0;
                foreach (var rule in rules.ProductionDict)
                {
                    //facts.PrintFacts();
                    var p = rule.Value;
                  k += GetNewFact(p);
                    // facts.PrintFacts();
                    foreach (var item in tasks.Tasks)
                    {
                        if (facts.IsFact(item))
                        {
                            facts.AddFact(item);
                            Result.Add(item);
                        }
                    }
                    
                }
            }
          
            if (Result.Count != 0)
            {
                return true;
            }
            return false;
        }
        void PrettyPrint()
        {
            char[] charArray = Path.First().ToCharArray();
            Array.Reverse(charArray);
            var t = new string(charArray);
           // Console.WriteLine(t);
            var tt = t.Split(',');
            for (int i = 0; i < tt.Length-1; i++)
            {
                Console.WriteLine(tt[i] + " => "+ tt[i+1]);
                facts.AddFact(tt[i]);
            }
            Path.Clear();
        }
        public void Run(int k) 
        {
            if (k == 0)
            {
                if (ForwardingSolve())
                {

                    Logs.Add("ForwardingSolve started...");
                    PrintPath();
                }
                else
                {
                    File.WriteAllText("../../result.txt", "Not found any laptops with this params");
                }
            }

            else
            //    if (ReverseSolve(t))
            //{
            //    Console.WriteLine("ReverseSolve");
            //    Console.WriteLine("Task " + t + " was solved");
            //    PrettyPrint();
            //    // Console.WriteLine(Path.First());
            //    facts.AddFact(t);
            //    tasks.RemoveTask(t);
            //}
            Console.WriteLine("End");

        }
    }
}
