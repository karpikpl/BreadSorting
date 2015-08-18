using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Bread
{
    class Generator
    {
        static int ReadInt(string input)
        {
            int next = 0;
            foreach (char c in input)
            {
                if (c == '\n')
                    break;

                next *= 10;
                next += c - '0';
            }

            return next;
        }

        static void ReadIntTable(string input, List<int> output)
        {
            int next = 0;
            foreach (char c in input)
            {
                if (c == ' ')
                {
                    output.Add(next);
                    next = 0;
                }
                else
                {
                    next *= 10;
                    next += c - '0';
                }
            }

            output.Add(next);
        }

        static void RedefineProblem(List<int> pattern, List<int> data, List<int> dataToSort)
        {
            int[] dict = new int[100001];
            for (int pix = 0; pix < pattern.Count; pix++)
            {
                dict[pattern[pix]] = pattern.Count - pix;
            }

            foreach (int d in data)
            {
                dataToSort.Add(dict[d]);
            }
        }


        public static string GenRandomizeData(int limit, bool swap)
        {
            // make pattern
            List<int> pattern = new List<int>();
            for (int i = limit; i > 0; i--)
            {
                pattern.Add(i);
            }

            // swap
            if (swap)
            {
                int tmp = pattern[pattern.Count - 2];
                pattern[pattern.Count - 2] = pattern[pattern.Count - 1];
                pattern[pattern.Count - 1] = tmp;
            }

            // add random padding
            for (int r = 0; r < rnd.Next(100); r++)
                RandomPad(pattern);

            // to string
            StringBuilder builder = new StringBuilder();
            foreach (int p in pattern)
            {
                builder.Append(p);
                builder.Append(" ");
            }

            return builder.ToString().Trim();
        }

        public static string GenPatternData(int limit)
        {
            StringBuilder builder = new StringBuilder();
            for (int i = limit; i > 0; i--)
            {
                builder.Append(i);
                builder.Append(" ");
            }

            return builder.ToString().Trim();
        }

        private static Random rnd = new Random(DateTime.Now.Millisecond);
        public static void RandomPad(List<int> data)
        {
            int index = rnd.Next(data.Count - 2);
            int tmp = data[index + 2];
            data[index + 2] = data[index + 1];
            data[index + 1] = data[index + 0];
            data[index + 0] = tmp;
        }


        public static Stream LetsTest(int size)
        {
            StreamWriter writer = new StreamWriter(new MemoryStream());
            writer.WriteLine(size);

            string line1 = GenRandomizeData(size, false);
            string line2 = GenRandomizeData(size, false);

            Console.WriteLine(line1);
            Console.WriteLine(line2);

            writer.WriteLine(line1);
            writer.WriteLine(line2);
            writer.Flush();
            writer.BaseStream.Seek(0, SeekOrigin.Begin);
            File.WriteAllText("current" + DateTime.Now.ToString("hhmmssz") + ".in", string.Format("{0}\n{1}\n{2}", size, line1, line2));

            return writer.BaseStream;
        }
    }
}
