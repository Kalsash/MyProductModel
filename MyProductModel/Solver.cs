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

        public Facts facts { get; set; }
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
        //public bool ReverseSolve(string t)
        //{
        //    bool f1 = false;
        //    bool f2 = false;
        //    bool f3 = false;
        //    bool f4 = false;
        //    HashSet<string> ff = new HashSet<string>();
        //    if (!rules.ProductionDict.ContainsKey(rules.GetByName(t)) || facts.FactsList.Count == 0)
        //        return false;
        //    var r = rules.ProductionDict[rules.GetByName(t)];

        //    Queue<(string, string)> q = new Queue<(string, string)>();
        //    HashSet<string> Visited = new HashSet<string>();
        //    Path.Clear();
        //    var path = t;
        //    q.Enqueue((t, path));
        //    while (q.Count != 0)
        //    {
        //        var tup = q.Dequeue();
        //        var s = tup.Item1;

        //        if (facts.IsFact(s)&& !facts.NotF.Contains(s))
        //        {
        //            if (facts.F1.Contains(s))
        //                f1 = true;
        //            if (facts.F2.Contains(s))
        //                f2 = true;
        //            if (facts.F3.Contains(s))
        //                f3 = true;
        //            if (facts.F4.Contains(s))
        //                f4 = true;
        //            if (facts.FF.Contains(s))
        //            {
        //                ff.Add(s);
        //            }
        //            Console.WriteLine(s);
        //            Path.Add(tup.Item2);
        //        }
        //        if (f1 && f2 && f3 && f4 && (ff.Count == facts.FF.Count))
        //        {
        //            return true;
        //        }
        //        if (rules.GetByName(s) == 0)
        //        {
        //            continue;
        //        }
        //        string temp = "";
        //        var prod = rules.ProductionDict[rules.GetByName(s)];
        //        var pt = tup.Item2;
        //        foreach (var con in prod.Conclusions)
        //        {
        //            foreach (var c in con)
        //            {
        //                if (!Visited.Contains(c))
        //                {
        //                    Visited.Add(c);
        //                    q.Enqueue((c, pt + "," + c));
        //                }
        //            }
        //        }
        //        int k = 1;
        //        while (rules.GetByName(s, k) != 0)
        //        {
        //            prod = rules.ProductionDict[rules.GetByName(s, k)];
        //            pt = tup.Item2;
        //            if (temp == pt)
        //                break;
        //            k++;
        //            foreach (var con in prod.Conclusions)
        //            {
        //                foreach (var c in con)
        //                {
        //                    if (!Visited.Contains(c))
        //                    {
        //                        Visited.Add(c);
        //                        q.Enqueue((c, pt + "," + c));
        //                    }
        //                }
        //            }

        //        }



        //    }


        //    return false;
        //}

        public bool ReverseSolve(string t)
        {
            bool f1 = false;
            bool f2 = false;
            bool f3 = false;
            bool f4 = false;
            HashSet<string> ff = new HashSet<string>();
            if (!rules.ProductionDict.ContainsKey(rules.GetByName(t)) || facts.FactsList.Count == 0)
                return false;
            var r = rules.ProductionDict[rules.GetByName(t)];
            Stack<(string, string)> stack = new Stack<(string, string)>();
            HashSet<string> Visited = new HashSet<string>();
            Path.Clear();
            var path = t;
            stack.Push((t, path));
            while (stack.Count != 0)
            {
                var tup = stack.Pop();
                var s = tup.Item1;

                if (rules.GetByName(s) != 0)
                {
                    if (!Visited.Contains(s))
                    {
                        Visited.Add(s);
                        stack.Push((s, tup.Item2));
                        foreach (var con in rules.ProductionDict[rules.GetByName(s)].Conclusions)
                        {
                            foreach (var c in con)
                            {
                                if (!Visited.Contains(c))
                                {
                                    stack.Push((c, tup.Item2 + "," + c));
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (facts.IsFact(s) && !facts.NotF.Contains(s))
                    {
                        if (facts.F1.Contains(s))
                            f1 = true;
                        if (facts.F2.Contains(s))
                            f2 = true;
                        if (facts.F3.Contains(s))
                            f3 = true;
                        if (facts.F4.Contains(s))
                            f4 = true;
                        if (facts.FF.Contains(s))
                        {
                            ff.Add(s);
                        }
                        Console.WriteLine(s);
                        Path.Add(tup.Item2);
                    }
                }

                if (f1 && f2 && f3 && f4 && (ff.Count == facts.FF.Count))
                {
                    return true;
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
            foreach (var c in Path)
            {
                //char[] charArray = Path.First().ToCharArray();
                char[] charArray = c.ToCharArray();
                // Array.Reverse(charArray);
                var t = new string(charArray);
                // Console.WriteLine(t);
                var tt = t.Split(',');
                for (int i = 0; i < tt.Length - 1; i++)
                {
                    Logs.Add(tt[i] + " => " + tt[i + 1] + "\n");    
                    Console.WriteLine(tt[i] + " => " + tt[i + 1]);
                    //facts.AddFact(tt[i]);
                }
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
            {
                Logs.Add("ReverseSolve started..." +"\n");
                foreach (var t in tasks.Tasks)
                {
                    if (ReverseSolve(t))
                    {
                        Logs.Add("\n");
                        Logs.Add(t+ "\n");
                        //Console.WriteLine("ReverseSolve");
                        // Console.WriteLine("Task " + t + " was solved");
                        PrettyPrint();
                        // Console.WriteLine(Path.First());
                        // facts.AddFact(t);
                        // tasks.RemoveTask(t);
                        Result.Add(t);
                    }
                }
                Logs.Add("Found "+Result.Count.ToString()+ " laptops" + "\n");
                var temp = "";
                foreach (var x in Logs)  
                    temp += x;
                foreach (var t in Result)
                    temp += t + "\n";
                File.WriteAllText("../../result.txt", temp);
            }
           

            Console.WriteLine("End");

        }
    }
}
