using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;

namespace Oyster.Examples.ProtoBuf
{
    internal class Program
    {
        private const int OfficeCount = 5;
        private const int EmployeeCount = 6;
        private const int TaskCount = 3;
        private const int IterationCount = 20000;

        private static void Main()
        {
            var binaryFormatter = new BinaryFormatter();
            var protoFormatter = new ProtoFormatter();
            TestSerializers(
                Tuple.Create("BinaryFormatter", (Action<Stream, object>)binaryFormatter.Serialize, (Func<Stream, object>)binaryFormatter.Deserialize),
                Tuple.Create("protobuf-net v2", (Action<Stream, object>)protoFormatter.Serialize, (Func<Stream, object>)protoFormatter.Deserialize));
        }

        private static void TestSerializers(params Tuple<string, Action<Stream, object>, Func<Stream, object>>[] serializers)
        {
            var data = GenerateData();
            var results = serializers
                .Select(t => new { Name = t.Item1, TimeSize = TestSerializer(IterationCount, data, t.Item2, t.Item3) })
                .ToList();

            Console.WriteLine("Serialization of {0} objects, {1} iterations:", OfficeCount * EmployeeCount * TaskCount, IterationCount);
            Console.WriteLine();

            WriteResult(string.Empty, "Duration(ms)", "Size(bytes)");
            foreach (var result in results)
            {
                WriteResult(result.Name, result.TimeSize.Item1.ToString(), result.TimeSize.Item2.ToString());
            }

            Console.ReadLine();
        }

        private static Tuple<int, int> TestSerializer(
            int iterationCount,
            object data,
            Action<Stream, object> serializeFunc,
            Func<Stream, object> deserializeFunc)
        {
            using (var ms = new MemoryStream())
            {
                int sizeBytes = 0;
                var sw = Stopwatch.StartNew();
                while (iterationCount-- > 0)
                {
                    ms.Position = 0;
                    serializeFunc(ms, data);
                    ms.Flush();
                    
                    sizeBytes = (int)ms.Position;

                    ms.Position = 0;
                    deserializeFunc(ms);
                    ms.Flush();
                }
                sw.Stop();

                return Tuple.Create((int)sw.ElapsedMilliseconds, sizeBytes);
            }
        }

        private static Office[] GenerateData()
        {
            return Enumerable.Repeat(0, OfficeCount).Select(_ => RandomData.GenerateOffice(EmployeeCount, TaskCount)).ToArray();
        }

        private static void WriteResult(string name, string duration, string size)
        {
            Console.WriteLine("{0}{1}{2}", name.PadRight(30), duration.PadLeft(15), size.PadLeft(15));
        }
    }
}
