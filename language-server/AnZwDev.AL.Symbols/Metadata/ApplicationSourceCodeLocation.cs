using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AnZwDev.AL.Symbols.Metadata
{
    public class ApplicationSourceCodeLocation
    {

        public required string? Commit { get; init; }
        public required string? RepositoryUrl { get; init; }

    }
}
