using AnZwDev.ALTools.ALSymbols.Internal;
using AnZwDev.ALTools.CodeTransformations;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.Extensions
{
    public static class List_SyntaxNode_Extensions
    {

        public static bool SortWithTrivia<T>(this List<T> list, IComparer<T> comparer) where T : SyntaxNode
        {
            if (list.Count <= 1)
                return false;

            bool sorted = false;
            bool listHasLeadingNewLine;
            bool listHasTrailingNewLine;
            SyntaxTrivia listLeadingTrivia;

            SortWithTriviaMoveTrivia(list, out listHasLeadingNewLine, out listHasTrailingNewLine, out listLeadingTrivia);
            sorted = !list.IsOrdered(comparer);
            list.Sort(comparer);
            SortWithTriviaRestoreTrivia(list, listHasLeadingNewLine, listHasTrailingNewLine, listLeadingTrivia);

            return sorted;
        }

        public static bool SortWithTrivia<T>(this List<SyntaxNodeSortInfo<T>> list, IComparer<SyntaxNodeSortInfo<T>> comparer) where T : SyntaxNode
        {
            if (list.Count <= 1)
                return false;

            bool sorted = false;
            bool listHasLeadingNewLine;
            bool listHasTrailingNewLine;
            SyntaxTrivia listLeadingTrivia;

            IList<T> syntaxNodeList = new SyntaxNodeSortInfoListSyntaxNodeProvider<T>(list);
            SortWithTriviaMoveTrivia(syntaxNodeList, out listHasLeadingNewLine, out listHasTrailingNewLine, out listLeadingTrivia);
            sorted = !list.IsOrdered(comparer);
            list.Sort(comparer);
            SortWithTriviaRestoreTrivia(syntaxNodeList, listHasLeadingNewLine, listHasTrailingNewLine, listLeadingTrivia);

            return sorted;
        }

        private static void SortWithTriviaMoveTrivia<T>(IList<T> list, out bool listHasLeadingNewLine, out bool listHasTrailingNewLine, out SyntaxTrivia listLeadingTrivia) where T : SyntaxNode
        {
            //create new line trivia
            SyntaxTrivia newLineTrivia = SyntaxFactory.ParseTrailingTrivia("\r\n", 0)[0]; //SyntaxFactory.EndOfLine(""));

            //check first node
            listLeadingTrivia = default(SyntaxTrivia);
            SyntaxTriviaList leadingTrivias = list[0].GetLeadingTrivia();
            listHasLeadingNewLine = ((leadingTrivias.Count > 0) && (leadingTrivias[0].Kind.ConvertToLocalType() == ConvertedSyntaxKind.EndOfLineTrivia));
            if (listHasLeadingNewLine)
            {
                listLeadingTrivia = leadingTrivias[0];
                leadingTrivias = leadingTrivias.RemoveAt(0);
                list[0] = list[0].WithLeadingTrivia(leadingTrivias);
            }

            //check last node
            SyntaxTriviaList trailingTrivias = list[list.Count - 1].GetTrailingTrivia();
            listHasTrailingNewLine = ((trailingTrivias.Count > 0) && (trailingTrivias[trailingTrivias.Count - 1].Kind.ConvertToLocalType() == ConvertedSyntaxKind.EndOfLineTrivia));
            if (!listHasTrailingNewLine)
            {
                trailingTrivias = trailingTrivias.Add(newLineTrivia);
                list[list.Count - 1] = list[list.Count - 1].WithTrailingTrivia(trailingTrivias);
            }

            for (int i = 0; i < (list.Count - 1); i++)
            {
                //check if we should move new line trivia
                trailingTrivias = list[i].GetTrailingTrivia();
                leadingTrivias = list[i + 1].GetLeadingTrivia();

                bool trailingNewLine = ((trailingTrivias.Count > 0) && (trailingTrivias[trailingTrivias.Count - 1].Kind.ConvertToLocalType() == ConvertedSyntaxKind.EndOfLineTrivia));
                bool leadingNewLine = ((leadingTrivias.Count > 0) && (leadingTrivias[0].Kind.ConvertToLocalType() == ConvertedSyntaxKind.EndOfLineTrivia));

                if (!trailingNewLine)
                {
                    if (leadingNewLine)
                    {
                        trailingTrivias = trailingTrivias.Add(leadingTrivias[0]);
                        leadingTrivias = leadingTrivias.RemoveAt(0);
                        list[i + 1] = list[i + 1].WithLeadingTrivia(leadingTrivias);
                    }
                    else
                        trailingTrivias = trailingTrivias.Add(newLineTrivia); //SyntaxFactory.EndOfLine(""));

                    list[i] = list[i].WithTrailingTrivia(trailingTrivias);
                }
            }
        }

        private static void SortWithTriviaRestoreTrivia<T>(IList<T> list, bool listHasLeadingNewLine, bool listHasTrailingNewLine, SyntaxTrivia listLeadingTrivia) where T : SyntaxNode
        {
            //restore trivias
            if (listHasLeadingNewLine)
                list[0] = list[0].WithLeadingTrivia(list[0].GetLeadingTrivia().Insert(0, listLeadingTrivia));
            if (!listHasTrailingNewLine)
            {
                SyntaxTriviaList trailingTrivias = list[list.Count - 1].GetTrailingTrivia();
                if ((trailingTrivias.Count > 0) && (trailingTrivias[trailingTrivias.Count - 1].Kind.ConvertToLocalType() == ConvertedSyntaxKind.EndOfLineTrivia))
                {
                    trailingTrivias = trailingTrivias.RemoveAt(trailingTrivias.Count - 1);
                    list[list.Count - 1] = list[list.Count - 1].WithTrailingTrivia(trailingTrivias);
                }
            }
        }


    }
}
