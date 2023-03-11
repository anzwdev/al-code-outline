using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using AnZwDev.ALTools.Workspace.Serialization;
using AnZwDev.ALTools.ALSymbols.SymbolReaders;
using AnZwDev.ALTools.ALSymbolReferences.Serialization;
using AnZwDev.ALTools.Core;
using AnZwDev.ALTools.ALSymbols;
using AnZwDev.ALTools.Extensions;
using AnZwDev.ALTools.ALSymbolReferences;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using AnZwDev.ALTools.CodeAnalysis;
using AnZwDev.ALTools.ALSymbolReferences.MergedReferences;

namespace AnZwDev.ALTools.Workspace
{
    public class ALProject
    {

        #region Public properties

        public ALWorkspace Workspace { get; set; }
        public ALProjectFilesCollection Files { get; }
        public string RootPath { get; set; }
        public string PackageCachePath { get; private set; }
        public List<string> CodeAnalyzers { get; private set; }

        private ALProjectProperties _properties;
        public ALProjectProperties Properties 
        { 
            get { return _properties; }
            set
            {
                _properties = value;
                OnPropertiesChanged();
            }
        }

        public List<string> MandatoryPrefixes { get; set; }
        public List<string> MandatorySuffixes { get; set; }
        public List<string> MandatoryAffixes { get; set; }
        public List<string> AdditionalMandatoryAffixesPatterns { get; set; }
        public ALProjectDependenciesCollection Dependencies { get; }
        
        /// <summary>
        /// Symbols defined in project
        /// </summary>
        public ALAppSymbolReference Symbols { get; set; }
        
        /// <summary>
        /// All symbols defined in project and in dependencies
        /// </summary>
        public MergedALAppSymbolReference AllSymbols { get; }


        #endregion

        #region Initialization

        public ALProject() : this(null, null)
        {
        }

        public ALProject(ALWorkspace workspace, ALProjectSource projectSource)
        {
            
            if (!String.IsNullOrWhiteSpace(projectSource?.packageCachePath))
                this.PackageCachePath = projectSource.packageCachePath;
            else
                this.PackageCachePath = ".alpackages";
            this.CodeAnalyzers = projectSource?.codeAnalyzers;
            this.AdditionalMandatoryAffixesPatterns = projectSource?.additionalMandatoryAffixesPatterns;
            this.MandatoryPrefixes = null;
            this.MandatorySuffixes = null;
            this.Workspace = workspace;
            this.RootPath = projectSource?.folderPath;
            this.Files = new ALProjectFilesCollection(this);
            this.Dependencies = new ALProjectDependenciesCollection();
            this.Symbols = new ALAppSymbolReference();
            this.Properties = null;
            this.AllSymbols = new MergedALAppSymbolReference(new ALProjectAllALAppSymbolReferencesCollection(this));
        }

        #endregion

        #region Loading

        public void Load()
        {
            this.Load(true);
        }

        protected void Load(bool reloadFiles)
        {
            //load app.json
            string projectManifestPath = Path.Combine(this.RootPath, "app.json");
            if (File.Exists(projectManifestPath))
            {
                ALProjectMetadataSerializer.LoadFromFile(this, projectManifestPath);
                if (reloadFiles)
                {
                    this.Files.Clear();
                    this.Files.Load();
                }
            }
            else
            {
                this.Properties = null;
                this.Files.Clear();
                this.Dependencies.Clear();
            }
            //load AppSourceCop.json
            this.LoadAppSourceCopSettings();
        }

        protected void ReloadProjectManifest()
        {
            if (this.Properties == null)
            {
                //it is not al project - app.json was missing, try to load it and if it exists, reload all files and dependencies
                this.Load();
                if (this.Properties != null)
                    this.Workspace.ResolveDependencies();
            }
            else
            {
                //this is already al project - reload manifest, do not reload files
                this.Load(false);
                if (this.Properties != null)
                    this.Workspace.ResolveDependencies();
            }
        }

