using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.AL.CodeAnalysis.Extensions
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
                switch (trivia.Kind)
                {
                    case SyntaxKind.WhiteSpaceTrivia:
                        addTrivia = (triviaIdx == (triviaList.Count - 1)) ||
                            (triviaList[triviaIdx + 1].Kind != SyntaxKind.EndOfLineTrivia);
                        break;
                    case SyntaxKind.EndOfLineTrivia:
                        addTrivia = (newList.Count == 0) ||
                            (newList[newList.Count - 1].Kind != SyntaxKind.EndOfLineTrivia);
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

                if (trivia.Kind == SyntaxKind.EndOfLineTrivia)
                {
                    //if valid line, then remember this trivia as last valid position
                    //if invalid line, remove elements after last valid position
                    if (validLine)
                        lastValidPos = newList.Count - 1;
                    else
                        newList.RemoveRange(lastValidPos + 1, newList.Count - lastValidPos - 1);
                    validLine = false;
                }
                else if (trivia.Kind != SyntaxKind.WhiteSpaceTrivia)
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
                bool endOfLine = (trivia.Kind == SyntaxKind.EndOfLineTrivia);

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
                if ((trivia.Kind != SyntaxKind.WhiteSpaceTrivia) && (trivia.Kind != SyntaxKind.EndOfLineTrivia))
                    return false;
            return true;
        }

        public static bool HasNewLine(this IEnumerable<SyntaxTrivia> triviaList)
        {
            if (triviaList == null)
                return false;
            foreach (SyntaxTrivia trivia in triviaList)
                if (trivia.Kind == SyntaxKind.EndOfLineTrivia)
                    return true;
            return false;
        }

        public static bool EndsWithNewLine(this SyntaxTriviaList triviaList)
        {
            if (triviaList.Count > 0)
                return triviaList[triviaList.Count - 1].Kind == SyntaxKind.EndOfLineTrivia;
            return false;
        }

        public static bool StartsWithNewLine(this SyntaxTriviaList triviaList)
        {
            if (triviaList.Count > 0)
                return triviaList[0].Kind == SyntaxKind.EndOfLineTrivia;
            return false;
        }

        public static bool OpensRegion(this IEnumerable<SyntaxTrivia> triviaList)
        {
            return (triviaList.GetRegionDepth() > 0);
        }

        public static bool ClosesRegion(this IEnumerable<SyntaxTrivia> triviaList)
        {
            return (triviaList.GetRegionDepth() < 0);
        }

        public static int GetRegionDepth(this IEnumerable<SyntaxTrivia> triviaList)
        {
            if (triviaList == null)
                return 0;

            var depth = 0;
            foreach (SyntaxTrivia trivia in triviaList)
            {
                switch (trivia.Kind)
                {
                    case SyntaxKind.RegionDirectiveTrivia:
                        depth++;
                        break;
                    case SyntaxKind.EndRegionDirectiveTrivia:
                        depth--;
                        break;
                }
            }
            return depth;
        }

        public static bool ContainsDirectives(this IEnumerable<SyntaxTrivia> triviaList)
        {
            if (triviaList == null)
                return false;
            foreach (SyntaxTrivia trivia in triviaList)
            {
                switch (trivia.Kind)
                {
                    case SyntaxKind.BadDirectiveTrivia:
                    case SyntaxKind.BadPragmaDirectiveTrivia:
                    case SyntaxKind.DefineDirectiveTrivia:
                    case SyntaxKind.ElifDirectiveTrivia:
                    case SyntaxKind.ElseDirectiveTrivia:
                    case SyntaxKind.EndRegionDirectiveTrivia:
                    case SyntaxKind.IfDirectiveTrivia:
                    case SyntaxKind.PragmaImplicitWithDirectiveTrivia:
                    case SyntaxKind.PragmaWarningDirectiveTrivia:
                    case SyntaxKind.PreprocessingMessageTrivia:
                    case SyntaxKind.RegionDirectiveTrivia:
                    case SyntaxKind.UndefDirectiveTrivia:
                        return true;
                }
            }
            return false;
        }

    }
}
