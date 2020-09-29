using System;
using System.Collections.Generic;
using System.Text;

namespace LAB_3___Compressor.Huffman_Coding
{
    class Node
    {
        public char Character { get; set; }
        public int RelativeFrequency { get; set; }
        public string Code { get; set; }
        public Node Left { get; set; }
        public Node Right { get; set; }

        public Node()
        {
            Character = '`';
            RelativeFrequency = 0;
            Code = " ";
            Left = null;
            Right = null;
        }
    }
}
