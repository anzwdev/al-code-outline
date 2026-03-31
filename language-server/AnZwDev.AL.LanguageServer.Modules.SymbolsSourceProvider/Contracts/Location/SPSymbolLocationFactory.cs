using AnZwDev.AL.Symbols;
using AnZwDev.AL.Symbols.Formatters;
using AnZwDev.AL.Workspaces.Symbols;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace AnZwDev.AL.LanguageServer.Modules.SymbolsSourceProvider.Contracts.Location
{
    public static class SPSymbolLocationFactory
    {

        public static SPSymbolLocation? BuildFileLocation(string? filePath)
        {
            if (String.IsNullOrWhiteSpace(filePath))
                return null;

            return new SPSymbolLocation()
            {
                Schema = SPSymbolLocationSchema.File,
                ContainerPath = filePath,
                SourcePath = filePath,
                Range = null
            };
        }

        public static SPSymbolLocation? BuildMSPreviewLocation(ObjectIdentifier objectIdentifier, string? appFilePath)
        {
            var sourcePath =
                ObjectKindFormatter.FormatAsObjectTypeName(objectIdentifier.ObjectKind) + "/" +
                objectIdentifier.Id.ToString() + "/" +
                WebUtility.UrlEncode(objectIdentifier.FullyQualifiedName.Name) + ".dal";

            return new SPSymbolLocation()
            {
                Schema = SPSymbolLocationSchema.ALPreview,
                ContainerPath = appFilePath,
                SourcePath = sourcePath,
                Range = null
            };
        }

        public static SPSymbolLocation? BuildAppContentLocation(string appFilePath, string? filePath)
        {
            if (String.IsNullOrWhiteSpace(filePath))
                return null;

            return new SPSymbolLocation()
            {
                Schema = SPSymbolLocationSchema.ALApp,
                ContainerPath = appFilePath,
                SourcePath = appFilePath + "::" + filePath,
                Range = null
            };
        }

        public static SPSymbolLocation? BuildLocation(ApplicationSymbol? appSymbol, ObjectSymbol? objectSymbol, bool directAppFileAccess)
        {
            if (objectSymbol == null)
                return null;

            if ((appSymbol != null) && (appSymbol.ReferenceSourceFileName != null) && (appSymbol.ReferenceSourceFileName.EndsWith(".app", StringComparison.OrdinalIgnoreCase)))
            {
                if (directAppFileAccess)
                    return BuildAppContentLocation(appSymbol.ReferenceSourceFileName, objectSymbol.ReferenceSourceFileName);
                else
                    return BuildMSPreviewLocation(objectSymbol.Identifier, appSymbol.ReferenceSourceFileName);
            }

            return BuildFileLocation(objectSymbol.ReferenceSourceFileName);
        }

    }
}
