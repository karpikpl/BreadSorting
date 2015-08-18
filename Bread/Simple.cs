using System;
using System.Collections.Generic;

namespace Bread
{
    class Simple
    {
        public static LinkedListNode<BreadPosition>[] OrderDictionary;

        public static void Swap<T>(LinkedListNode<T> node, LinkedListNode<T> toMoveBefore = null)
        {
            if (toMoveBefore == null)
                toMoveBefore = node.Previous.Previous;

            toMoveBefore.List.Remove(node);

            toMoveBefore.List.AddBefore(toMoveBefore, node);
        }

        public static void Solve(LinkedList<BreadPosition> list,
            LinkedListNode<BreadPosition>[] orderDictionary, int[] expected)
        {
            OrderDictionary = orderDictionary;

            MoveToBeginning(list, expected[0]);

            for (int i = 1; i < expected.Length; i++)
            {
                if (list.Count == 3)
                {
                    Verify(list, expected);
                    return;
                }

                if (!MoveTo(list, leftValue: expected[i - 1], value: expected[i]))
                {
                    Console.WriteLine("Impossible");
                    return;
                }
            }

            Console.WriteLine("Possible");
        }

        private static void Verify(LinkedList<BreadPosition> list, int[] expected)
        {
            Func
                <LinkedListNode<BreadPosition>, LinkedListNode<BreadPosition>,
                    LinkedListNode<BreadPosition>, bool> tester =
                        (a, b, c) =>
                        {
                            return a.Value.Value == expected[expected.Length - 3] &&
                                   b.Value.Value == expected[expected.Length - 2] & c.Value.Value == expected[expected.Length - 1];
                        };

            if (tester(list.First, list.First.Next, list.First.Next.Next))
            {
                Console.WriteLine("Possible");
                return;
            }

            Swap(list.Last);

            if (tester(list.First, list.First.Next, list.First.Next.Next))
            {
                Console.WriteLine("Possible");
                return;
            }

            Swap(list.Last);

            if (tester(list.First, list.First.Next, list.First.Next.Next))
            {
                Console.WriteLine("Possible");
                return;
            }

            Console.WriteLine("Impossible");
        }


        private static bool MoveTo(LinkedList<BreadPosition> list, int leftValue, int value)
        {
            var toMove = OrderDictionary[value];

            while (toMove.Previous.Value.Value != leftValue)
            {

                // easy case D x Z
                if (toMove.Previous.Previous.Value.Value == leftValue)
                {
                    // we have a problem, we need to use Next and we cannot touch Previous.Previous
                    if (toMove.Next == null)
                        return false;

                    Swap(toMove.Next);
                    Swap(toMove);

                    return toMove.Previous.Value.Value == leftValue;
                }

                // case x x Z
                Swap(toMove);
            }
            return true;
        }

        public static void MoveToBeginning(LinkedList<BreadPosition> list, int value)
        {
            var node = OrderDictionary[value];

            while (node.Previous != null)
            {
                // we need to swap
                // first case - we x x Z
                if (node.Previous.Previous != null)
                {
                    Swap(node);

                }
                else
                {
                    // case x Z x
                    Swap(node.Next);
                }
            }
        }
    }
}
