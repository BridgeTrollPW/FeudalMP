using FeudalMP.src.foundation;
using FeudalMP.src.network.entity;
using FeudalMP.src.network.server;
using FeudalMP.src.network.service;
using FeudalMP.src.util;

namespace FeudalMP.src.network.messages
{
    [System.Serializable]
    public class Connect : INetworkMessage
    {
        public string name;
        public bool success;
        public INetworkMessage Deserialize(byte[] byteArray)
        {
            return byteArray.Deserialize<Connect>();
        }

        public void ExecuteClient(int senderPeer)
        {
            Logger logger = new Logger(nameof(Connect));
            logger.Info("ExecuteClient() called");
            if (!success)
            {
                logger.Error("Server did not want to connect :'(");
            }

        }

        public void ExecuteServer(int senderPeer)
        {
            Logger logger = new Logger(nameof(Connect));
            logger.Info("ExecuteServer() called, Connect received with name=" + name);
            Server server = NodeTreeManager.Instance.ServiceLayer.GetNode<Server>("./Server");
            if (!server.Clients.ContainsKey(senderPeer))
            {
                server.NetworkMessageDispatcher.Dispatch(new ErrorMessage()
                {
                    Message = "The peer id is not present in already connected peers, disconnected",
                    Reason = "peer not properly connected"
                }, senderPeer);
            }
            server.Clients[senderPeer].Name = name;
            server.NetworkMessageDispatcher.Dispatch(new Connect
            {
                success = true
            }, senderPeer);
        }

        public NetworkMessageIdentifier GetNetworkMessageIdentifier()
        {
            return NetworkMessageIdentifier.CONNECT;
        }

        public byte[] Serialize()
        {
            return NetworkMessageSerializer.Serialize(this);
        }
    }
}