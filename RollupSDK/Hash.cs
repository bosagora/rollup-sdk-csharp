using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;

namespace BOSagora.Rollup.BlockChain
{
    /// <summary>
    /// The Class for hash
    /// </summary>
    public class Hash
    {
        public byte[] data;
        public const int Width = 32;

        public Hash(IEnumerable<byte> _data)
        {
            data = _data.ToArray();
            if (data.Length != Hash.Width) throw new Exception("The size of the data is abnormal.");
        }

        public override string ToString()
        {
            return "0x" + BitConverter.ToString(data).Replace("-", string.Empty).ToLower();
        }

        public static Hash From(string data)
        {
            return new Hash(Utils.ConvertHexStringToByte(data));
        }
    }
}
