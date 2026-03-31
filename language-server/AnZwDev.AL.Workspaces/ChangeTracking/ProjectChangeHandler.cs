using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.AL.Workspaces.ChangeTracking
{
    internal class ProjectChangeHandler
    {

        private Dictionary<ProjectFileType, ProjectFileChangeHandler> _fileChangeHandlers = new Dictionary<ProjectFileType, ProjectFileChangeHandler>();

        public ProjectChangeHandler()
        {
        }

        protected void RegisterFileChangeHandlers()
        {
            RegisterFileChangeHandler(new AppJsonProjectFileChangeHandler());
            RegisterFileChangeHandler(new AppSourceCopJsonProjectFileChangeHandler());
            RegisterFileChangeHandler(new ALProjectFileChangeHandler());
        }

        protected ProjectFileChangeHandler? GetFileChangeHandler(ProjectFileType fileType)
        {
            if (_fileChangeHandlers.ContainsKey(fileType))
                return _fileChangeHandlers[fileType];
            return null;
        }

        protected void RegisterFileChangeHandler(ProjectFileChangeHandler handler)
        {
            _fileChangeHandlers[handler.FileType] = handler;
        }

        public void FileChanged(ProjectFile file)
        {
            GetFileChangeHandler(file.Type)?.FileChanged(file);
        }

        public void FileAdded(ProjectFile file)
        {
            GetFileChangeHandler(file.Type)?.FileAdded(file);
        }

        public void FileRemoved(ProjectFile file)
        {
            GetFileChangeHandler(file.Type)?.FileRemoved(file);
        }

    }
}
