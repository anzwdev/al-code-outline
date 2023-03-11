using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.ALSymbols
{
    public struct ALMemberAccessExpression
    {

        public string Name { get; set; }
        public string Expression { get; set; }

        public ALMemberAccessExpression(string name, string expression)
        {
            this.Name = name;
            this.Expression = expression;
        }

        public string GetSourceFieldNameWithoutRec()
        {
            if (String.IsNullOrWhiteSpace(this.Expression))
                return this.Name;
            else if (this.Name.Equals("rec", StringComparison.CurrentCultureIgnoreCase))
                return this.Expression;
            return null;
        }


    }
}
