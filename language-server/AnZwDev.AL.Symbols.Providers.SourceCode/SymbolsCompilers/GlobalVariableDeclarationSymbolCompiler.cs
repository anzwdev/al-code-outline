using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnZwDev.AL.Symbols;
using AnZwDev.System.Extensions;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;

namespace AnZwDev.AL.Symbols.Providers.SourceCode.SymbolsCompilers
{
    internal static class GlobalVariableDeclarationSymbolCompiler
    {

        public static void Compile(GlobalVarSectionSyntax syntax, List<GlobalVariableDeclarationSymbol> variables)
        {
            var modifier = syntax.AccessModifier.Text;
            var protectedModifier = (modifier != null) && (modifier.Equals("protected", StringComparison.OrdinalIgnoreCase));

            for (int i = 0; i < syntax.Variables.Count; i++)
                Compile(syntax.Variables[i], protectedModifier, variables);
        }

        public static void Compile(VariableDeclarationBaseSyntax syntax, bool protectedModifier, List<GlobalVariableDeclarationSymbol> variables)
        {
            switch (syntax)
            {
                case VariableDeclarationSyntax variableDeclaration:
                    Compile(variableDeclaration, protectedModifier, variables);
                    break;
                case VariableListDeclarationSyntax variableListDeclarationSyntax:
                    Compile(variableListDeclarationSyntax, protectedModifier, variables);
                    break;
            }
        }

        private static void Compile(VariableDeclarationSyntax syntax, bool protectedModifier, List<GlobalVariableDeclarationSymbol> variables)
        {
            variables.Add(new GlobalVariableDeclarationSymbol()
            {
                Protected = protectedModifier,
                Attributes = AttributeSymbolCompiler.CompileList(syntax.Attributes),
                Name = NameCompiler.Compile(syntax.Name).NotNull(),
                TypeDefinition = TypeDefinitionSymbolCompiler.Compile(syntax.Type)
            });
        }

        private static void Compile(VariableListDeclarationSyntax syntax, bool protectedModifier, List<GlobalVariableDeclarationSymbol> variables)
        {
            var typeDefinition = TypeDefinitionSymbolCompiler.Compile(syntax.Type);
            var attributes = AttributeSymbolCompiler.CompileList(syntax.Attributes);
            for (int i=0; i<syntax.VariableNames.Count; i++)
            {
                var name = NameCompiler.Compile(syntax.VariableNames[i].Name).NotNull();
                variables.Add(new GlobalVariableDeclarationSymbol()
                {
                    Protected = protectedModifier,
                    Attributes = attributes,
                    Name = name,
                    TypeDefinition = typeDefinition
                });
            }
        }

    }
}
