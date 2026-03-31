using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.System
{
    public static class StringTypeConverter
    {

        public static bool ToBool(string value)
        {
            return ((value != null) && (value.Equals("true", StringComparison.OrdinalIgnoreCase)));
        }

        public static int ToInt(string value, int defaultValue = 0)
        {
            if ((value != null) && (Int32.TryParse(value, out var outVal)))
                return outVal;
            return defaultValue;
        }

        public static T ToEnum<T>(string value) where T : struct
        {
            if (Enum.TryParse<T>(value, true, out T result))
                return result;
            return default(T);
        }        

        public static Version ToVersion(string? value, int major, int minor, int build, int revision)
        {
            if (!String.IsNullOrWhiteSpace(value))
                return new Version(value);
            return new Version(major, minor, build, revision);
        }

        public static Version ToVersion(string? value, int major, int minor)
        {
            if (!String.IsNullOrWhiteSpace(value))
                return new Version(value);
            return new Version(major, minor);
        }


    }
}
