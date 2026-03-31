using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.AL.Workspaces.ChangeTracking
{
    internal class WorkspaceChangeHandler
    {

        public Workspace Workspace { get; }

        public WorkspaceChangeHandler(Workspace workspace)
        {
            Workspace = workspace;
        }

        public void ProjectsListChanged()
        {
            Workspace.DependencyResolver.Resolve();
        }

    }
}
