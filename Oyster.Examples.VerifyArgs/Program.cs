using System;
using System.Diagnostics;
using VerifyArgs;

namespace Oyster.Examples.VerifyArgs
{
	public static class Program
    {
        private const int IterationCount = 1000000;

        public static void Main()
        {
			string s1 = "1";
			string s2 = "2";

            // Init
            Verify.Args(new { s1, s2 }).NotNull().NotEmpty();

            Measure("Native",
                () =>
                {
                    for (int i = 0; i < IterationCount; i++)
                    {
                        if (s1 == null)
                        {
                            throw new ArgumentNullException("s1");
                        }
                        if (s2 == null)
                        {
                            throw new ArgumentNullException("s2");
                        }
                        if (s1.Length == 0)
                        {
                            throw new ArgumentException("Value can't be empty.", "s1");
                        }
                        if (s2.Length == 0)
                        {
                            throw new ArgumentException("Value can't be empty.", "s2");
                        }
                    }
                });
            Measure("UsualHelper",
                () =>
                {
                    for (int i = 0; i < IterationCount; i++)
                    {
                        VerifyUtil.NotNullOrEmpty(s1, "s1");
                        VerifyUtil.NotNullOrEmpty(s2, "s2");
                    }
                });
            Measure("VerifyArgs",
                () =>
                {
                    for (int i = 0; i < IterationCount; i++)
                    {
                        Verify.Args(new { s1, s2 }).NotNull().NotEmpty();
                    }
                });

            Console.ReadLine();
        }

        private static void Measure(string title, Action action)
        {
            var sw = Stopwatch.StartNew();
            action();
            sw.Stop();
			Console.WriteLine("{0}: {1}ns", title, Math.Round(1000000.0 / IterationCount * sw.Elapsed.TotalMilliseconds, MidpointRounding.AwayFromZero));
        }
    }
}
