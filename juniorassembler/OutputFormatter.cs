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

            // extra line for function symbol?
            if (verbose)
            {
                string knownSymbol = SymbolMap.find(CalcRealAddr(instr));
                if (knownSymbol != null)
                    output.WriteLine("{0:X4}: ------ {1}", CalcRealAddr(instr), knownSymbol);
            }

            string format = FormatDisassembledPart(instr, innerPos);

            if (format != null)
            {
                if (verbose)
                    format = PrependHexBytes(instr, innerPos) + format;
                Forward(format, instr);
            }
        }

        private int CalcRealAddr(ConcreteInstruction instr) =>  startAddr + instr.Address;

        private string FormatDisassembledPart(ConcreteInstruction instr, int innerPos)
        {
            switch (instr.NoOfBytes)
            {
                case 1:
                    return "{0}";
                case 2:
                    if (2 == innerPos)
                        if (verbose)
                            // special case for branch instructions
                            return "{0} {5}";
                        else
                            return "{0} {1:X2}";
                    else
                        return "{0} ??";
                case 3:
                    if (3 == innerPos)
                        return "{0} {2:X2}{1:X2}{6}"; // special postfix for instructions with symbols
                    else if (2 == innerPos)
                        return "{0} ??{1:X2}";
                    else
                        return "{0} ????";
                default:
                    Debug.Fail("bad instruction.NoOfBytes");
                    return null;
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

        public string GetArg1OrBranchDestination(ConcreteInstruction instr)
        {
            if (instr.IsBranchInstruction)
            {
                sbyte offset = (sbyte)instr.Arg1;
                int destination = CalcRealAddr(instr) + 2 + offset;
                return string.Format("{0}{1}>{2:X2}", offset >= 0 ? "+" : "", offset, destination & 0xFF);
            }
            else
                return string.Format("{0:X2}", instr.Arg1);
        }

        public string GetSymbolPostfix(ConcreteInstruction instr)
        {
            if (verbose && instr.NoOfBytes == 3)
            {
                string postfix = SymbolMap.find(instr.Argument);
                return postfix == null ? "" : " " + postfix;
            }
            return "";
        }

        private void Forward(string format, ConcreteInstruction instr)
        {
            output.WriteLine(format, instr.Label, instr.Arg1, instr.Arg2, instr.OpCode, CalcRealAddr(instr),
                             GetArg1OrBranchDestination(instr), GetSymbolPostfix(instr));
        }

        private readonly TextWriter output;
        private bool verbose;
        private ushort startAddr;
    }
}
