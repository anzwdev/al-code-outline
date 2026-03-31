namespace AnZwDev.AL.Symbols.Providers.AppPackages.Symbols
{
    internal static class AppSerializedSymbolsArrayExtensions
    {

        public static List<TSymbol> CreateSymbolsList<TSymbol, TSerializedSymbol>(this TSerializedSymbol[]? values, string? ns) where TSerializedSymbol : AppSerializedSymbol<TSymbol> where TSymbol : Symbol
        {
            var list = new List<TSymbol>();
            if (values != null)
                for (int i = 0; i < values.Length; i++)
                    list.Add(values[i].CreateSymbol(ns));
            return list;
        }

        public static List<TSymbol>? CreateSymbolsListOrNull<TSymbol, TSerializedSymbol>(this TSerializedSymbol[]? values, string? ns) where TSerializedSymbol : AppSerializedSymbol<TSymbol> where TSymbol : Symbol
        {
            if ((values == null) || (values.Length == 0))
                return null;

            return CreateSymbolsList<TSymbol, TSerializedSymbol>(values, ns);
        }

    }
}
