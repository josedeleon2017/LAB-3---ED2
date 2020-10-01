using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace LAB_3___API.Controllers
{
    [Route("api")]
    [ApiController]
    public class CompressionController : ControllerBase
    {
        private IWebHostEnvironment environment;
        public CompressionController(IWebHostEnvironment env)
        {
            environment = env;
        }

        [HttpGet]
        public string Index()
        {
            string text = "\t\t\t- LAB 3 -\n\nKevin Romero 1047519\nJosé De León 1072619";
            return text;
        }

        [HttpPost("compress/{name}")]
        public ActionResult CompressFile([FromForm] IFormFile file, string name)
        {
            string file_path = environment.ContentRootPath + $"\\compressions\\{name}.txt";

            string file_compressedpath = environment.ContentRootPath + $"\\compressions\\{name}_resultado.huff";
            FileManage _file = new FileManage() { OriginalFileName = file.Name+".txt", CompressedFileName = name+".huff", CompressedFilePath = file_compressedpath };

            //Save file in the server
            _file.SaveFile(file, file_path);

            //Compress the file previously saved
            _file.CompressFile(file_path);

            //Write on log file the compression result
            _file.WriteCompression(_file);


            FileStream result = new FileStream(_file.CompressedFilePath,FileMode.Open);
            return File(result,"text/plain");
        }

        [HttpPost("decompress")]
        public string DecompressFile([FromForm] IFormFile file)
        {
            //● Recibe un archivo.huff que se deberá descomprimir
            //● Retorna el archivo de texto con el nombre original

            return "name.txt";
        }

        [HttpGet("compressions")]
        public IEnumerable<FileManage> GetCompressions()
        {
            try
            {
                string path = environment.ContentRootPath+"\\compressions\\log\\log.txt";
                FileManage fm = new FileManage();
                return fm.GetAllCompressions(path);           
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
