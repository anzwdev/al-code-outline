using AnZwDev.AL.Symbols;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.AL.Symbols.Providers.SourceCode.SymbolsCompilers.DataTypeCompilers
{
    internal class PageViewChangeKindCompiler
    {

        public PageViewChangeKind Compile(SyntaxToken syntax)
        {
            var keyword = syntax.Text;

            if (keyword != null)
            {
                keyword = keyword.Trim().ToLower();

                switch (keyword?.ToLower())
                {
                    case "add":
                        return PageViewChangeKind.Add;
                    case "addfirst":
                        return PageViewChangeKind.AddFirst;
                    case "addlast":
                        return PageViewChangeKind.AddLast;
                    case "addbefore":
                        return PageViewChangeKind.AddBefore;
                    case "addafter":
                        return PageViewChangeKind.AddAfter;
                    case "movefirst":
                        return PageViewChangeKind.MoveFirst;
                    case "movelast":
                        return PageViewChangeKind.MoveLast;
                    case "movebefore":
                        return PageViewChangeKind.MoveBefore;
                    case "moveafter":
                        return PageViewChangeKind.MoveAfter;
                    case "modify":
                        return PageViewChangeKind.Modify;
                }
            }

            return PageViewChangeKind.Undefined;
        }


    }
}
