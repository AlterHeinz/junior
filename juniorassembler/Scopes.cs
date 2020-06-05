namespace juniorassembler
{
    // factory for IDualScope objects
    internal static class Scopes
    {
        internal static IDualScope OfBank1 { get { return new DualScope(Combine(new SymbolMapBank1(), new SymbolMapStandard()), AllSymbols); } }
        internal static IDualScope OfBank2 { get { return new DualScope(Combine(new SymbolMapBank2(), new SymbolMapStandard()), AllSymbols); } }

        private static IScope AllSymbols { get { return Combine(new SymbolMapBank2(), Combine(new SymbolMapBank1(), new SymbolMapStandard())); } }

        private static IScope Combine(IScope first, IScope second)
        {
            return new CombinedScope(first, second);
        }

        private class CombinedScope : IScope
        {
            private IScope first;
            private IScope second;

            public CombinedScope(IScope first, IScope second)
            {
                this.first = first;
                this.second = second;
            }

            public string find(ushort address)
            {
                return first.find(address) ?? second.find(address);
            }
        }

        private class DualScope : IDualScope
        {
            private IScope callerScope;
            private IScope usedScope;

            public DualScope(IScope callerScope, IScope usedScope)
            {
                this.callerScope = callerScope;
                this.usedScope = usedScope;
            }

            public string findCallerSymbol(ushort address)
            {
                return callerScope.find(address);
            }

            public string findUsedSymbol(ushort address)
            {
                return usedScope.find(address);
            }
        }
    }
}
