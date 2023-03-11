using AnZwDev.ALTools.ALSymbols.Internal;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.Extensions
{
    public static class SyntaxTriviaListExtensions
    {

        public static SyntaxTriviaList NormalizeSyntaxTriviaList(this SyntaxTriviaList triviaList)
        {
            List<SyntaxTrivia> newList = new List<SyntaxTrivia>();

            for (int triviaIdx = 0; triviaIdx < triviaList.Count; triviaIdx++)
            {
                SyntaxTrivia trivia = triviaList[triviaIdx];
                bool addTrivia = true;
                switch (trivia.Kind.ConvertToLocalType())
                {
                    case ConvertedSyntaxKind.WhiteSpaceTrivia:
                        addTrivia = (triviaIdx == (triviaList.Count - 1)) ||
                            (triviaList[triviaIdx + 1].Kind.ConvertToLocalType() != ConvertedSyntaxKind.EndOfLineTrivia);
                        break;
                    case ConvertedSyntaxKind.EndOfLineTrivia:
                        addTrivia = (newList.Count == 0) ||
                            (newList[newList.Count - 1].Kind.ConvertToLocalType() != ConvertedSyntaxKind.EndOfLineTrivia);
                        break;
                }
                if (addTrivia)
                    newList.Add(triviaList[triviaIdx]);
            }
            return SyntaxFactory.TriviaList(newList);
        }

        public static List<SyntaxTrivia> RemoveEmptyLines(this IEnumerable<SyntaxTrivia> triviaList)
        {
            List<SyntaxTrivia> newList = new List<SyntaxTrivia>();
            bool validLine = true;
            int lastValidPos = -1;

            foreach (SyntaxTrivia trivia in triviaList)
            {
                //add trivia to the list
                newList.Add(trivia);
                
                ConvertedSyntaxKind kind = trivia.Kind.ConvertToLocalType();

                if (kind == ConvertedSyntaxKind.EndOfLineTrivia)
                {
                    //if valid line, then remember this trivia as last valid position
                    //if invalid line, remove elements after last valid position
                    if (validLine)
                        lastValidPos = newList.Count - 1;
                    else
                        newList.RemoveRange(lastValidPos + 1, newList.Count - lastValidPos - 1);
                    validLine = false;
                } 
                else if (kind != ConvertedSyntaxKind.WhiteSpaceTrivia)
                    validLine = true;

                if (validLine)
                    lastValidPos = newList.Count - 1;
            }

            return newList;
        }

        public static List<SyntaxTrivia> FirstLineOnly(this IEnumerable<SyntaxTrivia> triviaList, bool includeEndOfLine)
        {           
            List<SyntaxTrivia> newTriviaList = new List<SyntaxTrivia>();
            foreach (SyntaxTrivia trivia in triviaList)
            {
                bool endOfLine = (trivia.Kind.ConvertToLocalType() == ConvertedSyntaxKind.EndOfLineTrivia);

                if ((!endOfLine) || (includeEndOfLine))
                    newTriviaList.Add(trivia);

                if (endOfLine)
                    return newTriviaList;
            }
            return newTriviaList;
        }

        public static bool IsNullOrWhiteSpace(this IEnumerable<SyntaxTrivia> triviaList)
        {
            if (triviaList == null)
                return true;
            foreach (SyntaxTrivia trivia in triviaList)
            {
                ConvertedSyntaxKind kind = trivia.Kind.ConvertToLocalType();
                if ((kind != ConvertedSyntaxKind.WhiteSpaceTrivia) && (kind != ConvertedSyntaxKind.EndOfLineTrivia))
                    return false;
            }
            return true;
        }

        public static bool HasNewLine(this IEnumerable<SyntaxTrivia> triviaList)
        {
            if (triviaList == null)
                return false;
            foreach (SyntaxTrivia trivia in triviaList)
            {
                if (trivia.Kind.ConvertToLocalType() == ConvertedSyntaxKind.EndOfLineTrivia)
                    return true;
            }
            return false;
        }


        public static bool ContainsDirectives(this IEnumerable<SyntaxTrivia> triviaList)
        {
            if (triviaList == null)
                return false;
            foreach (SyntaxTrivia trivia in triviaList)
            {
                ConvertedSyntaxKind kind = trivia.Kind.ConvertToLocalType();
                switch (kind)
                {
                    case ConvertedSyntaxKind.BadDirectiveTrivia:
                    case ConvertedSyntaxKind.BadPragmaDirectiveTrivia:
                    case ConvertedSyntaxKind.DefineDirectiveTrivia:
                    case ConvertedSyntaxKind.ElifDirectiveTrivia:
                    case ConvertedSyntaxKind.ElseDirectiveTrivia:
                    case ConvertedSyntaxKind.EndRegionDirectiveTrivia:
                    case ConvertedSyntaxKind.IfDirectiveTrivia:
                    case ConvertedSyntaxKind.PragmaImplicitWithDirectiveTrivia:
                    case ConvertedSyntaxKind.PragmaWarningDirectiveTrivia:
                    case ConvertedSyntaxKind.PreprocessingMessageTrivia:
                    case ConvertedSyntaxKind.RegionDirectiveTrivia:
                    case ConvertedSyntaxKind.UndefDirectiveTrivia:
                        return true;
                }
            }
            return false;
        }

    }
}