        protected void LoadAppSourceCopSettings()
        {
            this.MandatoryAffixes = null;
            this.MandatoryPrefixes = null;
            this.MandatorySuffixes = null;

            AppSourceCopSettings appSourceCopSettings = AppSourceCopSettings.FromFile(Path.Combine(this.RootPath, "AppSourceCop.json"));
            if (appSourceCopSettings != null)
            {
                if ((appSourceCopSettings.MandatoryAffixes != null) && (appSourceCopSettings.MandatoryAffixes.Length > 0))
                    this.MandatoryAffixes = new List<string>(appSourceCopSettings.MandatoryAffixes);

                if (!String.IsNullOrWhiteSpace(appSourceCopSettings.MandatoryPrefix))
                    this.MandatoryPrefixes = appSourceCopSettings.MandatoryPrefix.ToSingleElementList();
                
                if (!String.IsNullOrWhiteSpace(appSourceCopSettings.MandatorySuffix))
                    this.MandatorySuffixes = appSourceCopSettings.MandatorySuffix.ToSingleElementList();
            }
        }

        #endregion

        #region Dependencies

        public string GetALPackagesPath()
        {
            var absolute_path = Path.Combine(this.RootPath, this.PackageCachePath);
            absolute_path = Path.GetFullPath((new Uri(absolute_path)).LocalPath);

            return absolute_path;// Path.GetFullPath((new Uri(absolute_path)).LocalPath);
            //return Path.Combine(this.RootPath, this.PackageCachePath);
        }

        public void ResolveDependencies()
        {
            if (this.Properties == null)
                return;

            //remove propagated dependencies
            this.Dependencies.RemovePropagatedDependencies();

            //load package files list
            AppPackageInformationsCollection appPackagesCollection = new AppPackageInformationsCollection();
            appPackagesCollection.AddFromFolder(this.GetALPackagesPath());

            //process dependencies
            //foreach cannot be used because ResolveDependency function can add propagated dependencies to the list
            int index = 0;
            while (index < this.Dependencies.Count)
            {
                this.ResolveDependency(this.Dependencies[index], appPackagesCollection);
                index++;
            }

            //change internal references from ids to names
            if (!this.Dependencies.IdReferencesReplaced())
            {
                ALAppObjectIdMap objectIdMap = this.Dependencies.CreateALAppObjectIdMap();
                this.Dependencies.ReplaceIdReferences(objectIdMap);
            }

        }

        protected void ResolveDependency(ALProjectDependency dependency, AppPackageInformationsCollection appPackagesCollection)
        {
            dependency.SourceProject = this.Workspace.FindProject(dependency.Id, dependency.Name, dependency.Publisher, dependency.Version);
            if (dependency.SourceProject != null)
            {
                dependency.Symbols = dependency.SourceProject.Symbols;
                dependency.SourceAppFile = null;

                dependency.InternalsVisible = (
                    (this.Properties != null) &&
                    (dependency.SourceProject.Properties != null) &&
                    (dependency.SourceProject.Properties.InternalsVisibleTo != null) &&
                    (dependency.SourceProject.Properties.InternalsVisibleTo.FindProjectReference(this.Properties.Id, this.Properties.Name, this.Properties.Publisher) != null));
            }
            else
            {
                dependency.SourceAppFile = appPackagesCollection.Find(dependency.Id, dependency.Name, dependency.Publisher, dependency.Version);
                if (dependency.SourceAppFile != null)
                {
                    dependency.Symbols = this.Workspace.SymbolReferencesCache.GetSymbolReference(dependency.SourceAppFile.FullPath);

                    dependency.InternalsVisible = (
                        (this.Properties != null) &&
                        (dependency.SourceAppFile.Manifest != null) &&
                        (dependency.SourceAppFile.Manifest.InternalsVisibleTo != null) &&
                        (dependency.SourceAppFile.Manifest.InternalsVisibleTo.FindModuleReference(this.Properties.Id, this.Properties.Name, this.Properties.Publisher) != null));

                    //add propagated dependencies
                    NavxPackage navxPackage = AppPackageNavxSerializer.Deserialize(dependency.SourceAppFile.FullPath);

                    if ((navxPackage != null) && (navxPackage.App != null) && (navxPackage.App.PropagateDependencies) && (navxPackage.Dependencies != null))
                    {
                        foreach (NavxDependency navxDependency in navxPackage.Dependencies)
                        {
                            this.Dependencies.AddPropagatedDependency(navxDependency.Id, navxDependency.Name, navxDependency.Publisher, navxDependency.MinVersion);
                        }
                    }
                }
            }
        }

        #endregion

        #region Building symbols

