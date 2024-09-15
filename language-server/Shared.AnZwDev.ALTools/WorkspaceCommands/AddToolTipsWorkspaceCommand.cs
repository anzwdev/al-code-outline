﻿using AnZwDev.ALTools.ALSymbols;
using AnZwDev.ALTools.CodeTransformations;
using AnZwDev.ALTools.Extensions;
using AnZwDev.ALTools.Workspace;
using AnZwDev.ALTools.Workspace.SymbolsInformation;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.WorkspaceCommands
{
    public class AddToolTipsWorkspaceCommand : SyntaxRewriterWorkspaceCommand<ToolTipSyntaxRewriter>
    {

        public static string SortPropertiesParameterName = "sortProperties";
        public static string FieldTooltipParameterName = "toolTipField";
        public static string FieldTooltipCommentParameterName = "toolTipFieldComment";
        public static string ActionTooltipParameterName = "toolTipAction";
        public static string UseFieldDescriptionParameterName = "useFieldDescription";
        public static string ReuseToolTipsParameterName = "reuseToolTips";
        public static string DependencyNameParameterName = "toolTipDependency";
        public static string CreateFieldToolTipsParameterName = "createFieldToolTips";
        public static string CreateActionToolTipsParameterName = "createActionToolTips";

        public AddToolTipsWorkspaceCommand(ALDevToolsServer alDevToolsServer) : base(alDevToolsServer, "addToolTips", true)
        {
        }

        protected override void SetParameters(string sourceCode, ALProject project, string filePath, TextSpan span, Dictionary<string, string> parameters)
        {
            base.SetParameters(sourceCode, project, filePath, span, parameters);
            this.SyntaxRewriter.PageFieldTooltip = parameters.GetStringValue(FieldTooltipParameterName);
            this.SyntaxRewriter.PageActionTooltip = parameters.GetStringValue(ActionTooltipParameterName);
            this.SyntaxRewriter.PageFieldTooltipComment = parameters.GetStringValue(FieldTooltipCommentParameterName);
            this.SyntaxRewriter.UseFieldDescription = parameters.GetBoolValue(UseFieldDescriptionParameterName);
            this.SyntaxRewriter.SortProperties = parameters.GetBoolValue(SortPropertiesParameterName);

            this.SyntaxRewriter.CreateFieldToolTips = parameters.GetBoolValue(CreateFieldToolTipsParameterName);
            this.SyntaxRewriter.CreateActionToolTips = parameters.GetBoolValue(CreateActionToolTipsParameterName);


            bool reuseToolTips = parameters.GetBoolValue(ReuseToolTipsParameterName);

            if ((this.SyntaxRewriter.Project?.Symbols?.Tables != null) && (reuseToolTips))
            {
                List<string> toolTipDependencies = new List<string>();
                foreach (string key in parameters.Keys)
                {
                    if ((key.StartsWith(DependencyNameParameterName)) && (!String.IsNullOrWhiteSpace(parameters[key])))
                        toolTipDependencies.Add(parameters[key]);
                }
                if (toolTipDependencies.Count == 0)
                    toolTipDependencies = null;

                PageInformationProvider provider = new PageInformationProvider();
                this.SyntaxRewriter.ToolTipsCache = provider.CollectProjectTableFieldsToolTips(this.SyntaxRewriter.Project, toolTipDependencies);
            }
        }

        protected override void ClearParameters()
        {
            base.ClearParameters();
            this.SyntaxRewriter.ToolTipsCache = null;
        }

    }
}
