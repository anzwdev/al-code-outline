using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace AnZwDev.AL.Workspaces
{
    public abstract class ProjectFileAttachedData
    {

        public ProjectFile ProjectFile { get; }
        public ProjectFileAttachedDataStorageMode StorageMode { get; }

        public ProjectFileAttachedData(ProjectFile projectFile, ProjectFileAttachedDataStorageMode storageMode)
        {
            ProjectFile = projectFile;
            StorageMode = storageMode;
        }

        protected virtual void Clear()
        {
        }

        internal virtual void FileChanged()
        {
            Clear();
        }

        internal virtual void FileOpened()
        {
        }

        internal virtual void FileClosed()
        {
            if (StorageMode == ProjectFileAttachedDataStorageMode.Open)
                Clear();
        }

    }

}
