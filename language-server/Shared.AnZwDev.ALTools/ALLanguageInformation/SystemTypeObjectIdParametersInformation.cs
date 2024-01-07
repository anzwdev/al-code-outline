using AnZwDev.ALTools.ALSymbols.Internal;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.ALLanguageInformation
{
    internal class SystemTypeObjectIdParametersInformation
    {

        public ConvertedNavTypeKind NavTypeKind { get; }
        public Dictionary<string, MethodObjectIdParametersInformation> Methods { get; } = new Dictionary<string, MethodObjectIdParametersInformation>();

        public SystemTypeObjectIdParametersInformation(ConvertedNavTypeKind navTypeKind, params MethodObjectIdParametersInformation[] methods)
        {
            NavTypeKind = navTypeKind;
            for (int i=0; i<methods.Length; i++)
                Methods.Add(methods[i].Name.ToLower(), methods[i]);
        }

        public MethodObjectIdParametersInformation GetMethod(string name)
        {
            if (name != null)
            {
                name = name.ToLower();
                if (Methods.ContainsKey(name))
                    return Methods[name];
            }
            return null;
        }

    }
}
