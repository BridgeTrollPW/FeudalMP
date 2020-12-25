using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace FeudalMP.src.network.entity
{
    public static class NetworkMessageSerializer
    {
        public static byte[] Serialize<T>(T input)
        {
            using (var ms = new MemoryStream())
            {
                new BinaryFormatter().Serialize(ms, input);
                return ms.ToArray();
            }
        }
        public static T Deserialize<T>(this byte[] byteArray)
        {
            using (var ms = new MemoryStream(byteArray))
            {
                return (T)new BinaryFormatter().Deserialize(ms);
            }
        }
    }
}