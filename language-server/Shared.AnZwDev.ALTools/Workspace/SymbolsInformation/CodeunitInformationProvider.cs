using System;
using System.Collections.Generic;
using System.Text;
using AnZwDev.ALTools.ALSymbolReferences;
using AnZwDev.ALTools.Workspace.SymbolReferences;

namespace AnZwDev.ALTools.Workspace.SymbolsInformation
{
    public class CodeunitInformationProvider : BaseObjectInformationProvider<ALAppCodeunit>
    {

        public CodeunitInformationProvider() : base(x => x.Codeunits)
        {
        }

        public List<CodeunitInformation> GetCodeunits(ALProject project)
        {
            var infoList = new List<CodeunitInformation>();
            var codeunitsCollection = GetALAppObjectsCollection(project);
            var codeunitsEnumerable = codeunitsCollection.GetAll();
            foreach (var codeunit in codeunitsEnumerable)
                infoList.Add(new CodeunitInformation(codeunit));
            return infoList;
        }

    }
}
