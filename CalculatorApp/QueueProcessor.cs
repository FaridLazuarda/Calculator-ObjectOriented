using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;


namespace CalculatorApp
{
    /********************************/
    /*         QUEUEPROCESSOR       */
    /********************************/
    public class QueueProcessor
    {
        /* KAMUS DATA */
        Queue<Elemen<string>> expressionInfixQueue;
        Queue<Elemen<string>> expressionPostfixQueue;

        /***** DEFAULT CONSTRUCTOR ******/
        public QueueProcessor()
        {
            /** DESKRIPSI **/


            /** KAMUS LOKAL **/

            /** ALGORITMA **/
            this.expressionInfixQueue = new Queue<Elemen<string>>();
            this.expressionPostfixQueue = new Queue<Elemen<string>>();
        }

        /***** Set Queue to Process *****/
        public void setQueue(Queue<Elemen<string>> queue)
        {
            /** DESKRIPSI **/


            /** KAMUS LOKAL **/

            /** ALGORITMA **/
            this.expressionInfixQueue = queue;
        }

        public double solveQueue()
        {
            /** DESKRIPSI **/
            /* Menyelesaikan ekspresi di dalam queue menjadi sebuah nilai */

            /** KAMUS LOKAL **/
            double result = 0;

            /** ALGORITMA **/
            try
            {
                parseInfixExpression();
                parseInfixToPostfix();
                result = solvePostfixQueue();
            }
            catch (CalculatorException e)
            {
                throw (e);
            }
            finally
            {
                this.expressionInfixQueue.Clear();
            }

            return result;
        }

        private void parseInfixExpression()
        {
            /** DESKRIPSI **/
            /* Me-parse ekspresi infix sekaligus mengecek semantik dari ekspresi tersebut */

            /** KAMUS LOKAL **/
            Queue<Elemen<string>> tempQueue;
            Elemen<string> tempElmt;
            int state;  // integer yang melambangkan keadaan pembacaan sekarang
                        // 0 -> pembacaan selesai dan berhasil
                        // 1 -> pembacaan terminal
                        // 2 -> pembacaan operator
            int negCount;   // Penanda banyak minus yang berurutan

            /** ALGORITMA **/
            tempQueue = new Queue<Elemen<string>>();
            negCount = 0;
            state = 1;  // dimulai dengan pembacaan terminal

            // 1. Loop sampai semua elemen InfixQueue habis
            while (this.expressionInfixQueue.Count != 0)
            {
                // 2. Setiap elemen di-dequeue dan ditangani sesuai state sekarang
                tempElmt = this.expressionInfixQueue.Dequeue();
                switch (state)
                {
                    // 2.1  State 1 menandakan state untuk membaca terminal
                    //      Kasus yang ada di sini adalah:
                    //      1. Jika yang ditemukan terminal, maka berarti tidak ada masalah dan langsung dimasukkan
                    //      2. Jika yang ditemukan operator, dicek terlebih dahulu operator apakah itu
                    //         Jika ditemukan operator unary (seperti akar), maka diperbolehkan
                    //         Kasus jika bertemu operator unary negatif yang pada program ini ditangani operator yang sama
                    //         dengan binary pengurangan, dilakukan penandaan bahwa sudah ada 1 minus agar pada pembacaan terminal
                    //         selanjutnya nilai negatif dapat dimasukkan ke queue (hanya jika ada 1 minus)
                    //      3. Jika yang ditemukan operator, tetapi bukan "-" ataupun "akar", maka secara langsung dapat
                    //         disimpulkan sintaks error dalam ekspresi dan dikeluarkan exception
                    case 1:
                        //  Penanganan terminal
                        if (tempElmt.GetItem1() == "#")
                        {

                            state = 2;
                            // Penanganan terminal yang sebelumnya operator negatif
                            if (negCount == 1)
                            {
                                tempElmt = new Elemen<string>((tempElmt.GetItem2() * -1).ToString());
                            }
                        }
                        else    // Penanganan Operator
                        {
                            if (tempElmt.GetItem1() == "-" && negCount < 1)
                            {
                                negCount++;
                                continue;
                            }
                            else if (tempElmt.GetItem1() == "akar")
                            {

                            }
                            else
                            {
                                throw (new ExpressionSyntaxErrorException("Syntax Error : Two Consecutive Operators"));
                            }
                        }
                        break;
                    // 2.2  State 2 menandakan state untuk membaca operator
                    //      State 2 cenderung lebih sederhana karena hanya tinggal membaca operator
                    //      Tidak dilakukan pembacaan terminal karena sudah ada prekondisi dari GUI
                    //      tidak ada dua terminal yang berurutan
                    //      Pada kasus ini hanya me-set state kembali menjadi 1 dan mereset negCount
                    case 2:
                        if (tempElmt.GetItem1() != "#")
                        {
                            state = 1;
                            negCount = 0;
                        }
                        break;
                }
                //  3. Elemen yang di dequeue disimpan sementara dalam queue temporary
                tempQueue.Enqueue(tempElmt);


            }

            //  4. Dilakukan pengopian kembali queue yang sudah benar dari temporary ke InfixQueue
            while (tempQueue.Count != 0)
            {
                tempElmt = tempQueue.Dequeue();
                this.expressionInfixQueue.Enqueue(tempElmt);
            }

            state = 0;

        }

