using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;
using AnZwDev.ALTools.Core;

namespace AnZwDev.ALTools.Workspace
{
    public class ALProjectFilesCollection : IList<ALProjectFile>
    {

        #region Private members

        private List<ALProjectFile> _filesList { get; set; }
        private Dictionary<string, ALProjectFile> _filesDictionary { get; set; }

        #endregion

        #region Public properties
        
        public ALProject Project { get; }

        public int Count
        {
            get { return _filesList.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public ALProjectFile this[int index]
        {
            get { return _filesList[index]; }
            set 
            {
                if (_filesList[index] != value)
                {
                    if (_filesList[index] != null)
                        this.OnRemove(_filesList[index]);
                    _filesList[index] = value;
                    if (_filesList[index] != null)
                        this.OnAdd(_filesList[index]);
                }
            }
        }

        #endregion

        #region Initialization

        public ALProjectFilesCollection(ALProject project)
        {
            this.Project = project;
            _filesList = new List<ALProjectFile>();
            _filesDictionary = new Dictionary<string, ALProjectFile>();
        }

        #endregion

        #region IList implementation

        public int IndexOf(ALProjectFile item)
        {
            return _filesList.IndexOf(item);
        }

        public void Insert(int index, ALProjectFile item)
        {
            _filesDictionary.Add(item.FullPath, item);
            _filesList.Insert(index, item);
            this.OnAdd(item);
        }

        public void RemoveAt(int index)
        {
            ALProjectFile item = this[index];
            _filesDictionary.Remove(item.FullPath);
            _filesList.RemoveAt(index);
            OnRemove(item);
        }

        public void Add(ALProjectFile item)
        {
            _filesDictionary.Add(item.FullPath, item);
            _filesList.Add(item);
            this.OnAdd(item);
        }

        public void Clear()
        {
            _filesList.Clear();
            _filesDictionary.Clear();
        }

        public bool Contains(ALProjectFile item)
        {
            return _filesList.Contains(item);
        }

        public void CopyTo(ALProjectFile[] array, int arrayIndex)
        {
            _filesList.CopyTo(array, arrayIndex);
        }

        public bool Remove(ALProjectFile item)
        {
            _filesDictionary.Remove(item.FullPath);
            bool result = _filesList.Remove(item);
            this.OnRemove(item);
            return result;
        }

        public IEnumerator<ALProjectFile> GetEnumerator()
        {
            return _filesList.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _filesList.GetEnumerator();
        }

        #endregion

        #region Loading list of files

        public void Load()
        {
            this.Load("");
        }

        public void Load(string relPath)
        {
            string fullPath = (String.IsNullOrWhiteSpace(relPath)) ? this.Project.RootPath : Path.Combine(this.Project.RootPath, relPath);

            //process files
            string[] filePathsCollection = Directory.GetFiles(fullPath, "*.al");
            foreach (string filePath in filePathsCollection)
            {
                string fileName = Path.GetFileName(filePath);
                this.Add(new ALProjectFile(this.Project, Path.Combine(relPath, fileName)));
            }

            //process sub directiories
            string[] dirPathsCollection = Directory.GetDirectories(fullPath, "*.*");
            foreach (string dirPath in dirPathsCollection)
            {
                this.Load(Path.Combine(relPath, Path.GetFileName(dirPath)));
            }
        }

        #endregion

        #region Removing list of files

        public void RemoveFromFolder(string relPath)
        {
            List<ALProjectFile> _toRemove = null;
            foreach (ALProjectFile file in this)
            {
                if (PathUtils.ContainsPath(relPath, file.RelativePath))
                {
                    if (_toRemove == null)
                        _toRemove = new List<ALProjectFile>();
                    _toRemove.Add(file);
                }
            }
            if (_toRemove != null)
            {
                foreach (ALProjectFile file in _toRemove)
                {
                    this.Remove(file);
                }
            }
        }

        #endregion

        protected void OnAdd(ALProjectFile file)
        {
            file.OnAdd();
        }

        protected void OnRemove(ALProjectFile file)
        {
            file.OnDelete();
        }

        public ALProjectFile FindByRelativePath(string path)
        {
            return this.Where(p => (p.RelativePath == path)).FirstOrDefault();
        }

        public ALProjectFile FindByFullPath(string path)
        {
            return this.Where(p => (p.FullPath == path)).FirstOrDefault();
        }


    }
}
