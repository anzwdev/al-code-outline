using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.Tests.WorkspaceCommands
{
    public class WorkspaceCommandTest : ProjectTest
    {

        protected void RunWorkspaceCommandTest(string workspacePath, string commandName, string inputFilePath, string expectedTestResultsPath, Dictionary<string, string> parameters)
        {
            var host = WorkspaceCache.Instance.LoadWorkspace(workspacePath);
            var content = File.ReadAllText(inputFilePath);
            var project = host.ALDevToolsServer.Workspace.FindProject(inputFilePath);
            var commandResult = host.ALDevToolsServer.WorkspaceCommandsManager.RunCommand(commandName, content, project.RootPath, inputFilePath, null, parameters, null, null);

            if (commandResult.Error)
                Assert.Fail(commandResult.ErrorMessage);

            //empty content == no changes
            if (String.IsNullOrWhiteSpace(commandResult.Source))
                commandResult.Source = content;                

            var expectedContent = File.ReadAllText(expectedTestResultsPath);
            Assert.Equal(expectedContent, commandResult.Source);
        }

    }
}
