using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using LAB_3___Compressor.Huffman_Coding;
using System.Text.Json;

namespace LAB_3___API
{
    public class FileManage
    {
        public string OriginalFileName { get; set; }
        public string CompressedFileName { get; set; }
        public string CompressedFilePath { get; set; }
        public double CompressionRatio { get; set; }
        public double CompressionFactor { get; set; }
        public double ReductionPercentage { get; set; }
        public DateTime DateOfCompression { get; set; }

        public List<FileManage> GetAllCompressions(string path)
        {
            List<FileManage> list = new List<FileManage>();
            using (StreamReader sr = new StreamReader(path))
            {
                string result = sr.ReadToEnd();

                JsonSerializerOptions name_rule = new JsonSerializerOptions {IgnoreNullValues = true };
                list = JsonSerializer.Deserialize<List<FileManage>>(result, name_rule);
            }
            return list;
        }

        public void SaveFile(IFormFile file, string output_path)
        {
            if (File.Exists(output_path))
            {
                File.Delete(output_path);
            }
            using (var fs = new FileStream(output_path, FileMode.OpenOrCreate))
            {
                file.CopyTo(fs);
            }
            return;
        }

        public void CompressFile(string path)
        {
            Huffman hf = new Huffman();
            byte[] buffer;
            using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate))
            {
                buffer = new byte[fs.Length];
                using (var br = new BinaryReader(fs))
                {
                    br.Read(buffer,0,(int)fs.Length);
                }
            }
            string result = hf.EncodeData(buffer);
            using (var fs = new FileStream(CompressedFilePath, FileMode.OpenOrCreate))
            {
               using(StreamWriter sw = new StreamWriter(fs, Encoding.ASCII))
                {
                    sw.Write(result);
                }
            }

            CompressionRatio = hf.CompressionRatio();
            CompressionFactor = hf.CompressionFactor();
            ReductionPercentage = hf.ReductionPercentage();
        }

        public void WriteCompression(FileManage file)
        {
            string[] path = CompressedFilePath.Split("compressions");
            string file_path = path[0] + "\\compressions\\log\\log.txt";

            List<FileManage> list;
            string json_text = "";
            using(FileStream fs = new FileStream(file_path, FileMode.OpenOrCreate))
            {
              using(StreamReader sr = new StreamReader(fs))
                {
                    json_text = sr.ReadToEnd();
                }                                                   
            }
            if (json_text == "")
            {
                using (StreamWriter sw = new StreamWriter(file_path, false, Encoding.UTF8))
                {
                    string file_stats = JsonSerializer.Serialize(file);
                    sw.Write("[" + file_stats + "]");
                    return;
                }               
            }
            else
            {
                JsonSerializerOptions name_rule = new JsonSerializerOptions {IgnoreNullValues = true };
                list = JsonSerializer.Deserialize<List<FileManage>>(json_text, name_rule);
                list.Add(file);
            }
            using (StreamWriter sw = new StreamWriter(file_path, false, Encoding.UTF8))
            {
                string file_stats = JsonSerializer.Serialize(list);
                sw.Write(file_stats);
            }
        }

    }
}
