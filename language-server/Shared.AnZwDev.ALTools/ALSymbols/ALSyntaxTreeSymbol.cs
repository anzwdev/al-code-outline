using AnZwDev.ALTools.Extensions;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.ALTools.ALSymbols
{
    public class ALSyntaxTreeSymbol : ALSymbol
    {

        public string type { get; set; }
        public List<PropertyValue> properties { get; set; }
        public SyntaxNode syntaxTreeNode { get; set; }

        public ALSyntaxTreeSymbol()
        {
            this.properties = new List<PropertyValue>();
        }

        public ALSyntaxTreeSymbol(ALSymbolKind kindValue, string nameValue) : base(kindValue, nameValue)
        {
            this.properties = new List<PropertyValue>();
        }

        public ALSymbol ToALSymbolInformation()
        {
            return this.CreateCopy(true);
        }

        public ALSyntaxTreeSymbol CreateSerializableCopy()
        {
            ALSyntaxTreeSymbol symbol = new ALSyntaxTreeSymbol();
            symbol.kind = this.kind;
            symbol.name = this.name;
            symbol.fullName = this.fullName;
            symbol.range = this.range;
            symbol.selectionRange = this.selectionRange;
            symbol.type = this.type;

            symbol.LoadProperties(this.syntaxTreeNode);
            
            return symbol;
        }

        protected void LoadProperties(object node)
        {
            this.properties.Clear();
            if (node != null)
            {
                Type type = node.GetType();
                PropertyInfo[] propList = type.GetProperties();
                for (int i=0; i<propList.Length; i++)
                {
                    if (!propList[i].Name.EqualsToOneOf("Parent", "ParentTrivia", "SyntaxTree"))
                    {
                        object val = propList[i].GetValue(node);
                        if (val != null)
                        {
                            this.properties.Add(new PropertyValue(propList[i].Name, val.ToString()));
                        }
                    }
                }
            }

        }


    }
}
