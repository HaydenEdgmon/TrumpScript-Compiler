# TrumpScript-Compiler
A compiler for a simpler version of TrumpScript for my compiler construction class.

## Scanner

The scanner was implamented (by instructor requirement) and a state/token DFA, I used a switch statement to take the state then took action based on the next valid character. 

Enumerations were used for States and Token types so that no eronious values could exist for those types of variables. The Token class instantiates a Token object that contains the lexeme and the type that the token is determined to be at the end of the DFA. These Tokens are stored in the Symbol Table, which is a List object in the Bookkeeper class.

