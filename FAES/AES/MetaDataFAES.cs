using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAES.AES
{
    internal class MetaDataFAES
    {
        protected byte[] _passwordHint;

        public MetaDataFAES(string passwordHint)
        {
            _passwordHint = ConvertStringToBytes(passwordHint);
        }

        public MetaDataFAES(byte[] metaData)
        {
            _passwordHint = metaData.Take(64).ToArray();
        }

        private byte[] ConvertStringToBytes(string value)
        {
            if (value.Length > 64)
                return Encoding.UTF8.GetBytes(value.Substring(0, 64));
            else
                return Encoding.UTF8.GetBytes(value.PadRight(64, '¬'));
        }

        public string getPasswordHint()
        {
            return Encoding.UTF8.GetString(_passwordHint).Replace("¬", "");
        }

        private byte[] Combine(params byte[][] arrays)
        {
            byte[] ret = new byte[arrays.Sum(x => x.Length)];
            int offset = 0;
            foreach (byte[] data in arrays)
            {
                Buffer.BlockCopy(data, 0, ret, offset, data.Length);
                offset += data.Length;
            }
            return ret;
        }

        public byte[] getMetaData()
        {
            byte[] formedMetaData = new byte[256];
            Buffer.BlockCopy(_passwordHint, 0, formedMetaData, 0, _passwordHint.Length);

            return formedMetaData;
        }
    }
}
