using AnZwDev.ALTools.Workspace;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.CodeTransformations.Namespaces
{
    internal class FileNamespacesInformation
    {

        public SyntaxTree SyntaxTree { get; set; }
        public ALProject Project { get; }
        public string Path { get; }
        public string Namespace { get; set; } = null;
        public Dictionary<string, NamespaceUsingReference> Usings { get; set; } = new Dictionary<string, NamespaceUsingReference>();
        public bool FileUpdateRequired { get; set; } = false;

        public FileNamespacesInformation(ALProject project, string path, SyntaxTree syntaxTree)
        {
            this.Project = project;
            this.Path = path;
            this.SyntaxTree = syntaxTree;
        }

        public void AddNamespaceReference(string namespaceName, string filePath, bool inUsing, bool inVariableDeclaration, bool usingRequired)
        {
            NamespaceUsingReference usingInformation = new NamespaceUsingReference(namespaceName, filePath, inUsing, inVariableDeclaration, usingRequired);
            if (Usings.ContainsKey(usingInformation.Id))
                Usings[usingInformation.Id].Merge(usingInformation);
            else
                Usings.Add(usingInformation.Id, usingInformation);
        }

        public void AddFileUsings(HashSet<string> usings) 
        {
            if (usings != null)
                foreach (var namespaceName in usings)
                    AddNamespaceReference(namespaceName, null, true, false, false);
        }

        public bool UsingRequired(string namespaceName)
        {
            return
                (Usings != null) &&
                (Usings.ContainsKey(namespaceName)) &&
                (Usings[namespaceName].UsingRequired);
        }

    }
}
