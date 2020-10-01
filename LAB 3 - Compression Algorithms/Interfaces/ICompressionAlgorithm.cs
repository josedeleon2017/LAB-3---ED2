using System;
using System.Collections.Generic;
using System.Text;

namespace LAB_3___Compressor.Interfaces
{
    interface ICompressionAlgorithm
    {
        public string EncodeData(byte[] content);
        public string DecodeData(byte[] content);
        public double CompressionRatio();
        public double CompressionFactor();
        public double ReductionPercentage();

    }
}
