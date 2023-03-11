using Microsoft.Extensions.FileSystemGlobbing;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.Core
{
    public class ExcludedFilesMatcher
    {

        private Matcher _matcher;

        public ExcludedFilesMatcher(List<string> excludedFiles)
        {
            _matcher = null;
            if ((excludedFiles != null) && (excludedFiles.Count > 0))
            {
                _matcher = new Microsoft.Extensions.FileSystemGlobbing.Matcher(StringComparison.CurrentCultureIgnoreCase);
                _matcher.AddInclude("**/*.al");
                for (int i = 0; i < excludedFiles.Count; i++)
                    if (!String.IsNullOrWhiteSpace(excludedFiles[i]))
                        _matcher.AddExclude(excludedFiles[i]);
            }
        }

        public bool ValidFile(string basePath, string filePath)
        {
            if (_matcher != null)
            {
                var matchResult = _matcher.Match(basePath, filePath);
                return ((matchResult != null) && (matchResult.HasMatches));
            }
            return true;
        }

    }
}
