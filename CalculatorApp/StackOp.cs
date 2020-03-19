using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

    public class StackOp{
        
        public static void Main(String[] args){
            Stack<StackElmt> a = new Stack<StackElmt>();
            StackElmt elm1 = new StackElmt("123");
            StackElmt elm2 = new StackElmt("+");
            a.Push(elm1);
            a.Push(elm2);
            StackElmt elmt = a.Pop();
            Console.WriteLine(elmt.GetItem1()); 
            Console.WriteLine(elmt.GetItem2());
            elmt = a.Pop();
            Console.WriteLine(elmt.GetItem1()); 
            Console.WriteLine(elmt.GetItem2());
        }
    }

    class StackElmt{
        public Tuple<string,double> elmtStack;
        public StackElmt(string elmt){
            if(elmt.Equals("+")){
                elmtStack = new Tuple<string, double>(elmt,-1);
            }else if(elmt.Equals("-")){
                elmtStack = new Tuple<string, double>(elmt,-1);
            }else if(elmt.Equals("*")){
                elmtStack = new Tuple<string, double>(elmt,-1);
            }else if(elmt.Equals("/")){
                elmtStack = new Tuple<string, double>(elmt,-1);
            }else if(elmt.Equals("akar")){
                elmtStack = new Tuple<string, double>(elmt,-1);
            }else{
                elmtStack = new Tuple<string, double>("#",Convert.ToDouble(elmt));
            }
        }
        public string GetItem1(){
            return this.elmtStack.Item1;
        }
        public double GetItem2(){
            return this.elmtStack.Item2;
        }
    }
