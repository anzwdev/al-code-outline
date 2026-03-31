using AnZwDev.System.IO;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.AL.Syntax
{
    public class FileSystemSourceCodeProvider : ISourceCodeProvider
    {

        public string ProjectPath { get; }
        public IFile AppJsonFile { get; }
        public IEnumerable<IFile> SourceFiles { get; }

        public FileSystemSourceCodeProvider(string projectPath)
        {
            ProjectPath = projectPath;
            AppJsonFile = new FileSystemFile(Path.Combine(projectPath, "app.json"));
            SourceFiles = FileSystemDirectory.GetFiles(projectPath, "*.al", SearchOption.AllDirectories);
        }

    }
}
