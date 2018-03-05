using System;
using System.IO;
using System.Collections.Generic;


namespace application
{
    class Program
    {
        static void Main(string[] args)
        {
            StreamReader sr = new StreamReader("toParse.txt");
            Bookkeeper bk = new Bookkeeper();
            Token token;
            while(sr.Peek() != -1){
                application.Scanner theScanner = new application.Scanner(sr, bk);
                token = theScanner.detectToken();
                Console.WriteLine(token.ToString());
            }
            bk.printSymTab();
            sr.Close();
            Console.WriteLine("Hello World!");
        }
    }

    class Scanner{
        States state = States.INITIAL_STATE;
        StreamReader reader;
        Bookkeeper bookkeeper;
        string thisToken = "It Worked!";
        Token scannedToken;
        public Scanner(StreamReader passedReader, Bookkeeper passedBookkeeper){
            reader = passedReader;
            bookkeeper = passedBookkeeper;
        }
        public void printToken(){
            Console.WriteLine(thisToken);
        }
        public void printPeek(){
            Console.WriteLine((char)reader.Read());
        }
        public Token detectToken(){
            string scannedString = "";
            scannedToken = new Token(States.GENERIC_ERROR.ToString(), Type.ERROR);
            if(nextCharIsWhitespace()){
                while(nextCharIsWhitespace() && reader.Peek() != -1){
                    reader.Read();
                }
            }
            if((char)reader.Peek() == '#'){
                while((char)reader.Peek() == '#'){
                    //a single line as a comment
                    processComment();
                }
            }
            if(nextCharIsSpecial()){
                state = States.SPECIAL_CHARACTER;
                scannedString += (char)reader.Read();
                scannedToken = new Token(scannedString, Type.SPECIAL_SYMBOL);
            }
            while(!isEndOfToken() && scannedToken.TokenType != Type.SPECIAL_SYMBOL){
                //Console.WriteLine(state.ToString());
                switch(state){
                    case States.INITIAL_STATE:
                        if(reader.Peek() == 34){
                            scannedString += (char)reader.Read();
                            state = States.STRING_INVALID;
                        }
                        else if(64 < reader.Peek() && reader.Peek() < 91){
                            //uppercase letter checking for first lertters of Keywords
                            if(reader.Peek() == 65){
                                scannedString += getLowerCaseofNextLetter();
                                state = States.KEYWORD_A;
                            }
                            else if(reader.Peek() == 77){
                                scannedString += getLowerCaseofNextLetter();
                                state = States.KEYWORD_M;
                            }
                            else{
                                scannedString += getLowerCaseofNextLetter();
                                state = States.ID;
                            }
                        }
                        else if(96 < reader.Peek() && reader.Peek() < 123){
                            //lowercase letter checking for first lertters of Keywords
                            if(reader.Peek() == 97){
                                scannedString += (char)reader.Read();
                                state = States.KEYWORD_A;
                            }
                            else if(reader.Peek() == 109){
                                scannedString += (char)reader.Read();
                                state = States.KEYWORD_M;
                            }
                            else{
                                scannedString += (char)reader.Read();
                                state = States.ID;
                            }
                        }
                        else if(47 < reader.Peek() && reader.Peek() < 58){
                            scannedString += (char)reader.Read();
                            state = States.CONSTANT_LENGTH_1;
                        }
                        else{
                            if(64 < reader.Peek() && reader.Peek() < 91){
                                scannedString += getLowerCaseofNextLetter();
                                state = States.ID;
                            }
                            else if((96 < reader.Peek() && reader.Peek() < 123) ||(47 < reader.Peek() && reader.Peek() < 58)){
                                scannedString += (char)reader.Read();
                                state = States.ID;
                            }
                        }
                        break;
                    case States.STRING_INVALID:
                        if(reader.Peek() != 34){
                            scannedString += (char)reader.Read();
                        }
                        else{
                            scannedString += (char)reader.Read();
                            state = States.STRING_VALID;
                        }
                        break;
                    case States.STRING_VALID:
                        if(reader.Peek() == 34){
                            scannedString += (char)reader.Read();
                            state = States.STRING_INVALID;
                        }
                        break;
                    case States.CONSTANT_LENGTH_1:
                        if(47 < reader.Peek() && reader.Peek() < 58){
                            scannedString += (char)reader.Read();
                            state = States.CONSTANT_LENGTH_2;
                        }
                        else{
                            scannedString += (char)reader.Read();
                            state = States.CONSTANT_ERROR;
                        }
                        break;
                    case States.CONSTANT_LENGTH_2:
                        if(47 < reader.Peek() && reader.Peek() < 58){
                            scannedString += (char)reader.Read();
                            state = States.CONSTANT_LENGTH_3;
                        }
                        else{
                            scannedString += (char)reader.Read();
                            state = States.CONSTANT_ERROR;
                        }
                        break;
                    case States.CONSTANT_LENGTH_3:
                        if(47 < reader.Peek() && reader.Peek() < 58){
                            scannedString += (char)reader.Read();
                            state = States.CONSTANT_LENGTH_4;
                        }
                        else{
                            scannedString += (char)reader.Read();
                            state = States.CONSTANT_ERROR;
                        }
                        break;
                    case States.CONSTANT_LENGTH_4:
                        if(47 < reader.Peek() && reader.Peek() < 58){
                            scannedString += (char)reader.Read();
                            state = States.CONSTANT_LENGTH_5;
                        }
                        else{
                            scannedString += (char)reader.Read();
                            state = States.CONSTANT_ERROR;
                        }
                        break;
                    case States.CONSTANT_LENGTH_5:
                        if(47 < reader.Peek() && reader.Peek() < 58){
                            scannedString += (char)reader.Read();
                            state = States.CONSTANT_LENGTH_6;
                        }
                        else{
                            scannedString += (char)reader.Read();
                            state = States.CONSTANT_ERROR;
                        }
                        break;
                    case States.CONSTANT_LENGTH_6:
                        if(47 < reader.Peek() && reader.Peek() < 58){
                            scannedString += (char)reader.Read();
                            state = States.CONSTANT;
                        }
                        else{
                            scannedString += (char)reader.Read();
                            state = States.CONSTANT_ERROR;
                        }
                        break;
                    case States.CONSTANT:
                        if(47 < reader.Peek() && reader.Peek() < 58){
                            scannedString += (char)reader.Read();
                            state = States.CONSTANT;
                        }
                        else{
                            scannedString += (char)reader.Read();
                            state = States.CONSTANT_ERROR;
                        }
                        break;
                    case States.KEYWORD:
                         if(64 < reader.Peek() && reader.Peek() < 91){
                            scannedString += getLowerCaseofNextLetter();
                            state = States.ID;
                        }
                        else if((96 < reader.Peek() && reader.Peek() < 123) ||(47 < reader.Peek() && reader.Peek() < 58)){
                            scannedString += (char)reader.Read();
                            state = States.ID;
                        }
                        else{
                            scannedString += (char)reader.Read();
                            state = States.INVALID_ID;
                        }
                        break;
                    case States.KEYWORD_A:
                        if(64 < reader.Peek() && reader.Peek() < 91){
                            if(reader.Peek()==71){
                                scannedString += getLowerCaseofNextLetter();
                                state = States.KEYWORD_AG;
                            }
                            else if(reader.Peek()==77){
                                scannedString += getLowerCaseofNextLetter();
                                state = States.KEYWORD_AM;
                            }
                            else if(reader.Peek()==78){
                                scannedString += getLowerCaseofNextLetter();
                                state = States.KEYWORD_AN;
                            }
                            else{
                                scannedString += getLowerCaseofNextLetter();
                                state = States.ID;
                            }
                        }
                        else if((96 < reader.Peek() && reader.Peek() < 123) ||(47 < reader.Peek() && reader.Peek() < 58)){
                            if(reader.Peek()==103){
                                scannedString += (char)reader.Read();
                                state = States.KEYWORD_AG;
                            }
                            else if(reader.Peek()==109){
                                scannedString += (char)reader.Read();
                                state = States.KEYWORD_AM;
                            }
                            else if(reader.Peek()==110){
                                scannedString += (char)reader.Read();
                                state = States.KEYWORD_AN;
                            }
                            else{
                                scannedString += (char)reader.Read();
                                state = States.ID;
                            }
                        }
                        else{
                            scannedString += (char)reader.Read();
                            state = States.INVALID_ID;
                        }
                        break;
                    case States.KEYWORD_AG:
                        if(64 < reader.Peek() && reader.Peek() < 91){
                            if(reader.Peek()==65){
                                scannedString += getLowerCaseofNextLetter();
                                state = States.KEYWORD_AGA;
                            }
                            else{
                                scannedString += getLowerCaseofNextLetter();
                                state = States.ID;
                            }
                        }
                        else if((96 < reader.Peek() && reader.Peek() < 123) ||(47 < reader.Peek() && reader.Peek() < 58)){
                            if(reader.Peek()==97){
                                scannedString += (char)reader.Read();
                                state = States.KEYWORD_AGA;
                            }
                            else{
                                scannedString += (char)reader.Read();
                                state = States.ID;
                            }
                        }
                        else{
                            scannedString += (char)reader.Read();
                            state = States.INVALID_ID;
                        }
                        break;
                    case States.KEYWORD_AGA:
                        if(64 < reader.Peek() && reader.Peek() < 91){
                            if(reader.Peek()==73){
                                scannedString += getLowerCaseofNextLetter();
                                state = States.KEYWORD_AGAI;
                            }
                            else{
                                scannedString += getLowerCaseofNextLetter();
                                state = States.ID;
                            }
                        }
                        else if((96 < reader.Peek() && reader.Peek() < 123) ||(47 < reader.Peek() && reader.Peek() < 58)){
                            if(reader.Peek()==105){
                                scannedString += (char)reader.Read();
                                state = States.KEYWORD_AGAI;
                            }
                            else{
                                scannedString += (char)reader.Read();
                                state = States.ID;
                            }
                        }
                        else{
                            scannedString += (char)reader.Read();
                            state = States.INVALID_ID;
                        }
                        break;
                    case States.KEYWORD_AGAI:
                        if(64 < reader.Peek() && reader.Peek() < 91){
                            if(reader.Peek()==78){
                                scannedString += getLowerCaseofNextLetter();
                                state = States.KEYWORD;
                            }
                            else{
                                scannedString += getLowerCaseofNextLetter();
                                state = States.ID;
                            }
                        }
                        else if((96 < reader.Peek() && reader.Peek() < 123) ||(47 < reader.Peek() && reader.Peek() < 58)){
                            if(reader.Peek()==110){
                                scannedString += (char)reader.Read();
                                state = States.KEYWORD;
                            }
                            else{
                                scannedString += (char)reader.Read();
                                state = States.ID;
                            }
                        }
                        else{
                            scannedString += (char)reader.Read();
                            state = States.INVALID_ID;
                        }
                        break;
                    case States.KEYWORD_AM:
                        if(64 < reader.Peek() && reader.Peek() < 91){
                            if(reader.Peek()==69){
                                scannedString += getLowerCaseofNextLetter();
                                state = States.KEYWORD_AME;
                            }
                            else{
                                scannedString += getLowerCaseofNextLetter();
                                state = States.ID;
                            }
                        }
                        else if((96 < reader.Peek() && reader.Peek() < 123) ||(47 < reader.Peek() && reader.Peek() < 58)){
                            if(reader.Peek()==101){
                                scannedString += (char)reader.Read();
                                state = States.KEYWORD_AME;
                            }
                            else{
                                scannedString += (char)reader.Read();
                                state = States.ID;
                            }
                        }
                        else{
                            scannedString += (char)reader.Read();
                            state = States.INVALID_ID;
                        }
                        break;
                    case States.KEYWORD_AME:
                        if(64 < reader.Peek() && reader.Peek() < 91){
                            if(reader.Peek()==82){
                                scannedString += getLowerCaseofNextLetter();
                                state = States.KEYWORD_AMER;
                            }
                            else{
                                scannedString += getLowerCaseofNextLetter();
                                state = States.ID;
                            }
                        }
                        else if((96 < reader.Peek() && reader.Peek() < 123) ||(47 < reader.Peek() && reader.Peek() < 58)){
                            if(reader.Peek()==114){
                                scannedString += (char)reader.Read();
                                state = States.KEYWORD_AMER;
                            }
                            else{
                                scannedString += (char)reader.Read();
                                state = States.ID;
                            }
                        }
                        else{
                            scannedString += (char)reader.Read();
                            state = States.INVALID_ID;
                        }
                        break;
                    case States.KEYWORD_AMER:
                        if(64 < reader.Peek() && reader.Peek() < 91){
                            if(reader.Peek()==73){
                                scannedString += getLowerCaseofNextLetter();
                                state = States.KEYWORD_AMERI;
                            }
                            else{
                                scannedString += getLowerCaseofNextLetter();
                                state = States.ID;
                            }
                        }
                        else if((96 < reader.Peek() && reader.Peek() < 123) ||(47 < reader.Peek() && reader.Peek() < 58)){
                            if(reader.Peek()==105){
                                scannedString += (char)reader.Read();
                                state = States.KEYWORD_AMERI;
                            }
                            else{
                                scannedString += (char)reader.Read();
                                state = States.ID;
                            }
                        }
                        else{
                            scannedString += (char)reader.Read();
                            state = States.INVALID_ID;
                        }
                        break;
                    case States.KEYWORD_AMERI:
                        if(64 < reader.Peek() && reader.Peek() < 91){
                            if(reader.Peek()==67){
                                scannedString += getLowerCaseofNextLetter();
                                state = States.KEYWORD_AMERIC;
                            }
                            else{
                                scannedString += getLowerCaseofNextLetter();
                                state = States.ID;
                            }
                        }
                        else if((96 < reader.Peek() && reader.Peek() < 123) ||(47 < reader.Peek() && reader.Peek() < 58)){
                            if(reader.Peek()==99){
                                scannedString += (char)reader.Read();
                                state = States.KEYWORD_AMERIC;
                            }
                            else{
                                scannedString += (char)reader.Read();
                                state = States.ID;
                            }
                        }
                        else{
                            scannedString += (char)reader.Read();
                            state = States.INVALID_ID;
                        }
                        break;
                    case States.KEYWORD_AMERIC:
                        if(64 < reader.Peek() && reader.Peek() < 91){
                            if(reader.Peek()==65){
                                scannedString += getLowerCaseofNextLetter();
                                state = States.KEYWORD;
                            }
                            else{
                                scannedString += getLowerCaseofNextLetter();
                                state = States.ID;
                            }
                        }
                        else if((96 < reader.Peek() && reader.Peek() < 123) ||(47 < reader.Peek() && reader.Peek() < 58)){
                            if(reader.Peek()==97){
                                scannedString += (char)reader.Read();
                                state = States.KEYWORD;
                            }
                            else{
                                scannedString += (char)reader.Read();
                                state = States.ID;
                            }
                        }
                        else{
                            scannedString += (char)reader.Read();
                            state = States.INVALID_ID;
                        }
                        break;
                    case States.KEYWORD_AN:
                        if(64 < reader.Peek() && reader.Peek() < 91){
                            if(reader.Peek()==68){
                                scannedString += getLowerCaseofNextLetter();
                                state = States.KEYWORD;
                            }
                            else{
                                scannedString += getLowerCaseofNextLetter();
                                state = States.ID;
                            }
                        }
                        else if((96 < reader.Peek() && reader.Peek() < 123) ||(47 < reader.Peek() && reader.Peek() < 58)){
                            if(reader.Peek()==100){
                                scannedString += (char)reader.Read();
                                state = States.KEYWORD;
                            }
                            else{
                                scannedString += (char)reader.Read();
                                state = States.ID;
                            }
                        }
                        else{
                            scannedString += (char)reader.Read();
                            state = States.INVALID_ID;
                        }
                        break;
                    case States.KEYWORD_M:
                        if(64 < reader.Peek() && reader.Peek() < 91){
                            if(reader.Peek()==65){
                                scannedString += getLowerCaseofNextLetter();
                                state = States.KEYWORD_MA;
                            }
                            else if(reader.Peek()==79){
                                scannedString += getLowerCaseofNextLetter();
                                state = States.KEYWORD_MO;
                            }
                            else{
                                scannedString += getLowerCaseofNextLetter();
                                state = States.ID;
                            }
                        }
                        else if((96 < reader.Peek() && reader.Peek() < 123) ||(47 < reader.Peek() && reader.Peek() < 58)){
                            if(reader.Peek()==97){
                                scannedString += (char)reader.Read();
                                state = States.KEYWORD_MA;
                            }
                            else if(reader.Peek()==111){
                                scannedString += (char)reader.Read();
                                state = States.KEYWORD_MO;
                            }
                            else{
                                scannedString += (char)reader.Read();
                                state = States.ID;
                            }
                        }
                        else{
                            scannedString += (char)reader.Read();
                            state = States.INVALID_ID;
                        }
                        break;
                    case States.KEYWORD_MA:
                        if(64 < reader.Peek() && reader.Peek() < 91){
                            if(reader.Peek()==75){
                                scannedString += getLowerCaseofNextLetter();
                                state = States.KEYWORD_MAK;
                            }
                            else{
                                scannedString += getLowerCaseofNextLetter();
                                state = States.ID;
                            }
                        }
                        else if((96 < reader.Peek() && reader.Peek() < 123) ||(47 < reader.Peek() && reader.Peek() < 58)){
                            if(reader.Peek()==107){
                                scannedString += (char)reader.Read();
                                state = States.KEYWORD_MAK;
                            }
                            else{
                                scannedString += (char)reader.Read();
                                state = States.ID;
                            }
                        }
                        else{
                            scannedString += (char)reader.Read();
                            state = States.INVALID_ID;
                        }
                        break;
                    case States.KEYWORD_MAK:
                        if(64 < reader.Peek() && reader.Peek() < 91){
                            if(reader.Peek()==69){
                                scannedString += getLowerCaseofNextLetter();
                                state = States.KEYWORD;
                            }
                            else{
                                scannedString += getLowerCaseofNextLetter();
                                state = States.ID;
                            }
                        }
                        else if((96 < reader.Peek() && reader.Peek() < 123) ||(47 < reader.Peek() && reader.Peek() < 58)){
                            if(reader.Peek()==101){
                                scannedString += (char)reader.Read();
                                state = States.KEYWORD;
                            }
                            else{
                                scannedString += (char)reader.Read();
                                state = States.ID;
                            }
                        }
                        else{
                            scannedString += (char)reader.Read();
                            state = States.INVALID_ID;
                        }
                        break;
                    case States.KEYWORD_MO:
                        if(64 < reader.Peek() && reader.Peek() < 91){
                            if(reader.Peek()==82){
                                scannedString += getLowerCaseofNextLetter();
                                state = States.KEYWORD_MOR;
                            }
                            else{
                                scannedString += getLowerCaseofNextLetter();
                                state = States.ID;
                            }
                        }
                        else if((96 < reader.Peek() && reader.Peek() < 123) ||(47 < reader.Peek() && reader.Peek() < 58)){
                            if(reader.Peek()==114){
                                scannedString += (char)reader.Read();
                                state = States.KEYWORD_MOR;
                            }
                            else{
                                scannedString += (char)reader.Read();
                                state = States.ID;
                            }
                        }
                        else{
                            scannedString += (char)reader.Read();
                            state = States.INVALID_ID;
                        }
                        break;
                    case States.KEYWORD_MOR:
                        if(64 < reader.Peek() && reader.Peek() < 91){
                            if(reader.Peek()==69){
                                scannedString += getLowerCaseofNextLetter();
                                state = States.KEYWORD;
                            }
                            else{
                                scannedString += getLowerCaseofNextLetter();
                                state = States.ID;
                            }
                        }
                        else if((96 < reader.Peek() && reader.Peek() < 123) ||(47 < reader.Peek() && reader.Peek() < 58)){
                            if(reader.Peek()==101){
                                scannedString += (char)reader.Read();
                                state = States.KEYWORD;
                            }
                            else{
                                scannedString += (char)reader.Read();
                                state = States.ID;
                            }
                        }
                        else{
                            scannedString += (char)reader.Read();
                            state = States.INVALID_ID;
                        }
                        break;
                    case States.ID:
                        if(64 < reader.Peek() && reader.Peek() < 91){
                            scannedString += getLowerCaseofNextLetter();
                        }
                        else if((96 < reader.Peek() && reader.Peek() < 123) ||(47 < reader.Peek() && reader.Peek() < 58)){
                            scannedString += (char)reader.Read();
                        }
                        else{
                            scannedString += (char)reader.Read();
                            state = States.INVALID_ID;
                        }
                        break;
                    default:
                        scannedString += (char)reader.Read();
                        break;
                }
            }
            if(isInAcceptingState(state)){
                if(state == States.CONSTANT){
                    if(scannedString == "1000000"){
                        state = States.CONSTANT_ERROR;
                        scannedToken = new Token(state.ToString(), Type.ERROR);
                        
                    }
                    else{
                        scannedToken = new Token(scannedString, Type.CONSTANT); 
                        bookkeeper.addToken(scannedToken);
                    }
                }
                else if(state == States.STRING_VALID){
                    scannedToken = new Token(scannedString, Type.STRING); 
                    bookkeeper.addToken(scannedToken);
                }
                else if(state == States.ID){
                    scannedToken = new Token(scannedString, Type.ID); 
                    bookkeeper.addToken(scannedToken);
                }
                else if(state == States.SPECIAL_CHARACTER){
                    scannedToken = new Token(scannedString, Type.SPECIAL_SYMBOL); 
                }
                else if(state.ToString().Substring(0, 7) == "KEYWORD"){
                    if(state.ToString() == "KEYWORD"){
                        scannedToken = new Token(scannedString, Type.KEYWORD);
                    }
                    else{
                        scannedToken = new Token(scannedString, Type.ID);
                    }
                }
            }
            else{
                //print error here?
                scannedToken = new Token(state.ToString(), Type.ERROR);
            }
            return scannedToken;
        }
        public bool isEndOfToken(){
            //Console.WriteLine(reader.Peek());
            return nextCharIsSpecial() 
                || nextCharIsWhitespace() 
                || reader.Peek() == -1 
                || (char)reader.Peek() == '#';
        }
        public bool isInAcceptingState(States currentState){
            return (currentState == States.STRING_VALID) 
                || (currentState == States.CONSTANT) 
                || (currentState == States.ID)
                || (currentState == States.SPECIAL_CHARACTER)
                || (currentState.ToString().Substring(0, 7) == "KEYWORD");
        }
        
