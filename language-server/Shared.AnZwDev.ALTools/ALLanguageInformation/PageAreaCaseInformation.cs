using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.ALLanguageInformation
{
    public static class PageAreaCaseInformation
    {
        private static CaseInformationCollection _values = null;
        public static CaseInformationCollection Values
        {
            get
            {
                if (_values == null)
                    InitValues();
                return _values;
            }
        }

        private static void InitValues()
        {
            _values = new CaseInformationCollection();
            _values.Add("Content");
            _values.Add("FactBoxes");
        }
    }
}
