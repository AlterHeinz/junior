using System;

namespace juniorassembler
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Error.WriteLine("Hi, i'm juniorassembler");
            if (!(args.Length >= 1 && (   (args[0] == "-d"  && args.Length <= 2)
                                       || (args[0] == "-dv" && args.Length >= 2 && (args[1].Length == 4 || args[1].Length == 6) && args.Length <= 3))))
            {
                Console.Error.WriteLine("Usage: juniorassembler [-d | -dv <startaddrinHex4>[B2] ] [<binary file to read>]");
                return;
            }

            bool verbose = args[0] == "-dv";
            string startAddr = (verbose && args.Length >= 2) ? args[1] : "";
            IDualScope dualScope = Scopes.OfBank1;
            if (startAddr.Length == 6)
            {
                startAddr = startAddr.Substring(0, 4);
                dualScope = Scopes.OfBank2;
            }
                
            var filterPipeline = new OperationCombiner(new OutputFormatter(Console.Out, verbose, startAddr, dualScope));
            bool withoutFilename = args.Length == 1 || (verbose && args.Length == 2);
            if (withoutFilename)
            {
                Iterators.IterateTextChars(Console.In, new HexCharCombiner(filterPipeline));
            }
            else
            {
                string filename = verbose ? args[2]: args[1];
                Iterators.IterateFileBytes(filename, filterPipeline);
            }
            Console.Error.WriteLine("bye.");
        }
    }
}
