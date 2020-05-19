using System;
using System.Diagnostics;
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
                Console.Error.WriteLine("pos {0}: obsolete final byte: {1:X2}", pos, instruction.OpCode);
                if (2 == instruction.NoOfBytes)
                    Forward("{0} ??");
                else
                    Forward("{0} ????");
            }
            else if (innerPos == 2)
            {
                Console.Error.WriteLine("pos {0}: obsolete final bytes: {1:X2} {2:X2}", pos, instruction.OpCode, arg1);
                Forward("{0} ??{1:X2}");
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
                instruction = Instruction.find(value);
                innerPos++;
            }
            else if (innerPos == 1)
            {
                arg1 = value;
                innerPos++;
            }
            else
            {
                arg2 = value;
                innerPos++;
            }

            if (innerPos == instruction.NoOfBytes)
            {
                switch (innerPos)
                {
                    case 1:
                        Forward("{0}");
                        break;
                    case 2:
                        Forward("{0} {1:X2}");
                        break;
                    case 3:
                        Forward("{0} {2:X2}{1:X2}");
                        break;
                    default:
                        //Debug.Fail("bad innerPos");
                        break;
                }
                pos++;
                innerPos = 0;
            }
        }

        private void Forward(string format)
        {
            output.WriteLine(format, instruction.Label, arg1, arg2);
        }

        private readonly TextWriter output;
        private Instruction instruction;
        private byte arg1;
        private byte arg2;
        private int innerPos = 0;
        private int pos = 0;
    }
}
