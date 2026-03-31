using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.AL.Symbols.Providers.AppPackages.Symbols
{
    internal static class AppPermissionValue
    {

        public static int Read { get; } = 1;
        public static int Insert { get; } = 2;
        public static int Modify { get; } = 4;
        public static int Execute { get; } = 16;
        public static int Delete { get; } = 8;
        public static int IndirectRead { get; } = 32;
        public static int IndirectInsert { get; } = 64;
        public static int IndirectModify { get; } = 128;
        public static int IndirectDelete { get; } = 256;
        public static int IndirectExecute { get; } = 512;

    }
}
