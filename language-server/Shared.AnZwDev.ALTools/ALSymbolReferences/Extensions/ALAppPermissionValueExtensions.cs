using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.ALSymbolReferences.Extensions
{
    public static class ALAppPermissionValueExtensions
    {

        public static ALAppPermissionValue IndirectToDirect(this ALAppPermissionValue value)
        {
            value = value & ALAppPermissionValue.AllIndirect;
            value = (ALAppPermissionValue)(((int)value) >> 5);
            return value;
        }

        public static ALAppPermissionValue DirectToIndirect(this ALAppPermissionValue value)
        {
            value = value & ALAppPermissionValue.AllDirect;
            value = (ALAppPermissionValue)(((int)value) << 5);
            return value;
        }

        public static ALAppPermissionValue FromString(string source)
        {
            ALAppPermissionValue value = ALAppPermissionValue.Empty;
            if (source != null)
                for (int i = 0; i < source.Length; i++)
                    value = value | FromChar(source[i]);
            return value;
        }

        public static ALAppPermissionValue FromChar(char value)
        {
            switch (value)
            {
                case 'R': return ALAppPermissionValue.R;
                case 'I': return ALAppPermissionValue.I;
                case 'M': return ALAppPermissionValue.M;
                case 'D': return ALAppPermissionValue.D;
                case 'X': return ALAppPermissionValue.X;
                case 'r': return ALAppPermissionValue.IndirectR;
                case 'i': return ALAppPermissionValue.IndirectI;
                case 'm': return ALAppPermissionValue.IndirectM;
                case 'd': return ALAppPermissionValue.IndirectD;
                case 'x': return ALAppPermissionValue.IndirectX;
            }
            return ALAppPermissionValue.Empty;
        }

        public static string ToALString(this ALAppPermissionValue enumValue)
        {
            string alValue = "";

            if ((enumValue & ALAppPermissionValue.R) == ALAppPermissionValue.R)
                alValue = alValue + "R";
            if ((enumValue & ALAppPermissionValue.I) == ALAppPermissionValue.I)
                alValue = alValue + "I";
            if ((enumValue & ALAppPermissionValue.M) == ALAppPermissionValue.M)
                alValue = alValue + "M";
            if ((enumValue & ALAppPermissionValue.D) == ALAppPermissionValue.D)
                alValue = alValue + "D";
            if ((enumValue & ALAppPermissionValue.X) == ALAppPermissionValue.X)
                alValue = alValue + "X";

            if ((enumValue & ALAppPermissionValue.IndirectR) == ALAppPermissionValue.IndirectR)
                alValue = alValue + "r";
            if ((enumValue & ALAppPermissionValue.IndirectI) == ALAppPermissionValue.IndirectI)
                alValue = alValue + "i";
            if ((enumValue & ALAppPermissionValue.IndirectM) == ALAppPermissionValue.IndirectM)
                alValue = alValue + "m";
            if ((enumValue & ALAppPermissionValue.IndirectD) == ALAppPermissionValue.IndirectD)
                alValue = alValue + "d";
            if ((enumValue & ALAppPermissionValue.IndirectX) == ALAppPermissionValue.IndirectX)
                alValue = alValue + "x";

            return alValue;
        }

    }
}
