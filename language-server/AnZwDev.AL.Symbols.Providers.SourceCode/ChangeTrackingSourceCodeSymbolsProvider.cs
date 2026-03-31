using AnZwDev.AL.Syntax;
using AnZwDev.System.IO;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.AL.Symbols.Providers.SourceCode
{
    public class ChangeTrackingSourceCodeSymbolsProvider : SourceCodeSymbolsProvider
    {

        private List<IFile> _modifiedFiles = new List<IFile>();

        public ChangeTrackingSourceCodeSymbolsProvider(ISourceCodeProvider sourceCodeProvider) : 
            base(sourceCodeProvider)
        {
        }

        public void FileAdded(IFile file)
        {
            if (!_modifiedFiles.Contains(file))
                _modifiedFiles.Add(file);
        }

        public void FileRemoved(IFile file)
        {
            Symbols?.AllObjects.RemoveReferenceSourceFileName(file.FullPath);
        }

        public void FileChanged(IFile file)
        {
            if (!_modifiedFiles.Contains(file))
                _modifiedFiles.Add(file);
        }

        public override ApplicationSymbol? GetSymbols()
        {
            CompileModifiedFiles();
            return base.GetSymbols();
        }

        private void CompileModifiedFiles()
        {
            if ((Symbols != null) && (_modifiedFiles.Count > 0))
            {
                for (int i = 0; i < _modifiedFiles.Count; i++)
                {
                    Symbols!.AllObjects.RemoveReferenceSourceFileName(_modifiedFiles[i].FullPath);
                    CompileFile(this.Symbols!, _modifiedFiles[i], new ParseOptions(this.Symbols!.Metadata.BCRuntimeVersion, this.Symbols!.Metadata.PreprocessorSymbols));
                }
                _modifiedFiles.Clear();
            }
        }

    }
}
