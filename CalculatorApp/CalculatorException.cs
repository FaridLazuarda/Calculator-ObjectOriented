using System;
using System.Collections.Generic;


namespace CalculatorApp
{
    public class CalculatorException : Exception
    {
        public CalculatorException(string message) : base(message)
        {

        }
    }
    public class ExpressionSyntaxErrorException : CalculatorException
    {
        public ExpressionSyntaxErrorException(string message) : base(message)
        {

        }
    }

    public class NegativeRootException : CalculatorException
    {
        public NegativeRootException(string message) : base(message)
        {
        }
    }

    public class DivisionByZeroException : CalculatorException
    {
        public DivisionByZeroException(string message) : base(message)
        {

        }
    }
}
