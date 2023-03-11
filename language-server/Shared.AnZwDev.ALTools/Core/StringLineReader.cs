using AnZwDev.ALTools.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.Core
{
    public class StringLineReader
    {

        private string _content;
        private int _position;

        public StringLineReader(string content)
        {
            _content = content;
            _position = 0;
        }

        public bool TryReadLineAndTrimEnd(out string line, out string newLineSeparator)
        {
            int startPos = _position;
            int endPos = -1;

            line = null;
            newLineSeparator = null;

            while (_position < _content.Length)
            {
                char character = _content[_position];
                _position++;

                if (character.IsNewLine())
                {
                    int endLineStart = _position - 1;
                    if (_position < _content.Length)
                    {
                        char nextCharacter = _content[_position];
                        if ((nextCharacter.IsNewLine()) && (nextCharacter != character))
                            _position++;
                    }

                    newLineSeparator = _content.Substring(endLineStart, _position - endLineStart);
                    if (endPos > startPos)
                        line = _content.Substring(startPos, endPos - startPos);

                    return true;
                }

                if (!Char.IsWhiteSpace(character))
                    endPos = _position;
            }

            if (endPos > startPos)
            {
                line = _content.Substring(startPos, endPos - startPos);
                return true;
            }

            return false;
        }

        public bool EndOfString()
        {
            return (_position >= _content.Length);
        }

    }
}
