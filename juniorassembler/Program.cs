using System;
using System.IO;

namespace juniorassembler
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Error.WriteLine("Hi, i'm juniorassembler");
            if (!(args.Length >= 1 && (   (args[0] == "-d"  && args.Length <= 2)
                                       || (args[0] == "-dv" && args.Length >= 2 && args[1].Length == 4 && args.Length <= 3))))
            {
                Console.Error.WriteLine("Usage: juniorassembler [-d | -dv <startaddrinHex4>] [<binary file to read>]");
                return;
            }

            bool verbose = args[0] == "-dv";
            string startAddr = (verbose && args.Length >= 2) ? args[1] : "";
            bool withoutFilename = args.Length == 1 || (verbose && args.Length == 2);
            if (withoutFilename)
            {
                var chain = new HexCharCombiner(new OperationCombiner(new OutputFormatter(Console.Out, verbose, startAddr)));
                for (int c = Console.Read(); c != -1; c = Console.Read())
                    chain.OnNext((char)c);
                chain.OnCompleted();
            }
            else
            {
                string filename = verbose ? args[2]: args[1];
                byte[] data = File.ReadAllBytes(filename);
                var chain = new OperationCombiner(new OutputFormatter(Console.Out, verbose, startAddr));
                foreach (byte b in data)
                    chain.OnNext(b);
                chain.OnCompleted();
            }
            Console.Error.WriteLine("bye.");
        }
    }
}
