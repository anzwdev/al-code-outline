using AnZwDev.AL.Symbols.CodeAnalysis;
using AnZwDev.AL.Syntax;
using AnZwDev.System.IO;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using AnZwDev.AL.Symbols.Comparers;
using AnZwDev.AL.Symbols.Providers.SourceCode.Metadata;
using AnZwDev.AL.Symbols.Providers.SourceCode.SymbolsCompilers;

namespace AnZwDev.AL.Symbols.Providers.SourceCode
{
    public class SourceCodeSymbolsProvider : SymbolsProvider
    {

        public ISourceCodeProvider SourceCodeProvider { get; }

        public SourceCodeSymbolsProvider(ISourceCodeProvider sourceCodeProvider)
        {
            SourceCodeProvider = sourceCodeProvider;
        }

        public override void Load(bool metadataOnly)
        {
            var applicationSymbol = LoadFromMetadata();
            if ((applicationSymbol != null) && (!metadataOnly))
                CompileFiles(applicationSymbol);
            SetSymbols(applicationSymbol);
        }

        public MetadataReloadResult ReloadMetadata()
        {
            var result = new MetadataReloadResult();

            var prevApplicationSymbol = Symbols;
            if (prevApplicationSymbol == null)
            {
                Load(false);
                return result;
            }

            //load new symbols
            var applicationSymbol = LoadFromMetadata();

            if (applicationSymbol != null)
            {

                //dependencies and appid differences
                result.AppIdChanged = !String.Equals(prevApplicationSymbol.AppId, applicationSymbol.AppId, StringComparison.InvariantCultureIgnoreCase);

                var prevPreprocessorSymbols = new HashSet<string>(prevApplicationSymbol.Metadata?.PreprocessorSymbols ?? Array.Empty<string>());
                var newPreprocessorSymbols = new HashSet<string>(applicationSymbol.Metadata?.PreprocessorSymbols ?? Array.Empty<string>());
                var preprocessorSymbolsEqual = prevPreprocessorSymbols.SetEquals(newPreprocessorSymbols);

                result.DependenciesChanged =
                    ((prevApplicationSymbol.Metadata?.BCTestVersion) != (applicationSymbol.Metadata?.BCTestVersion)) ||
                    (!ApplicationDependencySymbolComparer.ReferencedAppsEquals(prevApplicationSymbol.Metadata?.Dependencies, applicationSymbol.Metadata?.Dependencies));

                if (preprocessorSymbolsEqual)
                {
                    prevApplicationSymbol.CopyMetadata(applicationSymbol);
                    return result;
                }

                CompileFiles(applicationSymbol);
            }

            SetSymbols(applicationSymbol);
            return result;
        }

        private ApplicationSymbol? LoadFromMetadata()
        {
            if (SourceCodeProvider.AppJsonFile == null)
                return null;

            var json = SourceCodeProvider.AppJsonFile.ReadAllText();
            var metadata = JsonSerializer.Deserialize<AppJsonMetadata>(json);
            var applicationSymbol = metadata?.CreateSymbol(SourceCodeProvider.AppJsonFile.FullPath);
            return applicationSymbol;
        }

        private void CompileFiles(ApplicationSymbol applicationSymbol)
        {
            var parseOptions = applicationSymbol.GetParseOptions();

            foreach (var file in SourceCodeProvider.SourceFiles)
                CompileFile(applicationSymbol, file, parseOptions);
        }

        protected void CompileFile(ApplicationSymbol applicationSymbol, IFile file, ParseOptions parseOptions)
        {
            var sourceCode = file.ReadAllText();
            if (String.IsNullOrWhiteSpace(sourceCode))
                return;

            var syntaxTree = SyntaxFactory.ParseSyntaxTree(sourceCode, file.FullPath, file.Encoding, parseOptions);
            if (syntaxTree == null)
                return;

            var compilationUnit = syntaxTree.GetRoot() as CompilationUnitSyntax;
            if (compilationUnit != null)
                CompilationUnitSymbolCompiler.Compile(compilationUnit, applicationSymbol.AllObjects, file.FullPath);
        }

    }
}
