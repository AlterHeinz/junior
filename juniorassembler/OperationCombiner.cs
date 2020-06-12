using System;

namespace juniorassembler
{
    // combines up to 3 consecutive bytes to an operation and outputs it.
    public class OperationCombiner : IObserver<byte>
    {
        public OperationCombiner(IObserver<DataBlock> output)
        {
            this.output = output;
        }

        public void OnCompleted()
        {
            if (IsInTextDataMode)
                FlushTextDataAndLeaveTextDataMode();
            else
            {
                if (instr != null)
                {
                    if (instr.CurNoOfBytes == 1)
                        Console.Error.WriteLine("posStart {0}: obsolete final byte: {1:X2}", posStart, instr.OpCode);
                    else
                        Console.Error.WriteLine("posStart {0}: obsolete final bytes: {1:X2} {2:X2}", posStart, instr.OpCode, instr.Arg1);
                    FlushInstruction();
                }
            }
        }

        public void OnError(Exception error)
        {
            throw new NotImplementedException();
        }

        public void OnNext(byte value)
        {
            if (IsInTextDataMode)
            {
                if (TextDataBlock.isPrintable(value))
                    textDataBlock.Append(value);
                else
                {
                    FlushTextDataAndLeaveTextDataMode();
                    // recurse to processs value now
                    OnNext(value);
                }
            }
            else
            {
                if (instr == null)
                    instr = new ConcreteInstruction(posStart, value);
                else
                    instr.Append(value);

                if (instr.IsComplete)
                {
                    FlushInstruction();
                    if (instr.IsTextDataLabel)
                        EnterTextDataMode();
                    instr = null;
                }
            }
        }

        private bool IsInTextDataMode => textDataBlock != null;

        private void EnterTextDataMode()
        {
            textDataBlock = new TextDataBlock(posStart);
        }

        private void FlushTextDataAndLeaveTextDataMode()
        {
            if (textDataBlock.CurNoOfBytes > 0)
                output.OnNext(textDataBlock);
            posStart += textDataBlock.CurNoOfBytes;
            textDataBlock = null;
        }

        private void FlushInstruction()
        {
            output.OnNext(instr);
            posStart += instr.CurNoOfBytes;
        }

        private readonly IObserver<DataBlock> output;
        private ushort posStart = 0;
        private ConcreteInstruction instr = null;
        private TextDataBlock textDataBlock = null;
    }
}
