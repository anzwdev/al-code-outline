using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.IO.Compression;
using Newtonsoft.Json;

namespace AnZwDev.ALTools.ALSymbolReferences.Serialization
{
    
    public static class AppPackageSymbolReferencesReader
    {

        public static ALAppSymbolReference Deserialize(string path)
        {
            ALAppSymbolReference symbolReference = null;

            Stream packageStream = AppFileHelper.OpenFileStreamWithRetry(path);

            Microsoft.Dynamics.Nav.CodeAnalysis.Packaging.NavAppPackage navAppPackage = Microsoft.Dynamics.Nav.CodeAnalysis.Packaging.NavAppPackage.Open(packageStream, false);
            Microsoft.Dynamics.Nav.CodeAnalysis.Packaging.NavAppPackageReader naAppPackageReader = new Microsoft.Dynamics.Nav.CodeAnalysis.Packaging.NavAppPackageReader(packageStream, navAppPackage, false);
            Stream symbolsStream = naAppPackageReader.ReadSymbolReferenceFile();

            using (StreamReader streamReader = new StreamReader(symbolsStream))
            using (JsonReader reader = new JsonTextReader(streamReader))
            {
                JsonSerializer serializer = new JsonSerializer();

                symbolReference = serializer.Deserialize<ALAppSymbolReference>(reader);
            }

            naAppPackageReader.Dispose();
            navAppPackage.Dispose();

            /*
            packageStream.Seek(AppPackageDataStream.HeaderLength, SeekOrigin.Begin);

            AppPackageDataStream dataStream = new AppPackageDataStream(packageStream);

            ZipArchive package = new ZipArchive(dataStream, ZipArchiveMode.Read);
            ZipArchiveEntry symbols = package.GetEntry("SymbolReference.json");
            using (Stream symbolsStream = symbols.Open())
            using (StreamReader streamReader = new StreamReader(symbolsStream))
            using (JsonReader reader = new JsonTextReader(streamReader))
            {
                JsonSerializer serializer = new JsonSerializer();
                symbolReference = serializer.Deserialize<ALAppSymbolReference>(reader);
            }
            package.Dispose();

            dataStream.Dispose();
            */
            packageStream.Close();
            packageStream.Dispose();

            if (symbolReference != null)
                symbolReference.ReferenceSourceFileName = path;

            return symbolReference;
        }

    }

}
