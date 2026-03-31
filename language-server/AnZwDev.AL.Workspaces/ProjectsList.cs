using AnZwDev.System.Collections;
using AnZwDev.System.IO;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.AL.Workspaces
{
    public class ProjectsList : ExtendableList<Project>
    {

        public Workspace Workspace { get; }

        public ProjectsList(Workspace workspace)
        {
            Workspace = workspace;
        }

        public Project? FindByPath(string path)
        {
            if (!String.IsNullOrWhiteSpace(path))
                for (int i = 0; i < Count; i++)
                    if (PathUtils.ContainsPath(this[i].RootPath, path))
                        return this[i];
            return null;
        }

        public Project? FindById(string appId)
        {
            for (int i = 0; i < Count; i++)
                if (appId.Equals(this[i].SymbolsProvider.ProjectCodeSymbolsProvider.AppId, StringComparison.OrdinalIgnoreCase))
                    return this[i];
            return null;
        }

        public void Add(ProjectDescriptor projectDescriptor)
        {
            if ((!String.IsNullOrWhiteSpace(projectDescriptor.ProjectPath)) && (this.FindByPath(projectDescriptor.ProjectPath) == null))
                this.Add(new Project(Workspace, projectDescriptor));
        }

        public void Remove(ProjectDescriptor projectDescriptor)
        {
            if (!String.IsNullOrWhiteSpace(projectDescriptor.ProjectPath))
                Remove(projectDescriptor.ProjectPath);
        }

        public void Remove(string projectPath)
        {
            var project = FindByPath(projectPath);
            if (project != null)
                Remove(project);
        }

        protected override void OnItemAdded(Project item)
        {
            item.Load();
            base.OnItemAdded(item);

            if (!IsInUpdate)
                Workspace.ChangeHandler.ProjectsListChanged();
        }

        protected override void OnItemRemoved(Project item)
        {
            base.OnItemRemoved(item);

            if (!IsInUpdate)
                Workspace.ChangeHandler.ProjectsListChanged();
        }

        public void Update(List<ProjectDescriptor>? addProjects, List<ProjectDescriptor>? removeProjects)
        {
            BeginUpdate();

            if (removeProjects != null)
                for (int i = 0; i < removeProjects.Count; i++)
                    this.Remove(removeProjects[i]);

            if (addProjects != null)
                for (int i = 0; i < addProjects.Count; i++)
                    this.Add(addProjects[i]);

            EndUpdate();

            if (!IsInUpdate)
                Workspace.ChangeHandler.ProjectsListChanged();
        }

    }
}
