using AnZwDev.AL.Symbols;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.AL.LanguageServer.Modules.ProjectInformationProvider.Contracts.Symbols
{
    internal class PITableFieldListItem
    {

        [JsonProperty("id")]
        public required int Id { get; init; }

        [JsonProperty("name")]
        public required string? Name { get; init; }

        [JsonProperty("displayString")]
        public required string? DisplayString { get; init; }

        [JsonProperty("caption")]
        public required string? Caption { get; init; }

        [JsonProperty("captionLabel", NullValueHandling = NullValueHandling.Ignore)]
        public required PILabel? CaptionLabel { get; init; }

        [JsonProperty("description")]
        public required string? Description { get; init; }

        [JsonProperty("dataType")]
        public required string? DataType { get; init; }

        [JsonProperty("class")]
        public required FieldClass Class { get; init; }

        [JsonProperty("toolTips", NullValueHandling = NullValueHandling.Ignore)]
        public required List<PILabel>? ToolTips { get; init; }

    }
}
