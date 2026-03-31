using AnZwDev.AL.Symbols;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.AL.Workspaces.InformationProviders.Objects
{
    public class PageTableFields
    {

        public ObjectIdentifier PageIdentifier { get; }
        public ObjectIdentifier TableIdentifier { get; }
        public List<TableFieldSymbol> AddedFields { get; } = new List<TableFieldSymbol>();
        public List<TableFieldSymbol> AvailableFields { get; } = new List<TableFieldSymbol>();

        public PageTableFields(ObjectIdentifier pageIdentifier, ObjectIdentifier tableIdentifier)
        {
            this.PageIdentifier = pageIdentifier;
            this.TableIdentifier = tableIdentifier;
        }

    }
}
