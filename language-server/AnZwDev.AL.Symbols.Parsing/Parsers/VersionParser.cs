using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.AL.Symbols.Parsing.Parsers
{
    internal class VersionParser
    {

        public Version Parse(string? version, int major, int minor, int build, int revision)
        {
            if ((!String.IsNullOrWhiteSpace(version)) && (Version.TryParse(version, out Version? value)))
                return value;
            return new Version(major, minor, build, revision);
        }

        public Version Parse(string? version, int major, int minor)
        {
            if ((!String.IsNullOrWhiteSpace(version)) && (Version.TryParse(version, out Version? value)))
                return value;
            return new Version(major, minor);
        }

    }
}
