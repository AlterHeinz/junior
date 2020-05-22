using System;
using System.Diagnostics;
using System.IO;

namespace juniorassembler
{
    // formats one ConcreteInstruction and writes it out
    public class OutputFormatter : IObserver<Tuple<ConcreteInstruction, int>>
    {
        public OutputFormatter(TextWriter output, bool verbose, string startAddr)
        {
            this.output = output;
            this.verbose = verbose;
            this.startAddr = startAddr.Length > 0 ? Convert.ToUInt16(startAddr, 16) : (ushort)0;
        }

        public void OnCompleted()
        {
            throw new NotImplementedException();
        }

        public void OnError(Exception error)
        {
            throw new NotImplementedException();
        }

        public void OnNext(Tuple<ConcreteInstruction, int> value)
        {
            ConcreteInstruction instr = value.Item1;
            int innerPos = value.Item2;
            string format = null;
            switch (instr.NoOfBytes)
            {
                case 1:
                    format = "{0}";
                    break;
                case 2:
                    if (2 == innerPos)
                        if (verbose)
                            // special case for branch instructions
                            format = "{0} {5}";
                        else
                            format = "{0} {1:X2}";
                    else
                        format = "{0} ??";
                    break;
                case 3:
                    if (3 == innerPos)
                        format = "{0} {2:X2}{1:X2}";
                    else if (2 == innerPos)
                        format = "{0} ??{1:X2}";
                    else
                        format = "{0} ????";
                    break;
                default:
                    Debug.Fail("bad instruction.NoOfBytes");
                    break;
            }

            if (format != null)
            {
                if (verbose)
                    format = PrependHexBytes(instr, innerPos) + format;
                Forward(format, instr);
            }
        }

        private string PrependHexBytes(ConcreteInstruction instr, int innerPos)
        {
            switch (instr.NoOfBytes)
            {
                case 1:
                    return "{4:X4}: {3:X2}     ";
                case 2:
                    if (2 == innerPos)
                        return "{4:X4}: {3:X2}{1:X2}   ";
                    else
                        return "{4:X4}: {3:X2}??   ";
                case 3:
                    if (3 == innerPos)
                        return "{4:X4}: {3:X2}{1:X2}{2:X2} ";
                    if (2 == innerPos)
                        return "{4:X4}: {3:X2}{1:X2}?? ";
                    return "{4:X4}: {3:X2}???? ";
                default:
                    Debug.Fail("bad instruction.NoOfBytes");
                    return "       ";
            }
        }

        private void Forward(string format, ConcreteInstruction instr)
        {
            output.WriteLine(format, instr.Label, instr.Arg1, instr.Arg2, instr.OpCode, startAddr + instr.Address, instr.GetArg1OrBranchDestination(startAddr));
        }

        private readonly TextWriter output;
        private bool verbose;
        private ushort startAddr;
    }
}
