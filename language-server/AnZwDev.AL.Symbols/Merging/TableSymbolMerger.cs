using AnZwDev.AL.Symbols.Collections;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Text;

namespace AnZwDev.AL.Symbols.Merging
{
    internal class TableSymbolMerger : SymbolMerger<TableSymbol, TableExtensionSymbol>
    {

        protected override TableSymbol CloneSymbol(TableSymbol mainSymbol)
        {
            return new TableSymbol(mainSymbol.Identifier, mainSymbol.Properties)
            {
                ReferenceSourceFileName = mainSymbol.ReferenceSourceFileName,
                Usings = mainSymbol.Usings,
                Variables = new List<GlobalVariableDeclarationSymbol>(mainSymbol.Variables),
                Methods = new List<MethodSymbol>(mainSymbol.Methods),
                Fields = new List<TableFieldSymbol>(mainSymbol.Fields),
                Keys = new List<TableKeySymbol>(mainSymbol.Keys),
                FieldGroups = new List<TableFieldGroupSymbol>(mainSymbol.FieldGroups)
            };
        }

        protected override void ApplyExtension(TableSymbol mainSymbol, TableExtensionSymbol symbolExtension)
        {
            mainSymbol.Variables.AddRange(symbolExtension.Variables);
            mainSymbol.Methods.AddRange(symbolExtension.Methods);

            if (symbolExtension.Fields != null)
                mainSymbol.Fields.AddRange(symbolExtension.Fields);

            if (symbolExtension.Keys != null)
                mainSymbol.Keys.AddRange(symbolExtension.Keys);

            //!!! TO-DO: check if we can merge field groups (we need to check if there are already field groups with the same name and merge them instead of adding new ones)
            //if (symbolExtension.FieldGroups != null)
            //    mainSymbol.FieldGroups.AddRange(symbolExtension.FieldGroups);
        }

    }
}
