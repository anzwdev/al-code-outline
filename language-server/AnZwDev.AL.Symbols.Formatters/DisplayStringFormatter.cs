using AnZwDev.AL.Symbols.Formatters.DisplayStringFormatters;
using AnZwDev.AL.Symbols.Formatters.FullNameFormatters;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.AL.Symbols.Formatters
{
    public static class DisplayStringFormatter
    {

        private static readonly DisplayStringFullyQualifiedNameFormatter _fullyQualifiedNameFormatter = new DisplayStringFullyQualifiedNameFormatter();
        public static void WriteFullyQualifiedName(TextWriter writer, FullyQualifiedName fullyQualifiedName)
        {
            _fullyQualifiedNameFormatter.Write(writer, fullyQualifiedName);
        }
        public static string FormatFullyQualifiedName(FullyQualifiedName fullyQualifiedName)
        {
            return _fullyQualifiedNameFormatter.Get(fullyQualifiedName);
        }

        private static readonly DisplayStringMethodSymbolFormatter _methodSymbolFormatter = new DisplayStringMethodSymbolFormatter();
        public static void WriteMethodSymbol(TextWriter writer, MethodSymbol methodSymbol)
        {
            _methodSymbolFormatter.Write(writer, methodSymbol);
        }
        public static string FormatMethodSymbol(MethodSymbol methodSymbol)
        {
            return _methodSymbolFormatter.Get(methodSymbol);
        }

        private static readonly DisplayStringMethodParameterSymbolFormatter _methodParameterSymbolFormatter = new DisplayStringMethodParameterSymbolFormatter();
        public static void WriteParameterSymbol(TextWriter writer, MethodParameterSymbol parameterSymbol)
        {
            _methodParameterSymbolFormatter.Write(writer, parameterSymbol);
        }
        public static string FormatParameterSymbol(MethodParameterSymbol parameterSymbol)
        {
            return _methodParameterSymbolFormatter.Get(parameterSymbol);
        }

        private static readonly DisplayStringTypeDefinitionSymbolFormatter _typeDefinitionSymbolFormatter = new DisplayStringTypeDefinitionSymbolFormatter();
        public static void WriteTypeDefinitionSymbol(TextWriter writer, TypeDefinitionSymbol typeDefinitionSymbol)
        {
            _typeDefinitionSymbolFormatter.Write(writer, typeDefinitionSymbol);
        }
        public static string FormatTypeDefinitionSymbol(TypeDefinitionSymbol typeDefinitionSymbol)
        {
            return _typeDefinitionSymbolFormatter.Get(typeDefinitionSymbol);
        }

        private static readonly DisplayStringSubtypeSymbolFormatter _subtypeSymbolFormatter = new DisplayStringSubtypeSymbolFormatter();
        public static void WriteSubtypeSymbol(TextWriter writer, SubtypeSymbol subtypeSymbol, bool isTemporary)
        {
            _subtypeSymbolFormatter.Write(writer, subtypeSymbol, isTemporary);
        }
        public static string FormatSubtypeSymbol(SubtypeSymbol subtypeSymbol, bool isTemporary)
        {
            return _subtypeSymbolFormatter.Get(subtypeSymbol, isTemporary);
        }

        private static readonly DisplayStringMethodReturnParameterDefinitionSymbolFormatter _returnParameterSymbolFormatter = new DisplayStringMethodReturnParameterDefinitionSymbolFormatter();
        public static void WriteReturnParameter(TextWriter writer, MethodReturnParameterDefinitionSymbol returnParameterSymbol)
        {
            _returnParameterSymbolFormatter.Write(writer, returnParameterSymbol);
        }
        public static string FormatReturnParameter(MethodReturnParameterDefinitionSymbol returnParameterSymbol)
        {
            return _returnParameterSymbolFormatter.Get(returnParameterSymbol);
        }

        private static readonly DisplayStringTableFieldSymbolFormatter _tableFieldSymbolFormatter = new DisplayStringTableFieldSymbolFormatter();
        public static void WriteTableField(TextWriter writer, TableFieldSymbol symbol)
        {
            _tableFieldSymbolFormatter.Write(writer, symbol);
        }
        public static string FormatTableField(TableFieldSymbol symbol)
        {
            return _tableFieldSymbolFormatter.Get(symbol);
        }

        private static readonly DisplayStringVariableDeclarationSymbolFormatter variableDeclarationSymbolFormatter = new DisplayStringVariableDeclarationSymbolFormatter();
        public static void WriteVariableDeclaration(TextWriter writer, VariableDeclarationSymbol symbol)
        {
            variableDeclarationSymbolFormatter.Write(writer, symbol);
        }
        public static string FormatVariableDeclaration(VariableDeclarationSymbol symbol)
        {
            return variableDeclarationSymbolFormatter.Get(symbol);
        }

    }
}
