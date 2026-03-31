using AnZwDev.System.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http.Headers;
using System.Reflection.Metadata;
using System.Text;

namespace AnZwDev.AL.Workspaces
{
    public class ProjectFile : IFile
    {

        public Project Project { get; }
        public string FullPath { get; private set; }
        public Encoding Encoding { get; private set; } = Encoding.UTF8;
        public bool IsOpenInEditor { get; private set; } = false;
        public ProjectFileType Type { get; }
        public ProjectFileAttachedDataStore AttachedData { get; }

        private string? _memoryContent = null;

        public ProjectFile(Project project, string fullPath)
        {
            Project = project;
            FullPath = PathUtils.NormalizePath(fullPath);
            Type = ProjectFileTypeUtils.GetFileType(project.RootPath, fullPath);
            AttachedData = new ProjectFileAttachedDataStore(this);
        }

        public void OpenEditor()
        {
            IsOpenInEditor = true;
            AttachedData.FileOpened();
        }

        public void CloseEditor()
        {
            IsOpenInEditor = false;
            _memoryContent = null;

            AttachedData.FileClosed();
            OnFileChanged();
        }

        public void FileContentChanged()
        {
            if (!IsOpenInEditor)
                OnFileChanged();
        }

        public void ChangeEditorContent(string content)
        {
            if (IsOpenInEditor)
            {
                _memoryContent = content;
                OnFileChanged();
            }
        }

        public string ReadAllText()
        {
            if (IsOpenInEditor && (_memoryContent != null))
                return _memoryContent;
            return ReadAllTextFromDisk();
        }

        public string ReadAllTextFromDisk()
        {
            return FileHelper.ReadAllTextWithRetry(FullPath);
        }

        public void WriteAllText(string content)
        {
            File.WriteAllText(FullPath, content, Encoding);
        }

        protected void OnFileChanged()
        {
            Project.ChangeHandler.FileChanged(this);
            AttachedData.FileChanged();
        }

    }
}
