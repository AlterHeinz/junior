using System;
using System.Diagnostics;

namespace juniorassembler
{
    public class Instruction
    {
        private Instruction(int opCode, string name, AddressMode am)
        {
            data = new Tuple<int, string, AddressMode>(opCode, name, am);
        }

        private Tuple<int, string, AddressMode> data;

        public static Instruction find(byte opCode)
        {
            var ret = allInstructions[opCode];
            Debug.Assert(opCode == ret.data.Item1);
            return ret;
        }

        public int OpCode => data.Item1;

        public bool IsBranchInstruction => data.Item3 == AddressMode.rel;

        public bool IsZeroAddressing => (NoOfArgBytes == 1 && data.Item3 != AddressMode.im && data.Item3 != AddressMode.rel);

        public int NoOfBytes
        {
            get
            {
                switch (data.Item3)
                {
                    case AddressMode.NONE:
                        return 1;
                    case AddressMode.NONE3:
                        return 3;
                    case AddressMode.ix:
                    case AddressMode.iy:
                    case AddressMode.zp:
                    case AddressMode.zx:
                    case AddressMode.zy:
                    case AddressMode.im:
                        return 2;
                    case AddressMode.a:
                        return 1;
                    case AddressMode.abs:
                        return 3;
                    case AddressMode.rel:
                        return 2;
                    case AddressMode.x:
                    case AddressMode.y:
                        return 3;
                    case AddressMode.i:
                        return 3;
                    default:
                        return 1;
                }
            }
        }

        public int NoOfArgBytes
        {
            get
            {
                switch (data.Item3)
                {
                    case AddressMode.NONE3:
                        return 0;
                    default:
                        return NoOfBytes - 1;
                }
            }
        }

        public string Label
        {
            get
            {
                return data.Item2 + AddressModeLabel;
            }
        }

        private string AddressModeLabel
        {
            get
            {
                switch (data.Item3)
                {
                    case AddressMode.NONE:
                    case AddressMode.NONE3:
                    case AddressMode.abs:
                    case AddressMode.rel:
                        return "";
                    case AddressMode.zp:
                        return "z";
                    case AddressMode.ix:
                    case AddressMode.iy:
                    case AddressMode.zx:
                    case AddressMode.zy:
                    case AddressMode.im:
                    case AddressMode.a:
                    case AddressMode.x:
                    case AddressMode.y:
                    case AddressMode.i:
                        return data.Item3.ToString();
                    default:
                        return "?";
                }
            }
        }

