using System;
using System.IO;
using System.Collections.Generic;


namespace application
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=======================================");
            Console.WriteLine("Source Program: ");
            StreamReader sr = new StreamReader("toParse.txt");
            while(sr.Peek() != -1){
                Console.Write((char)sr.Read());
            }
            Console.WriteLine("");
            Console.WriteLine("=======================================");
            Console.WriteLine("SCANNER Output: ");
            sr.DiscardBufferedData();
            sr.BaseStream.Seek(0, System.IO.SeekOrigin.Begin); 
            Bookkeeper bk = new Bookkeeper();
            ErrorHandler er = new ErrorHandler();
            Token token;
            while(sr.Peek() != -1){
                application.Scanner theScanner = new application.Scanner(sr, bk, er);
                token = theScanner.detectToken();
            }
            Console.WriteLine("=======================================");
            Console.WriteLine("SYMTAB Content: ");
            bk.printSymTab();
            sr.Close();
        }
    }

    class Scanner{
        States state = States.INITIAL_STATE;
        StreamReader reader;
        Bookkeeper bookkeeper;
        ErrorHandler errorHandler;
        Token scannedToken;
        public Scanner(StreamReader passedReader, Bookkeeper passedBookkeeper, ErrorHandler passedErrorHandler){
            reader = passedReader;
            bookkeeper = passedBookkeeper;
            errorHandler = passedErrorHandler;
        }
        public void printToken(){
            Console.WriteLine(scannedToken.ToString());
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
                        scannedString += scannedSymbol;
                        if(scannedSymbol == '\"'){
                            state = States.STRING_INVALID;
                        }
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
                        else if(thisCharIsLowerCaseLetter(scannedSymbol)){
                            state = States.ID;
                        }
                        else if(thisCharIsADigit(scannedSymbol)){
                            //next character is a number,
                            state = States.CONSTANT_LENGTH_1;
                        }
                        else{  
                            state = States.GENERIC_ERROR;
                        }
                        break;
                    case States.STRING_INVALID:
                        scannedString += scannedSymbol;
                        if(scannedSymbol == '\"'){
                            state = States.STRING_VALID;
                        }
                        break;
                    case States.STRING_VALID:
                        scannedString += scannedSymbol;
                        state = States.GENERIC_ERROR;
                        break;
                    case States.CONSTANT_LENGTH_1:
                        scannedString += scannedSymbol;
                        if(thisCharIsADigit(scannedSymbol)){
                            //next character is a number,
                            state = States.CONSTANT_LENGTH_2;
                        }
                        else{
                            state = States.CONSTANT_ERROR;
                        }
                        break;
                    case States.CONSTANT_LENGTH_2:
                        scannedString += scannedSymbol;
                        if(thisCharIsADigit(scannedSymbol)){
                            //next character is a number,
                            state = States.CONSTANT_LENGTH_3;
                        }
                        else{
                            state = States.CONSTANT_ERROR;
                        }
                        break;
                    case States.CONSTANT_LENGTH_3:
                        scannedString += scannedSymbol;
                        if(thisCharIsADigit(scannedSymbol)){
                            //next character is a number,
                            state = States.CONSTANT_LENGTH_4;
                        }
                        else{
                            state = States.CONSTANT_ERROR;
                        }
                        break;
                    case States.CONSTANT_LENGTH_4:
                        scannedString += scannedSymbol;
                        if(thisCharIsADigit(scannedSymbol)){
                            //next character is a number,
                            state = States.CONSTANT_LENGTH_5;
                        }
                        else{
                            state = States.CONSTANT_ERROR;
                        }
                        break;
                    case States.CONSTANT_LENGTH_5:
                        scannedString += scannedSymbol;
                        if(thisCharIsADigit(scannedSymbol)){
                            //next character is a number,
                            state = States.CONSTANT_LENGTH_6;
                        }
                        else{
                            state = States.CONSTANT_ERROR;
                        }
                        break;
                    case States.CONSTANT_LENGTH_6:
                        scannedString += scannedSymbol;
                        if(thisCharIsADigit(scannedSymbol)){
                            //next character is a number,
                            state = States.CONSTANT;
                        }
                        else{
                            state = States.CONSTANT_ERROR;
                        }
                        break;
                    case States.CONSTANT:
                        scannedString += scannedSymbol;
                        if(thisCharIsADigit(scannedSymbol)){
                            //next character is a number,
                            state = States.CONSTANT;
                        }
                        else{
                            state = States.CONSTANT_ERROR;
                        }
                        break;
                    case States.KEYWORD:
                        scannedString += scannedSymbol;
                        if(thisCharIsLowerCaseLetter(scannedSymbol) || thisCharIsADigit(scannedSymbol)){
                            state = States.ID;
                        }
                        else{
                            state = States.INVALID_ID;
                        }
                        break;
                    case States.KEYWORD_A:
                        scannedString += scannedSymbol;
                        if(scannedSymbol == 'g'){
                            state = States.KEYWORD_AG;
                        }
                        else if(scannedSymbol == 'm'){
                            state = States.KEYWORD_AM;
                        }
                        else if(scannedSymbol == 'n'){
                            state = States.KEYWORD_AN;
                        }
                        else if(scannedSymbol == 's'){
                            state = States.KEYWORD;
                        }
                        else if(thisCharIsLowerCaseLetter(scannedSymbol) || thisCharIsADigit(scannedSymbol)){
                            state = States.ID;
                        }
                        else{
                            state = States.INVALID_ID;
                        }
                        break;
                    case States.KEYWORD_AG:
                        scannedString += scannedSymbol;
                        if(scannedSymbol == 'a'){
                            state = States.KEYWORD_AGA;
                        }
                        else if(thisCharIsLowerCaseLetter(scannedSymbol) || thisCharIsADigit(scannedSymbol)){
                            state = States.ID;
                        }
                        else{
                            state = States.INVALID_ID;
                        }
                        break;
                    case States.KEYWORD_AGA:
                        scannedString += scannedSymbol;
                        if(scannedSymbol == 'i'){
                            state = States.KEYWORD_AGAI;
                        }
                        else if(thisCharIsLowerCaseLetter(scannedSymbol) || thisCharIsADigit(scannedSymbol)){
                            state = States.ID;
                        }
                        else{
                            state = States.INVALID_ID;
                        }
                        break;
                    case States.KEYWORD_AGAI:
                        scannedString += scannedSymbol;
                        if(scannedSymbol == 'n'){
                            state = States.KEYWORD;
                        }
                        else if(thisCharIsLowerCaseLetter(scannedSymbol) || thisCharIsADigit(scannedSymbol)){
                            state = States.ID;
                        }
                        else{
                            state = States.INVALID_ID;
                        }
                        break;
                    case States.KEYWORD_AM:
                        scannedString += scannedSymbol;
                        if(scannedSymbol == 'e'){
                            state = States.KEYWORD_AME;
                        }
                        else if(thisCharIsLowerCaseLetter(scannedSymbol) || thisCharIsADigit(scannedSymbol)){
                            state = States.ID;
                        }
                        else{
                            state = States.INVALID_ID;
                        }
                        break;
                    case States.KEYWORD_AME:
                        scannedString += scannedSymbol;
                        if(scannedSymbol == 'r'){
                            state = States.KEYWORD_AMER;
                        }
                        else if(thisCharIsLowerCaseLetter(scannedSymbol) || thisCharIsADigit(scannedSymbol)){
                            state = States.ID;
                        }
                        else{
                            state = States.INVALID_ID;
                        }
                        break;
                    case States.KEYWORD_AMER:
                        scannedString += scannedSymbol;
                        if(scannedSymbol == 'i'){
                            state = States.KEYWORD_AMERI;
                        }
                        else if(thisCharIsLowerCaseLetter(scannedSymbol) || thisCharIsADigit(scannedSymbol)){
                            state = States.ID;
                        }
                        else{
                            state = States.INVALID_ID;
                        }
                        break;
                    case States.KEYWORD_AMERI:
                        scannedString += scannedSymbol;
                        if(scannedSymbol == 'c'){
                            state = States.KEYWORD_AMERIC;
                        }
                        else if(thisCharIsLowerCaseLetter(scannedSymbol) || thisCharIsADigit(scannedSymbol)){
                            state = States.ID;
                        }
                        else{
                            state = States.INVALID_ID;
                        }
                        break;
                    case States.KEYWORD_AMERIC:
                        scannedString += scannedSymbol;
                        if(scannedSymbol == 'a'){
                            state = States.KEYWORD;
                        }
                        else if(thisCharIsLowerCaseLetter(scannedSymbol) || thisCharIsADigit(scannedSymbol)){
                            state = States.ID;
                        }
                        else{
                            state = States.INVALID_ID;
                        }
                        break;
                    case States.KEYWORD_AN:
                        scannedString += scannedSymbol;
                        if(scannedSymbol == 'd'){
                            state = States.KEYWORD;
                        }
                        else if(thisCharIsLowerCaseLetter(scannedSymbol) || thisCharIsADigit(scannedSymbol)){
                            state = States.ID;
                        }
                        else{
                            state = States.INVALID_ID;
                        }
                        break;
                    case States.KEYWORD_B:
                        scannedString += scannedSymbol;
                        if(scannedSymbol == 'o'){
                            state = States.KEYWORD_BO;
                        }
                        else if(thisCharIsLowerCaseLetter(scannedSymbol) || thisCharIsADigit(scannedSymbol)){
                            state = States.ID;
                        }
                        else{
                            state = States.INVALID_ID;
                        }
                        break;
                    case States.KEYWORD_BO:
                        scannedString += scannedSymbol;
                        if(scannedSymbol == 'o'){
                            state = States.KEYWORD_BOO;
                        }
                        else if(thisCharIsLowerCaseLetter(scannedSymbol) || thisCharIsADigit(scannedSymbol)){
                            state = States.ID;
                        }
                        else{
                            state = States.INVALID_ID;
                        }
                        break;
                    case States.KEYWORD_BOO:
                        scannedString += scannedSymbol;
                        if(scannedSymbol == 'l'){
                            state = States.KEYWORD_BOOL;
                        }
                        else if(thisCharIsLowerCaseLetter(scannedSymbol) || thisCharIsADigit(scannedSymbol)){
                            state = States.ID;
                        }
                        else{
                            state = States.INVALID_ID;
                        }
                        break;
                    case States.KEYWORD_BOOL:
                        scannedString += scannedSymbol;
                        if(scannedSymbol == 'e'){
                            state = States.KEYWORD_BOOLE;
                        }
                        else if(thisCharIsLowerCaseLetter(scannedSymbol) || thisCharIsADigit(scannedSymbol)){
                            state = States.ID;
                        }
                        else{
                            state = States.INVALID_ID;
                        }
                        break;
                    case States.KEYWORD_BOOLE:
                        scannedString += scannedSymbol;
                        if(scannedSymbol == 'a'){
                            state = States.KEYWORD_BOOLEA;
                        }
                        else if(thisCharIsLowerCaseLetter(scannedSymbol) || thisCharIsADigit(scannedSymbol)){
                            state = States.ID;
                        }
                        else{
                            state = States.INVALID_ID;
                        }
                        break;
                    case States.KEYWORD_BOOLEA:
                        scannedString += scannedSymbol;
                        if(scannedSymbol == 'n'){
                            state = States.KEYWORD;
                        }
                        else if(thisCharIsLowerCaseLetter(scannedSymbol) || thisCharIsADigit(scannedSymbol)){
                            state = States.ID;
                        }
                        else{
                            state = States.INVALID_ID;
                        }
                        break;
                    case States.KEYWORD_E:
                        scannedString += scannedSymbol;
                        if(scannedSymbol == 'l'){
                            state = States.KEYWORD_EL;
                        }
                        else if(thisCharIsLowerCaseLetter(scannedSymbol) || thisCharIsADigit(scannedSymbol)){
                            state = States.ID;
                        }
                        else{
                            state = States.INVALID_ID;
                        }
                        break;
                    case States.KEYWORD_EL:
                        scannedString += scannedSymbol;
                        if(scannedSymbol == 's'){
                            state = States.KEYWORD_ELS;
                        }
                        else if(thisCharIsLowerCaseLetter(scannedSymbol) || thisCharIsADigit(scannedSymbol)){
                            state = States.ID;
                        }
                        else{
                            state = States.INVALID_ID;
                        }
                        break;
                    case States.KEYWORD_ELS:
                        scannedString += scannedSymbol;
                        if(scannedSymbol == 'e'){
                            state = States.KEYWORD;
                        }
                        else if(thisCharIsLowerCaseLetter(scannedSymbol) || thisCharIsADigit(scannedSymbol)){
                            state = States.ID;
                        }
                        else{
                            state = States.INVALID_ID;
                        }
                        break;
                    case States.KEYWORD_F:
                        scannedString += scannedSymbol;
                        if(scannedSymbol == 'a'){
                            state = States.KEYWORD_FA;
                        }
                        else if(thisCharIsLowerCaseLetter(scannedSymbol) || thisCharIsADigit(scannedSymbol)){
                            state = States.ID;
                        }
                        else{
                            state = States.INVALID_ID;
                        }
                        break;
                    case States.KEYWORD_FA:
                        scannedString += scannedSymbol;
                        if(scannedSymbol == 'c'){
                            state = States.KEYWORD_FAC;
                        }
                        else if(thisCharIsLowerCaseLetter(scannedSymbol) || thisCharIsADigit(scannedSymbol)){
                            state = States.ID;
                        }
                        else{
                            state = States.INVALID_ID;
                        }
                        break;
                    case States.KEYWORD_FAC:
                        scannedString += scannedSymbol;
                        if(scannedSymbol == 't'){
                            state = States.KEYWORD;
                        }
                        else if(thisCharIsLowerCaseLetter(scannedSymbol) || thisCharIsADigit(scannedSymbol)){
                            state = States.ID;
                        }
                        else{
                            state = States.INVALID_ID;
                        }
                        break;
                    case States.KEYWORD_G:
                        scannedString += scannedSymbol;
                        if(scannedSymbol == 'r'){
                            state = States.KEYWORD_GR;
                        }
                        else if(thisCharIsLowerCaseLetter(scannedSymbol) || thisCharIsADigit(scannedSymbol)){
                            state = States.ID;
                        }
                        else{
                            state = States.INVALID_ID;
                        }
                        break;
                    case States.KEYWORD_GR:
                        scannedString += scannedSymbol;
                        if(scannedSymbol == 'e'){
                            state = States.KEYWORD_GRE;
                        }
                        else if(thisCharIsLowerCaseLetter(scannedSymbol) || thisCharIsADigit(scannedSymbol)){
                            state = States.ID;
                        }
                        else{
                            state = States.INVALID_ID;
                        }
                        break;
                    case States.KEYWORD_GRE:
                        scannedString += scannedSymbol;
                        if(scannedSymbol == 'a'){
                            state = States.KEYWORD_GREA;
                        }
                        else if(thisCharIsLowerCaseLetter(scannedSymbol) || thisCharIsADigit(scannedSymbol)){
                            state = States.ID;
                        }
                        else{
                            state = States.INVALID_ID;
                        }
                        break;
                    case States.KEYWORD_GREA:
                        scannedString += scannedSymbol;
                        if(scannedSymbol == 't'){
                            state = States.KEYWORD;
                        }
                        else if(thisCharIsLowerCaseLetter(scannedSymbol) || thisCharIsADigit(scannedSymbol)){
                            state = States.ID;
                        }
                        else{
                            state = States.INVALID_ID;
                        }
                        break;
                    case States.KEYWORD_I:
                        scannedString += scannedSymbol;
                        if(scannedSymbol == 's'){
                            state = States.KEYWORD;
                        }
                        else if(scannedSymbol == 'f'){
                            state = States.KEYWORD;
                        }
                        else if(thisCharIsLowerCaseLetter(scannedSymbol) || thisCharIsADigit(scannedSymbol)){
                            state = States.ID;
                        }
                        else{
                            state = States.INVALID_ID;
                        }
                        break;
                    case States.KEYWORD_L:
                        scannedString += scannedSymbol;
                        if(scannedSymbol == 'e'){
                            state = States.KEYWORD_LE;
                        }
                        else if(scannedSymbol == 'i'){
                            state = States.KEYWORD_LI;
                        }
                        else if(scannedSymbol == 'o'){
                            state = States.KEYWORD_LO;
                        }
                        else if(thisCharIsLowerCaseLetter(scannedSymbol) || thisCharIsADigit(scannedSymbol)){
                            state = States.ID;
                        }
                        else{
                            state = States.INVALID_ID;
                        }
                        break;
                    case States.KEYWORD_LE:
                        scannedString += scannedSymbol;
                        if(scannedSymbol == 's'){
                            state = States.KEYWORD_LES;
                        }
                        else if(thisCharIsLowerCaseLetter(scannedSymbol) || thisCharIsADigit(scannedSymbol)){
                            state = States.ID;
                        }
                        else{
                            state = States.INVALID_ID;
                        }
                        break;
                    case States.KEYWORD_LES:
                        scannedString += scannedSymbol;
                        if(scannedSymbol == 's'){
                            state = States.KEYWORD;
                        }
                        else if(thisCharIsLowerCaseLetter(scannedSymbol) || thisCharIsADigit(scannedSymbol)){
                            state = States.ID;
                        }
                        else{
                            state = States.INVALID_ID;
                        }
                        break;
                    case States.KEYWORD_LI:
                        scannedString += scannedSymbol;
                        if(scannedSymbol == 'e'){
                            state = States.KEYWORD;
                        }
                        else if(thisCharIsLowerCaseLetter(scannedSymbol) || thisCharIsADigit(scannedSymbol)){
                            state = States.ID;
                        }
                        else{
                            state = States.INVALID_ID;
                        }
                        break;
                    case States.KEYWORD_LO:
                        scannedString += scannedSymbol;
                        if(scannedSymbol == 'n'){
                            state = States.KEYWORD_LON;
                        }
                        else if(thisCharIsLowerCaseLetter(scannedSymbol) || thisCharIsADigit(scannedSymbol)){
                            state = States.ID;
                        }
                        else{
                            state = States.INVALID_ID;
                        }
                        break;
                    case States.KEYWORD_LON:
                        scannedString += scannedSymbol;
                        if(scannedSymbol == 'g'){
                            state = States.KEYWORD;
                        }
                        else if(thisCharIsLowerCaseLetter(scannedSymbol) || thisCharIsADigit(scannedSymbol)){
                            state = States.ID;
                        }
                        else{
                            state = States.INVALID_ID;
                        }
                        break;
                    case States.KEYWORD_M:
                        scannedString += scannedSymbol;
                        if(scannedSymbol == 'a'){
                            state = States.KEYWORD_MA;
                        }
                        else if(scannedSymbol == 'o'){
                            state = States.KEYWORD_MO;
                        }
                        else if(thisCharIsLowerCaseLetter(scannedSymbol) || thisCharIsADigit(scannedSymbol)){
                            state = States.ID;
                        }
                        else{
                            state = States.INVALID_ID;
                        }
                        break;
                    case States.KEYWORD_MA:
                        scannedString += scannedSymbol;
                        if(scannedSymbol == 'k'){
                            state = States.KEYWORD_MAK;
                        }
                        else if(thisCharIsLowerCaseLetter(scannedSymbol) || thisCharIsADigit(scannedSymbol)){
                            state = States.ID;
                        }
                        else{
                            state = States.INVALID_ID;
                        }
                        break;
                    case States.KEYWORD_MAK:
                        scannedString += scannedSymbol;
                        if(scannedSymbol == 'e'){
                            state = States.KEYWORD;
                        }
                        else if(thisCharIsLowerCaseLetter(scannedSymbol) || thisCharIsADigit(scannedSymbol)){
                            state = States.ID;
                        }
                        else{
                            state = States.INVALID_ID;
                        }
                        break;
                    case States.KEYWORD_MO:
                        scannedString += scannedSymbol;
                        if(scannedSymbol == 'r'){
                            state = States.KEYWORD_MOR;
                        }
                        else if(thisCharIsLowerCaseLetter(scannedSymbol) || thisCharIsADigit(scannedSymbol)){
                            state = States.ID;
                        }
                        else{
                            state = States.INVALID_ID;
                        }
                        break;
                    case States.KEYWORD_MOR:
                        scannedString += scannedSymbol;
                        if(scannedSymbol == 'e'){
                            state = States.KEYWORD;
                        }
                        else if(thisCharIsLowerCaseLetter(scannedSymbol) || thisCharIsADigit(scannedSymbol)){
                            state = States.ID;
                        }
                        else{
                            state = States.INVALID_ID;
                        }
                        break;
                    case States.KEYWORD_N:
                        scannedString += scannedSymbol;
                        if(scannedSymbol == 'o'){
                            state = States.KEYWORD_NO;
                        }
                        else if(scannedSymbol == 'u'){
                            state = States.KEYWORD_NU;
                        }
                        else if(thisCharIsLowerCaseLetter(scannedSymbol) || thisCharIsADigit(scannedSymbol)){
                            state = States.ID;
                        }
                        else{
                            state = States.INVALID_ID;
                        }
                        break;
                    case States.KEYWORD_NO:
                        scannedString += scannedSymbol;
                        if(scannedSymbol == 't'){
                            state = States.KEYWORD;
                        }
                        else if(thisCharIsLowerCaseLetter(scannedSymbol) || thisCharIsADigit(scannedSymbol)){
                            state = States.ID;
                        }
                        else{
                            state = States.INVALID_ID;
                        }
                        break;
                    case States.KEYWORD_NU:
                        scannedString += scannedSymbol;
                        if(scannedSymbol == 'm'){
                            state = States.KEYWORD_NUM;
                        }
                        else if(thisCharIsLowerCaseLetter(scannedSymbol) || thisCharIsADigit(scannedSymbol)){
                            state = States.ID;
                        }
                        else{
                            state = States.INVALID_ID;
                        }
                        break;
                    case States.KEYWORD_NUM:
                        scannedString += scannedSymbol;
                        if(scannedSymbol == 'b'){
                            state = States.KEYWORD_NUMB;
                        }
                        else if(thisCharIsLowerCaseLetter(scannedSymbol) || thisCharIsADigit(scannedSymbol)){
                            state = States.ID;
                        }
                        else{
                            state = States.INVALID_ID;
                        }
                        break;
                    case States.KEYWORD_NUMB:
                        scannedString += scannedSymbol;
                        if(scannedSymbol == 'e'){
                            state = States.KEYWORD_NUMBE;
                        }
                        else if(thisCharIsLowerCaseLetter(scannedSymbol) || thisCharIsADigit(scannedSymbol)){
                            state = States.ID;
                        }
                        else{
                            state = States.INVALID_ID;
                        }
                        break;
                    case States.KEYWORD_NUMBE:
                        scannedString += scannedSymbol;
                        if(scannedSymbol == 'r'){
                            state = States.KEYWORD;
                        }
                        else if(thisCharIsLowerCaseLetter(scannedSymbol) || thisCharIsADigit(scannedSymbol)){
                            state = States.ID;
                        }
                        else{
                            state = States.INVALID_ID;
                        }
                        break;
                    case States.KEYWORD_O:
                        scannedString += scannedSymbol;
                        if(scannedSymbol == 'r'){
                            state = States.KEYWORD;
                        }
                        else if(thisCharIsLowerCaseLetter(scannedSymbol) || thisCharIsADigit(scannedSymbol)){
                            state = States.ID;
                        }
                        else{
                            state = States.INVALID_ID;
                        }
                        break;
                    case States.KEYWORD_P:
                        scannedString += scannedSymbol;
                        if(scannedSymbol == 'l'){
                            state = States.KEYWORD_PL;
                        }
                        else if(scannedSymbol == 'r'){
                            state = States.KEYWORD_PR;
                        }
                        else if(thisCharIsLowerCaseLetter(scannedSymbol) || thisCharIsADigit(scannedSymbol)){
                            state = States.ID;
                        }
                        else{
                            state = States.INVALID_ID;
                        }
                        break;
                    case States.KEYWORD_PL:
                        scannedString += scannedSymbol;
                        if(scannedSymbol == 'u'){
                            state = States.KEYWORD_PLU;
                        }
                        else if(thisCharIsLowerCaseLetter(scannedSymbol) || thisCharIsADigit(scannedSymbol)){
                            state = States.ID;
                        }
                        else{
                            state = States.INVALID_ID;
                        }
                        break;
                    case States.KEYWORD_PLU:
                        scannedString += scannedSymbol;
                        if(scannedSymbol == 's'){
                            state = States.KEYWORD;
                        }
                        else if(thisCharIsLowerCaseLetter(scannedSymbol) || thisCharIsADigit(scannedSymbol)){
                            state = States.ID;
                        }
                        else{
                            state = States.INVALID_ID;
                        }
                        break;
                    case States.KEYWORD_PR:
                        scannedString += scannedSymbol;
                        if(scannedSymbol == 'o'){
                            state = States.KEYWORD_PRO;
                        }
                        else if(thisCharIsLowerCaseLetter(scannedSymbol) || thisCharIsADigit(scannedSymbol)){
                            state = States.ID;
                        }
                        else{
                            state = States.INVALID_ID;
                        }
                        break;
                    case States.KEYWORD_PRO:
                        scannedString += scannedSymbol;
                        if(scannedSymbol == 'g'){
                            state = States.KEYWORD_PROG;
                        }
                        else if(thisCharIsLowerCaseLetter(scannedSymbol) || thisCharIsADigit(scannedSymbol)){
                            state = States.ID;
                        }
                        else{
                            state = States.INVALID_ID;
                        }
                        break;
                    case States.KEYWORD_PROG:
                        scannedString += scannedSymbol;
                        if(scannedSymbol == 'r'){
                            state = States.KEYWORD_PROGR;
                        }
                        else if(thisCharIsLowerCaseLetter(scannedSymbol) || thisCharIsADigit(scannedSymbol)){
                            state = States.ID;
                        }
                        else{
                            state = States.INVALID_ID;
                        }
                        break;
                    case States.KEYWORD_PROGR:
                        scannedString += scannedSymbol;
                        if(scannedSymbol == 'a'){
                            state = States.KEYWORD_PROGRA;
                        }
                        else if(thisCharIsLowerCaseLetter(scannedSymbol) || thisCharIsADigit(scannedSymbol)){
                            state = States.ID;
                        }
                        else{
                            state = States.INVALID_ID;
                        }
                        break;
                    case States.KEYWORD_PROGRA:
                        scannedString += scannedSymbol;
                        if(scannedSymbol == 'm'){
                            state = States.KEYWORD_PROGRAM;
                        }
                        else if(thisCharIsLowerCaseLetter(scannedSymbol) || thisCharIsADigit(scannedSymbol)){
                            state = States.ID;
                        }
                        else{
                            state = States.INVALID_ID;
                        }
                        break;
                    case States.KEYWORD_PROGRAM:
                        scannedString += scannedSymbol;
                        if(scannedSymbol == 'm'){
                            state = States.KEYWORD_PROGRAMM;
                        }
                        else if(thisCharIsLowerCaseLetter(scannedSymbol) || thisCharIsADigit(scannedSymbol)){
                            state = States.ID;
                        }
                        else{
                            state = States.INVALID_ID;
                        }
                        break;
                    case States.KEYWORD_PROGRAMM:
                        scannedString += scannedSymbol;
                        if(scannedSymbol == 'i'){
                            state = States.KEYWORD_PROGRAMMI;
                        }
                        else if(thisCharIsLowerCaseLetter(scannedSymbol) || thisCharIsADigit(scannedSymbol)){
                            state = States.ID;
                        }
                        else{
                            state = States.INVALID_ID;
                        }
                        break;
                    case States.KEYWORD_PROGRAMMI:
                        scannedString += scannedSymbol;
                        if(scannedSymbol == 'n'){
                            state = States.KEYWORD_PROGRAMMIN;
                        }
                        else if(thisCharIsLowerCaseLetter(scannedSymbol) || thisCharIsADigit(scannedSymbol)){
                            state = States.ID;
                        }
                        else{
                            state = States.INVALID_ID;
                        }
                        break;
                    case States.KEYWORD_PROGRAMMIN:
                        scannedString += scannedSymbol;
                        if(scannedSymbol == 'g'){
                            state = States.KEYWORD;
                        }
                        else if(thisCharIsLowerCaseLetter(scannedSymbol) || thisCharIsADigit(scannedSymbol)){
                            state = States.ID;
                        }
                        else{
                            state = States.INVALID_ID;
                        }
                        break;
                    case States.KEYWORD_S:
                        scannedString += scannedSymbol;
                        if(scannedSymbol == 'a'){
                            state = States.KEYWORD_SA;
                        }
                        else if(thisCharIsLowerCaseLetter(scannedSymbol) || thisCharIsADigit(scannedSymbol)){
                            state = States.ID;
                        }
                        else{
                            state = States.INVALID_ID;
                        }
                        break;
                    case States.KEYWORD_SA:
                        scannedString += scannedSymbol;
                        if(scannedSymbol == 'y'){
                            state = States.KEYWORD;
                        }
                        else if(thisCharIsLowerCaseLetter(scannedSymbol) || thisCharIsADigit(scannedSymbol)){
                            state = States.ID;
                        }
                        else{
                            state = States.INVALID_ID;
                        }
                        break;
                    case States.KEYWORD_T:
                        scannedString += scannedSymbol;
                        if(scannedSymbol == 'e'){
                            state = States.KEYWORD_TE;
                        }
                        else if(scannedSymbol == 'i'){
                            state = States.KEYWORD_TI;
                        }
                        else if(thisCharIsLowerCaseLetter(scannedSymbol) || thisCharIsADigit(scannedSymbol)){
                            state = States.ID;
                        }
                        else{
                            state = States.INVALID_ID;
                        }
                        break;
                    case States.KEYWORD_TE:
                        scannedString += scannedSymbol;
                        if(scannedSymbol == 'l'){
                            state = States.KEYWORD_TEL;
                        }
                        else if(thisCharIsLowerCaseLetter(scannedSymbol) || thisCharIsADigit(scannedSymbol)){
                            state = States.ID;
                        }
                        else{
                            state = States.INVALID_ID;
                        }
                        break;
                    case States.KEYWORD_TEL:
                        scannedString += scannedSymbol;
                        if(scannedSymbol == 'l'){
                            state = States.KEYWORD;
                        }
                        else if(thisCharIsLowerCaseLetter(scannedSymbol) || thisCharIsADigit(scannedSymbol)){
                            state = States.ID;
                        }
                        else{
                            state = States.INVALID_ID;
                        }
                        break;
                    case States.KEYWORD_TI:
                        scannedString += scannedSymbol;
                        if(scannedSymbol == 'm'){
                            state = States.KEYWORD_TIM;
                        }
                        else if(thisCharIsLowerCaseLetter(scannedSymbol) || thisCharIsADigit(scannedSymbol)){
                            state = States.ID;
                        }
                        else{
                            state = States.INVALID_ID;
                        }
                        break;
                    case States.KEYWORD_TIM:
                        scannedString += scannedSymbol;
                        if(scannedSymbol == 'e'){
                            state = States.KEYWORD_TIME;
                        }
                        else if(thisCharIsLowerCaseLetter(scannedSymbol) || thisCharIsADigit(scannedSymbol)){
                            state = States.ID;
                        }
                        else{
                            state = States.INVALID_ID;
                        }
                        break;
                    case States.KEYWORD_TIME:
                        scannedString += scannedSymbol;
                        if(scannedSymbol == 's'){
                            state = States.KEYWORD;
                        }
                        else if(thisCharIsLowerCaseLetter(scannedSymbol) || thisCharIsADigit(scannedSymbol)){
                            state = States.ID;
                        }
                        else{
                            state = States.INVALID_ID;
                        }
                        break;
                    case States.ID:
                        scannedString += scannedSymbol;
                        if(thisCharIsLowerCaseLetter(scannedSymbol) || thisCharIsADigit(scannedSymbol)){
                            state = States.ID;
                        }
                        else{
                            state = States.INVALID_ID;
                        }
                        break;
                    case States.INVALID_ID:
                        scannedString += scannedSymbol;
                        state = States.INVALID_ID;
                        break;
                    case States.GENERIC_ERROR:
                        scannedString += scannedSymbol;
                        break;
                    default:
                        scannedString += scannedSymbol;
                        state = States.GENERIC_ERROR;
                        break;
                }
            }
            if(isInAcceptingState(state)){
                if(state == States.CONSTANT){
                    if(scannedString == "1000000"){
                        state = States.CONSTANT_ERROR;
                        scannedToken = new Token(state.ToString(), Type.ERROR);
                        errorHandler.printErrorForThisState(state);
                    }
                    else{
                        scannedToken = new Token(scannedString, Type.CONSTANT); 
                        bookkeeper.addToken(scannedToken);
                        printToken();
                    }
                }
                else if(state == States.STRING_VALID){
                    scannedToken = new Token(scannedString, Type.STRING); 
                    bookkeeper.addToken(scannedToken);
                    printToken();
                }
                else if(state == States.ID){
                    scannedToken = new Token(scannedString, Type.ID); 
                    bookkeeper.addToken(scannedToken);
                    printToken();
                }
                else if(state == States.SPECIAL_CHARACTER){
                    scannedToken = new Token(scannedString, Type.SPECIAL_SYMBOL); 
                    printToken();
                }
                else if(state.ToString().Substring(0, 7) == "KEYWORD"){
                    if(state.ToString() == "KEYWORD"){
                        scannedToken = new Token(scannedString, Type.KEYWORD);
                        printToken();
                    }
                    else{
                        scannedToken = new Token(scannedString, Type.ID);
                        printToken();
                    }
                }
            }
            else{
                scannedToken = new Token(state.ToString(), Type.ERROR);
                errorHandler.printErrorForThisState(state);
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
            set{
                tokenType = value;
            }
        }
        override
        public string ToString(){
            if(lexeme.Length < 7){
                return "Lexeme: " + lexeme + " \t\t Type: " + tokenType.ToString();
            }
            else{
                return "Lexeme: " + lexeme + " \t Type: " + tokenType.ToString();
            }
        }
    }

    class ErrorHandler{
        public ErrorHandler(){
            
        }
        KeyValuePair<string, object>[] errorOutputs = {
            new KeyValuePair<string,object>( "GENERIC_ERROR", "Any other erro: Trump doesn't want to hearit." ),
            new KeyValuePair<string,object>( "CONSTANT_ERROR", "[const] error: I'm really rich, part of the beauty of meis I'm very rich."),
            new KeyValuePair<string,object>( "INVALID_ID", "[id] error: This is a country where we speak English.")
        };
        public void printErrorForThisState(States stateToPrintErrorFor){
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
            Console.WriteLine(errorOutputs[indexOfError].Value);
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
            SymTab.Capacity = 100;
        }

        public void printSymTab(){
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
        KEYWORD_T, KEYWORD_TE, KEYWORD_TEL, KEYWORD_TI, KEYWORD_TIM, KEYWORD_TIME,
        SOMETHING_ELSE
    }
}
