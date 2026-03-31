using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.AL.Symbols.Parsing.Parsers
{
    internal class ObjectKindParser : HashTableParser<ObjectKind>
    {

        public ObjectKindParser() : base(ObjectKind.Unknown)
        {
            this.Add("tabledata", ObjectKind.TableData);
            this.Add("record", ObjectKind.Table);
            this.Add("table", ObjectKind.Table);
            this.Add("page", ObjectKind.Page);
            this.Add("report", ObjectKind.Report);
            this.Add("xmlport", ObjectKind.XmlPort);
            this.Add("query", ObjectKind.Query);
            this.Add("codeunit", ObjectKind.Codeunit);
            this.Add("controladdin", ObjectKind.ControlAddIn);
            this.Add("pageextension", ObjectKind.PageExtension);
            this.Add("tableextension", ObjectKind.TableExtension);
            this.Add("profile", ObjectKind.Profile);
            this.Add("profileextension", ObjectKind.ProfileExtension);
            this.Add("pagecustomization", ObjectKind.PageCustomization);
            this.Add("dotnet", ObjectKind.DotNetPackage);
            this.Add("dotnetpackage", ObjectKind.DotNetPackage);
            this.Add("enum", ObjectKind.EnumType);
            this.Add("enumextension", ObjectKind.EnumExtensionType);
            this.Add("interface", ObjectKind.Interface);
            this.Add("reportextension", ObjectKind.ReportExtension);
            this.Add("permissionset", ObjectKind.PermissionSet);
            this.Add("permissionsetextension", ObjectKind.PermissionSetExtension);
            this.Add("entitlement", ObjectKind.Entitlement);
        }

    }
}
