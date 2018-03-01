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
            char scannedSymbol;
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
                scannedSymbol = getNextCharacter();
                switch(state){
                    case States.INITIAL_STATE:
                        
                        if(scannedSymbol == '\"'){
                            scannedString += scannedSymbol;
                            state = States.STRING_INVALID;
                        }
                        else if(scannedSymbol == 'a'){
                            scannedString += scannedSymbol;
                            state = States.KEYWORD_A;
                        }
                        else if(scannedSymbol == 'g'){
                            scannedString += scannedSymbol;
                            state = States.KEYWORD_G;
                        }
                        else if(scannedSymbol == 'm'){
                            scannedString += scannedSymbol;
                            state = States.KEYWORD_M;
                        }
                        else if(scannedSymbol == 'p'){
                            scannedString += scannedSymbol;
                            state = States.KEYWORD_P;
                        }
                        else if(thisCharIsLowerCaseLetter(scannedSymbol)){
                            scannedString += scannedSymbol;
                            state = States.ID;
                        }
                        else if(thisCharIsADigit(scannedSymbol)){
                            //next character is a number,
                            scannedString += scannedSymbol;
                            state = States.CONSTANT_LENGTH_1;
                        }
                        else{
                            scannedString += scannedSymbol;
                            state = States.INVALID_ID;
                        }
                        break;
                    case States.STRING_INVALID:
                        if(scannedSymbol != '\"'){
                            scannedString += scannedSymbol;
                        }
                        else{
                            scannedString += scannedSymbol;
                            state = States.STRING_VALID;
                        }
                        break;
                    case States.STRING_VALID:
                        if(scannedSymbol != '\"'){
                            scannedString += scannedSymbol;
                            state = States.STRING_INVALID;
                        }
                        break;
                    case States.CONSTANT_LENGTH_1:
                        if(thisCharIsADigit(scannedSymbol)){
                            //next character is a number,
                            scannedString += scannedSymbol;
                            state = States.CONSTANT_LENGTH_2;
                        }
                        else{
                            scannedString += scannedSymbol;
                            state = States.CONSTANT_ERROR;
                        }
                        break;
                    case States.CONSTANT_LENGTH_2:
                        if(thisCharIsADigit(scannedSymbol)){
                            //next character is a number,
                            scannedString += scannedSymbol;
                            state = States.CONSTANT_LENGTH_3;
                        }
                        else{
                            scannedString += scannedSymbol;
                            state = States.CONSTANT_ERROR;
                        }
                        break;
                    case States.CONSTANT_LENGTH_3:
                        if(thisCharIsADigit(scannedSymbol)){
                            //next character is a number,
                            scannedString += scannedSymbol;
                            state = States.CONSTANT_LENGTH_4;
                        }
                        else{
                            scannedString += scannedSymbol;
                            state = States.CONSTANT_ERROR;
                        }
                        break;
                    case States.CONSTANT_LENGTH_4:
                        if(thisCharIsADigit(scannedSymbol)){
                            //next character is a number,
                            scannedString += scannedSymbol;
                            state = States.CONSTANT_LENGTH_5;
                        }
                        else{
                            scannedString += scannedSymbol;
                            state = States.CONSTANT_ERROR;
                        }
                        break;
                    case States.CONSTANT_LENGTH_5:
                        if(thisCharIsADigit(scannedSymbol)){
                            //next character is a number,
                            scannedString += scannedSymbol;
                            state = States.CONSTANT_LENGTH_6;
                        }
                        else{
                            scannedString += scannedSymbol;
                            state = States.CONSTANT_ERROR;
                        }
                        break;
                    case States.CONSTANT_LENGTH_6:
                        if(thisCharIsADigit(scannedSymbol)){
                            //next character is a number,
                            scannedString += scannedSymbol;
                            state = States.CONSTANT;
                        }
                        else{
                            scannedString += scannedSymbol;
                            state = States.CONSTANT_ERROR;
                        }
                        break;
                    case States.CONSTANT:
                        if(thisCharIsADigit(scannedSymbol)){
                            //next character is a number,
                            scannedString += scannedSymbol;
                            state = States.CONSTANT;
                        }
                        else{
                            scannedString += scannedSymbol;
                            state = States.CONSTANT_ERROR;
                        }
                        break;
                    case States.KEYWORD:
                        if(thisCharIsLowerCaseLetter(scannedSymbol) || thisCharIsADigit(scannedSymbol)){
                            scannedString += scannedSymbol;
                            state = States.ID;
                        }
                        else{
                            scannedString += scannedSymbol;
                            state = States.INVALID_ID;
                        }
                        break;
                    case States.KEYWORD_A:
                        if(scannedSymbol == 'g'){
                            scannedString += scannedSymbol;
                            state = States.KEYWORD_AG;
                        }
                        else if(scannedSymbol == 'm'){
                            scannedString += scannedSymbol;
                            state = States.KEYWORD_AM;
                        }
                        else if(scannedSymbol == 'n'){
                            scannedString += scannedSymbol;
                            state = States.KEYWORD_AN;
                        }
                        else if(thisCharIsLowerCaseLetter(scannedSymbol) || thisCharIsADigit(scannedSymbol)){
                            scannedString += scannedSymbol;
                            state = States.ID;
                        }
                        else{
                            scannedString += scannedSymbol;
                            state = States.INVALID_ID;
                        }
                        break;
                    case States.KEYWORD_AG:
                        if(scannedSymbol == 'a'){
                            scannedString += scannedSymbol;
                            state = States.KEYWORD_AGA;
                        }
                        else if(thisCharIsLowerCaseLetter(scannedSymbol) || thisCharIsADigit(scannedSymbol)){
                            scannedString += scannedSymbol;
                            state = States.ID;
                        }
                        else{
                            scannedString += scannedSymbol;
                            state = States.INVALID_ID;
                        }
                        break;
                    case States.KEYWORD_AGA:
                        if(scannedSymbol == 'i'){
                            scannedString += scannedSymbol;
                            state = States.KEYWORD_AGAI;
                        }
                        else if(thisCharIsLowerCaseLetter(scannedSymbol) || thisCharIsADigit(scannedSymbol)){
                            scannedString += scannedSymbol;
                            state = States.ID;
                        }
                        else{
                            scannedString += scannedSymbol;
                            state = States.INVALID_ID;
                        }
                        break;
                    case States.KEYWORD_AGAI:
                        if(scannedSymbol == 'n'){
                            scannedString += scannedSymbol;
                            state = States.KEYWORD;
                        }
                        else if(thisCharIsLowerCaseLetter(scannedSymbol) || thisCharIsADigit(scannedSymbol)){
                            scannedString += scannedSymbol;
                            state = States.ID;
                        }
                        else{
                            scannedString += scannedSymbol;
                            state = States.INVALID_ID;
                        }
                        break;
                    case States.KEYWORD_AM:
                        if(scannedSymbol == 'e'){
                            scannedString += scannedSymbol;
                            state = States.KEYWORD_AME;
                        }
                        else if(thisCharIsLowerCaseLetter(scannedSymbol) || thisCharIsADigit(scannedSymbol)){
                            scannedString += scannedSymbol;
                            state = States.ID;
                        }
                        else{
                            scannedString += scannedSymbol;
                            state = States.INVALID_ID;
                        }
                        break;
                    case States.KEYWORD_AME:
                        if(scannedSymbol == 'r'){
                            scannedString += scannedSymbol;
                            state = States.KEYWORD_AMER;
                        }
                        else if(thisCharIsLowerCaseLetter(scannedSymbol) || thisCharIsADigit(scannedSymbol)){
                            scannedString += scannedSymbol;
                            state = States.ID;
                        }
                        else{
                            scannedString += scannedSymbol;
                            state = States.INVALID_ID;
                        }
                        break;
                    case States.KEYWORD_AMER:
                        if(scannedSymbol == 'i'){
                            scannedString += scannedSymbol;
                            state = States.KEYWORD_AMERI;
                        }
                        else if(thisCharIsLowerCaseLetter(scannedSymbol) || thisCharIsADigit(scannedSymbol)){
                            scannedString += scannedSymbol;
                            state = States.ID;
                        }
                        else{
                            scannedString += scannedSymbol;
                            state = States.INVALID_ID;
                        }
                        break;
                    case States.KEYWORD_AMERI:
                        if(scannedSymbol == 'c'){
                            scannedString += scannedSymbol;
                            state = States.KEYWORD_AMERIC;
                        }
                        else if(thisCharIsLowerCaseLetter(scannedSymbol) || thisCharIsADigit(scannedSymbol)){
                            scannedString += scannedSymbol;
                            state = States.ID;
                        }
                        else{
                            scannedString += scannedSymbol;
                            state = States.INVALID_ID;
                        }
                        break;
                    case States.KEYWORD_AMERIC:
                        if(scannedSymbol == 'a'){
                            scannedString += scannedSymbol;
                            state = States.KEYWORD;
                        }
                        else if(thisCharIsLowerCaseLetter(scannedSymbol) || thisCharIsADigit(scannedSymbol)){
                            scannedString += scannedSymbol;
                            state = States.ID;
                        }
                        else{
                            scannedString += scannedSymbol;
                            state = States.INVALID_ID;
                        }
                        break;
                    case States.KEYWORD_AN:
                        if(scannedSymbol == 'd'){
                            scannedString += scannedSymbol;
                            state = States.KEYWORD;
                        }
                        else if(thisCharIsLowerCaseLetter(scannedSymbol) || thisCharIsADigit(scannedSymbol)){
                            scannedString += scannedSymbol;
                            state = States.ID;
                        }
                        else{
                            scannedString += scannedSymbol;
                            state = States.INVALID_ID;
                        }
                        break;
                    case States.KEYWORD_G:
                        if(scannedSymbol == 'r'){
                            scannedString += scannedSymbol;
                            state = States.KEYWORD_GR;
                        }
                        else if(thisCharIsLowerCaseLetter(scannedSymbol) || thisCharIsADigit(scannedSymbol)){
                            scannedString += scannedSymbol;
                            state = States.ID;
                        }
                        else{
                            scannedString += scannedSymbol;
                            state = States.INVALID_ID;
                        }
                        break;
                    case States.KEYWORD_GR:
                        if(scannedSymbol == 'e'){
                            scannedString += scannedSymbol;
                            state = States.KEYWORD_GRE;
                        }
                        else if(thisCharIsLowerCaseLetter(scannedSymbol) || thisCharIsADigit(scannedSymbol)){
                            scannedString += scannedSymbol;
                            state = States.ID;
                        }
                        else{
                            scannedString += scannedSymbol;
                            state = States.INVALID_ID;
                        }
                        break;
                    case States.KEYWORD_GRE:
                        if(scannedSymbol == 'a'){
                            scannedString += scannedSymbol;
                            state = States.KEYWORD_GREA;
                        }
                        else if(thisCharIsLowerCaseLetter(scannedSymbol) || thisCharIsADigit(scannedSymbol)){
                            scannedString += scannedSymbol;
                            state = States.ID;
                        }
                        else{
                            scannedString += scannedSymbol;
                            state = States.INVALID_ID;
                        }
                        break;
                    case States.KEYWORD_GREA:
                        if(scannedSymbol == 't'){
                            scannedString += scannedSymbol;
                            state = States.KEYWORD;
                        }
                        else if(thisCharIsLowerCaseLetter(scannedSymbol) || thisCharIsADigit(scannedSymbol)){
                            scannedString += scannedSymbol;
                            state = States.ID;
                        }
                        else{
                            scannedString += scannedSymbol;
                            state = States.INVALID_ID;
                        }
                        break;
                    case States.KEYWORD_M:
                        if(scannedSymbol == 'a'){
                            scannedString += scannedSymbol;
                            state = States.KEYWORD_MA;
                        }
                        else if(scannedSymbol == 'o'){
                            scannedString += scannedSymbol;
                            state = States.KEYWORD_MO;
                        }
                        else if(thisCharIsLowerCaseLetter(scannedSymbol) || thisCharIsADigit(scannedSymbol)){
                            scannedString += scannedSymbol;
                            state = States.ID;
                        }
                        else{
                            scannedString += scannedSymbol;
                            state = States.INVALID_ID;
                        }
                        break;
                    case States.KEYWORD_MA:
                        if(scannedSymbol == 'k'){
                            scannedString += scannedSymbol;
                            state = States.KEYWORD_MAK;
                        }
                        else if(thisCharIsLowerCaseLetter(scannedSymbol) || thisCharIsADigit(scannedSymbol)){
                            scannedString += scannedSymbol;
                            state = States.ID;
                        }
                        else{
                            scannedString += scannedSymbol;
                            state = States.INVALID_ID;
                        }
                        break;
                    case States.KEYWORD_MAK:
                        if(scannedSymbol == 'e'){
                            scannedString += scannedSymbol;
                            state = States.KEYWORD;
                        }
                        else if(thisCharIsLowerCaseLetter(scannedSymbol) || thisCharIsADigit(scannedSymbol)){
                            scannedString += scannedSymbol;
                            state = States.ID;
                        }
                        else{
                            scannedString += scannedSymbol;
                            state = States.INVALID_ID;
                        }
                        break;
                    case States.KEYWORD_MO:
                        if(scannedSymbol == 'r'){
                            scannedString += scannedSymbol;
                            state = States.KEYWORD_MOR;
                        }
                        else if(thisCharIsLowerCaseLetter(scannedSymbol) || thisCharIsADigit(scannedSymbol)){
                            scannedString += scannedSymbol;
                            state = States.ID;
                        }
                        else{
                            scannedString += scannedSymbol;
                            state = States.INVALID_ID;
                        }
                        break;
                    case States.KEYWORD_MOR:
                        if(scannedSymbol == 'e'){
                            scannedString += scannedSymbol;
                            state = States.KEYWORD;
                        }
                        else if(thisCharIsLowerCaseLetter(scannedSymbol) || thisCharIsADigit(scannedSymbol)){
                            scannedString += scannedSymbol;
                            state = States.ID;
                        }
                        else{
                            scannedString += scannedSymbol;
                            state = States.INVALID_ID;
                        }
                        break;
                    case States.KEYWORD_P:
                        if(scannedSymbol == 'r'){
                            scannedString += scannedSymbol;
                            state = States.KEYWORD_PR;
                        }
                        else if(thisCharIsLowerCaseLetter(scannedSymbol) || thisCharIsADigit(scannedSymbol)){
                            scannedString += scannedSymbol;
                            state = States.ID;
                        }
                        else{
                            scannedString += scannedSymbol;
                            state = States.INVALID_ID;
                        }
                        break;
                    case States.KEYWORD_PR:
                        if(scannedSymbol == 'o'){
                            scannedString += scannedSymbol;
                            state = States.KEYWORD_PRO;
                        }
                        else if(thisCharIsLowerCaseLetter(scannedSymbol) || thisCharIsADigit(scannedSymbol)){
                            scannedString += scannedSymbol;
                            state = States.ID;
                        }
                        else{
                            scannedString += scannedSymbol;
                            state = States.INVALID_ID;
                        }
                        break;
                    case States.KEYWORD_PRO:
                        if(scannedSymbol == 'g'){
                            scannedString += scannedSymbol;
                            state = States.KEYWORD_PROG;
                        }
                        else if(thisCharIsLowerCaseLetter(scannedSymbol) || thisCharIsADigit(scannedSymbol)){
                            scannedString += scannedSymbol;
                            state = States.ID;
                        }
                        else{
                            scannedString += scannedSymbol;
                            state = States.INVALID_ID;
                        }
                        break;
                    case States.KEYWORD_PROG:
                        if(scannedSymbol == 'r'){
                            scannedString += scannedSymbol;
                            state = States.KEYWORD_PROGR;
                        }
                        else if(thisCharIsLowerCaseLetter(scannedSymbol) || thisCharIsADigit(scannedSymbol)){
                            scannedString += scannedSymbol;
                            state = States.ID;
                        }
                        else{
                            scannedString += scannedSymbol;
                            state = States.INVALID_ID;
                        }
                        break;
                    case States.KEYWORD_PROGR:
                        if(scannedSymbol == 'a'){
                            scannedString += scannedSymbol;
                            state = States.KEYWORD_PROGRA;
                        }
                        else if(thisCharIsLowerCaseLetter(scannedSymbol) || thisCharIsADigit(scannedSymbol)){
                            scannedString += scannedSymbol;
                            state = States.ID;
                        }
                        else{
                            scannedString += scannedSymbol;
                            state = States.INVALID_ID;
                        }
                        break;
                    case States.KEYWORD_PROGRA:
                        if(scannedSymbol == 'm'){
                            scannedString += scannedSymbol;
                            state = States.KEYWORD_PROGRAM;
                        }
                        else if(thisCharIsLowerCaseLetter(scannedSymbol) || thisCharIsADigit(scannedSymbol)){
                            scannedString += scannedSymbol;
                            state = States.ID;
                        }
                        else{
                            scannedString += scannedSymbol;
                            state = States.INVALID_ID;
                        }
                        break;
                    case States.KEYWORD_PROGRAM:
                        if(scannedSymbol == 'm'){
                            scannedString += scannedSymbol;
                            state = States.KEYWORD_PROGRAMM;
                        }
                        else if(thisCharIsLowerCaseLetter(scannedSymbol) || thisCharIsADigit(scannedSymbol)){
                            scannedString += scannedSymbol;
                            state = States.ID;
                        }
                        else{
                            scannedString += scannedSymbol;
                            state = States.INVALID_ID;
                        }
                        break;
                    case States.KEYWORD_PROGRAMM:
                        if(scannedSymbol == 'i'){
                            scannedString += scannedSymbol;
                            state = States.KEYWORD_PROGRAMMI;
                        }
                        else if(thisCharIsLowerCaseLetter(scannedSymbol) || thisCharIsADigit(scannedSymbol)){
                            scannedString += scannedSymbol;
                            state = States.ID;
                        }
                        else{
                            scannedString += scannedSymbol;
                            state = States.INVALID_ID;
                        }
                        break;
                    case States.KEYWORD_PROGRAMMI:
                        if(scannedSymbol == 'n'){
                            scannedString += scannedSymbol;
                            state = States.KEYWORD_PROGRAMMIN;
                        }
                        else if(thisCharIsLowerCaseLetter(scannedSymbol) || thisCharIsADigit(scannedSymbol)){
                            scannedString += scannedSymbol;
                            state = States.ID;
                        }
                        else{
                            scannedString += scannedSymbol;
                            state = States.INVALID_ID;
                        }
                        break;
                    case States.KEYWORD_PROGRAMMIN:
                        if(scannedSymbol == 'g'){
                            scannedString += scannedSymbol;
                            state = States.KEYWORD;
                        }
                        else if(thisCharIsLowerCaseLetter(scannedSymbol) || thisCharIsADigit(scannedSymbol)){
                            scannedString += scannedSymbol;
                            state = States.ID;
                        }
                        else{
                            scannedString += scannedSymbol;
                            state = States.INVALID_ID;
                        }
                        break;
                    case States.ID:
                        if(thisCharIsLowerCaseLetter(scannedSymbol) || thisCharIsADigit(scannedSymbol)){
                            scannedString += scannedSymbol;
                            state = States.ID;
                        }
                        else{
                            scannedString += scannedSymbol;
                            state = States.INVALID_ID;
                        }
                        break;
                    case States.INVALID_ID:
                        scannedString += scannedSymbol;
                        state = States.INVALID_ID;
                        break;
                    default:
                        scannedString += scannedSymbol;
                        state = States.INVALID_ID;
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
                    }
                }
                else if(state == States.STRING_VALID){
                    scannedToken = new Token(scannedString, Type.STRING); 
                }
                else if(state == States.ID){
                    scannedToken = new Token(scannedString, Type.ID); 
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
        public char getNextCharacter(){
            char nextChar;
            if(64 < reader.Peek() && reader.Peek() < 91){
                nextChar = getLowerCaseofNextLetter();
            }
            else{
                nextChar = (char)reader.Read();
            }
            return nextChar;
        }
        public bool thisCharIsLowerCaseLetter(char passedChar){
            bool isLower = false;
            if(96 < (int)passedChar && (int)passedChar < 123){
                isLower = true;
            }
            return isLower;
        }
        public bool thisCharIsADigit(char passedChar){
            bool isDigit = false;
            if(47 < (int)passedChar && (int)passedChar < 58){
                isDigit = true;
            }
            return isDigit;
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
        public string checkString(){
            string returnString = "\"";
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
            returnString = returnString + "\"";
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
        KEYWORD_G, KEYWORD_GR, KEYWORD_GRE, KEYWORD_GREA, 
        KEYWORD_M, KEYWORD_MA, KEYWORD_MO, KEYWORD_MAK, KEYWORD_MOR, 
        KEYWORD_P, KEYWORD_PR, KEYWORD_PRO, KEYWORD_PROG, KEYWORD_PROGR, KEYWORD_PROGRA, KEYWORD_PROGRAM, KEYWORD_PROGRAMM, KEYWORD_PROGRAMMI, KEYWORD_PROGRAMMIN,
        SOMETHING_ELSE
    }
}
