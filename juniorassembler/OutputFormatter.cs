using System;
using System.Diagnostics;
using System.IO;

namespace juniorassembler
{
    // formats one ConcreteInstruction and writes it out
    internal class OutputFormatter : IObserver<DataBlock>
    {
        public OutputFormatter(TextWriter output, bool verbose, string startAddr, IDualScope dualScope)
        {
            this.output = output;
            this.verbose = verbose;
            this.startAddr = startAddr.Length > 0 ? Convert.ToUInt16(startAddr, 16) : (ushort)0;
            this.dualScope = dualScope;
        }

        public void OnCompleted()
        {
            throw new NotImplementedException();
        }

        public void OnError(Exception error)
        {
            throw new NotImplementedException();
        }

        public void OnNext(DataBlock value)
        {
            // extra line for function symbol?
            if (verbose)
            {
                string knownSymbol = dualScope.findCallerSymbol(CalcRealAddr(value));
                if (knownSymbol != null)
                    output.WriteLine("{0:X4}: ------ {1}", CalcRealAddr(value), knownSymbol);
            }

            if (verbose)
                PrependHexBytes(value);

            if (value is TextDataBlock)
                output.WriteLine("'{0}'", value.Text);
            else
            {
                output.Write("{0}", value.Text);

                var instr = value as ConcreteInstruction;
                OutputDisassembledPart(instr);
            }
        }

        private ushort CalcRealAddr(DataBlock value) => (ushort)(startAddr + value.Address);

        private void OutputDisassembledPart(ConcreteInstruction instr)
        {
            int missingArgBytes = instr.MissingBytes;

            switch (instr.NoOfArgBytes)
            {
                case 0:
                    output.WriteLine("");
                    break;
                case 1:
                    if (0 == missingArgBytes)
                    {
                        if (verbose && instr.IsBranchInstruction)
                            // special case for branch instructions
                            output.WriteLine(" {0}", GetArg1OrBranchDestination(instr));
                        else if (verbose && instr.IsZeroAddressing)
                            // special postfix for instructions with symbols
                            output.WriteLine(" {0:X2}{1}", instr.Arg1, GetSymbolPostfix(instr));
                        else
                            output.WriteLine(" {0:X2}", instr.Arg1);
                    }
                    else
                        output.WriteLine(" ??");
                    break;
                case 2:
                    if (0 == missingArgBytes)
                        // special postfix for instructions with symbols
                        output.WriteLine(" {1:X2}{0:X2}{2}", instr.Arg1, instr.Arg2, GetSymbolPostfix(instr));
                    else if (1 == missingArgBytes)
                        output.WriteLine(" ??{0:X2}", instr.Arg1);
                    else
                        output.WriteLine(" ????");
                    break;
                default:
                    Debug.Fail("bad instruction.NoOfArgBytes");
                    output.WriteLine("?????");
                    break;
            }
        }

        private void PrependHexBytes(DataBlock value)
        {
            output.Write("{0:X4}: ", CalcRealAddr(value));
            int n = 0;
            foreach (byte b in value.Bytes)
            {
                output.Write("{0:X2}", b);
                if ((++n & 3) == 0)
                    output.Write(" ");
            }

            if (value is ConcreteInstruction)
            {
                var instr = value as ConcreteInstruction;

                for (int i = 0; i < instr.MissingBytes; i++)
                    output.Write("??");
                for (int i = instr.NoOfBytesDesired; i < 3; i++)
                    output.Write("  ");
                output.Write(" ");
            }
            else
                output.Write(" ");
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
            if (verbose && instr.NoOfArgBytes > 0)
            {
                string postfix = dualScope.findUsedSymbol(instr.Argument);
                return postfix == null ? "" : " " + postfix;
            }
            return "";
        }

        private readonly TextWriter output;
        private bool verbose;
        private ushort startAddr;
        private IDualScope dualScope;
    }
}
