using System;
using System.IO;
using System.Collections.Generic;


namespace application
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
            Bookkeeper bk = new Bookkeeper();
            ErrorHandler er = new ErrorHandler();
            Token token;
            while(sr.Peek() != -1){
                //call new scanner for every detected Token
                Scanner theScanner = new Scanner(sr, bk, er);
                //scan and return a token from the scanner
                token = theScanner.detectToken();
            }
            //output conents of the symble table in the Bookkeeper
            Console.WriteLine("=======================================");
            Console.WriteLine("SYMTAB Content: ");
            bk.printSymTab();
            sr.Close();
        }
    }
    //Scanner class, usesstate and scanned symbol to detect 5 types of tokens
    class Scanner{
        States state = States.INITIAL_STATE;
        StreamReader reader;
        Bookkeeper bookkeeper;
        ErrorHandler errorHandler;
        Token scannedToken;
        public Scanner(StreamReader passedReader, Bookkeeper passedBookkeeper, ErrorHandler passedErrorHandler){
            //constructor that instantiates scanner using parameters passed from declaration in main
            reader = passedReader;
            bookkeeper = passedBookkeeper;
            errorHandler = passedErrorHandler;
        }
        //method used to detect and return valid token
        public Token detectToken(){
            //helper variables for detection
            char scannedSymbol;
            string scannedString = "";
            scannedToken = new Token(States.GENERIC_ERROR.ToString(), Type.ERROR);
            //read though whitespace, exit if EOF reached
            if(nextCharIsWhitespace()){
                while(nextCharIsWhitespace() && reader.Peek() != -1){
                    reader.Read();
                }
            }
            //process lines of comments
            if((char)reader.Peek() == '#'){
                while((char)reader.Peek() == '#'){
                    //a single line as a comment
                    processComment();
                }
            }
            //detect and 
            if(nextCharIsSpecial()){
                state = States.SPECIAL_CHARACTER;
                scannedString += (char)reader.Read();
                scannedToken = new Token(scannedString, Type.SPECIAL_SYMBOL);
            }
            while(!isEndOfToken() && scannedToken.TokenType != Type.SPECIAL_SYMBOL){
                //next symbol is not a comment whitespace or special symbol
                //read symbol
                scannedSymbol = getNextCharacter();
                switch(state){
                    case States.INITIAL_STATE:
                        //in initial state, check for string
                        scannedString += scannedSymbol;
                        if(scannedSymbol == '\"'){
                            state = States.STRING_INVALID;
                        }
                        //not a string check if start of keyword
                        else if(scannedSymbol == 'a'){
                            state = States.KEYWORD_A;
                        }
                        else if(scannedSymbol == 'b'){
                            state = States.KEYWORD_B;
                        }
                        else if(scannedSymbol == 'e'){
                            state = States.KEYWORD_E;
                        }
                        else if(scannedSymbol == 'f'){
                            state = States.KEYWORD_F;
                        }
                        else if(scannedSymbol == 'g'){
                            state = States.KEYWORD_G;
                        }
                        else if(scannedSymbol == 'i'){
                            state = States.KEYWORD_I;
                        }
                        else if(scannedSymbol == 'l'){
                            state = States.KEYWORD_L;
                        }
                        else if(scannedSymbol == 'm'){
                            state = States.KEYWORD_M;
                        }
                        else if(scannedSymbol == 'n'){
                            state = States.KEYWORD_N;
                        }
                        else if(scannedSymbol == 'o'){
                            state = States.KEYWORD_O;
                        }
                        else if(scannedSymbol == 'p'){
                            state = States.KEYWORD_P;
                        }
                        else if(scannedSymbol == 's'){
                            state = States.KEYWORD_S;
                        }
                        else if(scannedSymbol == 't'){
                            state = States.KEYWORD_T;
                        }
                        //not the beginning of a keyword, check if a letter at all 
                        else if(thisCharIsLowerCaseLetter(scannedSymbol)){
                            state = States.ID;
                        }
                        //not a letter or string, check if start of a number
                        else if(thisCharIsADigit(scannedSymbol)){
                            //next character is a number,
                            state = States.CONSTANT_LENGTH_1;
                        }
                        //if its none of the above it isn't a valid character, go to error
                        else{  
                            state = States.GENERIC_ERROR;
                        }
                        break;
                    case States.STRING_INVALID:
                        //start of string has been read stay in state until end is detected
                        scannedString += scannedSymbol;
                        if(scannedSymbol == '\"'){
                            //end of string detected go to valid state
                            state = States.STRING_VALID;
                        }
                        break;
                    case States.STRING_VALID:
                        //if anothersymbol is read in it invalidates the string, error
                        scannedString += scannedSymbol;
                        state = States.GENERIC_ERROR;
                        break;
                    case States.CONSTANT_LENGTH_1:
                        //check to make sure next char is a number 
                        scannedString += scannedSymbol;
                        if(thisCharIsADigit(scannedSymbol)){
                            state = States.CONSTANT_LENGTH_2;
                        }
                        else{
                            //not a number, go to constant error
                            state = States.CONSTANT_ERROR;
                        }
                        break;
                    case States.CONSTANT_LENGTH_2:
                        //check to make sure next char is a number 
                        scannedString += scannedSymbol;
                        if(thisCharIsADigit(scannedSymbol)){
                            state = States.CONSTANT_LENGTH_3;
                        }
                        else{
                            //not a number, go to constant error
                            state = States.CONSTANT_ERROR;
                        }
                        break;
                    case States.CONSTANT_LENGTH_3:
                       //check to make sure next char is a number 
                        scannedString += scannedSymbol;
                        if(thisCharIsADigit(scannedSymbol)){
                            state = States.CONSTANT_LENGTH_4;
                        }
                        else{
                            //not a number, go to constant error
                            state = States.CONSTANT_ERROR;
                        }
                        break;
                    case States.CONSTANT_LENGTH_4:
                        //check to make sure next char is a number 
                        scannedString += scannedSymbol;
                        if(thisCharIsADigit(scannedSymbol)){
                            state = States.CONSTANT_LENGTH_5;
                        }
                        else{
                            //not a number, go to constant error
                            state = States.CONSTANT_ERROR;
                        }
                        break;
                    case States.CONSTANT_LENGTH_5:
                        //check to make sure next char is a number 
                        scannedString += scannedSymbol;
                        if(thisCharIsADigit(scannedSymbol)){
                            state = States.CONSTANT_LENGTH_6;
                        }
                        else{
                            //not a number, go to constant error
                            state = States.CONSTANT_ERROR;
                        }
                        break;
                    case States.CONSTANT_LENGTH_6:
                        //check to make sure next char is a number 
                        scannedString += scannedSymbol;
                        if(thisCharIsADigit(scannedSymbol)){
                            //at 7 digits, go to valid constant state
                            state = States.CONSTANT;
                        }
                        else{
                            //not a number, go to constant error
                            state = States.CONSTANT_ERROR;
                        }
                        break;
                    case States.CONSTANT:
                        scannedString += scannedSymbol;
                        //keep reading constants and staying in this state
                        if(thisCharIsADigit(scannedSymbol)){
                            state = States.CONSTANT;
                        }
                        else{
                            //scanner char is not a number, go to constant error
                            state = States.CONSTANT_ERROR;
                        }
                        break;
                    case States.KEYWORD:
                        //generic keyword state for all validated keywords
                        scannedString += scannedSymbol;
                        //if scanned char is a letter or number, go to ID
                        if(thisCharIsLowerCaseLetter(scannedSymbol) || thisCharIsADigit(scannedSymbol)){
                            state = States.ID;
                        }
                        else{
                            //invalid character, got to invalid ID state
                            state = States.INVALID_ID;
                        }
                        break;
                    case States.KEYWORD_A:
                        //check the second letter of all keywords begining with 'a'
                        scannedString += scannedSymbol;
                        if(scannedSymbol == 'g'){
                            //beginning of "again" keyword go to AG state
                            state = States.KEYWORD_AG;
                        }
                        else if(scannedSymbol == 'm'){
                            //beginning of "america" keyword go to AM state
                            state = States.KEYWORD_AM;
                        }
                        else if(scannedSymbol == 'n'){
                            //beginning of "and" keyword go to AN state
                            state = States.KEYWORD_AN;
                        }
                        else if(scannedSymbol == 's'){
                            //full "as" keyword detected, go to KEYWORD state
                            state = States.KEYWORD;
                        }
                        //none of the possible letters indicate keyword, check if valid ID
                        else if(thisCharIsLowerCaseLetter(scannedSymbol) || thisCharIsADigit(scannedSymbol)){
                            state = States.ID;
                        }
                        else{
                            //invalid char, go to ID ERROR
                            state = States.INVALID_ID;
                        }
                        break;
                    case States.KEYWORD_AG:
                        scannedString += scannedSymbol;
                        //check for next letter of keyword
                        if(scannedSymbol == 'a'){
                            state = States.KEYWORD_AGA;
                        }
                        //none of the possible letters indicate keyword, check if valid ID
                        else if(thisCharIsLowerCaseLetter(scannedSymbol) || thisCharIsADigit(scannedSymbol)){
                            state = States.ID;
                        }
                        else{
                            //invalid char, go to ID ERROR
                            state = States.INVALID_ID;
                        }
                        break;
                    case States.KEYWORD_AGA:
                        scannedString += scannedSymbol;
                        //check for next letter of keyword
                        if(scannedSymbol == 'i'){
                            state = States.KEYWORD_AGAI;
                        }
                        //none of the possible letters indicate keyword, check if valid ID
                        else if(thisCharIsLowerCaseLetter(scannedSymbol) || thisCharIsADigit(scannedSymbol)){
                            state = States.ID;
                        }
                        else{
                            //invalid char, go to ID ERROR
                            state = States.INVALID_ID;
                        }
                        break;
                    case States.KEYWORD_AGAI:
                        scannedString += scannedSymbol;
                        //check for next letter of keyword
                        if(scannedSymbol == 'n'){
                             //keyword "again" detected , go to KEYWORD state
                            state = States.KEYWORD;
                        }
                        //none of the possible letters indicate keyword, check if valid ID
                        else if(thisCharIsLowerCaseLetter(scannedSymbol) || thisCharIsADigit(scannedSymbol)){
                            state = States.ID;
                        }
                        else{
                            //invalid char, go to ID ERROR
                            state = States.INVALID_ID;
                        }
                        break;
                    case States.KEYWORD_AM:
                        scannedString += scannedSymbol;
                        //check for next letter of keyword
                        if(scannedSymbol == 'e'){
                            state = States.KEYWORD_AME;
                        }
                        //none of the possible letters indicate keyword, check if valid ID
                        else if(thisCharIsLowerCaseLetter(scannedSymbol) || thisCharIsADigit(scannedSymbol)){
                            state = States.ID;
                        }
                        else{
                            //invalid char, go to ID ERROR
                            state = States.INVALID_ID;
                        }
                        break;
                    case States.KEYWORD_AME:
                        scannedString += scannedSymbol;
                        //check for next letter of keyword
                        if(scannedSymbol == 'r'){
                            state = States.KEYWORD_AMER;
                        }
                        //none of the possible letters indicate keyword, check if valid ID
                        else if(thisCharIsLowerCaseLetter(scannedSymbol) || thisCharIsADigit(scannedSymbol)){
                            state = States.ID;
                        }
                        else{
                            //invalid char, go to ID ERROR
                            state = States.INVALID_ID;
                        }
                        break;
                    case States.KEYWORD_AMER:
                        scannedString += scannedSymbol;
                        //check for next letter of keyword
                        if(scannedSymbol == 'i'){
                            state = States.KEYWORD_AMERI;
                        }
                        //none of the possible letters indicate keyword, check if valid ID
                        else if(thisCharIsLowerCaseLetter(scannedSymbol) || thisCharIsADigit(scannedSymbol)){
                            state = States.ID;
                        }
                        else{
                            //invalid char, go to ID ERROR
                            state = States.INVALID_ID;
                        }
                        break;
                    case States.KEYWORD_AMERI:
                        scannedString += scannedSymbol;
                        //check for next letter of keyword
                        if(scannedSymbol == 'c'){
                            state = States.KEYWORD_AMERIC;
                        }
                        //none of the possible letters indicate keyword, check if valid ID
                        else if(thisCharIsLowerCaseLetter(scannedSymbol) || thisCharIsADigit(scannedSymbol)){
                            state = States.ID;
                        }
                        else{
                            //invalid char, go to ID ERROR
                            state = States.INVALID_ID;
                        }
                        break;
                    case States.KEYWORD_AMERIC:
                        scannedString += scannedSymbol;
                        //check for next letter of keyword
                        if(scannedSymbol == 'a'){
                             //keyword "america" detected , go to KEYWORD state
                            state = States.KEYWORD;
                        }
                        //none of the possible letters indicate keyword, check if valid ID
                        else if(thisCharIsLowerCaseLetter(scannedSymbol) || thisCharIsADigit(scannedSymbol)){
                            state = States.ID;
                        }
                        else{
                            //invalid char, go to ID ERROR
                            state = States.INVALID_ID;
                        }
                        break;
                    case States.KEYWORD_AN:
                        scannedString += scannedSymbol;
                        //check for next letter of keyword
                        if(scannedSymbol == 'd'){
                             //keyword "and" detected , go to KEYWORD state
                            state = States.KEYWORD;
                        }
                        //none of the possible letters indicate keyword, check if valid ID
                        else if(thisCharIsLowerCaseLetter(scannedSymbol) || thisCharIsADigit(scannedSymbol)){
                            state = States.ID;
                        }
                        else{
                            //invalid char, go to ID ERROR
                            state = States.INVALID_ID;
                        }
                        break;
                    case States.KEYWORD_B:
                        scannedString += scannedSymbol;
                        //check for next letter of keyword
                        if(scannedSymbol == 'o'){
                            //check for keyword boolean
                            state = States.KEYWORD_BO;
                        }
                        //none of the possible letters indicate keyword, check if valid ID
                        else if(thisCharIsLowerCaseLetter(scannedSymbol) || thisCharIsADigit(scannedSymbol)){
                            state = States.ID;
                        }
                        else{
                            //invalid char, go to ID ERROR
                            state = States.INVALID_ID;
                        }
                        break;
                    case States.KEYWORD_BO:
                        scannedString += scannedSymbol;
                        //check for next letter of keyword
                        if(scannedSymbol == 'o'){
                            state = States.KEYWORD_BOO;
                        }
                        //none of the possible letters indicate keyword, check if valid ID
                        else if(thisCharIsLowerCaseLetter(scannedSymbol) || thisCharIsADigit(scannedSymbol)){
                            state = States.ID;
                        }
                        else{
                            //invalid char, go to ID ERROR
                            state = States.INVALID_ID;
                        }
                        break;
                    case States.KEYWORD_BOO:
                        scannedString += scannedSymbol;
                        //check for next letter of keyword
                        if(scannedSymbol == 'l'){
                            state = States.KEYWORD_BOOL;
                        }
                        //none of the possible letters indicate keyword, check if valid ID
                        else if(thisCharIsLowerCaseLetter(scannedSymbol) || thisCharIsADigit(scannedSymbol)){
                            state = States.ID;
                        }
                        else{
                            //invalid char, go to ID ERROR
                            state = States.INVALID_ID;
                        }
                        break;
                    case States.KEYWORD_BOOL:
                        scannedString += scannedSymbol;
                        //check for next letter of keyword
                        if(scannedSymbol == 'e'){
                            state = States.KEYWORD_BOOLE;
                        }
                        //none of the possible letters indicate keyword, check if valid ID
                        else if(thisCharIsLowerCaseLetter(scannedSymbol) || thisCharIsADigit(scannedSymbol)){
                            state = States.ID;
                        }
                        else{
                            //invalid char, go to ID ERROR
                            state = States.INVALID_ID;
                        }
                        break;
                    case States.KEYWORD_BOOLE:
                        scannedString += scannedSymbol;
                        //check for next letter of keyword
                        if(scannedSymbol == 'a'){
                            state = States.KEYWORD_BOOLEA;
                        }
                        //none of the possible letters indicate keyword, check if valid ID
                        else if(thisCharIsLowerCaseLetter(scannedSymbol) || thisCharIsADigit(scannedSymbol)){
                            state = States.ID;
                        }
                        else{
                            //invalid char, go to ID ERROR
                            state = States.INVALID_ID;
                        }
                        break;
                    case States.KEYWORD_BOOLEA:
                        scannedString += scannedSymbol;
                        //check for next letter of keyword
                        if(scannedSymbol == 'n'){
                             //keyword "boolean" detected , go to KEYWORD state
                            state = States.KEYWORD;
                        }
                        //none of the possible letters indicate keyword, check if valid ID
                        else if(thisCharIsLowerCaseLetter(scannedSymbol) || thisCharIsADigit(scannedSymbol)){
                            state = States.ID;
                        }
                        else{
                            //invalid char, go to ID ERROR
                            state = States.INVALID_ID;
                        }
                        break;
                    case States.KEYWORD_E:
                        scannedString += scannedSymbol;
                        //check for next letter of keyword
                        if(scannedSymbol == 'l'){
                            //check for keyword else
                            state = States.KEYWORD_EL;
                        }
                        //none of the possible letters indicate keyword, check if valid ID
                        else if(thisCharIsLowerCaseLetter(scannedSymbol) || thisCharIsADigit(scannedSymbol)){
                            state = States.ID;
                        }
                        else{
                            //invalid char, go to ID ERROR
                            state = States.INVALID_ID;
                        }
                        break;
                    case States.KEYWORD_EL:
                        scannedString += scannedSymbol;
                        //check for next letter of keyword
                        if(scannedSymbol == 's'){
                            state = States.KEYWORD_ELS;
                        }
                        //none of the possible letters indicate keyword, check if valid ID
                        else if(thisCharIsLowerCaseLetter(scannedSymbol) || thisCharIsADigit(scannedSymbol)){
                            state = States.ID;
                        }
                        else{
                            //invalid char, go to ID ERROR
                            state = States.INVALID_ID;
                        }
                        break;
                    case States.KEYWORD_ELS:
                        scannedString += scannedSymbol;
                        //check for next letter of keyword
                        if(scannedSymbol == 'e'){
                             //keyword "else" detected , go to KEYWORD state
                            state = States.KEYWORD;
                        }
                        //none of the possible letters indicate keyword, check if valid ID
                        else if(thisCharIsLowerCaseLetter(scannedSymbol) || thisCharIsADigit(scannedSymbol)){
                            state = States.ID;
                        }
                        else{
                            //invalid char, go to ID ERROR
                            state = States.INVALID_ID;
                        }
                        break;
                    case States.KEYWORD_F:
                        scannedString += scannedSymbol;
                        //check for next letter of keyword
                        if(scannedSymbol == 'a'){
                            //check for keyword fact
                            state = States.KEYWORD_FA;
                        }
                        //none of the possible letters indicate keyword, check if valid ID
                        else if(thisCharIsLowerCaseLetter(scannedSymbol) || thisCharIsADigit(scannedSymbol)){
                            state = States.ID;
                        }
                        else{
                            //invalid char, go to ID ERROR
                            state = States.INVALID_ID;
                        }
                        break;
                    case States.KEYWORD_FA:
                        scannedString += scannedSymbol;
                        //check for next letter of keyword
                        if(scannedSymbol == 'c'){
                            state = States.KEYWORD_FAC;
                        }
                        //none of the possible letters indicate keyword, check if valid ID
                        else if(thisCharIsLowerCaseLetter(scannedSymbol) || thisCharIsADigit(scannedSymbol)){
                            state = States.ID;
                        }
                        else{
                            //invalid char, go to ID ERROR
                            state = States.INVALID_ID;
                        }
                        break;
                    case States.KEYWORD_FAC:
                        scannedString += scannedSymbol;
                        //check for next letter of keyword
                        if(scannedSymbol == 't'){
                             //keyword "fact" detected , go to KEYWORD state
                            state = States.KEYWORD;
                        }
                        //none of the possible letters indicate keyword, check if valid ID
                        else if(thisCharIsLowerCaseLetter(scannedSymbol) || thisCharIsADigit(scannedSymbol)){
                            state = States.ID;
                        }
                        else{
                            //invalid char, go to ID ERROR
                            state = States.INVALID_ID;
                        }
                        break;
                    case States.KEYWORD_G:
                        scannedString += scannedSymbol;
                        //check for next letter of keyword
                        if(scannedSymbol == 'r'){
                            //check for keyword great
                            state = States.KEYWORD_GR;
                        }
                        //none of the possible letters indicate keyword, check if valid ID
                        else if(thisCharIsLowerCaseLetter(scannedSymbol) || thisCharIsADigit(scannedSymbol)){
                            state = States.ID;
                        }
                        else{
                            //invalid char, go to ID ERROR
                            state = States.INVALID_ID;
                        }
                        break;
                    case States.KEYWORD_GR:
                        scannedString += scannedSymbol;
                        //check for next letter of keyword
                        if(scannedSymbol == 'e'){
                            state = States.KEYWORD_GRE;
                        }
                        //none of the possible letters indicate keyword, check if valid ID
                        else if(thisCharIsLowerCaseLetter(scannedSymbol) || thisCharIsADigit(scannedSymbol)){
                            state = States.ID;
                        }
                        else{
                            //invalid char, go to ID ERROR
                            state = States.INVALID_ID;
                        }
                        break;
                    case States.KEYWORD_GRE:
                        scannedString += scannedSymbol;
                        //check for next letter of keyword
                        if(scannedSymbol == 'a'){
                            state = States.KEYWORD_GREA;
                        }
                        //none of the possible letters indicate keyword, check if valid ID
                        else if(thisCharIsLowerCaseLetter(scannedSymbol) || thisCharIsADigit(scannedSymbol)){
                            state = States.ID;
                        }
                        else{
                            //invalid char, go to ID ERROR
                            state = States.INVALID_ID;
                        }
                        break;
                    case States.KEYWORD_GREA:
                        scannedString += scannedSymbol;
                        //check for next letter of keyword
                        if(scannedSymbol == 't'){
                             //keyword "great" detected , go to KEYWORD state
                            state = States.KEYWORD;
                        }
                        //none of the possible letters indicate keyword, check if valid ID
                        else if(thisCharIsLowerCaseLetter(scannedSymbol) || thisCharIsADigit(scannedSymbol)){
                            state = States.ID;
                        }
                        else{
                            //invalid char, go to ID ERROR
                            state = States.INVALID_ID;
                        }
                        break;
                    case States.KEYWORD_I:
                        scannedString += scannedSymbol;
                        //check for next letter of keyword
                        if(scannedSymbol == 's'){
                            //keyword "is" detected , go to KEYWORD state
                            state = States.KEYWORD;
                        }
                        else if(scannedSymbol == 'f'){
                            //keyword "if" detected , go to KEYWORD state
                            state = States.KEYWORD;
                        }
                        //none of the possible letters indicate keyword, check if valid ID
                        else if(thisCharIsLowerCaseLetter(scannedSymbol) || thisCharIsADigit(scannedSymbol)){
                            state = States.ID;
                        }
                        else{
                            //invalid char, go to ID ERROR
                            state = States.INVALID_ID;
                        }
                        break;
                    case States.KEYWORD_L:
                        scannedString += scannedSymbol;
                        //check for next letter of keyword
                        if(scannedSymbol == 'e'){
                            //check for keyword less
                            state = States.KEYWORD_LE;
                        }
                        else if(scannedSymbol == 'i'){
                            //check for keyword lie
                            state = States.KEYWORD_LI;
                        }
                        else if(scannedSymbol == 'o'){
                            //check for keyword long
                            state = States.KEYWORD_LO;
                        }
                        //none of the possible letters indicate keyword, check if valid ID
                        else if(thisCharIsLowerCaseLetter(scannedSymbol) || thisCharIsADigit(scannedSymbol)){
                            state = States.ID;
                        }
                        else{
                            //invalid char, go to ID ERROR
                            state = States.INVALID_ID;
                        }
                        break;
                    case States.KEYWORD_LE:
                        scannedString += scannedSymbol;
                        //check for next letter of keyword
                        if(scannedSymbol == 's'){
                            state = States.KEYWORD_LES;
                        }
                        //none of the possible letters indicate keyword, check if valid ID
                        else if(thisCharIsLowerCaseLetter(scannedSymbol) || thisCharIsADigit(scannedSymbol)){
                            state = States.ID;
                        }
                        else{
                            //invalid char, go to ID ERROR
                            state = States.INVALID_ID;
                        }
                        break;
                    case States.KEYWORD_LES:
                        scannedString += scannedSymbol;
                        //check for next letter of keyword
                        if(scannedSymbol == 's'){
                             //keyword "less" detected , go to KEYWORD state
                            state = States.KEYWORD;
                        }
                        //none of the possible letters indicate keyword, check if valid ID
                        else if(thisCharIsLowerCaseLetter(scannedSymbol) || thisCharIsADigit(scannedSymbol)){
                            state = States.ID;
                        }
                        else{
                            //invalid char, go to ID ERROR
                            state = States.INVALID_ID;
                        }
                        break;
                    case States.KEYWORD_LI:
                        scannedString += scannedSymbol;
                        //check for next letter of keyword
                        if(scannedSymbol == 'e'){
                             //keyword "lie" detected , go to KEYWORD state
                            state = States.KEYWORD;
                        }
                        //none of the possible letters indicate keyword, check if valid ID
                        else if(thisCharIsLowerCaseLetter(scannedSymbol) || thisCharIsADigit(scannedSymbol)){
                            state = States.ID;
                        }
                        else{
                            //invalid char, go to ID ERROR
                            state = States.INVALID_ID;
                        }
                        break;
                    case States.KEYWORD_LO:
                        scannedString += scannedSymbol;
                        //check for next letter of keyword
                        if(scannedSymbol == 'n'){
                            state = States.KEYWORD_LON;
                        }
                        //none of the possible letters indicate keyword, check if valid ID
                        else if(thisCharIsLowerCaseLetter(scannedSymbol) || thisCharIsADigit(scannedSymbol)){
                            state = States.ID;
                        }
                        else{
                            //invalid char, go to ID ERROR
                            state = States.INVALID_ID;
                        }
                        break;
                    case States.KEYWORD_LON:
                        scannedString += scannedSymbol;
                        //check for next letter of keyword
                        if(scannedSymbol == 'g'){
                             //keyword "long" detected , go to KEYWORD state
                            state = States.KEYWORD;
                        }
                        //none of the possible letters indicate keyword, check if valid ID
                        else if(thisCharIsLowerCaseLetter(scannedSymbol) || thisCharIsADigit(scannedSymbol)){
                            state = States.ID;
                        }
                        else{
                            //invalid char, go to ID ERROR
                            state = States.INVALID_ID;
                        }
                        break;
                    case States.KEYWORD_M:
                        scannedString += scannedSymbol;
                        //check for next letter of keyword
                        if(scannedSymbol == 'a'){
                            //check for keyword make
                            state = States.KEYWORD_MA;
                        }
                        else if(scannedSymbol == 'o'){
                            //check for keyword more
                            state = States.KEYWORD_MO;
                        }
                        //none of the possible letters indicate keyword, check if valid ID
                        else if(thisCharIsLowerCaseLetter(scannedSymbol) || thisCharIsADigit(scannedSymbol)){
                            state = States.ID;
                        }
                        else{
                            //invalid char, go to ID ERROR
                            state = States.INVALID_ID;
                        }
                        break;
                    case States.KEYWORD_MA:
                        scannedString += scannedSymbol;
                        //check for next letter of keyword
                        if(scannedSymbol == 'k'){
                            state = States.KEYWORD_MAK;
                        }
                        //none of the possible letters indicate keyword, check if valid ID
                        else if(thisCharIsLowerCaseLetter(scannedSymbol) || thisCharIsADigit(scannedSymbol)){
                            state = States.ID;
                        }
                        else{
                            //invalid char, go to ID ERROR
                            state = States.INVALID_ID;
                        }
                        break;
                    case States.KEYWORD_MAK:
                        scannedString += scannedSymbol;
                        //check for next letter of keyword
                        if(scannedSymbol == 'e'){
                             //keyword "make" detected , go to KEYWORD state
                            state = States.KEYWORD;
                        }
                        //none of the possible letters indicate keyword, check if valid ID
                        else if(thisCharIsLowerCaseLetter(scannedSymbol) || thisCharIsADigit(scannedSymbol)){
                            state = States.ID;
                        }
                        else{
                            //invalid char, go to ID ERROR
                            state = States.INVALID_ID;
                        }
                        break;
                    case States.KEYWORD_MO:
                        scannedString += scannedSymbol;
                        //check for next letter of keyword
                        if(scannedSymbol == 'r'){
                            state = States.KEYWORD_MOR;
                        }
                        //none of the possible letters indicate keyword, check if valid ID
                        else if(thisCharIsLowerCaseLetter(scannedSymbol) || thisCharIsADigit(scannedSymbol)){
                            state = States.ID;
                        }
                        else{
                            //invalid char, go to ID ERROR
                            state = States.INVALID_ID;
                        }
                        break;
                    case States.KEYWORD_MOR:
                        scannedString += scannedSymbol;
                        //check for next letter of keyword
                        if(scannedSymbol == 'e'){
                             //keyword "more" detected , go to KEYWORD state
                            state = States.KEYWORD;
                        }
                        //none of the possible letters indicate keyword, check if valid ID
                        else if(thisCharIsLowerCaseLetter(scannedSymbol) || thisCharIsADigit(scannedSymbol)){
                            state = States.ID;
                        }
                        else{
                            //invalid char, go to ID ERROR
                            state = States.INVALID_ID;
                        }
                        break;
                    case States.KEYWORD_N:
                        scannedString += scannedSymbol;
                        //check for next letter of keyword
                        if(scannedSymbol == 'o'){
                            //check for keyword not
                            state = States.KEYWORD_NO;
                        }
                        else if(scannedSymbol == 'u'){
                            //check for keyword number
                            state = States.KEYWORD_NU;
                        }
                        //none of the possible letters indicate keyword, check if valid ID
                        else if(thisCharIsLowerCaseLetter(scannedSymbol) || thisCharIsADigit(scannedSymbol)){
                            state = States.ID;
                        }
                        else{
                            //invalid char, go to ID ERROR
                            state = States.INVALID_ID;
                        }
                        break;
                    case States.KEYWORD_NO:
                        scannedString += scannedSymbol;
                        //check for next letter of keyword
                        if(scannedSymbol == 't'){
                             //keyword "not" detected , go to KEYWORD state
                            state = States.KEYWORD;
                        }
                        //none of the possible letters indicate keyword, check if valid ID
                        else if(thisCharIsLowerCaseLetter(scannedSymbol) || thisCharIsADigit(scannedSymbol)){
                            state = States.ID;
                        }
                        else{
                            //invalid char, go to ID ERROR
                            state = States.INVALID_ID;
                        }
                        break;
                    case States.KEYWORD_NU:
                        scannedString += scannedSymbol;
                        //check for next letter of keyword
                        if(scannedSymbol == 'm'){
                            state = States.KEYWORD_NUM;
                        }
                        //none of the possible letters indicate keyword, check if valid ID
                        else if(thisCharIsLowerCaseLetter(scannedSymbol) || thisCharIsADigit(scannedSymbol)){
                            state = States.ID;
                        }
                        else{
                            //invalid char, go to ID ERROR
                            state = States.INVALID_ID;
                        }
                        break;
                    case States.KEYWORD_NUM:
                        scannedString += scannedSymbol;
                        //check for next letter of keyword
                        if(scannedSymbol == 'b'){
                            state = States.KEYWORD_NUMB;
                        }
                        //none of the possible letters indicate keyword, check if valid ID
                        else if(thisCharIsLowerCaseLetter(scannedSymbol) || thisCharIsADigit(scannedSymbol)){
                            state = States.ID;
                        }
                        else{
                            //invalid char, go to ID ERROR
                            state = States.INVALID_ID;
                        }
                        break;
                    case States.KEYWORD_NUMB:
                        scannedString += scannedSymbol;
                        //check for next letter of keyword
                        if(scannedSymbol == 'e'){
                            state = States.KEYWORD_NUMBE;
                        }
                        //none of the possible letters indicate keyword, check if valid ID
                        else if(thisCharIsLowerCaseLetter(scannedSymbol) || thisCharIsADigit(scannedSymbol)){
                            state = States.ID;
                        }
                        else{
                            //invalid char, go to ID ERROR
                            state = States.INVALID_ID;
                        }
                        break;
                    case States.KEYWORD_NUMBE:
                        scannedString += scannedSymbol;
                        //check for next letter of keyword
                        if(scannedSymbol == 'r'){
                             //keyword "numeber" detected , go to KEYWORD state
                            state = States.KEYWORD;
                        }
                        //none of the possible letters indicate keyword, check if valid ID
                        else if(thisCharIsLowerCaseLetter(scannedSymbol) || thisCharIsADigit(scannedSymbol)){
                            state = States.ID;
                        }
                        else{
                            //invalid char, go to ID ERROR
                            state = States.INVALID_ID;
                        }
                        break;
                    case States.KEYWORD_O:
                        scannedString += scannedSymbol;
                        //check for next letter of keyword
                        if(scannedSymbol == 'r'){
                             //keyword "or" detected , go to KEYWORD state
                            state = States.KEYWORD;
                        }
                        //none of the possible letters indicate keyword, check if valid ID
                        else if(thisCharIsLowerCaseLetter(scannedSymbol) || thisCharIsADigit(scannedSymbol)){
                            state = States.ID;
                        }
                        else{
                            //invalid char, go to ID ERROR
                            state = States.INVALID_ID;
                        }
                        break;
                    case States.KEYWORD_P:
                        scannedString += scannedSymbol;
                        //check for next letter of keyword
                        if(scannedSymbol == 'l'){
                            //check for keyword plus
                            state = States.KEYWORD_PL;
                        }
                        else if(scannedSymbol == 'r'){
                            //check for keyword programming
                            state = States.KEYWORD_PR;
                        }
                        //none of the possible letters indicate keyword, check if valid ID
                        else if(thisCharIsLowerCaseLetter(scannedSymbol) || thisCharIsADigit(scannedSymbol)){
                            state = States.ID;
                        }
                        else{
                            //invalid char, go to ID ERROR
                            state = States.INVALID_ID;
                        }
                        break;
                    case States.KEYWORD_PL:
                        scannedString += scannedSymbol;
                        //check for next letter of keyword
                        if(scannedSymbol == 'u'){
                            state = States.KEYWORD_PLU;
                        }
                        //none of the possible letters indicate keyword, check if valid ID
                        else if(thisCharIsLowerCaseLetter(scannedSymbol) || thisCharIsADigit(scannedSymbol)){
                            state = States.ID;
                        }
                        else{
                            //invalid char, go to ID ERROR
                            state = States.INVALID_ID;
                        }
                        break;
                    case States.KEYWORD_PLU:
                        scannedString += scannedSymbol;
                        //check for next letter of keyword
                        if(scannedSymbol == 's'){
                             //keyword "plus" detected , go to KEYWORD state
                            state = States.KEYWORD;
                        }
                        //none of the possible letters indicate keyword, check if valid ID
                        else if(thisCharIsLowerCaseLetter(scannedSymbol) || thisCharIsADigit(scannedSymbol)){
                            state = States.ID;
                        }
                        else{
                            //invalid char, go to ID ERROR
                            state = States.INVALID_ID;
                        }
                        break;
                    case States.KEYWORD_PR:
                        scannedString += scannedSymbol;
                        //check for next letter of keyword
                        if(scannedSymbol == 'o'){
                            state = States.KEYWORD_PRO;
                        }
                        //none of the possible letters indicate keyword, check if valid ID
                        else if(thisCharIsLowerCaseLetter(scannedSymbol) || thisCharIsADigit(scannedSymbol)){
                            state = States.ID;
                        }
                        else{
                            //invalid char, go to ID ERROR
                            state = States.INVALID_ID;
                        }
                        break;
                    case States.KEYWORD_PRO:
                        scannedString += scannedSymbol;
                        //check for next letter of keyword
                        if(scannedSymbol == 'g'){
                            state = States.KEYWORD_PROG;
                        }
                        //none of the possible letters indicate keyword, check if valid ID
                        else if(thisCharIsLowerCaseLetter(scannedSymbol) || thisCharIsADigit(scannedSymbol)){
                            state = States.ID;
                        }
                        else{
                            //invalid char, go to ID ERROR
                            state = States.INVALID_ID;
                        }
                        break;
                    case States.KEYWORD_PROG:
                        scannedString += scannedSymbol;
                        //check for next letter of keyword
                        if(scannedSymbol == 'r'){
                            state = States.KEYWORD_PROGR;
                        }
                        //none of the possible letters indicate keyword, check if valid ID
                        else if(thisCharIsLowerCaseLetter(scannedSymbol) || thisCharIsADigit(scannedSymbol)){
                            state = States.ID;
                        }
                        else{
                            //invalid char, go to ID ERROR
                            state = States.INVALID_ID;
                        }
                        break;
                    case States.KEYWORD_PROGR:
                        scannedString += scannedSymbol;
                        //check for next letter of keyword
                        if(scannedSymbol == 'a'){
                            state = States.KEYWORD_PROGRA;
                        }
                        //none of the possible letters indicate keyword, check if valid ID
                        else if(thisCharIsLowerCaseLetter(scannedSymbol) || thisCharIsADigit(scannedSymbol)){
                            state = States.ID;
                        }
                        else{
                            //invalid char, go to ID ERROR
                            state = States.INVALID_ID;
                        }
                        break;
                    case States.KEYWORD_PROGRA:
                        scannedString += scannedSymbol;
                        //check for next letter of keyword
                        if(scannedSymbol == 'm'){
                            state = States.KEYWORD_PROGRAM;
                        }
                        //none of the possible letters indicate keyword, check if valid ID
                        else if(thisCharIsLowerCaseLetter(scannedSymbol) || thisCharIsADigit(scannedSymbol)){
                            state = States.ID;
                        }
                        else{
                            //invalid char, go to ID ERROR
                            state = States.INVALID_ID;
                        }
                        break;
                    case States.KEYWORD_PROGRAM:
                        scannedString += scannedSymbol;
                        //check for next letter of keyword
                        if(scannedSymbol == 'm'){
                            state = States.KEYWORD_PROGRAMM;
                        }
                        //none of the possible letters indicate keyword, check if valid ID
                        else if(thisCharIsLowerCaseLetter(scannedSymbol) || thisCharIsADigit(scannedSymbol)){
                            state = States.ID;
                        }
                        else{
                            //invalid char, go to ID ERROR
                            state = States.INVALID_ID;
                        }
                        break;
                    case States.KEYWORD_PROGRAMM:
                        scannedString += scannedSymbol;
                        //check for next letter of keyword
                        if(scannedSymbol == 'i'){
                            state = States.KEYWORD_PROGRAMMI;
                        }
                        //none of the possible letters indicate keyword, check if valid ID
                        else if(thisCharIsLowerCaseLetter(scannedSymbol) || thisCharIsADigit(scannedSymbol)){
                            state = States.ID;
                        }
                        else{
                            //invalid char, go to ID ERROR
                            state = States.INVALID_ID;
                        }
                        break;
                    case States.KEYWORD_PROGRAMMI:
                        scannedString += scannedSymbol;
                        //check for next letter of keyword
                        if(scannedSymbol == 'n'){
                            state = States.KEYWORD_PROGRAMMIN;
                        }
                        //none of the possible letters indicate keyword, check if valid ID
                        else if(thisCharIsLowerCaseLetter(scannedSymbol) || thisCharIsADigit(scannedSymbol)){
                            state = States.ID;
                        }
                        else{
                            //invalid char, go to ID ERROR
                            state = States.INVALID_ID;
                        }
                        break;
                    case States.KEYWORD_PROGRAMMIN:
                        scannedString += scannedSymbol;
                        //check for next letter of keyword
                        if(scannedSymbol == 'g'){
                             //keyword "programming" detected , go to KEYWORD state
                            state = States.KEYWORD;
                        }
                        //none of the possible letters indicate keyword, check if valid ID
                        else if(thisCharIsLowerCaseLetter(scannedSymbol) || thisCharIsADigit(scannedSymbol)){
                            state = States.ID;
                        }
                        else{
                            //invalid char, go to ID ERROR
                            state = States.INVALID_ID;
                        }
                        break;
                    case States.KEYWORD_S:
                        scannedString += scannedSymbol;
                        //check for next letter of keyword
                        if(scannedSymbol == 'a'){
                            //check for keyword say
                            state = States.KEYWORD_SA;
                        }
                        //none of the possible letters indicate keyword, check if valid ID
                        else if(thisCharIsLowerCaseLetter(scannedSymbol) || thisCharIsADigit(scannedSymbol)){
                            state = States.ID;
                        }
                        else{
                            //invalid char, go to ID ERROR
                            state = States.INVALID_ID;
                        }
                        break;
                    case States.KEYWORD_SA:
                        scannedString += scannedSymbol;
                        //check for next letter of keyword
                        if(scannedSymbol == 'y'){
                             //keyword "say" detected , go to KEYWORD state
                            state = States.KEYWORD;
                        }
                        //none of the possible letters indicate keyword, check if valid ID
                        else if(thisCharIsLowerCaseLetter(scannedSymbol) || thisCharIsADigit(scannedSymbol)){
                            state = States.ID;
                        }
                        else{
                            //invalid char, go to ID ERROR
                            state = States.INVALID_ID;
                        }
                        break;
                    case States.KEYWORD_T:
                        scannedString += scannedSymbol;
                        //check for next letter of keyword
                        if(scannedSymbol == 'e'){
                            //check for keyword tell
                            state = States.KEYWORD_TE;
                        }
                        else if(scannedSymbol == 'i'){
                            //check for keyword times
                            state = States.KEYWORD_TI;
                        }
                        //none of the possible letters indicate keyword, check if valid ID
                        else if(thisCharIsLowerCaseLetter(scannedSymbol) || thisCharIsADigit(scannedSymbol)){
                            state = States.ID;
                        }
                        else{
                            //invalid char, go to ID ERROR
                            state = States.INVALID_ID;
                        }
                        break;
                    case States.KEYWORD_TE:
                        scannedString += scannedSymbol;
                        //check for next letter of keyword
                        if(scannedSymbol == 'l'){
                            state = States.KEYWORD_TEL;
                        }
                        //none of the possible letters indicate keyword, check if valid ID
                        else if(thisCharIsLowerCaseLetter(scannedSymbol) || thisCharIsADigit(scannedSymbol)){
                            state = States.ID;
                        }
                        else{
                            //invalid char, go to ID ERROR
                            state = States.INVALID_ID;
                        }
                        break;
                    case States.KEYWORD_TEL:
                        scannedString += scannedSymbol;
                        //check for next letter of keyword
                        if(scannedSymbol == 'l'){
                             //keyword "tell" detected , go to KEYWORD state
                            state = States.KEYWORD;
                        }
                        //none of the possible letters indicate keyword, check if valid ID
                        else if(thisCharIsLowerCaseLetter(scannedSymbol) || thisCharIsADigit(scannedSymbol)){
                            state = States.ID;
                        }
                        else{
                            //invalid char, go to ID ERROR
                            state = States.INVALID_ID;
                        }
                        break;
                    case States.KEYWORD_TI:
                        scannedString += scannedSymbol;
                        //check for next letter of keyword
                        if(scannedSymbol == 'm'){
                            state = States.KEYWORD_TIM;
                        }
                        //none of the possible letters indicate keyword, check if valid ID
                        else if(thisCharIsLowerCaseLetter(scannedSymbol) || thisCharIsADigit(scannedSymbol)){
                            state = States.ID;
                        }
                        else{
                            //invalid char, go to ID ERROR
                            state = States.INVALID_ID;
                        }
                        break;
                    case States.KEYWORD_TIM:
                        scannedString += scannedSymbol;
                        //check for next letter of keyword
                        if(scannedSymbol == 'e'){
                            state = States.KEYWORD_TIME;
                        }
                        //none of the possible letters indicate keyword, check if valid ID
                        else if(thisCharIsLowerCaseLetter(scannedSymbol) || thisCharIsADigit(scannedSymbol)){
                            state = States.ID;
                        }
                        else{
                            //invalid char, go to ID ERROR
                            state = States.INVALID_ID;
                        }
                        break;
                    case States.KEYWORD_TIME:
                        scannedString += scannedSymbol;
                        //check for next letter of keyword
                        if(scannedSymbol == 's'){
                             //keyword "times" detected , go to KEYWORD state
                            state = States.KEYWORD;
                        }
                        //none of the possible letters indicate keyword, check if valid ID
                        else if(thisCharIsLowerCaseLetter(scannedSymbol) || thisCharIsADigit(scannedSymbol)){
                            state = States.ID;
                        }
                        else{
                            //invalid char, go to ID ERROR
                            state = States.INVALID_ID;
                        }
                        break;
                    case States.ID:
                        scannedString += scannedSymbol;
                        //if scanned char is letter or digit it is still an ID
                        if(thisCharIsLowerCaseLetter(scannedSymbol) || thisCharIsADigit(scannedSymbol)){
                            state = States.ID;
                        }
                        else{
                            //invalid char, go to ID ERROR
                            state = States.INVALID_ID;
                        }
                        break;
                    case States.INVALID_ID:
                        //doesn't matter what the next char is the token is an invalid ID
                        scannedString += scannedSymbol;
                        state = States.INVALID_ID;
                        break;
                    case States.GENERIC_ERROR:
                        //doesn't matter what the next char is the token is invalid
                        scannedString += scannedSymbol;
                        break;
                    default:
                        //unknown state go to default error state
                        scannedString += scannedSymbol;
                        state = States.GENERIC_ERROR;
                        break;
                }
            }

            //check to see if in an accepting state
            if(isInAcceptingState(state)){
                if(state == States.CONSTANT){
                    //constant state detected, make sure scanned token is > 1,000,000
                    if(scannedString == "1000000"){
                        //1,000,000 not valid go to constant error
                        state = States.CONSTANT_ERROR;
                        scannedToken = new Token(state.ToString(), Type.ERROR);
                        errorHandler.printErrorForThisState(state);
                    }
                    else{
                        //valid CONSTANT, ask Bookkeeper to put in SymTab and print
                        scannedToken = new Token(scannedString, Type.CONSTANT); 
                        bookkeeper.addToken(scannedToken);
                        printToken();
                    }
                }
                else if(state == States.STRING_VALID){
                    //valid STRING, ask Bookkeeper to put in SymTab and print
                    scannedToken = new Token(scannedString, Type.STRING); 
                    bookkeeper.addToken(scannedToken);
                    printToken();
                }
                else if(state == States.ID){
                    //valid ID, ask Bookkeeper to put in SymTab and print
                    scannedToken = new Token(scannedString, Type.ID); 
                    bookkeeper.addToken(scannedToken);
                    printToken();
                }
                else if(state == States.SPECIAL_CHARACTER){
                    //special character, print
                    scannedToken = new Token(scannedString, Type.SPECIAL_SYMBOL); 
                    printToken();
                }
                //all states containing "KEYWORD" are valid
                else if(state.ToString().Substring(0, 7) == "KEYWORD"){
                    // only states containing exactly "KEYWORD" are a KEYWORD, print
                    if(state.ToString() == "KEYWORD"){
                        scannedToken = new Token(scannedString, Type.KEYWORD);
                        printToken();
                    }
                    else{
                        //if state is not exactly "KEYWORD", it is an ID, print
                        scannedToken = new Token(scannedString, Type.ID);
                        printToken();
                    }
                }
            }
            else{
                //unaccepts states are ERRORS, print them
                scannedToken = new Token(state.ToString(), Type.ERROR);
                errorHandler.printErrorForThisState(state);
            }
            return scannedToken;
        }
        public void printToken(){
            //output the scanned token
            Console.WriteLine(scannedToken.ToString());
        }
        public bool isEndOfToken(){
            //peeks to see if next char is a token seperator
            return nextCharIsSpecial() 
                || nextCharIsWhitespace() 
                || reader.Peek() == -1 
                || (char)reader.Peek() == '#';
        }
        public bool isInAcceptingState(States currentState){
            //detects accepting state
            return (currentState == States.STRING_VALID) 
                || (currentState == States.CONSTANT) 
                || (currentState == States.ID)
                || (currentState == States.SPECIAL_CHARACTER)
                || (currentState.ToString().Substring(0, 7) == "KEYWORD");
        }

        public char getNextCharacter(){
            //returns the nect char, if next char is capital letter, returns lowercase version
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
            //returns boolean of whether passed character is a letter
            bool isLower = false;
            if(96 < (int)passedChar && (int)passedChar < 123){
                isLower = true;
            }
            return isLower;
        }
        public bool thisCharIsADigit(char passedChar){
            //returns boolean of whether passed character is a digit
            bool isDigit = false;
            if(47 < (int)passedChar && (int)passedChar < 58){
                isDigit = true;
            }
            return isDigit;
        }

        public char getLowerCaseofNextLetter(){
            //returns upper case that is next letter to be read as lowercase letter
            char returnChar = (char)(reader.Read() + 32);
            return returnChar;
        }
        public bool nextCharIsWhitespace(){
            //returns whether next char is whitelspace
            bool isWhitespace = false;
            if(reader.Peek() == 32){
                //space character
                isWhitespace = true;
            }
            if(reader.Peek() == 9){
                //tab char
                isWhitespace = true;
            }
            if(reader.Peek() == 10){
                //line feed char
                isWhitespace = true;
            }
            if(reader.Peek() == 13){
                //carriage return char
                isWhitespace = true;
            }
            return isWhitespace;
        }
        public bool nextCharIsSpecial(){
            //checks for one of the 7 special characters
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
            //reads entire rest of line and does nothing with it
            reader.ReadLine();
        }
        public bool nextIsNewLineChar(){
            //checks if new line or return carriage char is next
            bool returnBool = false;
            if(reader.Peek() == 13 || reader.Peek() == 10){
                returnBool = true;
            }
            return returnBool;
        }
    }
    //Token class for storing lexemes and types as an object
    class Token{
        string lexeme;
        Type tokenType;
        //constructor
        public Token(string passedLexeme, Type passedType){
            lexeme = passedLexeme;
            tokenType = passedType;
        }
        //getter for Lexeme
        public string Lexeme{
            get{
                return lexeme;
            }
        }
        //getter/setter for Token Type
        public Type TokenType{
            get{
                return tokenType;
            } 
            set{
                tokenType = value;
            }
        }
        //overriden ToString method
        override
        public string ToString(){
            //prints Token, if statement for human readability in console
            if(lexeme.Length < 7){
                return "Lexeme: " + lexeme + " \t\t Type: " + tokenType.ToString();
            }
            else{
                return "Lexeme: " + lexeme + " \t Type: " + tokenType.ToString();
            }
        }
    }
    //simple class with predefined error messages
    class ErrorHandler{
        //constructor
        public ErrorHandler(){
            
        }
        KeyValuePair<string, object>[] errorOutputs = {
            new KeyValuePair<string,object>( "GENERIC_ERROR", "Any other erro: Trump doesn't want to hearit." ),
            new KeyValuePair<string,object>( "CONSTANT_ERROR", "[const] error: I'm really rich, part of the beauty of meis I'm very rich."),
            new KeyValuePair<string,object>( "INVALID_ID", "[id] error: This is a country where we speak English.")
        };
        public void printErrorForThisState(States stateToPrintErrorFor){
            //given state print corresponding error
            if(stateToPrintErrorFor.ToString() == "GENERIC_ERROR"){
                printErrorTest(0);
            }
            else if(stateToPrintErrorFor.ToString() == "CONSTANT_ERROR"){
                printErrorTest(1);
            }
            else if(stateToPrintErrorFor.ToString() == "INVALID_ID"){
                printErrorTest(2);
            }
        }
        public void printErrorTest(int indexOfError){
            //print error of certain type
            Console.WriteLine(errorOutputs[indexOfError].Value);
        }
    }
    //Bookkeeper for remembering unique instances of an ID, STRING, of CONSTANT
    class Bookkeeper{
        List<Token> SymTab = new List<Token>();
        
        //constructor
        public Bookkeeper(){
            //set capacity of 100
            setCapacity();
        }

        public void addToken(Token tokenToAdd){
            //addToken Token to List if is has not already been added
            bool found = false;
            for(int i = 0; i < SymTab.Count; i++){
                if(SymTab[i].Lexeme == tokenToAdd.Lexeme){
                    found = true;
                }
            }
            if(!found){
                SymTab.Add(tokenToAdd);
            }
        }
        public void setCapacity(){
            //set capacity of 100
            SymTab.Capacity = 100;
        }

        public void printSymTab(){
            //output entire SymTab
            for(int i = 0; i < SymTab.Count; i++){
                Console.WriteLine(SymTab[i].ToString());
            }
        }
    }
    //enumerations of Tyes of tokens
    enum Type {ID, CONSTANT, STRING, KEYWORD, SPECIAL_SYMBOL, ERROR};
    //enumeration of possible states
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
        //keyword states (alphabetically)
        KEYWORD,
        KEYWORD_A, KEYWORD_AM, KEYWORD_AME, KEYWORD_AMER, KEYWORD_AMERI, KEYWORD_AMERIC,
        KEYWORD_AG, KEYWORD_AGA, KEYWORD_AGAI,
        KEYWORD_AN,
        KEYWORD_B, KEYWORD_BO, KEYWORD_BOO, KEYWORD_BOOL, KEYWORD_BOOLE, KEYWORD_BOOLEA,
        KEYWORD_E, KEYWORD_EL, KEYWORD_ELS,
        KEYWORD_F, KEYWORD_FA, KEYWORD_FAC,
        KEYWORD_G, KEYWORD_GR, KEYWORD_GRE, KEYWORD_GREA,
        KEYWORD_I, 
        KEYWORD_L, KEYWORD_LE, KEYWORD_LES, KEYWORD_LI, KEYWORD_LO, KEYWORD_LON,
        KEYWORD_M, KEYWORD_MA, KEYWORD_MO, KEYWORD_MAK, KEYWORD_MOR, 
        KEYWORD_N, KEYWORD_NO, KEYWORD_NU, KEYWORD_NUM, KEYWORD_NUMB, KEYWORD_NUMBE, 
        KEYWORD_O,
        KEYWORD_P, KEYWORD_PL, KEYWORD_PLU, KEYWORD_PR, KEYWORD_PRO, KEYWORD_PROG, KEYWORD_PROGR, KEYWORD_PROGRA, KEYWORD_PROGRAM, KEYWORD_PROGRAMM, KEYWORD_PROGRAMMI, KEYWORD_PROGRAMMIN,
        KEYWORD_S, KEYWORD_SA,
        KEYWORD_T, KEYWORD_TE, KEYWORD_TEL, KEYWORD_TI, KEYWORD_TIM, KEYWORD_TIME
    }
}
