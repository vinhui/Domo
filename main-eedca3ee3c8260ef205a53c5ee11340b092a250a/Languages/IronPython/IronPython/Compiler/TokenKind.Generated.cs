/* ****************************************************************************
 *
 * Copyright (c) Microsoft Corporation. 
 *
 * This source code is subject to terms and conditions of the Apache License, Version 2.0. A 
 * copy of the license can be found in the License.html file at the root of this distribution. If 
 * you cannot locate the  Apache License, Version 2.0, please send an email to 
 * dlr@microsoft.com. By using this source code in any fashion, you are agreeing to be bound 
 * by the terms of the Apache License, Version 2.0.
 *
 * You must not remove this notice, or any other, from this software.
 *
 *
 * ***************************************************************************/

using System;
using System.Collections.Generic;
using Microsoft.Scripting;

namespace IronPython.Compiler {

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Dedent")]
    public enum TokenKind {
        EndOfFile = -1,
        Error = 0,
        NewLine = 1,
        Indent = 2,
        Dedent = 3,
        Comment = 4,
        Name = 8,
        Constant = 9,
        Dot = 31,

        #region Generated Token Kinds

        // *** BEGIN GENERATED CODE ***
        // generated by function: tokenkinds_generator from: generate_ops.py

        Add = 32,
        AddEqual = 33,
        Subtract = 34,
        SubtractEqual = 35,
        Power = 36,
        PowerEqual = 37,
        Multiply = 38,
        MultiplyEqual = 39,
        FloorDivide = 40,
        FloorDivideEqual = 41,
        Divide = 42,
        DivideEqual = 43,
        Mod = 44,
        ModEqual = 45,
        LeftShift = 46,
        LeftShiftEqual = 47,
        RightShift = 48,
        RightShiftEqual = 49,
        BitwiseAnd = 50,
        BitwiseAndEqual = 51,
        BitwiseOr = 52,
        BitwiseOrEqual = 53,
        ExclusiveOr = 54,
        ExclusiveOrEqual = 55,
        LessThan = 56,
        GreaterThan = 57,
        LessThanOrEqual = 58,
        GreaterThanOrEqual = 59,
        Equals = 60,
        NotEquals = 61,
        LessThanGreaterThan = 62,
        LeftParenthesis = 63,
        RightParenthesis = 64,
        LeftBracket = 65,
        RightBracket = 66,
        LeftBrace = 67,
        RightBrace = 68,
        Comma = 69,
        Colon = 70,
        BackQuote = 71,
        Semicolon = 72,
        Assign = 73,
        Twiddle = 74,
        At = 75,

        FirstKeyword = KeywordAnd,
        LastKeyword = KeywordWith,
        KeywordAnd = 76,
        KeywordAssert = 77,
        KeywordBreak = 78,
        KeywordClass = 79,
        KeywordContinue = 80,
        KeywordDef = 81,
        KeywordDel = 82,
        KeywordElseIf = 83,
        KeywordElse = 84,
        KeywordExcept = 85,
        KeywordExec = 86,
        KeywordFinally = 87,
        KeywordFor = 88,
        KeywordFrom = 89,
        KeywordGlobal = 90,
        KeywordIf = 91,
        KeywordImport = 92,
        KeywordIn = 93,
        KeywordIs = 94,
        KeywordLambda = 95,
        KeywordNot = 96,
        KeywordOr = 97,
        KeywordPass = 98,
        KeywordPrint = 99,
        KeywordRaise = 100,
        KeywordReturn = 101,
        KeywordTry = 102,
        KeywordWhile = 103,
        KeywordYield = 104,
        KeywordAs = 105,
        KeywordWith = 106,

        // *** END GENERATED CODE ***

        #endregion

        NLToken,
    }

    public static class Tokens {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly Token EndOfFileToken = new SymbolToken(TokenKind.EndOfFile, "<eof>");
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly Token NewLineToken = new SymbolToken(TokenKind.NewLine, "<newline>");
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly Token NLToken = new SymbolToken(TokenKind.NLToken, "<NL>");  // virtual token used for error reporting
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly Token IndentToken = new SymbolToken(TokenKind.Indent, "<indent>");

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Dedent")]
        public static readonly Token DedentToken = new SymbolToken(TokenKind.Dedent, "<dedent>");
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly Token CommentToken = new SymbolToken(TokenKind.Comment, "<comment>");
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly Token NoneToken = new ConstantValueToken(null);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly Token DotToken = new SymbolToken(TokenKind.Dot, ".");

        #region Generated Tokens

        // *** BEGIN GENERATED CODE ***
        // generated by function: tokens_generator from: generate_ops.py

