using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LAB_3___Compressor.DataStructures;

namespace LAB_3___Compressor.Huffman_Coding
{
    public class Huffman : Interfaces.ICompressionAlgorithm
    {
        Dictionary<byte, string> codes_encode = new Dictionary<byte, string>();
        Dictionary<string, byte> codes_decode = new Dictionary<string, byte>();

        PriorityQueue<CharacterNode> queue = new PriorityQueue<CharacterNode>();
        List<byte> metadata = new List<byte>();

        int count_chars;
        double original_weight;
        double compressed_weight;

        public byte[] EncodeData(byte[] content)
        {
            count_chars = content.Length;
            queue.PriorityComparison = PriorityComparison;

            //count all frequencies
            List<CharacterNode> base_frequencies = ProbeFrequencies(content);

            CharacterNode root = CreateTree(base_frequencies);
            GenerateCodes(root, "");

            //swap each char for its binary code
            string content_encoded = "";
            for (int i = 0; i < content.Length; i++)
            {
                content_encoded += codes_encode[content[i]];
            }

            //complete the last octet
            while (content_encoded.Length % 8 != 0)
            {
                content_encoded += "0";
            }

            //converting each binary code to a decimal number and then converting to byte
            List<byte> encode_final = metadata;
            for (int i = 0; i < content_encoded.Length; i += 8)
            {
                string code_bin = content_encoded.Substring(i, 8);
                int code_dec = Convert.ToInt32(code_bin, 2);
                encode_final.Add(Convert.ToByte(code_dec));
            }

            original_weight = content.Length;
            compressed_weight = encode_final.Count;
             
            return encode_final.ToArray();
        }


        private CharacterNode CreateTree(List<CharacterNode> list)
        {

            List<CharacterNode> list_complete = SetPercentages(list);

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


        private void GenerateCodes(CharacterNode root, string current_code)
        {
            if (root.Left == null && root.Right == null)
            {
                root.Code = current_code;
                codes_encode.Add(root.Character, root.Code);
                codes_decode.Add(root.Code, root.Character);
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

        private List<CharacterNode> ProbeFrequencies(byte[] text)
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

        private List<CharacterNode> SetPercentages(List<CharacterNode> list)
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

        private void SetMetaData(List<CharacterNode> list)
        {
            //number of different characters
            metadata.Add(Convert.ToByte(list.Count));

            //number of bytes for each character frequencie
            int number_bytes = (1 + 1);
            metadata.Add(Convert.ToByte(number_bytes));


            for (int i = 0; i < list.Count; i++)
            {
                CharacterNode _node = list[i];

                metadata.Add(Convert.ToByte(_node.Character));
                metadata.Add(Convert.ToByte(_node.Frequency));
            }
        }

        private int DetectPositionCharacter(List<CharacterNode> data_list, byte character)
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

        public byte[] DecodeData(byte[] content)
        {
            int characters = content[0];
            int lenght = content[1];

            //generates the codes with the metadata
            queue.PriorityComparison = PriorityComparison;
            List<CharacterNode> list = new List<CharacterNode>();
            int count = (characters * lenght)/2;
            for (int i = lenght; i <= count*lenght; i+=lenght)
            {
                CharacterNode node = new CharacterNode { Character = content[i], Frequency = content[i + 1] };
                count_chars += content[i + 1];
                list.Add(node);
            }
            CharacterNode root = CreateTree(list);
            GenerateCodes(root, "");

            //convert the bytes to binary code
            string content_decoded = "";
            int start = (characters * lenght) + 2;
            for (int i = start; i < content.Length; i++)
            {
                int dec = content[i];
                string bin = Convert.ToInt32(Convert.ToString(dec, 2)).ToString("D8"); ;
                content_decoded += bin;
            }

            //replace all binary code to the equivalent byte
            List<byte> result = new List<byte>();
            string current_code = "";

            int replaced = 0;
            int j = 0;
            while (replaced<count_chars)
            {
                current_code += content_decoded[j];
                if (codes_decode.ContainsKey(current_code))
                {
                    result.Add(codes_decode[current_code]);
                    current_code = "";
                    replaced++;
                }
                j++;
            }

            return result.ToArray();
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

        private static Comparison<CharacterNode> PriorityComparison = delegate (CharacterNode node1, CharacterNode node2)
        {
            if (node1.Percentage > node2.Percentage) return 1;
            if (node1.Percentage < node2.Percentage) return -1;
            return 0;

        };



    }
}
