using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using ProtoBuf;
using ProtoBuf.Meta;

namespace Oyster.Examples.ProtoBuf
{
    public class ProtoFormatter
    {
        private static readonly Encoding Utf8Encoding = new UTF8Encoding(false);
        private static readonly ReaderWriterLockSlim TypeLock = new ReaderWriterLockSlim(LockRecursionPolicy.NoRecursion);
        private static readonly Dictionary<byte[], Type> TypeDict = new Dictionary<byte[], Type>(BytesComparer.Instance);
        private static readonly Dictionary<Type, byte[]> TypestampDict = new Dictionary<Type, byte[]>();

        #region Types Auto-Registering

        // Register known assembly types on startup
        static ProtoFormatter()
        {
            var model = RuntimeTypeModel.Default;

            // Obtain all serializable types having no explicit proto contract
            var serializableTypes = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(t => t.IsSerializable && !Attribute.IsDefined(t, typeof (ProtoContractAttribute)));
            foreach (var type in serializableTypes)
            {
                var metaType = model.Add(type, false);
                metaType.AsReferenceDefault = true;
                metaType.UseConstructor = false; // skip default constructor call

                // Add contract for all the serializable fields
                var serializableFields = type
                    .GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                    .Where(fi => !Attribute.IsDefined(fi, typeof (NonSerializedAttribute)))
                    .OrderBy(fi => fi.Name) // it's important to keep the same fields order in all the AppDomains
                    .Select((fi, index) => new {info = fi, index});
                foreach (var field in serializableFields)
                {
                    var metaField = metaType.AddField(field.index + 1, field.info.Name);
                    metaField.AsReference = !field.info.FieldType.IsValueType; // cyclic references support
                    metaField.DynamicType = field.info.FieldType == typeof (object); // any type support
                }
            }

            // Compile model in place for better performance, .Compile() can be used if all types are known beforehand
            model.CompileInPlace();
        }

        #endregion

        public object Deserialize(Stream stream)
        {
            if (stream == null)
            {
                throw new ArgumentNullException("stream");
            }

            return Serializer.NonGeneric.Deserialize(ReadType(stream), stream);
        }

        public void Serialize(Stream stream, object graph)
        {
            if (stream == null)
            {
                throw new ArgumentNullException("stream");
            }
            if (graph == null)
            {
                throw new ArgumentNullException("graph");
            }

            WriteType(stream, graph.GetType());
            Serializer.NonGeneric.Serialize(stream, graph);
        }

        #region Handling Type Info

        private static Type ReadType(Stream stream)
        {
            // Read type stamp directly from stream
            var b1 = (byte)stream.ReadByte();
            var b2 = (byte)stream.ReadByte();
            var b3 = (byte)stream.ReadByte();
            var b4 = (byte)stream.ReadByte();

            int length = b1
                | (b2 << 8)
                | (b3 << 16)
                | (b4 << 24);
            var typestamp = new byte[length];
            stream.Read(typestamp, 0, length);

            // Lookup without any encoding overheads
            Type type;
            TypeLock.EnterReadLock();
            try
            {
                if (TypeDict.TryGetValue(typestamp, out type)) return type;
            }
            finally
            {
                TypeLock.ExitReadLock();
            }

            // Resolve and store type
            type = Type.GetType(Utf8Encoding.GetString(typestamp));
            StoreTypeInfo(type, typestamp);
            return type;
        }

        private void WriteType(Stream stream, Type type)
        {
            byte[] typestamp;

            TypeLock.EnterReadLock();
            try
            {
                TypestampDict.TryGetValue(type, out typestamp);
            }
            finally
            {
                TypeLock.ExitReadLock();
            }

            if (typestamp == null)
            {
                string typeName = type.AssemblyQualifiedName;
                int i = typeName.IndexOf(',');
                if (i >= 0)
                {
                    i = typeName.IndexOf(',', i + 1);
                }
                if (i >= 0)
                {
                    typeName = typeName.Substring(0, i);
                }
                typestamp = Utf8Encoding.GetBytes(typeName);

                StoreTypeInfo(type, typestamp);
            }

            // Write directly into stream
            int length = typestamp.Length;
            stream.WriteByte((byte)length);
            stream.WriteByte((byte)(length >> 8));
            stream.WriteByte((byte)(length >> 16));
            stream.WriteByte((byte)(length >> 24));
            stream.Write(typestamp, 0, length);
        }

        private static void StoreTypeInfo(Type type, byte[] typestamp)
        {
            TypeLock.EnterWriteLock();
            try
            {
                TypeDict[typestamp] = type;
                TypestampDict[type] = typestamp;
            }
            finally
            {
                TypeLock.ExitWriteLock();
            }
        }

        #endregion
    }
}