        private static readonly Token symAddToken = new OperatorToken(TokenKind.Add, "+", 4);
        private static readonly Token symAddEqualToken = new SymbolToken(TokenKind.AddEqual, "+=");
        private static readonly Token symSubtractToken = new OperatorToken(TokenKind.Subtract, "-", 4);
        private static readonly Token symSubtractEqualToken = new SymbolToken(TokenKind.SubtractEqual, "-=");
        private static readonly Token symPowerToken = new OperatorToken(TokenKind.Power, "**", 6);
        private static readonly Token symPowerEqualToken = new SymbolToken(TokenKind.PowerEqual, "**=");
        private static readonly Token symMultiplyToken = new OperatorToken(TokenKind.Multiply, "*", 5);
        private static readonly Token symMultiplyEqualToken = new SymbolToken(TokenKind.MultiplyEqual, "*=");
        private static readonly Token symFloorDivideToken = new OperatorToken(TokenKind.FloorDivide, "//", 5);
        private static readonly Token symFloorDivideEqualToken = new SymbolToken(TokenKind.FloorDivideEqual, "//=");
        private static readonly Token symDivideToken = new OperatorToken(TokenKind.Divide, "/", 5);
        private static readonly Token symDivideEqualToken = new SymbolToken(TokenKind.DivideEqual, "/=");
        private static readonly Token symModToken = new OperatorToken(TokenKind.Mod, "%", 5);
        private static readonly Token symModEqualToken = new SymbolToken(TokenKind.ModEqual, "%=");
        private static readonly Token symLeftShiftToken = new OperatorToken(TokenKind.LeftShift, "<<", 3);
        private static readonly Token symLeftShiftEqualToken = new SymbolToken(TokenKind.LeftShiftEqual, "<<=");
        private static readonly Token symRightShiftToken = new OperatorToken(TokenKind.RightShift, ">>", 3);
        private static readonly Token symRightShiftEqualToken = new SymbolToken(TokenKind.RightShiftEqual, ">>=");
        private static readonly Token symBitwiseAndToken = new OperatorToken(TokenKind.BitwiseAnd, "&", 2);
        private static readonly Token symBitwiseAndEqualToken = new SymbolToken(TokenKind.BitwiseAndEqual, "&=");
        private static readonly Token symBitwiseOrToken = new OperatorToken(TokenKind.BitwiseOr, "|", 0);
        private static readonly Token symBitwiseOrEqualToken = new SymbolToken(TokenKind.BitwiseOrEqual, "|=");
        private static readonly Token symExclusiveOrToken = new OperatorToken(TokenKind.ExclusiveOr, "^", 1);
        private static readonly Token symExclusiveOrEqualToken = new SymbolToken(TokenKind.ExclusiveOrEqual, "^=");
        private static readonly Token symLessThanToken = new OperatorToken(TokenKind.LessThan, "<", -1);
        private static readonly Token symGreaterThanToken = new OperatorToken(TokenKind.GreaterThan, ">", -1);
        private static readonly Token symLessThanOrEqualToken = new OperatorToken(TokenKind.LessThanOrEqual, "<=", -1);
        private static readonly Token symGreaterThanOrEqualToken = new OperatorToken(TokenKind.GreaterThanOrEqual, ">=", -1);
        private static readonly Token symEqualsToken = new OperatorToken(TokenKind.Equals, "==", -1);
        private static readonly Token symNotEqualsToken = new OperatorToken(TokenKind.NotEquals, "!=", -1);
        private static readonly Token symLessThanGreaterThanToken = new SymbolToken(TokenKind.LessThanGreaterThan, "<>");
        private static readonly Token symLeftParenthesisToken = new SymbolToken(TokenKind.LeftParenthesis, "(");
        private static readonly Token symRightParenthesisToken = new SymbolToken(TokenKind.RightParenthesis, ")");
        private static readonly Token symLeftBracketToken = new SymbolToken(TokenKind.LeftBracket, "[");
        private static readonly Token symRightBracketToken = new SymbolToken(TokenKind.RightBracket, "]");
        private static readonly Token symLeftBraceToken = new SymbolToken(TokenKind.LeftBrace, "{");
        private static readonly Token symRightBraceToken = new SymbolToken(TokenKind.RightBrace, "}");
        private static readonly Token symCommaToken = new SymbolToken(TokenKind.Comma, ",");
        private static readonly Token symColonToken = new SymbolToken(TokenKind.Colon, ":");
        private static readonly Token symBackQuoteToken = new SymbolToken(TokenKind.BackQuote, "`");
        private static readonly Token symSemicolonToken = new SymbolToken(TokenKind.Semicolon, ";");
        private static readonly Token symAssignToken = new SymbolToken(TokenKind.Assign, "=");
        private static readonly Token symTwiddleToken = new SymbolToken(TokenKind.Twiddle, "~");
        private static readonly Token symAtToken = new SymbolToken(TokenKind.At, "@");

