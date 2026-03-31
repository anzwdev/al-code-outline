using AnZwDev.AL.CodeAnalysis.Extensions;
using AnZwDev.AL.Symbols;
using AnZwDev.AL.Symbols.Providers.SourceCode;
using AnZwDev.AL.Syntax;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.AL.CodeAnalysis.Sorting
{
    public class MethodSortInfo<T> where T : MemberSyntax
    {
        public string Name { get; set; }
        public MemberKind Kind { get; set; }
        public int Index { get; set; }
        public T Node { get; set; }

        public MethodSortInfo(T node, int index)
        {
            Node = node;
            Index = index;
            Name = node.GetNameStringValue() ?? String.Empty;
            Kind = SourceCodeSymbolsCompiler.CompileMemberKind(node);
        }

        public static List<MethodSortInfo<T>> FromSyntaxList(SyntaxList<T> nodeList)
        {
            List<MethodSortInfo<T>> list = new List<MethodSortInfo<T>>();
            for (int i = 0; i < nodeList.Count; i++)
                list.Add(new MethodSortInfo<T>(nodeList[i], i));
            return list;
        }

        public static List<MethodSortInfo<T>> FromNodesList(List<T> nodeList)
        {
            List<MethodSortInfo<T>> list = new List<MethodSortInfo<T>>();
            for (int i = 0; i < nodeList.Count; i++)
                list.Add(new MethodSortInfo<T>(nodeList[i], i));
            return list;
        }

        public static SyntaxList<T> ToSyntaxList(List<MethodSortInfo<T>> sortInfoList)
        {
            List<T> nodeList = new List<T>();
            for (int i = 0; i < sortInfoList.Count; i++)
                nodeList.Add(sortInfoList[i].Node);
            return SyntaxFactory.List(nodeList);
        }

        public static List<T> ToNodesList(List<MethodSortInfo<T>> sortInfoList)
        {
            List<T> nodeList = new List<T>();
            for (int i = 0; i < sortInfoList.Count; i++)
                nodeList.Add(sortInfoList[i].Node);
            return nodeList;
        }

    }
}
