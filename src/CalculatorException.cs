using System;
using System.Collections.Generic;


namespace CalculatorApp
{
    public class CalculatorException : Exception
    {
        /********************************/
        /*     CALCULATOR EXCEPTION     */
        /********************************/

        /* CalculatorException merupakan base class untuk ExpressionSyntaxErrorException,
        NegativeRootException, dan DivisionByZeroException */

        public CalculatorException(string message) : base(message)
        {

        }
    }
    public class ExpressionSyntaxErrorException : CalculatorException
    {
        /* ExpressionSyntaxErrorException akan menghandle exception untuk penulisan expression 
        yang salah, misalnya dua operator ditulis secara consecutive */
        public ExpressionSyntaxErrorException(string message) : base(message)
        {

        }
    }

    public class NegativeRootException : CalculatorException
    {
        /* NegativeRootException menghandle exception untuk ekspresi dengan akar dari bilangan
        negatif */
        public NegativeRootException(string message) : base(message)
        {
        }
    }

    public class DivisionByZeroException : CalculatorException
    {
        /* DivisionByZeroException menghandle exception untuk ekspresi yang menghasilkan
        pembagian dengan bilangan 0 */
        public DivisionByZeroException(string message) : base(message)
        {

        }
    }
}
