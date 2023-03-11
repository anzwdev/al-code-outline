using AnZwDev.ALTools.ALSymbols.Internal;
using AnZwDev.ALTools.Extensions;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace AnZwDev.ALTools.CodeTransformations
{
    public class DirectiveTriviaMergedList
    {

        public List<SyntaxTrivia> List { get; }
        private bool _addNewLine = true;

        public DirectiveTriviaMergedList()
        {
            List = new List<SyntaxTrivia>();
        }

        public void Clear()
        {
            List.Clear();
            _addNewLine = true;
        }

        public void AddAllExceptClosingRegions(IEnumerable<SyntaxTrivia> collection)
        {
            bool regionTriviaAdded = false;
            foreach (var trivia in collection)
            {
                var kind = trivia.Kind.ConvertToLocalType();
                var canAdd = true;
                
                switch (kind)
                {
                    case ConvertedSyntaxKind.RegionDirectiveTrivia:
                        regionTriviaAdded = true;
                        break;
                    case ConvertedSyntaxKind.EndRegionDirectiveTrivia:
                        if (!regionTriviaAdded)
                            canAdd = !RemoveLastRegion();
                        break;
                }

                if (canAdd)
                    List.Add(trivia);
            }
        }

        public void AddRange(IEnumerable<SyntaxTrivia> collection)
        {
            foreach (var trivia in collection)
            {
                var kind = trivia.Kind.ConvertToLocalType();
                if (kind == ConvertedSyntaxKind.EndOfLineTrivia)
                {
                    if (_addNewLine)
                        List.Add(trivia);
                    _addNewLine = false;
                } 
                else if (CanMerge(kind))
                {
                    List.Add(trivia);
                    _addNewLine = true;
                }
            }
        }

        private bool CanMerge(ConvertedSyntaxKind kind)
        {
            switch (kind)
            {
                case ConvertedSyntaxKind.BadDirectiveTrivia:
                case ConvertedSyntaxKind.BadPragmaDirectiveTrivia:
                case ConvertedSyntaxKind.DefineDirectiveTrivia:
                case ConvertedSyntaxKind.ElifDirectiveTrivia:
                case ConvertedSyntaxKind.ElseDirectiveTrivia:
                case ConvertedSyntaxKind.IfDirectiveTrivia:
                case ConvertedSyntaxKind.PreprocessingMessageTrivia:
                case ConvertedSyntaxKind.RegionDirectiveTrivia:
                case ConvertedSyntaxKind.UndefDirectiveTrivia:
                case ConvertedSyntaxKind.PragmaImplicitWithDirectiveTrivia:
                case ConvertedSyntaxKind.PragmaWarningDirectiveTrivia:
                    return true;
                case ConvertedSyntaxKind.EndRegionDirectiveTrivia:
                    return !RemoveLastRegion();
            }
            return false;
        }

        private bool RemoveLastRegion()
        {
            for (int i = List.Count - 1; i>=0; i--)
            {
                var kind = List[i].Kind.ConvertToLocalType();
                switch (kind)
                {
                    case ConvertedSyntaxKind.RegionDirectiveTrivia:
                        var lastValidIndex = i - 1;
                        while ((lastValidIndex >= 0) && (List[lastValidIndex].Kind.ConvertToLocalType() != ConvertedSyntaxKind.EndOfLineTrivia))
                            lastValidIndex--;
                        lastValidIndex++;
                        List.RemoveRange(lastValidIndex, List.Count - lastValidIndex);
                        return true;
                    case ConvertedSyntaxKind.WhiteSpaceTrivia:
                    case ConvertedSyntaxKind.EndOfLineTrivia:
                        break;
                    default:
                        return false;
                }
            }
            return false;
        }

    }

}
