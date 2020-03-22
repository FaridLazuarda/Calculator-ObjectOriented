using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;


namespace CalculatorApp
{
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
            }
            else
            {
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
}
