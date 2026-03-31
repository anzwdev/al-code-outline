using AnZwDev.AL.Symbols;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.AL.LanguageServer.Modules.ProjectInformationProvider.Contracts.Symbols
{
    internal class PIObjectIdentifier
    {

        [JsonProperty("kind")]
        public ObjectKind Kind { get; set; }

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string? Name { get; set; }

        [JsonProperty("namespace")]
        public string? Namespace { get; set; }

        public ObjectIdentifier ToObjectIdentifier()
        {
            return new ObjectIdentifier(this.Kind, this.Id, new FullyQualifiedName(this.Namespace, this.Name ?? String.Empty));
        }

    }
}
