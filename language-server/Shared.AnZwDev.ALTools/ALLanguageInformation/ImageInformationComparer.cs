using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.ALLanguageInformation
{
    internal class ImageInformationComparer : IComparer<ImageInformation>
    {

        public ImageInformationComparer()
        {
        }

        public int Compare(ImageInformation x, ImageInformation y)
        {
            if (x.Name != null)
                return x.Name.CompareTo(y.Name);
            if (y.Name != null)
                return -y.Name.CompareTo(x.Name);
            return 0;
        }
    }
}
