using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.Tests.WorkspaceCommands
{
    public class RemoveWithCommandTest : WorkspaceCommandTest
    {

        [Theory]
        [InlineData(TestsSettings.BC24TestProject01Main, "src\\commandtests\\RemoveWithPage1.Page.al", TestsSettings.BC24TestProject01TestResults, "RemoveWith_RemoveWithPage1.Page.al")]
        [InlineData(TestsSettings.BC24TestProject01Main, "src\\commandtests\\RemoveWithCodeunit1.Codeunit.al", TestsSettings.BC24TestProject01TestResults, "RemoveWith_RemoveWithCodeunit1.Codeunit.al")]
        public void TestRemoveWith(string workspacePath, string testFilePath, string outputFolderPath, string expectedTestResultsPath)
        {
            workspacePath = Path.Join(TestsSettings.InDataPath, workspacePath);
            outputFolderPath = Path.Join(TestsSettings.OutDataPath, outputFolderPath);

            testFilePath = Path.Join(workspacePath, testFilePath);
            expectedTestResultsPath = Path.Join(outputFolderPath, expectedTestResultsPath);
            var parameters = new Dictionary<string, string>();

            RunWorkspaceCommandTest(workspacePath, "removeWith", testFilePath, expectedTestResultsPath, parameters);
        }

    }
}
