using System;

namespace BOSagora.Rollup.BlockChain
{
    public class Utils
    {
        public static string ConvertByteToHexString(byte[] convertArr)
        {
            var convertArrString = string.Empty;
            convertArrString = "0x" + string.Concat(Array.ConvertAll(convertArr, byt => byt.ToString("x2")));
            return convertArrString;
        }

        public static IEnumerable<byte> ConvertHexStringToByte(string convertString)
        {
            var prefix = convertString[..2];
            var str = (prefix.Equals("0x")) ? convertString[2..] : convertString;
            var convertArr = new byte[str.Length / 2];

            for (var i = 0; i < str.Length; i++)
            {
                convertArr[i] = Convert.ToByte(convertString.Substring(i * 2, 2), 16);
            }
            return convertArr;
        }
    }
}

