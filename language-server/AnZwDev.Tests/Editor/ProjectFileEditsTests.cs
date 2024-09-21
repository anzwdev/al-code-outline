using AnZwDev.ALTools.Workspace;
using AnZwDev.ALTools.Workspace.SymbolsInformation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.Tests.Editor
{
    public class ProjectFileEditsTests
    {

        [Theory]
        [InlineData(TestsSettings.BC23TestEditsProject03Folder, TestsSettings.BC23TestEditsProject03Main, TestsSettings.BC23TestEditsProject03SrcFileTest1, TestsSettings.BC23TestEditsProject03EditTest1, "\"Dep1 Table 1\"", "MainApp Field2")]

        public void TestTableFileEdit(string workspacePath, string projectPath, string filePath, string modifiedFilePath, string tableName, string fieldName)
        {
            workspacePath = Path.Join(TestsSettings.InDataPath, workspacePath);
            projectPath = Path.Join(TestsSettings.InDataPath, projectPath);
            filePath = Path.Join(TestsSettings.InDataPath, filePath);
            modifiedFilePath = Path.Join(TestsSettings.InDataPath, modifiedFilePath);

            var languageServerHost = WorkspaceLoader.LoadWorkspace(workspacePath);
            var project = languageServerHost.ALDevToolsServer.Workspace.FindProject(projectPath);
            var tableInformationProvider = new TableInformationProvider();

            //field should not exist before edit
            CheckFieldExists(project, tableInformationProvider, tableName, fieldName, false);

            //edit file in memory, field should exist
            var newContent = File.ReadAllText(modifiedFilePath);
            languageServerHost.ALDevToolsServer.Workspace.OnDocumentOpen(filePath);
            var root = languageServerHost.ALDevToolsServer.Workspace.OnDocumentChange(filePath, newContent, false);

            //field should not exist yet, modified files need to be rebuild
            CheckFieldExists(project, tableInformationProvider, tableName, fieldName, false);

            //field should exist after modifed files have beed rebuild
            languageServerHost.ALDevToolsServer.Workspace.RebuildModifiedSymbols();
            CheckFieldExists(project, tableInformationProvider, tableName, fieldName, true);

            //cancel edit, field should not exist
            languageServerHost.ALDevToolsServer.Workspace.OnDocumentClose(filePath);
            CheckFieldExists(project, tableInformationProvider, tableName, fieldName, false);
        }

        private void CheckFieldExists(ALProject project, TableInformationProvider tableInformationProvider, string tableName, string fieldName, bool successExpected)
        {
            var tableFieldInformation = tableInformationProvider.GetTableFields(
                project,
                new ALTools.ALSymbols.ALObjectReference(null, tableName),
                true, true, true, true, true, false, null);
            var fieldExists = tableFieldInformation.Where(p => p.Name == fieldName).Any();

            if (successExpected)
                Assert.True(
                    fieldExists,
                    $"Field '{fieldName}' not found in table '{tableName}'");
            else
                Assert.False(
                    fieldExists,
                    $"Field '{fieldName}' found in table '{tableName}'");
        }

    }
}
