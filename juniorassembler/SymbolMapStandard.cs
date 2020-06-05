﻿using System.Collections.Generic;

namespace juniorassembler
{
    // a lookup table for known symbols - function or variable addresses
    // Original junior module names: orig, monitor, editor, "".
    internal class SymbolMapStandard : IScope
    {
        public string find(ushort address)
        {
            string ret;
            return allCommonSymbols.TryGetValue(address, out ret) ? ret : null;
        }

        private static Dictionary<int, string> allCommonSymbols = new Dictionary<int, string>
        {
            { 0x0041, "hexedit.da.nr" },
            { 0x0042, "hexedit.da.laenge" },
            { 0x0043, "hexedit.da.fehler" },
            { 0x0044, "hexedit.da.opcode" },
            { 0x0045, "hexedit.da.erstes" },
            { 0x0046, "hexedit.da.zweites" },
            { 0x0047, "hexedit.da.drittes" },
            { 0x0048, "hexedit.da.adrL" },
            { 0x0049, "hexedit.da.adrH" },
            { 0x0055, "hexedit.helpL" },
            { 0x0056, "hexedit.helpH" },
            { 0x0057, "bs.topWait" },
            { 0x0058, "bs.aktZeitL" },
            { 0x0059, "bs.aktZeitH" },
            { 0x005A, "bs.neueZeitL" },
            { 0x005B, "bs.neueZeitH" },
            { 0x005C, "bs.uhrZeitL" },
            { 0x005D, "bs.uhrZeitH" },
            { 0x005E, "bs.breakZeitL" },
            { 0x005F, "bs.breakZeitH" },
            { 0x006D, "hexedit.spaTty" },
            { 0x006E, "hexedit.zeiTty" },
            { 0x007F, "bs.KPUe.saveRY" },
            { 0x0080, "bs.KPUe.savePCL" },
            { 0x0081, "bs.KPUe.savePCH" },
            { 0x0082, "bs.KPUe.adrParamsL" },
            { 0x0083, "bs.KPUe.adrParamsH" },
            { 0x008A, "bs.ACSave" },
            { 0x008B, "bs.RXSave" },
            { 0x008C, "bs.RYSave" },
            { 0x008D, "bs.SPSave" },
            { 0x008E, "bs.maxTask" },
            { 0x008F, "bs.aktTask" },
            { 0x0090, "bs.grenzPrio" },
            { 0x0091, "bs.modus" },
            { 0x0094, "bs.helpActivate" },
            { 0x0095, "hexedit.Prosa" },
            { 0x0098, "hexedit.videoSpa" },
            { 0x0099, "hexedit.video2" },
            { 0x009A, "hexedit.video3" },
            { 0x009B, "hexedit.video4" },
            { 0x00CA, "bs.Transfer3.vonL" },
            { 0x00CB, "bs.Transfer3.vonH" },
            { 0x00CC, "bs.Transfer3.bisL" },
            { 0x00CD, "bs.Transfer3.bisH" },
            { 0x00CE, "bs.Transfer3.nachL" },
            { 0x00CF, "bs.Transfer3.nachH" },
            { 0x00A0, "serial.CurrentL" },
            { 0x00A1, "serial.CurrentH" },
            { 0x00A2, "serial.CurrentHiOrLoBits" },
            { 0x00E1, "monitor.KEY" },
            { 0x00E2, "editor.BEGADL" },
            { 0x00E3, "editor.BEGADH" },
            { 0x00E4, "editor.ENDADL" },
            { 0x00E5, "editor.ENDADH" },
            { 0x00E6, "editor.CURADL" },
            { 0x00E7, "editor.CURADH" },
            { 0x00E8, "editor.CENDL" },
            { 0x00E9, "editor.CENDH" },
            { 0x00EF, "monitor.SavePCL" },
            { 0x00F0, "monitor.SavePCH" },
            { 0x00F1, "monitor.SaveP" },
            { 0x00F2, "monitor.SaveSP" },
            { 0x00F3, "monitor.SaveAC" },
            { 0x00F4, "monitor.SaveRy" },
            { 0x00F5, "monitor.SaveRX" },
            { 0x00F6, "monitor.BYTES" },
            { 0x00F9, "monitor.INH" },
            { 0x00FA, "monitor.POINTL" },
            { 0x00FB, "monitor.POINTH" },
            { 0x00FC, "monitor.TEMP" },
            { 0x00FE, "editor.NIBBLE" },
            { 0x00FF, "monitor.MODE" },
            { 0x0101, "bs.RetAdrL" },
            { 0x0102, "bs.RetAdrH" },
            { 0x0200, "serial.transmit to PC" },
            { 0x0260, "serial.sendLow/log1/Stopbit" },
            { 0x0264, "serial.sendHi/log0/Startbit" },
            { 0x0266, "serial.sendBit" },
            { 0x0280, "serial.send7BitWord" },
            { 0x0300, "serial.receive from PC" },
            { 0x037E, "hexedit.arrZeiNrL" },
            { 0x0397, "hexedit.arrZeiNrH" },
            { 0x03B0, "hexedit.arrProsa" },
            { 0x03C8, "bs.arrSP" },
            { 0x03D0, "bs.arrRWL" },
            { 0x03D8, "bs.arrSema" },
            { 0x03E0, "bs.arrName" },
            { 0x03E8, "bs.arrZeitpL" },
            { 0x03F0, "bs.arrZeitpH" },
            { 0x03F9, "bs.semaVar" },
            { 0x1005, "bs.RestartTastatur" },
            { 0x1022, "bs.IRouCB2" },
            { 0x105D, "bs.IRouSR.1" },
            { 0x106E, "bs.IRouSR.2" },
            { 0x1079, "bs.IRouSR.3" },
            { 0x1090, "bs.IRouWait" },
            { 0x1180, "bs.Transfer3" },
            { 0x1200, "bs.RKV" },
            { 0x1240, "bs.RWL" },
            { 0x126A, "bs.NRWLSelbst" },
            { 0x126C, "bs.NRWLAC" },
            { 0x1286, "bs.RKVOderNicht" },
            { 0x12A3, "bs.IRouFortsetzg" },
            { 0x1300, "bs.HauptIRou" },
            { 0x1368, "bs.TimerIRou" },
            { 0x136B, "bs.PA7IRou" },
            { 0x1400, "VIA.ORB/IRB" },
            { 0x1401, "VIA.ORA/IRA" },
            { 0x1402, "VIA.DDRB" },
            { 0x1403, "VIA.DDRA" },
            { 0x1404, "VIA.T1CL/LL" },
            { 0x1405, "VIA.T1CH" },
            { 0x1406, "VIA.T1LL" },
            { 0x1407, "VIA.T1LH" },
            { 0x1408, "VIA.T2CL/LL" },
            { 0x1409, "VIA.T2CH" },
            { 0x140A, "VIA.SR" },
            { 0x140B, "VIA.ACR" },
            { 0x140C, "VIA.PCR" },
            { 0x140D, "VIA.IFR" },
            { 0x140E, "VIA.IER" },
            { 0x140F, "VIA.ORA/IRA.noHS" },
            { 0x1600, "floppy.??" },
            { 0x1601, "floppy.??" },
            { 0x1A00, "bs.help1" },
            { 0x1A02, "bs.help2" },
            { 0x1A04, "bs.currentInput" },
            { 0x1A06, "bs.currentOutput" },
            { 0x1A08, "bs.param.ROMStatusMaster" },
            { 0x1A09, "bs.param.ROMStatusSlave" },
            { 0x1A60, "bs.irous.VIA.CA2" },
            { 0x1A62, "bs.irous.VIA.CA1" },
            { 0x1A64, "bs.irous.VIA.SR" },
            { 0x1A66, "bs.irous.VIA.CB2" },
            { 0x1A68, "bs.irous.VIA.CB1" },
            { 0x1A6A, "bs.irous.VIA.Timer2" },
            { 0x1A6C, "bs.irous.VIA.Timer1" },
            { 0x1A74, "bs.irous.PIA.PA7" },
            { 0x1A76, "bs.irous.PIA.Timer" },
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
            { 0x1E60, "editor.OPLEN2" },
            { 0x1E86, "comfortableReset" },
            { 0x1ED3, "editor.BEGIN" },
            { 0x1EDC, "editor.ADCEND" },
            { 0x1EEA, "editor.RECEND" },
            { 0x1EF9, "editor.ADCUR" },
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

            { 0xFFF8, "bios.flag EPROM lo" },
            { 0xFFF9, "bios.flag EPROM med" },
            { 0xFFFA, "bios.flag EPROM hi/NMIadr" },
            { 0xFFFC, "RSTadr" },
            { 0xFFFE, "IRQadr" },
            { 0xFFFF, "bios.flag EPROM Bank" },
        };
    }
}