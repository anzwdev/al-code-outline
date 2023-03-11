using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnZwDev.ALTools.ALSymbols;

namespace AnZwDev.ALTools.ALSymbolReferences
{
    public class ALAppTypeDefinition : ALAppElementWithName
    {

        public List<int> ArrayDimensions { get; set; }
        public bool Temporary { get; set; }
        public ALAppSubtypeDefinition Subtype { get; set; }
        public List<ALAppTypeDefinition> TypeArguments { get; set; }
        public List<string> OptionMembers { get; set; }

        public ALAppTypeDefinition()
        {
        }

        public override string GetSourceCode()
        {
            StringBuilder stringBuilder = new StringBuilder();
            this.AppendDefinitionSourceCode(stringBuilder);
            return stringBuilder.ToString();
        }

        public void AppendDefinitionSourceCode(StringBuilder builder)
        {
            //append array
            if ((this.ArrayDimensions != null) && (this.ArrayDimensions.Count > 0))
            {
                builder.Append("array[");
                for (int i = 0; i < this.ArrayDimensions.Count; i++)
                {
                    if (i > 0)
                        builder.Append(",");
                    builder.Append(this.ArrayDimensions[i].ToString());
                }
                builder.Append("] of ");
            }
            
            //type name
            builder.Append(this.Name);
            if (!this.EmptySubtype())
            {
                builder.Append(" ");
                builder.Append(ALSyntaxHelper.EncodeName(this.Subtype.Name.Trim()));
                if (this.Temporary)
                    builder.Append(" temporary");
            }

            //type arguments
            if ((this.TypeArguments != null) && (this.TypeArguments.Count > 0))
            {
                builder.Append(" of [");
                for (int i = 0; i < this.TypeArguments.Count; i++)
                {
                    if (i > 0)
                        builder.Append(", ");
                    this.TypeArguments[i].AppendDefinitionSourceCode(builder);
                }
                builder.Append("]");
            }

        }

        protected override ALSymbol CreateMainALSymbol()
        {
            return new ALSymbol(this.GetALSymbolKind(), this.GetSourceCode());
        }

        public bool EmptySubtype()
        {
            return ((this.Subtype == null) || (String.IsNullOrWhiteSpace(this.Subtype.Name)) || (this.Subtype.Name.ToLower() == "none"));
        }

        public bool IsEmpty()
        {
            return ((String.IsNullOrWhiteSpace(this.Name)) || (this.Name.ToLower() == "none"));
        }

    }
}
