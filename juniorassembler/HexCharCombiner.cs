using System;

namespace juniorassembler
{
    // combines 2 consecutive chars which must be hex digits, 
    // parses them to an 8-bit-byte and writes that to the output stream
    public class HexCharCombiner : IObserver<char>
    {
        public const byte QUESTION_MARK = 0x3F;

        public HexCharCombiner(IObserver<byte> output)
        {
            this.output = output;
        }

        public void OnCompleted()
        {
            if (!atFirst)
            {
                // do not Forward last char
                Console.Error.WriteLine("pos {0}: obsolete final char: {1}", pos, first);
            }
            output.OnCompleted();
        }

        public void OnError(Exception ex)
        {
            throw new NotImplementedException("unexpected:", ex);
        }

        public void OnNext(char value)
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

        public static byte CheckAndCombine(char first, char second, int pos)
        {
            // chars must contain hex digits!
            string combined = string.Concat(first, second);
            try {
                byte code = Convert.ToByte(combined, 16);
                return code;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("pos {0}: bad value: {1}, ex: {2}", pos, combined, ex);
                return QUESTION_MARK;
            }
        }

        private void Forward(byte value)
        {
            output.OnNext(value);
        }

        private IObserver<byte> output;
        private char first;
        private bool atFirst = true;
        private int pos = 0;
    }
}
