using System;
using System.Collections.Generic;
using System.IO;
using Bread;

namespace Kattis.IO
{
    public class NoMoreTokensException : Exception
    {
    }

    public class Tokenizer
    {
        string[] tokens = new string[0];
        private int pos;
        StreamReader reader;

        public Tokenizer(Stream inStream)
        {
            var bs = new BufferedStream(inStream);
            reader = new StreamReader(bs);
        }

        public Tokenizer()
            : this(Console.OpenStandardInput())
        {
            // Nothing more to do
        }

        void ReadIntTable(string input, List<int> output)
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

        private string PeekNext()
        {
            if (pos < 0)
                // pos < 0 indicates that there are no more tokens
                return null;
            if (pos < tokens.Length)
            {
                if (tokens[pos].Length == 0)
                {
                    ++pos;
                    return PeekNext();
                }
                return tokens[pos];
            }
            string line = reader.ReadLine();
            if (line == null)
            {
                // There is no more data to read
                pos = -1;
                return null;
            }
            // Split the line that was read on white space characters
            tokens = line.Split(null);
            pos = 0;
            return PeekNext();
        }

        public bool HasNext()
        {
            return (PeekNext() != null);
        }

        public string Next()
        {
            string next = PeekNext();
            if (next == null)
                throw new NoMoreTokensException();
            ++pos;
            return next;
        }

        public string ReadLine()
        {
            return reader.ReadLine();
        }

        public int[] ReadLineAsTArray(int size)
        {
            var array = new int[size];

            int next = 0;
            int index = 0;
            var line = reader.ReadLine();

            foreach (char c in line)
            {
                if (c == ' ')
                {
                    array[index] = next;
                    index++;
                    next = 0;
                }
                else
                {
                    next *= 10;
                    next += c - '0';
                }
            }

            if (index != size)
            {
                array[index] = next;
                index++;
            }
            return array;
        }

        public LinkedList<BreadPosition> ReadLineAsLinkList(int size)
        {
            var list = new LinkedList<BreadPosition>();

            int next = 0;
            int index = 0;
            var line = reader.ReadLine();

            foreach (char c in line)
            {
                if (c == ' ')
                {
                    if (list.Count == 0)
                    {
                        list.AddFirst(new BreadPosition { Index = index, Value = next });
                    }
                    else
                    {
                        list.AddAfter(list.Last, new BreadPosition { Index = index, Value = next });                        
                    }
                    index++;
                    next = 0;
                }
                else
                {
                    next *= 10;
                    next += c - '0';
                }
            }

            if (index != size)
            {
                list.AddAfter(list.Last, new BreadPosition { Index = index, Value = next });
                index++;
            }
            return list;
        }
    }


    public class Scanner : Tokenizer
    {
        public Scanner(Stream inStream)
            : base(inStream)
        {
        }

        public Scanner()
        {
            // Nothing more to do
        }


        public int NextInt()
        {
            return int.Parse(Next());
        }

        public long NextLong()
        {
            return long.Parse(Next());
        }

        public float NextFloat()
        {
            return float.Parse(Next());
        }

        public double NextDouble()
        {
            return double.Parse(Next());
        }
    }


    public class BufferedStdoutWriter : StreamWriter
    {
        public BufferedStdoutWriter()
            : base(new BufferedStream(Console.OpenStandardOutput()))
        {
        }
    }

}
