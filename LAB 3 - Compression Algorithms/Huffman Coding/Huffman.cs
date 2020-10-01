using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LAB_3___Compressor.Huffman_Coding
{
    public class Huffman : Interfaces.ICompressionAlgorithm
    {
        Dictionary<byte, string> codes = new Dictionary<byte, string>();
        string metadata= "";

        int count_chars = 0;
        double original_weight;
        double compressed_weight;

        public string EncodeData(byte[] content)
        {
            Node root = CreateTree(content);
            GenerateCodes(root, "");

            //swap each char for its binary code
            string content_encoded = "";
            for (int i = 0; i < content.Length; i++)
            {
                content_encoded += codes[content[i]];
            }

            //complete the last octet
            while (content_encoded.Length % 8 != 0)
            {
                content_encoded += "0";
            }

            //converting each binary code to a decimal number and then converting it to an ascii byte
            string encode_final = "";
            for (int i = 0; i < content_encoded.Length; i += 8)
            {
                string code_bin = content_encoded.Substring(i, 8);
                int code_dec = Convert.ToInt32(code_bin, 2);
                byte[] code_byte = new byte[] { Convert.ToByte(code_dec) };
                encode_final += Encoding.ASCII.GetString(code_byte);
            }

            //concatenate the metadata with the compressed data



            original_weight = content.Length;
            compressed_weight = encode_final.Length;

            return encode_final;
        }


        public Node CreateTree(byte[] text)
        {
            List<Node> base_frequencies = ProbeFrequencies(text);
            base_frequencies = SetPercentages(base_frequencies);
            SetMetaData(base_frequencies);

            while (base_frequencies.Count != 1)
            {
                Node intermediate_node = new Node { Percentage = base_frequencies.ElementAt(0).Percentage + base_frequencies.ElementAt(1).Percentage};
                intermediate_node.Left = base_frequencies.ElementAt(0);
                intermediate_node.Right = base_frequencies.ElementAt(1);
                base_frequencies.RemoveAt(0);
                base_frequencies.RemoveAt(0);
                base_frequencies.Add(intermediate_node);
                base_frequencies = SortCharacterList(base_frequencies);
            }
            return base_frequencies.ElementAt(0);
        }


        public void GenerateCodes(Node root, string current_code)
        {
            if (root.Left == null && root.Right == null)
            {
                root.Code = current_code;
                codes.Add(root.Character, root.Code);
                return;
            }
            if (root.Left != null)
            {
                current_code += "0";
                GenerateCodes(root.Left, current_code);
                current_code = current_code.Substring(0, current_code.Length - 1);
            }
            if (root.Right != null)
            {
                current_code += "1";
                GenerateCodes(root.Right, current_code);
            }
        }

        public List<Node> ProbeFrequencies(byte[] text)
        {
            List<Node> data_list = new List<Node>();
            int text_length = text.Length;
            for (int i = 0; i < text_length; i++)
            {
                int position_character = DetectPositionCharacter(data_list, text[i]);
                if (position_character != -1)
                {
                    data_list.ElementAt(position_character).Frequency++;
                }
                else
                {
                    Node new_character = new Node { Character = text[i], Frequency = 1 };
                    data_list.Add(new_character);
                    count_chars++;
                }
            }
            return SortCharacterList(data_list);
        }

        public List<Node> SetPercentages(List<Node> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                list[i].Percentage = list[i].Frequency / count_chars;
            }
            return list;
        }

        public void SetMetaData(List<Node> list)
        {
            //number of different characters
            byte[] count_byte = new byte[] { Convert.ToByte(count_chars) };
            metadata += Encoding.ASCII.GetString(count_byte);

            //number of bytes for each character frequencie
            int number_bytes = (1 + 1);
            byte[] number_byte = new byte[] { Convert.ToByte(number_bytes) };
            metadata += Encoding.ASCII.GetString(number_byte);


            for (int i = 0; i < list.Count; i++)
            {
                Node _node = list[i];
                byte[] frequency_byte = new byte[] { Convert.ToByte(_node.Frequency) };
                metadata += Convert.ToChar(_node.Character).ToString() + Encoding.ASCII.GetString(frequency_byte);
            }
        }

        public int DetectPositionCharacter(List<Node> data_list, byte character)
        {
            for (int i = 0; i < data_list.Count; i++)
            {
                if (data_list.ElementAt(i).Character == character)
                {
                    return i;
                }
            }
            return -1;
        }

        public List<Node> SortCharacterList(List<Node> data_list)
        {
            return data_list.OrderBy(x => x.Percentage).ToList();
        }

        public string DecodeData(byte[] content)
        {
            throw new NotImplementedException();
        }


        public double ReductionPercentage()
        {
            return 1 - (compressed_weight/original_weight);
        }

        public double CompressionFactor()
        {
            return (original_weight / compressed_weight);
        }

        public double CompressionRatio()
        {
            return (compressed_weight / original_weight);
        }
    }
}
