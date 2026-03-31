using AnZwDev.AL.Symbols;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.AL.Symbols.Providers.SourceCode.SymbolsCompilers.DataTypeCompilers
{
    internal class PageActionChangeKindCompiler
    {

        public PageActionChangeKind Compile(SyntaxToken syntax)
        {
            var keyword = syntax.Text;

            if (keyword != null)
            {
                keyword = keyword.Trim().ToLower();

                switch (keyword?.ToLower())
                {
                    case "add":
                        return PageActionChangeKind.Add;
                    case "addfirst":
                        return PageActionChangeKind.AddFirst;
                    case "addlast":
                        return PageActionChangeKind.AddLast;
                    case "addbefore":
                        return PageActionChangeKind.AddBefore;
                    case "addafter":
                        return PageActionChangeKind.AddAfter;
                    case "movefirst":
                        return PageActionChangeKind.MoveFirst;
                    case "movelast":
                        return PageActionChangeKind.MoveLast;
                    case "movebefore":
                        return PageActionChangeKind.MoveBefore;
                    case "moveafter":
                        return PageActionChangeKind.MoveAfter;
                    case "modify":
                        return PageActionChangeKind.Modify;
                }
            }

            return PageActionChangeKind.Undefined;
        }


    }
}
