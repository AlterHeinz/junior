using System;
using System.IO;

namespace juniorassembler
{
    public static class Iterators
    {
        public static void IterateTextChars(TextReader rdr, IObserver<char> output)
        {
            for (int c = rdr.Read(); c != -1; c = rdr.Read())
                output.OnNext((char)c);
            output.OnCompleted();
        }

        public static void IterateFileBytes(string fileName, IObserver<byte> output)
        {
            byte[] data = File.ReadAllBytes(fileName);
            foreach (byte b in data)
                output.OnNext(b);
            output.OnCompleted();
        }
    }
}
