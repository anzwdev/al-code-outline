using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.AL.Workspaces
{
    
    public class ProjectFileAttachedDataStore
    {

        public ProjectFile ProjectFile { get; }

        private Dictionary<string, ProjectFileAttachedData> _data = new();

        public ProjectFileAttachedDataStore(ProjectFile projectFile)
        {
            ProjectFile = projectFile;
        }

        public T Get<T>(ProjectFileAttachedDataFactory<T> factory) where T : ProjectFileAttachedData
        {
            if (_data.ContainsKey(factory.Key))
                return (T)_data[factory.Key];

            var content = factory.Create(ProjectFile);

            _data.Add(factory.Key, content);
            return content;
        }

        public void FileOpened()
        {
            foreach (var cachedContent in _data.Values)
                cachedContent.FileOpened();
        }

        public void FileClosed()
        {
            foreach (var cachedContent in _data.Values)
                cachedContent.FileClosed();
        }

        public void FileChanged()
        {
            foreach (var cachedContent in _data.Values)
                cachedContent.FileChanged();
        }

    }

}
