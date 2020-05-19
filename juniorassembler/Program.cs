using System;
using System.IO;

namespace juniorassembler
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Error.WriteLine("Hi, i'm juniorassembler");
            if (!((args.Length == 1 || args.Length == 2) && args[0] == "-d"))
            {
                Console.Error.WriteLine("Usage: juniorCopy -d [<binary file to read>]");
                return;
            }

            if (args.Length == 1)
            {
                var chain = new HexCharCombiner(new OperationProcessor(Console.Out));
                for (int c = Console.Read(); c != -1; c = Console.Read())
                    chain.OnNext((char)c);
                chain.OnCompleted();
            }
            else
            {
                byte[] data = File.ReadAllBytes(args[1]);
                var chain = new OperationProcessor(Console.Out);
                foreach (byte b in data)
                    chain.OnNext(b);
                chain.OnCompleted();
            }
            Console.Error.WriteLine("bye.");
        }
    }
}
