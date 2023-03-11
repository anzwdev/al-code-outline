using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.ALTools.ALSymbols
{
    public class ALSymbolsLibrary
    {

        public ALSymbol Root { get; set; }
        public ALSymbolsLibrarySource Source { get; set; }

        public ALSymbolsLibrary() : this(null, null)
        {
        }

        public ALSymbolsLibrary(ALSymbolsLibrarySource source, ALSymbol rootSymbol)
        {
            this.Source = source;
            this.Root = rootSymbol;
        }

        public virtual bool Load(bool forceReload)
        {
            return false;
        }

        public ALSymbol GetObjectsTree()
        {
            return this.Root?.GetObjectsTree();
        }

        public List<ALSymbol> GetSymbolsListByPath(int[][] paths, ALSymbolKind kind)
        {
            List<ALSymbol> symbolsList = new List<ALSymbol>();
            for (int i=0; i<paths.Length;i++)
            {
                ALSymbol symbol = this.GetSymbolByPath(paths[i]);
                if ((symbol != null) && (
                    (kind == ALSymbolKind.Undefined) ||
                    (symbol.kind == kind) ||
                    ((kind == ALSymbolKind.AnyALObject) && (symbol.kind.IsObjectDefinition()))))
                    symbolsList.Add(symbol);
            }
            return symbolsList;
        }

        public ALSymbol GetSymbolByPath(int[] path)
        {
            if ((this.Root != null) && (path != null) && (path.Length > 0))
            {
                ALSymbol symbol = this.Root;
                for (int i = path.Length - 1; i >= 0; i--)
                {
                    if ((symbol.childSymbols == null) || (path[i] >= symbol.childSymbols.Count))
                        return null;
                    if (path[i] == -1)
                        symbol = this.Root;
                    else
                        symbol = symbol.childSymbols[path[i]];
                }
                return symbol;
            }
            return null;
        }

        public ALSymbolSourceLocation GetSymbolSourceLocation(ALSymbol symbol)
        {
            if (this.Source != null)
                return this.Source.GetSymbolSourceLocation(symbol);
            return null;
        }


    }
}
