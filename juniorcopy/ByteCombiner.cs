using System;
using System.IO;

namespace juniorcopy
{
    // checks 2 consecutive 7-bit-bytes for consistency, 
    // combines them to an 8-bit-byte (like 0x54 and 0x29 -> 0xA9) and writes that to the output stream
    public class ByteCombiner : IObserver<byte>
    {
        public ByteCombiner(Stream output)
        {
            this.output = output;
        }

        public void OnCompleted()
        {
            if (!atFirst)
            {
                Console.Error.WriteLine("pos 0x{0:X}: obsolete final value: {1:X2}", pos, first);
                Forward(first);
            }
        }

        public void OnError(Exception ex)
        {
            throw new NotImplementedException("unexpected:", ex);
        }

        public void OnNext(byte value)
        {
            if (atFirst)
            {
                first = value;
                atFirst = false;
            }
            else
            {
                Forward(CheckAndCombine(first, value, pos));
                pos++;
                atFirst = true;
            }
        }

        // convert one group like "54 29 " to an 8-bit-byte ("54 29 "-> 0xA9)
        public static byte CheckAndCombine(byte first, byte second, int pos)
        {
            // middle 6 bits must match!
            if ((first & 0x3F) != (second >> 1))
                Console.Error.WriteLine("pos 0x{0:X}: strange values: {1:X2} {2:X2}", pos, first, second);

            return (byte)((first << 1) | second);
        }

        private void Forward(byte value)
        {
            output.WriteByte(value);
        }

        private Stream output;
        private byte first;
        private bool atFirst = true;
        private int pos = 0;
    }
}
