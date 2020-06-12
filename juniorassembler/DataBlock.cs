using System.Collections.Generic;

namespace juniorassembler
{
    // interface of a block of data in memory
    public interface DataBlock
    {
        ushort Address { get; }
        IEnumerable<byte> Bytes { get; }
        string Text { get; }
        ushort CurNoOfBytes { get; }

        void Append(byte value);
    }
}