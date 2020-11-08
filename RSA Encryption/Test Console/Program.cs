using System;
using RSA_Structures;

namespace Test_Console
{
    class Program
    {
        static void Main(string[] args)
        {
            RSA rsa = new RSA();
            string llaves = rsa.GetKeys(61, 53);
            Console.WriteLine(llaves);
            Console.ReadLine();
            //Console.WriteLine("Digite un número: ");
            //int rango = int.Parse(Console.ReadLine());

            //for (int i = 2; i <= rango; i++)
            //{
            //    int counter = 0;

            //    for (int j = 1; j < 10; j++)
            //    {
            //        if (i != j)
            //        {
            //            if (i % j == 0)
            //            {
            //                counter++;
            //            }
            //        }                   
            //    }
            //    if (counter == 1)
            //    {
            //        Console.WriteLine(i);
            //    }
            //}           
        }
    }
}
