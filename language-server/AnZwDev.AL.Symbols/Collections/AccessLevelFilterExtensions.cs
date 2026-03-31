using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.AL.Symbols.Collections
{
    public static class AccessLevelFilterExtensions
    {

        public static bool Valid(this AccessLevelFilter accessLevelFilter, AccessLevel accessLevel)
        {
            switch (accessLevelFilter)
            {
                case AccessLevelFilter.Public:
                    return accessLevel == AccessLevel.Public;

                case AccessLevelFilter.Protected:
                    return accessLevel == AccessLevel.Public || accessLevel == AccessLevel.Protected;

                case AccessLevelFilter.Internal:
                    return accessLevel == AccessLevel.Public || accessLevel == AccessLevel.Protected || accessLevel == AccessLevel.Internal;

                case AccessLevelFilter.Local:
                case AccessLevelFilter.All:
                case AccessLevelFilter.Accessible:
                default:
                    return true;
            }
        }

    }
}
