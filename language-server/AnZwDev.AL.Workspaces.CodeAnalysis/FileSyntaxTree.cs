using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using Microsoft.Dynamics.Nav.CodeAnalysis.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.AL.Workspaces.CodeAnalysis
{
    public class FileSyntaxTree
    {

        public string Path { get; }
        public string Content { get; }
        public SyntaxTree SyntaxTree { get; }
        public SourceText SourceText { get; }

        public FileSyntaxTree(string path, string content, ParseOptions parseOptions, Encoding? encoding = null)
        {
            if (encoding == null)
                encoding = Encoding.UTF8;

            Path = path;
            Content = content;
            SourceText = SourceText.From(content, encoding);
            SyntaxTree = SyntaxTree.ParseObjectText(SourceText, Path, parseOptions, encoding);
        }

    }
}
