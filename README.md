# TrumpScript-Compiler
A compiler for a simpler version of TrumpScript for my compiler construction class.

## Scanner and Lexical Analysis

The scanner was implamented (by instructor requirement) and a state/token DFA, I used a switch statement to take the state then took action based on the next valid character. 

Enumerations were used for States and Token types so that no eronious values could exist for those types of variables. The Token class instantiates a Token object that contains the lexeme and the type that the token is determined to be at the end of the DFA. These Tokens are stored in the Symbol Table, which is a List object in the Bookkeeper class.

## Parser and Syntactic Analysis

PDA implamentation of an LL(1) parser. the CFG was given to us as well as the LL(1) parse table. Implamentation done with a stack and the Scanner class, where the parser calls the Scanner to receive a lookaheads token and does not get a new one until the top of the stack and the lokkahead token match