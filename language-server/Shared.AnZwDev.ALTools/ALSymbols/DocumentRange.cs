using Microsoft.Dynamics.Nav.CodeAnalysis.Text;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.ALSymbols
{
    public class DocumentRange : Range
    {
        public string filePath { get; set; }

        public DocumentRange(string newFilePath, FileLinePositionSpan span) : base(span)
        {
            this.filePath = newFilePath;
        }

        public DocumentRange(string newFilePath, Position newStart, Position newEnd) : base(newStart, newEnd)
        {
            this.filePath = newFilePath;
        }

        public DocumentRange(string newFilePath, int startLine, int startCharacter, int endLine, int endCharacter) : base(startLine, startCharacter, endLine, endCharacter)
        {
            this.filePath = newFilePath;
        }

        public override string GetUniqueKey()
        {
            return $"{start.line}_{start.character}_{end.line}_{end.character}_{filePath}";
        }

    }
}
