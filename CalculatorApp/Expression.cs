using System;
using System.IO;

abstract public class Expression{
    public Expression(){}
    abstract public double solve();

        public static void Main(String[] args){
            // debugging
            TerminalExpresion a = new TerminalExpresion(2);
            TerminalExpresion b = new TerminalExpresion(3);

            AddExpression c = new AddExpression(a, b);
            SubstractExpression d = new SubstractExpression(a, b);
            MultiplyExpression e = new MultiplyExpression(a, b);
            DivideExpression f = new DivideExpression(a, b);

            TerminalExpresion g = new TerminalExpresion(e.solve());
            DivideExpression h = new DivideExpression(g, b);

            Console.WriteLine(c.solve()); // a + b
            Console.WriteLine(d.solve()); // a - b
            Console.WriteLine(e.solve()); // a * b
            Console.WriteLine(f.solve()); // a / b
            Console.WriteLine(h.solve()); // (a * b) / b
        }
    }

    public class TerminalExpresion : Expression{
        protected double x;
        public TerminalExpresion(double x){
            this.x = x;
        }
        public override double solve(){
            return this.x;
        }
    }

abstract public class UnaryExpression : Expression{
    protected Expression x;
    public UnaryExpression(Expression x){
        this.x = x;
    }
    abstract public override double solve();
}

    public class PositiveExpression : UnaryExpression{
        public PositiveExpression(Expression T) : base(T){}
        public override double solve(){
            return x.solve();
        }
    }

    public class NegativeExpression : UnaryExpression{
        public NegativeExpression(Expression T) : base(T){}
        public override double solve(){
            return (-1) * x.solve();
        }
    }

    public class RootExpression : UnaryExpression{
        public RootExpression(Expression T) : base(T){}
        public override double solve(){
            return Math.Sqrt(x.solve());
        }
    }

abstract public class BinaryExpression : Expression{
    protected Expression x;
    protected Expression y;
    public BinaryExpression(Expression x, Expression y){
        this.x = x;
        this.y = y;
    }
    abstract public override double solve();
}

    public class AddExpression : BinaryExpression{
        public AddExpression(Expression x, Expression y) : base(x,y){}
        public override double solve(){
            return x.solve() + y.solve();
        }
    }

    public class SubstractExpression : BinaryExpression{
        public SubstractExpression(Expression x, Expression y) : base(x,y){}
        public override double solve(){
            return x.solve() - y.solve();
        }
    }

    public class MultiplyExpression : BinaryExpression{
        public MultiplyExpression(Expression x, Expression y) : base(x,y){}
        public override double solve(){
            return x.solve() * y.solve();
        }
    }

    public class DivideExpression : BinaryExpression{
        public DivideExpression(Expression x, Expression y) : base(x,y){}
        public override double solve(){
            return x.solve() / y.solve();
        }
    }