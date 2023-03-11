using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AnZwDev.ALTools.ALSymbolReferences;
using AnZwDev.ALTools.ALSymbolReferences.Compiler;
using AnZwDev.ALTools.ALSymbolReferences.Serialization;
using AnZwDev.ALTools.ALSymbols;

namespace AnZwDev.ALTools.Workspace.SymbolsInformation
{
    public class ReportInformationProvider
    {

        public ReportInformationProvider()
        {
        }

        #region Get list of reports

        public List<ReportInformation> GetReports(ALProject project)
        {
            List<ReportInformation> infoList = new List<ReportInformation>();
            foreach (ALProjectDependency dependency in project.Dependencies)
            {
                if (dependency.Symbols != null)
                    AddReports(infoList, dependency.Symbols);
            }
            if (project.Symbols != null)
                AddReports(infoList, project.Symbols);
            return infoList;
        }

        private void AddReports(List<ReportInformation> infoList, ALAppSymbolReference symbols)
        {
            if (symbols.Reports != null)
            {
                for (int i = 0; i < symbols.Reports.Count; i++)
                    infoList.Add(new ReportInformation(symbols.Reports[i]));
            }
        }

        #endregion

        #region Find report

        protected ALAppReport FindReport(ALProject project, string name, bool parsed)
        {
            ALAppReport report = FindReport(project.Symbols, name, parsed);
            if (report != null)
                return report;
            foreach (ALProjectDependency dependency in project.Dependencies)
            {
                report = FindReport(dependency.Symbols, name, parsed);
                if (report != null)
                    return report;
            }
            return null;
        }

        protected ALAppReport FindReport(ALProject project, int id, bool parsed)
        {
            ALAppReport report = FindReport(project.Symbols, id, parsed);
            if (report != null)
                return report;
            foreach (ALProjectDependency dependency in project.Dependencies)
            {
                report = FindReport(dependency.Symbols, id, parsed);
                if (report != null)
                    return report;
            }
            return null;
        }

        protected ALAppReport FindReport(ALAppSymbolReference symbols, string name, bool parsed)
        {
            if (symbols != null)
                return symbols.FindObjectByName(symbols.Reports, name, parsed);
            return null;
        }

        protected ALAppReport FindReport(ALAppSymbolReference symbols, int id, bool parsed)
        {
            if (symbols != null)
                return symbols.FindObjectById(symbols.Reports, id, parsed);
            return null;
        }

        #endregion

        #region Find report extension

        protected ALAppReportExtension FindReportExtension(ALAppSymbolReference symbols, string reportName, bool parsed)
        {
            if ((symbols != null) && (symbols.ReportExtensions != null))
            {
                ALAppReportExtension reportExtension = symbols.ReportExtensions
                    .Where(p => (reportName.Equals(p.Target, StringComparison.CurrentCultureIgnoreCase)))
                    .FirstOrDefault();
                if ((reportExtension != null) && (parsed) && (!reportExtension.INT_Parsed))
                {
                    symbols.ParseObject(reportExtension);
                    reportExtension = symbols.ReportExtensions
                        .Where(p => (reportName.Equals(p.Target, StringComparison.CurrentCultureIgnoreCase)))
                        .FirstOrDefault();
                }
                return reportExtension;
            }
            return null;
        }

        #endregion

        #region Find report data item

        protected ALAppReportDataItem FindReportDataItem(ALProject project, string reportName, string dataItemName)
        {
            ALAppReportDataItem dataItem;
            //find report data item
            ALAppReport report = this.FindReport(project, reportName, true);
            if ((report != null) && (report.DataItems != null))
            {
                dataItem = report.FindDataItem(dataItemName);
                if (dataItem != null)
                    return dataItem;
            }

            //find report extension data item
            ALAppReportExtension reportExtension = FindReportExtension(project.Symbols, reportName, true);
            if ((reportExtension != null) && (reportExtension.DataItems != null))
            {
                dataItem = reportExtension.FindDataItem(dataItemName);
                if (dataItem != null)
                    return dataItem;
            }

            foreach (ALProjectDependency dependency in project.Dependencies)
            {
                reportExtension = FindReportExtension(dependency.Symbols, reportName, true);
                if ((reportExtension != null) && (reportExtension.DataItems != null))
                {
                    dataItem = reportExtension.FindDataItem(dataItemName);
                    if (dataItem != null)
                        return dataItem;
                }
            }
            return null;
        }

        #endregion

        #region Get report data item details

