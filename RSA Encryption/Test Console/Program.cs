using System;
using System.Numerics;
using System.Text;
using RSA_Structures;

namespace Test_Console
{
    class Program
    {
        public static BigInteger BinToDec(string value)
        {            
            BigInteger res = 0;           
            foreach (char c in value)
            {
                res <<= 1;
                res += c == '1' ? 1 : 0;
            }
            return res;
        }

        public static string ToBinaryString(BigInteger bigint)
        {
            var bytes = bigint.ToByteArray();
            var idx = bytes.Length - 1;

            var base2 = new StringBuilder(bytes.Length * 8);

            var binary = Convert.ToString(bytes[idx], 2);

            if (binary[0] != '0' && bigint.Sign == 1)
            {
                base2.Append('0');
            }

            base2.Append(binary);

            for (idx--; idx >= 0; idx--)
            {
                base2.Append(Convert.ToString(bytes[idx], 2).PadLeft(8, '0'));
            }

            return base2.ToString();
        }

        public static string ToNBase(BigInteger a, int n)
        {
            StringBuilder sb = new StringBuilder();
            while (a > 0)
            {
                sb.Insert(0, a % n);
                a /= n;
            }
            return sb.ToString();
        }

        static void Main(string[] args)
        {
            BigInteger okita = BinToDec("100101100");

            string binarioPrueba = ToNBase(0, 2);

            //
            //
            //
            RSA rsa = new RSA();
            string keys = rsa.GetKeys(47,43);
            string[] splits = keys.Split("|");
            string[] publicKey = splits[0].Split(",");
            string[] privateKey = splits[1].Split(",");

            byte[] oka = { 32, 123, 230 };
            int XD = 0;
            byte[] result = rsa.RSA_(oka, int.Parse(publicKey[0]), int.Parse(privateKey[1]), ref XD, ".txt");
            byte[] final = rsa.RSA_(result, int.Parse(privateKey[0]), int.Parse(publicKey[1]), ref XD, ".rsa");
            Console.WriteLine("Ok");
            Console.Clear();

            string llaves = rsa.GetKeys(61, 53);
            Console.WriteLine(llaves);
            Console.ReadLine();
            Console.WriteLine("Digite un número:");
            int rango = int.Parse(Console.ReadLine());

            for (int i = 2; i <= rango; i++)
            {
                int counter = 0;

                for (int j = 1; j < 10; j++)
                {
                    if (i != j)
                    {
                        if (i % j == 0)
                        {
                            counter++;
                        }
                    }
                }
                if (counter == 1)
                {
                    Console.WriteLine(i);
                }
            }
        }
    }
}