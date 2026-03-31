using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.AL.Symbols.Metadata
{
    public class ApplicationDependency
    {

        public required string Id { get; set; }
        public required string Name { get; set; }
        public required string Publisher { get; set; }
        public required Version Version { get; set; }

    }
}
