using AnZwDev.ALTools.ALSymbols;
using AnZwDev.ALTools.Core;
using AnZwDev.ALTools.Extensions;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.CodeTransformations
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
            this._typePriorities = new Dictionary<string, int>();
            this._typePriorities.Add("table", TYPE_TABLE);
            this._typePriorities.Add("tabledata", TYPE_TABLEDATA);
            this._typePriorities.Add("codeunit", TYPE_CODEUNIT);
            this._typePriorities.Add("page", TYPE_PAGE);
            this._typePriorities.Add("query", TYPE_QUERY);
            this._typePriorities.Add("report", TYPE_REPORT);
            this._typePriorities.Add("xmlport", TYPE_XMLPORT);
        }

        public int Compare(PermissionSyntax x, PermissionSyntax y)
        {
            int xType = this.GetTypePriority(ALSyntaxHelper.DecodeName(x.ObjectType.ToString()));
            string xName = ALSyntaxHelper.DecodeName(x.ObjectReference.Identifier.ToString());

            int yType = this.GetTypePriority(ALSyntaxHelper.DecodeName(y.ObjectType.ToString()));
            string yName = ALSyntaxHelper.DecodeName(y.ObjectReference.Identifier.ToString());

            bool tableTypes = this.IsTableType(xType) && this.IsTableType(yType);

            if ((!tableTypes) && (xType != yType))
                return xType - yType;

            int stringResult = _stringComparer.Compare(xName, yName);

            if ((!tableTypes) || (stringResult != 0))
                return stringResult;

            return xType - yType;
        }

        protected bool IsTableType(int typePriority)
        {
            return ((typePriority == TYPE_TABLE) || (typePriority == TYPE_TABLEDATA));
        }


        protected int GetTypePriority(string type)
        {
            if (type != null)
            {
                type = type.ToLower();
                if (this._typePriorities.ContainsKey(type))
                    return this._typePriorities[type];
            }
            return 0;
        }
    }

}
