using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.ALLanguageInformation
{
    public static class MemberAttributeNameCaseInformation
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
            _values.Add("CommitBehavior");
            _values.Add("ConfirmHandler");
            _values.Add("ErrorBehavior");
            _values.Add("EventSubscriber");
            _values.Add("ExternalBusinessEvent");
            _values.Add("FilterPageHandler");
            _values.Add("HandlerFunctions");
            _values.Add("HyperlinkHandler");
            _values.Add("InDataSet");
            _values.Add("InherentPermissions");
            _values.Add("IntegrationEvent");
            _values.Add("InternalEvent");
            _values.Add("MessageHandler");
            _values.Add("ModalPageHandler");
            _values.Add("NonDebuggable");
            _values.Add("None");
            _values.Add("Normal");
            _values.Add("Obsolete");
            _values.Add("PageHandler");
            _values.Add("RecallNotificationHandler");
            _values.Add("ReportHandler");
            _values.Add("RequestPageHandler");
            _values.Add("RequiredPermissions");
            _values.Add("RunOnClient");
            _values.Add("Scope");
            _values.Add("SecurityFiltering");
            _values.Add("SendNotificationHandler");
            _values.Add("ServiceEnabled");
            _values.Add("SessionSettingsHandler");
            _values.Add("StrMenuHandler");
            _values.Add("SuppressDispose");
            _values.Add("Test");
            _values.Add("TestPermissions");
            _values.Add("TransactionModel");
            _values.Add("TryFunction");
            _values.Add("WithEvents");
        }

    }
}
