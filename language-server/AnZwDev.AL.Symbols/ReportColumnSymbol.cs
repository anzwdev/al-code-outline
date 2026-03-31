using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AnZwDev.AL.Symbols
{
    public class ReportColumnSymbol : NamedSymbolWithIdAndProperties
    {

        public required string? OwningDataItemName { get; init; }
        public required string? SourceExpression { get; init; }
        public required TypeDefinitionSymbol? TypeDefinition { get; init; }


    }
}
