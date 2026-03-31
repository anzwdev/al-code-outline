using AnZwDev.AL.Syntax.Formatters;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.AL.Syntax
{
    public class ALLiteralFormatter
    {

        private static ALNameFormatter _nameFormatter = new ALNameFormatter();
        public static string GetName(string value, bool force = false)
        {
            return _nameFormatter.Get(value, force);
        }
        public static void WriteName(TextWriter writer, string value, bool force = false)
        {
            _nameFormatter.Write(writer, value, force);
        }

        public static string GetNameList(List<string> values, string separator = ",", bool force = false)
        {
            return _nameFormatter.GetList(values, separator, force);
        }
        public static void WriteNameList(TextWriter writer, List<string> values, string separator = ",", bool force = false)
        {
            _nameFormatter.WriteList(writer, values, separator, force);
        }

        private static ALStringFormatter _stringFormatter = new ALStringFormatter();
        public static string GetString(string value)
        {
            return _stringFormatter.Get(value);
        }
        public static void WriteString(TextWriter writer, string value)
        {
            _stringFormatter.Write(writer, value);
        }

        private static ALBoolFormatter _boolFormatter = new ALBoolFormatter();
        public static string GetBoolean(bool value)
        {
            return _boolFormatter.Get(value);
        }
        public static void WriteBoolean(TextWriter writer, bool value)
        {
            _boolFormatter.Write(writer, value);
        }

        private static ALIntFormatter _intFormatter = new ALIntFormatter();
        public static string GetInt(int value)
        {
            return _intFormatter.Get(value);
        }
        public static void WriteInt(TextWriter writer, int value)
        {
            _intFormatter.Write(writer, value);
        }

        private static ALKeywordFormatter _keywordFormatter = new ALKeywordFormatter();
        public static string GetKeyword(string value)
        {
            return _keywordFormatter.Get(value);
        }
        public static void WriteKeyword(TextWriter writer, string value)
        {
            _keywordFormatter.Write(writer, value);
        }

    }
}
