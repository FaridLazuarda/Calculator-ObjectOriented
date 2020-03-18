using System;
using System.Collections;
using System.IO;
using ExpressionLibrary;

namespace StackLibrary{
    public class Stack{
        private Stack<Tuple<string,double>> stackOperation;

        public Stack(){
            stackOperation = new Stack<Tuple<string,double>>();
        }

        public void PushElmtStack(string elmt){
            if(elmt.Equals('+')){
                Tuple<string,double> elmtStack = new Tuple<string, double>(elmt,-1);
                this.stackOperation.Push(elmtStack);
            }else if(elmt.Equals('-')){
                Tuple<string,double> elmtStack = new Tuple<string, double>(elmt,-1);
                this.stackOperation.Push(elmtStack);
            }else if(elmt.Equals('*')){
                Tuple<string,double> elmtStack = new Tuple<string, double>(elmt,-1);
                this.stackOperation.Push(elmtStack);
            }else if(elmt.Equals('/')){
                Tuple<string,double> elmtStack = new Tuple<string, double>(elmt,-1);
                this.stackOperation.Push(elmtStack);
            }else if(elmt.Equals('akar')){
                Tuple<string,double> elmtStack = new Tuple<string, double>(elmt,-1);
                this.stackOperation.Push(elmtStack);
            }else{
                Tuple<string,double> elmtStack = new Tuple<string, double>(elmt,Convert.ToDouble(elmt));
                this.stackOperation.Push(elmtStack);
            }
        }
        public Tuple<string,double> PopElmtStack(){
            return this.stackOperation.Pop();
        }
    }
}