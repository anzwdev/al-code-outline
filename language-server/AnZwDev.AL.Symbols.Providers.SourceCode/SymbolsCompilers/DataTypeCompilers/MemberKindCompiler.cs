using AnZwDev.AL.Symbols.Parsing;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Xml.Linq;

namespace AnZwDev.AL.Symbols.Providers.SourceCode.SymbolsCompilers.DataTypeCompilers
{
    internal class MemberKindCompiler
    {

        public MemberKind Compile(MemberSyntax memberSyntax)
        {
            switch (memberSyntax.Kind)
            {
                case SyntaxKind.TriggerDeclaration:
                    return MemberKind.TriggerDeclaration;
                case SyntaxKind.MethodDeclaration:
                    var methodSyntax = memberSyntax as MethodDeclarationSyntax;
                    if (methodSyntax != null)
                        return Compile(methodSyntax.Attributes);
                    break;
                case SyntaxKind.GlobalVarSection:
                    return MemberKind.GlobalVarSection;
                case SyntaxKind.VarSection:
                    return MemberKind.GlobalVarSection;
            }
            return MemberKind.Undefined;
        }

        private MemberKind Compile(SyntaxList<MemberAttributeSyntax> memberAttributes)
        {
            foreach (MemberAttributeSyntax att in memberAttributes)
            {
                var memberKind = ALSymbolExpressionParser.ParseMemberKind(att.GetNameStringValue());
                if (memberKind != MemberKind.Undefined)
                    return memberKind;
            }
            return MemberKind.Undefined;
        }

    }
}
