using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace AnZwDev.AL.Workspaces.AttachedData.SyntaxTrees
{
    public class ProjectFileAttachedSyntaxTreeFactory : ProjectFileAttachedDataFactory<ProjectFileAttachedSyntaxTree>
    {

        public static ProjectFileAttachedSyntaxTreeFactory Instance { get; } = new ProjectFileAttachedSyntaxTreeFactory();

        public ProjectFileAttachedSyntaxTreeFactory() : base()
        {
        }

        public override ProjectFileAttachedSyntaxTree Create(ProjectFile projectFile)
        {
            return new ProjectFileAttachedSyntaxTree(projectFile);
        }

    }
}
