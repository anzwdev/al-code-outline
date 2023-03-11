using AnZwDev.ALTools.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.Workspace
{
    public class ALProjectPathComparer : IComparer<ALProject>
    {
        public int Compare(ALProject x, ALProject y)
        {
            string xPath = x?.RootPath;
            string yPath = y?.RootPath;
            bool emptyXPath = String.IsNullOrWhiteSpace(xPath);
            bool emptyYPath = String.IsNullOrWhiteSpace(yPath);

            if (emptyXPath && emptyYPath)
                return 0;
            if (emptyYPath)
                return 1;
            if (emptyXPath)
                return -1;

            if (xPath == yPath)
                return 0;
            if (xPath.StartsWith(yPath))
                return -1;
            if (yPath.StartsWith(xPath))
                return 1;

            return String.Compare(xPath, yPath);
        }
    }
}
