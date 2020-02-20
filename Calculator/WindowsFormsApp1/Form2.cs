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
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void BackButton_Click(object sender, EventArgs e) //Возврат на 1-ю форму
        {
            Form1 form1 = new Form1();
            form1.Show();
            Hide();
        }

        private void ClearButton_Click(object sender, EventArgs e) //Очистка текстового поля ввода
        {
            calculatorTextBox.Clear();
            calculatorTextBox.Focus();
        }

        private void CalculatorTextBox_TextChanged(object sender, EventArgs e)
        {
            string text = calculatorTextBox.Text;
            if (!calculatorTextBox.Text.Equals("") && calculatorTextBox.Text.EndsWith("=")) //При вводе '=',
            {
                text = text.Replace(" ", ""); //убираем из выражения пробелы
                text = text.Replace("=", ""); //и '=',
                calculatorTextBox.Text += CalculateRPF(DoRPF(text)); //вычисляем выражение и дописываем результат в поле
            }
        }

        public static string DoRPF(string input) //Преобразование выражения в Обратную Польскую Форму (ОПФ)
        {
            string output = "";
            Stack<char> operatorsStack = new Stack<char>();
            for (int i = 0; i < input.Length; i++) //Перебираем входную строку
            {
                if (Char.IsDigit(input[i])) //Если встречена цифра,
                {
                    while (!IsOperator(input[i])) //то идём по цифрам, пока не встретим оператор
                    {
                        output += input[i]; //Запись цифр числа в итоговую строку
                        i++; //Переход к следующему элементу
                        if (i == input.Length) //Если вышли за границы входной строки,
                        {
                            break; //то выходим из цикла
                        }
                    }
                    output += " "; //Отделение записанного в итоговую строку числа пробелом
                    i--; //Компенсация использования 'i++' в цикле 'while'
                }
                if (IsOperator(input[i])) //Если встречен оператор
                {
                    if (input[i] == '(') //Если встречена открывающая скобка,
                    {
                        operatorsStack.Push(input[i]); //то записываем её в стек
                    }
                    else
                    {
                        if (input[i] == ')') //Если встречена закрывающая скобка,
                        { //то выписываем операторы из стека в итоговую строку, пока не встретим открывающую скобку. В итоге эта пара скобок исчезнет
                            char c = operatorsStack.Pop();
                            while (c != '(')
                            {
                                output += c;
                                c = operatorsStack.Pop();
                            }
                        }
                        else //Если встречен оператор, но не скобки
                        {
                            if (operatorsStack.Count > 0) //Если в стеке есть операторы
                            {
                                if (GetRank(operatorsStack.Peek()) >= GetRank(input[i])) //Если ранг верхнего оператора стека больше или равен рангу встреченного оператора,
                                {
                                    output += operatorsStack.Pop(); //то верхний оператор стека записываем в итоговую строку
                                    i--; //На следующем шаге обращаемся к тому же встреченному оператору, т. к. с ним не производилось действий
                                }
                                else //Если ранг верхнего оператора стека меньше ранга встреченного оператора,
                                {
                                    operatorsStack.Push(input[i]); //то добавляем встреченный оператор в стек
                                }
                            }
                            else //Если в стеке пусто,
                            {
                                operatorsStack.Push(input[i]); //то добавляем встреченный оператор в стек
                            }
                        }
                    }
                }
            }
            //После прохода по входной строке, в стеке остались операторы, и нужно их достать
            while (operatorsStack.Count > 0) //Пока в стеке ещё есть операторы
            {
                output += operatorsStack.Pop(); //Добавляем оператор в итоговую строку
            }
            return output; //Возвращаем итоговую строку
        }

        public static bool IsOperator(char x) //Определение: является символ оператором или нет
        {
            if ("()+-*/".IndexOf(x) == -1) //Если символа нет в строке, состоящей из операторов,
            {
                return false; //то он не оператор,
            }
            else
            {
                return true; //иначе он оператор
            }
        }

        public static int GetRank(char x) //Получение ранга оператора
        {
            switch (x)
            {
                case '(': return 0;
                case '+':
                case '-': return 1;
                case '*':
                case '/': return 2;
                default: return 3;
            }
        }

        public static double CalculateRPF(string input) //Вычисление выражения в ОПФ
        {
            double result = 0;
            Stack<double> numbersStack = new Stack<double>();
            for (int i = 0; i < input.Length; i++) //Перебираем входную строку
            {
                if (Char.IsDigit(input[i])) //Если встретили цифру,
                {
                    string number = "";
                    while (input[i] != ' ') //то, пока не встретим пробел,
                    {
                        number += input[i]; //собираем из цифр число
                        i++; //Переход к следующему элементу
                    }
                    numbersStack.Push(double.Parse(number)); //После цикла, кладём собранное число в стек
                }
                if (IsOperator(input[i])) //Если встретили оператор,
                {
                    double b = numbersStack.Pop(); //то достаём из стека 2-й операнд (число)
                    double a = numbersStack.Pop(); //и 1-й операнд
                    double r = 0;
                    switch (input[i]) //В зависимости от оператора, выполняем действие над операндами
                    {
                        case '+': r = a + b; break;
                        case '-': r = a - b; break;
                        case '*': r = a * b; break;
                        case '/': r = a / b; break;
                        default: break;
                    }
                    numbersStack.Push(r); //Результат действия кладём в стек
                }
            }
            result = numbersStack.Pop(); //После прохода по входной строке, достатаём из стека оставшийся результат
            return result; //Возвращаем результат
        }
    }
}
