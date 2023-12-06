using AnZwDev.ALTools.Core;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.CommandLine;
using Microsoft.Dynamics.Nav.CodeAnalysis.Diagnostics;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using Microsoft.Dynamics.Nav.CodeAnalysis.Text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace AnZwDev.ALTools.Workspace.ALCompiler
{

#if BC

    public class ALProjectCompilation
    {

        public ALProject Project { get; }
        public List<SyntaxTree> SyntaxTrees { get; } = new List<SyntaxTree>();
        public Compilation Compilation { get; private set; }

        private readonly Dictionary<string, ALFileContent> _additionalFiles = new Dictionary<string, ALFileContent>();

        public ALProjectCompilation(ALProject project, params ALFileContent[] additionalFiles)
        {
            Project = project;
            LoadFiles(additionalFiles);
            CreateCompilation();
        }

        private void LoadFiles(ALFileContent[] additionalFiles)
        {
            var parseOptions = Project.GetSyntaxTreeParseOptions();
            if (additionalFiles != null)
                LoadAdditionalFiles(parseOptions, additionalFiles);
            LoadProjectFiles(parseOptions);
        }

        private void LoadAdditionalFiles(ParseOptions parseOptions, ALFileContent[] additionalFiles)
        {
            for (int i = 0; i < additionalFiles.Length; i++)
            {
                var file = additionalFiles[i];
                if (file.SyntaxTree == null)
                {
                    var fileSourceText = SourceText.From(file.Content);
                    file.SyntaxTree = SyntaxTree.ParseObjectText(fileSourceText, file.FullFilePath, parseOptions);
                }
                SyntaxTrees.Add(file.SyntaxTree);
                _additionalFiles.Add(file.FullFilePath, file);
            }
        }

        private void LoadProjectFiles(ParseOptions parseOptions)
        {
            var filePaths = Directory.GetFiles(Project.RootPath, "*", SearchOption.AllDirectories);
            for (int i = 0; i < filePaths.Length; i++)
            {
                if (!_additionalFiles.ContainsKey(filePaths[i]))
                {
                    var fileSourceText = SourceText.From(FileUtils.SafeReadAllText(filePaths[i]));
                    var syntaxTree = SyntaxTree.ParseObjectText(fileSourceText, filePaths[i], parseOptions);
                    SyntaxTrees.Add(syntaxTree);
                }
            }
        }

        private void CreateCompilation()
        {
            List<Diagnostic> diagnostics = new List<Diagnostic>();

            //load project manifest
            var projectFile = Path.Combine(Project.RootPath, "app.json");
            var manifest = ProjectManifest.ReadFromString(projectFile, FileUtils.SafeReadAllText(projectFile), diagnostics);

            //initialize compilation options
            var compilationOptions = CreateCompilationOptions(manifest);

            //create compilation
            var compilation = Microsoft.Dynamics.Nav.CodeAnalysis.Compilation.Create("MyCompilation", manifest.AppManifest.AppPublisher,
                manifest.AppManifest.AppVersion, manifest.AppManifest.AppId,
                null, SyntaxTrees,
                compilationOptions);

            var packageCachePathList = new List<string>
            {
                Path.Combine(Project.RootPath, Project.PackageCachePath)
            };

            LocalCacheSymbolReferenceLoader referenceLoader =
                this.SafeCreateLocalCacheSymbolReferenceLoader(packageCachePathList);

            compilation = compilation
                .WithReferenceLoader(referenceLoader)
                .WithReferences(manifest.GetAllReferences());

            Compilation = compilation;
        }

        protected LocalCacheSymbolReferenceLoader SafeCreateLocalCacheSymbolReferenceLoader(IEnumerable<string> packagesCachePath)
        {
            //return new LocalCacheSymbolReferenceLoader(packagesCachePath);
            Type type = typeof(LocalCacheSymbolReferenceLoader);
            ConstructorInfo[] constructors = type.GetConstructors();
            ParameterInfo[] parameters = constructors[0].GetParameters();
            object[] parametersValues = new object[parameters.Length];
            parametersValues[0] = packagesCachePath;
            for (int i = 1; i < parametersValues.Length; i++)
            {
                parametersValues[i] = null;
            }
            return (LocalCacheSymbolReferenceLoader)constructors[0].Invoke(parametersValues);
        }

        protected virtual CompilerFeatures GetCompilerFeatures(ProjectManifest manifest)
        {
            return manifest.AppManifest.CompilerFeatures;
        }

        protected virtual CompilationOptions CreateCompilationOptions(ProjectManifest manifest)
        {
            return new CompilationOptions(
                target: manifest.AppManifest.Target,
                compilerFeatures: GetCompilerFeatures(manifest));
        }

    }

#endif
}
