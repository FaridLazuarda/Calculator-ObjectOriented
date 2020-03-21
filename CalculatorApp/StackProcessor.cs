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

public class QueueElmt
{
    public Tuple<string, double> elmtStack;
    public QueueElmt(string elmt)
    {
        if (elmt.Equals("+") || elmt.Equals("-") || elmt.Equals("*") || elmt.Equals("/") || elmt.Equals("akar"))
        {
            elmtStack = new Tuple<string, double>(elmt, -1);
        }
        else
        {
            elmtStack = new Tuple<string, double>("#", Convert.ToDouble(elmt));
        }
    }
    public string GetItem1()
    {
        return this.elmtStack.Item1;
    }
    public double GetItem2()
    {
        return this.elmtStack.Item2;
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
    Queue<QueueElmt> expressionInfixQueue;
    Queue<QueueElmt> expressionPostfixQueue;
    TerminalExpression terminal1, terminal2;
    String expresionOp;
    int terminalState;

    /***** DEFAULT CONSTRUCTOR ******/
    public QueueProcessor()
    {
        /** DESKRIPSI **/
        

        /** KAMUS LOKAL **/

        /** ALGORITMA **/
        this.expressionInfixQueue = new Queue<QueueElmt>();
        this.expressionPostfixQueue = new Queue<QueueElmt>();
    }

    /***** Set Queue to Process *****/
    public void setQueue(Queue<QueueElmt> queue)
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
        try{
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
        Queue<QueueElmt> tempQueue;
        QueueElmt tempElmt;
        String operatorString;
        int state;  // integer yang melambangkan keadaan pembacaan sekarang
                    // 0 -> pembacaan selesai dan berhasil
                    // 1 -> pembacaan terminal
                    // 2 -> pembacaan operator
        int negCount=0; //Penanda banyak minus yang berurutan

        /** ALGORITMA **/
        tempQueue = new Queue<QueueElmt>();
        state = 1;  // dimulai dengan pembacaan terminal

        while (this.expressionInfixQueue.Count != 0)
        {
            
            tempElmt = this.expressionInfixQueue.Dequeue();
            switch (state){
                case 1:
                    if (tempElmt.GetItem1() == "#")
                    {
                        state = 2;
                        Console.WriteLine("angka {0}", tempElmt.GetItem2());
                    }
                    else
                    {
                        if (tempElmt.GetItem1() == "-" && negCount < 1)
                        {
                            negCount++;
                        }
                        else if(tempElmt.GetItem1() == "akar")
                        {
                            
                        }
                        else
                        {
                            throw (new ExpressionSyntaxErrorException("Syntax Error : Two Consecutive Operators"));
                        }
                    }
                    break;
                case 2:
                    if(tempElmt.GetItem1() != "#")
                    {
                        state = 1;
                        negCount = 0;
                        Console.WriteLine("operator {0}", tempElmt.GetItem1());

                    }
                    break;
            }
            tempQueue.Enqueue(tempElmt);
                
        }

        while(tempQueue.Count != 0)
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
        Stack<QueueElmt> operatorStack;     // Stack untuk menyimpan urutan operator
        QueueElmt queueTemp, stackTemp;
        int operatorPrecedence;

        /** ALGORITMA **/
        operatorStack = new Stack<QueueElmt>();
        
        clearQueue(ref expressionPostfixQueue);
        
        while(this.expressionInfixQueue.Count != 0)
        {
            queueTemp = this.expressionInfixQueue.Dequeue();
            if(queueTemp.GetItem1() != "#")
            {
                if(operatorStack.Count == 0)
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

                    if(operatorStack.Count == 0)
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

        while(operatorStack.Count != 0)
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
        QueueElmt queueTemp;
        TerminalExpression term, term1, term2;
        Expression exp;

        /** ALGORITMA **/
        printQueue(this.expressionPostfixQueue);
        operationStack = new Stack<TerminalExpression>();
        while(this.expressionPostfixQueue.Count != 0)
        {
            queueTemp = this.expressionPostfixQueue.Dequeue();
            if(queueTemp.GetItem1() == "#")
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

    public void printQueue(Queue<QueueElmt> queue)
    {
        Queue<QueueElmt> tempQueue = new Queue<QueueElmt>();
        QueueElmt temp;
        while(queue.Count != 0)
        {
            temp = queue.Dequeue();
            Console.WriteLine("< " + temp.GetItem1() + " , " + temp.GetItem2() + " >");
            tempQueue.Enqueue(temp);
        }

        while(tempQueue.Count != 0)
        {
            temp = tempQueue.Dequeue();
            queue.Enqueue(temp);
        }
    }

    private void clearQueue(ref Queue<QueueElmt> queue)
    {   
        /** DESKRIPSI **/
        /* Mengosongkan Queue sehingga dapat digunakan seperti awal */

        /** KAMUS DATA **/
        QueueElmt temp;
        
        /** ALGORITMA **/
        while(queue.Count != 0)
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

        if(xValue > yValue)
        {
            return 1;
        } else if(xValue < yValue)
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