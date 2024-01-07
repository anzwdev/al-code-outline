using AnZwDev.ALTools.SyntaxHelpers;
using AnZwDev.ALTools.Workspace;
using AnZwDev.ALTools.Workspace.ALCompiler;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using Microsoft.Dynamics.Nav.CodeAnalysis.Text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AnZwDev.ALTools.CodeTransformations.Namespaces
{

#if BC

    public class AddProjectNamespacesConverter
    {
        public string RootNamespace { get; set; }
        public bool UseFolderStructure { get; set; }

        public ProjectChangeResult AddNamespacesToProject(ALWorkspace workspace, ALProject project, string rootNamespace, bool useFolderStructure)
        {
            try
            {
                var projectNamespaces = CollectProjectNamespaces(project);

                var noOfChanges = UpdateNamespaceNames(project, projectNamespaces, rootNamespace, useFolderStructure);
                noOfChanges += UpdateUsings(projectNamespaces);

                (var noOfChangedFiles, var success, var errorMessage) = UpdateFiles(projectNamespaces, false, false);

                return new ProjectChangeResult(noOfChangedFiles, noOfChanges, success, errorMessage);
            }
            catch (Exception exception)
            {
                return new ProjectChangeResult(exception);
            }
        }

        private (int, bool, string) UpdateFiles(Dictionary<string, FileNamespacesInformation> projectNamespaces, bool removeUnused, bool sort)
        {
            var noOfChangedFiles = 0;

            foreach (var fileNamespaces in projectNamespaces.Values)
                if (fileNamespaces.FileUpdateRequired)
                {
                    try
                    {
                        var compilationUnit = fileNamespaces.SyntaxTree.GetRoot() as CompilationUnitSyntax;
                        if (compilationUnit != null)
                        {
                            compilationUnit = UpdateNamespaces(compilationUnit, fileNamespaces, removeUnused, sort);
                            File.WriteAllText(fileNamespaces.Path, compilationUnit.ToFullString());
                            noOfChangedFiles++;
                        }
                    }
                    catch (Exception exception)
                    {
                        return (noOfChangedFiles, false, exception.Message);
                    }
                }
            return (noOfChangedFiles, true, null);
        }

        private CompilationUnitSyntax UpdateNamespaces(CompilationUnitSyntax compilationUnit, FileNamespacesInformation fileNamespaces, bool removeUnused, bool sort)
        {
            compilationUnit = UpdateNamespaceName(compilationUnit, fileNamespaces.Namespace);
            compilationUnit = AddUsings(compilationUnit, fileNamespaces, removeUnused);
            return compilationUnit;
        }

        private CompilationUnitSyntax AddUsings(CompilationUnitSyntax compilationUnit, FileNamespacesInformation fileNamespaces, bool removeUnused)
        {
            var usingsList = new List<UsingDirectiveSyntax>();
            var usingsEnumerable = fileNamespaces.Usings.Values.OrderBy(p => p.Namespace);

            foreach (var namespaceUsing in usingsEnumerable)
                if ((!namespaceUsing.InUsing) && (namespaceUsing.UsingRequired) && (namespaceUsing.Namespace != fileNamespaces.Namespace))
                {
                    var usingSyntax = SyntaxFactory.UsingDirective(
                        SyntaxFactory.Token(SyntaxKind.UsingKeyword),
                        ExpressionFactory.NamespaceName(namespaceUsing.Namespace)
                            .WithLeadingTrivia(SyntaxFactory.WhiteSpace(" ")),
                        SyntaxFactory.Token(SyntaxKind.SemicolonToken));

                    usingSyntax = usingSyntax.WithTrailingTrivia(
                        SyntaxFactory.EndOfLine("\n"));
                    usingsList.Add(usingSyntax);
                }

            if (usingsList.Count > 0)
            {
                if (compilationUnit.Usings.Count == 0)
                    usingsList[usingsList.Count - 1] = usingsList[usingsList.Count - 1]
                        .WithTrailingTrivia(
                            SyntaxFactory.EndOfLine("\n"),
                            SyntaxFactory.WhiteSpace("    "),
                            SyntaxFactory.EndOfLine("\n")); ;

                compilationUnit = compilationUnit.WithUsings(
                    compilationUnit.Usings.AddRange(usingsList));
            }

            return compilationUnit;
        }

        private CompilationUnitSyntax UpdateNamespaceName(CompilationUnitSyntax compilationUnit, string namespaceName)
        {
            if (!String.IsNullOrWhiteSpace(namespaceName))
            {
                var existingNamespace = compilationUnit.NamespaceDeclaration?.Name?.ToString();
                if (existingNamespace != namespaceName)
                {
                    var newName = ExpressionFactory.NamespaceName(namespaceName);
                    if (compilationUnit.NamespaceDeclaration?.Name != null)
                        newName = newName.WithTriviaFrom(compilationUnit.NamespaceDeclaration.Name);
                    else
                        newName = newName.WithLeadingTrivia(SyntaxFactory.WhiteSpace(" "));

                    var newNamespaceDeclaration = compilationUnit.NamespaceDeclaration;
                    if (newNamespaceDeclaration == null)
                        newNamespaceDeclaration = SyntaxFactory.NamespaceDeclaration(newName)
                            .WithTrailingTrivia(
                                SyntaxFactory.EndOfLine("\n"), 
                                SyntaxFactory.EndOfLine("\n"));
                    else
                        newNamespaceDeclaration = newNamespaceDeclaration.WithName(newName);

                    compilationUnit = compilationUnit.WithNamespaceDeclaration(newNamespaceDeclaration);
                }
            }
            return compilationUnit;
        }


        private int UpdateUsings(Dictionary<string, FileNamespacesInformation> projectNamespaces)
        {
            var noOfChanges = 0;
            foreach (var fileNamespaces in projectNamespaces.Values)
                foreach (var usingInformation in fileNamespaces.Usings.Values)
                {
                    if ((String.IsNullOrWhiteSpace(usingInformation.Namespace)) && (projectNamespaces.ContainsKey(usingInformation.FilePath)))
                    {
                        usingInformation.Namespace = projectNamespaces[usingInformation.FilePath].Namespace;
                        fileNamespaces.FileUpdateRequired = true;
                        noOfChanges++;
                    }
                }
            return noOfChanges;
        }

        private int UpdateNamespaceNames(ALProject project, Dictionary<string, FileNamespacesInformation> projectNamespaces, string rootNamespace, bool useFolderStructure)
        {
            var noOfChanges = 0;
            foreach (var fileNamespaceInformation in projectNamespaces.Values)
                if (String.IsNullOrWhiteSpace(fileNamespaceInformation.Namespace))
                {
                    fileNamespaceInformation.FileUpdateRequired = true;
                    fileNamespaceInformation.Namespace = NamespacesHelper.GetNamespaceName(
                        project.RootPath, 
                        fileNamespaceInformation.Path, 
                        rootNamespace, 
                        useFolderStructure);
                    noOfChanges++;
                }
            return noOfChanges;
        }

        private Dictionary<string, FileNamespacesInformation> CollectProjectNamespaces(ALProject project)
        {
            var projectNamespacesInformation = new Dictionary<string, FileNamespacesInformation>();
            foreach (var file in project.Files)
            {
                var fileNamespaceInformation = CollectFileNamespaces(project, file.FullPath);
                if (fileNamespaceInformation != null)
                    projectNamespacesInformation.Add(fileNamespaceInformation.Path, fileNamespaceInformation);
            }
            return projectNamespacesInformation;
        }

        private FileNamespacesInformation CollectFileNamespaces(ALProject project, string filePath)
        {
            var parseOptions = project.GetSyntaxTreeParseOptions();
            var sourceText = SourceText.From(System.IO.File.ReadAllText(filePath));
            var syntaxTree = SyntaxTree.ParseObjectText(sourceText, filePath, parseOptions);

            SyntaxTreeUsingsCollector usings = new SyntaxTreeUsingsCollector();
            return usings.CollectUsings(project, syntaxTree, filePath);
        }

    }

#endif

}
