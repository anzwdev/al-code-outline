using AnZwDev.AL.Symbols.Providers.AppPackages.Metadata;
using AnZwDev.AL.Symbols.Providers.AppPackages.Symbols;
using AnZwDev.System.IO;
using Microsoft.Dynamics.Nav.CodeAnalysis.Packaging;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.AL.Symbols.Providers.AppPackages
{
    public class AppPackageContentProvider
    {

        public string? GetAppPackageContent(string appPackagePath, string contentPath)
        {
            using (var stream = FileHelper.OpenFileStreamWithRetry(appPackagePath))
            {
                if (stream != null)
                {
                    using (var navAppPackage = NavAppPackage.Open(stream, false))
                    using (var naAppPackageReader = new NavAppPackageReader(stream, navAppPackage, false))
                    using (var contentStream = naAppPackageReader.ReadFile(contentPath))
                    using (var contentStreamReader = new StreamReader(contentStream))
                    {
                        return contentStreamReader.ReadToEnd();
                    }
                }
            }
            return null;
        }

    }
}
