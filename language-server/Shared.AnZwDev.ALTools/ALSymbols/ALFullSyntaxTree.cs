using AnZwDev.ALTools.ALSymbols.SymbolReaders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.ALTools.ALSymbols
{
    public class ALFullSyntaxTree
    {

        public ALFullSyntaxTreeNode Root { get; set; }

        public ALFullSyntaxTree()
        {
        }

        public void Load(string source, string filePath)
        {
            ALFullSyntaxTreeReader reader = new ALFullSyntaxTreeReader();
            if (!String.IsNullOrEmpty(source))
                this.Root = reader.ProcessSourceCode(source);
            else
                this.Root = reader.ProcessSourceFile(filePath);
        }


    }
}
