using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.AL.Symbols
{
    public enum MemberKind
    {

        Undefined = 0,

        MethodDeclaration = 1,
        InternalMethodDeclaration = 2,
        ProtectedMethodDeclaration = 3,
        LocalMethodDeclaration = 4,

        TriggerDeclaration = 10,

        EventDeclaration = 20,
        InternalEventDeclaration = 21,
        BusinessEventDeclaration = 22,
        ExternalBusinessEventDeclaration = 23,
        IntegrationEventDeclaration = 24,

        EventSubscriberDeclaration = 40,


        TestDeclaration = 100,
        ConfirmHandlerDeclaration = 101,
        FilterPageHandlerDeclaration = 102,
        HyperlinkHandlerDeclaration = 103,
        MessageHandlerDeclaration = 104,
        ModalPageHandlerDeclaration = 105,
        PageHandlerDeclaration = 106,
        ReportHandlerDeclaration = 107,
        RequestPageHandlerDeclaration = 108,
        SendNotificationHandlerDeclaration = 109,
        SessionSettingsHandlerDeclaration = 110,
        StrMenuHandlerDeclaration = 111,

        GlobalVarSection = 500

    }
}
