using System;
using System.IO;
using System.Collections.Generic;

using CompilerHelpers;
using Parsers;

namespace Application
{
    class Program
    {
        static void Main(string[] args)
        {
            //outputing source program
            Console.WriteLine("=======================================");
            Console.WriteLine("Source Program: ");
            //creating single stream reader to be used throughout execution 
            StreamReader sr = new StreamReader("toParse.txt");
            while(sr.Peek() != -1){
                Console.Write((char)sr.Read());
            }
            Console.WriteLine("");
            Console.WriteLine("=======================================");
            Console.WriteLine("SCANNER Output: ");
            //reset stream to 0 index to allow SCANNER to scan
            sr.DiscardBufferedData();
            sr.BaseStream.Seek(0, System.IO.SeekOrigin.Begin); 
            //create bookkeeper and error handler 
                    // Token token;
                    // while(sr.Peek() != -1){
                    //     //call new scanner for every detected Token
                    //     Scanner theScanner = new Scanner(sr, bk, er);
                    //     //scan and return a token from the scanner
                    //     token = theScanner.detectToken();
                    // }
            //output conents of the symble table in the Bookkeeper
            Console.WriteLine("=======================================");
            Console.WriteLine("SYMTAB Content: ");
                    // bk.printSymTab();
            Console.WriteLine("=======================================");
            sr.Close();
        }
    }
    //Scanner class, usesstate and scanned symbol to detect 5 types of tokens
    
    
}
