using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using System.IO.Compression;
using AnZwDev.ALTools.Logging;

namespace AnZwDev.ALTools.ALSymbolReferences.Serialization
{
    public static class AppPackageNavxSerializer
    {

        public static NavxPackage Deserialize(string path)
        {
            try
            {
                NavxPackage navxPackage = null;

                Stream packageStream = AppFileHelper.OpenFileStreamWithRetry(path);
                packageStream.Seek(40, SeekOrigin.Begin);

                AppPackageDataStream contentStream = new AppPackageDataStream(packageStream);

                ZipArchive package = new ZipArchive(contentStream, ZipArchiveMode.Read);
                ZipArchiveEntry metadata = package.GetEntry("NavxManifest.xml");
                using (Stream metadataStream = metadata.Open())
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(NavxPackage));
                    navxPackage = (NavxPackage)serializer.Deserialize(metadataStream);
                }
                package.Dispose();

                contentStream.Dispose();

                packageStream.Close();
                packageStream.Dispose();

                return navxPackage;
            }
            catch (Exception e)
            {
                MessageLog.LogError(e);
                return null;
            }
        }

    }
}
