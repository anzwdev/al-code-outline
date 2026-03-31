using AnZwDev.AL.Symbols.Collections;
using AnZwDev.AL.Symbols.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AnZwDev.AL.Symbols
{
    public class ApplicationSymbol : Symbol
    {

        public required string ReferenceSourceFileName { get; init; }

        public required string AppId { get; set; }
        public required string Name { get; set; }
        public required string Publisher { get; set; }
        public required Version Version { get; set; }

        public required ApplicationMetadata Metadata { get; set; }

        public TableSymbolsCollection Tables { get; } = new();
        public ObjectSymbolsCollection<CodeunitSymbol> Codeunits { get; } = new();
        public ObjectSymbolsCollection<PageSymbol> Pages { get; } = new();
        public ObjectExtensionSymbolsCollection<PageExtensionSymbol> PageExtensions { get; } = new();
        public ObjectExtensionSymbolsCollection<PageCustomizationSymbol> PageCustomizations { get; } = new();
        public ObjectSymbolsCollection<ReportSymbol> Reports { get; } = new();
        public ObjectExtensionSymbolsCollection<ReportExtensionSymbol> ReportExtensions { get; } = new();
        public ObjectSymbolsCollection<XmlPortSymbol> XmlPorts { get; } = new();
        public ObjectSymbolsCollection<QuerySymbol> Queries { get; } = new();
        public ObjectSymbolsCollection<ControlAddInSymbol> ControlAddIns { get; } = new();
        public ObjectSymbolsCollection<EnumTypeSymbol> EnumTypes { get; } = new();
        public ObjectSymbolsCollection<DotNetPackageSymbol> DotNetPackages { get; } = new();
        public ObjectSymbolsCollection<InterfaceSymbol> Interfaces { get; } = new();
        public ObjectSymbolsCollection<PermissionSetSymbol> PermissionSets { get; } = new();
        public ObjectExtensionSymbolsCollection<PermissionSetExtensionSymbol> PermissionSetExtensions { get; } = new();
        public ObjectExtensionSymbolsCollection<EnumExtensionTypeSymbol> EnumExtensionTypes { get; } = new();
        public ObjectExtensionSymbolsCollection<TableExtensionSymbol> TableExtensions { get; } = new();
        public ObjectSymbolsCollection<ProfileSymbol> Profiles { get; } = new();
        public ObjectExtensionSymbolsCollection<ProfileExtensionSymbol> ProfileExtensions { get; } = new();
        public AllObjectSymbolsCollection AllObjects { get; }

        public ApplicationSymbol()
        {
            AllObjects = new AllObjectSymbolsCollection(this);
        }

        public void CopyMetadata(ApplicationSymbol source)
        {
            this.AppId = source.AppId;
            this.Name = source.Name;
            this.Publisher = source.Publisher;
            this.Version = source.Version;
            this.Metadata = source.Metadata;
        }

    }
}
