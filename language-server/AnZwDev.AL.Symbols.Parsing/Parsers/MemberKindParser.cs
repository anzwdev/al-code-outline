using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.AL.Symbols.Parsing.Parsers
{
    internal class MemberKindParser : HashTableParser<MemberKind>
    {

        public MemberKindParser() : base(MemberKind.Undefined)
        {
            this.Add("IntegrationEvent", MemberKind.IntegrationEventDeclaration);
            this.Add("BusinessEvent", MemberKind.BusinessEventDeclaration);
            this.Add("InternalEvent", MemberKind.InternalEventDeclaration);
            this.Add("ExternalBusinessEvent", MemberKind.ExternalBusinessEventDeclaration);
            this.Add("EventSubscriber", MemberKind.EventSubscriberDeclaration);
            //tests
            this.Add("Test", MemberKind.TestDeclaration);
            this.Add("ConfirmHandler", MemberKind.ConfirmHandlerDeclaration);
            this.Add("FilterPageHandler", MemberKind.FilterPageHandlerDeclaration);
            this.Add("HyperlinkHandler", MemberKind.HyperlinkHandlerDeclaration);
            this.Add("MessageHandler", MemberKind.MessageHandlerDeclaration);
            this.Add("ModalPageHandler", MemberKind.ModalPageHandlerDeclaration);
            this.Add("PageHandler", MemberKind.PageHandlerDeclaration);
            this.Add("ReportHandler", MemberKind.ReportHandlerDeclaration);
            this.Add("RequestPageHandler", MemberKind.RequestPageHandlerDeclaration);
            this.Add("SendNotificationHandler", MemberKind.SendNotificationHandlerDeclaration);
            this.Add("SessionSettingsHandler", MemberKind.SessionSettingsHandlerDeclaration);
            this.Add("StrMenuHandler", MemberKind.StrMenuHandlerDeclaration);
        }


    }
}
