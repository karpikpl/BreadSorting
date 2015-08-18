using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Kattis.IO;

namespace Bread
{
    class Program
    {
        public static LinkedListNode<BreadPosition>[] OrderDictionary;
        public static int[] expected;

        private static void Main(string[] args)
        {
            Stopwatch sw = null;
            int size;
            try
            {
                Scanner reader;
                if (args.Length == 1)
                {
                    reader = new Scanner(File.OpenRead(args[0]));
                }
                else if (args.Length == 2)
                {
                    size = int.Parse(args[1]);
                    var testStream = Generator.LetsTest(size);
                    reader = new Scanner(testStream);
                }
                else
                {
                    reader = new Scanner();
                }

#if DEBUG
                sw = new Stopwatch();
                sw.Start();
#endif
                size = int.Parse(reader.ReadLine());

                // read data from stream
                LinkedList<BreadPosition> listWithDist = reader.ReadLineAsLinkList(size);

                // create dictionary with order
                CreateOrderDict(listWithDist);

                expected = reader.ReadLineAsTArray(size);

#if DEBUG
                if (args.Length > 0)
                {
                    Console.WriteLine("Init {0}", sw.Elapsed);
                }
#endif

                for (int i = 0; i < expected.Length; i++)
                {
                    if (listWithDist.Count == 3)
                    {
                        Simple.Solve(listWithDist, OrderDictionary, expected);
                        return;
                    }


                    MoveToBeginning(listWithDist, expected[i]);

#if DEBUG
                    if (args.Length > 1 && (args[1] == "v" || (args.Length > 2 && args[2] == "v")))
                    {
                        VerifyDist(listWithDist);
                        VerifyUnique(listWithDist);
                    }
#endif
                }

            }
            finally
            {
#if DEBUG
                sw.Stop();
                if (args.Length > 0)
                {
                    Console.WriteLine("{0} in {1}", args[0], sw.Elapsed);
                }
#endif
            }
        }

        private static void CreateOrderDict(LinkedList<BreadPosition> listWithDist)
        {
            var node = listWithDist.First;

            // create dictionary with order
            OrderDictionary = new LinkedListNode<BreadPosition>[listWithDist.Count + 1];

            for (int i = 0; i < listWithDist.Count; i++)
            {
                OrderDictionary[node.Value.Value] = node;
                node = node.Next;
            }
        }

        private static void VerifyUnique(LinkedList<BreadPosition> listWithDist)
        {
            var ok = listWithDist.Select(d => d.Value).Distinct().Count() == listWithDist.Count;

            if (!ok)
                throw new Exception("Unique");
        }

        private static void VerifyDist(LinkedList<BreadPosition> listWithDist)
        {
            var node = listWithDist.First;

            while (node.Next != null)
            {
                var ok = node.Next.Value.Index == node.Value.Index + 1;
                node = node.Next;

                if (!ok)
                    throw new Exception("Index");
            }
        }

        public static bool MoveToBeginning(LinkedList<BreadPosition> list, int value)
        {
            // first Find first
            var node = OrderDictionary[value];

            int distance = node.Value.Index - RemovedForFreeCount;

            var movedNext = node.Next;
            var movedNextNext = node.Next != null ? node.Next.Next : null;

            if (node.Previous != null)
            {
                // remove from the list because after it will be first it's no longer neded
                list.Remove(node);

                if (distance % 2 != 0)
                {
                    // clasic flip and switch
                    var first = list.First;
                    var second = list.First.Next;
                    list.Remove(first);
                    list.Remove(second);

                    list.AddBefore(list.First, first);
                    list.AddBefore(list.First, second);

                    // change distance value
                    list.First.Value = new BreadPosition { Index = RemovedForFreeCount, Value = list.First.Value.Value };
                    list.First.Next.Value = new BreadPosition { Index = RemovedForFreeCount + 1, Value = list.First.Next.Value.Value };

                    //// in case we moved one node neighbours
                    if (first == movedNext || second == movedNext)
                    {
                        movedNext = movedNextNext;
                    }
                }

                // here we need to put a value in the place of the removed one so we don't have to modify indexes
                //1.    x x x x x x x x 3 x x x Y
                //2.  3 x x x x x x x x _ x x x Y
                //3.    x x x x x x x x Y x x x

                if (list.Count > 5 && (movedNextNext != list.Last) && (movedNext != null))
                {
                    if ((list.Last.Value.Index - movedNext.Value.Index - 1) % 2 == 0)
                    {
                        var toMove = list.Last.Previous;
                        toMove.Value = toMove.Value.CreateCopyWithNewIndex(movedNext.Value.Index - 1);
                        list.Last.Value = list.Last.Value.CreateCopyWithDecrementedIndex();
                        list.Remove(toMove);

                        list.AddBefore(movedNext, toMove);
                    }
                    else
                    {
                        var toMove = list.Last;

                        toMove.Value = toMove.Value.CreateCopyWithNewIndex(movedNext.Value.Index - 1);

                        if (movedNext != toMove)
                        {
                            list.Remove(toMove);
                            list.AddBefore(movedNext, toMove);
                        }
                    }
                }
                else
                    // when there was less than 5 elements - just do it old way
                    while (movedNext != null)
                    {
                        movedNext.Value = movedNext.Value.CreateCopyWithDecrementedIndex();

                        movedNext = movedNext.Next;
                    }
            }
            else
            {
                // node was first so just remove it and increase the 'removed free counter'
                list.Remove(node);
                RemovedForFreeCount++;
            }

            return false;
        }

        /// <summary>
        /// Number of nodes removed for free (from the beginning)
        /// </summary>
        private static int RemovedForFreeCount = 0;
    }

    public struct BreadPosition
    {
        public int Index;
        public int Value;

        public override string ToString()
        {
            return String.Format("[{1}]={0}", Index, Value);
        }

        public BreadPosition CreateCopyWithDecrementedIndex()
        {
            return new BreadPosition { Index = this.Index - 1, Value = this.Value };
        }

        public BreadPosition CreateCopyWithNewIndex(int distance)
        {
            return new BreadPosition { Index = distance, Value = this.Value };
        }
    }
}
