using AnZwDev.AL.Symbols;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.AL.LanguageServer.Modules.ProjectInformationProvider.Contracts.Symbols
{
    internal static class PIObjectIdentifierListExtensions
    {

        public static List<ObjectIdentifier> ToObjectIdentifierList(this PIObjectIdentifier[]? list)
        {
            var identifiers = new List<ObjectIdentifier>();
            if (list != null)
                for (var i = 0; i < list.Length; i++)
                    identifiers.Add(list[i].ToObjectIdentifier());
            return identifiers;
        }

    }
}
