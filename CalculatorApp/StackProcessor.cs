using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;


abstract public class Expression
{
    public Expression() { }
    abstract public double solve();
}

public class TerminalExpression : Expression
{
    protected double x;
    public TerminalExpression(double x)
    {
        this.x = x;
    }
    public override double solve()
    {
        return this.x;
    }
}

abstract public class UnaryExpression : Expression
{
    protected Expression x;
    public UnaryExpression(Expression x)
    {
        this.x = x;
    }
    abstract public override double solve();
}

public class PositiveExpression : UnaryExpression
{
    public PositiveExpression(Expression T) : base(T) { }
    public override double solve()
    {
        return x.solve();
    }
}

public class NegativeExpression : UnaryExpression
{
    public NegativeExpression(Expression T) : base(T) { }
    public override double solve()
    {
        return (-1) * x.solve();
    }
}

public class RootExpression : UnaryExpression
{
    public RootExpression(Expression T) : base(T) { }
    public override double solve()
    {
        return Math.Sqrt(x.solve());
    }
}

abstract public class BinaryExpression : Expression
{
    protected Expression x;
    protected Expression y;
    public BinaryExpression(Expression x, Expression y)
    {
        this.x = x;
        this.y = y;
    }
    abstract public override double solve();
}

public class AddExpression : BinaryExpression
{
    public AddExpression(Expression x, Expression y) : base(x, y) { }
    public override double solve()
    {
        return x.solve() + y.solve();
    }
}

public class SubstractExpression : BinaryExpression
{
    public SubstractExpression(Expression x, Expression y) : base(x, y) { }
    public override double solve()
    {
        return x.solve() - y.solve();
    }
}

public class MultiplyExpression : BinaryExpression
{
    public MultiplyExpression(Expression x, Expression y) : base(x, y) { }
    public override double solve()
    {
        return x.solve() * y.solve();
    }
}

public class DivideExpression : BinaryExpression
{
    public DivideExpression(Expression x, Expression y) : base(x, y) { }
    public override double solve()
    {
        return x.solve() / y.solve();
    }
}

public class Elemen<T> 
{
    protected T elmt1;
    protected double elmt2;
    public Elemen(T elmt)
    {
	Type param = typeof(T);	
	if (typeof(string).IsAssignableFrom(param))
	{
		if (elmt.Equals("+") || elmt.Equals("-") || elmt.Equals("*") || elmt.Equals("/") || elmt.Equals("akar"))
    		{
			elmt1 = elmt;
			elmt2 = -1;
    		}
    		else 
    		{
			string s = "#";
			elmt1 = (T)(object)s;
			elmt2 = Convert.ToDouble(elmt);
    		}
	}else{
		elmt1 = elmt;
		elmt2 = 1;
	}   
    }
    public T GetItem1()
    {
        return this.elmt1;
    }
    public double GetItem2()
    {
        return this.elmt2;
    } 
}

//namespace CalculatorDefinedException{
public class ExpressionSyntaxErrorException : Exception
{
    public ExpressionSyntaxErrorException(string message) : base(message)
    {

    }
}
//}


/********************************/
/*         QUEUEPROCESSOR       */
/********************************/
public class QueueProcessor
{
    /* KAMUS DATA */
    Queue<Elemen<string>> expressionInfixQueue;
    Queue<Elemen<string>> expressionPostfixQueue;
    TerminalExpression terminal1, terminal2;
    String expresionOp;
    int terminalState;

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
            printQueue(this.expressionInfixQueue);
            parseInfixToPostfix();
            result = solvePostfixQueue();
        }
        catch (ExpressionSyntaxErrorException e)
        {
            Console.WriteLine(e);
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
        String operatorString;
        int state;  // integer yang melambangkan keadaan pembacaan sekarang
                    // 0 -> pembacaan selesai dan berhasil
                    // 1 -> pembacaan terminal
                    // 2 -> pembacaan operator
        int negCount;   // Penanda banyak minus yang berurutan
        bool insert;

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
                            Console.WriteLine("neg");
                            tempElmt = new Elemen<string>((tempElmt.GetItem2() * -1).ToString());
                        }
                        Console.WriteLine("angka {0}", tempElmt.GetItem2());
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
            Console.WriteLine("< {0} , {1} >", tempElmt.GetItem1(), tempElmt.GetItem2());
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

        clearQueue(ref expressionPostfixQueue);

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
        printQueue(this.expressionPostfixQueue);
        operationStack = new Stack<TerminalExpression>();
        while (this.expressionPostfixQueue.Count != 0)
        {
            queueTemp = this.expressionPostfixQueue.Dequeue();
            if (queueTemp.GetItem1() == "#")
            {
                term = new TerminalExpression(queueTemp.GetItem2());
                operationStack.Push(term);
                Console.WriteLine(queueTemp.GetItem2());
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
            Console.WriteLine("< " + temp.GetItem1() + " , " + temp.GetItem2() + " >");
            tempQueue.Enqueue(temp);
        }

        while (tempQueue.Count != 0)
        {
            temp = tempQueue.Dequeue();
            queue.Enqueue(temp);
        }
    }

    private void clearQueue(ref Queue<Elemen<string>> queue)
    {
        /** DESKRIPSI **/
        /* Mengosongkan Queue sehingga dapat digunakan seperti awal */

        /** KAMUS DATA **/
        Elemen<string> temp;

        /** ALGORITMA **/
        while (queue.Count != 0)
        {
            temp = queue.Dequeue();
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


//public class StackProcessorTest
//{
//    public static void Main(String[] args)
//    {
//        //Stack<Elemen<string>> stackList = new Stack<Elemen<string>>();
//        //StackProcessor stackProcessor = new StackProcessor();
//
//        Queue<Elemen<string>> queueList = new Queue<Elemen<string>>();
//        QueueProcessor queueProcessor = new QueueProcessor();
//        queueProcessor.setQueue(queueList);
//
//        //Elemen<string> a = new Elemen<string>("-");
///        //queueList.Enqueue(a); 
//        //Elemen<string> a = new Elemen<string>("-");
//        Elemen<string> a = new Elemen<string>("1");
//        queueList.Enqueue(a);
//        a = new Elemen<string>("+");
//        queueList.Enqueue(a);
//        a = new Elemen<string>("-");
//        queueList.Enqueue(a);
//        a = new Elemen<string>("2");
//        queueList.Enqueue(a);
//        a = new Elemen<string>("-");
//        queueList.Enqueue(a);
//        a = new Elemen<string>("3");
//       queueList.Enqueue(a);

//        Console.WriteLine(queueProcessor.solveQueue());

//    }
//}