using System;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using IntXLib;
using Oyster.Examples.IntXTest.Math;

namespace Oyster.Examples.IntXTest
{
    internal class Program
    {
        private const int MaxNExp = 6;
        private const int MaxNDigits = MaxNExp + 1;

        private static void Main()
        {
            var indexes = Enumerable.Range(1, MaxNExp).Select(exp => (int)System.Math.Round(System.Math.Pow(10, exp))).ToList();

            // Measure performance
            Console.WriteLine("Fibonacci numbers calculation time (in ms):");
            Console.WriteLine();
            Console.WriteLine("              BigInt    IntX  BigInt    IntX");
            Console.WriteLine("                              string  string");
            foreach (var n in indexes)
            {
                Func<Action, string> measure =
                    action =>
                        {
                            var sw = Stopwatch.StartNew();
                            action();
                            sw.Stop();
                            return sw.ElapsedMilliseconds.ToString().PadLeft(8);
                        };

                var fib = Algorithms<IntX>.Fib(n).ToString();
                Console.WriteLine(
                    "fib({0}){1}{2}{3}{4}",
                    n.ToString().PadRight(MaxNDigits),
                    measure(() => Algorithms<BigInteger>.Fib(n)),
                    measure(() => Algorithms<IntX>.Fib(n)),
                    measure(() => Algorithms<BigInteger>.Fib(n).ToString()),
                    measure(() => Algorithms<IntX>.Fib(n).ToString()));
            }

            // Write fibonacci values
            Console.WriteLine();
            Console.WriteLine("Fibonacci numbers:");
            Console.WriteLine();
            foreach (var n in indexes)
            {
                var fib = Algorithms<IntX>.Fib(n).ToString();
                Console.WriteLine(
                    "fib({0}) = {1} ({2} digits)",
                    n.ToString().PadRight(MaxNDigits),
                    fib.Length <= 23 ? fib.PadRight(23) : fib.Substring(0, 10) + "..." + fib.Substring(fib.Length - 10),
                    fib.Length);
            }

            Console.WriteLine();
            Console.WriteLine("Press Enter to close...");
            Console.ReadLine();
        }
    }
}
