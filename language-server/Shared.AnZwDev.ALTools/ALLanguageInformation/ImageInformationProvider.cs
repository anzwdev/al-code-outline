using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using AnZwDev.ALTools.Logging;

namespace AnZwDev.ALTools.ALLanguageInformation
{
    public class ImageInformationProvider
    {

        public ImageInformationProvider()
        {
        }

        public List<ImageInformation> GetActionImages()
        {
            return this.GetImages("GetActionImageResources");
        }

        public List<ImageInformation> GetFieldCueGroupImages()
        {
            return this.GetImages("GetFieldCueGroupImageResources");
        }

        public List<ImageInformation> GetActionCueGroupImages()
        {
            return this.GetImages("GetActionCueGroupImageResources");
        }

        public List<ImageInformation> GetRoleCenterActionImages()
        {
            return this.GetImages("GetRoleCenterActionGroupImageResources");
        }

        protected List<ImageInformation> GetImages(string methodName)
        {
            List<ImageInformation> imagesList = new List<ImageInformation>();
            string imageType = "data:image/png;base64,";

            try
            {
                Type knownType = typeof(Microsoft.Dynamics.Nav.CodeAnalysis.SyntaxFactory);
                Type imageResourcesType = knownType.Assembly.GetType("Microsoft.Dynamics.Nav.CodeAnalysis.ImageResources");
                MethodInfo getImagesMethodInfo = imageResourcesType.GetMethod(methodName, BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
                object imagesObject = getImagesMethodInfo.Invoke(null, null);
                IDictionary<string, string> imagesDictionary = imagesObject as IDictionary<string, string>;
                if (imagesDictionary != null)
                {
                    foreach (string name in imagesDictionary.Keys)
                    {
                        string content = Uri.UnescapeDataString(imagesDictionary[name]);
                        if (!content.StartsWith("data:", StringComparison.CurrentCultureIgnoreCase))
                            content = imageType + content;
                        imagesList.Add(new ImageInformation(name, content));
                    }
                }
            }
            catch (Exception e)
            {
                MessageLog.LogError(e);
            }
            imagesList.Sort(new ImageInformationComparer());
            return imagesList;
        }

    }
}
