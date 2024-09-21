using AnZwDev.ALTools.Server;
using AnZwDev.ALTools.Workspace;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.Tests
{
    public static class WorkspaceLoader
    {

        public static LanguageServerHost LoadWorkspace(string path)
        {
            List<ALProjectSource> projectSources = new List<ALProjectSource>();

            if (File.Exists(Path.Join(path, "app.json")))
                projectSources.Add(new ALProjectSource(path, null, null, null));
            else
            {
                var folders = Directory.GetDirectories(path);
                foreach (var folder in folders)
                    projectSources.Add(new ALProjectSource(folder, null, null, null));
            }

            var languageServerHost = LanguageServerHostFactory.CreateLanguageServerHost();
            languageServerHost.ALDevToolsServer.Workspace.LoadProjects(projectSources.ToArray());
            languageServerHost.ALDevToolsServer.Workspace.ResolveDependencies();

            return languageServerHost;
        }


    }
}
