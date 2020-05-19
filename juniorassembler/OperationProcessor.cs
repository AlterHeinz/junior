using System;
using System.IO;

namespace juniorassembler
{
    // combines up to 3 consecutive bytes to an operation and outputs it.
    public class OperationProcessor : IObserver<byte>
    {
        public OperationProcessor(TextWriter output)
        {
            this.output = output;
        }

        public void OnCompleted()
        {
            if (innerPos == 1)
            {
                Console.Error.WriteLine("pos {0}: obsolete final byte: {1:X2}", pos, opCode);
                Forward("{0} ????", instruction.GetLabel());
            }
            else if (innerPos == 2)
            {
                Console.Error.WriteLine("pos {0}: obsolete final bytes: {1:X2} {2:X2}", pos, opCode, arg1);
                Forward("{0} ??{1:X2}", instruction.GetLabel(), arg1);
            }
        }

        public void OnError(Exception error)
        {
            throw new NotImplementedException();
        }

        public void OnNext(byte value)
        {
            if (innerPos == 0)
            {
                opCode = value;
                instruction = Instruction.allInstructions[opCode];
                innerPos++;
            }
            else if (innerPos == 1)
            {
                arg1 = value;
                innerPos++;
            }
            else
                innerPos++;

            if (innerPos == instruction.GetNoOfBytes())
            {
                Forward(Combine(instruction.GetLabel(), arg1, value, innerPos));
                pos++;
                innerPos = 0;
            }
        }

        public static string Combine(string opCode, byte arg1, byte arg2, int innerPos)
        {
            switch (innerPos)
            {
                case 1:
                    return string.Format("{0}", opCode ?? "??");
                case 2:
                    return string.Format("{0} {1:X2}", opCode ?? "??", arg1);
                case 3:
                    return string.Format("{0} {1:X2}{2:X2}", opCode ?? "??", arg2, arg1);
                default:
                    return "?#?#?";
            }
        }

        private void Forward(string format, params object[] args)
        {
            output.WriteLine(format, args);
        }

        private readonly TextWriter output;
        private byte opCode;
        private Instruction instruction;
        private byte arg1;
        private int innerPos = 0;
        private int pos = 0;
    }
}
