using UnityEngine;

namespace NetworkCompression
{
    public struct RawOutputStream
    {
        public RawOutputStream(byte[] buffer, int bufferOffset) {
            m_Buffer = buffer;
            m_BufferOffset = bufferOffset;
            m_CurrentByteIndex = bufferOffset;
        }

        public void Initialize(byte[] buffer, int bufferOffset) {
            this = new RawOutputStream(buffer, bufferOffset);
        }

        public void WriteRawBits(uint value, int numbits) {
            for (int i = 0; i < numbits; i += 8) {
                m_Buffer[m_CurrentByteIndex++] = (byte)value;
                value >>= 8;
            }
        }

        unsafe public void WriteRawBytes(byte* value, int count) {
            for (int i = 0; i < count; i++)
                m_Buffer[m_CurrentByteIndex + i] = value[i];
            m_CurrentByteIndex += count;
        }

        public void WritePackedNibble(uint value, int context) {
            Debug.Assert(value < 16);

            m_Buffer[m_CurrentByteIndex++] = (byte)value;
        }

        public void WritePackedUInt(uint value, int context) {

            m_Buffer[m_CurrentByteIndex + 0] = (byte)value;
            m_Buffer[m_CurrentByteIndex + 1] = (byte)(value >> 8);
            m_Buffer[m_CurrentByteIndex + 2] = (byte)(value >> 16);
            m_Buffer[m_CurrentByteIndex + 3] = (byte)(value >> 24);
            m_CurrentByteIndex += 4;
        }

        public void WritePackedIntDelta(int value, int baseline, int context) {
            WritePackedUIntDelta((uint)value, (uint)baseline, context);
        }

        public void WritePackedUIntDelta(uint value, uint baseline, int context) {
            int diff = (int)(baseline - value);
            uint interleaved = (uint)((diff >> 31) ^ (diff << 1));      // interleave negative values between positive values: 0, -1, 1, -2, 2
            WritePackedUInt(interleaved, context);
        }

        public int GetBitPosition2() {
            return (m_CurrentByteIndex - m_BufferOffset) * 8;
        }

        public int Flush() {
            return m_CurrentByteIndex - m_BufferOffset;
        }
        byte[] m_Buffer;
        int m_BufferOffset;
        int m_CurrentByteIndex;
    }
}