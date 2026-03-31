using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.AL.Syntax
{
    public class TextRange
    {

        [JsonProperty("start")]
        public TextPosition Start { get; set; }

        [JsonProperty("end")]
        public TextPosition End { get; set; }

        [JsonProperty("isEmpty")]
        public bool IsEmpty { get { return Start.CompareTo(End) == 0; } }

        [JsonProperty("isSingleLine")]
        public bool IsSingleLine { get { return Start.Line == End.Line; } }

        public TextRange() : this(new TextPosition(0, 0), new TextPosition(0, 0))
        {
        }

        public TextRange(TextPosition start, TextPosition end)
        {
            Start = start;
            End = end;
        }

        public TextRange(int startLine, int startCharacter, int endLine, int endCharacter) : 
            this(new TextPosition(startLine, startCharacter), new TextPosition(endLine, endCharacter))
        {
        }

        public int CompareTo(TextRange other)
        {
            var result = Start.CompareTo(other.Start);
            if (result != 0)
                return result;
            return End.CompareTo(other.End);
        }

        public void Expand(TextRange range)
        {
            if (Start.CompareTo(range.Start) > 0)
                Start = range.Start;

            if (End.CompareTo(range.End) < 0)
                End = range.End;
        }

        public TextRange Clone()
        {
            return new TextRange(
                new TextPosition(Start.Line, Start.Character), 
                new TextPosition(End.Line, End.Character));
        }

    }
}
