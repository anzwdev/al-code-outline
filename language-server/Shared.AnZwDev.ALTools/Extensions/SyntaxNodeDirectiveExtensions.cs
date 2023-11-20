using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace AnZwDev.ALTools.Extensions
{

#if BC

    public static class SyntaxNodeDirectiveExtensions
    {

        public static bool HasOpenDirectives(this SyntaxNode node)
        {
            var regionDepth = 0;

            Dictionary<string, bool> warnings = null;
            var directives = node.GetDirectives();
            foreach (var directive in directives)
            {
                switch (directive)
                {
                    case PragmaWarningDirectiveTriviaSyntax pragmaWarning:
                        warnings = CollectPragmaWarnings(warnings, pragmaWarning);                        
                        break;
                    case RegionDirectiveTriviaSyntax region:
                        regionDepth++;
                        break;
                    case EndRegionDirectiveTriviaSyntax endRegion:
                        regionDepth--;
                        break;
                    //these cases are not needed as define and undef must be at the beginning of the file
                    //case DefineDirectiveTriviaSyntax defineDirective:
                    //    break;
                    //case UndefDirectiveTriviaSyntax undefDirective:
                    //    break;
                }
            }

            return 
                ((warnings != null) && (warnings.Count > 0)) ||
                (regionDepth != 0);
        }

        private static Dictionary<string, bool> CollectPragmaWarnings(Dictionary<string, bool> warnings, PragmaWarningDirectiveTriviaSyntax pragmaWarningDirective)
        {
            if (warnings == null)
                warnings = new Dictionary<string, bool>();

            var pragmaType = pragmaWarningDirective.DisableOrRestoreKeyword.Text;
            var warningEnabled = (pragmaType != null) && (pragmaType.Equals("restore", StringComparison.CurrentCultureIgnoreCase));

            if ((pragmaWarningDirective.ErrorCodes != null) && (pragmaWarningDirective.ErrorCodes.Count > 0))
            {
                for (int i=0; i < pragmaWarningDirective.ErrorCodes.Count; i++)
                {
                    CollectPragmaWarning(pragmaWarningDirective.ErrorCodes[i].ToString(), warningEnabled, warnings);
                }
            } 
            else
            {
                CollectPragmaWarning("all", warningEnabled, warnings);
            }

            return warnings;
        }

        private static void CollectPragmaWarning(string warningCode, bool enabled, Dictionary<string, bool> warnings)
        {
            if (!String.IsNullOrWhiteSpace(warningCode))
            {
                warningCode = warningCode.Trim().ToLower();

                if (warnings.ContainsKey(warningCode))
                {
                    if (warnings[warningCode] != enabled)
                        warnings.Remove(warningCode);
                }
                else
                    warnings.Add(warningCode, enabled);
            }
        }

    }

#endif

}
