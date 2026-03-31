using AnZwDev.AL.Symbols.Providers.AppPackages;
using AnZwDev.AL.Symbols.Providers.SourceCode;
using AnZwDev.AL.Syntax;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.AL.Symbols.Tests
{
    internal static class ProjectLoaderHelpers
    {

        public static ApplicationSymbol? LoadFromAppFile(string projectPath, bool metadataOnly, AppPackageSymbolsCache? cache)
        {
            var appFile = FindFirstAppFile(projectPath);
            var loader = new AppPackageSymbolsProvider(appFile, cache);
            loader.Load(metadataOnly);
            return loader.GetSymbols();
        }

        public static ApplicationSymbol? LoadFromProjectBuild(string projectPath, bool metadataOnly)
        {
            var sourceCodeProvider = new FileSystemSourceCodeProvider(projectPath);
            var loader = new SourceCodeSymbolsProvider(sourceCodeProvider);
            loader.Load(metadataOnly);
            return loader.GetSymbols();
        }

        public static ApplicationSymbol? LoadFromChangeTrackingProjectBuild(string projectPath, bool metadataOnly)
        {
            var sourceCodeProvider = new FileSystemSourceCodeProvider(projectPath);
            var loader = new ChangeTrackingSourceCodeSymbolsProvider(sourceCodeProvider);
            loader.Load(metadataOnly);
            return loader.GetSymbols();
        }

        private static string FindFirstAppFile(string projectPath)
        {
            var files = Directory.GetFiles(projectPath, "*.app", SearchOption.TopDirectoryOnly);
            if (files.Length == 0)
                throw new Exception("No .app files found in " + projectPath);
            return files[0];
        }

    }
}
