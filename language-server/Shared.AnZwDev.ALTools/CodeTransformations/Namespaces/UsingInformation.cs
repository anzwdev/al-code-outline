using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.CodeTransformations.Namespaces
{
    internal class UsingInformation
    {

        public string Id { get; }
        public string Namespace { get; set; }
        public string FilePath { get; }
        public bool IsFile { get; set; }
        public bool InUsing { get; set; }
        public bool InVariableDeclaration { get; set; }
        public bool UsingRequired { get; set; }

        public UsingInformation() 
        {
        }

        public UsingInformation(string namespaceName, string filePath, bool inUsing, bool inVariableDeclaration, bool usingRequired)
        {
            Namespace = namespaceName;
            FilePath = filePath;
            InUsing = inUsing;
            InVariableDeclaration = inVariableDeclaration;
            UsingRequired = usingRequired;
            if (!String.IsNullOrWhiteSpace(Namespace))            
                Id = Namespace;
            else
                Id = FilePath;
        }

        public void Merge(UsingInformation usingInformation)
        {
            InUsing = InUsing | usingInformation.InUsing;
            InVariableDeclaration = InVariableDeclaration | usingInformation.InVariableDeclaration;
            UsingRequired = UsingRequired | usingInformation.UsingRequired;
        }

    }
}
