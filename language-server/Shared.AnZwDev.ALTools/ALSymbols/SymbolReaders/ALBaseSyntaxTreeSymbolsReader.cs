using AnZwDev.ALTools.Logging;
using AnZwDev.ALTools.Workspace;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.ALSymbols.SymbolReaders
{
    public abstract class ALBaseSyntaxTreeSymbolsReader
    {

        public ALBaseSyntaxTreeSymbolsReader()
        {
        }

        public ALSyntaxTreeSymbol ProcessSourceFile(string path, ALProject project)
        {
            string sourceCode;
            try
            {
                System.IO.StreamReader reader = new System.IO.StreamReader(path);
                sourceCode = reader.ReadToEnd();
                reader.Close();
                reader.Dispose();

                return ProcessSourceCode(sourceCode, project);
            }
            catch (Exception e)
            {
                MessageLog.LogError(e);
                return new ALSyntaxTreeSymbol(ALSymbolKind.Undefined, "LangServer Error: " + e.Message);
            }
        }

        public abstract ALSyntaxTreeSymbol ProcessSourceCode(string source, ALProject project);

    }
}
