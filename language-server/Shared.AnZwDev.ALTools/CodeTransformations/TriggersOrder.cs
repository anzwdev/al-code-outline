using AnZwDev.ALTools.ALSymbols.Internal;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;
using System.Text;
using System.Xml.Linq;

namespace AnZwDev.ALTools.CodeTransformations
{

    internal class TriggersOrder
    {
        public ConvertedSyntaxKind Kind { get; }
        public string[] Triggers { get; }
        private Dictionary<string, int> _triggersOrder = new Dictionary<string, int>();

        public TriggersOrder(ConvertedSyntaxKind kind, string[] triggers)
        {
            this.Kind = kind;
            this.Triggers = triggers;

            for (int i = 0; i < this.Triggers.Length; i++)
            {
                this.Triggers[i] = this.Triggers[i].ToLower();
                this._triggersOrder.Add(triggers[i], i);
            }
        }

        public int Compare(string nameX, string nameY)
        {
            nameX = nameX.ToLower();
            nameY = nameY.ToLower();
            bool containsX = this._triggersOrder.ContainsKey(nameX);
            bool containsY = this._triggersOrder.ContainsKey(nameY);

            if (containsX && containsY)
                return _triggersOrder[nameX] - _triggersOrder[nameY];
            else if (containsX)
                return -1;
            else if (containsY)
                return 1;
            else
                return StringComparer.OrdinalIgnoreCase.Compare(nameX, nameY);
        }

    }
}
