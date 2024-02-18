using AnZwDev.ALTools.ALSymbols;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AnZwDev.ALTools.ALSymbolReferences;
using AnZwDev.ALTools.Extensions;
using AnZwDev.ALTools.ALSymbolReferences.MergedReferences;
using AnZwDev.ALTools.Workspace.SymbolsInformation.Internal;
using AnZwDev.ALTools.Workspace.SymbolReferences;
using AnZwDev.ALTools.ALSymbolReferences.Search;
using AnZwDev.ALTools.CodeTransformations;

namespace AnZwDev.ALTools.Workspace.SymbolsInformation
{

    //!!! TO-DO !!!
    //!!! Review !!!
    //!!! Fix issues !!!

    public class PageInformationProvider : BaseExtendableObjectInformationProvider<ALAppPage, ALAppPageExtension>
    {

        public PageInformationProvider() : base(x => x.Pages, x => x.PageExtensions)
        {
        }

        #region Get list of pages

        public List<PageInformation> GetPages(ALProject project)
        {
            var infoList = new List<PageInformation>();
            var alAppPagesCollection = this.GetALAppObjectsCollection(project);
            foreach (ALAppPage alAppPage in alAppPagesCollection)
                infoList.Add(new PageInformation(alAppPage));
            return infoList;
        }

        #endregion

        #region Get page details

