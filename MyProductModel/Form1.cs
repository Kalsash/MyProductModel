using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace MyProductModel
{
    public partial class Form1 : Form
    {
        int IsForward = 0;
        HashSet<string> F1 = new HashSet<string>();
        HashSet<string> F2 = new HashSet<string>();
        HashSet<string> F3 = new HashSet<string>();
        HashSet<string> F4 = new HashSet<string>();
        HashSet<string> FF = new HashSet<string>();
        public Form1()
        {
            InitializeComponent();
            richTextBox1.SelectionFont = new Font(richTextBox1.Font, FontStyle.Bold);
            this.Load += Form_Load;
            

        }
        public void Initialize ()
            {
            Facts facts = new Facts("../../facts.txt");
            Productions ps = new Productions("../../rules.txt");
            TaskManager tm = new TaskManager("../../tasks.txt");
             Solver s = new Solver(facts, ps, tm);
            s.facts.GroupFacts(F1,F2,F3,F4,FF);
            s.Run(IsForward);
           
        }

        public RichTextBox GetTextBox()
        {
            return richTextBox1;
        }
        public void Form_Load(object sender, EventArgs e)
        {
            richTextBox1.Text = "Здесь будет отображаться вывод";
          
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
            private void MyClick()
        {
            // Получение выбранных элементов
            List<object> selectedItems = new List<object>();

            foreach (object selectedItem in checkedListBox1.CheckedItems)
            {
                F1.Add(selectedItem.ToString());
                selectedItems.Add(selectedItem);
            }
            foreach (object selectedItem in checkedListBox2.CheckedItems)
            {
                F2.Add(selectedItem.ToString());
                selectedItems.Add(selectedItem);
            }
            foreach (object selectedItem in checkedListBox3.CheckedItems)
            {
                F3.Add(selectedItem.ToString());
                selectedItems.Add(selectedItem);
            }
            foreach (object selectedItem in checkedListBox4.CheckedItems)
            {
                FF.Add(selectedItem.ToString());
                selectedItems.Add(selectedItem);
            }
            foreach (object selectedItem in checkedListBox5.CheckedItems)
            {
                F4.Add(selectedItem.ToString());
                selectedItems.Add(selectedItem);
            }

            Dictionary<string, string> NotDict = new Dictionary<string, string>();
            using (StreamReader rules = new StreamReader("../../not.txt"))
            {
                string line;
                while ((line = rules.ReadLine()) != null)
                {
                    if (line == "")
                    {
                        break;
                    }
                    line = line.Replace(" ", "");
                    int ind = line.IndexOf('=');
                    string key = line.Substring(0, ind);
                    string val = line.Substring(ind + 2);
                    if (val.EndsWith("\n"))
                    {
                        val = val.Substring(0, val.Length - 1);
                    }
                    NotDict.Add(key, val);
                }
            }
            foreach (var item in NotDict.Keys)
            {
                if (!checkedListBox4.CheckedItems.Contains(item))
                    selectedItems.Add(NotDict[item]);
            }

            foreach (object selectedItem in checkedListBox6.CheckedItems)
            {
                if (selectedItem.ToString() == "Обратный")
                {
                    IsForward = 1;
                }
                else
                    IsForward = 0;
            }

            // Преобразование выбранных элементов в массив строк, если нужно
            string[] selectedValues = selectedItems.Select(item => item.ToString()).ToArray();
            var temp = "";
            foreach (string value in selectedValues)
            {
                temp += value + ",";
            }

             File.WriteAllText("../../facts.txt", temp);
            //File.WriteAllText("../../result.txt", temp);
           Initialize();

        }
        private void button1_Click(object sender, EventArgs e)
        {
            MyClick();
            string filePath = "../../result.txt"; // Замените на путь к вашему файлу

            try
            {
                // Читаем содержимое файла
                string fileContent = File.ReadAllText(filePath);

                // Отображаем содержимое файла в TextBox
                richTextBox1.Text = fileContent;

                // Устанавливаем свойство ReadOnly в true
                richTextBox1.ReadOnly = true;
            }
            catch (Exception ex)
            {
                // В случае ошибки выводим сообщение
                MessageBox.Show("Ошибка при чтении файла: " + ex.Message);
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear();
        }
    }
}