        private void parseInfixToPostfix()
        {
            /** DESKRIPSI **/
            /* Mengubah ekspresi Infix pada queue menjadi Postfix */

            /** KAMUS DATA **/
            Stack<Elemen<string>> operatorStack;     // Stack untuk menyimpan urutan operator
            Elemen<string> queueTemp, stackTemp;
            int operatorPrecedence;

            /** ALGORITMA **/
            operatorStack = new Stack<Elemen<string>>();

            this.expressionPostfixQueue.Clear();

            while (this.expressionInfixQueue.Count != 0)
            {
                queueTemp = this.expressionInfixQueue.Dequeue();
                if (queueTemp.GetItem1() != "#")
                {
                    if (operatorStack.Count == 0)
                    {
                        operatorStack.Push(queueTemp);
                    }
                    else
                    {
                        do
                        {
                            stackTemp = operatorStack.Pop();
                            operatorPrecedence = checkOperatorPrecedence(stackTemp.GetItem1(), queueTemp.GetItem1());
                            if (operatorPrecedence != -1)
                            {
                                if (determineOperatorValue(stackTemp.GetItem1()) == 3 && determineOperatorValue(queueTemp.GetItem1()) == 3)
                                {
                                    operatorStack.Push(stackTemp);
                                    operatorStack.Push(queueTemp);
                                    break;
                                }
                                else
                                {
                                    this.expressionPostfixQueue.Enqueue(stackTemp);

                                }
                            }
                            else
                            {
                                operatorStack.Push(stackTemp);
                                operatorStack.Push(queueTemp);
                            }
                        }
                        while (operatorStack.Count != 0 && operatorPrecedence != -1);

                        if (operatorStack.Count == 0)
                        {
                            operatorStack.Push(queueTemp);
                        }

                    }
                }
                else
                {
                    this.expressionPostfixQueue.Enqueue(queueTemp);
                }
            }

            while (operatorStack.Count != 0)
            {
                stackTemp = operatorStack.Pop();
                this.expressionPostfixQueue.Enqueue(stackTemp);
            }

        }

