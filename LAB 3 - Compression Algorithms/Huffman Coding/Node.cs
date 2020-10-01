using System;
using System.Collections.Generic;
using System.Text;

namespace LAB_3___Compressor.Huffman_Coding
{
    public class Node
    {
        public byte Character { get; set; }
        public int Frequency { get; set; }
        public double Percentage { get; set; }
        public string Code { get; set; }
        public Node Left { get; set; }
        public Node Right { get; set; }

        public Node()
        {
            Character = 00;
            Frequency = 0;
            Percentage = 0;
            Code = " ";
            Left = null;
            Right = null;
        }
    }
}