        public static Token AddToken {
            get { return symAddToken; }
        }

        public static Token AddEqualToken {
            get { return symAddEqualToken; }
        }

        public static Token SubtractToken {
            get { return symSubtractToken; }
        }

        public static Token SubtractEqualToken {
            get { return symSubtractEqualToken; }
        }

        public static Token PowerToken {
            get { return symPowerToken; }
        }

        public static Token PowerEqualToken {
            get { return symPowerEqualToken; }
        }

        public static Token MultiplyToken {
            get { return symMultiplyToken; }
        }

        public static Token MultiplyEqualToken {
            get { return symMultiplyEqualToken; }
        }

        public static Token FloorDivideToken {
            get { return symFloorDivideToken; }
        }

        public static Token FloorDivideEqualToken {
            get { return symFloorDivideEqualToken; }
        }

        public static Token DivideToken {
            get { return symDivideToken; }
        }

        public static Token DivideEqualToken {
            get { return symDivideEqualToken; }
        }

        public static Token ModToken {
            get { return symModToken; }
        }

        public static Token ModEqualToken {
            get { return symModEqualToken; }
        }

        public static Token LeftShiftToken {
            get { return symLeftShiftToken; }
        }

        public static Token LeftShiftEqualToken {
            get { return symLeftShiftEqualToken; }
        }

        public static Token RightShiftToken {
            get { return symRightShiftToken; }
        }

        public static Token RightShiftEqualToken {
            get { return symRightShiftEqualToken; }
        }

        public static Token BitwiseAndToken {
            get { return symBitwiseAndToken; }
        }

        public static Token BitwiseAndEqualToken {
            get { return symBitwiseAndEqualToken; }
        }

        public static Token BitwiseOrToken {
            get { return symBitwiseOrToken; }
        }

        public static Token BitwiseOrEqualToken {
            get { return symBitwiseOrEqualToken; }
        }

        public static Token ExclusiveOrToken {
            get { return symExclusiveOrToken; }
        }

        public static Token ExclusiveOrEqualToken {
            get { return symExclusiveOrEqualToken; }
        }

        public static Token LessThanToken {
            get { return symLessThanToken; }
        }

        public static Token GreaterThanToken {
            get { return symGreaterThanToken; }
        }

        public static Token LessThanOrEqualToken {
            get { return symLessThanOrEqualToken; }
        }

        public static Token GreaterThanOrEqualToken {
            get { return symGreaterThanOrEqualToken; }
        }

        public static Token EqualsToken {
            get { return symEqualsToken; }
        }

        public static Token NotEqualsToken {
            get { return symNotEqualsToken; }
        }

        public static Token LessThanGreaterThanToken {
            get { return symLessThanGreaterThanToken; }
        }

        public static Token LeftParenthesisToken {
            get { return symLeftParenthesisToken; }
        }

        public static Token RightParenthesisToken {
            get { return symRightParenthesisToken; }
        }

        public static Token LeftBracketToken {
            get { return symLeftBracketToken; }
        }

        public static Token RightBracketToken {
            get { return symRightBracketToken; }
        }

        public static Token LeftBraceToken {
            get { return symLeftBraceToken; }
        }

        public static Token RightBraceToken {
            get { return symRightBraceToken; }
        }

        public static Token CommaToken {
            get { return symCommaToken; }
        }

        public static Token ColonToken {
            get { return symColonToken; }
        }

        public static Token BackQuoteToken {
            get { return symBackQuoteToken; }
        }

        public static Token SemicolonToken {
            get { return symSemicolonToken; }
        }

        public static Token AssignToken {
            get { return symAssignToken; }
        }

        public static Token TwiddleToken {
            get { return symTwiddleToken; }
        }

        public static Token AtToken {
            get { return symAtToken; }
        }

