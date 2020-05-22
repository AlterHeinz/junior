namespace juniorassembler
{
    // a concrete instruction consisting of an address, OpCode and up to 2 argument bytes.
    public class ConcreteInstruction
    {
        public ConcreteInstruction(int address, byte opCode)
        {
            this.address = address;
            instruction = Instruction.find(opCode);
        }

        public int Address => address;
        public byte OpCode => (byte)instruction.OpCode;
        public string Label => instruction.Label;
        public int NoOfBytes => instruction.NoOfBytes;
        public byte Arg1 { get; set; }
        public byte Arg2 { get; set; }

        public string GetArg1OrBranchDestination(int startAddr)
        {
            if (instruction.IsBranchInstruction)
            {
                sbyte offset = (sbyte)Arg1;
                int destination = startAddr + address + 2 + offset;
                return string.Format("{0}{1}>{2:X2}", offset >= 0 ? "+" : "", offset, destination & 0xFF);
            }
            else
                return string.Format("{0:X2}", Arg1);
        }

        private int address;
        private Instruction instruction;
    }
}
