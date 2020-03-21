using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CalculatorApp
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main(String[] args)
        {
            Queue<QueueElmt> queueList = new Queue<QueueElmt>();
            QueueProcessor queueProcessor = new QueueProcessor();
            queueProcessor.setQueue(queueList);

            QueueElmt a = new QueueElmt("1");
            queueList.Enqueue(a);
            a = new QueueElmt("-");
            queueList.Enqueue(a);
            a = new QueueElmt("2");
            queueList.Enqueue(a);
            a = new QueueElmt("*");
            queueList.Enqueue(a);
            a = new QueueElmt("4");
            queueList.Enqueue(a);
            a = new QueueElmt("-");
            queueList.Enqueue(a);
            a = new QueueElmt("akar");
            queueList.Enqueue(a);
            a = new QueueElmt("akar");
            queueList.Enqueue(a);
            a = new QueueElmt("akar");
            queueList.Enqueue(a);
            a = new QueueElmt("256");
            queueList.Enqueue(a);

            Console.WriteLine(queueProcessor.solveQueue());
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
