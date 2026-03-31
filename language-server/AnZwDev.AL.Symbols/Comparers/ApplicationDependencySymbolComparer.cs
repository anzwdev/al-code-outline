using System;
using System.Collections.Generic;
using System.Text;
using AnZwDev.AL.Symbols;
using AnZwDev.AL.Symbols.Metadata;

namespace AnZwDev.AL.Symbols.Comparers
{
    public static class ApplicationDependencySymbolComparer
    {

        public static bool ReferencedAppsEquals(List<ApplicationDependency>? set1, List<ApplicationDependency>? set2)
        {
            var hashSet1 = ToHashSet(set1);
            var hashSet2 = ToHashSet(set2);
            return hashSet1.SetEquals(hashSet2);
        }

        private static HashSet<string> ToHashSet(List<ApplicationDependency>? set)
        {
            var hashSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            if (set != null)
                for (int i = 0; i < set.Count; i++)
                    if (!hashSet.Contains(set[i].Id))
                        hashSet.Add(set[i].Id);
            return hashSet;
        }

    }
}
