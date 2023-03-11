using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnZwDev.ALTools.Workspace.SymbolsInformation
{
    public class ReportDataItemInformationFieldsBuffer
    {


        public ReportDataItemInformation DataItemInformation { get; }
        public string Name 
        { 
            get { return this.DataItemInformation.Name; } 
        }
        public string TableName { get; private set; }
        public List<TableFieldInformaton> AllTableFieldsList { get; private set; }
        public Dictionary<string, TableFieldInformaton> AvailableTableFieldsDict { get; private set; }
        public List<TableFieldInformaton> ReportDataItemFields { get; private set; }

        public ReportDataItemInformationFieldsBuffer(ReportDataItemInformation newDataItemInformation)
        {
            this.DataItemInformation = newDataItemInformation;
            this.TableName = null;
            this.AllTableFieldsList = null;
            this.AvailableTableFieldsDict = null;
            this.ReportDataItemFields = null;
        }

        public void SetTable(string newTableName, List<TableFieldInformaton> newAllTableFieldsList)
        {
            this.TableName = newTableName;
            this.AllTableFieldsList = newAllTableFieldsList;
            this.AvailableTableFieldsDict = newAllTableFieldsList.ToDictionary();
            this.ReportDataItemFields = new List<TableFieldInformaton>();
        }

        public void ApplyToDataItemInformation(bool getExistingFields, bool getAvailableFields)
        {
            if (getExistingFields)
                this.DataItemInformation.ExistingTableFields = this.ReportDataItemFields;
            if ((getAvailableFields) && (this.AvailableTableFieldsDict != null))
                this.DataItemInformation.AvailableTableFields = this.AvailableTableFieldsDict.Values.ToList();
        }

    }
}
