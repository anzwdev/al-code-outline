using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.Tests.WorkspaceCommands
{
    public class AddTableFieldCaptionsCommandTest : WorkspaceCommandTest
    {

        [Theory]
        [InlineData(TestsSettings.BC23TestProject01Main, "TableWithoutCaptions.Table.al", TestsSettings.BC23TestProject01TestResults, "AddTableFieldCaptions_TableWithoutCaptions.Table.al")]
        public void TestTableCaptions(string workspacePath, string testFilePath, string outputFolderPath, string expectedTestResultsPath)
        {
            workspacePath = Path.Join(TestsSettings.InDataPath, workspacePath);
            outputFolderPath = Path.Join(TestsSettings.OutDataPath, outputFolderPath);

            testFilePath = Path.Join(workspacePath, testFilePath);
            expectedTestResultsPath = Path.Join(outputFolderPath, expectedTestResultsPath);
            var parameters = new Dictionary<string, string>();

            RunWorkspaceCommandTest(workspacePath, "addFieldCaptions", testFilePath, expectedTestResultsPath, parameters);
        }

    }
}
