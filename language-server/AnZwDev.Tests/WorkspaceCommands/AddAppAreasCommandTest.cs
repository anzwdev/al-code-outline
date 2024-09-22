using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.Tests.WorkspaceCommands
{
    
    public class AddAppAreasCommandTest : WorkspaceCommandTest
    {

        [Theory]
        [InlineData(TestsSettings.BC24TestProject01Main, "src\\commandtests\\AppAreasPage1.Page.al", TestsSettings.BC24TestProject01TestResults, "AppAreas_AppAreasPage1_1.Page.al", "All", "addToAllControls", false)]
        [InlineData(TestsSettings.BC24TestProject01Main, "src\\commandtests\\AppAreasPage2.Page.al", TestsSettings.BC24TestProject01TestResults, "AppAreas_AppAreasPage2_1.Page.al", "All", "addToAllControls", false)]
        [InlineData(TestsSettings.BC24TestProject01Main, "src\\commandtests\\AppAreasPage3.Page.al", TestsSettings.BC24TestProject01TestResults, "AppAreas_AppAreasPage3_1.Page.al", "All", "addToAllControls", false)]
        [InlineData(TestsSettings.BC24TestProject01Main, "src\\commandtests\\AppAreasPage1.Page.al", TestsSettings.BC24TestProject01TestResults, "AppAreas_AppAreasPage1_2.Page.al", "All", "inheritFromMainObject", false)]
        [InlineData(TestsSettings.BC24TestProject01Main, "src\\commandtests\\AppAreasPage2.Page.al", TestsSettings.BC24TestProject01TestResults, "AppAreas_AppAreasPage2_2.Page.al", "All", "inheritFromMainObject", false)]
        [InlineData(TestsSettings.BC24TestProject01Main, "src\\commandtests\\AppAreasPage3.Page.al", TestsSettings.BC24TestProject01TestResults, "AppAreas_AppAreasPage3_2.Page.al", "All", "inheritFromMainObject", false)]
        //!!! TO-DO !!!
        //!!! Add tests with sorting properties !!!
        public void TestTableCaptions(string workspacePath, string testFilePath, string outputFolderPath, string expectedTestResultsPath, string appArea, string mode, bool sortProperties)
        {
            //mode:
            //inheritFromMainObject = 0,
            //addToAllControls = 1

            workspacePath = Path.Join(TestsSettings.InDataPath, workspacePath);
            outputFolderPath = Path.Join(TestsSettings.OutDataPath, outputFolderPath);

            testFilePath = Path.Join(workspacePath, testFilePath);
            expectedTestResultsPath = Path.Join(outputFolderPath, expectedTestResultsPath);
            var parameters = new Dictionary<string, string>();

            parameters.Add("sortProperties", sortProperties.ToString());
            parameters.Add("appArea", appArea);
            parameters.Add("appAreaMode", mode);

            RunWorkspaceCommandTest(workspacePath, "addAppAreas", testFilePath, expectedTestResultsPath, parameters);
        }

    }

}
