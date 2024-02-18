using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AnZwDev.ALTools.ALSymbolReferences;
using AnZwDev.ALTools.ALSymbolReferences.Compiler;
using AnZwDev.ALTools.ALSymbolReferences.Search;
using AnZwDev.ALTools.ALSymbolReferences.Serialization;
using AnZwDev.ALTools.ALSymbols;
using AnZwDev.ALTools.Workspace.SymbolReferences;
using AnZwDev.ALTools.WorkspaceCommands;

namespace AnZwDev.ALTools.Workspace.SymbolsInformation
{
    public class ReportInformationProvider : BaseExtendableObjectInformationProvider<ALAppReport, ALAppReportExtension>
    {

        public ReportInformationProvider() : base(x => x.Reports, x => x.ReportExtensions)
        {
        }

        #region Get list of reports

        public List<ReportInformation> GetReports(ALProject project)
        {
            var infoList = new List<ReportInformation>();
            var objectEnumerable = GetALAppObjectsCollection(project);
            foreach (var obj in objectEnumerable)
                infoList.Add(new ReportInformation(obj));
            return infoList;
        }

        #endregion

        #region Find report

        protected ALAppReport FindReport(ALProject project, ALObjectReference objectReference, bool parsed)
        {
            return GetALAppObjectsCollection(project)
                .FindFirst(objectReference);
        }

        #endregion

        #region Find report data item

        protected (HashSet<string>, ALAppReportDataItem, ALObjectIdentifier) FindReportDataItem(ALProject project, ALObjectReference reportReference, string dataItemName)
        {
            ALAppReportDataItem dataItem;
            //find report data item
            ALAppReport report = this.FindReport(project, reportReference, true);
            if (report != null)
            {
                if (report.DataItems != null)
                {
                    dataItem = report.FindDataItem(dataItemName);
                    if (dataItem != null)
                        return (report.Usings, dataItem, report.GetIdentifier());
                }

                //find report extension data item
                var reportIdentifier = report.GetIdentifier();
                var reportExtensionsEnumerable = project
                    .GetAllSymbolReferences()
                    .GetObjectExtensions<ALAppReportExtension>(x => x.ReportExtensions, reportIdentifier);
                foreach (var reportExtension in reportExtensionsEnumerable)
                    if (reportExtension.DataItems != null)
                    {
                        dataItem = reportExtension.FindDataItem(dataItemName);
                        if (dataItem != null)
                            return (reportExtension.Usings, dataItem, reportIdentifier);
                    }
            }

            return (null, null, new ALObjectIdentifier());
        }

        #endregion

        #region Get report data item details

        public ReportDataItemInformation GetReportDataItemInformationDetails(ALProject project, ALObjectReference reportReference, string dataItemName, bool getExistingFields, bool getAvailableFields)
        {
            (var usings, var reportDataItem, var reportIdentifier) = this.FindReportDataItem(project, reportReference, dataItemName);
            if (reportDataItem == null)
                return null;

            ReportDataItemInformation reportDataItemInformation = new ReportDataItemInformation(reportDataItem);
            var tableReference = new ALObjectReference(usings, reportDataItem.RelatedTable);
            if ((!tableReference.IsEmpty()) && (getExistingFields || getAvailableFields))
            {
                TableInformationProvider tableInformationProvider = new TableInformationProvider();
                List<TableFieldInformaton> allTableFieldsList = tableInformationProvider.GetTableFields(project, tableReference, false, false, true, true, false, false, null);

                ReportDataItemInformationFieldsBuffer dataItemFieldsBuffer = new ReportDataItemInformationFieldsBuffer(reportDataItemInformation);
                dataItemFieldsBuffer.SetTable(tableReference.Name, allTableFieldsList);

                if (reportDataItem.Columns != null)
                    this.CollectReportDataItemFields(reportDataItem.Columns, dataItemFieldsBuffer);

                //collect fields from report extensions
                var reportExtensionsEnumerable = project
                    .GetAllSymbolReferences()
                    .GetObjectExtensions<ALAppReportExtension>(x => x.ReportExtensions, reportIdentifier);
                foreach (var reportExtension in reportExtensionsEnumerable)
                {
                    if (reportExtension.Columns != null)
                        this.CollectReportDataItemFields(reportExtension.Columns.Where(p => (dataItemName.Equals(p.OwningDataItemName, StringComparison.OrdinalIgnoreCase))), dataItemFieldsBuffer); //!!! TO-DO !!! Check this code with namespaces
                }
                //add fields
                dataItemFieldsBuffer.ApplyToDataItemInformation(getExistingFields, getAvailableFields);
            }

            return reportDataItemInformation;
        }

