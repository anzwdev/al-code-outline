using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.AL.Symbols.Platform
{
    public class PlatformCapabilities
    {

        public Version Version { get; }

        public bool Interfaces { get; }
        public bool AppAreasInheritance { get; }
        public bool Namespaces { get; }
        public bool TableToolTips { get; }


        public PlatformCapabilities(Version version)
        {
            this.Version = version;

            this.Interfaces = CheckVersion(5);
            this.AppAreasInheritance = CheckVersion(10);
            this.Namespaces = CheckVersion(12);
            this.TableToolTips = CheckVersion(13);
        }

        private bool CheckVersion(int major, int minor = 0)
        {
            return
                (Version.Major > major) ||
                ((Version.Major == major) && (Version.Minor >= minor));
        }

    }
}
