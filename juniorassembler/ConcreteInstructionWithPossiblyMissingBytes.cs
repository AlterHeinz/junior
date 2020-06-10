namespace juniorassembler
{
    public class ConcreteInstructionWithPossiblyMissingBytes
    {
        public ConcreteInstructionWithPossiblyMissingBytes(ConcreteInstruction instr, int missingArgBytes)
        {
            Instr = instr;
            MissingArgBytes = missingArgBytes;
        }

        public ConcreteInstruction Instr { get; private set; }
        public int MissingArgBytes { get; private set; }
    }
}
