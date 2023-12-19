using AnZwDev.ALTools.Workspace;
using AnZwDev.ALTools.Workspace.ALCompiler;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.CodeTransformations.Namespaces
{

#if BC

    public class AddProjectNamespacesConverter
    {

        public string RootNamespace { get; set; }
        public bool UseFolderStructure { get; set; }

        public void AddNamespacesToProject(ALWorkspace workspace, ALProject project, string rootNamespace, bool useFolderStructure)
        {

        }

        public void TestFile(ALProject project, string filePath)
        {
            SyntaxTree tree = SyntaxTree.ParseObjectText(System.IO.File.ReadAllText(filePath));
            SyntaxTreeUsingsCollector usings = new SyntaxTreeUsingsCollector();
            usings.Project = project;

            usings.CollectUsings(tree.GetRoot() as CompilationUnitSyntax);

            foreach (var usingInformation in usings.UsingInformationCollection.Values)
            {
                Console.WriteLine(usingInformation.Namespace);
            }

        }

    }

#endif

}
