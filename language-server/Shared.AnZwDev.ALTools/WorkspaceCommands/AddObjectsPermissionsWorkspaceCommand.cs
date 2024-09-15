﻿using AnZwDev.ALTools.CodeTransformations;
using AnZwDev.ALTools.Extensions;
using AnZwDev.ALTools.Workspace;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.WorkspaceCommands
{
    public class AddObjectsPermissionsWorkspaceCommand : SyntaxRewriterWorkspaceCommand<AddObjectsPermissionsSyntaxRewriter>
    {

        public static string ExcludePermissionsFromIncludedPermissionSetsParameterName = "excludeIncludedPermissionSetsPermissions";
        public static string ExcludePermissionsFromExcludedPermissionSetsParameterName = "excludeExcludedPermissionSetsPermissions";

        public AddObjectsPermissionsWorkspaceCommand(ALDevToolsServer alDevToolsServer) : base(alDevToolsServer, "addAllObjectsPermissions", true)
        {
        }

        protected override SyntaxNode FormatSyntaxNode(SyntaxNode node)
        {
            //disable format because default formatter keeps all permissions in a single line
            return node;
        }

        protected override void SetParameters(string sourceCode, ALProject project, string filePath, TextSpan span, Dictionary<string, string> parameters)
        {
            base.SetParameters(sourceCode, project, filePath, span, parameters);
            this.SyntaxRewriter.ExcludePermissionsFromIncludedPermissionSets = parameters.GetBoolValue(ExcludePermissionsFromIncludedPermissionSetsParameterName);
            this.SyntaxRewriter.ExcludePermissionsFromExcludedPermissionSets = parameters.GetBoolValue(ExcludePermissionsFromExcludedPermissionSetsParameterName);
        }


    }
}
