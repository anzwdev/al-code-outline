using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using AnZwDev.AL.Symbols.Parsing;

namespace AnZwDev.AL.Symbols.Providers.AppPackages.Symbols
{
    internal class AppPageControlSymbol : AppSerializedSymbol<PageControlSymbol>
    {

        [JsonPropertyName("Id")]
        public int Id { get; set; }

        [JsonPropertyName("Name")]
        public string? Name { get; set; }

        [JsonPropertyName("Kind")]
        public int Kind { get; set; }

        [JsonPropertyName("Properties")]
        public AppPropertySymbol[]? Properties { get; set; }

        [JsonPropertyName("Controls")]
        public AppPageControlSymbol[]? Controls { get; set; }

        [JsonPropertyName("Actions")]
        public AppPageActionSymbol[]? Actions { get; set; }

        [JsonPropertyName("TypeDefinition")]
        public AppTypeDefinitionSymbol? TypeDefinition { get; set; }

        [JsonPropertyName("RelatedPagePartId")]
        public AppRelatedPagePartIdSymbol? RelatedPagePartId { get; set; }

        [JsonPropertyName("RelatedControlAddIn")]
        public string? RelatedControlAddIn { get; set; }

        [JsonPropertyName("RelatedControlAddInPublicKey")]
        public string? RelatedControlAddInPublicKey { get; set; }

        public override PageControlSymbol CreateSymbol(string? ns)
        {
            return new PageControlSymbol()
            {
                Id = Id,
                Name = Name ?? String.Empty,
                Properties = AppPropertySymbol.CreatePropertySymbolsCollection(Properties),
                Kind = AppEnumConverters.IntToPageControlKind(Kind),
                Controls = Controls.CreateSymbolsListOrNull<PageControlSymbol, AppPageControlSymbol>(ns),
                Actions = Actions.CreateSymbolsListOrNull<PageActionSymbol, AppPageActionSymbol>(ns),
                RelatedControlAddIn = (String.IsNullOrWhiteSpace(RelatedControlAddIn)) ? null : ALSymbolExpressionParser.ParseObjectReference(ObjectKind.ControlAddIn, RelatedControlAddIn, null),
                TypeDefinition = TypeDefinition?.CreateSymbol(ns),
                RelatedControlAddInPublicKey = RelatedControlAddInPublicKey,
                RelatedPagePartId = (RelatedPagePartId != null) ? new ObjectReference(ObjectKind.Page, null, RelatedPagePartId.Id, new FullyQualifiedName(null, RelatedPagePartId.Name ?? String.Empty), null) : null
            };
        }

    }
}
