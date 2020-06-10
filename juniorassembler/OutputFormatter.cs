﻿using System;
using System.Diagnostics;
using System.IO;

namespace juniorassembler
{
    // formats one ConcreteInstruction and writes it out
    internal class OutputFormatter : IObserver<ConcreteInstructionWithPossiblyMissingBytes>
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

        public void OnNext(ConcreteInstructionWithPossiblyMissingBytes value)
        {
            ConcreteInstruction instr = value.Instr;

            // extra line for function symbol?
            if (verbose)
            {
                string knownSymbol = dualScope.findCallerSymbol(CalcRealAddr(instr));
                if (knownSymbol != null)
                    output.WriteLine("{0:X4}: ------ {1}", CalcRealAddr(instr), knownSymbol);
            }

            string format = FormatDisassembledPart(value);

            if (format != null)
            {
                if (verbose)
                    format = PrependHexBytes(value) + format;
                Forward(format, instr);
            }
        }

        private ushort CalcRealAddr(ConcreteInstruction instr) => (ushort)(startAddr + instr.Address);

        private string FormatDisassembledPart(ConcreteInstructionWithPossiblyMissingBytes value)
        {
            ConcreteInstruction instr = value.Instr;
            int missingArgBytes = value.MissingArgBytes;

            switch (instr.NoOfArgBytes)
            {
                case 0:
                    return "{0}";
                case 1:
                    if (0 == missingArgBytes)
                    {
                        if (verbose && instr.IsBranchInstruction)
                            // special case for branch instructions
                            return "{0} {5}";
                        else if (verbose && instr.IsZeroAddressing)
                            return "{0} {1:X2}{6}"; // special postfix for instructions with symbols
                        else
                            return "{0} {1:X2}";
                    }
                    else
                        return "{0} ??";
                case 2:
                    if (0 == missingArgBytes)
                        return "{0} {2:X2}{1:X2}{6}"; // special postfix for instructions with symbols
                    else if (1 == missingArgBytes)
                        return "{0} ??{1:X2}";
                    else
                        return "{0} ????";
                default:
                    Debug.Fail("bad instruction.NoOfArgBytes");
                    return null;
            }
        }

        private string PrependHexBytes(ConcreteInstructionWithPossiblyMissingBytes value)
        {
            int missingArgBytes = value.MissingArgBytes;

            switch (value.Instr.NoOfBytes)
            {
                case 1:
                    return "{4:X4}: {3:X2}     ";
                case 2:
                    if (0 == missingArgBytes)
                        return "{4:X4}: {3:X2}{1:X2}   ";
                    else
                        return "{4:X4}: {3:X2}??   ";
                case 3:
                    if (0 == missingArgBytes)
                        return "{4:X4}: {3:X2}{1:X2}{2:X2} ";
                    if (1 == missingArgBytes)
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
            if (verbose && instr.NoOfArgBytes > 0)
            {
                string postfix = dualScope.findUsedSymbol(instr.Argument);
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
        private IDualScope dualScope;
    }
}
