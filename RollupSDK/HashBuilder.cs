using System.Numerics;
using System.Security.Cryptography;
using System.Text;

namespace BOSagora.Rollup.BlockChain
{
    /// <summary>
    /// The Class for creating hash
    /// </summary>
    public class HashBuilder
    {
        private readonly MemoryStream stream;
        private readonly BinaryWriter writer;

        public HashBuilder()
        {
            stream = new MemoryStream();
            writer = new BinaryWriter(stream, Encoding.UTF8, false);
        }

        public void Clear()
        {
            writer.Flush();
            stream.SetLength(0);
        }

        public void Update(string value)
        {
            var bytes = Encoding.UTF8.GetBytes(value);
            UpdateVarInt(bytes.Length);
            writer.Write(bytes);
        }

        public void Update(byte[] value)
        {
            UpdateVarInt(value.Length);
            writer.Write(value);
        }

        public void Update(int value)
        {
            writer.Write(Convert.ToUInt64(value));
        }

        public void Update(long value)
        {
            writer.Write(Convert.ToUInt64(value));
        }

        public void Update(BigInteger value)
        {
            var bytes = value.ToByteArray();
            Array.Reverse(bytes);
            var check = true;
            foreach (var t in bytes)
            {
                if (check && t == 0x00) continue;
                writer.Write(t);
                check = false;
            }
        }

        private void UpdateVarInt(long value)
        {
            if (value <= 0xfc)
            {
                writer.Write(Convert.ToByte(value));
            }
            else if (value <= 0xffff)
            {
                writer.Write(0xfd);
                writer.Write(Convert.ToUInt16(value));
            }
            else if (value < 0xffffffff)
            {
                writer.Write(0xfe);
                writer.Write(Convert.ToUInt32(value));
            }
            else
            {
                writer.Write(0xff);
                writer.Write(Convert.ToUInt64(value));
            }
        }

        public Hash Digest()
        {
            writer.Flush();
            return new Hash(SHA256.HashData(stream.ToArray()));
        }
    }
}
