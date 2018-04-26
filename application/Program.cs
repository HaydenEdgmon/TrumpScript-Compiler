using System;
using System.IO;
using System.Collections.Generic;

using CompilerHelpers;
using Parsers;
using Scanners;

namespace Application
{
    class Program
    {
        static void Main(string[] args)
        {
                    // Bookkeeper bk = new Bookkeeper();
                    // ErrorHandler er = new ErrorHandler();
                    // Token token;
            //outputing source program
            Console.WriteLine("============================================================================");
            Console.WriteLine("Source Program: ");
            //creating single stream reader to be used throughout execution 
            StreamReader sr = new StreamReader("toParse.txt");
            while(sr.Peek() != -1){
                Console.Write((char)sr.Read());
            }
            Console.WriteLine("");
            Console.WriteLine("============================================================================");
            Console.WriteLine("SCANNER Output: ");
            Console.WriteLine("Action # \tStack Top \tLOKAHEAD \tACTION");
            Console.WriteLine("____________________________________________________________________________");
            //reset stream to 0 index to allow SCANNER to scan
            sr.DiscardBufferedData();
            sr.BaseStream.Seek(0, System.IO.SeekOrigin.Begin); 
            //create Parser object and call parse program
            Parser parser = new Parser(sr);
            parser.parseProgram();
            
            Console.WriteLine("============================================================================");
            sr.Close();
        }
    }   
    
}
