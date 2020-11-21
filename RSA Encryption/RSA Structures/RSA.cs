using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace RSA_Structures
{
    public class RSA : IRSA
    {
        #region "Keys"
        public string GetKeys(BigInteger p, BigInteger q)
        {
            if (IsPrimeNumber(p) && IsPrimeNumber(q))
            {
                BigInteger n = p * q;
                BigInteger phi = (p - 1) * (q - 1);
                BigInteger e = FindE(phi);
                BigInteger d = FindD(phi, e);
                return n + "," + e + "|" + n + "," + d;
            }
            else
            {
                return "";
            }
        }
        public bool IsPrimeNumber(BigInteger number)
        {
            int counter = 0;
            for (int j = 1; j < 10; j++)
            {
                if (number != j)
                {
                    if (number % j == 0)
                    {
                        counter++;
                    }
                }                
            }
            if (counter == 1) return true; else return false;                        
        }
        public BigInteger FindE(BigInteger phi)
        {
            BigInteger e = 0;
            for (int i = 2; i < phi; i++)
            {
                if (IsPrimeNumber(i))
                {
                    if (phi % i != 0)
                    {
                        e = i;
                        break;
                    }
                }
            }
            return e;
        }
        public BigInteger FindD(BigInteger phi, BigInteger e)
        {
            BigInteger phiAux1 = phi, phiAux2 = phi;
            BigInteger eAux = e;
            BigInteger one = 1;

            BigInteger d = 0;
            bool match = false;

            while (!match)
            {
                BigInteger division = phiAux1 / eAux;

                BigInteger multiplication1 = division * eAux;
                BigInteger multiplication2 = division * one;

                BigInteger subtraction1 = phiAux1 - multiplication1;
                BigInteger subtraction2 = phiAux2 - multiplication2;

                if (subtraction1 == 1)
                {
                    if (subtraction2 < 0)
                    {
                        bool positive = false;
                        while (!positive)
                        {
                            subtraction2 += phi;
                            if (subtraction2 > 0)
                            {
                                positive = true;
                            }
                        }                       
                    }
                    d = subtraction2;
                    match = true;
                }
                else
                {
                    if (subtraction2 < 0)
                    {
                        subtraction2 += phi;
                    }
                    phiAux1 = eAux;
                    phiAux2 = one;
                    eAux = subtraction1;
                    one = subtraction2;
                }               
            }
            return d;
        }
        #endregion
        #region "RSA"
        public byte[] RSA_(byte[] originalBytes, BigInteger n, BigInteger ed, ref int isEncrypted, string extension)
        {
            byte[] result;

            if (extension == ".rsa")
            {
                List<BigInteger> decimals = new List<BigInteger>();
                for (int i = 0; i < originalBytes.Length; i++)
                {
                    decimals.Add(originalBytes[i]);
                }

                List<string> bytes = BinariesWithMaximunLength(decimals, 8);
                int length = ToNBase(n, 2).Length;
                List<BigInteger> originalBytesAux = OriginalBytes(bytes, length);
   
                result = new byte[originalBytesAux.Count];
                for (int i = 0; i < originalBytesAux.Count; i++)
                {
                    BigInteger resultAux = BigInteger.ModPow(originalBytesAux[i], ed, n);
                    result[i] = (byte)(resultAux);
                }
            }
            else
            {
                List<BigInteger> resultingBytes = new List<BigInteger>();
                for (int i = 0; i < originalBytes.Length; i++)
                {
                    BigInteger power = BigInteger.ModPow(originalBytes[i], ed, n);
                    resultingBytes.Add(power);
                }

                BigInteger length = ToNBase(n, 2).Length;
                List<string> finalBytes = BinariesWithMaximunLength(resultingBytes, length);               
                List<BigInteger> decimals = new List<BigInteger>();

                for (int i = 0; i < finalBytes.Count; i++)
                {
                    decimals.Add(BinToDec(finalBytes[i]));
                }

                result = new byte[decimals.Count];
                
                for (int i = 0; i < result.Length; i++)
                {
                    result[i] = (byte)decimals[i];
                }
                isEncrypted = 1;
            }                        
            return result;
        }
        #endregion       
        #region "Auxiliaries"  

        public int ConvertBinaryToDecimal(string binary)
        {
            StringBuilder auxiliar = new StringBuilder();
            auxiliar.Append(binary);

            int exponent = auxiliar.Length - 1;
            int decimalNumber = 0;

            for (int i = 0; i < auxiliar.Length; i++)
            {
                if (int.Parse(auxiliar.ToString(i, 1)) == 1)
                {
                    decimalNumber += int.Parse(System.Math.Pow(2, double.Parse(exponent.ToString())).ToString());
                }
                exponent--;
            }
            return decimalNumber;
        }
        //NUEVO
        public BigInteger BinToDec(string value)
        {
            BigInteger res = 0;
            foreach (char c in value)
            {
                res <<= 1;
                res += c == '1' ? 1 : 0;
            }
            return res; 
        }
        //NUEVO

        public string ConvertDecimalToBinary(BigInteger number)
        {
            string result = "";
            while (number > 0)
            {
                if (number % 2 == 0)
                {
                    result = "0" + result;
                }
                else
                {
                    result = "1" + result;
                }
                number = (BigInteger)(number / 2);
            }
            return result;
        }
        //NUEVO
        public string ToNBase(BigInteger a, int n)
        {
            StringBuilder sb = new StringBuilder();
            while (a > 0)
            {
                sb.Insert(0, a % n);
                a /= n;
            }
            return sb.ToString();
        }
        //NUEVO

        public List<string> SeparateBytes(string largeBinary, int length)
        {
            StringBuilder copy = new StringBuilder();
            copy.Append(largeBinary);
            List<string> result = new List<string>();
            bool OK = false;
            while (!OK)
            {
                if (copy.Length >= length)
                {
                    result.Add(copy.ToString(0, length));
                    copy.Remove(0, length);
                }
                else
                {
                    if (copy.Length > 0)
                    {
                        for (int i = copy.Length; i < length; i++)
                        {
                            copy.Append("0");
                        }
                        result.Add(copy.ToString());
                    }
                    OK = true;
                }
            }
            return result;
        }
        public string LargeBinary(StringBuilder binary, int mod)
        {
            bool isDivisible = false;
  
            while (!isDivisible)
            {
                if (binary.Length % mod == 0)
                {
                    isDivisible = true;
                }
                else
                {
                    binary.Insert(0, "0");
                }
            }
            return binary.ToString();
        }
        public List<string> BinariesWithMaximunLength(List<BigInteger> listOfBinaries, BigInteger length)
        {
            StringBuilder auxBinary = new StringBuilder();
            for (int i = 0; i < listOfBinaries.Count; i++)
            {
                StringBuilder decimalToBinary = new StringBuilder();
                decimalToBinary.Append(ToNBase(listOfBinaries[i],2));
                                 
                if (decimalToBinary.Length < length)
                {
                    BigInteger missing = length - decimalToBinary.Length;
                    for (int j = 0; j < missing; j++)
                    {
                        decimalToBinary.Insert(0, "0");
                    }
                }
                auxBinary.Append(decimalToBinary.ToString());
            }

            string paddedWithBytes = LargeBinary(auxBinary, 8);
            List<string> result = SeparateBytes(paddedWithBytes, 8);

            return result;
        }
        public List<BigInteger> OriginalBytes(List<string> bytes, int length)
        {
            StringBuilder auxiliar = new StringBuilder();
            for (int i = 0; i < bytes.Count; i++)
            {
                auxiliar.Append(bytes[i]);
            }

            bool isDivisible = false;
            while (!isDivisible)
            {
                if (auxiliar.Length % length == 0)
                {
                    isDivisible = true;
                }
                else
                {
                    auxiliar.Remove(0, 1);
                }
            }

            List<string> originalBinaries = SeparateBytes(auxiliar.ToString(), length);
            List<BigInteger> result = new List<BigInteger>();
            for (int i = 0; i < originalBinaries.Count; i++)
            {
                result.Add(BinToDec(originalBinaries[i]));
            }            
            return result;
        }
        #endregion
    }
}