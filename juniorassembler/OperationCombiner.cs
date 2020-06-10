using System;

namespace juniorassembler
{
    // combines up to 3 consecutive bytes to an operation and outputs it.
    public class OperationCombiner : IObserver<byte>
    {
        public OperationCombiner(IObserver<ConcreteInstructionWithPossiblyMissingBytes> output)
        {
            this.output = output;
        }

        public void OnCompleted()
        {
            if (innerPos > 0)
            {
                if (innerPos == 1)
                    Console.Error.WriteLine("posStart {0}: obsolete final byte: {1:X2}", posStart, instr.OpCode);
                else
                    Console.Error.WriteLine("posStart {0}: obsolete final bytes: {1:X2} {2:X2}", posStart, instr.OpCode, instr.Arg1);
                output.OnNext(new ConcreteInstructionWithPossiblyMissingBytes(instr, instr.NoOfBytes - innerPos));
            }
        }

        public void OnError(Exception error)
        {
            throw new NotImplementedException();
        }

        public void OnNext(byte value)
        {
            if (innerPos == 0)
                instr = new ConcreteInstruction(posStart, value);
            else if (innerPos == 1)
                instr.Arg1 = value;
            else
                instr.Arg2 = value;
            innerPos++;

            if (innerPos == instr.NoOfBytes)
            {
                output.OnNext(new ConcreteInstructionWithPossiblyMissingBytes(instr, 0));
                posStart += innerPos;
                innerPos = 0;
            }
        }

        private readonly IObserver<ConcreteInstructionWithPossiblyMissingBytes> output;
        private ConcreteInstruction instr;
        private int innerPos = 0;
        private int posStart = 0;
    }
}
