using System;
using System.IO;


namespace application
{
    class Program
    {
        static void Main(string[] args)
        {
            StreamReader sr = new StreamReader("toParse.txt");
            Token token;
            while(sr.Peek() != -1){
                application.Scanner theScanner = new application.Scanner(sr);
                token = theScanner.detectToken();
                Console.WriteLine(token.ToString());
            }
            sr.Close();
            Console.WriteLine("Hello World!");
        }
    }

    class Scanner{
        States state = States.INITIAL_STATE;
        StreamReader reader;
        string thisToken = "It Worked!";
        int currentLength = 0;
        Token scannedToken;
        public Scanner(StreamReader passedReader){
            reader = passedReader;
        }
        public void printToken(){
            Console.WriteLine(thisToken);
        }
        public void printPeek(){
            Console.WriteLine((char)reader.Read());
        }
        public Token detectToken(){
            
            char[] printString = new char[50];
            string s = "";
            int i = 0;
            while(reader.Peek() == 32){
                reader.Read();
            }
            while(nextIsNewLineChar()){
                reader.Read();
            }
            while((char)reader.Peek() == '#'){
                //line is a comment
                processComment();
            }
            switch(state){
                case States.INITIAL_STATE:
                    if(47 < reader.Peek() && reader.Peek() < 58){
                        //checking if token is a constant
                        s += reader.Read(); 
                        state = States.CONSTANT_LENGTH_1;
                    }
                    break;
                case States.CONSTANT_LENGTH_1:
                    if(47 < reader.Peek() && reader.Peek() < 58){
                        //checking if token is a constant
                        s += reader.Read(); 
                        state = States.CONSTANT_LENGTH_2;
                    }
                    break;
                case States.CONSTANT_LENGTH_2:
                    if(47 < reader.Peek() && reader.Peek() < 58){
                        //checking if token is a constant
                        s += reader.Read(); 
                        state = States.CONSTANT_LENGTH_3;
                    }
                    break;
                case States.CONSTANT_LENGTH_3:
                    if(47 < reader.Peek() && reader.Peek() < 58){
                        //checking if token is a constant
                        s += reader.Read(); 
                        state = States.CONSTANT_LENGTH_4;
                    }
                    break;
                case States.CONSTANT_LENGTH_4:
                    if(47 < reader.Peek() && reader.Peek() < 58){
                        //checking if token is a constant
                        s += reader.Read(); 
                        state = States.CONSTANT_LENGTH_5;
                    }
                    break;
                case States.CONSTANT_LENGTH_5:
                    if(47 < reader.Peek() && reader.Peek() < 58){
                        //checking if token is a constant
                        s += reader.Read(); 
                        state = States.CONSTANT_LENGTH_6;
                    }
                    break;
                case States.CONSTANT_LENGTH_6:
                    if(47 < reader.Peek() && reader.Peek() < 58){
                        //checking if token is a constant
                        s += reader.Read(); 
                        state = States.CONSTANT;
                    }
                    break;
                case States.CONSTANT:
                    if(47 < reader.Peek() && reader.Peek() < 58){
                        //checking if token is a constant
                        s += reader.Read(); 
                        state = States.CONSTANT;
                    }
                    else if(nextCharIsSpecial()){
                        scannedToken = new Token(s, 2);
                        return scannedToken;
                    }
                    else if(nextCharIsWhitespace()){
                        //!!!!!!!!!!!!HERE!!!!!!!!!!!
                    }
                    break;
                default:
                    break;
            }



            int k = reader.Peek();
            //Console.WriteLine(reader.Peek());
            // if(47 < reader.Peek() && reader.Peek() < 58){
            //     //checking if token is a constant
            //     s = checkConstant();
            //     if(s != "#*ERROR*#"){
            //         scannedToken = new Token(s, 1);
            //         return scannedToken;
            //     }
            //     else{
            //         state = States.CONSTANT_ERROR;
            //         scannedToken = new Token(state.ToString(), 5);
            //         return scannedToken;
            //         //return s;
            //     }
            // }
            if(nextCharIsSpecial()){
                // checkString if token is a special character
                s = "" + (char)reader.Read();
                scannedToken = new Token(s, 4);
                return scannedToken;
            }
            else if(reader.Peek() == 34){
                //checking for quoted string
                s = checkString();
                if(s != "#*ERROR*#"){
                    //Console.WriteLine("......string......");
                    scannedToken = new Token(s, 2);
                    return scannedToken;
                }
                else{
                    state = States.GENERIC_ERROR;
                    scannedToken = new Token(state.ToString(), 5);
                    return scannedToken;
                }
            }
            else{
                while(reader.Peek()!=32 ){
                    if(reader.Peek() == -1){
                        break;
                    }
                    else if(!nextCharIsSpecial()){
                        printString[i] = (char)reader.Read();
                        i++;
                    }
                    else{
                        break;
                    }
                }
                state = States.SOMETHING_ELSE;
                s = new string(printString, 0, i);
                scannedToken = new Token(s, 0);
                return scannedToken;
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
                else if(nextCharIsSpecial()){
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
               // Console.Write("1000000 ");
                constString = "#*ERROR*#";
            }
            //check token seperators
            return constString;
           
        }

        public char makeLowerCase(){
            char returnChar = (char)(reader.Read() + 32);
            return returnChar;
        }
        public bool nextCharIsWhitespace(){
            bool isWhitespace = false;
            if(reader.Peek() == 32){
                isWhitespace = true;
            }
            if(reader.Peek() == 9){
                isWhitespace = true;
            }
            if(reader.Peek() == 10){
                isWhitespace = true;
            }
            if(reader.Peek() == 13){
                isWhitespace = true;
            }
            return isWhitespace;
        }
        public bool nextCharIsSpecial(){
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
            else if(nextIsNewLineChar()){
                //ASCII value of '\n' or '\r'
                isSpecial = true;
            }
            return isSpecial;
        }
        public string checkString(){
            string returnString = "";
            reader.Read();
            while(reader.Peek() != 34){
                if(nextCharIsSpecial()){
                    //detected end of token before quotation wasx closed
                    return "#*ERROR*#";
                }
                else{
                    returnString = returnString + (char)reader.Read();
                }
            }
            return returnString;
        }
        public void processComment(){
            reader.ReadLine();
        }
        public bool nextIsNewLineChar(){
            bool returnBool = false;
            if(reader.Peek() == 13 || reader.Peek() == 10){
                returnBool = true;
            }
            return returnBool;
        }
    }
    class Token{
        string lexeme;
        Type tokenType;
        public Token(string passedLexeme, int passedType){
            lexeme = passedLexeme;
            tokenType = (Type)passedType;
        }
        public string Lexeme{
            get{
                return lexeme;
            }
        }
        public string TokenType{
            get{
                return tokenType.ToString();
            }
        }
        override
        public string ToString(){
            return "Lexeme: " + lexeme + " Type: " + tokenType.ToString();
        }
    }
    enum Type {ID, CONSTANT, STRING, KEYWORD, SPECIAL_SYMBOL, ERROR};
    enum States{
        INITIAL_STATE,
        GENERIC_ERROR,
        //string states
        STRING_INVALID, STRING_VALID,
        //Constant states
        CONSTANT_LENGTH_1, CONSTANT_LENGTH_2, CONSTANT_LENGTH_3, CONSTANT_LENGTH_4, CONSTANT_LENGTH_5, CONSTANT_LENGTH_6, CONSTANT, CONSTANT_ERROR,
        SOMETHING_ELSE
    }
}
