using AnZwDev.ALTools.ALSymbols;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.Tests.SymbolInformation
{
    public class SymbolReferencesTests
    {

        [Theory]
        //test for single BC18 project
        //  table from dependencies
        [InlineData(TestsSettings.BC18TestProject01Main, TestsSettings.BC18TestProject01Main, null, "51000", true)]             
        [InlineData(TestsSettings.BC18TestProject01Main, TestsSettings.BC18TestProject01Main, null, "\"Dep1 Table 1\"", true)]
        //  main table
        [InlineData(TestsSettings.BC18TestProject01Main, TestsSettings.BC18TestProject01Main, null, "50300", true)]
        [InlineData(TestsSettings.BC18TestProject01Main, TestsSettings.BC18TestProject01Main, null, "\"TableWithoutCaptions\"", true)]
        //test for single BC23 project without namespaces and dependencies in app files
        //  table from dependencies
        [InlineData(TestsSettings.BC24TestProject01Main, TestsSettings.BC24TestProject01Main, null, "51000", true)]
        [InlineData(TestsSettings.BC24TestProject01Main, TestsSettings.BC24TestProject01Main, null, "\"Dep1 Table 1\"", true)]
        //  main table
        [InlineData(TestsSettings.BC24TestProject01Main, TestsSettings.BC24TestProject01Main, null, "50300", true)]
        [InlineData(TestsSettings.BC24TestProject01Main, TestsSettings.BC24TestProject01Main, null, "\"TableWithoutCaptions\"", true)]
        //test for single BC23 project with namespaces and dependencies in app files
        //  table from dependencies
        [InlineData(TestsSettings.BC24TestProject02Main, TestsSettings.BC24TestProject02Main, null, "51000", true)]
        [InlineData(TestsSettings.BC24TestProject02Main, TestsSettings.BC24TestProject02Main, null, "\"Dep1 Table 1\"", true)]
        [InlineData(TestsSettings.BC24TestProject02Main, TestsSettings.BC24TestProject02Main, new string[] { "Random.Namespace.Name" }, "\"Dep1 Table 1\"", false)]
        [InlineData(TestsSettings.BC24TestProject02Main, TestsSettings.BC24TestProject02Main, null, "Dependency1.\"Dep1 Table 1\"", true)]
        [InlineData(TestsSettings.BC24TestProject02Main, TestsSettings.BC24TestProject02Main, new string[] { "Random.Namespace.Name" }, "Dependency1.\"Dep1 Table 1\"", true)]
        [InlineData(TestsSettings.BC24TestProject02Main, TestsSettings.BC24TestProject02Main, new string[] { "Random.Namespace.Name" }, "Dependency3.\"Dep1 Table 1\"", false)]
        [InlineData(TestsSettings.BC24TestProject02Main, TestsSettings.BC24TestProject02Main, new string[] { "Main", "Dependency1" }, "Dependency1.\"Dep1 Table 1\"", true)]
        [InlineData(TestsSettings.BC24TestProject02Main, TestsSettings.BC24TestProject02Main, new string[] { "Main", "Dependency1" }, "Dependency3.\"Dep1 Table 1\"", false)]
        [InlineData(TestsSettings.BC24TestProject02Main, TestsSettings.BC24TestProject02Main, new string[] { "Main", "Dependency1"}, "\"Dep1 Table 1\"", true)]
        //  main table
        [InlineData(TestsSettings.BC24TestProject02Main, TestsSettings.BC24TestProject02Main, null, "50300", true)]
        [InlineData(TestsSettings.BC24TestProject02Main, TestsSettings.BC24TestProject02Main, null, "\"TableWithoutCaptions\"", true)]
        [InlineData(TestsSettings.BC24TestProject02Main, TestsSettings.BC24TestProject02Main, new string[] { "Random.Namespace.Name" }, "\"TableWithoutCaptions\"", false)]
        [InlineData(TestsSettings.BC24TestProject02Main, TestsSettings.BC24TestProject02Main, new string[] { "Random.Namespace.Name" }, "TableWithoutCaptions", false)]
        [InlineData(TestsSettings.BC24TestProject02Main, TestsSettings.BC24TestProject02Main, null, "MainProject.TableWithoutCaptions", true)]
        [InlineData(TestsSettings.BC24TestProject02Main, TestsSettings.BC24TestProject02Main, null, "MainProject.\"TableWithoutCaptions\"", true)]
        [InlineData(TestsSettings.BC24TestProject02Main, TestsSettings.BC24TestProject02Main, new string[] { "Random.Namespace.Name" }, "MainProject.TableWithoutCaptions", true)]
        [InlineData(TestsSettings.BC24TestProject02Main, TestsSettings.BC24TestProject02Main, new string[] { "Random.Namespace.Name" }, "MainProject.\"TableWithoutCaptions\"", true)]
        [InlineData(TestsSettings.BC24TestProject02Main, TestsSettings.BC24TestProject02Main, new string[] { "Random.Namespace.Name" }, "Dependency1.TableWithoutCaptions", false)]
        [InlineData(TestsSettings.BC24TestProject02Main, TestsSettings.BC24TestProject02Main, new string[] { "Random.Namespace.Name" }, "Dependency1.\"TableWithoutCaptions\"", false)]
        [InlineData(TestsSettings.BC24TestProject02Main, TestsSettings.BC24TestProject02Main, new string[] { "Main", "MainProject" }, "MainProject.TableWithoutCaptions", true)]
        [InlineData(TestsSettings.BC24TestProject02Main, TestsSettings.BC24TestProject02Main, new string[] { "Main", "MainProject" }, "MainProject.\"TableWithoutCaptions\"", true)]
        [InlineData(TestsSettings.BC24TestProject02Main, TestsSettings.BC24TestProject02Main, new string[] { "Main", "MainProject" }, "TableWithoutCaptions", true)]
        [InlineData(TestsSettings.BC24TestProject02Main, TestsSettings.BC24TestProject02Main, new string[] { "Main", "MainProject" }, "\"TableWithoutCaptions\"", true)]
        public void FindTableByReference(string workspacePath, string projectPath, string[]? usings, string tableName, bool successExpected)
        {
            workspacePath = Path.Join(TestsSettings.InDataPath, workspacePath);
            projectPath = Path.Join(TestsSettings.InDataPath, projectPath);

            var languageServerHost = WorkspaceCache.Instance.LoadWorkspace(workspacePath);
            var project = languageServerHost.ALDevToolsServer.Workspace.FindProject(projectPath);

            HashSet<string>? usingsHashSet = null;
            if (usings != null)
                usingsHashSet = usings.ToHashSet();

            var objectReference = new ALObjectReference(usingsHashSet, tableName);
            var objectSymbol = project.SymbolsWithDependencies.Tables.FindFirst(objectReference);

            AssertTableFound(objectSymbol != null, tableName, successExpected);
        }

        protected void AssertTableFound(bool found, string tableName, bool successExpected)
        {
            if (successExpected)
                Assert.True(
                    found,
                    $"Table '{tableName}' not found");
            else
                Assert.False(
                    found,
                    $"Table '{tableName}' found");
        }



    }
}
