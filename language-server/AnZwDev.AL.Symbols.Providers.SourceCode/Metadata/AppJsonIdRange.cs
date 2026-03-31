using AnZwDev.AL.Symbols;
using AnZwDev.AL.Symbols.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AnZwDev.AL.Symbols.Providers.SourceCode.Metadata
{
    internal class AppJsonIdRange
    {

        [JsonPropertyName("from")]
        public int From { get; set; }

        [JsonPropertyName("to")]
        public int To { get; set; }


        public IdRange CreateSymbol()
        {
            return new IdRange()
            {
                From = From,
                To = To
            };
        }

    }
}
