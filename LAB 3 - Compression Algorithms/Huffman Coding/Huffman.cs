using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LAB_3___Compressor.DataStructures;

namespace LAB_3___Compressor.Huffman_Coding
{
    public class Huffman : Interfaces.ICompressionAlgorithm
    {
        Dictionary<byte, string> codes = new Dictionary<byte, string>();
        PriorityQueue<CharacterNode> queue = new PriorityQueue<CharacterNode>();
        
        string metadata= "";

        int count_chars;
        double original_weight;
        double compressed_weight;

        public string EncodeData(byte[] content)
        {
            count_chars = content.Length;
            queue.PriorityComparison = PriorityComparison;

            CharacterNode root = CreateTree(content);
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
            encode_final = metadata + encode_final;

            original_weight = content.Length;
            compressed_weight = encode_final.Length;

            return encode_final;
        }


        public CharacterNode CreateTree(byte[] text)
        {
            List<CharacterNode> base_frequencies = ProbeFrequencies(text);
            List<CharacterNode> list_complete = SetPercentages(base_frequencies);

            SetMetaData(list_complete);
            SetQueue(list_complete);


            while (queue.Count != 1)
            {
                CharacterNode left_node = queue.Pop();
                CharacterNode right_node = queue.Pop();

                CharacterNode intermadiate_note = new CharacterNode() { Percentage= (left_node.Percentage + right_node.Percentage) ,Left = left_node, Right = right_node};
                queue.Push(intermadiate_note);
            }
            return queue.Pop();
        }


        public void GenerateCodes(CharacterNode root, string current_code)
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

        public List<CharacterNode> ProbeFrequencies(byte[] text)
        {
            List<CharacterNode> data_list = new List<CharacterNode>();
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
                    CharacterNode new_character = new CharacterNode { Character = text[i], Frequency = 1 };
                    data_list.Add(new_character);
                }
            }
            return data_list;
        }

        public List<CharacterNode> SetPercentages(List<CharacterNode> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                double frequency = Convert.ToDouble(list[i].Frequency);
                double total = Convert.ToDouble(count_chars);
                double percentage = frequency / total;

                list[i].Percentage = percentage;
            }
            return list;
        }

        public void SetMetaData(List<CharacterNode> list)
        {
            //number of different characters
            byte[] count_byte = new byte[] { Convert.ToByte(list.Count) };
            metadata += Encoding.ASCII.GetString(count_byte);

            //number of bytes for each character frequencie
            int number_bytes = (1 + 1);
            byte[] number_byte = new byte[] { Convert.ToByte(number_bytes) };
            metadata += Encoding.ASCII.GetString(number_byte);


            for (int i = 0; i < list.Count; i++)
            {
                CharacterNode _node = list[i];
                byte[] frequency_byte = new byte[] { Convert.ToByte(_node.Frequency) };
                metadata += Convert.ToChar(_node.Character).ToString() + Encoding.ASCII.GetString(frequency_byte);
            }
        }

        public int DetectPositionCharacter(List<CharacterNode> data_list, byte character)
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

        private void SetQueue(List<CharacterNode> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                queue.Push(list[i]);
            }
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

        public static Comparison<CharacterNode> PriorityComparison = delegate (CharacterNode node1, CharacterNode node2)
        {
            if (node1.Percentage > node2.Percentage) return 1;
            if (node1.Percentage < node2.Percentage) return -1;
            return 0;

        };



    }
}
