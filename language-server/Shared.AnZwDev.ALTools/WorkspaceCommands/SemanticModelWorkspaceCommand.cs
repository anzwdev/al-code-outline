﻿using AnZwDev.ALTools.ALSymbols;
using AnZwDev.ALTools.Core;
using AnZwDev.ALTools.Extensions;
using AnZwDev.ALTools.Logging;
using AnZwDev.ALTools.Workspace;
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

namespace AnZwDev.ALTools.WorkspaceCommands
{

#if BC
    public class SemanticModelWorkspaceCommand: WorkspaceCommand
    {

        public SemanticModelWorkspaceCommand(ALDevToolsServer alDevToolsServer, string name, bool modifiedSymbolsRebuildRequired) : base(alDevToolsServer, name, modifiedSymbolsRebuildRequired)
        {
        }

        public override WorkspaceCommandResult Run(string sourceCode, ALProject project, string filePath, TextRange range, Dictionary<string, string> parameters, List<string> excludeFiles, List<string> includeFiles)
        {
            SourceText sourceText = null;
            SyntaxTree sourceSyntaxTree = null;
            string newSourceCode = null;
            bool success = true;
            string errorMessage = null;
            ParseOptions parseOptions = project.GetSyntaxTreeParseOptions();

            //load project and single file
            List<SyntaxTree> syntaxTrees = new List<SyntaxTree>();
            Compilation compilation = this.LoadProject(project.RootPath, parseOptions, syntaxTrees, sourceCode, filePath, project.PackageCachePath, out sourceSyntaxTree, out sourceText);

            if (!String.IsNullOrWhiteSpace(filePath))
            {
                if ((!String.IsNullOrEmpty(sourceCode)) && (sourceSyntaxTree != null))
                {
                    (newSourceCode, success, errorMessage) = this.ProcessSourceCode(sourceText, sourceSyntaxTree, compilation, project, range, parameters);
                    if (!success)
                        return new WorkspaceCommandResult(newSourceCode, true, errorMessage);
                }
            }
            else
                (success, errorMessage) = this.ProcessDirectory(syntaxTrees, compilation, project, parameters, excludeFiles, includeFiles);

            if (success)
                return new WorkspaceCommandResult(newSourceCode);
            return new WorkspaceCommandResult(newSourceCode, true, errorMessage);
        }

    #region Project loading

