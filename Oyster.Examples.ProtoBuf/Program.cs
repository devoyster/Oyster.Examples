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
        private const int IterationCount = 1000;

        private static void Main()
        {
            Func<string, Action<Stream, object>, Func<Stream, object>, Tuple<string, Action<Stream, object>, Func<Stream, object>>> serializer = Tuple.Create;

            var binaryFormatter = new BinaryFormatter();
            var protoFormatter = new ProtoFormatter();
            TestSerializers(
                serializer("BinaryFormatter", binaryFormatter.Serialize, binaryFormatter.Deserialize),
                serializer("protobuf-net v2", protoFormatter.Serialize, protoFormatter.Deserialize));
        }

        private static void TestSerializers(params Tuple<string, Action<Stream, object>, Func<Stream, object>>[] serializers)
        {
            var data = GenerateData();
            var results = serializers
                .Select(t => new { Name = t.Item1, TimeSize = TestSerializer(IterationCount, data, t.Item2, t.Item3) })
                .ToList();

            Console.WriteLine("Serialization of {0} objects, {1} iterations:", OfficeCount * EmployeeCount * TaskCount, IterationCount);
            Console.WriteLine();

            WriteResult(string.Empty, "Write(ms)", "Read(ms)", "Size(bytes)");
            foreach (var result in results)
            {
                WriteResult(result.Name, result.TimeSize.Item1.ToString(), result.TimeSize.Item2.ToString(), result.TimeSize.Item3.ToString());
            }
        }

        private static Tuple<int, int, int> TestSerializer(
            int iterationCount,
            object data,
            Action<Stream, object> serializeFunc,
            Func<Stream, object> deserializeFunc)
        {
            // Do one run to eliminate init time
            using (var ms = new MemoryStream())
            {
                serializeFunc(ms, data);
                ms.Flush();
                ms.Position = 0;
                deserializeFunc(ms);
            }

            using (var ms = new MemoryStream())
            {
                int sizeBytes = 0;
                var serializeWatch = new Stopwatch();
                var deserializeWatch = new Stopwatch();
                while (iterationCount-- > 0)
                {
                    ms.Position = 0;
                    serializeWatch.Start();
                    serializeFunc(ms, data);
                    ms.Flush();
                    serializeWatch.Stop();
                    
                    sizeBytes = (int)ms.Position;

                    ms.Position = 0;
                    deserializeWatch.Start();
                    deserializeFunc(ms);
                    ms.Flush();
                    deserializeWatch.Stop();
                }

                return Tuple.Create((int)serializeWatch.ElapsedMilliseconds, (int)deserializeWatch.ElapsedMilliseconds, sizeBytes);
            }
        }

        private static Office[] GenerateData()
        {
            return Enumerable.Repeat(0, OfficeCount).Select(_ => RandomData.GenerateOffice(EmployeeCount, TaskCount)).ToArray();
        }

        private static void WriteResult(string name, string serializeDuration, string deserializeDuration, string size)
        {
            Console.WriteLine(name.PadRight(20) + serializeDuration.PadLeft(10) + deserializeDuration.PadLeft(10) + size.PadLeft(13));
        }
    }
}
