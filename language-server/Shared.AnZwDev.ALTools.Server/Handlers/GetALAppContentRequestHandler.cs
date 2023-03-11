using AnZwDev.ALTools.ALSymbolReferences.Serialization;
using AnZwDev.ALTools.Server.Contracts;
using AnZwDev.VSCodeLangServer.Protocol.Server;
using AnZwDev.VSCodeLangServer.Protocol.MessageProtocol;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.ALTools.Server.Handlers
{
    public class GetALAppContentRequestHandler : BaseALRequestHandler<GetALAppContentRequest, GetALAppContentResponse>
    {

        public GetALAppContentRequestHandler(ALDevToolsServer server, LanguageServerHost languageServerHost) : base(server, languageServerHost, "al/getalappcontent")
        {
        }

#pragma warning disable 1998
        protected override async Task<GetALAppContentResponse> HandleMessage(GetALAppContentRequest parameters, RequestContext<GetALAppContentResponse> context)
        {
            GetALAppContentResponse response = new GetALAppContentResponse();
            try
            {
                AppPackageInformation appPackageInformation = new AppPackageInformation(parameters.appPath);

                
                if ((appPackageInformation.Manifest == null) || (appPackageInformation.Manifest.App == null))
                    response.source = "Cannot find application manifest in the \"" + parameters.appPath + "\" file";
                else if ((appPackageInformation.Manifest.ResourceExposurePolicy == null) && (!appPackageInformation.Manifest.App.ShowMyCode))
                    response.source = "File cannot be opened. Publisher of the application has set ShowMyCode property to false.";
                else if ((appPackageInformation.Manifest.ResourceExposurePolicy != null) && (!appPackageInformation.Manifest.ResourceExposurePolicy.IncludeSourceInSymbolFile))
                    response.source = "File cannot be opened. Publisher of the application has set ResourceExposurePolicy.IncludeSourceInSymbolFile property to false.";
                else if (appPackageInformation.IsRuntimePackage)
                    response.source = "File cannot be opened.";
                else
                {
                    using (FileStream packageStream = AppFileHelper.OpenFileStreamWithRetry(parameters.appPath))
                    {
                        packageStream.Seek(AppPackageDataStream.HeaderLength, SeekOrigin.Begin);

                        using (AppPackageDataStream dataStream = new AppPackageDataStream(packageStream))
                        {
                            string contentFilePath = parameters.filePath.Replace("\\", "/");
                            //encode
                            contentFilePath = contentFilePath.Replace("%", "%25").Replace(" ", "%20");
                            //encode second time
                            contentFilePath = contentFilePath.Replace("%", "%25");
                            contentFilePath = "src/" + contentFilePath;

                            using (ZipArchive package = new ZipArchive(dataStream, ZipArchiveMode.Read))
                            {

                                ZipArchiveEntry contentFile = package.GetEntry(contentFilePath);
                                if (contentFile == null)
                                {
                                    response.source = "File \"" + parameters.filePath + "\" not found in the app file";
                                }
                                else
                                {
                                    using (Stream contentStream = contentFile.Open())
                                    using (StreamReader streamReader = new StreamReader(contentStream))
                                    {
                                        response.source = streamReader.ReadToEnd();
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
                response.source = "Reading file content failed. " + e.Message;
                this.LogError(e);
            }
            return response;
        }
#pragma warning restore 1998

    }
}
