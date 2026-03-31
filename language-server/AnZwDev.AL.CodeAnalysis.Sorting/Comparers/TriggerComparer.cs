using Microsoft.Dynamics.Nav.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;
using System.Text;
using System.Xml.Linq;

namespace AnZwDev.AL.CodeAnalysis.Sorting.Comparers
{

    public class TriggerComparer
    {
        public SyntaxKind Kind { get; }
        public string[] Triggers { get; }
        private Dictionary<string, int> _triggersOrder = new Dictionary<string, int>();

        public TriggerComparer(SyntaxKind kind, string[] triggers)
        {
            Kind = kind;
            Triggers = triggers;

            for (int i = 0; i < Triggers.Length; i++)
            {
                Triggers[i] = Triggers[i].ToLower();
                _triggersOrder.Add(triggers[i], i);
            }
        }

        public int Compare(string nameX, string nameY)
        {
            nameX = nameX.ToLower();
            nameY = nameY.ToLower();
            bool containsX = _triggersOrder.ContainsKey(nameX);
            bool containsY = _triggersOrder.ContainsKey(nameY);

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
