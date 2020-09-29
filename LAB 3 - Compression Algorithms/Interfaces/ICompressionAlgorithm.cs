using System;
using System.Collections.Generic;
using System.Text;

namespace LAB_3___Compressor.Interfaces
{
    interface ICompressionAlgorithm
    {
        public string EncodeData(string content);
        public string DecodeData(byte[] content);
        public float CompressionRatio(float compressed, float original);
        public float CompressionFactor(float compressed, float original);
        public float ReductionPercentage(float compressed, float original);

    }
}
