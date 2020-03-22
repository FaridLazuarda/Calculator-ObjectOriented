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
    /********************************/
    /*             GUI              */
    /********************************/

    public partial class Form1 : Form
    {
        /** KAMUS DATA **/
        Double value = 0;
        String operation = "";
        bool operation_pressed = false;
        bool equal_pressed = false;
        bool exception_raised = false;
        Queue<Elemen<string>> operationQueue;
        QueueProcessor queueOp;
        Double ans = 0;

        Queue<Elemen<string>> memory;

        /** DEFAULT CONSTRUCTOR **/
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
            /** DESKRIPSI **/
            /* button_click menangani input operand (angka 0 - 9) */
            
            /** KAMUS DATA **/
            Button b = (Button)sender;

            /** ALGORITMA **/
            if (exception_raised)
            {
                result.Clear();
                result.Font = new Font(result.Font.FontFamily, 18, result.Font.Style);
                exception_raised = false;
            }
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
            /** DESKRIPSI **/
            /* operator_click menangani input operator (+, -, *, /, dan akar) */
            
            /** KAMUS DATA **/
            Elemen<String> clicked;
            Button b = (Button)sender;
            bool Eq = (b.Text == "=");

            /** ALGORITMA **/
            operation = b.Text;
            if (exception_raised)
            {
                result.Clear();
                result.Font = new Font(result.Font.FontFamily, 18, result.Font.Style);
                result.Text = "0";
                exception_raised = false;
            }
            value = Double.Parse(result.Text);
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
                else if(operation_pressed)
                {
                    equation.Text += " " + operation;
                }
                else
                {
                    equation.Text += " " + value + " " + operation;

                }
            }

            /** memasukkan hasil pembacaan user ke dalam Queue  **/
            Elemen<String> operand = new Elemen<String>(value.ToString());
            if(operation == "√")
            {
                clicked = new Elemen<String>("akar");
                operationQueue.Enqueue(clicked);
            } else if (operation_pressed)
            {
                clicked = new Elemen<String>(operation);
                operationQueue.Enqueue(clicked);
            }
            else
            {
                clicked = new Elemen<String>(operation);
                operationQueue.Enqueue(operand);
                operationQueue.Enqueue(clicked);

            }
            operation_pressed = true;
        }

        private void button18_Click(object sender, EventArgs e)
        {
            /** DESKRIPSI **/
            /* button18_click menangani input operator = */
            
            /** KAMUS DATA **/
            double solveRes;

            /** ALGORITMA **/
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
                result.Font = new Font(result.Font.FontFamily, 12, result.Font.Style);
                result.Text = exc.Message;
                exception_raised = true;
            }
            finally
            {
                equal_pressed = true;
            }
        }

        private void button15_Click(object sender, EventArgs e)
        {
            /** DESKRIPSI **/
            /* button15_click menangani input tombol "C" yakni clear*/
            
            /** ALGORITMA **/
            result.Clear();
            equation.Text = "";
            value = 0;
            result.Text = "0";
            operationQueue.Clear();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void ansButton_click(object sender, EventArgs e)
        {
            /** DESKRIPSI **/
            /* ansButton_click menangani input tombol "ans"*/
            
            /** ALGORITMA **/
            result.Clear();
            result.Text = ans.ToString();
        }

        private void mcButton_Click(object sender, EventArgs e)
        {
            /** DESKRIPSI **/
            /* mcButton_click menangani input tombol "MC"
            tombol ini akan memasukkan hasil yang ada pada layar dalam suatu Queue */
            
            /** KAMUS DATA **/
            double mcValue;
            Elemen<String> mcElemen;

            /** ALGORITMA **/
            if (!exception_raised)
            {
                mcValue = Double.Parse(result.Text);
                mcElemen = new Elemen<string>(result.Text);
                memory.Enqueue(mcElemen);
            }

        }

        private void mrButton_Click(object sender, EventArgs e)
        {
            /** DESKRIPSI **/
            /* mrButton_click menangani input tombol "MR"
            tombol ini akan menampilkan pada layar nilai simpanan yang disimpan MC */
            
            /** KAMUS DATA **/
            Elemen<string> mrValue;

            /** ALGORITMA **/
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
