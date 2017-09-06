/*
 * 4
 * 6
 * 8
 * 670
 * 55252
 * 4816030
 */
using System;
using System.Collections.Generic;
using System.Text;

namespace LuckyTickets
{
    class Program
    {
        const int NINE = 9;
        static void Main(string[] args)
        {
            var dict = new Dictionary<int, ulong>();

            var n = int.Parse(Console.ReadLine());
            var half = n/2;
            uint max = NINE;
            for (var i = 1; i < half; i++)
            {
                max += NINE * (uint)(Math.Pow(10,i));
            }

            var visited = new HashSet<string>();
            for (var i = 0; i <= max; i++)
            {
                var hasVisited = Visited(visited, i, half);
                if (!hasVisited)
                {
                    var dividend = Factorial(half);
                    var distinctNumbers = GetDistinctNumbers(i, half);
                    var divisor = Factorial((half - distinctNumbers) + 1);
                    var permutation = dividend / divisor;

                    var key = GetSumOfCharacters(i, half);
                    if (dict.ContainsKey(key))
                    {
                        dict[key] += permutation;
                    }
                    else
                    {
                        dict.Add(key, permutation);
                    }
                }
            } 

            ulong result = 0;
            ulong sum = 0;
            foreach (var entry in dict)
            {
                sum += entry.Value;
                result += entry.Value * entry.Value;
            }

            Console.WriteLine(result);
        }

        private static bool Visited(HashSet<string> visited, int num, int length)
        {
            var paddedNum = num.ToString().PadLeft(length, '0');
            if (visited.Contains(paddedNum))
            {
                return true;
            }

            var sb = new StringBuilder(paddedNum);
            PermuteString(visited, sb, 0);

            return false;
        }

        private static void PermuteString(HashSet<string> visited, StringBuilder sb, int start)
        {
            if (start >= sb.Length)
            {
                if (!visited.Contains(sb.ToString()))
                {
                    visited.Add(sb.ToString());
                }
            }

            for (var i = start; i < sb.Length; i++)
            {
                    Swap(ref sb, i, start);
                    PermuteString(visited, sb, start + 1);
                    Swap(ref sb, start , i);
            }
        }

        private static void Swap(ref StringBuilder str, int i1, int i2)
        {
            var temp = str[i1];
            str[i1] = str[i2];
            str[i2] = temp;
        }


        private static int GetSumOfCharacters(int num, int length)
        {
            var str = num.ToString().PadLeft(length, '0');

            var sum = 0;
            for (var i = 0; i < str.Length; i++)
            {
                sum += int.Parse(str[i].ToString());
            }

            return sum;
        }

        private static int GetDistinctNumbers(int num, int length)
        {
            var str = num.ToString().PadLeft(length, '0');

            var hs = new HashSet<char>();
            for (var i = 0; i < str.Length; i++)
            {
                hs.Add(str[i]);
            }

            return hs.Count;
        }

        private static ulong Factorial(int n)
        {
            ulong result = 1;
            if (n == 1) { return result; }
            
            for (var i = 2; i <= n; i++)
            {
                result *= (ulong)i;
            }
            return result;
        }
    }
}
