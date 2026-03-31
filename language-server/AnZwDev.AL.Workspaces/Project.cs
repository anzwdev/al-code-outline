using AnZwDev.AL.Symbols;
using AnZwDev.AL.Workspaces.ChangeTracking;
using AnZwDev.AL.Workspaces.Symbols;
using AnZwDev.System.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AnZwDev.AL.Workspaces
{
    public class Project
    {

        public Workspace Workspace { get; }
        
        public string RootPath { get; }
        public ProjectSymbolsProvider SymbolsProvider { get; }
        public ProjectSymbolsView Symbols { get; }

        public ProjectSettings Settings { get; private set; }
        public AppSourceCopSettings? AppSourceCopSettings { get; private set; }
        public ProjectFilesList Files { get; }

        internal ProjectChangeHandler ChangeHandler { get; }

        public Project(Workspace workspace, ProjectDescriptor projectDescriptor)
        {
            var rootPath = PathUtils.NormalizePath(projectDescriptor.ProjectPath ?? String.Empty);

            RootPath = rootPath;
            Workspace = workspace;
            Settings = projectDescriptor.Settings ?? new ProjectSettings();
            AppSourceCopSettings = null;
            Files = new ProjectFilesList(this);
            SymbolsProvider = new ProjectSymbolsProvider(this);
            Symbols = new ProjectSymbolsView(SymbolsProvider);

            ChangeHandler = new ProjectChangeHandler();
        }

        public void Load()
        {
            var filePaths = Directory.GetFiles(this.RootPath, "*.*", SearchOption.AllDirectories);
            Files.AddRange(filePaths, true);
            SymbolsProvider.ProjectCodeSymbolsProvider.Load(false);
            LoadAppSourceCopJson();
        }

        public void Update(ProjectDescriptor projectDescriptor)
        {
            var prevPackagesPath = Settings.PackagesCachePath;
            
            Settings = projectDescriptor.Settings ?? new ProjectSettings();

            if (prevPackagesPath != Settings.PackagesCachePath)
                Workspace.DependencyResolver.Resolve(this);
        }

        public void ReloadAppJson()
        {
            var changes = SymbolsProvider.ProjectCodeSymbolsProvider.ReloadMetadata();
            if (changes.AppIdChanged)
                Workspace.DependencyResolver.Resolve();
            else if (changes.DependenciesChanged)
                Workspace.DependencyResolver.Resolve(this);
        }

        public void LoadAppSourceCopJson()
        {
            AppSourceCopSettings = AppSourceCopSettings.LoadFromFile(Files.AppSourceCopJson?.FullPath);
        }

        public string GetPackagesCacheFullPath()
        {
            var packagesFullPath = (String.IsNullOrWhiteSpace(Settings.PackagesCachePath)) ? WorkspacesConst.DefaultPackagesCachePath : Settings.PackagesCachePath;
            if (!Path.IsPathRooted(packagesFullPath))
                packagesFullPath = Path.Combine(RootPath, packagesFullPath);
            return packagesFullPath;
        }

    }
}
