using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CalculatorApp
{
    public partial class Form1 : Form
    {
        Double value = 0;
        String operation = "";
        bool operation_pressed = false;
        Queue<Elemen<string>> operationQueue;
        QueueProcessor queueOp;
        public Form1()
        {
            InitializeComponent();
            operationQueue = new Queue<Elemen<string>>();
            queueOp = new QueueProcessor();
            queueOp.setQueue(operationQueue);
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button_Click(object sender, EventArgs e)
        {
            
            Button b = (Button)sender;
            if (b.Text == ".")
            {
                if (!result.Text.Contains("."))
                {
                    result.Text = result.Text + b.Text;
                }
            }
            else
            {
                if ((result.Text == "0") || (operation_pressed))
                {
                    result.Clear();
                }
                result.Text = result.Text + b.Text;
            }
            operation_pressed = false;


        }

        private void button16_Click(object sender, EventArgs e)
        {

        }

        private void operator_Click(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            bool Eq = (b.Text == "=");

            operation = b.Text;
            value = Double.Parse(result.Text);
            operation_pressed = true;
            equation.Text += " " + value + " " + operation;

            Elemen<String> operand = new Elemen<String>(value.ToString());
            Elemen<String> clicked = new Elemen<String>(operation);
            operationQueue.Enqueue(operand);
            if (!Eq)
            {
                Console.WriteLine("masuk");
                operationQueue.Enqueue(clicked);
            }
            else
            {
                Console.WriteLine("hasil");
                Console.WriteLine(queueOp.solveQueue());
            }

            /* while (!Eq)
             {
                 Console.WriteLine("masuk");
                 operation = b.Text;
                 value = Double.Parse(result.Text);
                 operation_pressed = true;
                 equation.Text = value + " " + operation;

                 Elemen<string> operand = new Elemen<String>(value.ToString());
                 Elemen<string> clicked = new Elemen<string>(b.Text);
                 operationQueue.Enqueue(operand);

                 if (b.Text == "=")
                 {
                     Eq = true;
                 }
                 else
                 {
                     operationQueue.Enqueue(clicked);
                 }
             }*/
        }

        private void button18_Click(object sender, EventArgs e)
        {
            value = Double.Parse(result.Text);
            equation.Text += " " + value + " " + operation;

            Elemen<String> operand = new Elemen<String>(value.ToString());
            operationQueue.Enqueue(operand);
            Console.WriteLine("hasil");
            double solveRes = queueOp.solveQueue();
            Console.WriteLine(solveRes);
            result.Text = (solveRes).ToString();

            /*equation.Text = "";
            switch (operation)
            {
                case "+":
                    result.Text = (value + Double.Parse(result.Text)).ToString();
                    break;
                case "-":
                    result.Text = (value - Double.Parse(result.Text)).ToString();
                    break;
                case "*":
                    result.Text = (value * Double.Parse(result.Text)).ToString();
                    break;
                case "/":
                    result.Text = (value / Double.Parse(result.Text)).ToString();
                    break;
                default:
                    break;
            }//end switch*/
        }

        private void button15_Click(object sender, EventArgs e)
        {
            result.Clear();
            value = 0;
            result.Text = "0";
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
