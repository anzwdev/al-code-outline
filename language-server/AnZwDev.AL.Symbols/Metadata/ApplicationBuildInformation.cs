using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AnZwDev.AL.Symbols.Metadata
{
    public class ApplicationBuildInformation
    {

        public required string? By { get; init; }
        public required string? Url { get; init; }


    }
}
