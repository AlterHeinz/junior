using System.Collections.Generic;

namespace juniorassembler
{
    // a lookup table for known symbols - function or variable addresses
    internal class SymbolMap
    {
        internal static string find(int argument)
        {
            string ret;
            return allDefaultSymbols.TryGetValue(argument, out ret) ? ret : null;
        }

        private static Dictionary<int, string> allDefaultSymbols = new Dictionary<int, string>
        {
            { 0x0200, "transmit to PC" },
            { 0x0300, "receive from PC" },
            { 0x1A80, "PAD" },
            { 0x1A81, "PADD" },
            { 0x1A82, "PBD" },
            { 0x1A83, "PBDD" },
            { 0x1C1D, "RST" },
            { 0x1D4D, "??" },
            { 0x1D8E, "??" },
            { 0x1DF9, "??" },
            { 0x1E5C, "??" },
            { 0x1FBE, "read 1 byte from EPROM" },
            { 0x1FC4, "activate EPROM bank and segments(1A08)" },
            { 0x1FC7, "activate EPROM bank and segments(AC)" },
            { 0x1FD4, "activate EPROM bank and segments(1A09)" },
            { 0x1FDD, "activate EPROM segments(1A08)" },
            { 0x1FE0, "activate EPROM segments(AC)" },
            { 0x1FEE, "deactivate EPROM segments" },
            { 0x2AAA, "Reversi" }, // Bank2
            { 0x2F00, "Hexeditor" }, // Bank1
            { 0x5655, "Pacman" }, // Bank2
            { 0x6800, "Superhirn" }, // Bank2
            { 0xFFF8, "flag EPROM lo" },
            { 0xFFF9, "flag EPROM me" },
            { 0xFFFA, "flag EPROM hi" },
            { 0xFFFF, "flag EPROM Bank" },
        };
    }
}