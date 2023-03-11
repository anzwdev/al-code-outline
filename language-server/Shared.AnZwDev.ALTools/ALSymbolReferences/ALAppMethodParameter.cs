using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnZwDev.ALTools.ALSymbols;

namespace AnZwDev.ALTools.ALSymbolReferences
{
    public class ALAppMethodParameter : ALAppVariable
    {

        public bool IsVar { get; set; }

        public ALAppMethodParameter()
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
            if (this.IsVar)
                builder.Append("var ");
            builder.Append(ALSyntaxHelper.EncodeName(this.Name));
            if (this.TypeDefinition != null)
            {
                builder.Append(": ");
                this.TypeDefinition.AppendDefinitionSourceCode(builder);
            }
        }

        public override ALSymbolKind GetALSymbolKind()
        {
            return ALSymbolKind.Parameter;
        }

    }
}
