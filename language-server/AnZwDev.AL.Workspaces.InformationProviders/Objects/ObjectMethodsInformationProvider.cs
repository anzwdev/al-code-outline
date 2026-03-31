using AnZwDev.AL.Symbols;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.AL.Workspaces.InformationProviders.Objects
{
    public class ObjectMethodsInformationProvider
    {

        public static IEnumerable<MethodSymbol> GetObjectMethods(Project project, ObjectIdentifier objectIdentifier, bool includePrivate)
        {
            return GetObjectMethods(project, project.Symbols.AllObjects.FindFirst(objectIdentifier), includePrivate);
        }

        public static IEnumerable<MethodSymbol> GetObjectMethods(Project project, ObjectReference objectReference, bool includePrivate)
        {
            return GetObjectMethods(project, project.Symbols.AllObjects.FindFirst(objectReference), includePrivate);
        }

        public static IEnumerable<MethodSymbol> GetObjectMethods(Project project, ObjectSymbol? objectSymbol, bool includePrivate)
        {
            if (objectSymbol != null)
            {
                List<MethodSymbol>? mainMethods = null;

                switch (objectSymbol)
                {
                    case ObjectWithCodeSymbol objectWithCodeSymbol:
                        mainMethods = objectWithCodeSymbol.Methods;
                        break;
                    case InterfaceSymbol interfaceSymbol:
                        mainMethods = interfaceSymbol.Methods;
                        break;
                }

                if (mainMethods != null)
                    for (int i = 0; i < mainMethods.Count; i++)
                        if ((includePrivate) || (mainMethods[i].IsPublic()))
                            yield return mainMethods[i];

                switch (objectSymbol)
                {
                    case TableSymbol:
                        var tableExtensionsEnumerable = project.Symbols.TableExtensions.FindExtensions(objectSymbol.Identifier);
                        if (tableExtensionsEnumerable != null)
                            foreach (var extension in tableExtensionsEnumerable)
                                if (extension.Methods != null)
                                    for (int i = 0; i < extension.Methods.Count; i++)
                                        if ((includePrivate) || (extension.Methods[i].IsPublic()))
                                            yield return extension.Methods[i];
                        break;

                    case PageSymbol:
                        var pageExtensionsEnumerable = project.Symbols.PageExtensions.FindExtensions(objectSymbol.Identifier);
                        if (pageExtensionsEnumerable != null)
                            foreach (var extension in pageExtensionsEnumerable)
                                if (extension.Methods != null)
                                    for (int i = 0; i < extension.Methods.Count; i++)
                                        if ((includePrivate) || (extension.Methods[i].IsPublic()))
                                            yield return extension.Methods[i];
                        break;

                    case ReportSymbol:
                        var reportExtensionsEnumerable = project.Symbols.ReportExtensions.FindExtensions(objectSymbol.Identifier);
                        if (reportExtensionsEnumerable != null)
                            foreach (var extension in reportExtensionsEnumerable)
                                if (extension.Methods != null)
                                    for (int i = 0; i < extension.Methods.Count; i++)
                                        if ((includePrivate) || (extension.Methods[i].IsPublic()))
                                            yield return extension.Methods[i];
                        break;
                }

            }

        }


    }
}
