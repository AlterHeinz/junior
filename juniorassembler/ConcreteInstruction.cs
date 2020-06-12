using System;
using System.Collections.Generic;

namespace juniorassembler
{
    // a concrete instruction consisting of an address, OpCode and up to 2 argument bytes.
    public class ConcreteInstruction : DataBlock
    {
        public ConcreteInstruction(ushort address, byte opCode)
        {
            Address = address;
            instruction = Instruction.find(opCode);
            CurNoOfBytes = 1;
        }

        public ushort Address { get; }
        public IEnumerable<byte> Bytes
        {
            get
            {
                yield return OpCode;
                if (CurNoOfBytes >= 2)
                    yield return Arg1;
                if (CurNoOfBytes >= 3)
                    yield return Arg2;
            }
        }
        public string Text => IsTextDataLabel ? "---- text data" : instruction.Label;
        public ushort CurNoOfBytes { get; private set; }

        public void Append(byte value)
        {
            if (CurNoOfBytes == 1)
                Arg1 = value;
            else
                Arg2 = value;
            CurNoOfBytes++;
        }

        public byte OpCode => (byte)instruction.OpCode;
        public ushort NoOfBytesDesired => (ushort)instruction.NoOfBytes;
        public ushort NoOfArgBytes => (ushort)instruction.NoOfArgBytes;
        public bool IsBranchInstruction => instruction.IsBranchInstruction;
        public bool IsZeroAddressing => instruction.IsZeroAddressing;
        public byte Arg1 { get; private set; }
        public byte Arg2 { get; private set; }
        public bool IsComplete => CurNoOfBytes == NoOfBytesDesired;
        public int MissingBytes => NoOfBytesDesired - CurNoOfBytes;
        public ushort Argument => (ushort)((NoOfArgBytes == 2) ? (Arg2 << 8 | Arg1) : (NoOfArgBytes == 1) ? Arg1 : 0);
        public bool IsTextDataLabel => instruction.OpCode == 0xFF && Arg1 == 0xFF && Arg2 == 0x02;

        private Instruction instruction;
    }
}
