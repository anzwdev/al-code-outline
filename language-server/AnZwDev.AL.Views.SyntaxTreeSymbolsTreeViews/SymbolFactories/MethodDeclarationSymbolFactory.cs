using AnZwDev.AL.Syntax;
using AnZwDev.AL.CodeAnalysis.Extensions;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.AL.Views.SyntaxTreeSymbolsTreeViews.SymbolFactories
{
    internal static class MethodDeclarationSymbolFactory
    {
        public static SyntaxTreeSymbolsTreeViewNode CreateSymbol(MethodDeclarationSyntax node, SyntaxTreeSymbolsTreeViewNode? parentNode)
        {
            ALSyntaxNodeKind kind = GetMethodALSymbolKind(node);
            var symbol = MethodOrTriggerDeclarationSymbolFactory.CreateSymbol(node, parentNode, kind);

            foreach (var attribute in node.Attributes)
            {
                if (attribute != null)
                {
                    var memberAttributeName = attribute.GetNameStringValue(); //.GetSyntaxNodeName().NotNull();
                    if (!String.IsNullOrWhiteSpace(memberAttributeName))
                    {
                        kind = MemberAttributeToMethodKind(memberAttributeName);
                        if (kind != ALSyntaxNodeKind.Undefined)
                            symbol.Kind = kind;
                        symbol.Subtype = memberAttributeName;
                    }
                }
            }

            return symbol;
        }

        private static ALSyntaxNodeKind GetMethodALSymbolKind(MethodDeclarationSyntax methodSyntax)
        {
            switch (methodSyntax.AccessModifier.Kind)
            {
                case SyntaxKind.ProtectedKeyword:
                    return ALSyntaxNodeKind.ProtectedMethodDeclaration;
                case SyntaxKind.LocalKeyword:
                    return ALSyntaxNodeKind.LocalMethodDeclaration;
                case SyntaxKind.InternalKeyword:
                    return ALSyntaxNodeKind.InternalMethodDeclaration;
            }
            return ALSyntaxNodeKind.MethodDeclaration;
        }

        private static ALSyntaxNodeKind MemberAttributeToMethodKind(string name)
        {
            //events
            if (name.Equals("IntegrationEvent", StringComparison.OrdinalIgnoreCase))
                return ALSyntaxNodeKind.IntegrationEventDeclaration;
            if (name.Equals("BusinessEvent", StringComparison.OrdinalIgnoreCase))
                return ALSyntaxNodeKind.BusinessEventDeclaration;
            if (name.Equals("InternalEvent", StringComparison.OrdinalIgnoreCase))
                return ALSyntaxNodeKind.InternalEventDeclaration;
            if (name.Equals("ExternalBusinessEvent", StringComparison.OrdinalIgnoreCase))
                return ALSyntaxNodeKind.ExternalBusinessEventDeclaration;
            if (name.Equals("EventSubscriber", StringComparison.OrdinalIgnoreCase))
                return ALSyntaxNodeKind.EventSubscriberDeclaration;
            //tests
            if (name.Equals("Test", StringComparison.OrdinalIgnoreCase))
                return ALSyntaxNodeKind.TestDeclaration;
            if (name.Equals("ConfirmHandler", StringComparison.OrdinalIgnoreCase))
                return ALSyntaxNodeKind.ConfirmHandlerDeclaration;
            if (name.Equals("FilterPageHandler", StringComparison.OrdinalIgnoreCase))
                return ALSyntaxNodeKind.FilterPageHandlerDeclaration;
            if (name.Equals("HyperlinkHandler", StringComparison.OrdinalIgnoreCase))
                return ALSyntaxNodeKind.HyperlinkHandlerDeclaration;
            if (name.Equals("MessageHandler", StringComparison.OrdinalIgnoreCase))
                return ALSyntaxNodeKind.MessageHandlerDeclaration;
            if (name.Equals("ModalPageHandler", StringComparison.OrdinalIgnoreCase))
                return ALSyntaxNodeKind.ModalPageHandlerDeclaration;
            if (name.Equals("PageHandler", StringComparison.OrdinalIgnoreCase))
                return ALSyntaxNodeKind.PageHandlerDeclaration;
            if (name.Equals("ReportHandler", StringComparison.OrdinalIgnoreCase))
                return ALSyntaxNodeKind.ReportHandlerDeclaration;
            if (name.Equals("RequestPageHandler", StringComparison.OrdinalIgnoreCase))
                return ALSyntaxNodeKind.RequestPageHandlerDeclaration;
            if (name.Equals("SendNotificationHandler", StringComparison.OrdinalIgnoreCase))
                return ALSyntaxNodeKind.SendNotificationHandlerDeclaration;
            if (name.Equals("SessionSettingsHandler", StringComparison.OrdinalIgnoreCase))
                return ALSyntaxNodeKind.SessionSettingsHandlerDeclaration;
            if (name.Equals("StrMenuHandler", StringComparison.OrdinalIgnoreCase))
                return ALSyntaxNodeKind.StrMenuHandlerDeclaration;

            return ALSyntaxNodeKind.Undefined;
        }

    }
}
