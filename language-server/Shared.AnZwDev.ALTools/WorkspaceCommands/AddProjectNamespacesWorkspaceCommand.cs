using AnZwDev.ALTools.ALSymbols;
using AnZwDev.ALTools.CodeTransformations.Namespaces;
using AnZwDev.ALTools.Extensions;
using AnZwDev.ALTools.Workspace;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.WorkspaceCommands
{

#if BC

    public class AddProjectNamespacesWorkspaceCommand : WorkspaceCommand
    {

        public static string RootNamespaceParameterName = "rootNamespace";
        public static string UseFolderStructureParameterName = "useFolderStructure";

        public AddProjectNamespacesWorkspaceCommand(ALDevToolsServer alDevToolsServer) : base(alDevToolsServer, "enableNamespacesSupport")
        {
        }

        public override WorkspaceCommandResult Run(string sourceCode, ALProject project, string filePath, Range range, Dictionary<string, string> parameters, List<string> excludeFiles)
        {
            if (!String.IsNullOrWhiteSpace(filePath))
                return new WorkspaceCommandResult(null, true, "Namespaces support cannot be enabled in single file only.");

            if (String.IsNullOrWhiteSpace(project.RootPath))
                return new WorkspaceCommandResult(null, true, $"Please specify project path for namespaces converter");

            WorkspaceCommandResult result;
            var rootNamespaceName = parameters.GetStringValue(RootNamespaceParameterName);
            var useFolderStructure = parameters.GetBoolValue(UseFolderStructureParameterName);
            var namespacesConverter = new AddProjectNamespacesConverter();

            var namespaceUpdateResult = namespacesConverter.AddNamespacesToProject(project.Workspace, project, rootNamespaceName, useFolderStructure);

            result = new WorkspaceCommandResult(null, !namespaceUpdateResult.Success, namespaceUpdateResult.ErrorMessage);
            result.SetParameter(NoOfChangesParameterName, namespaceUpdateResult.NoOfChanges.ToString());
            result.SetParameter(NoOfChangedFilesParameterName, namespaceUpdateResult.NoOfChangedFiles.ToString());
            return result;
        }

    }

#endif

}
