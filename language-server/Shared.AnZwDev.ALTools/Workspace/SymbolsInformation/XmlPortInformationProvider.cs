using AnZwDev.ALTools.ALSymbolReferences;
using AnZwDev.ALTools.ALSymbols;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnZwDev.ALTools.Workspace.SymbolsInformation
{
    public class XmlPortInformationProvider
    {

        public XmlPortInformationProvider()
        {
        }

        #region Get list of xml ports

        public List<XmlPortInformation> GetXmlPorts(ALProject project)
        {
            List<XmlPortInformation> infoList = new List<XmlPortInformation>();
            foreach (ALProjectDependency dependency in project.Dependencies)
            {
                if (dependency.Symbols != null)
                    AddXmlPorts(infoList, dependency.Symbols);
            }
            if (project.Symbols != null)
                AddXmlPorts(infoList, project.Symbols);
            return infoList;
        }

        private void AddXmlPorts(List<XmlPortInformation> infoList, ALAppSymbolReference symbols)
        {
            if (symbols.XmlPorts != null)
            {
                for (int i = 0; i < symbols.XmlPorts.Count; i++)
                    infoList.Add(new XmlPortInformation(symbols.XmlPorts[i]));
            }
        }

        #endregion

        #region Find xml port

        protected ALAppXmlPort FindXmlPort(ALProject project, string name)
        {
            ALAppXmlPort xmlPort = FindXmlPort(project.Symbols, name);
            if (xmlPort != null)
                return xmlPort;
            foreach (ALProjectDependency dependency in project.Dependencies)
            {
                xmlPort = FindXmlPort(dependency.Symbols, name);
                if (xmlPort != null)
                    return xmlPort;
            }
            return null;
        }

        protected ALAppXmlPort FindXmlPort(ALProject project, int id)
        {
            ALAppXmlPort xmlPort = FindXmlPort(project.Symbols, id);
            if (xmlPort != null)
                return xmlPort;
            foreach (ALProjectDependency dependency in project.Dependencies)
            {
                xmlPort = FindXmlPort(dependency.Symbols, id);
                if (xmlPort != null)
                    return xmlPort;
            }
            return null;
        }

        protected ALAppXmlPort FindXmlPort(ALAppSymbolReference symbols, string name)
        {
            if ((symbols != null) && (symbols.XmlPorts != null))
                return symbols.XmlPorts
                    .Where(p => (name.Equals(p.Name, StringComparison.CurrentCultureIgnoreCase)))
                    .FirstOrDefault();
            return null;
        }

        protected ALAppXmlPort FindXmlPort(ALAppSymbolReference symbols, int id)
        {
            if ((symbols != null) && (symbols.XmlPorts != null))
                return symbols.XmlPorts
                    .Where(p => (p.Id == id))
                    .FirstOrDefault();
            return null;
        }

        #endregion

        #region Get xml port table element details

        public XmlPortTableElementInformation GetXmlPortTableElementDetails(ALProject project, string xmlPortName, string xmlPortTableElementName, bool getExistingFields, bool getAvailableFields)
        {
            ALAppXmlPort xmlPort = this.FindXmlPort(project, xmlPortName);
            if ((xmlPort == null) || (xmlPort.Schema == null))
                return null;
            ALAppXmlPortNode xmlPortTableElement = xmlPort.FindNode(xmlPortTableElementName, ALAppXmlPortNodeKind.TableElement);
            if (xmlPortTableElement == null)
                return null;

            XmlPortTableElementInformation tableElementInformation = new XmlPortTableElementInformation(xmlPortTableElement);
            if ((!String.IsNullOrWhiteSpace(tableElementInformation.Source)) && (getExistingFields || getAvailableFields))
            {
                TableInformationProvider tableInformationProvider = new TableInformationProvider();
                List<TableFieldInformaton> allTableFieldsList = tableInformationProvider.GetTableFields(project, tableElementInformation.Source, false, false, true, true, false, false, null);

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

        protected void CollectXmlPortTableFields(string xmlPortTableElementName, ALAppElementsCollection<ALAppXmlPortNode> nodesList, Dictionary<string, TableFieldInformaton> availableTableFieldsDict, List<TableFieldInformaton> xmlPortTableFields)
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

        public List<ALAppVariable> GetXmlPortVariables(ALProject project, string name)
        {
            ALAppXmlPort xmlPort = this.FindXmlPort(project, name);
            if ((xmlPort != null) && (xmlPort.Variables != null) && (xmlPort.Variables.Count > 0))
                return xmlPort.Variables;
            return null;
        }

        #endregion

    }
}
