using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.AL.Syntax
{
    public struct TextPosition
    {

        [JsonProperty("line")]
        public int Line { get; set; }

        [JsonProperty("character")]
        public int Character { get; set; }

        public TextPosition()
        {
            Line = 0;
            Character = 0;
        }

        public TextPosition(int line, int character)
        {
            Line = line;
            Character = character;
        }

        public int CompareTo(TextPosition other)
        {
            var result = Line.CompareTo(other.Line);
            if (result != 0)
                return result;
            return Character.CompareTo(other.Character);
        }

    }
}
