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
            theScanner.printToken();
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
    }
}
