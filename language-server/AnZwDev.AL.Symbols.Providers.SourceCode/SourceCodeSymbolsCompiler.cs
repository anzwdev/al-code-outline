using AnZwDev.AL.Symbols.Providers.SourceCode.SymbolsCompilers.DataTypeCompilers;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.AL.Symbols.Providers.SourceCode
{
    public static class SourceCodeSymbolsCompiler
    {

        private static readonly MemberKindCompiler _memberKindCompiler = new MemberKindCompiler();
        public static MemberKind CompileMemberKind(MemberSyntax memberSyntax)
        {
            return _memberKindCompiler.Compile(memberSyntax);
        }

        private static readonly PageActionChangeKindCompiler _pageActionChangeKindCompiler = new PageActionChangeKindCompiler();
        public static PageActionChangeKind CompilePageActionChangeKind(SyntaxToken syntaxToken)
        {
            return _pageActionChangeKindCompiler.Compile(syntaxToken);
        }

        private static readonly PageControlChangeKindCompiler _pageControlChangeKindCompiler = new PageControlChangeKindCompiler();
        public static PageControlChangeKind CompilePageControlChangeKind(SyntaxToken syntaxToken)
        {
            return _pageControlChangeKindCompiler.Compile(syntaxToken);
        }

        private static readonly PageViewChangeKindCompiler _pageViewChangeKindCompiler = new PageViewChangeKindCompiler();
        public static PageViewChangeKind CompilePageViewChangeKind(SyntaxToken syntaxToken)
        {
            return _pageViewChangeKindCompiler.Compile(syntaxToken);
        }

    }
}
