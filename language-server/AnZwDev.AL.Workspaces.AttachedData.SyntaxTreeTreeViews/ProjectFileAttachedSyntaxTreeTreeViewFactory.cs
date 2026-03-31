using AnZwDev.AL.Workspaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.AL.Workspaces.AttachedData.SyntaxTreeSymbolsTreeViews
{
    public class ProjectFileAttachedSyntaxTreeTreeViewFactory : ProjectFileAttachedDataFactory<ProjectFileAttachedSyntaxTreeTreeView>
    {

        public static ProjectFileAttachedSyntaxTreeTreeViewFactory Instance { get; } = new ProjectFileAttachedSyntaxTreeTreeViewFactory();

        public ProjectFileAttachedSyntaxTreeTreeViewFactory() : base()
        {
        }

        public override ProjectFileAttachedSyntaxTreeTreeView Create(ProjectFile projectFile)
        {
            return new ProjectFileAttachedSyntaxTreeTreeView(projectFile);
        }


    }
}
