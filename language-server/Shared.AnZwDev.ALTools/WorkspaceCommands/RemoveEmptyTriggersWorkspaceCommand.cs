﻿using AnZwDev.ALTools.CodeTransformations;
using AnZwDev.ALTools.Extensions;
using AnZwDev.ALTools.Workspace;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.WorkspaceCommands
{
    public class RemoveEmptyTriggersWorkspaceCommand : SyntaxRewriterWorkspaceCommand<RemoveEmptyTriggersSyntaxRewriter>
    {
        public static string RemoveTriggersParameterName = "removeTriggers";
        public static string RemoveSubscribersParameterName = "removeSubscribers";
        public static string IgnoreCommentsParameterName = "ignoreComments";

        public RemoveEmptyTriggersWorkspaceCommand(ALDevToolsServer alDevToolsServer) : base(alDevToolsServer, "removeEmptyTriggers", false)
        {
        }

        protected override void SetParameters(string sourceCode, ALProject project, string filePath, TextSpan span, Dictionary<string, string> parameters)
        {
            base.SetParameters(sourceCode, project, filePath, span, parameters);
            this.SyntaxRewriter.RemoveTriggers = parameters.GetBoolValue(RemoveTriggersParameterName);
            this.SyntaxRewriter.RemoveSubscribers = parameters.GetBoolValue(RemoveSubscribersParameterName);
            this.SyntaxRewriter.IgnoreComments = parameters.GetBoolValue(IgnoreCommentsParameterName);
        }

    }
}
