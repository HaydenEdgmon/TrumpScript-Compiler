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
                if(currentLength != -1){
                    if(checkConstant()){
                        printString[i] = (char)reader.Read();
                        Console.WriteLine("============================================");
                    }
                    else{
                        printString[i] = (char)reader.Read();
                        currentLength = -1;
                    }
                }
                else{
                    currentLength = -1;
                    printString[i] = (char)reader.Read();
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
        public bool checkConstant(){
            bool isConstant = false;
            if((char)reader.Peek() =='1'){
                isConstant = true;
            }
            // case((char)reader.peek()){
            //     case '0':
            //         if(currentLength < 1){
            //             Console.WriteLine("@@@@@@@Not a constant@@@@@");
            //             isConstant = false;
            //         }
            //         else{
            //             currentLength++;
            //             isConstant = true;
            //         }
            //         break;
            //     case '1':
            //         currentLength++;
            //         isConstant = true;
            //         break;
            //     case '2':
            //         currentLength++;
            //         isConstant = true;
            //         break;
            //     case '3':
            //         currentLength++;
            //         isConstant = true;
            //         break;
            //     case '4':
            //         currentLength++;
            //         isConstant = true;
            //         break;
            //     case '5':
            //         currentLength++;
            //         isConstant = true;
            //         break;
            //     case '6':
            //         currentLength++;
            //         isConstant = true;
            //         break;
            //     case '7':
            //         currentLength++;
            //         isConstant = true;
            //         break;
            //     case '8':
            //         currentLength++;
            //         isConstant = true;
            //         break;
            //     case '9':
            //         currentLength++;
            //         isConstant = true;
            //         break;
            //     case ' ':
            //         if(currentLength > 6){
            //             isConstant = true;
            //         }
            //         else{
            //             Console.WriteLine("@@@@@@@Not a constant@@@@@");
            //             isConstant = false;
            //         }
            //     default:
            //         Console.WriteLine("@@@@@@@Not a constant@@@@@");
            //         isConstant = false;
            //         break;
            // }
            return isConstant;
        }
    }
}
