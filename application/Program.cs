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
                        else if(reader.Peek() == 77 || reader.Peek() == 109){
                            //m detected, check for keywords 'make' and 'more'
                            if(reader.Peek() == 77){
                                scannedString += getLowerCaseofNextLetter();
                            }
                            else{
                                scannedString += (char)reader.Read();
                            }
                            state = States.KEYWORD_M;
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
        KEYWORD_M, KEYWORD_MA, KEYWORD_MO, KEYWORD_MAK, KEYWORD_MOR, 
        SOMETHING_ELSE
    }
}
