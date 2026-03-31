using AnZwDev.AL.Symbols.Providers;
using AnZwDev.AL.Symbols.Providers.AppPackages;
using AnZwDev.AL.Workspaces.ChangeTracking;
using AnZwDev.System.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AnZwDev.AL.Workspaces
{
    public class Workspace
    {

        public ProjectsList Projects { get; }
        public DependencyResolver DependencyResolver { get; }
        public AppPackageSymbolsCache SymbolsCache { get; } = new AppPackageSymbolsCache();
        public IServiceProvider Services { get; }

        internal WorkspaceChangeHandler ChangeHandler { get; }

        public Workspace(IServiceProvider services)
        {
            Services = services;
            Projects = new ProjectsList(this);
            DependencyResolver = new DependencyResolver(this);
            ChangeHandler = new WorkspaceChangeHandler(this);
        }

    }
}
