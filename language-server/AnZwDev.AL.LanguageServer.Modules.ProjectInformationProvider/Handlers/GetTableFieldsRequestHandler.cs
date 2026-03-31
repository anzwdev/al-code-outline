using AnZwDev.AL.LanguageServer.Modules.ProjectInformationProvider.Contracts;
using AnZwDev.AL.LanguageServer.Modules.ProjectInformationProvider.Contracts.Symbols;
using AnZwDev.AL.Symbols;
using AnZwDev.AL.Symbols.Formatters;
using AnZwDev.AL.Workspaces;
using AnZwDev.AL.Workspaces.InformationProviders.Objects;
using AnZwDev.AL.Workspaces.InformationProviders.ToolTips;
using AnZwDev.LanguageServer;
using AnZwDev.System.Logging;
using AnZwDev.System.ServiceModel;
using StreamJsonRpc;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.AL.LanguageServer.Modules.ProjectInformationProvider.Handlers
{
    internal class GetTableFieldsRequestHandler : RequestHandler
    {

        public GetTableFieldsRequestHandler(IServiceProvider services) : base(services)
        {
        }

        [JsonRpcMethod("al/projectinformation/gettablefieldslist", UseSingleObjectParameterDeserialization = true)]
        public GetTableFieldsResponse GetTableFieldsList(GetTableFieldsRequest parameters)
        {
            var fieldsList = new List<PITableFieldListItem>();

            if ((parameters.Path != null) && (parameters.TableIdentifier != null))
            {
                try
                {
                    
                    var project = this.Services
                        .GetService<Workspace>()?
                        .Projects.FindByPath(parameters.Path);

                    if ((parameters.TableIdentifier != null) && (project != null))
                    {
                        var objectIdentifier = parameters.TableIdentifier.ToObjectIdentifier();
                        var table = project.Symbols.Tables.FindFirst(objectIdentifier);
                        if (table != null)
                        {
                            HashSet<FieldClass>? fieldClassFilter = null;
                            if((parameters.FieldClassFilter != null) && (parameters.FieldClassFilter.Length > 0))
                                fieldClassFilter = new HashSet<FieldClass>(parameters.FieldClassFilter);

                            var fieldsEnumerable = TableFieldListInformationProvider.GetTableFields(project, table, fieldClassFilter);
                            var tableToolTips = GetTableToolTips(project, table, parameters.IncludeToolTips, parameters.ToolTipsSourceDependencies);

                            AddFields(fieldsList, fieldsEnumerable, tableToolTips);

                            return new GetTableFieldsResponse()
                            {
                                Fields = fieldsList
                            };
                        }
                    }

                }
                catch (Exception e)
                {
                    Services.GetService<ILogger>()?.Log(e);
                }
            }

            return new GetTableFieldsResponse()
            {
                Fields = null,
            };
        }

        private void AddFields(List<PITableFieldListItem> fieldsList, IEnumerable<TableFieldSymbol> fieldsSymbolsList, TableToolTips? tableToolTips)
        {
            foreach (var fieldSymbol in fieldsSymbolsList)
            {
                string? description = null;
                string? caption = null;
                string? captionComment = null;
                if (fieldSymbol.Properties != null)
                {
                    var captionLabel = fieldSymbol.Properties.Caption;
                    caption = captionLabel.Text;
                    captionComment = captionLabel.Comment;
                    description = fieldSymbol.Properties.Description;
                }

                fieldsList.Add(new PITableFieldListItem()
                {
                    Id = fieldSymbol.Id,
                    Name = fieldSymbol.Name,
                    DisplayString = DisplayStringFormatter.FormatTableField(fieldSymbol),
                    Caption = caption ?? String.Empty,
                    CaptionLabel = new PILabel()
                    {
                        Value = caption,
                        Comment = captionComment
                    },
                    Description = description,
                    DataType = (fieldSymbol.TypeDefinition != null) ? DisplayStringFormatter.FormatTypeDefinitionSymbol(fieldSymbol.TypeDefinition) : String.Empty,
                    Class = (fieldSymbol.Properties != null) ? fieldSymbol.Properties.FieldClass : FieldClass.Normal,
                    ToolTips = GetFieldToolTips(fieldSymbol.Name, tableToolTips)
                });
            }
        }

        private TableToolTips? GetTableToolTips(Project project, TableSymbol table, bool includeToolTips, string[]? toolTipsDependenciesSource)
        {
            if (!includeToolTips)
                return null;

            HashSet<string>? pagesAppIdFilter = null;
            if ((toolTipsDependenciesSource != null) && (toolTipsDependenciesSource.Length > 0))
                pagesAppIdFilter = new HashSet<string>(toolTipsDependenciesSource, StringComparer.OrdinalIgnoreCase);

            return ToolTipsInformationProvider.GetTableToolTips(project, table, true, pagesAppIdFilter);
        }

        private List<PILabel>? GetFieldToolTips(string fieldName, TableToolTips? tableToolTips)
        {
            if ((tableToolTips != null) && (tableToolTips.Fields.ContainsKey(fieldName)))
            {
                var field = tableToolTips.Fields[fieldName];
                if (field.ToolTips.Count > 0)
                {
                    var fieldToolTipsLabels = new List<PILabel>();

                    for (var i = 0; i < field.ToolTips.Count; i++)
                    {
                        var toolTip = field.ToolTips[i];
                        fieldToolTipsLabels.Add(new PILabel()
                        {
                            Value = toolTip.Value.Text,
                            Comment = toolTip.Value.Comment
                        });
                    }

                    return fieldToolTipsLabels;
                }

            }
            return null;
        }

    }
}
