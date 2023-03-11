using AnZwDev.ALTools.ALSymbols;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AnZwDev.ALTools.ALSymbolReferences;
using AnZwDev.ALTools.Extensions;
using AnZwDev.ALTools.ALSymbolReferences.MergedReferences;
using AnZwDev.ALTools.Workspace.SymbolsInformation.Internal;

namespace AnZwDev.ALTools.Workspace.SymbolsInformation
{
    public class PageInformationProvider : BaseExtendableObjectInformationProvider<ALAppPage, ALAppPageExtension>
    {

        public PageInformationProvider()
        {
        }

        #region Objects list and search

        protected override MergedALAppObjectsCollection<ALAppPage> GetALAppObjectsCollection(ALProject project)
        {
            return project.AllSymbols.Pages;
        }

        protected override MergedALAppObjectExtensionsCollection<ALAppPageExtension> GetALAppObjectExtensionsCollection(ALProject project)
        {
            return project.AllSymbols.PageExtensions;
        }

        #endregion

        #region Get list of pages

        public List<PageInformation> GetPages(ALProject project)
        {
            List<PageInformation> infoList = new List<PageInformation>();
            IEnumerable<ALAppPage> alAppPagesCollection = this.GetALAppObjectsCollection(project).GetObjects();
            foreach (ALAppPage alAppPage in alAppPagesCollection)
            {
                infoList.Add(new PageInformation(alAppPage));
            }

            /*
            foreach (ALProjectDependency dependency in project.Dependencies)
            {
                if (dependency.Symbols != null)
                    AddPages(infoList, dependency.Symbols);
            }
            if (project.Symbols != null)
                AddPages(infoList, project.Symbols);
            */
            return infoList;
        }

        /*
        private void AddPages(List<PageInformation> infoList, ALAppSymbolReference symbols)
        {
            if (symbols.Pages != null)
            {
                for (int i = 0; i < symbols.Pages.Count; i++)
                    infoList.Add(new PageInformation(symbols.Pages[i]));
            }
        }
        */

        #endregion

        #region Get page details

        public PageInformation GetPageDetails(ALProject project, string pageName, bool getExistingFields, bool getAvailableFields, bool getToolTips, IEnumerable<string> toolTipsSourceDependencies)
        {
            ALAppPage pageSymbol = this.GetALAppObjectsCollection(project).FindObject(pageName);
            if (pageSymbol == null)
                return null;
            PageInformation pageInformation = new PageInformation(pageSymbol);

            //collect fields
            if ((!String.IsNullOrWhiteSpace(pageInformation.Source)) && (getExistingFields || getAvailableFields))
            {
                TableInformationProvider tableInformationProvider = new TableInformationProvider();
                List<TableFieldInformaton> allTableFieldsList = tableInformationProvider.GetTableFields(project, pageInformation.Source, false, false, true, true, false, getToolTips, toolTipsSourceDependencies);

                Dictionary<string, TableFieldInformaton> availableTableFieldsDict = allTableFieldsList.ToDictionary();

                List<TableFieldInformaton> pageTableFields = new List<TableFieldInformaton>();
                
                if (pageSymbol.Controls != null)
                    this.CollectVisibleFields(pageSymbol.Controls, availableTableFieldsDict, pageTableFields);

                //collect fields from page extensions
                IEnumerable<ALAppPageExtension> alAppPageExtensionsCollection = this.GetALAppObjectExtensionsCollection(project).FindAllExtensions(pageName);
                foreach (ALAppPageExtension pageExtensionSymbol in alAppPageExtensionsCollection)
                {
                    if (pageExtensionSymbol.ControlChanges != null)
                        this.CollectVisibleFields(pageExtensionSymbol.ControlChanges, availableTableFieldsDict, pageTableFields);
                }

                if (getExistingFields)
                    pageInformation.ExistingTableFields = pageTableFields;
                if (getAvailableFields)
                    pageInformation.AvailableTableFields = availableTableFieldsDict.Values.ToList();
            }

            return pageInformation;
        }

        protected void CollectVisibleFields(ALAppElementsCollection<ALAppPageControlChange> controlsList, Dictionary<string, TableFieldInformaton> availableTableFieldsDict, List<TableFieldInformaton> pageTableFields)
        {
            for (int i=0; i<controlsList.Count; i++)
            {
                if (controlsList[i].Controls != null)
                    this.CollectVisibleFields(controlsList[i].Controls, availableTableFieldsDict, pageTableFields);
            }
        }


