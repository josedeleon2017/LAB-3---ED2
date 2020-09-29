using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LAB_3___API
{
    public class CompressionResult
    {
        public string OriginalFileName { get; set; }
        public string CompressedFileName { get; set; }
        public string CompressedFilePath { get; set; }
        public float CompressionRatio { get; set; }
        public float CompressionFactor { get; set; }
        public float ReductionPercentage { get; set; }

        public List<CompressionResult> GetAllCompressions()
        {
            //Leer de un archivo en disco todos los resultados de las compresiones, deserializar el json y retornar la lista de estos objetos
            return null;
        }
    }
}
