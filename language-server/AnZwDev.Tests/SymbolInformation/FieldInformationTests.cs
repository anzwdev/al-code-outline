using AnZwDev.ALTools.Workspace.SymbolsInformation;
using AnZwDev.ALTools.ALSymbols;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.Tests.SymbolInformation
{
    public class FieldInformationTests : ProjectTest
    {

        [Theory]
        //test for single BC18 project and dependencies in app files
        [InlineData(TestsSettings.BC18TestProject01Main, TestsSettings.BC18TestProject01Main, "\"Dep1 Table 1\"", "No.", true)]
        [InlineData(TestsSettings.BC18TestProject01Main, TestsSettings.BC18TestProject01Main, "\"Dep1 Table 1\"", "Dep2 Field1", true)]
        [InlineData(TestsSettings.BC18TestProject01Main, TestsSettings.BC18TestProject01Main, "\"Dep1 Table 1\"", "MainApp Field1", true)]
        //test for single BC24 project and dependencies in app files
        [InlineData(TestsSettings.BC24TestProject01Main, TestsSettings.BC24TestProject01Main, "\"Dep1 Table 1\"", "No.", true)]
        [InlineData(TestsSettings.BC24TestProject01Main, TestsSettings.BC24TestProject01Main, "\"Dep1 Table 1\"", "Dep2 Field1", true)]
        [InlineData(TestsSettings.BC24TestProject01Main, TestsSettings.BC24TestProject01Main, "\"Dep1 Table 1\"", "MainApp Field1", true)]
        [InlineData(TestsSettings.BC24TestProject01Main, TestsSettings.BC24TestProject01Main, "\"Dep1 Table 1\"", "Dep2 Field2", true)]
        [InlineData(TestsSettings.BC24TestProject01Main, TestsSettings.BC24TestProject01Main, "\"Dep1 Table 1\"", "MainApp Field2", true)]
        //test for BC24 workspace with 3 projects
        [InlineData(TestsSettings.BC24TestProject01Folder, TestsSettings.BC24TestProject01Main, "\"Dep1 Table 1\"", "No.", true)]
        [InlineData(TestsSettings.BC24TestProject01Folder, TestsSettings.BC24TestProject01Main, "\"Dep1 Table 1\"", "Dep2 Field1", true)]
        [InlineData(TestsSettings.BC24TestProject01Folder, TestsSettings.BC24TestProject01Main, "\"Dep1 Table 1\"", "MainApp Field1", true)]
        [InlineData(TestsSettings.BC24TestProject01Folder, TestsSettings.BC24TestProject01Main, "\"Dep1 Table 1\"", "Dep2 Field2", true)]
        [InlineData(TestsSettings.BC24TestProject01Folder, TestsSettings.BC24TestProject01Main, "\"Dep1 Table 1\"", "MainApp Field2", true)]
        //test for single BC24 project with namespaces and dependencies in app files
        [InlineData(TestsSettings.BC24TestProject02Main, TestsSettings.BC24TestProject02Main, "\"Dep1 Table 1\"", "No.", true)]
        [InlineData(TestsSettings.BC24TestProject02Main, TestsSettings.BC24TestProject02Main, "\"Dep1 Table 1\"", "Dep2 Field1", true)]
        [InlineData(TestsSettings.BC24TestProject02Main, TestsSettings.BC24TestProject02Main, "\"Dep1 Table 1\"", "MainApp Field1", true)]
        [InlineData(TestsSettings.BC24TestProject02Main, TestsSettings.BC24TestProject02Main, "Dependency1.\"Dep1 Table 1\"", "No.", true)]
        [InlineData(TestsSettings.BC24TestProject02Main, TestsSettings.BC24TestProject02Main, "Dependency1.\"Dep1 Table 1\"", "Dep2 Field1", true)]
        [InlineData(TestsSettings.BC24TestProject02Main, TestsSettings.BC24TestProject02Main, "Dependency1.\"Dep1 Table 1\"", "MainApp Field1", true)]
        [InlineData(TestsSettings.BC24TestProject02Main, TestsSettings.BC24TestProject02Main, "Dependency1.\"Dep1 Table 1\"", "Dep2 Field2", true)]
        [InlineData(TestsSettings.BC24TestProject02Main, TestsSettings.BC24TestProject02Main, "Dependency1.\"Dep1 Table 1\"", "MainApp Field2", true)]
        //test for BC24 workspace with namespaces and 3 projects
        [InlineData(TestsSettings.BC24TestProject02Folder, TestsSettings.BC24TestProject02Main, "\"Dep1 Table 1\"", "No.", true)]
        [InlineData(TestsSettings.BC24TestProject02Folder, TestsSettings.BC24TestProject02Main, "\"Dep1 Table 1\"", "Dep2 Field1", true)]
        [InlineData(TestsSettings.BC24TestProject02Folder, TestsSettings.BC24TestProject02Main, "\"Dep1 Table 1\"", "MainApp Field1", true)]
        [InlineData(TestsSettings.BC24TestProject02Folder, TestsSettings.BC24TestProject02Main, "Dependency1.\"Dep1 Table 1\"", "No.", true)]
        [InlineData(TestsSettings.BC24TestProject02Folder, TestsSettings.BC24TestProject02Main, "Dependency1.\"Dep1 Table 1\"", "Dep2 Field1", true)]
        [InlineData(TestsSettings.BC24TestProject02Folder, TestsSettings.BC24TestProject02Main, "Dependency1.\"Dep1 Table 1\"", "MainApp Field1", true)]
        [InlineData(TestsSettings.BC24TestProject02Folder, TestsSettings.BC24TestProject02Main, "Dependency2.\"Dep1 Table 1\"", "No.", false)]
        [InlineData(TestsSettings.BC24TestProject02Folder, TestsSettings.BC24TestProject02Main, "Dependency2.\"Dep1 Table 1\"", "Dep2 Field1", false)]
        [InlineData(TestsSettings.BC24TestProject02Folder, TestsSettings.BC24TestProject02Main, "Dependency2.\"Dep1 Table 1\"", "MainApp Field1", false)]
        public void TableInformationGetFields(string workspacePath, string projectPath, string tableName, string fieldName, bool successExpected)
        {
            workspacePath = Path.Join(TestsSettings.InDataPath, workspacePath);
            projectPath = Path.Join(TestsSettings.InDataPath, projectPath);

            var languageServerHost = WorkspaceCache.Instance.LoadWorkspace(workspacePath);
            var project = languageServerHost.ALDevToolsServer.Workspace.FindProject(projectPath);

            TableInformationProvider tableInformationProvider = new TableInformationProvider();
            var tableFieldInformation = tableInformationProvider.GetTableFields(
                project,
                new ALObjectReference(null, tableName),
                true,
                true,
                true,
                true,
                true,
                false,
                null);

            //check if we can find all required fields
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

        private void AssertField(List<TableFieldInformaton> tableFields, string fieldName)
        {
        }

        //[Fact]
        [Theory]
        //test for single BC18 project and dependencies in app files
        [InlineData(TestsSettings.BC18TestProject01Main, TestsSettings.BC18TestProject01Main, "\"Dep1 Table 1\"", "No.", "No. field list tooltip")]
        [InlineData(TestsSettings.BC18TestProject01Main, TestsSettings.BC18TestProject01Main, "\"Dep1 Table 1\"", "No.", "No. field card tooltip")]
        [InlineData(TestsSettings.BC18TestProject01Main, TestsSettings.BC18TestProject01Main, "\"Dep1 Table 1\"", "No.", "No. field list Dep2 tooltip")]
        [InlineData(TestsSettings.BC18TestProject01Main, TestsSettings.BC18TestProject01Main, "\"Dep1 Table 1\"", "No.", "No. field MainApp list tooltip")]
        [InlineData(TestsSettings.BC18TestProject01Main, TestsSettings.BC18TestProject01Main, "\"Dep1 Table 1\"", "Name 3", "Dep2 PageExt Name 3 field tooltip")]
        [InlineData(TestsSettings.BC18TestProject01Main, TestsSettings.BC18TestProject01Main, "\"Dep1 Table 1\"", "Dep2 Field1", "MainApp PageExt Dep2 Fiel1 field tooltip")]
        //test for single BC23 project and dependencies in app files
        [InlineData(TestsSettings.BC24TestProject01Main, TestsSettings.BC24TestProject01Main, "\"Dep1 Table 1\"", "No.", "No. field list tooltip")]
        [InlineData(TestsSettings.BC24TestProject01Main, TestsSettings.BC24TestProject01Main, "\"Dep1 Table 1\"", "No.", "No. field card tooltip")]
        [InlineData(TestsSettings.BC24TestProject01Main, TestsSettings.BC24TestProject01Main, "\"Dep1 Table 1\"", "No.", "No. field list Dep2 tooltip")]
        [InlineData(TestsSettings.BC24TestProject01Main, TestsSettings.BC24TestProject01Main, "\"Dep1 Table 1\"", "No.", "No. field MainApp list tooltip")]
        [InlineData(TestsSettings.BC24TestProject01Main, TestsSettings.BC24TestProject01Main, "\"Dep1 Table 1\"", "Name 3", "Dep2 PageExt Name 3 field tooltip")]
        [InlineData(TestsSettings.BC24TestProject01Main, TestsSettings.BC24TestProject01Main, "\"Dep1 Table 1\"", "Dep2 Field1", "MainApp PageExt Dep2 Fiel1 field tooltip")]
        //test for BC23 workspace with 3 projects
        [InlineData(TestsSettings.BC24TestProject01Folder, TestsSettings.BC24TestProject01Main, "\"Dep1 Table 1\"", "No.", "No. field list tooltip")]
        [InlineData(TestsSettings.BC24TestProject01Folder, TestsSettings.BC24TestProject01Main, "\"Dep1 Table 1\"", "No.", "No. field card tooltip")]
        [InlineData(TestsSettings.BC24TestProject01Folder, TestsSettings.BC24TestProject01Main, "\"Dep1 Table 1\"", "No.", "No. field list Dep2 tooltip")]
        [InlineData(TestsSettings.BC24TestProject01Folder, TestsSettings.BC24TestProject01Main, "\"Dep1 Table 1\"", "No.", "No. field MainApp list tooltip")]
        [InlineData(TestsSettings.BC24TestProject01Folder, TestsSettings.BC24TestProject01Main, "\"Dep1 Table 1\"", "Name 3", "Dep2 PageExt Name 3 field tooltip")]
        [InlineData(TestsSettings.BC24TestProject01Folder, TestsSettings.BC24TestProject01Main, "\"Dep1 Table 1\"", "Dep2 Field1", "MainApp PageExt Dep2 Fiel1 field tooltip")]
        //test for single BC23 project with namespaces and dependencies in app files
        [InlineData(TestsSettings.BC24TestProject02Main, TestsSettings.BC24TestProject02Main, "\"Dep1 Table 1\"", "No.", "No. field list tooltip")]
        [InlineData(TestsSettings.BC24TestProject02Main, TestsSettings.BC24TestProject02Main, "\"Dep1 Table 1\"", "No.", "No. field card tooltip")]
        [InlineData(TestsSettings.BC24TestProject02Main, TestsSettings.BC24TestProject02Main, "\"Dep1 Table 1\"", "No.", "No. field list Dep2 tooltip")]
        [InlineData(TestsSettings.BC24TestProject02Main, TestsSettings.BC24TestProject02Main, "\"Dep1 Table 1\"", "No.", "No. field MainApp list tooltip")]
        [InlineData(TestsSettings.BC24TestProject02Main, TestsSettings.BC24TestProject02Main, "\"Dep1 Table 1\"", "Name 3", "Dep2 PageExt Name 3 field tooltip")]
        [InlineData(TestsSettings.BC24TestProject02Main, TestsSettings.BC24TestProject02Main, "\"Dep1 Table 1\"", "Dep2 Field1", "MainApp PageExt Dep2 Fiel1 field tooltip")]
        //test for BC23 workspace with namespaces and 3 projects
        [InlineData(TestsSettings.BC24TestProject02Folder, TestsSettings.BC24TestProject02Main, "\"Dep1 Table 1\"", "No.", "No. field list tooltip")]
        [InlineData(TestsSettings.BC24TestProject02Folder, TestsSettings.BC24TestProject02Main, "\"Dep1 Table 1\"", "No.", "No. field card tooltip")]
        [InlineData(TestsSettings.BC24TestProject02Folder, TestsSettings.BC24TestProject02Main, "\"Dep1 Table 1\"", "No.", "No. field list Dep2 tooltip")]
        [InlineData(TestsSettings.BC24TestProject02Folder, TestsSettings.BC24TestProject02Main, "\"Dep1 Table 1\"", "No.", "No. field MainApp list tooltip")]
        [InlineData(TestsSettings.BC24TestProject02Folder, TestsSettings.BC24TestProject02Main, "\"Dep1 Table 1\"", "Name 3", "Dep2 PageExt Name 3 field tooltip")]
        [InlineData(TestsSettings.BC24TestProject02Folder, TestsSettings.BC24TestProject02Main, "\"Dep1 Table 1\"", "Dep2 Field1", "MainApp PageExt Dep2 Fiel1 field tooltip")]
        public void TableInformationGetFieldsToolTips(string workspacePath, string projectPath, string tableName, string fieldName, string toolTip)
        {
            workspacePath = Path.Join(TestsSettings.InDataPath, workspacePath);
            projectPath = Path.Join(TestsSettings.InDataPath, projectPath);

            var languageServerHost = WorkspaceCache.Instance.LoadWorkspace(workspacePath);
            var project = languageServerHost.ALDevToolsServer.Workspace.FindProject(projectPath);

            TableInformationProvider tableInformationProvider = new TableInformationProvider();
            var tableFieldInformation = tableInformationProvider.GetTableFields(
                project,
                new ALObjectReference(null, tableName),
                true,
                true,
                true,
                true,
                true,
                true,
                null);
            AssertToolTip(tableFieldInformation, fieldName, toolTip);
        }

        private void AssertToolTip(List<TableFieldInformaton> tableFields, string fieldName, string toolTip)
        {
            var field = tableFields.Where(p => (p.Name == fieldName)).FirstOrDefault();
            Assert.True(field != null, $"Field {fieldName} not found.");
            var toolTipExists = field.ToolTips.Where(p => (p.Value == toolTip)).Any();
            Assert.True(toolTipExists, $"Field '{fieldName}' tooltip '{toolTip}' not found");
        }

    }
}
