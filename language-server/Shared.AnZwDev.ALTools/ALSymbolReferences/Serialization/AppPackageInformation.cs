using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.IO.Compression;
using System.Xml.Serialization;
using AnZwDev.ALTools.Logging;

namespace AnZwDev.ALTools.ALSymbolReferences.Serialization
{
    public class AppPackageInformation
    {

        public string FullPath { get; private set; }
        public string FullName { get; private set; }
        public byte[] UId { get; private set; }
        public NavxPackage Manifest { get; private set; }
        public bool IsRuntimePackage { get; private set; }

        public AppPackageInformation(string fullPath)
        {
            this.IsRuntimePackage = false;
            this.FullPath = fullPath;
            this.FullName = Path.GetFileNameWithoutExtension(this.FullPath);
            this.Reload();
        }

        public void Reload()
        {
            try
            {
                if (File.Exists(this.FullPath))
                {
                    using (Stream stream = AppFileHelper.OpenFileStreamWithRetry(this.FullPath))
                    {
                        //load unique id
                        this.UId = new byte[AppPackageDataStream.HeaderLength];
                        stream.Read(this.UId, 0, this.UId.Length);

                        Microsoft.Dynamics.Nav.CodeAnalysis.Packaging.NavAppPackage navAppPackage = Microsoft.Dynamics.Nav.CodeAnalysis.Packaging.NavAppPackage.Open(stream, false);
#if BC
                        this.IsRuntimePackage = navAppPackage.IsRuntimePackage;
#else
                        this.IsRuntimePackage = false;
#endif
                        Microsoft.Dynamics.Nav.CodeAnalysis.Packaging.NavAppPackageReader naAppPackageReader = new Microsoft.Dynamics.Nav.CodeAnalysis.Packaging.NavAppPackageReader(stream, navAppPackage, false);
                        Microsoft.Dynamics.Nav.CodeAnalysis.Packaging.NavAppManifest navAppManifest = naAppPackageReader.ReadNavAppManifest();
                        naAppPackageReader.Dispose();
                        navAppPackage.Dispose();

                        this.Manifest = NavAppManifestNavxPackageConverter.CreateNavxPackageManifest(navAppManifest);                       

                        /*
                        //load metadata
                        using (AppPackageDataStream dataStream = new AppPackageDataStream(stream))
                        {
                            using (ZipArchive package = new ZipArchive(dataStream, ZipArchiveMode.Read))
                            {
                                ZipArchiveEntry metadata = package.GetEntry("NavxManifest.xml");
                                using (Stream metadataStream = metadata.Open())
                                {
                                    XmlSerializer serializer = new XmlSerializer(typeof(NavxPackage));
                                    this.Manifest = (NavxPackage)serializer.Deserialize(metadataStream);
                                }
                            }
                        }
                        */
                    }
                } 
                else
                {
                    this.Manifest = null;
                }
            }
            catch (Exception e)
            {
                MessageLog.LogError(e);
                this.Manifest = null;
            }
        }

        public bool TheSameFile(AppPackageInformation appPackage)
        {
            return this.FullPath.Equals(appPackage.FullPath);
        }

        public bool TheSameContent(AppPackageInformation appPackage)
        {
            return ((this.TheSameExtension(appPackage, true)) && ((!ALToolsConst.CompareAppPackageUId) || (this.TheSameUId(appPackage))));
        }

        public bool TheSameExtension(AppPackageInformation appPackage, bool compareVersion)
        {
            return ((!this.IsEmpty()) &&
                (!appPackage.IsEmpty()) &&
                (this.Manifest.App.Equals(appPackage.Manifest.App, compareVersion)));
        }

        public bool TheSameUId(AppPackageInformation appPackage)
        {
            return this.TheSameUId(appPackage.UId);
        }

        public bool TheSameUId(byte[] newUId)
        {
            for (int i = 0; i < this.UId.Length; i++)
            {
                if (this.UId[i] != newUId[i])
                    return false;
            }
            return true;
        }
        public bool FileUIdChanged()
        {
            bool result = false;
            using (Stream stream = AppFileHelper.OpenFileStreamWithRetry(this.FullPath))
            {
                byte[] newUid = new byte[AppPackageDataStream.HeaderLength];
                stream.Read(newUid, 0, this.UId.Length);
                result = !this.TheSameUId(newUid);
            }
            return result;
        }

        public bool IsEmpty()
        {
            return ((this.Manifest == null) || (this.Manifest.App == null));
        }

    }
}
