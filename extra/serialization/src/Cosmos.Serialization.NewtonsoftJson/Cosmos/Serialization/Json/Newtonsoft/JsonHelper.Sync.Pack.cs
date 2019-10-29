using System;
using System.IO;
using Newtonsoft.Json;

namespace Cosmos.Serialization.Json.Newtonsoft
{
    /// <summary>
    /// Newtonsoft Json Helper
    /// </summary>
    public static partial class JsonHelper
    {
        /// <summary>
        /// Pack
        /// </summary>
        /// <param name="o"></param>
        /// <param name="settings"></param>
        /// <param name="withNodaTime"></param>
        /// <returns></returns>
        public static Stream Pack(object o, JsonSerializerSettings settings = null, bool withNodaTime = false)
        {
            var ms = new MemoryStream();

            if (o is null)
                return ms;

            Pack(o, ms, settings, withNodaTime);

            return ms;
        }

        /// <summary>
        /// Pack
        /// </summary>
        /// <param name="o"></param>
        /// <param name="stream"></param>
        /// <param name="settings"></param>
        /// <param name="withNodaTime"></param>
        public static void Pack(object o, Stream stream, JsonSerializerSettings settings = null, bool withNodaTime = false)
        {
            if (o is null)
                return;

            var bytes = JsonHelper.SerializeToBytes(o, settings, withNodaTime);

            stream.Write(bytes, 0, bytes.Length);
        }

        /// <summary>
        /// Unpack
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="settings"></param>
        /// <param name="withNodaTime"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T Unpack<T>(Stream stream, JsonSerializerSettings settings = null, bool withNodaTime = false)
        {
            return stream is null
                ? default
                : JsonHelper.Deserialize<T>(JsonManager.DefaultEncoding.GetString(StreamToBytes(stream)), settings, withNodaTime);
        }

        /// <summary>
        /// Unpack
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="type"></param>
        /// <param name="settings"></param>
        /// <param name="withNodaTime"></param>
        /// <returns></returns>
        public static object Unpack(Stream stream, Type type, JsonSerializerSettings settings = null, bool withNodaTime = false)
        {
            return stream is null
                ? default
                : JsonHelper.Deserialize(JsonManager.DefaultEncoding.GetString(StreamToBytes(stream)), type, settings, withNodaTime);
        }

        private static byte[] StreamToBytes(Stream stream)
        {
            var bytes = new byte[stream.Length];
            
            if (stream.Position > 0 && stream.CanSeek)
                stream.Seek(0, SeekOrigin.Begin);
            
            stream.Read(bytes, 0, bytes.Length);
            
            if (stream.CanSeek) 
                stream.Seek(0, SeekOrigin.Begin);
            
            return bytes;
        }
    }
}