        // inspired by links in de.wikipedia.org/wiki/MOS_Technology_6502
        private static readonly Instruction[] allInstructions = new Instruction[]
        {
            new Instruction(0x00, "BRK", AddressMode.NONE),
            new Instruction(0x01, "ORA", AddressMode.ix),
            new Instruction(0x02, "", AddressMode.NONE),
            new Instruction(0x03, "", AddressMode.NONE),
            new Instruction(0x04, "", AddressMode.NONE),
            new Instruction(0x05, "ORA", AddressMode.zp),
            new Instruction(0x06, "ASL", AddressMode.zp),
            new Instruction(0x07, "", AddressMode.NONE),
            new Instruction(0x08, "PHP", AddressMode.NONE),
            new Instruction(0x09, "ORA", AddressMode.im),
            new Instruction(0x0A, "ASL", AddressMode.a),
            new Instruction(0x0B, "", AddressMode.NONE),
            new Instruction(0x0C, "", AddressMode.NONE),
            new Instruction(0x0D, "ORA", AddressMode.abs),
            new Instruction(0x0E, "ASL", AddressMode.abs),
            new Instruction(0x0F, "", AddressMode.NONE),
            new Instruction(0x10, "BPL", AddressMode.rel),
            new Instruction(0x11, "ORA", AddressMode.iy),
            new Instruction(0x12, "", AddressMode.NONE),
            new Instruction(0x13, "", AddressMode.NONE),
            new Instruction(0x14, "", AddressMode.NONE),
            new Instruction(0x15, "ORA", AddressMode.zx),
            new Instruction(0x16, "ASL", AddressMode.zx),
            new Instruction(0x17, "", AddressMode.NONE),
            new Instruction(0x18, "CLC", AddressMode.NONE),
            new Instruction(0x19, "ORA", AddressMode.y),
            new Instruction(0x1A, "", AddressMode.NONE),
            new Instruction(0x1B, "", AddressMode.NONE),
            new Instruction(0x1C, "", AddressMode.NONE),
            new Instruction(0x1D, "ORA", AddressMode.x),
            new Instruction(0x1E, "ASL", AddressMode.x),
            new Instruction(0x1F, "", AddressMode.NONE),
            new Instruction(0x20, "JSR", AddressMode.abs),
            new Instruction(0x21, "AND", AddressMode.ix),
            new Instruction(0x22, "", AddressMode.NONE),
            new Instruction(0x23, "", AddressMode.NONE),
            new Instruction(0x24, "BIT", AddressMode.zp),
            new Instruction(0x25, "AND", AddressMode.zp),
            new Instruction(0x26, "ROL", AddressMode.zp),
            new Instruction(0x27, "", AddressMode.NONE),
            new Instruction(0x28, "PLP", AddressMode.NONE),
            new Instruction(0x29, "AND", AddressMode.im),
            new Instruction(0x2A, "ROL", AddressMode.a),
            new Instruction(0x2B, "", AddressMode.NONE),
            new Instruction(0x2C, "BIT", AddressMode.abs),
            new Instruction(0x2D, "AND", AddressMode.abs),
            new Instruction(0x2E, "ROL", AddressMode.abs),
            new Instruction(0x2F, "", AddressMode.NONE),
            new Instruction(0x30, "BMI", AddressMode.rel),
            new Instruction(0x31, "AND", AddressMode.iy),
            new Instruction(0x32, "", AddressMode.NONE),
            new Instruction(0x33, "", AddressMode.NONE),
            new Instruction(0x34, "", AddressMode.NONE),
            new Instruction(0x35, "AND", AddressMode.zx),
            new Instruction(0x36, "ROL", AddressMode.zx),
            new Instruction(0x37, "", AddressMode.NONE),
            new Instruction(0x38, "SEC", AddressMode.NONE),
            new Instruction(0x39, "AND", AddressMode.y),
            new Instruction(0x3A, "", AddressMode.NONE),
            new Instruction(0x3B, "", AddressMode.NONE),
            new Instruction(0x3C, "", AddressMode.NONE),
            new Instruction(0x3D, "ORA", AddressMode.x),
            new Instruction(0x3E, "ASL", AddressMode.x),
            new Instruction(0x3F, "", AddressMode.NONE),
            new Instruction(0x40, "RTI", AddressMode.NONE),
            new Instruction(0x41, "EOR", AddressMode.ix),
            new Instruction(0x42, "", AddressMode.NONE),
            new Instruction(0x43, "", AddressMode.NONE),
            new Instruction(0x44, "", AddressMode.NONE),
            new Instruction(0x45, "EOR", AddressMode.zp),
            new Instruction(0x46, "LSR", AddressMode.zp),
            new Instruction(0x47, "", AddressMode.NONE),
            new Instruction(0x48, "PHA", AddressMode.NONE),
            new Instruction(0x49, "EOR", AddressMode.im),
            new Instruction(0x4A, "LSR", AddressMode.a),
            new Instruction(0x4B, "", AddressMode.NONE),
            new Instruction(0x4C, "JMP", AddressMode.abs),
            new Instruction(0x4D, "EOR", AddressMode.abs),
            new Instruction(0x4E, "LSR", AddressMode.abs),
            new Instruction(0x4F, "", AddressMode.NONE),
            new Instruction(0x50, "BVC", AddressMode.rel),
            new Instruction(0x51, "EOR", AddressMode.iy),
            new Instruction(0x52, "", AddressMode.NONE),
            new Instruction(0x53, "", AddressMode.NONE),
            new Instruction(0x54, "", AddressMode.NONE),
            new Instruction(0x55, "EOR", AddressMode.zx),
            new Instruction(0x56, "LSR", AddressMode.zx),
            new Instruction(0x57, "", AddressMode.NONE),
            new Instruction(0x58, "CLI", AddressMode.NONE),
            new Instruction(0x59, "EOR", AddressMode.y),
            new Instruction(0x5A, "", AddressMode.NONE),
            new Instruction(0x5B, "", AddressMode.NONE),
            new Instruction(0x5C, "", AddressMode.NONE),
            new Instruction(0x5D, "EOR", AddressMode.x),
            new Instruction(0x5E, "LSR", AddressMode.x),
            new Instruction(0x5F, "", AddressMode.NONE),
            new Instruction(0x60, "RTS", AddressMode.NONE),
            new Instruction(0x61, "ADC", AddressMode.ix),
            new Instruction(0x62, "", AddressMode.NONE),
            new Instruction(0x63, "", AddressMode.NONE),
            new Instruction(0x64, "", AddressMode.NONE),
            new Instruction(0x65, "ADC", AddressMode.zp),
            new Instruction(0x66, "ROR", AddressMode.zp),
            new Instruction(0x67, "", AddressMode.NONE),
            new Instruction(0x68, "PLA", AddressMode.NONE),
            new Instruction(0x69, "ADC", AddressMode.im),
            new Instruction(0x6A, "ROR", AddressMode.a),
            new Instruction(0x6B, "", AddressMode.NONE),
            new Instruction(0x6C, "JMP", AddressMode.i),
            new Instruction(0x6D, "ADC", AddressMode.abs),
            new Instruction(0x6E, "ROR", AddressMode.abs),
            new Instruction(0x6F, "", AddressMode.NONE),
            new Instruction(0x70, "BCS", AddressMode.rel),
            new Instruction(0x71, "ADC", AddressMode.iy),
            new Instruction(0x72, "", AddressMode.NONE),
            new Instruction(0x73, "", AddressMode.NONE),
            new Instruction(0x74, "", AddressMode.NONE),
            new Instruction(0x75, "ADC", AddressMode.zx),
            new Instruction(0x76, "ROR", AddressMode.zx),
            new Instruction(0x77, "", AddressMode.NONE),
            new Instruction(0x78, "SEI", AddressMode.NONE),
            new Instruction(0x79, "ADC", AddressMode.y),
            new Instruction(0x7A, "", AddressMode.NONE),
            new Instruction(0x7B, "", AddressMode.NONE),
            new Instruction(0x7C, "", AddressMode.NONE),
            new Instruction(0x7D, "ADC", AddressMode.x),
            new Instruction(0x7E, "ROR", AddressMode.x),
            new Instruction(0x7F, "", AddressMode.NONE),
            new Instruction(0x80, "", AddressMode.NONE),
            new Instruction(0x81, "STA", AddressMode.ix),
            new Instruction(0x82, "", AddressMode.NONE),
            new Instruction(0x83, "", AddressMode.NONE),
            new Instruction(0x84, "STY", AddressMode.zp),
            new Instruction(0x85, "STA", AddressMode.zp),
            new Instruction(0x86, "STX", AddressMode.zp),
            new Instruction(0x87, "", AddressMode.NONE),
            new Instruction(0x88, "DEY", AddressMode.NONE),
            new Instruction(0x89, "", AddressMode.NONE),
            new Instruction(0x8A, "TXA", AddressMode.a),
            new Instruction(0x8B, "", AddressMode.NONE),
            new Instruction(0x8C, "STY", AddressMode.abs),
            new Instruction(0x8D, "STA", AddressMode.abs),
            new Instruction(0x8E, "STX", AddressMode.abs),
            new Instruction(0x8F, "", AddressMode.NONE),
            new Instruction(0x90, "BCC", AddressMode.rel),
            new Instruction(0x91, "STA", AddressMode.iy),
            new Instruction(0x92, "", AddressMode.NONE),
            new Instruction(0x93, "", AddressMode.NONE),
            new Instruction(0x94, "STY", AddressMode.zp),
            new Instruction(0x95, "STA", AddressMode.zx),
            new Instruction(0x96, "STX", AddressMode.zy),
            new Instruction(0x97, "", AddressMode.NONE),
            new Instruction(0x98, "TYA", AddressMode.NONE),
            new Instruction(0x99, "STA", AddressMode.y),
            new Instruction(0x9A, "TXS", AddressMode.NONE),
            new Instruction(0x9B, "", AddressMode.NONE),
            new Instruction(0x9C, "", AddressMode.NONE),
            new Instruction(0x9D, "STA", AddressMode.x),
            new Instruction(0x9E, "", AddressMode.NONE),
            new Instruction(0x9F, "", AddressMode.NONE),
            new Instruction(0xA0, "LDY", AddressMode.im),
            new Instruction(0xA1, "LDA", AddressMode.ix),
            new Instruction(0xA2, "LDX", AddressMode.im),
            new Instruction(0xA3, "", AddressMode.NONE),
            new Instruction(0xA4, "LDY", AddressMode.zp),
            new Instruction(0xA5, "LDA", AddressMode.zp),
            new Instruction(0xA6, "LDX", AddressMode.zp),
            new Instruction(0xA7, "", AddressMode.NONE),
            new Instruction(0xA8, "TAY", AddressMode.NONE),
            new Instruction(0xA9, "LDA", AddressMode.im),
            new Instruction(0xAA, "TAX", AddressMode.NONE),
            new Instruction(0xAB, "", AddressMode.NONE),
            new Instruction(0xAC, "LDY", AddressMode.abs),
            new Instruction(0xAD, "LDA", AddressMode.abs),
            new Instruction(0xAE, "LDX", AddressMode.abs),
            new Instruction(0xAF, "", AddressMode.NONE),
            new Instruction(0xB0, "BCS", AddressMode.rel),
            new Instruction(0xB1, "LDA", AddressMode.iy),
            new Instruction(0xB2, "", AddressMode.NONE),
            new Instruction(0xB3, "", AddressMode.NONE),
            new Instruction(0xB4, "LDY", AddressMode.zp),
            new Instruction(0xB5, "LDA", AddressMode.zx),
            new Instruction(0xB6, "LDX", AddressMode.zy),
            new Instruction(0xB7, "", AddressMode.NONE),
            new Instruction(0xB8, "CLV", AddressMode.NONE),
            new Instruction(0xB9, "LDA", AddressMode.y),
            new Instruction(0xBA, "TSX", AddressMode.NONE),
            new Instruction(0xBB, "", AddressMode.NONE),
            new Instruction(0xBC, "LDY", AddressMode.x),
            new Instruction(0xBD, "LDA", AddressMode.x),
            new Instruction(0xBE, "LDX", AddressMode.y),
            new Instruction(0xBF, "", AddressMode.NONE),
            new Instruction(0xC0, "CPY", AddressMode.im),
            new Instruction(0xC1, "CMP", AddressMode.ix),
            new Instruction(0xC2, "", AddressMode.NONE),
            new Instruction(0xC3, "", AddressMode.NONE),
            new Instruction(0xC4, "CPY", AddressMode.zp),
            new Instruction(0xC5, "CMP", AddressMode.zp),
            new Instruction(0xC6, "DEC", AddressMode.zp),
            new Instruction(0xC7, "", AddressMode.NONE),
            new Instruction(0xC8, "INY", AddressMode.NONE),
            new Instruction(0xC9, "CMP", AddressMode.im),
            new Instruction(0xCA, "DEX", AddressMode.NONE),
            new Instruction(0xCB, "", AddressMode.NONE),
            new Instruction(0xCC, "CPY", AddressMode.abs),
            new Instruction(0xCD, "CMP", AddressMode.abs),
            new Instruction(0xCE, "DEC", AddressMode.abs),
            new Instruction(0xCF, "", AddressMode.NONE),
            new Instruction(0xD0, "BNE", AddressMode.rel),
            new Instruction(0xD1, "CMP", AddressMode.iy),
            new Instruction(0xD2, "", AddressMode.NONE),
            new Instruction(0xD3, "", AddressMode.NONE),
            new Instruction(0xD4, "", AddressMode.NONE),
            new Instruction(0xD5, "CMP", AddressMode.zx),
            new Instruction(0xD6, "DEC", AddressMode.zx),
            new Instruction(0xD7, "", AddressMode.NONE),
            new Instruction(0xD8, "CLD", AddressMode.NONE),
            new Instruction(0xD9, "CMP", AddressMode.y),
            new Instruction(0xDA, "", AddressMode.NONE),
            new Instruction(0xDB, "", AddressMode.NONE),
            new Instruction(0xDC, "", AddressMode.NONE),
            new Instruction(0xDD, "CMP", AddressMode.x),
            new Instruction(0xDE, "DEC", AddressMode.x),
            new Instruction(0xDF, "", AddressMode.NONE),
            new Instruction(0xE0, "CPX", AddressMode.im),
            new Instruction(0xE1, "SBC", AddressMode.ix),
            new Instruction(0xE2, "", AddressMode.NONE),
            new Instruction(0xE3, "", AddressMode.NONE),
            new Instruction(0xE4, "CPX", AddressMode.zp),
            new Instruction(0xE5, "SBC", AddressMode.zp),
            new Instruction(0xE6, "INC", AddressMode.zp),
            new Instruction(0xE7, "", AddressMode.NONE),
            new Instruction(0xE8, "INX", AddressMode.NONE),
            new Instruction(0xE9, "SBC", AddressMode.im),
            new Instruction(0xEA, "NOP", AddressMode.NONE),
            new Instruction(0xEB, "SBC", AddressMode.im),
            new Instruction(0xEC, "CPX", AddressMode.abs),
            new Instruction(0xED, "SBC", AddressMode.abs),
            new Instruction(0xEE, "INC", AddressMode.abs),
            new Instruction(0xEF, "", AddressMode.NONE),
            new Instruction(0xF0, "BEQ", AddressMode.rel),
            new Instruction(0xF1, "SBC", AddressMode.iy),
            new Instruction(0xF2, "", AddressMode.NONE),
            new Instruction(0xF3, "", AddressMode.NONE),
            new Instruction(0xF4, "", AddressMode.NONE),
            new Instruction(0xF5, "SBC", AddressMode.zx),
            new Instruction(0xF6, "INC", AddressMode.zx),
            new Instruction(0xF7, "", AddressMode.NONE),
            new Instruction(0xF8, "SED", AddressMode.NONE),
            new Instruction(0xF9, "SBC", AddressMode.y),
            new Instruction(0xFA, "", AddressMode.NONE),
            new Instruction(0xFB, "", AddressMode.NONE),
            new Instruction(0xFC, "", AddressMode.NONE),
            new Instruction(0xFD, "SBC", AddressMode.x),
            new Instruction(0xFE, "INC", AddressMode.x),
            new Instruction(0xFF, "", AddressMode.NONE3)
        };
    }
}
