using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AnZwDev.ALTools.Workspace.ALCompiler
{
    public class ALFileContent
    {

        private string _filePath = null;
        public string FilePath
        {
            get { return _filePath; }
            set
            {
                _filePath = value;
                FullFilePath = Path.GetFullPath(_filePath);
            }
        }

        public string FullFilePath { get; private set; } = null;
        public string Content { get; set; } = null;
        public SyntaxTree SyntaxTree { get; set; } = null;

        public ALFileContent()
        {
        }

        public ALFileContent(string filePath, string content)
        {
            FilePath = filePath;
            Content = content;
        }

    }
}
