using AnZwDev.ALTools.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.Tests
{
    public static class LanguageServerHostFactory
    {

        public static LanguageServerHost CreateLanguageServerHost()
        {
            var languageServerHost = new LanguageServerHost(TestsSettings.BCCompilerPath);
            return languageServerHost;
        }

    }
}
