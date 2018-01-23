using System;
namespace ACEngine.Math
{
    class Random
    {
        public static int Range(int x, int y)
        {
            return new System.Random(GetRandomSeed()).Next(x,y);
        }
        static int GetRandomSeed()
        {
            byte[] bytes = new byte[4];
            System.Security.Cryptography.RNGCryptoServiceProvider rng = new System.Security.Cryptography.RNGCryptoServiceProvider();
            rng.GetBytes(bytes);
            return BitConverter.ToInt32(bytes, 0);
        }
    }
}
