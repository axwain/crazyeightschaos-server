using System.Text;

namespace CrazyEights.Utils
{
    public static class Base15
    {
        private static string alphabet = "0123456789BCDFZ";

        public static string Encode(Int32 input)
        {
            if (input < 0)
            {
                throw new ArgumentOutOfRangeException("input", "Base15 encode input should be greater than 0");
            }

            var currentValue = input;
            var sb = new StringBuilder(8);
            do
            {
                sb.Insert(0, alphabet[(byte)(currentValue % 15)]);
                currentValue /= 15;
            }
            while (currentValue != 0);

            return sb.ToString().PadLeft(8, alphabet[0]);
        }

        public static Int32 Decode(string code)
        {
            if (code.Length != 8)
            {
                throw new ArgumentException("Code should be eight characters long", "code");
            }

            var currentValue = 0;

            for (var i = 0; i < 8; i++)
            {
                var index = alphabet.IndexOf(code[i]);
                if (index < 0)
                {
                    throw new ArgumentOutOfRangeException("code", "Code contains invalid characters");
                }
                currentValue = 15 * currentValue + index;
            }

            return currentValue;
        }
    }
}