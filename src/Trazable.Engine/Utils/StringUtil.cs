using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Trazable.Engine.Utils
{
    /// <summary>
    /// Utils class for strings
    /// </summary>
    public static class StringUtil
    {
        #region - Static Methods -

        #region - Private Static Methods -

        /// <summary>
        /// Computes the md5 hash.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        private static string ComputeMD5Hash(byte[] input)
        {
            byte[] hash = new MD5CryptoServiceProvider().ComputeHash(input);
            return ByteArrayToHex(hash);
        }

        #endregion

        #region - Public Static Methods -

        /// <summary>
        /// Converts a Byte Array into an Hexadecimal string.
        /// </summary>
        /// <param name="arr">The byte array.</param>
        /// <returns></returns>
        public static string ByteArrayToHex(byte[] arr)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < arr.Length; i++)
            {
                sb.Append(arr[i].ToString("X2"));
            }
            return sb.ToString();
        }


        /// <summary>
        /// Computes the m d5 hash.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static string ComputeMD5Hash(string input)
        {
            return ComputeMD5Hash(Encoding.ASCII.GetBytes(input));
        }

        /// <summary>
        /// Unquotes the specified string.
        /// </summary>
        /// <param name="s">The s.</param>
        /// <returns></returns>
        public static string Unquote(string s)
        {
            int n = s.Length;
            return (n > 2 && s.First() == '"' && s[n - 1] == '"')
                ? s.Substring(1, n - 2)
                : s;
        }

        /// <summary>
        /// Camelizes one string.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="fistCapital">if set to <c>true</c> [fist capital].</param>
        /// <returns></returns>
        public static string Camelize(string name, bool fistCapital = false)
        {
            if (string.IsNullOrEmpty(name))
            {
                return name;
            }
            StringBuilder builder = new StringBuilder();
            name = name.ToLower();
            bool capitalize = fistCapital;
            for (int i = 0; i < name.Length; i++)
            {
                char c = name[i];
                if (char.IsLetter(c))
                {
                    if (char.IsUpper(c))
                    {
                        builder.Append(c);
                    }
                    else
                    {
                        builder.Append(capitalize ? char.ToUpper(c) : c);
                    }
                    capitalize = false;
                }
                else
                {
                    if (char.IsDigit(c))
                    {
                        builder.Append(c);
                    }
                    capitalize = true;
                }
            }
            return builder.ToString();
        }

        /// <summary>
        /// Pascalizes one string.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public static string Pascalize(string name)
        {
            return StringUtil.Camelize(name, true);
        }

        public static string Pad(long i, long l)
        {
            string result = i.ToString();
            while (result.Length < l)
            {
                result = "0" + result;
            }
            return result;
        }

        #endregion

        #endregion
    }
}
