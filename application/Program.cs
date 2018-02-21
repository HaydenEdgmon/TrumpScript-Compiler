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
            if(47 < reader.Peek() && reader.Peek() < 58){
                s = checkConstant();
                if(s != "#*ERROR*#"){
                    Console.WriteLine("+++++++++++++++++++++++");
                    return s;
                }
                else{
                    s = s + " IS NOT A CONSTANT";
                    return s;
                }
            }
            else if(isNextSpecialChar()){
                Console.WriteLine("The following is a token seperator");
                s = "" + (char)reader.Read();
                return s;
            }//check for string and comments
            else{
                while(reader.Peek()!=32 ){
                    if(reader.Peek() == -1){
                        break;
                    }
                    printString[i] = (char)reader.Read();
                    i++;
                    s = new string(printString);
                }
                return s;
            }         
            
        }

        public bool checkKeyword(){
            bool isKeyword = false;


            return isKeyword;
        }
        public string checkConstant(){
            string constString = "";
            while(reader.Peek() != 32){
                if((char)reader.Peek() =='1'){
                    currentLength++;
                    constString = constString + (char)reader.Read();
                }
                else if((char)reader.Peek() =='2'){
                    currentLength++;
                    constString = constString + (char)reader.Read();
                }
                else if((char)reader.Peek() =='3'){
                    currentLength++;
                    constString = constString + (char)reader.Read();
                }
                else if((char)reader.Peek() =='4'){
                    currentLength++;
                    constString = constString + (char)reader.Read();
                }
                else if((char)reader.Peek() =='5'){
                    currentLength++;
                    constString = constString + (char)reader.Read();
                }
                else if((char)reader.Peek() =='6'){
                    currentLength++;
                    constString = constString + (char)reader.Read();
                }
                else if((char)reader.Peek() =='7'){
                    currentLength++;
                    constString = constString + (char)reader.Read();
                }
                else if((char)reader.Peek() =='8'){
                    currentLength++;
                    constString = constString + (char)reader.Read();
                }
                else if((char)reader.Peek() =='9'){
                    currentLength++;
                    constString = constString + (char)reader.Read();
                }
                else if((char)reader.Peek() =='0'){
                    if(currentLength < 1){
                        //make error type
                        constString = "#*ERROR*#";
                        break;
                    }
                    else{
                        currentLength++;
                        constString = constString + (char)reader.Read();
                    }
                }
                else if(isNextSpecialChar()){
                    break;
                }
                else{
                    constString = "#*ERROR*#";
                    break;
                    //make error type
                    //not a constant, return error
                }
            }
            if(constString.Length < 7){
                constString = "#*ERROR*#";
            }
            else if(constString == "1000000"){
                Console.Write("1000000 ");
                constString = "#*ERROR*#";
            }
            //check token seperators
            return constString;
           
        }

        public char makeLowerCase(){
            char returnChar = (char)(reader.Read() + 32);
            return returnChar;
        }
        public bool isNextSpecialChar(){
            bool isSpecial = false;
            if(reader.Peek()==44){
                //ASCII value of ','
                isSpecial = true;
            }
            else if(reader.Peek()==59){
                //ASCII value of ';'
                isSpecial = true;
            }
            else if(reader.Peek()==58){
                //ASCII value of ':'
                isSpecial = true;
            }
            else if(reader.Peek()==33){
                //ASCII value of '!'
                isSpecial = true;
            }
            else if(reader.Peek()==63){
                //ASCII value of ';'
                isSpecial = true;
            }
            else if(reader.Peek()==40){
                //ASCII value of '('
                isSpecial = true;
            }
            else if(reader.Peek()==41){
                //ASCII value of ')'
                isSpecial = true;
            }
            return isSpecial;
        }
    }
}