        protected Compilation LoadProject(string projectPath, ParseOptions parseOptions, List<SyntaxTree> syntaxTrees, string sourceCode, string sourcePath, string alPackagesPath, out SyntaxTree sourceSyntaxTree, out SourceText sourceText)
        {
            //load all syntax trees
            syntaxTrees.Clear();
            this.LoadProjectALFiles(projectPath, parseOptions, syntaxTrees, sourceCode, sourcePath, out sourceSyntaxTree, out sourceText);

            List<Diagnostic> diagnostics = new List<Diagnostic>();

            //load project manifest
            string projectFile = Path.Combine(projectPath, "app.json");
            ProjectManifest manifest = ProjectManifest.ReadFromString(projectFile, FileUtils.SafeReadAllText(projectFile), diagnostics);

            //initialize compilation options
            var compilationOptions = CreateCompilationOptions(manifest);

            //create compilation
            Compilation compilation = Compilation.Create("MyCompilation", manifest.AppManifest.AppPublisher,
                manifest.AppManifest.AppVersion, manifest.AppManifest.AppId,
                null, syntaxTrees,
                compilationOptions);

            List<string> packageCachePathList = new List<string>();
            packageCachePathList.Add(Path.Combine(projectPath, alPackagesPath));

            LocalCacheSymbolReferenceLoader referenceLoader =
                this.SafeCreateLocalCacheSymbolReferenceLoader(packageCachePathList);
            
            compilation = compilation
                .WithReferenceLoader(referenceLoader)
                .WithReferences(manifest.GetAllReferences());

            return compilation;
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

        protected LocalCacheSymbolReferenceLoader SafeCreateLocalCacheSymbolReferenceLoader(IEnumerable<string> packagesCachePath)
        {
            //return new LocalCacheSymbolReferenceLoader(packagesCachePath);
            Type type = typeof(LocalCacheSymbolReferenceLoader);
            ConstructorInfo[] constructors = type.GetConstructors();
            ParameterInfo[] parameters = constructors[0].GetParameters();
            object[] parametersValues = new object[parameters.Length];
            parametersValues[0] = packagesCachePath;
            for (int i=1; i< parametersValues.Length; i++)
            {
                parametersValues[i] = null;
            }
            return (LocalCacheSymbolReferenceLoader)constructors[0].Invoke(parametersValues);
        }

        protected void LoadProjectALFiles(string projectPath, ParseOptions parseOptions, List<SyntaxTree> syntaxTrees, string sourceCode, string sourcePath, out SyntaxTree sourceSyntaxTree, out SourceText sourceText)
        {
            bool useSource = (!String.IsNullOrWhiteSpace(sourcePath));
            if (useSource)
                sourcePath = Path.GetFullPath(sourcePath);
            sourceSyntaxTree = null;
            sourceText = null;

            string[] filePaths = Directory.GetFiles(projectPath, "*.al", SearchOption.AllDirectories);
            for (int i = 0; i < filePaths.Length; i++)
            {
                bool sourceFile = ((useSource) && (sourcePath.Equals(Path.GetFullPath(filePaths[i]), PathUtils.GetPathComparison())));
                string content;
                if (sourceFile)
                    content = sourceCode;
                else
                    content = FileUtils.SafeReadAllText(filePaths[i]);

                SourceText fileSourceText = SourceText.From(content);
                SyntaxTree syntaxTree = SyntaxTree.ParseObjectText(fileSourceText, filePaths[i], parseOptions);

                syntaxTrees.Add(syntaxTree);

                if (sourceFile)
                {
                    sourceSyntaxTree = syntaxTree;
                    sourceText = fileSourceText;
                }
            }
        }

    #endregion

    #region Project files processing

        protected (bool, string) ProcessDirectory(List<SyntaxTree> syntaxTrees, Compilation compilation, ALProject project, Dictionary<string, string> parameters, List<string> excludeFiles, List<string> includeFiles)
        {
            HashSet<string> includeFilesHashSet = ((includeFiles != null) && (includeFiles.Count > 0)) ? includeFiles.ToLowerCaseHashSet() : null;

            var matcher = new ExcludedFilesMatcher(excludeFiles);

            //process files
            foreach (SyntaxTree syntaxTree in syntaxTrees)
            {
                if ((includeFilesHashSet == null) || (includeFilesHashSet.Contains(syntaxTree.FilePath.ToLower())))
                {
                    if (matcher.ValidFile(project.RootPath, syntaxTree.FilePath))
                    {
                        (bool success, string errorMessage) = this.ProcessFile(null, syntaxTree, compilation, project, null, parameters);
                        if (!success)
                            return (false, errorMessage);
                    }
                }
            }
            return (true, null);
        }

        protected (bool, string) ProcessFile(SourceText sourceText, SyntaxTree syntaxTree, Compilation compilation, ALProject project, TextRange range, Dictionary<string, string> parameters)
        {
            SyntaxNode rootNode = syntaxTree.GetRoot();
            if (rootNode != null)
            {
                (string newSource, bool success, string errorMessage) = this.ProcessSourceCode(sourceText, syntaxTree, compilation, project, range, parameters);
                if ((success) && (!String.IsNullOrWhiteSpace(newSource)))
                    System.IO.File.WriteAllText(syntaxTree.FilePath, newSource);
                return (success, errorMessage);
            }
            return (true, null);
        }

        protected (string, bool, string) ProcessSourceCode(SourceText sourceText, SyntaxTree syntaxTree, Compilation compilation, ALProject project, TextRange range, Dictionary<string, string> parameters)
        {
            try
            {
                SyntaxNode rootNode = syntaxTree.GetRoot();
                if (rootNode != null)
                {
                    //convert range to TextSpan
                    TextSpan span = new TextSpan(0, 0);
                    if ((range != null) && (sourceText != null))
                    {
                        LinePositionSpan srcRange = new LinePositionSpan(new LinePosition(range.start.line, range.start.character), new LinePosition(range.end.line, range.end.character));
                        span = sourceText.Lines.GetTextSpan(srcRange);
                    }

                    //fix nodes
                    SemanticModel semanticModel = compilation.GetSemanticModel(syntaxTree);
                    SyntaxNode newRootNode = this.ProcessSyntaxNode(syntaxTree, rootNode, semanticModel, project, span, parameters);

                    //return new source code
                    if (newRootNode != null)
                        return (newRootNode.ToFullString(), true, null);
                }
                return (null, true, null);
            }
            catch (Exception ex)
            {
                string filePath = syntaxTree.FilePath;
                string errorMessage = (String.IsNullOrEmpty(filePath)) ?
                    $"Workspace command {this.Name} error during processing source code" : $"Workspace command {this.Name} error during processing file '{filePath}'";
                MessageLog.LogError(ex, errorMessage + ": ");
                return (null, false, errorMessage);
            }
        }

        public virtual SyntaxNode ProcessSyntaxNode(SyntaxTree syntaxTree, SyntaxNode node, SemanticModel semanticModel, ALProject project, TextSpan span, Dictionary<string, string> parameters)
        {
            bool skipFormatting = this.GetSkipFormattingValue(parameters); //((parameters != null) && (parameters.ContainsKey("skipFormatting")) && (parameters["skipFormatting"] == "true"));
            if (!skipFormatting)
                node = FormatSyntaxNode(node);
            return node;
        }

    #endregion

    }
#endif
}
