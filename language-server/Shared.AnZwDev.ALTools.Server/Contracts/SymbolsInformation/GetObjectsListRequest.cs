using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.Server.Contracts.SymbolsInformation
{
    public class GetObjectsListRequest : GetSymbolsInformationRequest
    {
        public bool includeTables;
        public bool includePages;
        public bool includeReports;
        public bool includeXmlPorts;
        public bool includeQueries;
        public bool includeCodeunits;
        public bool includeControlAddIns;
        public bool includePageExtensions;
        public bool includeTableExtensions;
        public bool includePofiles;
        public bool includePageCustomizations;
        public bool includeDotNetPackages;
        public bool includeEnumTypes;
        public bool includeEnumExtensionTypes;
        public bool includeInterfaces;
        public bool includeReportExtensions;
        public bool includePermissionSets;
        public bool includePermissionSetExtensions;

        public bool includeDependencies;
        public bool includeObsolete;
    }
}
