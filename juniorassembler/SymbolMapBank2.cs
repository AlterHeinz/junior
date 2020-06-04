using System.Collections.Generic;

namespace juniorassembler
{
    // a lookup table for known symbols in EPROM Bank 2 - function or variable addresses
    internal class SymbolMapBank2 : IScope
    {
        public string find(ushort address)
        {
            string ret;
            return allSymbolsBank2.TryGetValue(address, out ret) ? ret : null;
        }

        private static Dictionary<int, string> allSymbolsBank2 = new Dictionary<int, string>
        {
            { 0x2AAA, "Reversi" },
            { 0x5655, "Pacman" },
            { 0x6800, "Superhirn" },
        };
    }
}