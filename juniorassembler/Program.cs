using System;
using System.IO;

namespace juniorassembler
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Error.WriteLine("Hi, i'm juniorassembler");
            if (!((args.Length == 1 || args.Length == 2) && (args[0] == "-d" || args[0] == "-dv")))
            {
                Console.Error.WriteLine("Usage: juniorassembler -d [<binary file to read>]");
                return;
            }

            bool verbose = args[0] == "-dv";
            if (args.Length == 1)
            {
                var chain = new HexCharCombiner(new OperationCombiner(new OutputFormatter(Console.Out, verbose)));
                for (int c = Console.Read(); c != -1; c = Console.Read())
                    chain.OnNext((char)c);
                chain.OnCompleted();
            }
            else
            {
                byte[] data = File.ReadAllBytes(args[1]);
                var chain = new OperationCombiner(new OutputFormatter(Console.Out, verbose));
                foreach (byte b in data)
                    chain.OnNext(b);
                chain.OnCompleted();
            }
            Console.Error.WriteLine("bye.");
        }
    }
}
