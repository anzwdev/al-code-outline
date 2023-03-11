using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.ALLanguageInformation
{
    public static class ApplicationAreaCaseInformation
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
            _values.Add("All");
            _values.Add("Basic");
            _values.Add("Suite");
            _values.Add("Relationship Mgmt");
            _values.Add("Jobs");
            _values.Add("Fixed Assets");
            _values.Add("Location");
            _values.Add("BasicHR");
            _values.Add("Assembly");
            _values.Add("Item Charges");
            _values.Add("Advanced");
            _values.Add("Warehouse");
            _values.Add("Service");
            _values.Add("Manufacturing");
            _values.Add("Planning");
            _values.Add("Dimensions");
            _values.Add("Item Tracking");
            _values.Add("Intercompany");
            _values.Add("Sales Return Order");
            _values.Add("Purch Return Order");
            _values.Add("Prepayments");
            _values.Add("Cost Accounting");
            _values.Add("Sales Budget");
            _values.Add("Purchase Budget");
            _values.Add("Item Budget");
            _values.Add("Sales Analysis");
            _values.Add("Purchase Analysis");
            _values.Add("XBRL");
        }

    }
}
