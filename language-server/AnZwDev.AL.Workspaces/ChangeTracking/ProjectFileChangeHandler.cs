using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Text;

namespace AnZwDev.AL.Workspaces.ChangeTracking
{
    public abstract class ProjectFileChangeHandler
    {

        public ProjectFileType FileType { get; }

        public ProjectFileChangeHandler(ProjectFileType fileType)
        {
            this.FileType = fileType;
        }

        public virtual void FileChanged(ProjectFile file)
        {
        }

        public virtual void FileAdded(ProjectFile file)
        {
        }

        public virtual void FileRemoved(ProjectFile file)
        {
        }

    }
}
