using AnZwDev.AL.Symbols;
using AnZwDev.System.Extensions;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.AL.Symbols.Providers.SourceCode.SymbolsCompilers
{
    internal static class TypeDefinitionSymbolCompiler
    {

        public static TypeDefinitionSymbol? Compile(TypeReferenceBaseSyntax? syntax)
        {
            if (syntax != null)
            {
                var arrayDimensions = ArrayDimensionsCompiler.Compile(syntax.Array);

                switch (syntax)
                {
                    case SimpleTypeReferenceSyntax simpleTypeReferenceSyntax:
                        return Compile(simpleTypeReferenceSyntax.DataType, arrayDimensions);
                    case RecordTypeReferenceSyntax recordTypeReferenceSyntax:
                        var isTemporary = (recordTypeReferenceSyntax.Temporary.Text != null) && (recordTypeReferenceSyntax.Temporary.Text.Equals("temporary", StringComparison.OrdinalIgnoreCase));
                        return Compile(recordTypeReferenceSyntax.DataType, arrayDimensions, isTemporary);
                    case DotNetTypeReferenceSyntax dotNetTypeReferenceSyntax:
                        return Compile(dotNetTypeReferenceSyntax.DataType, arrayDimensions);
                }
            }

            return null;
        }

        private static List<TypeDefinitionSymbol>? Compile(SeparatedSyntaxList<DataTypeSyntax> syntaxList, List<int>? arrayDimensions = null, bool temporary = false)
        {
            if (syntaxList.Count == 0)
                return null;

            var list = new List<TypeDefinitionSymbol>(syntaxList.Count);
            for (int i = 0; i < syntaxList.Count; i++)
            {
                var typeDef = Compile(syntaxList[i], arrayDimensions, temporary);
                if (typeDef != null)
                    list.Add(typeDef);
            }
            return list;
        }

        public static TypeDefinitionSymbol? Compile(DataTypeSyntax syntax, List<int>? arrayDimensions = null, bool temporary = false)
        {
            switch (syntax)
            {
                case SimpleNamedDataTypeSyntax simpleNamedDataTypeSyntax:
                    return Compile(simpleNamedDataTypeSyntax, arrayDimensions, temporary);
                case LengthDataTypeSyntax lengthDataTypeSyntax:
                    return Compile(lengthDataTypeSyntax, arrayDimensions, temporary);
                case SubtypedDataTypeSyntax subtypedDataTypeSyntax:
                    return Compile(subtypedDataTypeSyntax, arrayDimensions, temporary);
                case GenericNamedDataTypeSyntax genericNamedDataTypeSyntax:
                    return Compile(genericNamedDataTypeSyntax, arrayDimensions, temporary);
                case OptionDataTypeSyntax optionDataTypeSyntax:
                    return Compile(optionDataTypeSyntax, arrayDimensions, temporary);
                case EnumDataTypeSyntax enumDataTypeSyntax:
                    return Compile(enumDataTypeSyntax, arrayDimensions, temporary);
                case LabelDataTypeSyntax labelDataTypeSyntax:
                    return Compile(labelDataTypeSyntax, arrayDimensions, temporary);
                case TextConstDataTypeSyntax textConstDataTypeSyntax:
                    return Compile(textConstDataTypeSyntax, arrayDimensions, temporary);
                case DotNetDataTypeSyntax dotNetDataTypeSyntax:
                    return Compile(dotNetDataTypeSyntax, arrayDimensions, temporary);
            }

            return null;
        }

        private static TypeDefinitionSymbol? Compile(SimpleNamedDataTypeSyntax syntax, List<int>? arrayDimensions, bool temporary)
        {
            return new TypeDefinitionSymbol()
            {
                Name = syntax.TypeName.Text.NotNull(),
                ArrayDimensions = arrayDimensions,
                Temporary = temporary,
                Subtype = null,
                OptionMembers = null,
                TypeArguments = null,
            };
        }

        private static TypeDefinitionSymbol? Compile(LengthDataTypeSyntax syntax, List<int>? arrayDimensions, bool temporary)
        {
            var name = syntax.TypeName.Text.NotNull();
            var length = syntax.Length.Text;
            if (!String.IsNullOrEmpty(length))
                name = name + "[" + length + "]";

            return new TypeDefinitionSymbol()
            {
                Name = name,
                ArrayDimensions = arrayDimensions,
                Temporary = temporary,
                Subtype = null,
                OptionMembers = null,
                TypeArguments = null,
            };
        }

        private static TypeDefinitionSymbol? Compile(SubtypedDataTypeSyntax syntax, List<int>? arrayDimensions, bool temporary)
        {
            var name = syntax.TypeName.Text.NotNull();

            return new TypeDefinitionSymbol()
            {
                Name = name,
                ArrayDimensions = arrayDimensions,
                Temporary = temporary,
                Subtype = SubtypeSymbolCompiler.Compile(syntax.Subtype),
                OptionMembers = null,
                TypeArguments = null,
            };
        }

        private static TypeDefinitionSymbol? Compile(GenericNamedDataTypeSyntax syntax, List<int>? arrayDimensions, bool temporary)
        {
            var name = syntax.TypeName.Text.NotNull();

            return new TypeDefinitionSymbol()
            {
                Name = name,
                ArrayDimensions = arrayDimensions,
                Temporary = temporary,
                Subtype = null,
                OptionMembers = null,
                TypeArguments = Compile(syntax.TypeArguments)
            };
        }

        private static TypeDefinitionSymbol? Compile(OptionDataTypeSyntax syntax, List<int>? arrayDimensions, bool temporary)
        {

            var name = syntax.TypeName.Text.NotNull();
            List<string>? optionMembers = null;
            if (syntax.OptionValues != null)
                optionMembers = NameCompiler.Compile(syntax.OptionValues.Options);

            return new TypeDefinitionSymbol()
            {
                Name = name,
                ArrayDimensions = arrayDimensions,
                Temporary = temporary,
                Subtype = null,
                OptionMembers = optionMembers,
                TypeArguments = null
            };
        }

        private static TypeDefinitionSymbol? Compile(EnumDataTypeSyntax syntax, List<int>? arrayDimensions, bool temporary)
        {
            var name = syntax.TypeName.Text.NotNull();

            return new TypeDefinitionSymbol()
            {
                Name = name,
                ArrayDimensions = arrayDimensions,
                Temporary = temporary,
                Subtype = SubtypeSymbolCompiler.Compile(syntax.EnumTypeName),
                OptionMembers = null,
                TypeArguments = null,
            };
        }

        private static TypeDefinitionSymbol? Compile(TextConstDataTypeSyntax syntax, List<int>? arrayDimensions, bool temporary)
        {
            var name = syntax.TypeName.Text.NotNull();

            return new TypeDefinitionSymbol()
            {
                Name = name,
                ArrayDimensions = arrayDimensions,
                Temporary = temporary,
                Subtype = null,
                OptionMembers = null,
                TypeArguments = null,
            };
        }

        private static TypeDefinitionSymbol? Compile(LabelDataTypeSyntax syntax, List<int>? arrayDimensions, bool temporary)
        {
            var name = syntax.TypeName.Text.NotNull();

            return new TypeDefinitionSymbol()
            {
                Name = name,
                ArrayDimensions = arrayDimensions,
                Temporary = temporary,
                Subtype = null,
                OptionMembers = null,
                TypeArguments = null,
            };
        }


        private static TypeDefinitionSymbol? Compile(DotNetDataTypeSyntax syntax, List<int>? arrayDimensions, bool temporary)
        {
            var name = syntax.TypeName.Text.NotNull();

            return new TypeDefinitionSymbol()
            {
                Name = name,
                ArrayDimensions = arrayDimensions,
                Temporary = temporary,
                Subtype = null,
                OptionMembers = null,
                TypeArguments = null,
            };
        }

    }
}
