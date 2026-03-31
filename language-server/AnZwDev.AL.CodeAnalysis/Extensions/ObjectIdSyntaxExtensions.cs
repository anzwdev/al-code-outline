using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AnZwDev.AL.CodeAnalysis.Extensions
{
    public static class ObjectIdSyntaxExtensions
    {

        public static int GetIntValue(this ObjectIdSyntax syntax)
        {
            var idString = syntax?.Value.ValueText;
            if (!String.IsNullOrWhiteSpace(idString) && Int32.TryParse(idString, out int id))
                return id;
            return 0;
        }

    }
}
