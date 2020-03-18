using System;
using System.Collections;
using System.IO;
using ExpressionLibrary;

namespace QueueLibrary{
    public class Queue{
        private Queue<double> queueOperation;

        public Queue(){
            queueOperation = new Queue<double>();
        }
        public addQueue(double elmt){

            this.queueOperation.Enqueue(elmt);
        }
        public double popQueue(){
            return this.queueOperation.Dequeue();
        }
    }
}