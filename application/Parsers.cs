using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using CompilerHelpers;
using Scanners;

namespace Parsers{
    class Parser{
        Stack<int> parserStack = new Stack<int>();
        StreamReader reader;
        Bookkeeper bk = new Bookkeeper();
        ErrorHandler er = new ErrorHandler();
        Dictionary<int, String> dictionary = new Dictionary<int, String>();
        int programStep = 0;
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
            int lookahead = -1;
            
            Boolean scanNewLookahead = true;
            //Boolean errorDetected = false;
            while(reader.Peek() != -1){
                Console.WriteLine(scanNewLookahead);
                switch(parserStack.Peek()){
                    case 0:
                        printAction(parserStack.Peek(), -1, "Push(<Trump>)");
                        parserStack.Push(34);
                        break;
                    case 1:
                        if(scanNewLookahead){lookahead = getLookahead();}
                        if(lookahead == 1){
                            printAction(parserStack.Peek(), lookahead, "matching, pop([ID])");
                            parserStack.Pop();
                            scanNewLookahead = true;
                        }
                        break;
                    case 2:
                        if(scanNewLookahead){lookahead = getLookahead();}
                        if(lookahead == 2){
                            printAction(parserStack.Peek(), lookahead, "matching, pop([CONSTANT])");
                            parserStack.Pop();
                            scanNewLookahead = true;
                        }
                        break;
                    case 3:
                        if(scanNewLookahead){lookahead = getLookahead();}
                        if(lookahead == 3){
                            printAction(parserStack.Peek(), lookahead, "matching, pop([STRING])");
                            parserStack.Pop();
                            scanNewLookahead = true;
                        }
                        break;
                    case 4:
                        if(scanNewLookahead){lookahead = getLookahead();}
                        if(lookahead == 4){
                            printAction(parserStack.Peek(), lookahead, "matching, pop(make))");
                            parserStack.Pop();
                            scanNewLookahead = true;
                        }
                        break;
                    case 5:
                        if(scanNewLookahead){lookahead = getLookahead();}
                        if(lookahead == 5){
                            printAction(parserStack.Peek(), lookahead, "matching, pop(america)");
                            parserStack.Pop();
                            scanNewLookahead = true;
                        }
                        break;
                    case 6:
                        if(scanNewLookahead){lookahead = getLookahead();}
                        if(lookahead == 6){
                            printAction(parserStack.Peek(), lookahead, "matching, pop(great)");
                            parserStack.Pop();
                            scanNewLookahead = true;
                        }
                        break;
                    case 7:
                        if(scanNewLookahead){lookahead = getLookahead();}
                        if(lookahead == 7){
                            printAction(parserStack.Peek(), lookahead, "matching, pop(again)");
                            parserStack.Pop();
                            scanNewLookahead = true;
                        }
                        break;
                    case 8:
                        if(scanNewLookahead){lookahead = getLookahead();}
                        if(lookahead == 8){
                            printAction(parserStack.Peek(), lookahead, "matching, pop(america)");
                            parserStack.Pop();
                            scanNewLookahead = true;
                        }
                        break;
                    case 9:
                        if(scanNewLookahead){lookahead = getLookahead();}
                        if(lookahead == 9){
                            printAction(parserStack.Peek(), lookahead, "matching, pop(is)");
                            parserStack.Pop();
                            scanNewLookahead = true;
                        }
                        break;
                    case 10:
                        if(scanNewLookahead){lookahead = getLookahead();}
                        if(lookahead == 10){
                            printAction(parserStack.Peek(), lookahead, "matching, pop(else)");
                            parserStack.Pop();
                            scanNewLookahead = true;
                        }
                        break;
                    case 11:
                        if(scanNewLookahead){lookahead = getLookahead();}
                        if(lookahead == 11){
                            printAction(parserStack.Peek(), lookahead, "matching, pop(number)");
                            parserStack.Pop();
                            scanNewLookahead = true;
                        }
                        break;
                    case 12:
                        if(scanNewLookahead){lookahead = getLookahead();}
                        if(lookahead == 12){
                            printAction(parserStack.Peek(), lookahead, "matching, pop(boolean)");
                            parserStack.Pop();
                            scanNewLookahead = true;
                        }
                        break;
                    case 13:
                        if(scanNewLookahead){lookahead = getLookahead();}
                        if(lookahead == 13){
                            printAction(parserStack.Peek(), lookahead, "matching, pop(if)");
                            parserStack.Pop();
                            scanNewLookahead = true;
                        }
                        break;
                    case 14:
                        if(scanNewLookahead){lookahead = getLookahead();}
                        if(lookahead == 14){
                            printAction(parserStack.Peek(), lookahead, "matching, pop(as)");
                            parserStack.Pop();
                            scanNewLookahead = true;
                        }
                        break;
                    case 15:
                        if(scanNewLookahead){lookahead = getLookahead();}
                        if(lookahead == 15){
                            printAction(parserStack.Peek(), lookahead, "matching, pop(long)");
                            parserStack.Pop();
                            scanNewLookahead = true;
                        }
                        break;
                    case 16:
                        if(scanNewLookahead){lookahead = getLookahead();}
                        if(lookahead == 16){
                            printAction(parserStack.Peek(), lookahead, "matching, pop(tell)");
                            parserStack.Pop();
                            scanNewLookahead = true;
                        }
                        break;
                    case 17:
                        if(scanNewLookahead){lookahead = getLookahead();}
                        if(lookahead == 17){
                            printAction(parserStack.Peek(), lookahead, "matching, pop(say)");
                            parserStack.Pop();
                            scanNewLookahead = true;
                        }
                        break;
                    case 18:
                        if(scanNewLookahead){lookahead = getLookahead();}
                        if(lookahead == 18){
                            printAction(parserStack.Peek(), lookahead, "matching, pop(fact)");
                            parserStack.Pop();
                            scanNewLookahead = true;
                        }
                        break;
                    case 19:
                        if(scanNewLookahead){lookahead = getLookahead();}
                        if(lookahead == 19){
                            printAction(parserStack.Peek(), lookahead, "matching, pop(lie)");
                            parserStack.Pop();
                            scanNewLookahead = true;
                        }
                        break;
                    case 20:
                        if(scanNewLookahead){lookahead = getLookahead();}
                        if(lookahead == 20){
                            printAction(parserStack.Peek(), lookahead, "matching, pop(not)");
                            parserStack.Pop();
                            scanNewLookahead = true;
                        }
                        break;
                    case 21:
                        if(scanNewLookahead){lookahead = getLookahead();}
                        if(lookahead == 21){
                            printAction(parserStack.Peek(), lookahead, "matching, pop(and)");
                            parserStack.Pop();
                            scanNewLookahead = true;
                        }
                        break;
                    case 22:
                        if(scanNewLookahead){lookahead = getLookahead();}
                        if(lookahead == 22){
                            printAction(parserStack.Peek(), lookahead, "matching, pop(or)");
                            parserStack.Pop();
                            scanNewLookahead = true;
                        }
                        break;
                    case 23:
                        if(scanNewLookahead){lookahead = getLookahead();}
                        if(lookahead == 23){
                            printAction(parserStack.Peek(), lookahead, "matching, pop(less)");
                            parserStack.Pop();
                            scanNewLookahead = true;
                        }
                        break;
                    case 24:
                        if(scanNewLookahead){lookahead = getLookahead();}
                        if(lookahead == 23){
                            printAction(parserStack.Peek(), lookahead, "matching, pop(more)");
                            parserStack.Pop();
                            scanNewLookahead = true;
                        }
                        break;
                    case 25:
                        if(scanNewLookahead){lookahead = getLookahead();}
                        if(lookahead == 25){
                            printAction(parserStack.Peek(), lookahead, "matching, pop(plus)");
                            parserStack.Pop();
                            scanNewLookahead = true;
                        }
                        break;
                    case 26:
                        if(scanNewLookahead){lookahead = getLookahead();}
                        if(lookahead == 23){
                            printAction(parserStack.Peek(), lookahead, "matching, pop(times)");
                            parserStack.Pop();
                            scanNewLookahead = true;
                        }
                        break;
                    case 27:
                        if(scanNewLookahead){lookahead = getLookahead();}
                        if(lookahead == 27){
                            printAction(parserStack.Peek(), lookahead, "matching, pop( , )");
                            parserStack.Pop();
                            scanNewLookahead = true;
                        }
                        break;
                    case 28:
                        if(scanNewLookahead){lookahead = getLookahead();}
                        if(lookahead == 28){
                            printAction(parserStack.Peek(), lookahead, "matching, pop( ; )");
                            parserStack.Pop();
                            scanNewLookahead = true;
                        }
                        break;
                    case 29:
                        if(scanNewLookahead){lookahead = getLookahead();}
                        if(lookahead == 29){
                            printAction(parserStack.Peek(), lookahead, "matching, pop( : )");
                            parserStack.Pop();
                            scanNewLookahead = true;
                        }
                        break;
                    case 30:
                        if(scanNewLookahead){lookahead = getLookahead();}
                        if(lookahead == 30){
                            printAction(parserStack.Peek(), lookahead, "matching, pop( ! )");
                            parserStack.Pop();
                            scanNewLookahead = true;
                        }
                        break;
                    case 31:
                        if(scanNewLookahead){lookahead = getLookahead();}
                        if(lookahead == 31){
                            printAction(parserStack.Peek(), lookahead, "matching, pop( ? )");
                            parserStack.Pop();
                            scanNewLookahead = true;
                        }
                        break;
                    case 32:
                        if(scanNewLookahead){lookahead = getLookahead();}
                        if(lookahead == 32){
                            printAction(parserStack.Peek(), lookahead, "matching, pop( ( )");
                            parserStack.Pop();
                            scanNewLookahead = true;
                        }
                        break;
                    case 33:
                        if(scanNewLookahead){lookahead = getLookahead();}
                        if(lookahead == 33){
                            printAction(parserStack.Peek(), lookahead, "matching, pop( ) )");
                            parserStack.Pop();
                            scanNewLookahead = true;
                        }
                        break;
                    case 34:
                        if(scanNewLookahead){lookahead = getLookahead();}
                        if(lookahead == 4){
                            printAction(parserStack.Peek(), lookahead, "rule 1: pop(<Trump>), push(<last>, <stmts>, <first>)");
                            parserStack.Pop();
                            parserStack.Push(36);
                            parserStack.Push(37);
                            parserStack.Push(35);
                            scanNewLookahead = false;
                        }
                        else{
                            //errorDetected = true;
                        }
                        break;
                    case 35:
                        if(scanNewLookahead){lookahead = getLookahead();}
                        if(lookahead == 4){
                            printAction(parserStack.Peek(), lookahead, "rule 2: pop(<first>), push(again, great, america, make)");
                            parserStack.Pop();
                            parserStack.Push(7);
                            parserStack.Push(6);
                            parserStack.Push(5);
                            parserStack.Push(4);
                            scanNewLookahead = false;
                        }
                        break;
                    case 36:
                        break;
                    case 37:
                        if(scanNewLookahead){lookahead = getLookahead();}
                        if(lookahead == 4 || lookahead == 1 ||  lookahead == 13 || lookahead == 14 || lookahead == 16 || lookahead == 17){
                            printAction(parserStack.Peek(), lookahead, "rule 4: pop(<stmts>), push(<more-stmts>, ;, <stmt>)");
                            parserStack.Pop();
                            parserStack.Push(38);
                            parserStack.Push(28);
                            parserStack.Push(39);
                            scanNewLookahead = false;
                        }
                        break;
                    case 38:
                        if(scanNewLookahead){lookahead = getLookahead();}
                        if(lookahead == 4 || lookahead == 1 ||  lookahead == 13 || lookahead == 14 || lookahead == 16 || lookahead == 17){
                            printAction(parserStack.Peek(), lookahead, "rule 5: push(;, <stmt>)");
                            parserStack.Push(28);
                            parserStack.Push(39);

                            scanNewLookahead = false;
                        }
                        else if(lookahead == 8){
                            printAction(parserStack.Peek(), lookahead, "rule 6: pop(<more-stmts>)");
                            parserStack.Pop();
                            scanNewLookahead = false;
                        }
                        break;
                    case 39:
                        if(scanNewLookahead){lookahead = getLookahead();}
                        if(lookahead == 4){
                            printAction(parserStack.Peek(), lookahead, "rule 7: pop(<stmt>), push(<decl>)");
                            parserStack.Pop();
                            parserStack.Push(40);
                            scanNewLookahead = false;
                        }
                        else if(lookahead == 1){
                            printAction(parserStack.Peek(), lookahead, "rule 8: pop(<stmt>), push(<asmt>)");
                            parserStack.Pop();
                            parserStack.Push(42);
                            scanNewLookahead = false;
                        }
                        else if(lookahead == 13){
                            printAction(parserStack.Peek(), lookahead, "rule 9: pop(<stmt>), push(<cond>)");
                            parserStack.Pop();
                            parserStack.Push(43);
                            scanNewLookahead = false;
                        }
                        else if(lookahead == 14){
                            printAction(parserStack.Peek(), lookahead, "rule 10: pop(<stmt>), push(<loop>)");
                            parserStack.Pop();
                            parserStack.Push(44);
                            scanNewLookahead = false;
                        }
                        else if(lookahead == 16 || lookahead == 17){
                            printAction(parserStack.Peek(), lookahead, "rule 9: pop(<stmt>), push(<output>)");
                            parserStack.Pop();
                            parserStack.Push(45);
                            scanNewLookahead = false;
                        }
                        break;
                    case 40:
                        if(scanNewLookahead){lookahead = getLookahead();}
                        if(lookahead == 4){
                            printAction(parserStack.Peek(), lookahead, "rule 12: pop(<decl>), push(<type>, <ids>, make)");
                            parserStack.Pop();
                            parserStack.Push(41);
                            parserStack.Push(46);
                            parserStack.Push(4);
                            scanNewLookahead = false;
                        }
                        break;
                    case 41:
                        if(scanNewLookahead){lookahead = getLookahead();}
                        if(lookahead == 11){
                            printAction(parserStack.Peek(), lookahead, "rule 13: pop(<type>), push(number)");
                            parserStack.Pop();
                            parserStack.Push(11);
                            scanNewLookahead = false;
                        }
                        else if(lookahead == 12){
                            printAction(parserStack.Peek(), lookahead, "rule 14: pop(<type>), push(boolean)");
                            parserStack.Pop();
                            parserStack.Push(12);
                            scanNewLookahead = false;
                        }
                        break;
                    case 42:
                        if(lookahead == 1){
                            printAction(parserStack.Peek(), lookahead, "rule 15: pop(<asmt>), push(<expr>, is, [id])");
                            parserStack.Pop();
                            parserStack.Push(48);
                            parserStack.Push(9);
                            parserStack.Push(1);
                            scanNewLookahead = false;
                        }
                        break;
                    case 43:
                        break;
                    case 44:
                        break;
                    case 45:
                        break;
                    case 46:
                        if(scanNewLookahead){lookahead = getLookahead();}
                        if(lookahead == 1){
                            printAction(parserStack.Peek(), lookahead, "rule 20: pop(<ids>), push(<more-ids>, [id])");
                            parserStack.Pop();
                            parserStack.Push(47);
                            parserStack.Push(1);
                            scanNewLookahead = false;
                        }
                        break;
                    case 47:
                        if(scanNewLookahead){lookahead = getLookahead();}
                        if(lookahead == 1){
                            printAction(parserStack.Peek(), lookahead, "rule 21: pop(<more-ids>), push(<more-ids>, [id])");
                            parserStack.Pop();
                            parserStack.Push(47);
                            parserStack.Push(1);
                            scanNewLookahead = false;
                        }
                        else if(lookahead == 11 || lookahead == 12 || lookahead == 26){
                            printAction(parserStack.Peek(), lookahead, "rule 22: pop(<more-ids>)");
                            parserStack.Pop();
                            scanNewLookahead = false;
                        }
                        break;
                    case 48:
                        Console.WriteLine("here");
                        if(scanNewLookahead){lookahead = getLookahead(); Console.WriteLine("there");}
                        Console.WriteLine("everywhere");
                        if(lookahead == 18 || lookahead == 19 || lookahead == 20 || lookahead == 23 || lookahead == 9 || lookahead == 24){
                            printAction(parserStack.Peek(), lookahead, "rule 23: pop(<expr>), push(<bool>)");
                            parserStack.Pop();
                            parserStack.Push(49);
                            scanNewLookahead = false;
                        }
                        else if(lookahead == 1 || lookahead == 2 || lookahead == 32){
                            printAction(parserStack.Peek(), lookahead, "rule 24: pop(<expr>), push(<arith>)");
                            parserStack.Pop();
                            parserStack.Push(52);
                            scanNewLookahead = false;
                        }
                        break;
                    case 49:
                        break;
                    case 50:
                        break;
                    case 51:
                        break;
                    case 52:
                        break;
                    case 53:
                        break;
                    default:
                        //errorDetected = true;
                        break;
                }
                
            }
        }
        private void printAction(int stackTop, int passedLookahead, String action){
            if(dictionary[passedLookahead].Length > 7){
                if(dictionary[stackTop].Length > 7){
                    Console.WriteLine(programStep + "\t" + dictionary[stackTop] + "\t" + dictionary[passedLookahead] + "\t" + action);
                }
                else{
                    Console.WriteLine(programStep + "\t" + dictionary[stackTop] + "\t\t" + dictionary[passedLookahead] + "\t" + action);
                }
            }
            else{
                if(dictionary[stackTop].Length > 7){
                    Console.WriteLine(programStep + "\t" + dictionary[stackTop] + "\t" + dictionary[passedLookahead] + "\t\t" + action);
                }
                else{
                    Console.WriteLine(programStep + "\t" + dictionary[stackTop] + "\t\t" + dictionary[passedLookahead] + "\t\t" + action);
                }
            }
            programStep++;
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
                returnInt = dictionary.Last(x => x.Value == passedToken.Lexeme).Key;
            }
            return returnInt;
        }
        private int getLookahead(){
            Scanner scanner = new Scanner(reader, bk, er);
            Token lookahead = scanner.detectToken();
            Console.WriteLine(parserStack.Peek() + " lookahead: " + lookahead.Lexeme);
            return getIntegerCodeofToken(lookahead);
        }
        private void addDictionaryDefinitions(){
            dictionary.Add( -1, "-none-");
            dictionary.Add( 0, "Zo");
            dictionary.Add( 1, "[ID]");
            dictionary.Add( 2, "[CONSTANT]");
            dictionary.Add( 3, "[STRING]");
            dictionary.Add( 4, "make" );
            dictionary.Add( 5, "programming");
            dictionary.Add( 6, "great");
            dictionary.Add( 7, "again" );
            dictionary.Add( 8, "america");
            dictionary.Add( 9, "is");
            dictionary.Add( 10, "else" );
            dictionary.Add( 11, "number");
            dictionary.Add( 12, "boolean");
            dictionary.Add( 13, "if" );
            dictionary.Add( 14, "as");
            dictionary.Add( 15, "long");
            dictionary.Add( 16, "tell" );
            dictionary.Add( 17, "say");
            dictionary.Add( 18, "fact");
            dictionary.Add( 19, "lie");
            dictionary.Add( 20, "not");
            dictionary.Add( 21, "and");
            dictionary.Add( 22, "or");
            dictionary.Add( 23, "less");
            dictionary.Add( 24, "more");
            dictionary.Add( 25, "plus");
            dictionary.Add( 26, "times");
            dictionary.Add( 27, ",");
            dictionary.Add( 28, ";");
            dictionary.Add( 29, ":");
            dictionary.Add( 30, "!");
            dictionary.Add( 31, "?");
            dictionary.Add( 32, "(");
            dictionary.Add( 33, ")");
            dictionary.Add(34, "<Trump>");
            dictionary.Add( 35, "<first>");
            dictionary.Add( 36, "<last>");
            dictionary.Add( 37, "<stmts>");
            dictionary.Add( 38, "<more-stmts>");
            dictionary.Add( 39, "<stmt>");
            dictionary.Add( 40, "<decl>");
            dictionary.Add( 41, "<type>");
            dictionary.Add( 42, "<asmt>");
            dictionary.Add( 43, "<cond>");
            dictionary.Add( 44, "<loop>");
            dictionary.Add( 45, "<output>");
            dictionary.Add( 46, "<ids>");
            dictionary.Add( 47, "<more-ids>");
            dictionary.Add( 48, "<expr>");
            dictionary.Add( 49, "<bool>");
            dictionary.Add( 50, "<bool-tail>");
            dictionary.Add( 51, "<test>");
            dictionary.Add( 52, "<arith>");
            dictionary.Add( 53, "<arith-tail>");
        }
    }
}