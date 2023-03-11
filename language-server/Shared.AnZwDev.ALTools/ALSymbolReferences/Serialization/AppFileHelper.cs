using AnZwDev.ALTools.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace AnZwDev.ALTools.ALSymbolReferences.Serialization
{
    public static class AppFileHelper
    {

        public static string GetAppFileContent(string appFilePath, string filePath)
        {
            string content = null;
            try
            {
                AppPackageInformation appPackageInformation = new AppPackageInformation(appFilePath);
                if ((appPackageInformation.Manifest != null) && 
                    (appPackageInformation.Manifest.App != null) &&
                    (!appPackageInformation.IsRuntimePackage))
                {
                    using (FileStream packageStream = OpenFileStreamWithRetry(appFilePath))
                    {
                        packageStream.Seek(AppPackageDataStream.HeaderLength, SeekOrigin.Begin);

                        using (AppPackageDataStream dataStream = new AppPackageDataStream(packageStream))
                        {
                            string contentFilePath = filePath.Replace("\\", "/");
                            //encode
                            contentFilePath = contentFilePath.Replace("%", "%25").Replace(" ", "%20");
                            //encode second time
                            contentFilePath = contentFilePath.Replace("%", "%25");
                            contentFilePath = "src/" + contentFilePath;

                            using (ZipArchive package = new ZipArchive(dataStream, ZipArchiveMode.Read))
                            {

                                ZipArchiveEntry contentFile = package.GetEntry(contentFilePath);
                                if (contentFile != null)
                                {
                                    using (Stream contentStream = contentFile.Open())
                                    using (StreamReader streamReader = new StreamReader(contentStream))
                                    {
                                        content = streamReader.ReadToEnd();
                                    }
                                }
                            }
                            dataStream.Close();
                        }
                        packageStream.Close();
                    }
                }
            }
            catch (Exception e)
            {
                MessageLog.LogError(e);
                return null;
            }
            return content;
        }

        public static FileStream OpenFileStreamWithRetry(string path)
        {
            int openCount = 5;
            int failedOpenDelay = 1000;

            while (openCount > 0)
            {
                try
                {
                    FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                    return stream;
                }
                catch (Exception)
                {
                    openCount--;
                    if (openCount <= 0)
                        throw;
                    System.Threading.Thread.Sleep(failedOpenDelay);
                }
            }

            return null;
        }
    }
}