        protected void CollectReportDataItemFields(IEnumerable<ALAppReportColumn> columnsList, ReportDataItemInformationFieldsBuffer dataItemFieldsBuffer)
        {
            foreach (ALAppReportColumn reportColumn in columnsList)
            {
                this.CollectSingleReportDataItemFields(reportColumn, dataItemFieldsBuffer);
            }
        }

        protected void CollectAllReportDataItemFields(IEnumerable<ALAppReportColumn> columnsList, Dictionary<string, ReportDataItemInformationFieldsBuffer> dataItemFieldsBuffersDict)
        {
            foreach (ALAppReportColumn reportColumn in columnsList)
            {
                if (!String.IsNullOrWhiteSpace(reportColumn.OwningDataItemName))
                {
                    string dataItemName = reportColumn.OwningDataItemName.ToLower();
                    if (dataItemFieldsBuffersDict.ContainsKey(dataItemName))
                        this.CollectSingleReportDataItemFields(reportColumn, dataItemFieldsBuffersDict[dataItemName]);
                }
            }
        }

        protected void CollectSingleReportDataItemFields(ALAppReportColumn reportColumn, ReportDataItemInformationFieldsBuffer dataItemFieldsBuffer)
        {
            if (!String.IsNullOrWhiteSpace(reportColumn.SourceExpression))
            {
                ALMemberAccessExpression memberAccessExpression = ALSyntaxHelper.DecodeMemberAccessExpression(reportColumn.SourceExpression);
                bool isMemberAccess = !String.IsNullOrWhiteSpace(memberAccessExpression.Expression);

                string sourceExpression = null;
                if (!isMemberAccess)
                    sourceExpression = memberAccessExpression.Name.ToLower();
                else if (dataItemFieldsBuffer.Name.Equals(memberAccessExpression.Name, StringComparison.OrdinalIgnoreCase))
                    sourceExpression = memberAccessExpression.Expression.ToLower();

                if ((!String.IsNullOrWhiteSpace(sourceExpression)) && (dataItemFieldsBuffer.AvailableTableFieldsDict.ContainsKey(sourceExpression)))
                {
                    dataItemFieldsBuffer.ReportDataItemFields.Add(dataItemFieldsBuffer.AvailableTableFieldsDict[sourceExpression]);
                    dataItemFieldsBuffer.AvailableTableFieldsDict.Remove(sourceExpression);
                }
            }
        }



        #endregion

        #region Report variables

        public List<ALAppVariable> GetReportVariables(ALProject project, ALObjectReference objectReference)
        {
            List<ALAppVariable> variables = new List<ALAppVariable>();

            ALAppReport report = this.FindReport(project, objectReference, false);
            if ((report != null) && (report.Variables != null) && (report.Variables.Count > 0))
                variables.AddRange(report.Variables);

            var extensionsEnumerable = GetALAppObjectExtensionsCollection(project, report);
            foreach (var extension in extensionsEnumerable)
                if ((extension.Variables != null) && (extension.Variables.Count > 0))
                    variables.AddRange(extension.Variables);

            return variables;
        }

        #endregion

        #region Full report information

