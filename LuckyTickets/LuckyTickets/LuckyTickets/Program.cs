/* NOT COMPLETED. Will revisit later.
 * 
 * INPUT:
 * 4
 * 6
 * 8
 * OUTPUT:
 * 670
 * 55252
 * 4816030
 * Idea 1: Use permutation to figure out the number of occurences of two halves of n digits that sum to r.
 *      n!/n-d+1 where d == the number of distinct digits in half of n.
 * Idea 2: Use char arrays to generate 0 to 9 of n characters instead of using numbers.
 * 
 * Spoiler:
 * http://math.mit.edu/~rothvoss/18.304.1PM/Presentations/1-Noor-LuckyTickets.pdf
 * Reddit comment:
 * https://www.reddit.com/r/learnmath/comments/453quw/mathematical_coding_problem/
 * I made an efficient implementation of /u/eruonna's idea using dynamic programming. Gist Link
   The idea is to have an intermediate function c = f(s,k) that takes the sum s and the number of digits k and calculates the number of combinations c of digits that make up the sum. Example: f(2,3) = 6. The combinations are (0,0,2),(0,2,0),(2,0,0),(0,1,1),(1,0,1),(1,1,0).
   You can use that function to get the solution
   #lucky_tickets = sum{i=0 to (9*n/2)}{ f(i, n/2)^2 }
   But how do you calculate f ?
   You can define it recursively:
   f(s, 1) = 1, if s < 10
   f(s, 1) = 0, if s >= 10
   f(s, k) = sum{i=0 to s}{ f(i, floor(k/2)) * f(s-i, ceil(k/2)) }
   The idea behind the recursion is a divide and conquer strategy: Split the digits in two groups and demand that the left group adds up to i and the right group adds up to s-i. Multiply to get all combinations from the left and right group. Then sum over all possible splits i of the sum.
   Now we have the solution on a mathematical level but evaluating f for large arguments will take forever because of the recursion. Here dynamic programming comes to the rescue. If you run through the recursion you will notice that f is frequently evaluated with the same arguments over and over again. So it would make sense to cache and reuse the results. But dynamic programming goes a step further and builds the results from the bottom up. Note that the arguments for f only get smaller in the recursion.
   You can build an array F[k][s] that contains all return values of f, start by writing the first row F[1][s] which is trivial, then the following rows F[2][s], F[3][s], ... only depend on the values already saved in F.
 */
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace LuckyTickets
{
    class Program
    {
        const int NINE = 9;
        static void Main(string[] args)
        {
            var dict = new Dictionary<int, BigInteger>();

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

            BigInteger result = 0;
            BigInteger sum = 0;
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
                Swap(ref sb, start, i);
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