        private static readonly Token kwAndToken = new SymbolToken(TokenKind.KeywordAnd, "and");
        private static readonly Token kwAsToken = new SymbolToken(TokenKind.KeywordAs, "as");
        private static readonly Token kwAssertToken = new SymbolToken(TokenKind.KeywordAssert, "assert");
        private static readonly Token kwBreakToken = new SymbolToken(TokenKind.KeywordBreak, "break");
        private static readonly Token kwClassToken = new SymbolToken(TokenKind.KeywordClass, "class");
        private static readonly Token kwContinueToken = new SymbolToken(TokenKind.KeywordContinue, "continue");
        private static readonly Token kwDefToken = new SymbolToken(TokenKind.KeywordDef, "def");
        private static readonly Token kwDelToken = new SymbolToken(TokenKind.KeywordDel, "del");
        private static readonly Token kwElseIfToken = new SymbolToken(TokenKind.KeywordElseIf, "elif");
        private static readonly Token kwElseToken = new SymbolToken(TokenKind.KeywordElse, "else");
        private static readonly Token kwExceptToken = new SymbolToken(TokenKind.KeywordExcept, "except");
        private static readonly Token kwExecToken = new SymbolToken(TokenKind.KeywordExec, "exec");
        private static readonly Token kwFinallyToken = new SymbolToken(TokenKind.KeywordFinally, "finally");
        private static readonly Token kwForToken = new SymbolToken(TokenKind.KeywordFor, "for");
        private static readonly Token kwFromToken = new SymbolToken(TokenKind.KeywordFrom, "from");
        private static readonly Token kwGlobalToken = new SymbolToken(TokenKind.KeywordGlobal, "global");
        private static readonly Token kwIfToken = new SymbolToken(TokenKind.KeywordIf, "if");
        private static readonly Token kwImportToken = new SymbolToken(TokenKind.KeywordImport, "import");
        private static readonly Token kwInToken = new SymbolToken(TokenKind.KeywordIn, "in");
        private static readonly Token kwIsToken = new SymbolToken(TokenKind.KeywordIs, "is");
        private static readonly Token kwLambdaToken = new SymbolToken(TokenKind.KeywordLambda, "lambda");
        private static readonly Token kwNotToken = new SymbolToken(TokenKind.KeywordNot, "not");
        private static readonly Token kwOrToken = new SymbolToken(TokenKind.KeywordOr, "or");
        private static readonly Token kwPassToken = new SymbolToken(TokenKind.KeywordPass, "pass");
        private static readonly Token kwPrintToken = new SymbolToken(TokenKind.KeywordPrint, "print");
        private static readonly Token kwRaiseToken = new SymbolToken(TokenKind.KeywordRaise, "raise");
        private static readonly Token kwReturnToken = new SymbolToken(TokenKind.KeywordReturn, "return");
        private static readonly Token kwTryToken = new SymbolToken(TokenKind.KeywordTry, "try");
        private static readonly Token kwWhileToken = new SymbolToken(TokenKind.KeywordWhile, "while");
        private static readonly Token kwWithToken = new SymbolToken(TokenKind.KeywordWith, "with");
        private static readonly Token kwYieldToken = new SymbolToken(TokenKind.KeywordYield, "yield");


        public static Token KeywordAndToken {
            get { return kwAndToken; }
        }

        public static Token KeywordAsToken {
            get { return kwAsToken; }
        }

        public static Token KeywordAssertToken {
            get { return kwAssertToken; }
        }

        public static Token KeywordBreakToken {
            get { return kwBreakToken; }
        }

        public static Token KeywordClassToken {
            get { return kwClassToken; }
        }

        public static Token KeywordContinueToken {
            get { return kwContinueToken; }
        }

        public static Token KeywordDefToken {
            get { return kwDefToken; }
        }

        public static Token KeywordDelToken {
            get { return kwDelToken; }
        }

        public static Token KeywordElseIfToken {
            get { return kwElseIfToken; }
        }

        public static Token KeywordElseToken {
            get { return kwElseToken; }
        }

        public static Token KeywordExceptToken {
            get { return kwExceptToken; }
        }

        public static Token KeywordExecToken {
            get { return kwExecToken; }
        }

        public static Token KeywordFinallyToken {
            get { return kwFinallyToken; }
        }

        public static Token KeywordForToken {
            get { return kwForToken; }
        }

        public static Token KeywordFromToken {
            get { return kwFromToken; }
        }

        public static Token KeywordGlobalToken {
            get { return kwGlobalToken; }
        }

        public static Token KeywordIfToken {
            get { return kwIfToken; }
        }

        public static Token KeywordImportToken {
            get { return kwImportToken; }
        }

        public static Token KeywordInToken {
            get { return kwInToken; }
        }

        public static Token KeywordIsToken {
            get { return kwIsToken; }
        }

        public static Token KeywordLambdaToken {
            get { return kwLambdaToken; }
        }

        public static Token KeywordNotToken {
            get { return kwNotToken; }
        }

        public static Token KeywordOrToken {
            get { return kwOrToken; }
        }

        public static Token KeywordPassToken {
            get { return kwPassToken; }
        }

        public static Token KeywordPrintToken {
            get { return kwPrintToken; }
        }

        public static Token KeywordRaiseToken {
            get { return kwRaiseToken; }
        }

        public static Token KeywordReturnToken {
            get { return kwReturnToken; }
        }

        public static Token KeywordTryToken {
            get { return kwTryToken; }
        }

        public static Token KeywordWhileToken {
            get { return kwWhileToken; }
        }

        public static Token KeywordWithToken {
            get { return kwWithToken; }
        }

        public static Token KeywordYieldToken {
            get { return kwYieldToken; }
        }



        // *** END GENERATED CODE ***

        #endregion
    }
}
