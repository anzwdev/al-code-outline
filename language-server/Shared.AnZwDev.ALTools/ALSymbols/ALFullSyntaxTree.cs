using AnZwDev.ALTools.ALSymbols.SymbolReaders;
using AnZwDev.ALTools.Workspace;
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

        public void Load(string source, string filePath, ALProject project)
        {
            ALFullSyntaxTreeReader reader = new ALFullSyntaxTreeReader();
            if (!String.IsNullOrEmpty(source))
                this.Root = reader.ProcessSourceCode(source, project);
            else
                this.Root = reader.ProcessSourceFile(filePath, project);
        }


    }
}