        protected void CollectVisibleFields(ALAppElementsCollection<ALAppPageControl> controlsList, Dictionary<string, TableFieldInformaton> availableTableFieldsDict, List<TableFieldInformaton> pageTableFields)
        {
            for (int i=0; i<controlsList.Count; i++)
            {
                ALAppPageControl pageControl = controlsList[i];
                string sourceExpression = pageControl.GetSourceExpression();
                if (!String.IsNullOrWhiteSpace(sourceExpression))
                {
                    ALMemberAccessExpression memberAccessExpression = ALSyntaxHelper.DecodeMemberAccessExpression(sourceExpression);
                    sourceExpression = memberAccessExpression.GetSourceFieldNameWithoutRec();
                    if (!String.IsNullOrWhiteSpace(sourceExpression))
                    {
                        sourceExpression = sourceExpression.ToLower();
                        if (availableTableFieldsDict.ContainsKey(sourceExpression))
                        {
                            pageTableFields.Add(availableTableFieldsDict[sourceExpression]);
                            availableTableFieldsDict.Remove(sourceExpression);
                        }
                    }
                }

                if (pageControl.Controls != null)
                    this.CollectVisibleFields(pageControl.Controls, availableTableFieldsDict, pageTableFields);
            }
        }

        #endregion

        #region Page fields

        public List<TableFieldInformaton> GetAllTableFieldsForPage(ALProject project, string pageName, bool includeDisabled, bool includeObsolete, bool includeNormal, bool includeFlowFields, bool includeFlowFilters)
        {
            ALAppPage page = this.GetALAppObjectsCollection(project).FindObject(pageName);
            if (page != null)
            {
                string tableName = page.Properties.GetValue("SourceTable");
                if (!String.IsNullOrWhiteSpace(tableName))
                {
                    TableInformationProvider tableInformationProvider = new TableInformationProvider();
                    return tableInformationProvider.GetTableFields(project, tableName, includeDisabled, includeObsolete, includeNormal, includeFlowFields, includeFlowFilters, false, null);
                }
            }
            return null;
        }

        #endregion

        #region List of tables used on pages in the project

        public IEnumerable<string> GetProjectTablesList(ALProject project)
        {
            HashSet<string> tables = new HashSet<string>();
            //collect tables from pages
            if (project.Symbols.Pages != null)
            {
                foreach (ALAppPage page in project.Symbols.Pages)
                {
                    string tableName = page.GetSourceTable()?.ToLower();
                    if ((!String.IsNullOrWhiteSpace(tableName)) && (!tables.Contains(tableName)))
                        tables.Add(tableName);
                }
            }
            //collect tables from page extensions
            if (project.Symbols.PageExtensions != null)
            {
                foreach (ALAppPageExtension pageExtension in project.Symbols.PageExtensions)
                {
                    string pageName = pageExtension.GetTargetObjectName();
                    if (!String.IsNullOrWhiteSpace(pageName))
                    {
                        ALAppPage page = project.AllSymbols.Pages.FindObject(pageName);
                        if (page != null)
                        {
                            string tableName = page.GetSourceTable()?.ToLower();
                            if ((!String.IsNullOrWhiteSpace(tableName)) && (!tables.Contains(tableName)))
                                tables.Add(tableName);
                        }
                    }
                }
            }
            return tables;
        }

        #endregion

        #region Tooltips information

        public List<string> GetPageFieldAvailableToolTips(ALProject project, string objectType, string objectName, string sourceTable, string fieldExpression)
        {
            if ((String.IsNullOrWhiteSpace(objectType)) || (String.IsNullOrWhiteSpace(objectName)) || (String.IsNullOrWhiteSpace(fieldExpression)))
                return null;

            //find source table if not specified
            if (String.IsNullOrWhiteSpace(sourceTable))
            {
                string pageName = null;
                if (objectType.Equals("PageExtension", StringComparison.CurrentCultureIgnoreCase))
                    pageName = this.GetALAppObjectExtensionsCollection(project).FindObject(objectName)?.GetTargetObjectName();
                else if (objectType.Equals("Page", StringComparison.CurrentCultureIgnoreCase))
                    pageName = objectName;
                if (pageName != null)
                    sourceTable = this.GetALAppObjectsCollection(project).FindObject(pageName)?.GetSourceTable();
                if (String.IsNullOrWhiteSpace(sourceTable))
                    return null;
            }

            //find field name
            ALMemberAccessExpression memberAccessExpression = ALSyntaxHelper.DecodeMemberAccessExpression(fieldExpression);
            string fieldName = memberAccessExpression.GetSourceFieldNameWithoutRec();
            if (String.IsNullOrWhiteSpace(fieldName))
                return null;

            //collect table fields tooltips
            string[] tableNames = { sourceTable };
            string tableKey = sourceTable.ToLower();
            string fieldKey = fieldName.ToLower();
            Dictionary<string, Dictionary<string, List<string>>> allToolTips = this.CollectTableFieldsToolTips(project, tableNames, null);
            if ((allToolTips.ContainsKey(tableKey)) && (allToolTips[tableKey].ContainsKey(fieldKey)))
                return allToolTips[tableKey][fieldKey];

            return null;
        }

