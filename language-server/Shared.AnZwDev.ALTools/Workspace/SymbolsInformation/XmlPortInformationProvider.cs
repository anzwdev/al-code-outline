using AnZwDev.ALTools.ALSymbolReferences;
using AnZwDev.ALTools.ALSymbolReferences.Search;
using AnZwDev.ALTools.ALSymbols;
using AnZwDev.ALTools.Workspace.SymbolReferences;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnZwDev.ALTools.Workspace.SymbolsInformation
{
    public class XmlPortInformationProvider : BaseObjectInformationProvider<ALAppXmlPort>
    {

        public XmlPortInformationProvider() : base(x => x.XmlPorts)
        {
        }

        #region Get list of xml ports

        public List<XmlPortInformation> GetXmlPorts(ALProject project)
        {
            var infoList = new List<XmlPortInformation>();
            var objectsEnumerable = GetALAppObjectsCollection(project);
            foreach (var obj in objectsEnumerable)
                infoList.Add(new XmlPortInformation(obj));
            return infoList;

        }

        #endregion

        #region Get xml port table element details

        public XmlPortTableElementInformation GetXmlPortTableElementDetails(ALProject project, ALObjectReference xmlPortReference, string xmlPortTableElementName, bool getExistingFields, bool getAvailableFields)
        {
            ALAppXmlPort xmlPort = FindXmlPort(project, xmlPortReference);
            if ((xmlPort == null) || (xmlPort.Schema == null))
                return null;
            ALAppXmlPortNode xmlPortTableElement = xmlPort.FindNode(xmlPortTableElementName, ALAppXmlPortNodeKind.TableElement);
            if (xmlPortTableElement == null)
                return null;

            XmlPortTableElementInformation tableElementInformation = new XmlPortTableElementInformation(xmlPortTableElement);
            var tableReference = new ALObjectReference(xmlPort.Usings, tableElementInformation.Source);
            if ((!tableReference.IsEmpty()) && (getExistingFields || getAvailableFields))
            {
                TableInformationProvider tableInformationProvider = new TableInformationProvider();
                List<TableFieldInformaton> allTableFieldsList = tableInformationProvider.GetTableFields(project, tableReference, false, false, true, true, false, false, null);

                Dictionary<string, TableFieldInformaton> availableTableFieldsDict = allTableFieldsList.ToDictionary();
                List<TableFieldInformaton> xmlPortTableFields = new List<TableFieldInformaton>();

                if (xmlPortTableElement.Schema != null)
                    this.CollectXmlPortTableFields(tableElementInformation.Name, xmlPortTableElement.Schema, availableTableFieldsDict, xmlPortTableFields);


                if (getExistingFields)
                    tableElementInformation.ExistingTableFields = xmlPortTableFields;
                if (getAvailableFields)
                    tableElementInformation.AvailableTableFields = availableTableFieldsDict.Values.ToList();
            }

            return tableElementInformation;
        }

        protected void CollectXmlPortTableFields(string xmlPortTableElementName, ALAppSymbolsCollection<ALAppXmlPortNode> nodesList, Dictionary<string, TableFieldInformaton> availableTableFieldsDict, List<TableFieldInformaton> xmlPortTableFields)
        {
            for (int i = 0; i < nodesList.Count; i++)
            {
                ALAppXmlPortNode xmlPortNode = nodesList[i];

                switch (xmlPortNode.Kind)
                {
                    case ALAppXmlPortNodeKind.FieldElement:
                    case ALAppXmlPortNodeKind.FieldAttribute:
                        if (!String.IsNullOrWhiteSpace(xmlPortNode.Expression))
                        {
                            ALMemberAccessExpression memberAccessExpression = ALSyntaxHelper.DecodeMemberAccessExpression(xmlPortNode.Expression);
                            if ((!String.IsNullOrWhiteSpace(memberAccessExpression.Expression)) && (xmlPortTableElementName.Equals(memberAccessExpression.Name, StringComparison.CurrentCultureIgnoreCase)))
                            {
                                string sourceExpression = memberAccessExpression.Expression.ToLower();
                                if (availableTableFieldsDict.ContainsKey(sourceExpression))
                                {
                                    xmlPortTableFields.Add(availableTableFieldsDict[sourceExpression]);
                                    availableTableFieldsDict.Remove(sourceExpression);
                                }
                            }
                        }
                        break;
                }

                if (xmlPortNode.Schema != null)
                    this.CollectXmlPortTableFields(xmlPortTableElementName, xmlPortNode.Schema, availableTableFieldsDict, xmlPortTableFields);
            }
        }

        #endregion

        #region XmlPort variables

        public List<ALAppVariable> GetXmlPortVariables(ALProject project, ALObjectReference objectReference)
        {
            ALAppXmlPort xmlPort = this.FindXmlPort(project, objectReference);
            if ((xmlPort != null) && (xmlPort.Variables != null) && (xmlPort.Variables.Count > 0))
                return xmlPort.Variables;
            return null;
        }

        #endregion

        private ALAppXmlPort FindXmlPort(ALProject project, ALObjectReference objectReference)
        {
            return project
                .GetAllSymbolReferences()
                .GetAllObjects<ALAppXmlPort>(x => x.XmlPorts)
                .FindFirst(objectReference);
        }

    }
}
