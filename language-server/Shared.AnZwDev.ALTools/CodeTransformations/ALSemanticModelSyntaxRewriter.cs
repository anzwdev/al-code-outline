using Microsoft.Dynamics.Nav.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.CodeTransformations
{
    public class ALSemanticModelSyntaxRewriter : ALSyntaxRewriter
    {
        public SemanticModel SemanticModel { get; set; }

        public ALSemanticModelSyntaxRewriter()
        { 
        }


    }
}
