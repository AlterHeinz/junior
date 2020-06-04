namespace juniorassembler
{
    // factory for IScope objects
    internal static class Scopes
    {
        internal static IScope OfBank1 { get { return new SymbolMap(false); } }
        internal static IScope OfBank2 { get { return new SymbolMap(true); } }
    }
}
