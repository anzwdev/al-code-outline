using AnZwDev.AL.Symbols;
using AnZwDev.AL.Syntax;
using AnZwDev.System.IO;

namespace AnZwDev.AL.Workspaces.InformationProviders.Namespaces
{
    public static class NamespaceSuggestionProvider
    {

        public static NamespaceInformation SuggestNamespaceAndUsings(Project project, ObjectIdentifier? objectIdentifier, List<ObjectIdentifier> referencedObjects, string? filePath)
        {
            var namespacesState = NamespaceInformationProvider.GetNamespacesState(project);
            if ((namespacesState != NamespacesState.Recommended) && (namespacesState != NamespacesState.Required))
                return new NamespaceInformation 
                { 
                    Namespace = null, 
                    Usings = null 
                };

            var namespaceName = objectIdentifier?.FullyQualifiedName.Namespace;
            if (String.IsNullOrWhiteSpace(namespaceName))
                namespaceName = SuggestNamespace(project.Settings.RootNamespace, project.RootPath, filePath);
            var usings = SuggestUsings(namespaceName, referencedObjects);

            return new NamespaceInformation 
            { 
                Namespace = namespaceName, 
                Usings = usings 
            };
        }

        public static string? SuggestNamespace(string? rootNamespace, string? projectPath, string? filePath)
        {
            if (String.IsNullOrWhiteSpace(projectPath) || String.IsNullOrWhiteSpace(filePath))
                return rootNamespace;

            // Get the directory containing the file
            var fileDirectory = Path.GetDirectoryName(filePath);
            if (String.IsNullOrWhiteSpace(fileDirectory))
                return rootNamespace;

            projectPath = PathUtils.NormalizePath(projectPath).TrimEnd(Path.DirectorySeparatorChar);
            fileDirectory = PathUtils.NormalizePath(fileDirectory).TrimEnd(Path.DirectorySeparatorChar);

            // Calculate relative path from project to file directory
            var relativePath = Path.GetRelativePath(projectPath, fileDirectory);

            // If file is in project root or relative path goes up (starts with ..), return root namespace
            if (String.IsNullOrWhiteSpace(relativePath) || relativePath == "." || relativePath.StartsWith(".."))
                return rootNamespace;

            // Convert path separators to dots for namespace format
            var fullNamespace = relativePath.Replace(Path.DirectorySeparatorChar, ALLanguageFacts.FullyQualifiedNameSeparatorChar);

            // Combine root namespace with the path-based suffix
            if (!String.IsNullOrWhiteSpace(rootNamespace))
                fullNamespace = rootNamespace + ALLanguageFacts.FullyQualifiedNameSeparatorChar + fullNamespace;

            return ALNamespaceNormalizer.Normalize(fullNamespace);
        }

        public static HashSet<string> SuggestUsings(string? namespaceName, List<ObjectIdentifier> referencedObjects)
        {
            var mainNamespaceDefined = !String.IsNullOrWhiteSpace(namespaceName);
            var usings = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            for (int i = 0; i < referencedObjects.Count; i++)
            {
                var objectNamespace = referencedObjects[i].FullyQualifiedName.Namespace;
                if (
                    (!String.IsNullOrWhiteSpace(objectNamespace)) &&
                    ((!mainNamespaceDefined) || (!objectNamespace.Equals(namespaceName, StringComparison.OrdinalIgnoreCase))) &&
                    (!usings.Contains(objectNamespace))
                )
                    usings.Add(objectNamespace);
            }
            return usings;
        }

    }
}
