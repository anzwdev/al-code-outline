using AnZwDev.AL.Workspaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.AL.Workspaces.AttachedData.SyntaxTreeSymbolsTreeViews
{
    public class ProjectFileAttachedSyntaxTreeSymbolsTreeViewFactory : ProjectFileAttachedDataFactory<ProjectFileAttachedSyntaxTreeSymbolsTreeView>
    {

        public static ProjectFileAttachedSyntaxTreeSymbolsTreeViewFactory Instance { get; } = new ProjectFileAttachedSyntaxTreeSymbolsTreeViewFactory();

        public ProjectFileAttachedSyntaxTreeSymbolsTreeViewFactory() : base()
        {
        }

        public override ProjectFileAttachedSyntaxTreeSymbolsTreeView Create(ProjectFile projectFile)
        {
            return new ProjectFileAttachedSyntaxTreeSymbolsTreeView(projectFile);
        }


    }
}
