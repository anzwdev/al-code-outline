using Microsoft.Dynamics.Nav.CodeAnalysis.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.ALTools.ALSymbols
{
    public class TextRange
    {

        public Position start { get; set; }
        public Position end { get; set; }
        public bool isEmpty { get; private set; }
        public bool isSingleLine { get; private set; }

        public TextRange()
        {
            this.start = null;
            this.end = null;
            UpdateStatus();
        }

        public TextRange(FileLinePositionSpan span): this(span.StartLinePosition.Line, span.StartLinePosition.Character, span.EndLinePosition.Line, span.EndLinePosition.Character)
        {
            UpdateStatus();
        }

        public TextRange(Position newStart, Position newEnd)
        {
            this.start = newStart;
            this.end = newEnd;
            UpdateStatus();
        }

        public TextRange(int startLine, int startCharacter, int endLine, int endCharacter)
        {
            this.start = new Position(startLine, startCharacter);
            this.end = new Position(endLine, endCharacter);
        }

        protected void UpdateStatus()
        {
            this.isEmpty = ((this.start == null) || (this.end == null));
            this.isSingleLine = (this.isEmpty) || (this.start.line == this.end.line);
        }

        public virtual string GetUniqueKey()
        {
            return $"{start.line}_{start.character}_{end.line}_{end.character}";
        }

        public void Add(TextRange range)
        {
            if (range.start != null)
            {
                if (start == null)
                    start = new Position(range.start.line, range.start.character);
                else if (start.IsGreater(range.start))
                    start.Set(range.start);
            }
            
            if (range.end != null)
            {
                if (end == null)
                    end = new Position(range.end.line, range.end.character);
                else if (end.IsLower(range.end))
                    end.Set(range.end);

            }

            UpdateStatus();
        }

    }
}
