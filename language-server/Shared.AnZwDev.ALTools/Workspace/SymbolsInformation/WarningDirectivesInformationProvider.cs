using AnZwDev.ALTools.ALSymbolReferences;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnZwDev.ALTools.Workspace.SymbolsInformation
{
    public class WarningDirectivesInformationProvider
    {

        const string AllRulesId = "All";
        const string AllRulesDescription = "All rules";
        public ALDevToolsServer Server { get; }

        public WarningDirectivesInformationProvider(ALDevToolsServer server)
        {
            Server = server;
        }

        public List<WarningDirectiveInfo> GetWarningDirectives(ALWorkspace workspace)
        {
            Server.CodeAnalyzersLibraries.LoadCodeAnalyzers(workspace);

            List<WarningDirectiveInfo> projectInfoCollection = new List<WarningDirectiveInfo>();
            if (workspace != null)
                for (int i = 0; i < workspace.Projects.Count; i++)
                {
                    var projectInfo = GetWarningDirectives(workspace.Projects[i]);
                    if (projectInfo != null)
                        projectInfoCollection.Add(projectInfo);
                }
            return projectInfoCollection;
        }

        private WarningDirectiveInfo GetWarningDirectives(ALProject project)
        {
            if ((project.Files != null) && (project.Files.Count > 0))
            {
                var rules = new Dictionary<string, WarningDirectiveInfo>();
                for (int i = 0; i < project.Files.Count; i++)
                    CollectDirectives(project.Files[i], rules);
                if (rules.Count > 0)
                    return WarningDirectiveInfo.Create(project, rules.Values.ToList());
            }

            return null;
        }

        private void CollectDirectives(ALProjectFile file, Dictionary<string, WarningDirectiveInfo> rules)
        {
            if (file.Directives != null)
                foreach (var directive in file.Directives)
                    if (directive is ALAppPragmaWarningDirective warningDirective)
                    {
                        if (warningDirective.Rules != null)
                            foreach (var rule in warningDirective.Rules)
                                AddDirective(file, rules, warningDirective, rule);
                        else
                            AddDirective(file, rules, warningDirective, AllRulesId);
                    }


        }

        private void AddDirective(ALProjectFile file, Dictionary<string, WarningDirectiveInfo> rules, ALAppPragmaWarningDirective directive, string ruleId)
        {
            ruleId = ruleId.ToUpper();
            var ruleInfo = FindOrCreateRuleInfo(rules, ruleId);
            var fileInfo = FindOrCreateFileInfo(ruleInfo, file);
            fileInfo.AddChildItem(WarningDirectiveInfo.Create(ruleId, directive.Disabled, directive.Range));
        }

        private WarningDirectiveInfo FindOrCreateRuleInfo(Dictionary<string, WarningDirectiveInfo> rules, string ruleId)
        {
            if (rules.ContainsKey(ruleId))
                return rules[ruleId];
            var info = WarningDirectiveInfo.CreateRule(ruleId, GetRuleDescription(ruleId));
            rules.Add(ruleId, info);
            return info;
        }

        private WarningDirectiveInfo FindOrCreateFileInfo(WarningDirectiveInfo ruleInfo, ALProjectFile file)
        {
            if ((ruleInfo.ChildItems != null) &&
                (ruleInfo.ChildItems.Count > 0) &&
                (ruleInfo.ChildItems[ruleInfo.ChildItems.Count - 1].FullPath == file.FullPath))
                return ruleInfo.ChildItems[ruleInfo.ChildItems.Count - 1];

            var fileInfo = WarningDirectiveInfo.Create(file);
            ruleInfo.AddChildItem(fileInfo);
            return fileInfo;
        }

        private string GetRuleDescription(string ruleId)
        {
            if (ruleId == AllRulesId)
                return AllRulesDescription;

            var rule = Server.CodeAnalyzersLibraries.FindCachedRule(ruleId);

            if (!String.IsNullOrWhiteSpace(rule?.title))
                return rule.title;

            return ruleId;
        }

    }
}
