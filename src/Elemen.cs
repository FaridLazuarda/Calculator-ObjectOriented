using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;


namespace CalculatorApp
{
    public class Elemen<T>
    {
        /********************************/
        /*            ELEMEN            */
        /********************************/

        /** DESKRIPSI **/
        /* Class Elemen adalah class generic.
        Kelas ini memiliki dua atribut private yakni elmt1 dan elmt2. elmt2 bertipe double,
        Namun untuk elmt1 akan diinstansiasikan menjadi String dan double. Class Elemen ini 
        nantinya akan dipakai menjadi elemen dalam Queue untuk QueueProcessor, serta elemen 
        stack untuk StackProcessor */

        /** KAMUS DATA **/
        protected T elmt1;
        protected double elmt2;

        /** DEFAULT CONSTRUCTOR **/
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

        /** GETTER **/
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
