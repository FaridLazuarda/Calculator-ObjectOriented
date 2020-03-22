using System;
using System.IO;

namespace CalculatorApp
{
    /********************************/
    /*          EXPRESSION          */
    /********************************/
    abstract public class Expression
    {
        /** DESKRIPSI **/
        /* Expression sebagai base class untuk Terminal, Binary, dan Unary Expression 
        Kelas-kelas turunan dari kelas ini yang akan melakukan operasi pada suatu ekspresi
        matematika, yang secara lebih lanjut akan diproses dalam StackProcessor */

        /** DEFAULT CONSTRUCTOR **/ 
        public Expression() { }

        /** abstract method solve() **/
        abstract public double solve();
    }

    public class TerminalExpression : Expression
    {
         /** KAMUS DATA **/
        protected double x;

        /** DEFAULT CONSTRUCTOR **/
        public TerminalExpression(double x)
        {
            this.x = x;
        }

        /** Implementasi abstract method solve() **/
        public override double solve()
        {
            return this.x;
        }
    }

    abstract public class UnaryExpression : Expression
    {
        /** DESKRIPSI **/
        /* UnaryExpression sebagai base class untuk Positive, Negative, dan Root Expression */
        /* hanya memiliki 1 atribut Expression x */

        /** KAMUS DATA **/
        protected Expression x;

        /** DEFAULT CONSTRUCTOR **/
        public UnaryExpression(Expression x)
        {
            this.x = x;
        }
        abstract public override double solve();
    }

    public class PositiveExpression : UnaryExpression
    {
        /** DEFAULT CONSTRUCTOR **/
        public PositiveExpression(Expression T) : base(T) { }

        /** Implementasi abstract method solve() **/
        public override double solve()
        {
            return x.solve();
        }
    }

    public class NegativeExpression : UnaryExpression
    {
        /** DEFAULT CONSTRUCTOR **/
        public NegativeExpression(Expression T) : base(T) { }

        /** Implementasi abstract method solve() **/
        public override double solve()
        {
            return (-1) * x.solve();
        }
    }

    public class RootExpression : UnaryExpression
    {
        /** DEFAULT CONSTRUCTOR **/
        public RootExpression(Expression T) : base(T) { }

        /** Implementasi abstract method solve() **/
        /* exception handling untuk akar bilangan negatif */
        public override double solve()
        {
            double result = Math.Sqrt(x.solve());
            if (!Double.IsNaN(result))
            {
                return result;
            }
            else
            {
                throw (new NegativeRootException("Exception: Negative value in Root"));
            }

        }
    }

    abstract public class BinaryExpression : Expression
    {
        /** DESKRIPSI **/
        /* Binary sebagai base class untuk Add, Substract, Multiply, dan Divide Expression */
        /* memiliki 2 atribut Expression, x dan y */

        /** KAMUS DATA **/
        protected Expression x;
        protected Expression y;

        /** DEFAULT CONSTRUCTOR **/
        public BinaryExpression(Expression x, Expression y)
        {
            this.x = x;
            this.y = y;
        }
        abstract public override double solve();
    }

    public class AddExpression : BinaryExpression
    {
        /** DEFAULT CONSTRUCTOR **/
        public AddExpression(Expression x, Expression y) : base(x, y) { }

        /** Implementasi abstract method solve() **/
        public override double solve()
        {
            return x.solve() + y.solve();
        }
    }

    public class SubstractExpression : BinaryExpression
    {
        /** DEFAULT CONSTRUCTOR **/
        public SubstractExpression(Expression x, Expression y) : base(x, y) { }

        /** Implementasi abstract method solve() **/
        public override double solve()
        {
            return x.solve() - y.solve();
        }
    }

    public class MultiplyExpression : BinaryExpression
    {
        /** DEFAULT CONSTRUCTOR **/
        public MultiplyExpression(Expression x, Expression y) : base(x, y) { }

        /** Implementasi abstract method solve() **/
        public override double solve()
        {
            return x.solve() * y.solve();
        }
    }

    public class DivideExpression : BinaryExpression
    {
        /** DEFAULT CONSTRUCTOR **/
        public DivideExpression(Expression x, Expression y) : base(x, y) { }

        /** Implementasi abstract method solve() **/
        /* Exception handling untuk pembagian dengan bilangan 0 */
        public override double solve()
        {
            double result;
            result = x.solve() / y.solve();
            if (Double.IsInfinity(result))
            {
                throw (new DivisionByZeroException("Exception: Division by Zero"));
            }
            return result;
        }
    }
}
