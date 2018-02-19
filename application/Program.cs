using System;
using System.IO;


namespace application
{
    class Program
    {
        static void Main(string[] args)
        {
            StreamReader sr = new StreamReader("toParse.txt");
            application.Scanner theScanner = new application.Scanner(sr);
            while(sr.Peek() != -1){
                if(sr.Peek() == 32){
                    sr.Read();
                }
                else{
                    theScanner.printSeperatedBySpace();
                }
            }
            sr.Close();
            Console.WriteLine("Hello World!");
        }
    }

    class Scanner{
        StreamReader reader;
        string thisToken = "It Worked!";
        public Scanner(StreamReader passedReader){
            reader = passedReader;
        }
        public void printToken(){
            Console.WriteLine(thisToken);
        }
        public void printPeek(){
            Console.WriteLine((char)reader.Read());
        }
        public void printSeperatedBySpace(){
            char[] printString = new char[50];
            int i = 0;
            while(reader.Peek()!=32){
                printString[i] = (char)reader.Read();
                i++;
            }
            string s = new string(printString);
            Console.WriteLine(s);
        }
    }
}
