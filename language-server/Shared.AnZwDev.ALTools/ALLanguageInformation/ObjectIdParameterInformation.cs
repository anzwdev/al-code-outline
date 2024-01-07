using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.ALLanguageInformation
{
    internal class ObjectIdParameterInformation
    {

        public int Index { get; }
        public string ObjectType { get; }

        public ObjectIdParameterInformation(int index, string objectType) 
        {
            Index = index;
            ObjectType = objectType;
        }


    }
}
