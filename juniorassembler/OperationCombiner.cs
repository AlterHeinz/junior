using System;
using System.Collections.Generic;

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
            if (isInTextDataMode)
            {
                if (isPrintable(value))
                {
                    textDataBytes.Add(value);
                    textData += (0x1D == value || 0xFF == value) ? '?' : Convert.ToChar(value);
                }
                else
                {
                    if (textData.Length > 0)
                        output.OnNext(new TextDataBlock(posStart, textDataBytes, textData));
                    posStart += textData.Length;
                    LeaveTextDataMode();
                    // recurse
                    OnNext(value);
                }
            }
            else
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
                    if (instr.IsTextDataLabel)
                        EnterTextDataMode();
                }
            }
        }

        private bool isPrintable(byte value)
        {
            return 0x1D == value || (0x20 <= value && value <= 0x5E) || (0x60 <= value && value <= 0x7F) || 0xFF == value;
        }

        private void EnterTextDataMode()
        {
            isInTextDataMode = true;
            textDataBytes = new List<byte>();
            textData = "";
        }

        private void LeaveTextDataMode()
        {
            isInTextDataMode = false;
            textDataBytes = new List<byte>();
            textData = "";
        }

        private readonly IObserver<ConcreteInstructionWithPossiblyMissingBytes> output;
        private ConcreteInstruction instr;
        private int innerPos = 0;
        private int posStart = 0;
        private bool isInTextDataMode = false;
        private List<byte> textDataBytes;
        private string textData;
    }
}
