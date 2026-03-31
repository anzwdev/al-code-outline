using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AnZwDev.AL.Symbols.Metadata
{
    public class ALResourceExposurePolicy
    {

        public required bool AllowDebugging { get; init; }
        public required bool AllowDownloadingSource { get; init; }
        public required bool ApplyToDevExtension { get; init; }
        public required bool IncludeSourceInSymbolFile { get; init; }

    }
}
