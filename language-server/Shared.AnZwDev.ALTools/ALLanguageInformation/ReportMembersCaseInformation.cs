using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.ALLanguageInformation
{
    public class ReportMembersCaseInformation
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
            _values.Add("SaveAsExcel");
            _values.Add("SaveAsHtml");
            _values.Add("SaveAsPdf");
            _values.Add("SaveAsWord");
            _values.Add("SaveAsXml");
            _values.Add("Print");
            _values.Add("PrintOnlyIfDetail");
        }
    }
}
