using AnZwDev.ALTools.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.ALLanguageInformation
{
    public static class KeywordInformation
    {

        private static readonly HashSet<string> _alCodeKeywords = new HashSet<string>(new string[] {
            "false", "true", "div", "mod", "and", "or", "xor", "not", "exit", "begin", "case",
            "do", "downto", "else", "end", "for", "foreach", "if", "in", "of", "repeat", "then",
            "to", "until", "with", "while", "asserterror", "var", "trigger", "procedure", "local",
            "internal", "protected", "break", "event"});

        private static readonly HashSet<string> _propertyKeywords = new HashSet<string>(new string[] {
            "false", "true", "if", "else", "where", "field", "const", "filter", "upperlimit", "exist",
            "lookup", "min", "max", "average", "sum", "count", "tabledata", "system", "sorting",
            "order", "ascending", "descending", "elif", "endif", "region", "endregion", "define",
            "undef", "pragma", "warning", "disable", "restore", "enable", "implicitwith"});

        private static readonly HashSet<string> _objectCodeKeywords = new HashSet<string>(new string[] {
            "event", "temporary", "trigger", "program", "procedure", "function", "var", "array",
            "of", "local", "internal", "protected", "suppressdispose", "securityfiltering", "fields",
            "field", "keys", "key", "fieldgroups", "fieldgroup", "extends", "add", "addfirst", "addlast",
            "addbefore", "addafter", "movefirst", "movelast", "movebefore", "moveafter", "modify",
            "layout", "area", "group", "repeater", "cuegroup", "fixed", "grid", "part", "systempart",
            "chartpart", "usercontrol", "actions", "action", "separator", "views", "view", "dataset",
            "dataitem", "column", "labels", "label", "requestpage", "elements", "filter", "schema",
            "tableelement", "fieldelement", "textelement", "fieldattribute", "textattribute",
            "customizes", "begin", "end", "false", "true", "assembly", "type", "enum", "value",
            "enumextension"});

        private static readonly HashSet<string> _directiveKeywords = new HashSet<string>(new string[] {
            "false", "true", "if", "else", "elif", "endif", "region", "endregion", "define", "undef",
            "pragma", "warning", "disable", "restore", "enable", "and", "or", "not", "implicitwith"});

        private static readonly HashSet<string> _objectTypeKeywords = new HashSet<string>(new string[] {
            "codeunit", "page", "query", "xmlport", "report", "reportextension", "table", "tableextension",
            "pageextension", "profile", "interface", "implements", "profileextension", "pagecustomization",
            "controladdin", "dotnet", "enum", "enumextension", "permissionset", "permissionsetextension",
            "entitlement"});

        private static readonly HashSet<string> _dataTypeNames = new HashSet<string>(new string[]
        {
            "action", "array", "automation", "biginteger", "bigtext", "blob", "boolean", "byte",
            "char", "clienttype", "code", "codeunit", "completiontriggererrorlevel", "connectiontype",
            "database", "dataclassification", "datascope", "date", "dateformula", "datetime", "decimal",
            "defaultlayout", "dialog", "dictionary", "dotnet", "dotnetassembly", "dotnettypedeclaration",
            "duration", "enum", "errorinfo", "errortype", "executioncontext", "executionmode",
            "fieldclass", "fieldref", "fieldtype", "file", "filterpagebuilder", "guid", "instream",
            "integer", "joker", "keyref", "list", "moduledependencyinfo", "moduleinfo",
            "notification", "notificationscope", "objecttype", "option", "outstream", "page",
            "pageresult", "query", "record", "recordid", "recordref", "report", "reportformat",
            "securityfilter", "securityfiltering", "table", "tableconnectiontype", "tablefilter",
            "testaction", "testfield", "testfilterfield", "testpage", "testpermissions", "testrequestpage",
            "text", "textbuilder", "textconst", "textencoding", "time", "transactionmodel",
            "transactiontype", "variant", "verbosity", "version", "xmlport", "httpcontent", "httpheaders",
            "httpclient", "httprequestmessage", "httpresponsemessage", "jsontoken", "jsonvalue",
            "jsonarray", "jsonobject", "view", "views", "xmlattribute", "xmlattributecollection",
            "xmlcomment", "xmlcdata", "xmldeclaration", "xmldocument", "xmldocumenttype", "xmlelement",
            "xmlnamespacemanager", "xmlnametable", "xmlnode", "xmlnodelist", "xmlprocessinginstruction",
            "xmlreadoptions", "xmltext", "xmlwriteoptions", "webserviceactioncontext",
            "webserviceactionresultcode", "sessionsettings"
        });

        public static bool IsALCodeKeyword(string keyword)
        {
            return ((keyword != null) && (_alCodeKeywords.Contains(keyword.ToLower())));
        }

        public static bool IsPropertyKeyword(string keyword)
        {
            return ((keyword != null) && (_propertyKeywords.Contains(keyword.ToLower())));
        }

        public static bool IsObjectCodeKeyword(string keyword)
        {
            if (keyword == null)
                return false;
            keyword = keyword.ToLower();
            return ((_objectCodeKeywords.Contains(keyword)) || (_objectTypeKeywords.Contains(keyword)));
        }
        public static bool IsDirectiveKeyword(string keyword)
        {
            return ((keyword != null) && (_directiveKeywords.Contains(keyword.ToLower())));
        }
        public static bool IsObjectTypeKeyword(string keyword)
        {
            return ((keyword != null) && (_objectTypeKeywords.Contains(keyword.ToLower())));
        }

        public static bool IsAnyKeyword(string keyword)
        {
            if (keyword == null)
                return false;
            keyword = keyword.ToLower();
            return
                (_alCodeKeywords.Contains(keyword)) ||
                (_propertyKeywords.Contains(keyword)) ||
                (_objectCodeKeywords.Contains(keyword)) ||
                (_directiveKeywords.Contains(keyword)) ||
                (_objectTypeKeywords.Contains(keyword));
        }

        public static bool IsAnyKeywordOrDataTypeName(string keyword)
        {
            if (keyword == null)
                return false;
            keyword = keyword.ToLower();
            return
                (_alCodeKeywords.Contains(keyword)) ||
                (_propertyKeywords.Contains(keyword)) ||
                (_objectCodeKeywords.Contains(keyword)) ||
                (_directiveKeywords.Contains(keyword)) ||
                (_objectTypeKeywords.Contains(keyword)) ||
                (_dataTypeNames.Contains(keyword));
        }

        public static bool IsDataTypeName(string name)
        {
            return ((name != null) && (_dataTypeNames.Contains(name.ToLower())));
        }

    }
}
