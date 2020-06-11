using System.Collections.Generic;

namespace juniorassembler
{
    // represents a block of text data in memory; should be disassembled as printable chars.
    internal class TextDataBlock : ConcreteInstructionWithPossiblyMissingBytes
    {
        public byte[] Bytes { get; private set; }
        public string Text { get; private set; }

        public TextDataBlock(int address, List<byte> textDataBytes, string textData)
            : base(new ConcreteInstruction(address, 0), 0)
        {
            Bytes = textDataBytes.ToArray();
            Text = textData;
        }
    }
}