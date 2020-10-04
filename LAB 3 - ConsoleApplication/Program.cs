using System;
using LAB_3___Compressor.Huffman_Coding;

namespace LAB_3___ConsoleApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("\t\t\t\t\t\t- LAB 3 -\n\nKevin Romero 1047519\nJosé De León 1072619");
            Huffman hf = new Huffman();

            string original = "Tu trabajo va a llenar gran parte de tu vida, la única manera de estar realmente satisfecho es hacer lo que creas es un gran trabajo y la única manera de hacerlo es amar lo que haces. Si no lo has encontrado aún, sigue buscando. Como con todo lo que tiene que ver con el corazón, sabrás cuando lo hayas encontrado. -Steve Jobs";
            Console.WriteLine("\n\nTEXTO ORIGINAL          \n" + original);

            byte[] compression_result  = hf.EncodeData(ConvertToByte(original));
            Console.WriteLine("\n\nTEXTO COMPRIMIDO");
            Console.WriteLine(ConvertToChar(compression_result));

            Huffman hf2 = new Huffman();

            byte[] descompression_result = hf2.DecodeData(compression_result);
            Console.WriteLine("\n\nTEXTO DESCOMPRIMIDO");
            Console.WriteLine(ConvertToChar(descompression_result));


            Console.ReadLine();
        }

        public static byte[] ConvertToByte(string content)
        {
            byte[] array = new byte[content.Length];
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = Convert.ToByte(content[i]);
            }
            return array;
        }

        public static char[] ConvertToChar(byte[] content)
        {
            char[] array = new char[content.Length];
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = Convert.ToChar(content[i]);
            }
            return array;
        }
    }
}
