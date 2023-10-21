using AnZwDev.ALTools.ALSymbols.SymbolReaders;
using AnZwDev.ALTools.Logging;
using AnZwDev.ALTools.Workspace;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.ALTools.ALSymbols
{
    public class ALSyntaxTree
    {

        public string Path { get; set; }
        public ALSyntaxTreeSymbol RootSymbolWithProperties { get; set; }
        public ALSymbol RootSymbol { get; set; }
        public int OpenCount { get; set; }

        protected ALBaseSyntaxTreeSymbolsReader SyntaxTreeReader { get; set; }

        public ALSyntaxTree(string filePath, ALBaseSyntaxTreeSymbolsReader syntaxTreeReader)
        {
            this.Path = filePath;
            this.SyntaxTreeReader = syntaxTreeReader;
            this.OpenCount = 0;
        }

        public void Load(string content, ALProject project)
        {
            try
            {
                if (String.IsNullOrWhiteSpace(content))
                    this.RootSymbolWithProperties = this.SyntaxTreeReader.ProcessSourceFile(this.Path, project);
                else
                    this.RootSymbolWithProperties = this.SyntaxTreeReader.ProcessSourceCode(content, project);
            }
            catch (Exception e)
            {
                MessageLog.LogError(e);
                this.RootSymbolWithProperties = new ALSyntaxTreeSymbol();
                this.RootSymbolWithProperties.fullName = e.Message;
            }

            if (this.RootSymbolWithProperties != null)
                this.RootSymbol = this.RootSymbolWithProperties.ToALSymbolInformation();
            else
                this.RootSymbol = null;
        }

        public ALSyntaxTreeSymbol GetSyntaxTreeSymbolByPath(int[] path)
        {
            if ((this.RootSymbolWithProperties != null) && (path != null) && (path.Length > 0))
            {
                ALSyntaxTreeSymbol symbol = this.RootSymbolWithProperties;
                for (int i = path.Length - 1; i >= 0; i--)
                {
                    if ((symbol.childSymbols == null) || (path[i] >= symbol.childSymbols.Count))
                        return null;
                    if (path[i] == -1)
                        symbol = this.RootSymbolWithProperties;
                    else
                        symbol = (ALSyntaxTreeSymbol)symbol.childSymbols[path[i]];
                }
                return symbol;
            }

            return null;            
        }

    }
}
