using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.AL.Symbols.Providers.SourceCode
{
    public struct MetadataReloadResult
    {

        public bool AppIdChanged { get; set; }
        public bool DependenciesChanged { get; set; }

        public MetadataReloadResult()
        {
            AppIdChanged = false;
            DependenciesChanged = false;
        }

    }
}
