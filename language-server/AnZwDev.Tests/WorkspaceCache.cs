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

            var languageServerHost = WorkspaceLoader.LoadWorkspace(path);
            _hostsCache.Add(path, languageServerHost);           

            return languageServerHost;
        }

    }
}