        public PageInformation GetPageDetails(ALProject project, ALObjectReference pageReference, bool getExistingFields, bool getAvailableFields, bool getToolTips, IEnumerable<string> toolTipsSourceDependencies)
        {
            ALAppPage pageObject = this.GetALAppObjectsCollection(project).FindFirst(pageReference);
            if (pageObject == null)
                return null;
            PageInformation pageInformation = new PageInformation(pageObject);

            //collect fields
            var tableReference = pageObject.GetSourceTable();
            if ((!tableReference.IsEmpty()) && (getExistingFields || getAvailableFields))
            {
                TableInformationProvider tableInformationProvider = new TableInformationProvider();
                List<TableFieldInformaton> allTableFieldsList = tableInformationProvider.GetTableFields(project, tableReference, false, false, true, true, false, getToolTips, toolTipsSourceDependencies);

                Dictionary<string, TableFieldInformaton> availableTableFieldsDict = allTableFieldsList.ToDictionary();

                List<TableFieldInformaton> pageTableFields = new List<TableFieldInformaton>();
                
                if (pageObject.Controls != null)
                    this.CollectVisibleFields(pageObject.Controls, availableTableFieldsDict, pageTableFields);

                //collect fields from page extensions
                IEnumerable<ALAppPageExtension> alAppPageExtensionsCollection = this.GetALAppObjectExtensionsCollection(project, pageObject);
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

        protected void CollectVisibleFields(ALAppSymbolsCollection<ALAppPageControlChange> controlsList, Dictionary<string, TableFieldInformaton> availableTableFieldsDict, List<TableFieldInformaton> pageTableFields)
        {
            for (int i=0; i<controlsList.Count; i++)
            {
                if (controlsList[i].Controls != null)
                    this.CollectVisibleFields(controlsList[i].Controls, availableTableFieldsDict, pageTableFields);
            }
        }


        protected void CollectVisibleFields(ALAppSymbolsCollection<ALAppPageControl> controlsList, Dictionary<string, TableFieldInformaton> availableTableFieldsDict, List<TableFieldInformaton> pageTableFields)
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

        public List<TableFieldInformaton> GetAllTableFieldsForPage(ALProject project, ALAppPage page, bool includeDisabled, bool includeObsolete, bool includeNormal, bool includeFlowFields, bool includeFlowFilters)
        {
            if (page != null)
            {
                var tableReference = page.GetSourceTable();
                if (!tableReference.IsEmpty())
                {
                    TableInformationProvider tableInformationProvider = new TableInformationProvider();
                    return tableInformationProvider.GetTableFields(project, tableReference, includeDisabled, includeObsolete, includeNormal, includeFlowFields, includeFlowFilters, false, null);
                }
            }
            return null;
        }

        #endregion

        #region List of tables used on pages in the project

        private void CollectPageTableIdentifiers(Dictionary<int, ALObjectIdentifier> tableIdentifiersCollection, ALProject project, ALAppPage page)
        {
            var tableReference = page.GetSourceTable();
            if (!tableReference.IsEmpty())
            {
                var table = project.GetAllSymbolReferences()
                    .GetAllObjects<ALAppTable>(x => x.Tables)
                    .FindFirst(tableReference);
                if (table != null)
                {
                    var tableIdentifier = table.GetIdentifier();
                    if (!tableIdentifiersCollection.ContainsKey(tableReference.ObjectId))
                        tableIdentifiersCollection.Add(tableReference.ObjectId, tableIdentifier);
                }
            }
        }

        public IEnumerable<ALObjectIdentifier> GetProjectTablesIdentifiers(ALProject project)
        {
            Dictionary<int, ALObjectIdentifier> tableReferencesCollection = new Dictionary<int, ALObjectIdentifier>();

            //collect tables from pages
            if (project.Symbols.Pages != null)
            {
                foreach (ALAppPage page in project.Symbols.Pages)
                    CollectPageTableIdentifiers(tableReferencesCollection, project, page);
            }
            //collect tables from page extensions
            if (project.Symbols.PageExtensions != null)
            {
                foreach (ALAppPageExtension pageExtension in project.Symbols.PageExtensions)
                {
                    var pageReference = pageExtension.GetTargetObjectReference();
                    if (!pageReference.IsEmpty())
                    { 
                        var page = project.GetAllSymbolReferences()
                            .GetAllObjects<ALAppPage>(x => x.Pages)
                            .FindFirst(pageReference);
                        if (page != null)
                            CollectPageTableIdentifiers(tableReferencesCollection, project, page);
                    }
                }
            }

            return tableReferencesCollection.Values;
        }

        #endregion

        #region Tooltips information

        public List<LabelInformation> GetPageFieldAvailableToolTips(ALProject project, string objectType, ALObjectIdentifier fieldOwnerIdentifier, ALObjectReference tableReference, string fieldExpression)
        {
            if ((String.IsNullOrWhiteSpace(objectType)) || (String.IsNullOrWhiteSpace(fieldExpression)))
                return null;

            //find source table if not specified
            if (tableReference.IsEmpty())
            {
                if (objectType.Equals("PageExtension", StringComparison.OrdinalIgnoreCase))
                {
                    var pageExtension = project
                        .GetAllSymbolReferences()
                        .GetAllObjects<ALAppPageExtension>(x => x.PageExtensions)
                        .FindFirst(fieldOwnerIdentifier);
                    if (pageExtension != null)
                    {
                        var page = project
                            .GetAllSymbolReferences()
                            .GetAllObjects<ALAppPage>(x => x.Pages)
                            .FindFirst(pageExtension.GetTargetObjectReference());
                        tableReference = page.GetSourceTable();
                    }
                }
                else if (objectType.Equals("Page", StringComparison.OrdinalIgnoreCase))
                {
                    var page = project
                        .GetAllSymbolReferences()
                        .GetAllObjects<ALAppPage>(x => x.Pages)
                        .FindFirst(fieldOwnerIdentifier);
                    tableReference = page.GetSourceTable();
                }

                if (tableReference.IsEmpty())
                    return null;
            }

            //find table
            var table = project
                .GetAllSymbolReferences()
                .GetAllObjects<ALAppTable>(x => x.Tables)
                .FindFirst(tableReference);
            if (table == null)
                return null;

            //find field name
            ALMemberAccessExpression memberAccessExpression = ALSyntaxHelper.DecodeMemberAccessExpression(fieldExpression);
            string fieldName = memberAccessExpression.GetSourceFieldNameWithoutRec();
            if (String.IsNullOrWhiteSpace(fieldName))
                return null;

            //collect table fields tooltips
            ALObjectIdentifier[] tableIdentifiers = { table.GetIdentifier() };
            string fieldKey = fieldName.ToLower();           
            Dictionary<int, Dictionary<string, List<LabelInformation>>> allToolTips = this.CollectTableFieldsToolTips(project, tableIdentifiers, null);
            if ((allToolTips.ContainsKey(table.Id)) && (allToolTips[table.Id].ContainsKey(fieldKey)))
                return allToolTips[table.Id][fieldKey];

            return null;
        }

        public Dictionary<int, Dictionary<string, List<LabelInformation>>> CollectProjectTableFieldsToolTips(ALProject project, IEnumerable<string> dependenciesList)
        {
            return this.CollectTableFieldsToolTips(project, GetProjectTablesIdentifiers(project), dependenciesList);
        }


        //!!! TO-DO !!!
        //!!! identify objects by id or by full name with namespace !!!
        public Dictionary<int, Dictionary<string, List<LabelInformation>>> CollectTableFieldsToolTips(ALProject project, IEnumerable<ALObjectIdentifier> tableIdentifiersList, IEnumerable<string> dependenciesList)
        {
            HashSet<string> dependenciesHashSet = ((dependenciesList != null) && (!dependenciesList.Contains("*")))? dependenciesList.ToHashSet(true) : null;
            Dictionary<int, IntPageWithControlsWithLabelPropertyValue> pagesCacheDictionary = new Dictionary<int, IntPageWithControlsWithLabelPropertyValue>();
            List<ALObjectIdentifier> pagesIdentifiersCache = new List<ALObjectIdentifier>();

            //collect pages with controls
            IEnumerable<ALAppPage> alAppPagesCollection = project
                .GetAllSymbolReferences()
                .FilterByName(dependenciesHashSet)
                .GetAllObjects<ALAppPage>(x => x.Pages);

            foreach (ALAppPage alAppPage in alAppPagesCollection)
            {
                var tableReference = alAppPage.GetSourceTable();
                if ((!tableReference.IsEmpty()) && (tableIdentifiersList.Any(p => p.IsReferencedBy(tableReference))) && (!pagesCacheDictionary.ContainsKey(alAppPage.Id)))
                {
                    var tableIdentifier = tableIdentifiersList.Where(p => p.IsReferencedBy(tableReference)).FirstOrDefault();
                    pagesCacheDictionary.Add(alAppPage.Id, new IntPageWithControlsWithLabelPropertyValue(alAppPage, tableIdentifier, "ToolTip"));
                    pagesIdentifiersCache.Add(alAppPage.GetIdentifier());
                }
            }

            //apply extensions
            IEnumerable<(ALObjectIdentifier, ALAppPageExtension)> alAppPageExtensionsCollection = project
                .GetAllSymbolReferences()
                .FilterByName(dependenciesHashSet)
                .GetAllObjects<ALAppPageExtension>(x => x.PageExtensions)
                .FindAllExtensions(pagesIdentifiersCache);

            foreach ((var pageIdentifier, var alAppPageExtension) in alAppPageExtensionsCollection)
                pagesCacheDictionary[pageIdentifier.Id].ApplyPageExtension(alAppPageExtension);

            //collect property values
            Dictionary<int, Dictionary<string, List<LabelInformation>>> tablesPropertyValuesDictionary = new Dictionary<int, Dictionary<string, List<LabelInformation>>>();
            foreach (IntPageWithControlsWithLabelPropertyValue pageCache in pagesCacheDictionary.Values)
            {
                var tableIdentifier = pageCache.SourceTableIdentifier;
                Dictionary<string, List<LabelInformation>> tableFieldsProperties = tablesPropertyValuesDictionary.FindOrCreate(tableIdentifier.Id);

                //add fields
                foreach (IntPageControlWithLabelPropertyValue controlCache in pageCache.Controls.Values)
                {
                    if (!String.IsNullOrWhiteSpace(controlCache.PropertyValue?.Value))
                    {
                        //get field name
                        string fieldName = null;
                        ALMemberAccessExpression memberAccessExpression = ALSyntaxHelper.DecodeMemberAccessExpression(controlCache.ControlSource);
                        fieldName = memberAccessExpression.GetSourceFieldNameWithoutRec();
                        //collect field name
                        if (!String.IsNullOrWhiteSpace(fieldName))
                        {
                            fieldName = fieldName.ToLower();
                            List<LabelInformation> propertyValues = tableFieldsProperties.FindOrCreate(fieldName);
                            if (!propertyValues.ContainsValue(controlCache.PropertyValue.Value))
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

