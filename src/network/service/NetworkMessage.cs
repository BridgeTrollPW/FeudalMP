using FeudalMP.src.network.service;

namespace FeudalMP.src.network.entity
{
    public interface INetworkMessage
    {
        void ExecuteServer();
        void ExecuteClient();
        byte[] Serialize();
        INetworkMessage Deserialize(byte[] byteArray);
        NetworkMessageIdentifier GetNetworkMessageIdentifier();
    }
}