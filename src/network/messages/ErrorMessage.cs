using System;
using FeudalMP.src.network.entity;
using FeudalMP.src.network.service;
using FeudalMP.src.util;

namespace FeudalMP.src.network.messages
{
    [Serializable]
    public class ErrorMessage : INetworkMessage
    {
        private string message;
        private string reason;

        public string Message { get => message; set => message = value; }
        public string Reason { get => reason; set => reason = value; }

        public INetworkMessage Deserialize(byte[] byteArray)
        {
            return byteArray.Deserialize<ErrorMessage>();
        }

        public void ExecuteClient(int senderPeer)
        {
            Logger log = new Logger(nameof(ErrorMessage));
            log.Error(string.Format("Server sent an error to your client reason={0} message={1}", reason, message));
        }

        public void ExecuteServer(int senderPeer)
        {
            Logger log = new Logger(nameof(ErrorMessage));
            log.Error(string.Format("Client sent an error to this server reason={0} message={1}", reason, message));
        }

        public NetworkMessageIdentifier GetNetworkMessageIdentifier()
        {
            return NetworkMessageIdentifier.ERROR_MESSAGE;
        }

        public byte[] Serialize()
        {
            return NetworkMessageSerializer.Serialize(this);
        }
    }
}