        public ReportInformation GetFullReportInformation(ALProject project, ALObjectReference reportReference)
        {
            ALAppReport report = this.FindReport(project, reportReference, true);
            if (report == null)
                return null;

            Dictionary<string, ReportDataItemInformationFieldsBuffer> dataItemsFieldsBufferDictionary = new Dictionary<string, ReportDataItemInformationFieldsBuffer>();

            ReportInformation reportInformation = new ReportInformation(report)
            {
                DataItems = new List<ReportDataItemInformation>()
            };
            if (report.DataItems != null)
                this.AddDataItemInformationList(project, reportInformation.DataItems, dataItemsFieldsBufferDictionary, report.Usings, report.DataItems);

            //add report data items from report extensions
            var extensionsEnumerable = GetALAppObjectExtensionsCollection(project, report);
            foreach (var reportExtension in extensionsEnumerable)
            {
                if (reportExtension.DataItems != null)
                    this.AddDataItemInformationList(project, reportInformation.DataItems, dataItemsFieldsBufferDictionary, reportExtension.Usings, reportExtension.DataItems);
                if (reportExtension.Columns != null)
                    this.CollectAllReportDataItemFields(reportExtension.Columns, dataItemsFieldsBufferDictionary);

            }

            //apply field buffers to data items information entries
            foreach (ReportDataItemInformationFieldsBuffer fieldsBuffer in dataItemsFieldsBufferDictionary.Values)
            {
                fieldsBuffer.ApplyToDataItemInformation(true, true);
            }

            //return report information
            return reportInformation;
        }

        protected void AddDataItemInformationList(ALProject project, List<ReportDataItemInformation> dataItemInfoList, Dictionary<string, ReportDataItemInformationFieldsBuffer> dataItemsFieldsBuffer, HashSet<string> usings, List<ALAppReportDataItem> dataItemsList)
        {
            for (int i = 0; i < dataItemsList.Count; i++)
                this.AddDataItemInformation(project, dataItemInfoList, dataItemsFieldsBuffer, usings, dataItemsList[i], 0);
        }

        protected void AddDataItemInformation(ALProject project, List<ReportDataItemInformation> dataItemInfoList, Dictionary<string, ReportDataItemInformationFieldsBuffer> dataItemsFieldsBuffer, HashSet<string> usings, ALAppReportDataItem dataItem, int indent)
        {
            //create data item information
            if (!String.IsNullOrWhiteSpace(dataItem.Name))
            {
                string nameKey = dataItem.Name.ToLower();
                ReportDataItemInformationFieldsBuffer fieldsBuffer;
                if (dataItemsFieldsBuffer.ContainsKey(nameKey))
                    fieldsBuffer = dataItemsFieldsBuffer[nameKey];
                else
                {
                    ReportDataItemInformation reportDataItemInformation = new ReportDataItemInformation(dataItem)
                    {
                        Indent = indent
                    };
                    fieldsBuffer = new ReportDataItemInformationFieldsBuffer(reportDataItemInformation);
                    dataItemsFieldsBuffer.Add(nameKey, fieldsBuffer);

                    var tableReference = new ALObjectReference(usings, dataItem.RelatedTable);
                    if (!tableReference.IsEmpty())
                    {
                        TableInformationProvider tableInformationProvider = new TableInformationProvider();
                        List<TableFieldInformaton> allTableFieldsList = tableInformationProvider.GetTableFields(project, tableReference, false, false, true, true, false, false, null);

                        fieldsBuffer.SetTable(tableReference.GetFullName(), allTableFieldsList);
                    }

                    dataItemInfoList.Add(reportDataItemInformation);
                }

                if (dataItem.Columns != null)
                    this.CollectReportDataItemFields(dataItem.Columns, fieldsBuffer);
            }

            //add child data items
            if (dataItem.DataItems != null)
                for (int i=0; i<dataItem.DataItems.Count;i++)
                    this.AddDataItemInformation(project, dataItemInfoList, dataItemsFieldsBuffer, usings,dataItem.DataItems[i], indent + 1);
        }

        #endregion

    }
}
