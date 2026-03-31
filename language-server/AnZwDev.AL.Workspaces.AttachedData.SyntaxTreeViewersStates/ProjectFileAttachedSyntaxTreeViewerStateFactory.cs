using AnZwDev.AL.Workspaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.AL.Workspaces.AttachedData.SyntaxTreeViewersStates
{
    public class ProjectFileAttachedSyntaxTreeViewerStateFactory : ProjectFileAttachedDataFactory<ProjectFileAttachedSyntaxTreeViewerState>
    {

        public static ProjectFileAttachedSyntaxTreeViewerStateFactory Instance { get; } = new ProjectFileAttachedSyntaxTreeViewerStateFactory();

        public ProjectFileAttachedSyntaxTreeViewerStateFactory() : base()
        {
        }

        public override ProjectFileAttachedSyntaxTreeViewerState Create(ProjectFile projectFile)
        {
            return new ProjectFileAttachedSyntaxTreeViewerState(projectFile);
        }


    }
}
