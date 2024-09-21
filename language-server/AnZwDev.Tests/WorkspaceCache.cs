using AnZwDev.ALTools.Server;
using AnZwDev.ALTools.Workspace;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.Tests
{
    public class WorkspaceCache
    {

        private static WorkspaceCache? _instance = null;
        public static WorkspaceCache Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new WorkspaceCache();
                return _instance;
            }
        }

        private Dictionary<string, LanguageServerHost> _hostsCache = new Dictionary<string, LanguageServerHost>();

        public WorkspaceCache() 
        { 
        }

        public LanguageServerHost LoadWorkspace(string path)
        {
            if (_hostsCache.ContainsKey(path))
                return _hostsCache[path];

            //_workspacePath = path;

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

            _hostsCache.Add(path, languageServerHost);           

            return languageServerHost;
        }

    }
}
