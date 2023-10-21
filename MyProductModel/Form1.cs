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
            s.PrintInfo();
            s.Run();
           
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

        private void Click()
        {
            Initialize();
            // Получение выбранных элементов
            List<object> selectedItems = new List<object>();

            foreach (object selectedItem in checkedListBox1.CheckedItems)
            {
                selectedItems.Add(selectedItem);
            }

            // Преобразование выбранных элементов в массив строк, если нужно
            string[] selectedValues = selectedItems.Select(item => item.ToString()).ToArray();

        }
        private void button1_Click(object sender, EventArgs e)
        {
            Initialize();
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