        private double solvePostfixQueue()
        {
            /** DESKRIPSI **/
            /* Menyelesaikan ekspresi Postfix di queue menjadi nilai */

            /** KAMUS DATA **/
            Stack<TerminalExpression> operationStack;     // Stack untuk menyimpan nilai-nilai operasi
            Elemen<string> queueTemp;
            TerminalExpression term, term1, term2;
            Expression exp;

            /** ALGORITMA **/
            operationStack = new Stack<TerminalExpression>();
            while (this.expressionPostfixQueue.Count != 0)
            {
                queueTemp = this.expressionPostfixQueue.Dequeue();
                if (queueTemp.GetItem1() == "#")
                {
                    term = new TerminalExpression(queueTemp.GetItem2());
                    operationStack.Push(term);
                }
                else
                {
                    switch (queueTemp.GetItem1())
                    {
                        case "+":
                            term2 = operationStack.Pop();
                            term1 = operationStack.Pop();
                            exp = new AddExpression(term1, term2);
                            term = new TerminalExpression(exp.solve());
                            operationStack.Push(term);
                            break;
                        case "-":
                            term2 = operationStack.Pop();
                            term1 = operationStack.Pop();
                            exp = new SubstractExpression(term1, term2);
                            term = new TerminalExpression(exp.solve());
                            operationStack.Push(term);
                            break;
                        case "*":
                            term2 = operationStack.Pop();
                            term1 = operationStack.Pop();
                            exp = new MultiplyExpression(term1, term2);
                            term = new TerminalExpression(exp.solve());
                            operationStack.Push(term);
                            break;
                        case "/":
                            term2 = operationStack.Pop();
                            term1 = operationStack.Pop();
                            exp = new DivideExpression(term1, term2);
                            term = new TerminalExpression(exp.solve());
                            operationStack.Push(term);
                            break;
                        case "akar":
                            term1 = operationStack.Pop();
                            exp = new RootExpression(term1);
                            term = new TerminalExpression(exp.solve());
                            operationStack.Push(term);
                            break;
                    }
                }
            }

            term = operationStack.Pop();
            return term.solve();
        }

        public void printQueue(Queue<Elemen<string>> queue)
        {
            Queue<Elemen<string>> tempQueue = new Queue<Elemen<string>>();
            Elemen<string> temp;
            while (queue.Count != 0)
            {
                temp = queue.Dequeue();
                if (temp.GetItem1() != "#")
                {
                    if (temp.GetItem1() == "akar")
                    {
                        Console.Write("√");
                    }
                    else
                    {
                        Console.Write(temp.GetItem1());

                    }
                }
                else
                {
                    Console.Write(temp.GetItem2());
                }
                tempQueue.Enqueue(temp);
            }
            Console.WriteLine("");

            while (tempQueue.Count != 0)
            {
                temp = tempQueue.Dequeue();
                queue.Enqueue(temp);
            }
        }

        private int checkOperatorPrecedence(string x, string y)
        {
            /** DESKRIPSI **/
            /* Mengecek tingkat presedensi antara operator x dan y */
            /* return value: */
            /*  1 : x > y */
            /* -1 : x < y */
            /*  0 : x = y */

            /** KAMUS LOKAL **/
            int xValue, yValue; // Penanda nilai x dan y secara relatif

            /** ALGORITMA **/
            xValue = determineOperatorValue(x);
            yValue = determineOperatorValue(y);

            if (xValue > yValue)
            {
                return 1;
            }
            else if (xValue < yValue)
            {
                return -1;
            }
            else
            {
                return 0;
            }
        }

        private int determineOperatorValue(string x)
        {
            /** DESKRIPSI **/
            /* Memberikan nilai terhadap operator x secara relatif */
            /* return value: */
            /*  3 : √ */
            /*  2 : * / */
            /*  1 : + - */
            /*  0 : else */

            /** KAMUS LOKAL **/
            int value;

            /** ALGORITMA **/
            switch (x)
            {
                case ("akar"):
                    value = 3;
                    break;
                case ("*"):
                case ("/"):
                    value = 2;
                    break;
                case ("+"):
                case ("-"):
                    value = 1;
                    break;
                default:
                    value = 0;
                    break;
            }
            return value;
        }

    }
}