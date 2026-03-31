using AnZwDev.AL.Symbols;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.AL.Symbols.Providers.SourceCode.SymbolsCompilers.DataTypeCompilers
{
    internal class PageControlChangeKindCompiler
    {

        public PageControlChangeKind Compile(SyntaxToken syntax)
        {
            var keyword = syntax.Text;

            if (keyword != null)
            {
                keyword = keyword.Trim().ToLower();

                switch (keyword?.ToLower())
                {
                    case "add":
                        return PageControlChangeKind.Add;
                    case "addfirst":
                        return PageControlChangeKind.AddFirst;
                    case "addlast":
                        return PageControlChangeKind.AddLast;
                    case "addbefore":
                        return PageControlChangeKind.AddBefore;
                    case "addafter":
                        return PageControlChangeKind.AddAfter;
                    case "movefirst":
                        return PageControlChangeKind.MoveFirst;
                    case "movelast":
                        return PageControlChangeKind.MoveLast;
                    case "movebefore":
                        return PageControlChangeKind.MoveBefore;
                    case "moveafter":
                        return PageControlChangeKind.MoveAfter;
                    case "modify":
                        return PageControlChangeKind.Modify;
                }
            }

            return PageControlChangeKind.Undefined;
        }

    }
}
