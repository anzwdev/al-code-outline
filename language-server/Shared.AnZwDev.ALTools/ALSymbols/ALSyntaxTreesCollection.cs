using AnZwDev.ALTools.ALSymbols.SymbolReaders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.ALTools.ALSymbols
{
    public class ALSyntaxTreesCollection
    {

        public Dictionary<string, ALSyntaxTree> SyntaxTrees { get; }
        protected ALSyntaxTreeSymbolsReader SyntaxTreeReader { get; }

        public ALSyntaxTreesCollection()
        {
            this.SyntaxTrees = new Dictionary<string, ALSyntaxTree>();
            this.SyntaxTreeReader = new ALSyntaxTreeSymbolsReader();
        }

        public bool Close(string filePath)
        {
            if (this.SyntaxTrees.ContainsKey(filePath))
            {
                ALSyntaxTree syntaxTree = this.SyntaxTrees[filePath];
                syntaxTree.OpenCount--;
                if (syntaxTree.OpenCount <= 0)
                {
                    this.SyntaxTrees.Remove(filePath);
                    return true;
                }
                return false;
            }
            return true;
        }

        public ALSyntaxTree FindOrCreate(string path, bool open)
        {
            ALSyntaxTree syntaxTree = null;

            if (this.SyntaxTrees.ContainsKey(path))
            {
                syntaxTree = this.SyntaxTrees[path];
                if (open)
                    syntaxTree.OpenCount++;
            }
            else
            {
                syntaxTree = new ALSyntaxTree(path, this.SyntaxTreeReader);
                syntaxTree.OpenCount = 1;
                this.SyntaxTrees.Add(path, syntaxTree);
            }
            return syntaxTree;
        }

    }
}
