using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.AL.Symbols
{
    public static class SymbolsFacts
    {

        public static readonly string MicrosoftAppPublisher = "Microsoft";

        public static readonly string MicrosoftApplicationAppName = "Application";
        public static readonly string MicrosoftSystemAppName = "System";
        public static readonly string MicrosoftTestAppName = "Test";

        public static bool IsMicrosoftApp(string? id, string? name, string? publisher)
        {
            return (publisher != null) && (publisher.Equals(MicrosoftAppPublisher, StringComparison.OrdinalIgnoreCase));
        }

        public static string? GetMicrosoftAppAltId(string? id, string? name, string? publisher)
        {
            if ((IsMicrosoftApp(id, name, publisher)) && (!String.IsNullOrWhiteSpace(name)))
                return publisher + "_" + name;
            return null;
        }

    }
}
