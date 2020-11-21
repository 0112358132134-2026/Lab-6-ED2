using System.Numerics;

namespace RSA_Structures
{
    public interface IRSA
    {
        public byte[] RSA_(byte[] originalBytes, BigInteger n, BigInteger ed, ref int isEncrypted, string extension);
    }
}
