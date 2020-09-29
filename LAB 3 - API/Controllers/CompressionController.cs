using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace LAB_3___API.Controllers
{
    [Route("api")]
    [ApiController]
    public class CompressionController : ControllerBase
    {
        [HttpGet]
        public string Index()
        {
            string text = "\t\t\t- LAB 3 -\n\nKevin Romero 1047519\nJosé De León 1072619";
            return text;
        }

        [HttpPost("compress/{name}")]
        public string CompressFile(string name, [FromForm] IFormFile file)
        {
            //● Recibe un archivo de texto que se deberá comprimir
            //● Retorna un archivo<name>.huff con el contenido del archivo

            return name +".huff";
        }

        [HttpPost("decompress")]
        public string DecompressFile([FromForm] IFormFile file)
        {
            //● Recibe un archivo.huff que se deberá descomprimir
            //● Retorna el archivo de texto con el nombre original

            return "name.txt";
        }

        [HttpGet("compressions")]
        public IEnumerable<CompressionResult> GetCompressions()
        {
            try
            {
                //● Devuelve un JSON con el listado de todas las compresiones con los siguientes valores:
                    //○ Nombre del archivo original
                    //○ Nombre y ruta del archivo comprimido
                    //○ Razón de compresión
                    //○ Factor de compresión
                    //○ Porcentaje de reducción
                CompressionResult result = new CompressionResult();
                return result.GetAllCompressions();
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
