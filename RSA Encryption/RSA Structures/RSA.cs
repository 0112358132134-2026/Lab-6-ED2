using System;
using System.Globalization;

namespace RSA_Structures
{
    public class RSA
    {
        #region "Keys"
        public string GetKeys(int p, int q)
        {
            if (IsPrimeNumber(p) && IsPrimeNumber(q))
            {
                int n = p * q;
                int phi = (p - 1) * (q - 1);
                int e = FindE(phi);
                int d = FindD(phi, e);
                return n + "," + e + "|" + n + "," + d;
            }
            else
            {
                return "";
            }
        }
        public bool IsPrimeNumber(int number)
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
        public int FindE(int phi)
        {
            int e = 0;
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
        public int FindD(int phi, int e)
        {
            int phiAux1 = phi, phiAux2 = phi;
            int eAux = e;
            int one = 1;

            int d = 0;
            bool match = false;

            while (!match)
            {
                int division1 = phiAux1 / eAux;
                
                int multiplication1 = division1 * eAux;
                int multiplication2 = division1 * one;

                int subtraction1 = phiAux1 - multiplication1;
                int subtraction2 = phiAux2 - multiplication2;

                if (subtraction1 == 1)
                {
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

        public byte[] RSA_()
        {
            return null;
        }
    }
}