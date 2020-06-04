namespace juniorassembler
{
    // factory for IScope objects
    internal static class Scopes
    {
        internal static IScope OfBank1 { get { return Combine(new SymbolMapBank1(), new SymbolMapStandard()); } }
        internal static IScope OfBank2 { get { return Combine(new SymbolMapBank2(), new SymbolMapStandard()); } }
        internal static IScope Combine(IScope first, IScope second)
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
    }
}
