using System;
using System.IO;
using System.Collections.Generic;
using CompilerHelpers;
using Scanners;

namespace Parsers{
    class Parser{
        Stack<int> parserStack = new Stack<int>();
        StreamReader reader;
        Bookkeeper bk = new Bookkeeper();
        ErrorHandler er = new ErrorHandler();
        Dictionary<String, int> dictionary = new Dictionary<String, int>();
        
        public Parser(){
            parserStack.Push(0);
            this.addDictionaryDefinitions();
        }
        public Parser(StreamReader passedReader){
            reader = passedReader;
            parserStack.Push(0);
            this.addDictionaryDefinitions();
        }
        public void parseProgram(){
            Token lookahead;
            while(reader.Peek() != -1){
                Scanner scanner = new Scanner(reader, bk, er);
                lookahead = scanner.detectToken();
                Console.WriteLine(getIntegerCodeofToken(lookahead));
            }
        }
        public int getIntegerCodeofToken(Token passedToken){
            int returnInt; 
            if(passedToken.TokenType == LexemeType.ID){
                returnInt=1;
            }
            else if(passedToken.TokenType == LexemeType.CONSTANT){
                returnInt=2;
            }
            else if(passedToken.TokenType == LexemeType.STRING){
                returnInt=3;
            }
            else if(passedToken.TokenType == LexemeType.ERROR){
                returnInt=-1;
            }
            else{
                returnInt = dictionary[passedToken.Lexeme];
            }
            return returnInt;
        }
        private void addDictionaryDefinitions(){
            dictionary.Add( "ID", 1);
            dictionary.Add( "CONSTANT", 2);
            dictionary.Add( "STRING",3);
            dictionary.Add( "make", 4 );
            dictionary.Add( "programming", 5);
            dictionary.Add( "great",6);
            dictionary.Add( "again", 7 );
            dictionary.Add( "america", 8);
            dictionary.Add( "is",9);
            dictionary.Add( "else", 10 );
            dictionary.Add( "number", 11);
            dictionary.Add( "boolean",12);
            dictionary.Add( "if", 13 );
            dictionary.Add( "as", 14);
            dictionary.Add( "long",15);
            dictionary.Add( "tell", 16 );
            dictionary.Add( "say", 17);
            dictionary.Add( "fact",18);
            dictionary.Add( "lie", 19);
            dictionary.Add( "not", 20);
            dictionary.Add( "and",21);
            dictionary.Add( "or", 22);
            dictionary.Add( "less", 23);
            dictionary.Add( "more",24);
            dictionary.Add( "plus", 25);
            dictionary.Add( "times", 26);
            dictionary.Add( ",",27);
            dictionary.Add( ";",28);
            dictionary.Add( ":",29);
            dictionary.Add( "!",30);
            dictionary.Add( "?",31);
            dictionary.Add( "(",32);
            dictionary.Add( ")",33);
        }
    }
}