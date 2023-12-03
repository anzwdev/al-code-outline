using AnZwDev.ALTools.CodeTransformations;
using AnZwDev.ALTools.Extensions;
using AnZwDev.ALTools.Workspace;
using AnZwDev.ALTools.WorkspaceCommands.Parameters;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.WorkspaceCommands
{
    internal class SortProceduresWorkspaceCommand : SyntaxRewriterWorkspaceCommand<SortProceduresSyntaxRewriter>
    {

        public static string TriggersSortModeParameterName = "triggersSortMode";
        public static string TriggersNaturalOrderParameterName = "triggersNaturalOrder";
        public static string SortSingleNodeRegionsParameterName = "sortSingleNodeRegions";

        public SortProceduresWorkspaceCommand(ALDevToolsServer alDevToolsServer) : base(alDevToolsServer, "sortProcedures")
        {
        }

        protected override void SetParameters(string sourceCode, ALProject project, string filePath, TextSpan span, Dictionary<string, string> parameters)
        {
            base.SetParameters(sourceCode, project, filePath, span, parameters);

            if (parameters.ContainsKey(TriggersSortModeParameterName))
                this.SyntaxRewriter.TriggerSortMode = parameters.GetEnumValue(TriggersSortModeParameterName, SortProceduresTriggerSortMode.None);

            if (parameters.ContainsKey(TriggersNaturalOrderParameterName))
            {
                var triggersOrder = parameters.GetJsonValue<TriggersOrderParameter[]>(TriggersNaturalOrderParameterName);
                if (triggersOrder != null)
                    for (int i = 0; i < triggersOrder.Length; i++)
                        this.SyntaxRewriter.TriggersOrder.Add(triggersOrder[i].ToTriggersOrder());
            }

            this.SyntaxRewriter.SortSingleNodeRegions = parameters.GetBoolValue(SortSingleNodeRegionsParameterName);
        }

    }
}