        public char getLowerCaseofNextLetter(){
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
        public Token(string passedLexeme, Type passedType){
            lexeme = passedLexeme;
            tokenType = passedType;
        }
        public string Lexeme{
            get{
                return lexeme;
            }
        }
        public Type TokenType{
            get{
                return tokenType;
            }
        }
        override
        public string ToString(){
            return "Lexeme: " + lexeme + " Type: " + tokenType.ToString();
        }
    }

    class Bookkeeper{
        List<Token> SymTab = new List<Token>();
        
        public Bookkeeper(){
            setCapacity();
        }
        public bool peviouslyAdded(Token t){
            bool wasPreviouslyAdded = SymTab.Contains(t);
            return wasPreviouslyAdded;
        }

        public void addToken(Token tokenToAdd){
            if(!SymTab.Contains(tokenToAdd)){
                SymTab.Add(tokenToAdd);
            }
        }
        public void setCapacity(){
            SymTab.Capacity = 100;
        }

        public void printSymTab(){
            Console.WriteLine("SYMTAB: ");
            for(int i = 0; i < SymTab.Count; i++){
                Console.WriteLine(SymTab[i].ToString());
            }
        }
    }

    enum Type {ID, CONSTANT, STRING, KEYWORD, SPECIAL_SYMBOL, ERROR};
    enum States{
        INITIAL_STATE,
        GENERIC_ERROR,
        SPECIAL_CHARACTER,
        //string states
        STRING_INVALID, STRING_VALID,
        //Constant states
        CONSTANT_LENGTH_1, CONSTANT_LENGTH_2, CONSTANT_LENGTH_3, CONSTANT_LENGTH_4, CONSTANT_LENGTH_5, CONSTANT_LENGTH_6, CONSTANT, CONSTANT_ERROR,
        ID,
        INVALID_ID,
        KEYWORD,
        KEYWORD_A, KEYWORD_AM, KEYWORD_AME, KEYWORD_AMER, KEYWORD_AMERI, KEYWORD_AMERIC,
        KEYWORD_AG, KEYWORD_AGA, KEYWORD_AGAI,
        KEYWORD_AN,
        KEYWORD_M, KEYWORD_MA, KEYWORD_MO, KEYWORD_MAK, KEYWORD_MOR, 
        SOMETHING_ELSE
    }
}
