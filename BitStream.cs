using System;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;

namespace DbHierarchy
{
    public class BitStream
    {
        private readonly byte[] _bytes;

        public BitStream(byte[] bytes)
        {
            _bytes = bytes.ToArray();
        }

        public BitStream(int size)
        {
            _bytes = new byte[size];
        }

        public int ReadBits(int offset, int bits)
        {
            if(bits > 32)
                throw new InvalidOperationException("You cannot read more than 32 bits");
            
            var firstByte = offset / 8;
            var lastByte = (offset + bits) / 8;
            var currentOffset = 0;
            var targetValue = 0;

            for (int i = firstByte; i <= lastByte; i++)
            {
                var localOffset = Math.Max(0, offset - (i * 8));
                var localBits = i == firstByte
                    ? 8 - localOffset
                    : Math.Min(8, ((offset + bits) - (i * 8)));
                
                // shift right by localOffset and mask off local bits
                var localValue = (_bytes[i] >> localOffset) & ~((~0) << localBits);


                // write bits to int
                targetValue |= (localValue << currentOffset);
                currentOffset += localBits;
            }

            return targetValue;
        }

        public int WriteBits(int offset, int bits, int value)
        {
            throw new NotImplementedException();
        }
    }
}
