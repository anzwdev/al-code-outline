using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.ALLanguageInformation
{
    public static class PageActionAreaCaseInformation
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
            _values.Add("Creation");
            _values.Add("Navigation");
            _values.Add("Processing");
            _values.Add("Reporting");
        }
    }
}
