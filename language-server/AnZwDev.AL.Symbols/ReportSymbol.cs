using AnZwDev.AL.Symbols.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AnZwDev.AL.Symbols
{

    public class ReportSymbol : ObjectWithCodeSymbol
    {

        public required RequestPageSymbol? RequestPage { get; init; }
        public required List<ReportDataItemSymbol>? DataItems { get; init; }
        public required List<ReportLabelSymbol>? Labels { get; init; }
        public required List<ReportLayoutSymbol>? Layouts { get; init; }


        public ReportSymbol(int id, FullyQualifiedName fullyQualifiedName, PropertySymbolsCollection properties)
            : base(id, fullyQualifiedName, properties)
        {
        }

        protected override ObjectKind GetObjectType()
        {
            return ObjectKind.Report;
        }

    }

}
