using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void NextButton_Click(object sender, EventArgs e) //Переход на 2-ю форму
        {
            if (confirmCheck.Checked)
            {
                Form2 form2 = new Form2();
                form2.Show();
                Hide();
            }
            else
            {
                confirmationTextBox.Text = "Вы не согласны. До свидания.";
            }
        }

        private void ExitButton_Click(object sender, EventArgs e) //Закрытие окна
        {
            Close();   
        }
    }
}
