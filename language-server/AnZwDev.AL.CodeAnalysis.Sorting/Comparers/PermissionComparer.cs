using AnZwDev.AL.Syntax;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.AL.CodeAnalysis.Sorting.Comparers
{
    public class PermissionComparer : IComparer<PermissionSyntax>
    {

        protected static int TYPE_TABLE = 0;
        protected static int TYPE_TABLEDATA = 1;
        protected static int TYPE_CODEUNIT = 2;
        protected static int TYPE_PAGE = 3;
        protected static int TYPE_QUERY = 4;
        protected static int TYPE_REPORT = 5;
        protected static int TYPE_XMLPORT = 6;

        private Dictionary<string, int> _typePriorities;
        protected static IComparer<string> _stringComparer = new SyntaxNodeNameComparer();

        public PermissionComparer()
        {
            _typePriorities = new Dictionary<string, int>
            {
                { "table", TYPE_TABLE },
                { "tabledata", TYPE_TABLEDATA },
                { "codeunit", TYPE_CODEUNIT },
                { "page", TYPE_PAGE },
                { "query", TYPE_QUERY },
                { "report", TYPE_REPORT },
                { "xmlport", TYPE_XMLPORT }
            };
        }

        public int Compare(PermissionSyntax? x, PermissionSyntax? y)
        {
            int xType = this.GetTypePriority(ALLiteralParser.ParseName(x?.ObjectType.ToString()));
            string xName = ALLiteralParser.ParseName(x?.ObjectReference.Identifier.ToString());

            int yType = this.GetTypePriority(ALLiteralParser.ParseName(y?.ObjectType.ToString()));
            string yName = ALLiteralParser.ParseName(y?.ObjectReference.Identifier.ToString());

            bool tableTypes = IsTableType(xType) && IsTableType(yType);

            if (!tableTypes && xType != yType)
                return xType - yType;

            int stringResult = _stringComparer.Compare(xName, yName);

            if (!tableTypes || stringResult != 0)
                return stringResult;

            return xType - yType;
        }

        protected bool IsTableType(int typePriority)
        {
            return typePriority == TYPE_TABLE || typePriority == TYPE_TABLEDATA;
        }

        protected int GetTypePriority(string type)
        {
            if (type != null)
            {
                type = type.ToLower();
                if (_typePriorities.ContainsKey(type))
                    return _typePriorities[type];
            }
            return 0;
        }
    }

}
