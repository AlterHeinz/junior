namespace juniorassembler
{
    internal interface IDualScope
    {
        string findUsedSymbol(ushort address);
        string findCallerSymbol(ushort address);
    }
}