using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyProductModel
{
    internal class TaskManager
    {
        public List<string> Tasks { get; } = new List<string>();

        public TaskManager(string filePath) { Tasks = LoadTasksFromFile(filePath); }
        private List<string> LoadTasksFromFile(string filePath)
        {
            List<string> tasks = new List<string>();
            using (StreamReader tasksReader = new StreamReader(filePath))
            {
                string line;
                while ((line = tasksReader.ReadLine()) != null)
                {
                    line = line.Replace(" ", "");
                    string[] tasksSplit = line.Split(',');
                    tasks.AddRange(tasksSplit);
                }
            }
            return tasks;
        }

        public void AddTask(string task)
        {
            Tasks.Add(task);
        }
        public void RemoveTask(string task)
        {
            Tasks.Remove(task);
        }
        public int TasksCount()
        {
            return Tasks.Count;
        }
        public string GetTask()
        {
           return Tasks.First().ToString();
        }
        public void PrintTasks()
        {
            Console.WriteLine("Tasks:");
            foreach (string task in Tasks)
            {
                Console.WriteLine(task);
            }
            Console.WriteLine();
        }
    }
}
