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
            { 0x00E1, "monitor.??" },
            { 0x00E2, "editor.BEGADL" },
            { 0x00E3, "editor.BEGADH" },
            { 0x00E4, "editor.ENDADL" },
            { 0x00E5, "editor.ENDADH" },
            { 0x00E6, "editor.CURADL" },
            { 0x00E7, "editor.CURADH" },
            { 0x00E8, "editor.CENDL" },
            { 0x00E9, "editor.CENDH" },
            { 0x00EF, "monitor.??" },
            { 0x00F0, "monitor.??" },
            { 0x00F1, "monitor.??" },
            { 0x00F2, "monitor.??" },
            { 0x00F3, "monitor.??" },
            { 0x00F4, "monitor.??" },
            { 0x00F5, "monitor.??" },
            { 0x00F6, "monitor.??/editor.BYTES" },
            { 0x00F9, "monitor.??" },
            { 0x00FA, "monitor.??" },
            { 0x00FB, "monitor.??" },
            { 0x00FC, "monitor.??" },
            { 0x00FE, "editor.NIBBLE" },
            { 0x00FF, "monitor.??" },
            { 0x0200, "serial.transmit to PC" },
            { 0x0300, "serial.receive from PC" },
            { 0x1A7A, "orig.NMIL" },
            { 0x1A7B, "orig.NMIH" },
            { 0x1A7E, "orig.IRQL" },
            { 0x1A7F, "orig.IRQH" },
            { 0x1A80, "PAD" },
            { 0x1A81, "PADD" },
            { 0x1A82, "PBD" },
            { 0x1A83, "PBDD" },
            { 0x1AD4, "RDTDIS" },
            { 0x1AD5, "RDFLAG" },
            { 0x1ADC, "RDTEN" },
            { 0x1AE4, "EDETA" },
            { 0x1AE5, "EDETB" },
            { 0x1AE6, "EDETC" },
            { 0x1AE7, "EDETD" },
            { 0x1AF4, "CNTA" },
            { 0x1AF5, "CNTB" },
            { 0x1AF6, "CNTC" },
            { 0x1AF7, "CNTD" },
            { 0x1AFC, "CNTE" },
            { 0x1AFD, "CNTF" },
            { 0x1AFE, "CNTG" },
            { 0x1AFF, "CNTH" },
            { 0x1C00, "monitor.SAVEAllRegisters" },
            { 0x1C1D, "monitor.RESET" },
            { 0x1C33, "monitor.START" },
            { 0x1C49, "monitor.GO" },
            { 0x1C5C, "monitor.handleKey" },
            { 0x1CBB, "floppy.writeExec" },
            { 0x1CEE, "floppy.vglExec" },
            { 0x1D4D, "editor.SCAN" },
            { 0x1D5C, "editor.SCANA" },
            { 0x1D61, "editor.SCANB" },
            { 0x1D6F, "editor.GETBYT" },
            { 0x1D88, "monitor.SCAND" },
            { 0x1D8E, "monitor.SCANDS" },
            { 0x1DB5, "monitor.ONEKEY" },
            { 0x1DCC, "monitor.SHOW" },
            { 0x1DDF, "monitor.CONVD" },
            { 0x1DF9, "monitor.GETKEY" },
            { 0x1E09, "monitor.KEYIN" },
            { 0x1E23, "floppy.readExec" },
            { 0x1E5C, "editor.OPLEN" },
            { 0x1E86, "comfortableReset" },
            { 0x1ED3, "editor.BEGIN" },
            { 0x1EDC, "editor.ADCEND" },
            { 0x1EEA, "editor.RECEND" },
            { 0x1EF9, "editor.??" },
            { 0x1F0F, "monitor.LOOKhexTo7Segm" },
            { 0x1F1F, "editor.opLen" },
            { 0x1F3D, "texteditor.sr1" },
            { 0x1F62, "texteditor.sr2" },
            { 0x1F73, "texteditor.sr3" },
            { 0x1F86, "texteditor.sr4" },
            { 0x1F91, "texteditor.sr5" },
            { 0x1F9C, "bios.readfileSameBank" },
            { 0x1FAA, "hexeditor.start" },
            { 0x1FBE, "bios.read 1 byte from EPROM" },
            { 0x1FC4, "bios.activate EPROM bank and segments(1A08)" },
            { 0x1FC7, "bios.activate EPROM bank and segments(AC)" },
            { 0x1FD4, "bios.activate EPROM bank and segments(1A09)" },
            { 0x1FDD, "bios.activate EPROM segments(1A08)" },
            { 0x1FE0, "bios.activate EPROM segments(AC)" },
            { 0x1FEE, "bios.deactivate EPROM segments" },
            { 0x2AAA, "Reversi" }, // Bank2
            { 0x2F00, "Hexeditor" }, // Bank1
            { 0x4000, "Texteditor" }, // Bank?
            { 0x4F30, "Directory" }, // Bank?
            { 0x5000, "Floppy" }, // Bank?
            { 0x5655, "Pacman" }, // Bank2
            { 0x6800, "Superhirn" }, // Bank2
            { 0xFFF8, "bios.flag EPROM lo" },
            { 0xFFF9, "bios.flag EPROM med" },
            { 0xFFFA, "bios.flag EPROM hi/NMIadr" },
            { 0xFFFC, "RSTadr" },
            { 0xFFFE, "IRQadr" },
            { 0xFFFF, "bios.flag EPROM Bank" },
        };
    }
}