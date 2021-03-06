/*
    Hayden Edgmon
    Written for: CS 4323 Compiler Construction at The Univerity of Oklahoma

    Parser requirements: 
     * use of integer stack for PDA (named parserStack in implamentation below)
     * integer codes for all terminals and non-terminals in PDA
     * Parser called once in main()
     * all parsing done in Parser
     * use 0 as the stack bottom marker (pushed in Parser constructor)
     * Parser calls Scanner when a new lookahead token is needed
     * Scanner returns a Token to the parser
 */

using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using CompilerHelpers;
using Scanners;

namespace Parsers{
    class Parser{
        //private variables
        Stack<int> parserStack = new Stack<int>();
        StreamReader reader;
        Bookkeeper bk = new Bookkeeper();
        ErrorHandler er = new ErrorHandler();
        Dictionary<int, String> dictionary = new Dictionary<int, String>();
        int programStep = 0;
        //constructors
        public Parser(){
            parserStack.Push(0);
            this.addDictionaryDefinitions();
        }
        public Parser(StreamReader passedReader){
            reader = passedReader;
            parserStack.Push(0);
            this.addDictionaryDefinitions();
        }
        //parser's main function, implamentation of PDA actions in theis method
        public void parseProgram(){
            //set initial values for PDA
            int lookahead = -1;
            Token token = new Token();
            Boolean scanNewLookahead = true;
            //read file until EoF 
            while(reader.Peek() != -1){
                switch(parserStack.Peek()){
                    case 0:
                        //first action, push <Trump>
                        printAction(parserStack.Peek(), -1, "Push(<Trump>)");
                        parserStack.Push(34);
                        break;
                    case 1:
                        // [id] at top of stack
                        if(scanNewLookahead){
                            token = getLookaheadToken();
                            lookahead = getLookahead(token);
                        }
                        if(lookahead == 1){
                            printAction(parserStack.Peek(), lookahead, "matching, pop([ID])");
                            parserStack.Pop();
                            scanNewLookahead = true;
                        }
                        break;
                    case 2:
                        // [const] at top of stack
                        if(scanNewLookahead){
                            token = getLookaheadToken();
                            lookahead = getLookahead(token);
                        }
                        if(lookahead == 2){
                            printAction(parserStack.Peek(), lookahead, "matching, pop([CONSTANT])");
                            parserStack.Pop();
                            scanNewLookahead = true;
                        }
                        break;
                    case 3:
                        // [string] at top of stack
                        if(scanNewLookahead){
                            token = getLookaheadToken();
                            lookahead = getLookahead(token);
                        }
                        if(lookahead == 3){
                            printAction(parserStack.Peek(), lookahead, "matching, pop([STRING])");
                            parserStack.Pop();
                            scanNewLookahead = true;
                        }
                        break;
                    case 4:
                        // "make" at top of stack
                        if(scanNewLookahead){
                            token = getLookaheadToken();
                            lookahead = getLookahead(token);
                        }
                        if(lookahead == 4){
                            printAction(parserStack.Peek(), lookahead, "matching, pop(make))");
                            parserStack.Pop();
                            scanNewLookahead = true;
                        }
                        break;
                    case 5:
                        // "programming" at top of stack
                        if(scanNewLookahead){
                            token = getLookaheadToken();
                            lookahead = getLookahead(token);
                        }
                        if(lookahead == 5){
                            printAction(parserStack.Peek(), lookahead, "matching, pop(america)");
                            parserStack.Pop();
                            scanNewLookahead = true;
                        }
                        break;
                    case 6:
                        // "great" at top of stack
                        if(scanNewLookahead){
                            token = getLookaheadToken();
                            lookahead = getLookahead(token);
                        }
                        if(lookahead == 6){
                            printAction(parserStack.Peek(), lookahead, "matching, pop(great)");
                            parserStack.Pop();
                            scanNewLookahead = true;
                        }
                        break;
                    case 7:
                        // "again" at top of stack
                        if(scanNewLookahead){
                            token = getLookaheadToken();
                            lookahead = getLookahead(token);
                        }
                        if(lookahead == 7){
                            printAction(parserStack.Peek(), lookahead, "matching, pop(again)");
                            parserStack.Pop();
                            scanNewLookahead = true;
                        }
                        break;
                    case 8:
                        // "america" at top of stack
                        if(scanNewLookahead){
                            token = getLookaheadToken();
                            lookahead = getLookahead(token);
                        }
                        if(lookahead == 8){
                            printAction(parserStack.Peek(), lookahead, "matching, pop(america)");
                            parserStack.Pop();
                            scanNewLookahead = true;
                        }
                        break;
                    case 9:
                        // "is" at top of stack
                        if(scanNewLookahead){
                            token = getLookaheadToken();
                            lookahead = getLookahead(token);
                        }
                        if(lookahead == 9){
                            printAction(parserStack.Peek(), lookahead, "matching, pop(is)");
                            parserStack.Pop();
                            scanNewLookahead = true;
                        }
                        break;
                    case 10:
                        // "else" at top of stack
                        if(scanNewLookahead){
                            token = getLookaheadToken();
                            lookahead = getLookahead(token);
                        }
                        if(lookahead == 10){
                            printAction(parserStack.Peek(), lookahead, "matching, pop(else)");
                            parserStack.Pop();
                            scanNewLookahead = true;
                        }
                        break;
                    case 11:
                        // "number" at top of stack
                        if(scanNewLookahead){
                            token = getLookaheadToken();
                            lookahead = getLookahead(token);
                        }
                        if(lookahead == 11){
                            printAction(parserStack.Peek(), lookahead, "matching, pop(number)");
                            parserStack.Pop();
                            scanNewLookahead = true;
                        }
                        break;
                    case 12:
                        // "boolean" at top of stack
                        if(scanNewLookahead){
                            token = getLookaheadToken();
                            lookahead = getLookahead(token);
                        }
                        if(lookahead == 12){
                            printAction(parserStack.Peek(), lookahead, "matching, pop(boolean)");
                            parserStack.Pop();
                            scanNewLookahead = true;
                        }
                        break;
                    case 13:
                        // "if" at top of stack
                        if(scanNewLookahead){
                            token = getLookaheadToken();
                            lookahead = getLookahead(token);
                        }
                        if(lookahead == 13){
                            printAction(parserStack.Peek(), lookahead, "matching, pop(if)");
                            parserStack.Pop();
                            scanNewLookahead = true;
                        }
                        break;
                    case 14:
                        // "as" at top of stack
                        if(scanNewLookahead){
                            token = getLookaheadToken();
                            lookahead = getLookahead(token);
                        }
                        if(lookahead == 14){
                            printAction(parserStack.Peek(), lookahead, "matching, pop(as)");
                            parserStack.Pop();
                            scanNewLookahead = true;
                        }
                        break;
                    case 15:
                        // "long" at top of stack
                        if(scanNewLookahead){
                            token = getLookaheadToken();
                            lookahead = getLookahead(token);
                        }
                        if(lookahead == 15){
                            printAction(parserStack.Peek(), lookahead, "matching, pop(long)");
                            parserStack.Pop();
                            scanNewLookahead = true;
                        }
                        break;
                    case 16:
                        // "tell" at top of stack
                        if(scanNewLookahead){
                            token = getLookaheadToken();
                            lookahead = getLookahead(token);
                        }
                        if(lookahead == 16){
                            printAction(parserStack.Peek(), lookahead, "matching, pop(tell)");
                            parserStack.Pop();
                            scanNewLookahead = true;
                        }
                        break;
                    case 17:
                        // "say" at top of stack
                        if(scanNewLookahead){
                            token = getLookaheadToken();
                            lookahead = getLookahead(token);
                        }
                        if(lookahead == 17){
                            printAction(parserStack.Peek(), lookahead, "matching, pop(say)");
                            parserStack.Pop();
                            scanNewLookahead = true;
                        }
                        break;
                    case 18:
                        // "fact at top of stack
                        if(scanNewLookahead){
                            token = getLookaheadToken();
                            lookahead = getLookahead(token);
                        }
                        if(lookahead == 18){
                            printAction(parserStack.Peek(), lookahead, "matching, pop(fact)");
                            parserStack.Pop();
                            scanNewLookahead = true;
                        }
                        break;
                    case 19:
                        // "lie" at top of stack
                        if(scanNewLookahead){
                            token = getLookaheadToken();
                            lookahead = getLookahead(token);
                        }
                        if(lookahead == 19){
                            printAction(parserStack.Peek(), lookahead, "matching, pop(lie)");
                            parserStack.Pop();
                            scanNewLookahead = true;
                        }
                        break;
                    case 20:
                        // "not" at top of stack
                        if(scanNewLookahead){
                            token = getLookaheadToken();
                            lookahead = getLookahead(token);
                        }
                        if(lookahead == 20){
                            printAction(parserStack.Peek(), lookahead, "matching, pop(not)");
                            parserStack.Pop();
                            scanNewLookahead = true;
                        }
                        break;
                    case 21:
                        // "and" at top of stack
                        if(scanNewLookahead){
                            token = getLookaheadToken();
                            lookahead = getLookahead(token);
                        }
                        if(lookahead == 21){
                            printAction(parserStack.Peek(), lookahead, "matching, pop(and)");
                            parserStack.Pop();
                            scanNewLookahead = true;
                        }
                        break;
                    case 22:
                        // "or" at top of stack
                        if(scanNewLookahead){
                            token = getLookaheadToken();
                            lookahead = getLookahead(token);
                        }
                        if(lookahead == 22){
                            printAction(parserStack.Peek(), lookahead, "matching, pop(or)");
                            parserStack.Pop();
                            scanNewLookahead = true;
                        }
                        break;
                    case 23:
                        // "less" at top of stack
                        if(scanNewLookahead){
                            token = getLookaheadToken();
                            lookahead = getLookahead(token);
                        }
                        if(lookahead == 23){
                            printAction(parserStack.Peek(), lookahead, "matching, pop(less)");
                            parserStack.Pop();
                            scanNewLookahead = true;
                        }
                        break;
                    case 24:
                        // "more" at top of stack
                        if(scanNewLookahead){
                            token = getLookaheadToken();
                            lookahead = getLookahead(token);
                        }
                        if(lookahead == 24){
                            printAction(parserStack.Peek(), lookahead, "matching, pop(more)");
                            parserStack.Pop();
                            scanNewLookahead = true;
                        }
                        break;
                    case 25:
                        // "plus" at top of stack
                        if(scanNewLookahead){
                            token = getLookaheadToken();
                            lookahead = getLookahead(token);
                        }
                        if(lookahead == 25){
                            printAction(parserStack.Peek(), lookahead, "matching, pop(plus)");
                            parserStack.Pop();
                            scanNewLookahead = true;
                        }
                        break;
                    case 26:
                        // "times" at top of stack
                        if(scanNewLookahead){
                            token = getLookaheadToken();
                            lookahead = getLookahead(token);
                        }
                        if(lookahead == 26){
                            printAction(parserStack.Peek(), lookahead, "matching, pop(times)");
                            parserStack.Pop();
                            scanNewLookahead = true;
                        }
                        break;
                    case 27:
                        // "," at top of stack
                        if(scanNewLookahead){
                            token = getLookaheadToken();
                            lookahead = getLookahead(token);
                        }
                        if(lookahead == 27){
                            printAction(parserStack.Peek(), lookahead, "matching, pop( , )");
                            parserStack.Pop();
                            //Console.WriteLine(parserStack.Peek());
                            scanNewLookahead = true;
                        }
                        break;
                    case 28:
                        // ";" at top of stack
                        if(scanNewLookahead){
                            token = getLookaheadToken();
                            lookahead = getLookahead(token);
                        }
                        if(lookahead == 28){
                            printAction(parserStack.Peek(), lookahead, "matching, pop( ; )");
                            parserStack.Pop();
                            scanNewLookahead = true;
                        }
                        break;
                    case 29:
                        // ":" at top of stack
                        if(scanNewLookahead){
                            token = getLookaheadToken();
                            lookahead = getLookahead(token);
                        }
                        if(lookahead == 29){
                            printAction(parserStack.Peek(), lookahead, "matching, pop( : )");
                            parserStack.Pop();
                            scanNewLookahead = true;
                        }
                        break;
                    case 30:
                        // "!" at top of stack
                        if(scanNewLookahead){
                            token = getLookaheadToken();
                            lookahead = getLookahead(token);
                        }
                        if(lookahead == 30){
                            printAction(parserStack.Peek(), lookahead, "matching, pop( ! )");
                            parserStack.Pop();
                            scanNewLookahead = true;
                        }
                        break;
                    case 31:
                        // "?" at top of stack
                        if(scanNewLookahead){
                            token = getLookaheadToken();
                            lookahead = getLookahead(token);
                        }
                        if(lookahead == 31){
                            printAction(parserStack.Peek(), lookahead, "matching, pop( ? )");
                            parserStack.Pop();
                            scanNewLookahead = true;
                        }
                        break;
                    case 32:
                        // "(" at top of stack
                        if(scanNewLookahead){
                            token = getLookaheadToken();
                            lookahead = getLookahead(token);
                        }
                        if(lookahead == 32){
                            printAction(parserStack.Peek(), lookahead, "matching, pop( ( )");
                            parserStack.Pop();
                            scanNewLookahead = true;
                        }
                        break;
                    case 33:
                        // ")" at top of stack
                        if(scanNewLookahead){
                            token = getLookaheadToken();
                            lookahead = getLookahead(token);
                        }
                        if(lookahead == 33){
                            printAction(parserStack.Peek(), lookahead, "matching, pop( ) )");
                            parserStack.Pop();
                            scanNewLookahead = true;
                        }
                        break;
                    case 34:
                        // <Trump> at top of stack
                        if(scanNewLookahead){
                            token = getLookaheadToken();
                            lookahead = getLookahead(token);
                        }
                        if(lookahead == 4){
                            printAction(parserStack.Peek(), lookahead, "rule 1: pop(<Trump>), push(<last>, <stmts>, <first>)");
                            parserStack.Pop();
                            parserStack.Push(36);
                            parserStack.Push(37);
                            parserStack.Push(35);
                            scanNewLookahead = false;
                        }
                        break;
                    case 35:
                        // <first> at top of stack
                        if(scanNewLookahead){
                            token = getLookaheadToken();
                            lookahead = getLookahead(token);
                        }
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
                        // <last> at top of stack
                        if(scanNewLookahead){
                            token = getLookaheadToken();
                            lookahead = getLookahead(token);
                        }
                        if(lookahead == 8){
                            printAction(parserStack.Peek(), lookahead, "rule 3: pop(<last>), push(great, is, america)");
                            parserStack.Pop();
                            parserStack.Push(6);
                            parserStack.Push(9);
                            parserStack.Push(8);
                            scanNewLookahead = false;
                        }
                        break;
                    case 37:
                        // <stmts> at top of stack
                        if(scanNewLookahead){
                            token = getLookaheadToken();
                            lookahead = getLookahead(token);
                        }
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
                        // <more-stmts> at top of stack
                        if(scanNewLookahead){
                            token = getLookaheadToken();
                            lookahead = getLookahead(token);
                        }
                        if(lookahead == 4 || lookahead == 1 ||  lookahead == 13 || lookahead == 14 || lookahead == 16 || lookahead == 17){
                            printAction(parserStack.Peek(), lookahead, "rule 5: push(;, <stmt>)");
                            parserStack.Push(28);
                            parserStack.Push(39);

                            scanNewLookahead = false;
                        }
                        else if(lookahead == 8 || lookahead == 30){
                            printAction(parserStack.Peek(), lookahead, "rule 6: pop(<more-stmts>)");
                            parserStack.Pop();
                            scanNewLookahead = false;
                        }
                        break;
                    case 39:
                        // <stmt> at top of stack
                        if(scanNewLookahead){
                            token = getLookaheadToken();
                            lookahead = getLookahead(token);
                        }
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
                        // <decl> at top of stack
                        if(scanNewLookahead){
                            token = getLookaheadToken();
                            lookahead = getLookahead(token);
                        }
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
                        // <type> at top of stack
                        if(scanNewLookahead){
                            token = getLookaheadToken();
                            lookahead = getLookahead(token);
                        }
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
                        // <asmt> at top of stack
                        if(scanNewLookahead){
                            token = getLookaheadToken();
                            lookahead = getLookahead(token);
                        }
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
                        // <cond> at top of stack
                        if(scanNewLookahead){
                            token = getLookaheadToken();
                            lookahead = getLookahead(token);
                        }
                        if(lookahead == 13){
                            printAction(parserStack.Peek(), lookahead, "rule 16: pop(<cond>), push(! , <stmts>, : , else, ! , <stmts>, : , ; , <bool>, , , if)");
                            parserStack.Pop();
                            parserStack.Push(30);
                            parserStack.Push(37);
                            parserStack.Push(29);
                            parserStack.Push(10);
                            parserStack.Push(30);
                            parserStack.Push(37);
                            parserStack.Push(29);
                            parserStack.Push(28);
                            parserStack.Push(49);
                            parserStack.Push(27);
                            parserStack.Push(13);
                            scanNewLookahead = false;
                        }
                        break;
                    case 44:
                        // <loop> at top of stack
                        if(scanNewLookahead){
                            token = getLookaheadToken();
                            lookahead = getLookahead(token);
                        }
                        if(lookahead == 14){
                            printAction(parserStack.Peek(), lookahead, "rule 17: pop(<loop>), push(! , <stmts>, : , ; , <bool>, , , as, long, as)");
                            parserStack.Pop();
                            parserStack.Push(30);
                            parserStack.Push(37);
                            parserStack.Push(29);
                            parserStack.Push(28);
                            parserStack.Push(49);
                            parserStack.Push(27);
                            parserStack.Push(14);
                            parserStack.Push(15);
                            parserStack.Push(14);
                            scanNewLookahead = false;
                        }
                        break;
                    case 45:
                        // <output> at top of stack
                        if(scanNewLookahead){
                            token = getLookaheadToken();
                            lookahead = getLookahead(token);
                        }
                        if(lookahead == 16){
                            printAction(parserStack.Peek(), lookahead, "rule 18: pop(<output>), push(<ids>, tell)");
                            parserStack.Pop();
                            parserStack.Push(46);
                            parserStack.Push(16);
                            scanNewLookahead = false;
                        }
                        else if(lookahead == 17){
                            printAction(parserStack.Peek(), lookahead, "rule 19: pop(<output>), push([string], say)");
                            parserStack.Pop();
                            parserStack.Push(3);
                            parserStack.Push(17);
                            scanNewLookahead = false;
                        }
                        break;
                    case 46:
                        // <ids> at top of stack
                        if(scanNewLookahead){
                            token = getLookaheadToken();
                            lookahead = getLookahead(token);
                        }
                        if(lookahead == 1){
                            printAction(parserStack.Peek(), lookahead, "rule 20: pop(<ids>), push(<more-ids>, [id])");
                            parserStack.Pop();
                            
                            parserStack.Push(47);
                            
                            parserStack.Push(1);
                            
                            scanNewLookahead = false;
                        }
                        break;
                    case 47:
                        // <more-ids> at top of stack
                        if(scanNewLookahead){
                            token = getLookaheadToken();
                            lookahead = getLookahead(token);
                        }
                        if(lookahead == 1){
                            printAction(parserStack.Peek(), lookahead, "rule 21: pop(<more-ids>), push(<more-ids>, [id])");
                            parserStack.Pop();
                            
                            parserStack.Push(47);
                            parserStack.Push(1);
                            
                            scanNewLookahead = false;
                        }
                        else if(lookahead == 11 || lookahead == 12 || lookahead == 28){
                            printAction(parserStack.Peek(), lookahead, "rule 22: pop(<more-ids>)");
                            parserStack.Pop();
                            scanNewLookahead = false;
                        }
                        break;
                    case 48:
                        // <expr> at top of stack
                        if(scanNewLookahead){
                            token = getLookaheadToken();
                            lookahead = getLookahead(token);
                        }
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
                        // <bool> at top of stack
                        if(scanNewLookahead){
                            token = getLookaheadToken();
                            lookahead = getLookahead(token);
                        }
                        if(lookahead == 18){
                            printAction(parserStack.Peek(), lookahead, "rule 25: pop(<bool>), push(<bool-tail>, fact)");
                            parserStack.Pop();
                            parserStack.Push(50);
                            parserStack.Push(18);
                            scanNewLookahead = false;
                        }
                        else if(lookahead == 19){
                            printAction(parserStack.Peek(), lookahead, "rule 26: pop(<bool>), push(<bool-tail>, lie)");
                            parserStack.Pop();
                            parserStack.Push(50);
                            parserStack.Push(19);
                            scanNewLookahead = false;
                        }
                        else if(lookahead == 20){
                            printAction(parserStack.Peek(), lookahead, "rule 27: pop(<bool>), push(<bool>, not)");
                            parserStack.Pop();
                            parserStack.Push(49);
                            parserStack.Push(20);
                            scanNewLookahead = false;
                        }
                        else if(lookahead == 23 || lookahead == 9 || lookahead == 24){
                            printAction(parserStack.Peek(), lookahead, "rule 28: pop(<bool>), push(?, <bool>, <boot>, <test>)");
                            parserStack.Pop();
                            parserStack.Push(31);
                            parserStack.Push(52);
                            parserStack.Push(52);
                            parserStack.Push(51);
                            scanNewLookahead = false;
                        }
                        break;
                    case 50:
                        // <bool-tail> at top of stack
                        if(scanNewLookahead){
                            token = getLookaheadToken();
                            lookahead = getLookahead(token);
                        }
                        if(lookahead == 21){
                            printAction(parserStack.Peek(), lookahead, "rule 29: pop(<bool-tail>), push(<bool>, and)");
                            parserStack.Pop();
                            parserStack.Push(49);
                            parserStack.Push(21);
                            scanNewLookahead = false;
                        }
                        else if(lookahead == 22){
                            printAction(parserStack.Peek(), lookahead, "rule 30: pop(<bool-tail>), push(<bool>, or)");
                            parserStack.Pop();
                            parserStack.Push(49);
                            parserStack.Push(22);
                            scanNewLookahead = false;
                        }
                        else if(lookahead == 28){
                            printAction(parserStack.Peek(), lookahead, "rule 31: pop(<bool-tail>)");
                            parserStack.Pop();
                            scanNewLookahead = false;
                        }
                        break;
                    case 51:
                        // <test> at top of stack
                        if(scanNewLookahead){
                            token = getLookaheadToken();
                            lookahead = getLookahead(token);
                        }
                        if(lookahead == 23){
                            printAction(parserStack.Peek(), lookahead, "rule 32: pop(<bool-tail>), push(less)");
                            parserStack.Pop();
                            parserStack.Push(23);
                            scanNewLookahead = false;
                        }
                        else if(lookahead == 9){
                            printAction(parserStack.Peek(), lookahead, "rule 33: pop(<bool-tail>), push(is)");
                            parserStack.Pop();
                            parserStack.Push(9);
                            scanNewLookahead = false;
                        }
                        else if(lookahead == 24){
                            printAction(parserStack.Peek(), lookahead, "rule 34: pop(<bool-tail>), push(more)");
                            parserStack.Pop();
                            parserStack.Push(24);
                            scanNewLookahead = false;
                        }
                        break;
                    case 52:
                        // <arith> at top of stack
                        if(scanNewLookahead){
                            token = getLookaheadToken();
                            lookahead = getLookahead(token);
                        }
                        if(lookahead == 1){
                            printAction(parserStack.Peek(), lookahead, "rule 35: pop(<arith>), push(<arith-tail>, [id])");
                            parserStack.Pop();
                            parserStack.Push(53);
                            parserStack.Push(1);
                            scanNewLookahead = false;
                        }
                        else if(lookahead == 2){
                            printAction(parserStack.Peek(), lookahead, "rule 36: pop(<arith>), push(<arith-tail>, [const])");
                            parserStack.Pop();
                            parserStack.Push(53);
                            parserStack.Push(2);
                            scanNewLookahead = false;
                        }
                        else if(lookahead == 32){
                            printAction(parserStack.Peek(), lookahead, "rule 37: pop(<arith>), push(<arith-tail>, ( , <arith> , ))");
                            parserStack.Pop();
                            parserStack.Push(53);
                            parserStack.Push(33);
                            parserStack.Push(52);
                            parserStack.Push(32);
                            scanNewLookahead = false;
                        }
                        break;
                    case 53:
                        // <arith-tail> at top of stack
                        if(scanNewLookahead){
                            token = getLookaheadToken();
                            lookahead = getLookahead(token);
                        }
                        if(lookahead == 25){
                            printAction(parserStack.Peek(), lookahead, "rule 38: pop(<arith-tail>), push(<arith>, plus)");
                            parserStack.Pop();
                            parserStack.Push(52);
                            parserStack.Push(25);
                            scanNewLookahead = false;
                        }
                        else if(lookahead == 26){
                            printAction(parserStack.Peek(), lookahead, "rule 39: pop(<arith-tail>), push(<arith>, times)");
                            parserStack.Pop();
                            parserStack.Push(52);
                            parserStack.Push(26);
                            scanNewLookahead = false;
                        }
                        else if(lookahead == 28 || lookahead == 1 || lookahead == 2 || lookahead == 32 || lookahead == 31 || lookahead == 33 ){
                            printAction(parserStack.Peek(), lookahead, "rule 40: pop(<arith-tail>), epsilon)");
                            parserStack.Pop();
                            scanNewLookahead = false;
                        }
                        break;
                    default:
                        //errorDetected = true;
                        break;
                }
                
            }
            Console.WriteLine("=======================================");
            Console.WriteLine("SYMTAB Content: ");
            bk.printSymTab();
        }
        //Output the information for an action(if/else(s) for alignment in console)
        private void printAction(int stackTop, int passedLookahead, String action){
            if(dictionary[passedLookahead].Length > 7){
                if(dictionary[stackTop].Length > 7){
                    Console.WriteLine(programStep + "\t\t" + dictionary[stackTop] + "\t" + dictionary[passedLookahead] + "\t" + action);
                }
                else{
                    Console.WriteLine(programStep + "\t\t" + dictionary[stackTop] + "\t\t" + dictionary[passedLookahead] + "\t" + action);
                }
            }
            else{
                if(dictionary[stackTop].Length > 7){
                    Console.WriteLine(programStep + "\t\t" + dictionary[stackTop] + "\t" + dictionary[passedLookahead] + "\t\t" + action);
                }
                else{
                    Console.WriteLine(programStep + "\t\t" + dictionary[stackTop] + "\t\t" + dictionary[passedLookahead] + "\t\t" + action);
                }
            }
            programStep++;
        }
        //looks up the dictionary integer value based on a Token object
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
        //get integer lookahead for the scanned token
        private int getLookahead(Token lookahead){
            return getIntegerCodeofToken(lookahead);
        }
        //get a token from the scanner
        private Token getLookaheadToken(){
            Scanner scanner = new Scanner(reader, bk, er);
            return scanner.detectToken();
        }
        //dictionary of all terminals and non-terminals in PDA
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