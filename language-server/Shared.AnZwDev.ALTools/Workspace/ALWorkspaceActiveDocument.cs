using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.Workspace
{
    public class ALWorkspaceActiveDocument
    {

        public ALWorkspace Workspace { get; }
        public ALProject Project { get; private set; }
        public string DocumentPath { get; private set; }
        public SyntaxTree SyntaxTree { get; private set; }

        public ALWorkspaceActiveDocument(ALWorkspace workspace)
        {
            Workspace = workspace;
            Project = null;
            DocumentPath = null;
            SyntaxTree = null;
        }

        public void Update(string path, SyntaxTree syntaxTree)
        {
            this.DocumentPath = path;
            this.SyntaxTree = syntaxTree;
            this.Project = this.Workspace.FindProject(path);
        }

    }
}
