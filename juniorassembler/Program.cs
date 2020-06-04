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
            IScope scope = Scopes.OfBank1;
            if (startAddr.Length == 6)
            {
                startAddr = startAddr.Substring(0, 4);
                scope = Scopes.OfBank2;
            }
                
            bool withoutFilename = args.Length == 1 || (verbose && args.Length == 2);
            if (withoutFilename)
            {
                var filterPipeline = new HexCharCombiner(new OperationCombiner(new OutputFormatter(Console.Out, verbose, startAddr, scope)));
                Iterators.IterateTextChars(Console.In, filterPipeline);
            }
            else
            {
                string filename = verbose ? args[2]: args[1];
                var filterPipeline = new OperationCombiner(new OutputFormatter(Console.Out, verbose, startAddr, scope));
                Iterators.IterateFileBytes(filename, filterPipeline);
            }
            Console.Error.WriteLine("bye.");
        }
    }
}
