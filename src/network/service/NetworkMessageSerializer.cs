using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace FeudalMP.src.network.entity
{
    public static class NetworkMessageSerializer
    {
        public static byte[] Serialize<T>(T input) where T : INetworkMessage
        {
            using (var ms = new MemoryStream())
            {
                new BinaryFormatter().Serialize(ms, input.GetData());
                return ms.ToArray();
            }
        }
        public static T Deserialize<T>(this byte[] byteArray) where T : INetworkMessage
        {
            using (var ms = new MemoryStream(byteArray))
            {
                return (T)System.Activator.CreateInstance(typeof(T), new BinaryFormatter().Deserialize(ms));
            }
        }
    }
}