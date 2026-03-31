using AnZwDev.System.Collections;
using AnZwDev.System.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.AL.Workspaces
{
    public class ProjectFilesList : ExtendableList<ProjectFile>
    {

        public Project Project { get; }
        public ProjectFile? AppJson { get; private set; } = null;
        public ProjectFile? AppSourceCopJson { get; private set; } = null;

        public ProjectFilesList(Project project)
        {
            this.Project = project;
        }

        public ProjectFile? Find(string path)
        {
            for (int i = 0; i < this.Count; i++)
                if (PathUtils.Equals(this[i].FullPath, path))
                    return this[i];
            return null;
        }

        public void Add(string path)
        {
            Add(new ProjectFile(this.Project, path));
        }

        public void Remove(string path)
        {
            var file = this.Find(path);
            if (file != null)
                this.Remove(file);
        }

        public void AddRange(string[] paths, bool clear)
        {
            BeginUpdate();

            if (clear)
                Clear();

            for (int i = 0; i < paths.Length; i++)
                this.Add(paths[i]);

            EndUpdate();
        }

        protected override void OnItemAdded(ProjectFile item)
        {
            base.OnItemAdded(item);

            switch (item.Type)
            {
                case ProjectFileType.AppJson:
                    this.AppJson = item;
                    break;
                case ProjectFileType.AppSourceCopJson:
                    this.AppSourceCopJson = item;
                    break;
            }

            if (!IsInUpdate)
                this.Project.ChangeHandler.FileAdded(item);
        }

        protected override void OnItemRemoved(ProjectFile item)
        {
            base.OnItemRemoved(item);

            switch (item.Type)
            {
                case ProjectFileType.AppJson:
                    this.AppJson = null;
                    break;
                case ProjectFileType.AppSourceCopJson:
                    this.AppSourceCopJson = null;
                    break;
            }

            if (!IsInUpdate)
                this.Project.ChangeHandler.FileRemoved(item);
        }

        public IEnumerable<ProjectFile> FilterByType(ProjectFileType fileType)
        {
            for (int i = 0; i < this.Count; i++)
                if (this[i].Type == fileType)
                    yield return this[i];
        }

    }
}