        public ReportDataItemInformation GetReportDataItemInformationDetails(ALProject project, string reportName, string dataItemName, bool getExistingFields, bool getAvailableFields)
        {
            ALAppReportDataItem reportDataItem = this.FindReportDataItem(project, reportName, dataItemName);
            if (reportDataItem == null)
                return null;

            ReportDataItemInformation reportDataItemInformation = new ReportDataItemInformation(reportDataItem);
            if ((!String.IsNullOrWhiteSpace(reportDataItem.RelatedTable)) && (getExistingFields || getAvailableFields))
            {
                string tableName = reportDataItem.RelatedTable;
                TableInformationProvider tableInformationProvider = new TableInformationProvider();
                List<TableFieldInformaton> allTableFieldsList = tableInformationProvider.GetTableFields(project, tableName, false, false, true, true, false, false, null);

                ReportDataItemInformationFieldsBuffer dataItemFieldsBuffer = new ReportDataItemInformationFieldsBuffer(reportDataItemInformation);
                dataItemFieldsBuffer.SetTable(tableName, allTableFieldsList);

                if (reportDataItem.Columns != null)
                    this.CollectReportDataItemFields(reportDataItem.Columns, dataItemFieldsBuffer);

                //collect fields from report extensions
                foreach (ALProjectDependency dependency in project.Dependencies)
                {
                    ALAppReportExtension reportExtension = FindReportExtension(dependency.Symbols, reportName, true);
                    if ((reportExtension != null) && (reportExtension.Columns != null))
                        this.CollectReportDataItemFields(reportExtension.Columns.Where(p => (dataItemName.Equals(p.OwningDataItemName, StringComparison.CurrentCultureIgnoreCase))), dataItemFieldsBuffer);
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
                else if (dataItemFieldsBuffer.Name.Equals(memberAccessExpression.Name, StringComparison.CurrentCultureIgnoreCase))
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

        public List<ALAppVariable> GetReportVariables(ALProject project, string name)
        {
            List<ALAppVariable> variables = new List<ALAppVariable>();

            ALAppReport report = this.FindReport(project, name, false);
            if ((report != null) && (report.Variables != null) && (report.Variables.Count > 0))
                variables.AddRange(report.Variables);

            ALAppReportExtension reportExtension = FindReportExtension(project.Symbols, name, false);
            if ((reportExtension != null) && (reportExtension.Variables != null) && (reportExtension.Variables.Count > 0))
                variables.AddRange(reportExtension.Variables);
            foreach (ALProjectDependency dependency in project.Dependencies)
            {
                reportExtension = FindReportExtension(dependency.Symbols, name, false);
                if ((reportExtension != null) && (reportExtension.Variables != null) && (reportExtension.Variables.Count > 0))
                    variables.AddRange(reportExtension.Variables);
            }

            return variables;
        }

        #endregion

        #region Full report information

        public ReportInformation GetFullReportInformation(ALProject project, string reportName)
        {
            ALAppReport report = this.FindReport(project, reportName, true);
            if (report == null)
                return null;

            Dictionary<string, ReportDataItemInformationFieldsBuffer> dataItemsFieldsBufferDictionary = new Dictionary<string, ReportDataItemInformationFieldsBuffer>();

            ReportInformation reportInformation = new ReportInformation(report)
            {
                DataItems = new List<ReportDataItemInformation>()
            };
            if (report.DataItems != null)
                this.AddDataItemInformationList(project, reportInformation.DataItems, dataItemsFieldsBufferDictionary, report.DataItems);

            //add report data items from report extensions
            foreach (ALProjectDependency dependency in project.Dependencies)
            {
                ALAppReportExtension reportExtension = FindReportExtension(dependency.Symbols, reportName, true);
                if (reportExtension != null)
                {
                    if (reportExtension.DataItems != null)
                        this.AddDataItemInformationList(project, reportInformation.DataItems, dataItemsFieldsBufferDictionary, reportExtension.DataItems);
                    if (reportExtension.Columns != null)
                        this.CollectAllReportDataItemFields(reportExtension.Columns, dataItemsFieldsBufferDictionary);
                }
            }

            //apply field buffers to data items information entries
            foreach (ReportDataItemInformationFieldsBuffer fieldsBuffer in dataItemsFieldsBufferDictionary.Values)
            {
                fieldsBuffer.ApplyToDataItemInformation(true, true);
            }

            //return report information
            return reportInformation;
        }

        protected void AddDataItemInformationList(ALProject project, List<ReportDataItemInformation> dataItemInfoList, Dictionary<string, ReportDataItemInformationFieldsBuffer> dataItemsFieldsBuffer, List<ALAppReportDataItem> dataItemsList)
        {
            for (int i = 0; i < dataItemsList.Count; i++)
            {
                this.AddDataItemInformation(project, dataItemInfoList, dataItemsFieldsBuffer, dataItemsList[i], 0);
            }
        }

        protected void AddDataItemInformation(ALProject project, List<ReportDataItemInformation> dataItemInfoList, Dictionary<string, ReportDataItemInformationFieldsBuffer> dataItemsFieldsBuffer, ALAppReportDataItem dataItem, int indent)
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

                    if (!String.IsNullOrWhiteSpace(dataItem.RelatedTable))
                    {
                        string tableName = dataItem.RelatedTable;
                        TableInformationProvider tableInformationProvider = new TableInformationProvider();
                        List<TableFieldInformaton> allTableFieldsList = tableInformationProvider.GetTableFields(project, tableName, false, false, true, true, false, false, null);

                        fieldsBuffer.SetTable(tableName, allTableFieldsList);
                    }

                    dataItemInfoList.Add(reportDataItemInformation);
                }

                if (dataItem.Columns != null)
                    this.CollectReportDataItemFields(dataItem.Columns, fieldsBuffer);
            }

            //add child data items
            if (dataItem.DataItems != null)
            {
                for (int i=0; i<dataItem.DataItems.Count;i++)
                {
                    this.AddDataItemInformation(project, dataItemInfoList, dataItemsFieldsBuffer, dataItem.DataItems[i], indent + 1);
                }
            }
        }

        #endregion

    }
}
