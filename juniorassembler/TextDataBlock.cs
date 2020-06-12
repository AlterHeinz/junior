using System;
using System.Collections.Generic;

namespace juniorassembler
{
    // represents a block of text data in memory; should be disassembled as printable chars.
    internal class TextDataBlock : DataBlock
    {
        public ushort Address { get; }
        public IEnumerable<byte> Bytes => textDataBytes.ToArray();
        public string Text { get; private set; }
        public ushort CurNoOfBytes => (ushort)textDataBytes.Count;

        public TextDataBlock(ushort address)
        {
            Address = address;
            textDataBytes = new List<byte>();
            Text = "";
        }

        public static bool isPrintable(byte value)
        {
            return 0x1D == value || (0x20 <= value && value <= 0x5E) || (0x60 <= value && value <= 0x7F) || 0xFF == value;
        }

        public void Append(byte value)
        {
            textDataBytes.Add(value);
            Text += convert(value);
        }

        private static char convert(byte value)
        {
            return (0x1D == value || 0xFF == value) ? '?' : Convert.ToChar(value);
        }

        private List<byte> textDataBytes = new List<byte>();
    }
}