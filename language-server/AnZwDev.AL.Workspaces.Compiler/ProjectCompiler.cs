using AnZwDev.AL.Symbols.CodeAnalysis;
using AnZwDev.AL.Workspaces.AttachedData.SyntaxTrees;
using AnZwDev.System.IO;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.CommandLine;
using Microsoft.Dynamics.Nav.CodeAnalysis.Diagnostics;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using Microsoft.Dynamics.Nav.CodeAnalysis.Text;
using Microsoft.Dynamics.Nav.CodeAnalysis.Utilities;
using System.Text;

namespace AnZwDev.AL.Workspaces.Compiler
{
    public class ProjectCompiler
    {

        public Project Project { get; }
        public Dictionary<string, string> FileContentOverride { get; } = new Dictionary<string, string>(PathUtils.GetPathComparer());
        public Compilation? Compilation { get; private set; } = null;
        public Dictionary<string, SyntaxTree> SyntaxTrees { get; } = new Dictionary<string, SyntaxTree>(PathUtils.GetPathComparer());

        public CompilerFeatures AddCompilerFeatures { get; set; } = CompilerFeatures.None;
        public CompilerFeatures RemoveCompilerFeatures { get; set; } = CompilerFeatures.None;

        public ProjectCompiler(Project project)
        {
            Project = project;
        }

        public void Compile()
        {
            Compilation = CreateCompilation();
        }

        private Compilation? CreateCompilation()
        {
            if (Project.Files.AppJson == null)
                return null;

            List<Diagnostic> diagnostics = new List<Diagnostic>();

            //load project manifest
            var appJsonFile = Project.Files.AppJson;
            var manifest = ProjectManifest.ReadFromString(appJsonFile.FullPath, appJsonFile.ReadAllText(), diagnostics);
            if (manifest == null)
                return null;

            //load all syntax trees
            LoadSyntaxTrees();

            //initialize compilation options
            var compilationOptions = CreateCompilationOptions(manifest);

            //create compilation
            Compilation compilation = Compilation.Create("MyCompilation", 
                manifest.AppManifest.AppPublisher,
                manifest.AppManifest.AppVersion, manifest.AppManifest.AppId,
                null, SyntaxTrees.Values,
                compilationOptions);

            List<string> packageCachePathList = [Project.GetPackagesCacheFullPath()];
            LocalCacheSymbolReferenceLoader referenceLoader = new LocalCacheSymbolReferenceLoader(packageCachePathList);

            compilation = compilation
                .WithReferenceLoader(referenceLoader)
                .WithReferences(manifest.GetAllReferences());

            return compilation;
        }

        private void LoadSyntaxTrees()
        {
            var symbols = Project.SymbolsProvider.ProjectCodeSymbolsProvider.GetSymbols();
            if (symbols == null)
                return;

            var parseOptions = symbols.GetParseOptions();

            for (int i = 0; i < Project.Files.Count; i++)
                if (Project.Files[i].Type == ProjectFileType.AL)
                {
                    var file = Project.Files[i];
                    var attachedSyntaxTree = file.AttachedData.Get(ProjectFileAttachedSyntaxTreeFactory.Instance);
                    var fileSyntaxTree = attachedSyntaxTree.Get(parseOptions);
                    SyntaxTrees.Add(file.FullPath, fileSyntaxTree);
                }
        }

        private CompilationOptions CreateCompilationOptions(ProjectManifest manifest)
        {
            var compilerFeatures = manifest.AppManifest.CompilerFeatures;
            if (AddCompilerFeatures != CompilerFeatures.None)
                compilerFeatures |= AddCompilerFeatures;
            if (RemoveCompilerFeatures != CompilerFeatures.None)
                compilerFeatures &= ~RemoveCompilerFeatures;

            return new CompilationOptions(
                target: manifest.AppManifest.Target,
                compilerFeatures: compilerFeatures);
        }

    }
}
