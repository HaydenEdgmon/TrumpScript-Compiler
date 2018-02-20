using System;
using System.IO;


namespace application
{
    class Program
    {
        static void Main(string[] args)
        {
            StreamReader sr = new StreamReader("toParse.txt");
            
            while(sr.Peek() != -1){
                application.Scanner theScanner = new application.Scanner(sr);
                string token = theScanner.detectToken();
                Console.WriteLine(token);
            }
            sr.Close();
            Console.WriteLine("Hello World!");
        }
    }

    class Scanner{
        StreamReader reader;
        string thisToken = "It Worked!";
        int currentLength = 0;
        public Scanner(StreamReader passedReader){
            reader = passedReader;
        }
        public void printToken(){
            Console.WriteLine(thisToken);
        }
        public void printPeek(){
            Console.WriteLine((char)reader.Read());
        }
        public string detectToken(){
            char[] printString = new char[50];
            string s = "";
            int i = 0;
            while(reader.Peek() == 32){
                reader.Read();
            }
            while(reader.Peek()!=32){
                if(reader.Peek() == -1){
                    break;
                }
                if(checkConstant()){
                    //printString[i] = (char)reader.Read();
                    Console.WriteLine("============================================");
                }
                else{
                    printString[i] = (char)reader.Read();
                    currentLength = -1;
                }
                i++;
                s = new string(printString);
            }
            return s;
        }

        public bool checkKeyword(){
            bool isKeyword = false;


            return isKeyword;
        }
        public string checkConstant(){
            string constString = "";
            //
            if((char)reader.Peek() =='1'){
                constString = constString + (char)reader.Read();
            }
            else if((char)reader.Peek() =='2'){
                constString = constString + (char)reader.Read();
            }
            else if((char)reader.Peek() =='3'){
                constString = constString + (char)reader.Read();
            }
            else if((char)reader.Peek() =='4'){
                constString = constString + (char)reader.Read();
            }
            else if((char)reader.Peek() =='5'){
                constString = constString + (char)reader.Read();
            }
            else if((char)reader.Peek() =='6'){
                constString = constString + (char)reader.Read();
            }
            else if((char)reader.Peek() =='7'){
                constString = constString + (char)reader.Read();
            }
            else if((char)reader.Peek() =='8'){
                constString = constString + (char)reader.Read();
            }
            else if((char)reader.Peek() =='9'){
                constString = constString + (char)reader.Read();
            }
            else if((char)reader.Peek() =='0'){
                //check if leading 0
                constString = constString + (char)reader.Read();
            }
            //check token seperators
            //check if letter and throw error
            return constString;
        }
    }
}
