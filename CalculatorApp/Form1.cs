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
        bool equal_pressed = false;
        Queue<Elemen<string>> operationQueue;
        QueueProcessor queueOp;
        Double ans = 0;

        Queue<Elemen<string>> memory;

        public Form1()
        {
            InitializeComponent();
            operationQueue = new Queue<Elemen<string>>();
            queueOp = new QueueProcessor();
            queueOp.setQueue(operationQueue);
            memory = new Queue<Elemen<string>>();
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
                if ((result.Text == "0") || (operation_pressed) || (equal_pressed))
                {
                    result.Clear();
                    if (equal_pressed)
                    {
                        equation.Text = "";
                    }
                }
                result.Text = result.Text + b.Text;
            }
            operation_pressed = false;
            equal_pressed = false;


        }


        private void operator_Click(object sender, EventArgs e)
        {
            Elemen<String> clicked;
            Button b = (Button)sender;
            bool Eq = (b.Text == "=");

            operation = b.Text;
            value = Double.Parse(result.Text);
            operation_pressed = true;
            if (equal_pressed)
            {
                if (operation == "√")
                {
                    equation.Text = " " + operation;
                }
                else
                {
                    equation.Text = value + " " + operation;

                }
                equal_pressed = false;
            }
            else
            {
                if(operation == "√")
                {
                    equation.Text += " " + operation;
                }
                else
                {
                    equation.Text += " " + value + " " + operation;

                }
            }

            Elemen<String> operand = new Elemen<String>(value.ToString());
            if(operation == "√")
            {
                clicked = new Elemen<String>("akar");
                operationQueue.Enqueue(clicked);
            }
            else
            {
                clicked = new Elemen<String>(operation);
                operationQueue.Enqueue(operand);
                operationQueue.Enqueue(clicked);

            }

        }

        private void button18_Click(object sender, EventArgs e)
        {
            double solveRes;
            value = Double.Parse(result.Text);
            equation.Text += " " + value;

            Elemen<String> operand = new Elemen<String>(value.ToString());
            operationQueue.Enqueue(operand);

            try
            {
                solveRes = queueOp.solveQueue();
                result.Text = (solveRes).ToString();
                ans = solveRes;
            }
            catch (CalculatorException exc)
            {
                result.Text = exc.Message;
            }
            finally
            {
                equal_pressed = true;
            }


        }

        private void button15_Click(object sender, EventArgs e)
        {
            result.Clear();
            equation.Text = "";
            value = 0;
            result.Text = "0";
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void ansButton_click(object sender, EventArgs e)
        {
            result.Clear();
            result.Text = ans.ToString();
        }

        private void mcButton_Click(object sender, EventArgs e)
        {
            double mcValue;
            Elemen<String> mcElemen;

            mcValue = Double.Parse(result.Text);
            mcElemen = new Elemen<string>(result.Text);

            memory.Enqueue(mcElemen);

        }

        private void mrButton_Click(object sender, EventArgs e)
        {
            Elemen<string> mrValue;

            try
            {
                mrValue = memory.Dequeue();
                result.Clear();
                result.Text = mrValue.GetItem2().ToString();

            } catch (InvalidOperationException exc)
            {
                
            }

        }
    }
}
