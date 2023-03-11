using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.ALLanguageInformation
{
    public static class SystemPartCaseInformation
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
            _values.Add("Links");
            _values.Add("MyNotes");
            _values.Add("Notes");
            _values.Add("Outlook");
        }
    }
}
