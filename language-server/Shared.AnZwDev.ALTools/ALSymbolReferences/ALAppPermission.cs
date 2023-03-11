using AnZwDev.ALTools.ALSymbolReferences.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.ALSymbolReferences
{
    public class ALAppPermission : ALAppBaseElement
    {

        public ALAppPermissionObjectType PermissionObject { get; set; }
        public ALAppPermissionValue Value { get; set; }
        public int Id { get; set; }
        public string ObjectName { get; set; }

        public ALAppPermission()
        {
        }

        public void ReplaceIdReferences(ALAppObjectIdMap idMap)
        {
            if (Id == 0)
                ObjectName = "*";
            else
                switch (PermissionObject)
                {
                    case ALAppPermissionObjectType.TableData:
                    case ALAppPermissionObjectType.Table:
                        if (idMap.TableIdMap.ContainsKey(Id))
                            ObjectName = idMap.TableIdMap[Id].Name;
                        break;
                    case ALAppPermissionObjectType.Report:
                        if (idMap.ReportIdMap.ContainsKey(Id))
                            ObjectName = idMap.ReportIdMap[Id].Name;
                        break;
                    case ALAppPermissionObjectType.Codeunit:
                        if (idMap.CodeunitIdMap.ContainsKey(Id))
                            ObjectName = idMap.CodeunitIdMap[Id].Name;
                        break;
                    case ALAppPermissionObjectType.XmlPort:
                        if (idMap.XmlPortIdMap.ContainsKey(Id))
                            ObjectName = idMap.XmlPortIdMap[Id].Name;
                        break;
                    case ALAppPermissionObjectType.Page:
                        if (idMap.PageIdMap.ContainsKey(Id))
                            ObjectName = idMap.PageIdMap[Id].Name;
                        break;
                    case ALAppPermissionObjectType.Query:
                        if (idMap.QueryIdMap.ContainsKey(Id))
                            ObjectName = idMap.QueryIdMap[Id].Name;
                        break;
                }
        }

        public string GetObjectNameKey()
        {
            if (ObjectName != null)
                return ObjectName.ToLower();
            return "";
        }

    }
}
