using System;
using System.IO;
using System.Collections.Generic;

namespace CompilerHelpers{
    //Token class for storing lexemes and types as an object
    class Token{
        string lexeme;
        LexemeType tokenType;
        //constructor
        public Token(){
            lexeme = "error";
            tokenType = LexemeType.ERROR;
        }
        public Token(string passedLexeme, LexemeType passedType){
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
        public LexemeType TokenType{
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
                    break;
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
    enum LexemeType {ID, CONSTANT, STRING, KEYWORD, SPECIAL_SYMBOL, ERROR};
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