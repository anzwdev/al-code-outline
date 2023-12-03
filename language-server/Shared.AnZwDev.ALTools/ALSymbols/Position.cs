using Microsoft.Dynamics.Nav.CodeAnalysis.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.ALTools.ALSymbols
{
    public class Position
    {

        public int line { get; set; }
        public int character { get; set; }

        public Position() : this(0, 0)
        {
        }

        public Position(int newLine, int newCharacter)
        {
            this.line = newLine;
            this.character = newCharacter;
        }

        public int Compare(LinePosition pos)
        {
            if (this.line == pos.Line)
                return this.character - pos.Character;
            return this.line - pos.Line;
        }

        public bool IsGreater(LinePosition pos)
        {
            return (this.Compare(pos) > 0);
        }

        public bool IsLower(LinePosition pos)
        {
            return (this.Compare(pos) < 0);
        }

        public bool IsGreaterOrEqual(LinePosition pos)
        {
            return (this.Compare(pos) >= 0);
        }

        public bool IsLowerOrEqual(LinePosition pos)
        {
            return (this.Compare(pos) <= 0);
        }

        public bool IsEqual(LinePosition pos)
        {
            return (this.Compare(pos) == 0);
        }

        public int Compare(Position pos)
        {
            if (this.line == pos.line)
                return this.character - pos.character;
            return this.line - pos.line;
        }

        public bool IsGreater(Position pos)
        {
            return (this.Compare(pos) > 0);
        }

        public bool IsLower(Position pos)
        {
            return (this.Compare(pos) < 0);
        }

        public bool IsGreaterOrEqual(Position pos)
        {
            return (this.Compare(pos) >= 0);
        }

        public bool IsLowerOrEqual(Position pos)
        {
            return (this.Compare(pos) <= 0);
        }

        public bool IsEqual(Position pos)
        {
            return (this.Compare(pos) == 0);
        }

        public void Set(Position source)
        {
            this.line = source.line;
            this.character = source.character;
        }

    }
}
