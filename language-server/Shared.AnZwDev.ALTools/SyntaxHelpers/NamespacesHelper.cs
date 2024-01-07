using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.SyntaxHelpers
{
    public static class NamespacesHelper
    {

        public static string GetNamespaceName(string projectPath, string filePath, string rootNamespace, bool useFoldersStructure = true)
        {
            var namespaceName = rootNamespace.Trim();

            if (useFoldersStructure)
            {
                Uri projectUri = new Uri(projectPath);
                Uri fileUri = new Uri(filePath);
                Uri relativeUri = fileUri.MakeRelativeUri(projectUri);

                namespaceName = namespaceName + "." + MakeValidNamespace(
                    relativeUri.ToString()
                        .Replace('/', '.')
                        .Replace('\\', '.'));
            }

            return namespaceName;
        }

        private static string MakeValidNamespace(string namespaceName)
        {
            var firstCharacter = true;
            var outputNamespaceName = "";
            var startPos = 0;

            for (int i=0; i<namespaceName.Length; i++)
            {
                var character = namespaceName[i];
                if (character == '.')
                    firstCharacter = true;
                else if (!ValidNamespacePartCharacter(character, firstCharacter))
                {
                    if (startPos < i)
                        outputNamespaceName = outputNamespaceName + namespaceName.Substring(startPos, i - startPos);
                    startPos = i + 1;
                }
            }
            if (startPos < namespaceName.Length)
                outputNamespaceName = outputNamespaceName + namespaceName.Substring(startPos);

            while (outputNamespaceName.StartsWith("."))
                outputNamespaceName = outputNamespaceName.Substring(1);

            while (outputNamespaceName.EndsWith("."))
                outputNamespaceName = outputNamespaceName.Substring(0, outputNamespaceName.Length - 1);

            return outputNamespaceName;
        }

        private static bool ValidFirstNamespacePartCharacter(char character)
        {
            return
                (character >= 'a' && character <= 'z') ||
                (character >= 'A' && character <= 'Z') ||
                (character == '_');
        }

        private static bool ValidMiddleNamespacePartCharacter(char character)
        {
            return
                ValidFirstNamespacePartCharacter(character) ||
                (character >= '0' && character <= '9');
        }

        private static bool ValidNamespacePartCharacter(char character, bool firstCharacter)
        {
            if (firstCharacter)
                return ValidFirstNamespacePartCharacter(character);
            return ValidMiddleNamespacePartCharacter(character);
        }



    }
}
