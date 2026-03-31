using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Workspaces.Formatting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.AL.CodeAnalysis.Formatting
{
    public class ALFormatter
    {

        private Microsoft.Dynamics.Nav.EditorServices.Protocol.VsCodeWorkspace? _workspace = null;

        public SyntaxNode Format(SyntaxNode node)
        {
            return Microsoft.Dynamics.Nav.CodeAnalysis.Workspaces.Formatting.Formatter.Format(node, this.GetWorkspace());
        }

        protected Microsoft.Dynamics.Nav.EditorServices.Protocol.VsCodeWorkspace GetWorkspace()
        {
            if (this._workspace == null)
                this._workspace = new Microsoft.Dynamics.Nav.EditorServices.Protocol.VsCodeWorkspace();
            return this._workspace;
        }

    }
}