        public void RebuildSymbolReferences()
        {
            foreach (ALProjectFile file in this.Files)
            {
                file.CompileSymbolReferences(true);
            }
        }

        public void UpdateDirtySymbolReferences()
        {
            foreach (ALProjectFile file in this.Files)
            {
                if (file.IsDirty)
                {
                    file.CompileSymbolReferences(true);
                }
            }
        }


        #endregion

        public bool IsALProject()
        {
            return (this.Properties != null);
        }

        #region Path and file names methods

        public bool ContainsPath(string path)
        {
            return PathUtils.ContainsPath(this.RootPath, path);
        }

        protected string GetRelativePath(string fullPath)
        {
            return PathUtils.GetRelativePath(this.RootPath, fullPath);
        }

        public ALProjectFileType GetFileType(string path)
        {
            string ext = Path.GetExtension(path);

            if (ext.Equals(".al", StringComparison.CurrentCultureIgnoreCase))
                return ALProjectFileType.AL;

            if (ext.Equals(".xml", StringComparison.CurrentCultureIgnoreCase))
                return ALProjectFileType.Xml;

            if (ext.Equals(".json", StringComparison.CurrentCultureIgnoreCase))
            {
                string relativePath = this.GetRelativePath(path);
                if (relativePath.Equals("app.json", StringComparison.CurrentCultureIgnoreCase))
                    return ALProjectFileType.AppJson;

                if (relativePath.Equals("AppSourceCop.json", StringComparison.CurrentCultureIgnoreCase))
                    return ALProjectFileType.AppSourceCopJson;
            }

            if (ext.Equals(".app", StringComparison.CurrentCultureIgnoreCase))
                return ALProjectFileType.AppPackage;

            return ALProjectFileType.Undefined;
        }

        #endregion

        #region Change tracking

        public void UpdateConfiguration(ALProjectSource projectSource)
        {
            string newALPackagesPath = String.IsNullOrWhiteSpace(projectSource?.packageCachePath) ? ".alpackages" : projectSource.packageCachePath;
            if (newALPackagesPath != this.PackageCachePath)
            {
                this.PackageCachePath = newALPackagesPath;
                this.ResolveDependencies();
            }
            this.CodeAnalyzers = projectSource.codeAnalyzers;
            this.AdditionalMandatoryAffixesPatterns = projectSource.additionalMandatoryAffixesPatterns;
        }

        public void OnDocumentOpen(string path)
        {
            ALProjectFileType fileType = this.GetFileType(path);
            if (fileType == ALProjectFileType.AL)
            {
                ALProjectFile file = this.Files.FindByRelativePath(this.GetRelativePath(path));
                if (file != null)
                    file.OnOpen();
            }
        }

        public void OnDocumentClose(string path)
        {
            ALProjectFileType fileType = this.GetFileType(path);
            if (fileType == ALProjectFileType.AL)
            {
                ALProjectFile file = this.Files.FindByRelativePath(this.GetRelativePath(path));
                if (file != null)
                    file.OnClose();
            }
        }

        public ALSymbol OnDocumentChange(string path, string content, bool returnSymbols)
        {
            ALProjectFileType fileType = this.GetFileType(path);
            switch (fileType)
            {
                case ALProjectFileType.AL:
                    ALProjectFile file = this.Files.FindByRelativePath(this.GetRelativePath(path));
                    if (file != null)
                        return file.OnChange(content, returnSymbols);
                    break;
            }
            return null;
        }
        
        public void OnDocumentSave(string path)
        {
        }

        public void OnFileCreate(string path)
        {
        }

        public void OnFileDelete(string path)
        {
        }

        public void OnFileRename(string oldPath, string newPath)
        {
        }

        public void OnFileSystemFileChange(string path)
        {
            if (File.Exists(path))
            {
                ALProjectFileType fileType = this.GetFileType(path);
                switch (fileType)
                {
                    case ALProjectFileType.AL:
                        ALProjectFile file = this.Files.FindByRelativePath(this.GetRelativePath(path));
                        if (file != null)
                            file.OnSave();
                        break;
                    case ALProjectFileType.AppJson:
                        this.ReloadProjectManifest();
                        break;
                    case ALProjectFileType.AppSourceCopJson:
                        this.LoadAppSourceCopSettings();
                        break;
                    case ALProjectFileType.AppPackage:
                        if (PathUtils.ContainsPath(this.GetALPackagesPath(), path))
                        {
                            this.Workspace.SymbolReferencesCache.Remove(path);
                            //reload dependency symbols
                            ALProjectDependency dependency = this.Dependencies.FindAppPackageDependency(path);
                            if (dependency != null)
                                this.ResolveDependencies();
                        }
                        break;
                }
            }
        }

