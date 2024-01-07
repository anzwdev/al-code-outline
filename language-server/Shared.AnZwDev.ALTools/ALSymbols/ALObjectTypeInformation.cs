using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.ALSymbols
{
    public class ALObjectTypeInformation
    {

        public ALObjectType ALObjectType { get; }
        public ALSymbolKind ALSymbolKind { get; }
        public ALSymbolKind ALSymbolsListKind { get; }
        public string ObjectTypeName { get; }
        public string VariableTypeName { get; }
        public bool ServerDefinitionAvailable { get; }

        public ALObjectTypeInformation(ALObjectType alObjectType, ALSymbolKind alSymbolKind, ALSymbolKind alSymbolsListKind, string objectTypeName, string variableTypeName, bool serverDefinitionAvailable)
        {
            ALSymbolKind = alSymbolKind;
            ALObjectType = alObjectType;
            ALSymbolsListKind = alSymbolsListKind;
            ObjectTypeName = objectTypeName;
            VariableTypeName = variableTypeName;
            ServerDefinitionAvailable = serverDefinitionAvailable;
        }

    }
}
