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
        public bool IsBranchInstruction => instruction.IsBranchInstruction;
        public byte Arg1 { get; set; }
        public byte Arg2 { get; set; }

        private int address;
        private Instruction instruction;
    }
}