        public void OnFileSystemFileCreate(string path)
        {
            if (File.Exists(path))
            {
                ALProjectFileType fileType = this.GetFileType(path);
                switch (fileType)
                {
                    case ALProjectFileType.AL:
                        path = this.GetRelativePath(path);
                        ALProjectFile file = this.Files.FindByRelativePath(path);
                        if (file == null)
                            this.Files.Add(new ALProjectFile(this, path));
                        else
                            file.OnSave();
                        break;
                    case ALProjectFileType.AppJson:
                        this.ReloadProjectManifest();
                        break;
                    case ALProjectFileType.AppSourceCopJson:
                        this.LoadAppSourceCopSettings();
                        break;
                    case ALProjectFileType.AppPackage:
                        if (PathUtils.ContainsPath(this.GetALPackagesPath(), path))
                            this.ResolveDependencies();
                        break;
                }
            } 
            else if (Directory.Exists(path))
            {
                //add al files from path
                string relPath = PathUtils.GetRelativePath(this.RootPath, path);
                this.Files.Load(relPath);

                //resolve dependencies
                if (PathUtils.ContainsPath(this.GetALPackagesPath(), path))
                    this.ResolveDependencies();
            }
        }

        public void OnFileSystemFileDelete(string path)
        {
            ALProjectFileType fileType = this.GetFileType(path);
            switch (fileType)
            {
                case ALProjectFileType.AL:
                    ALProjectFile file = this.Files.FindByRelativePath(this.GetRelativePath(path));
                    if (file != null)
                    {
                        this.Files.Remove(file);
                        return;
                    }
                    break;
                case ALProjectFileType.AppJson:
                    this.ReloadProjectManifest();
                    break;
                case ALProjectFileType.AppSourceCopJson:
                    this.LoadAppSourceCopSettings();
                    break;
            }

            //delete al files from folder
            string relPath = PathUtils.GetRelativePath(this.RootPath, path);
            this.Files.RemoveFromFolder(path);

            //resolve dependencies
            if (PathUtils.ContainsPath(this.GetALPackagesPath(), path))
            {
                this.Workspace.SymbolReferencesCache.RemoveDeletedFiles();
                this.ResolveDependencies();
            }
        }

        protected void FileEvent(string name, string path)
        {
        }

        #endregion

        #region Build symbols library

        protected string GetFullName()
        {
            if (this.Properties != null)
                return StringExtensions.Merge(this.Properties.Publisher, this.Properties.Name, (this.Properties.Version != null)?this.Properties.Version.Version:null);
            return "";
        }

        public ALSymbolsLibrary CreateSymbolsLibrary(bool includeDependencies)
        {
            ALSymbol rootSymbol;
                      
            if ((includeDependencies) && (this.Dependencies.Count > 0))
            {
                rootSymbol = new ALSymbol(ALSymbolKind.ProjectDefinition, this.GetFullName());

                //add dependencies
                ALSymbol dependenciesListSymbol = new ALSymbol(ALSymbolKind.Dependencies, "Dependencies");
                foreach (ALProjectDependency dependency in this.Dependencies)
                {
                    if (dependency.Symbols != null)
                        dependenciesListSymbol.AddChildSymbol(dependency.Symbols.ToALSymbol());
                }
                rootSymbol.AddChildSymbol(dependenciesListSymbol);

                //add project
                rootSymbol.AddChildSymbol(this.Symbols.ToALSymbol());
            } 
            else
            {
                rootSymbol = this.Symbols.ToALSymbol();
            }

            return new ALSymbolsLibrary(new ALProjectSymbolsLibrarySource(this), rootSymbol);
        }

        #endregion

        protected void OnPropertiesChanged()
        {
            if (this.Properties != null)
            {
                this.Symbols.AppId = this.Properties.Id;
                this.Symbols.Name = this.Properties.Name;
                this.Symbols.Publisher = this.Properties.Publisher;
                this.Symbols.Version = (this.Properties.Version != null)?this.Properties.Version.Version:"";
            }
        }

    }
}
