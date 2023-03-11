using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.Workspace.SymbolsInformation
{
    public class DotNetTypeInformation
    {

        public string AliasName { get; set; }
        public string TypeName { get; set; }

        public DotNetTypeInformation()
        {
        }

        public DotNetTypeInformation(string newTypeName, string newAliasName)
        {
            this.TypeName = newTypeName;
            this.AliasName = newAliasName;
        }

    }
}
