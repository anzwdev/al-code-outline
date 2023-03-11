using AnZwDev.ALTools.ALSymbols;
using AnZwDev.ALTools.ALSymbols.Internal;
using AnZwDev.ALTools.Extensions;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace AnZwDev.ALTools.CodeTransformations
{
    public class SyntaxNodeSortInfo<T> where T: SyntaxNode
    {

        public string Name { get; }
        public int Index { get; }
        internal ConvertedSyntaxKind Kind { get; }
        public T Node { get; set; }

        public static List<SyntaxNodeSortInfo<T>> FromSyntaxList(SyntaxList<T> syntaxList)
        {
            List<SyntaxNodeSortInfo<T>> list = new List<SyntaxNodeSortInfo<T>>();
            for (int i=0; i<syntaxList.Count; i++)
            {
                list.Add(new SyntaxNodeSortInfo<T>(syntaxList[i], i));
            }
            return list;
        }

        public static SyntaxList<T> ToSyntaxList(List<SyntaxNodeSortInfo<T>> sortInfoList)
        {
            List<T> list = new List<T>();
            for (int i=0; i<sortInfoList.Count; i++)
            {
                list.Add(sortInfoList[i].Node);
            }
            return SyntaxFactory.List<T>(list);
        }

        public static List<SyntaxNodeSortInfo<T>> FromNodesList(List<T> syntaxList)
        {
            List<SyntaxNodeSortInfo<T>> list = new List<SyntaxNodeSortInfo<T>>();
            for (int i = 0; i < syntaxList.Count; i++)
            {
                list.Add(new SyntaxNodeSortInfo<T>(syntaxList[i], i));
            }
            return list;
        }

        public static List<T> ToNodesList(List<SyntaxNodeSortInfo<T>> sortInfoList)
        {
            List<T> list = new List<T>();
            for (int i = 0; i < sortInfoList.Count; i++)
            {
                list.Add(sortInfoList[i].Node);
            }
            return list;
        }

        public SyntaxNodeSortInfo(T node, int index)
        {
            this.Index = index;
            this.Node = node;
            this.Kind = node.Kind.ConvertToLocalType();
            this.Name = this.Node.GetSyntaxNodeName().NotNull();
        }

    }
}
