using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.AL.Symbols.Metadata
{
    public partial class ApplicationMetadata
    {

        public void AddMissingMicrosoftDependencies()
        {
            AddMissingMicrosoftDependency("", SymbolsFacts.MicrosoftApplicationAppName, SymbolsFacts.MicrosoftAppPublisher, BCApplicationVersion);
            AddMissingMicrosoftDependency("", SymbolsFacts.MicrosoftSystemAppName, SymbolsFacts.MicrosoftAppPublisher, BCPlatformVersion);
            if (BCTestVersion != null)
                AddMissingMicrosoftDependency("", SymbolsFacts.MicrosoftTestAppName, SymbolsFacts.MicrosoftAppPublisher, BCTestVersion);
        }

        private void AddMissingMicrosoftDependency(string id, string name, string publisher, Version version)
        {
            var dependency = (String.IsNullOrWhiteSpace(id)) ?
                Dependencies
                    .Where(p =>
                        (name.Equals(p.Name, StringComparison.OrdinalIgnoreCase)) &&
                        (publisher.Equals(p.Publisher, StringComparison.OrdinalIgnoreCase)))
                    .FirstOrDefault()
                :
                Dependencies
                    .Where(p => (id.Equals(p.Id, StringComparison.OrdinalIgnoreCase)))
                    .FirstOrDefault();

            if (dependency == null)
                Dependencies.Add(new ApplicationDependency()
                {
                    Id = id,
                    Name = name,
                    Publisher = publisher,
                    Version = version,
                });
            else if (dependency.Version < version)
                dependency.Version = version;
        }


    }
}