        public Dictionary<string, Dictionary<string, List<string>>> CollectProjectTableFieldsToolTips(ALProject project, IEnumerable<string> dependenciesList)
        {
            return this.CollectTableFieldsToolTips(project, GetProjectTablesList(project), dependenciesList);
        }

        public Dictionary<string, Dictionary<string, List<string>>> CollectTableFieldsToolTips(ALProject project, IEnumerable<string> tableNamesList, IEnumerable<string> dependenciesList)
        {
            HashSet<string> dependenciesHashSet = ((dependenciesList != null) && (!dependenciesList.Contains("*")))? dependenciesList.ToHashSet(true) : null;
            HashSet<string> tableNamesHashSet = tableNamesList.ToLowerCaseHashSet();
            Dictionary<string, IntPageWithControlsWithPropertyValue> pagesCacheDictionary = new Dictionary<string, IntPageWithControlsWithPropertyValue>();

            //collect pages with controls
            IEnumerable<ALAppPage> alAppPagesCollection = this.GetALAppObjectsCollection(project).GetObjects(dependenciesHashSet);
            foreach (ALAppPage alAppPage in alAppPagesCollection)
            {
                string tableName = alAppPage.GetSourceTable()?.ToLower();
                string pageName = alAppPage.Name?.ToLower();
                if ((!String.IsNullOrWhiteSpace(tableName)) && (tableNamesHashSet.Contains(tableName)) && (!String.IsNullOrWhiteSpace(pageName)) && (!pagesCacheDictionary.ContainsKey(pageName)))
                    pagesCacheDictionary.Add(pageName, new IntPageWithControlsWithPropertyValue(alAppPage, "ToolTip"));
            }

            //apply extensions
            IEnumerable<ALAppPageExtension> alAppPageExtensionsCollection = this.GetALAppObjectExtensionsCollection(project).GetObjects(dependenciesHashSet);
            foreach (ALAppPageExtension alAppPageExtension in alAppPageExtensionsCollection)
            {
                string pageName = alAppPageExtension.GetTargetObjectName()?.ToLower();
                if ((!String.IsNullOrWhiteSpace(pageName)) && (pagesCacheDictionary.ContainsKey(pageName)))
                    pagesCacheDictionary[pageName].ApplyPageExtension(alAppPageExtension);
            }

            //collect property values
            Dictionary<string, Dictionary<string, List<string>>> tablesPropertyValuesDictionary = new Dictionary<string, Dictionary<string, List<string>>>();
            foreach (IntPageWithControlsWithPropertyValue pageCache in pagesCacheDictionary.Values)
            {
                string tableName = pageCache.SourceTable.ToLower();
                Dictionary<string, List<string>> tableFieldsProperties = tablesPropertyValuesDictionary.FindOrCreate(tableName);
                //add fields
                foreach (IntPageControlWithPropertyValue controlCache in pageCache.Controls.Values)
                {
                    if (!String.IsNullOrWhiteSpace(controlCache.PropertyValue))
                    {
                        //get field name
                        string fieldName = null;
                        ALMemberAccessExpression memberAccessExpression = ALSyntaxHelper.DecodeMemberAccessExpression(controlCache.ControlSource);
                        fieldName = memberAccessExpression.GetSourceFieldNameWithoutRec();
                        //collect field name
                        if (!String.IsNullOrWhiteSpace(fieldName))
                        {
                            fieldName = fieldName.ToLower();
                            List<string> propertyValues = tableFieldsProperties.FindOrCreate(fieldName);
                            if (!propertyValues.Contains(controlCache.PropertyValue))
                                propertyValues.Add(controlCache.PropertyValue);
                        }
                    }
                }
            }

            return tablesPropertyValuesDictionary;
        }

        #endregion

    }
}

