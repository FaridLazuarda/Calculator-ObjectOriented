using System;
using System.IO;

namespace CalculatorApp
{
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
