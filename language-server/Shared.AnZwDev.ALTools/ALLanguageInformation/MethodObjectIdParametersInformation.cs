using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.ALLanguageInformation
{
    internal class MethodObjectIdParametersInformation
    {

        public string Name {  get; }
        public ObjectIdParameterInformation[] Parameters { get; }

        public MethodObjectIdParametersInformation(string name, params ObjectIdParameterInformation[] parameters)
        {
            Name = name;
            Parameters = parameters;
        }

    }
}
