using System;
using System.Collections.Generic;
using System.Linq;
using FeudalMP.src.network.entity;
using FeudalMP.src.util;
using Godot;


namespace FeudalMP.src.network.service
{
    public class NetworkMessageDispatcher : Node
    {
        private readonly Dictionary<NetworkMessageIdentifier, INetworkMessage> MessageRegister = new Dictionary<NetworkMessageIdentifier, INetworkMessage>();
        private readonly Logger logger = new Logger(nameof(NetworkMessageDispatcher));

        public override void _Ready()
        {
            Name = "NetworkMessageDispatcher";
        }
        public Error Dispatch(INetworkMessage networkMessage, int peerId, NetworkedMultiplayerPeer.TransferModeEnum mode)
        {
            //short range should be enough, over 32k possible different message types
            byte[] identifierBytes = BitConverter.GetBytes((short)networkMessage.GetNetworkMessageIdentifier());
            byte[] finalPackage = identifierBytes.Concat(networkMessage.Serialize()).ToArray();
            return GetTree().Multiplayer.SendBytes(finalPackage, peerId, mode);
        }

        public void Process(int senderPeer, byte[] networkMessageBytes)
        {
            //first two bytes should describe the GetNetworkMessageIdentifier
            //allocate new byte array with length of a short, which is 2 bytes
            byte[] identifierBytes = new byte[2];

            //copy the first two bytes from the whole message to our new identifierBytes
            Array.Copy(networkMessageBytes, 0, identifierBytes, 0, 2);
            //convert two bytes to short and short to NetworkMessageIdentifier Enum
            short enumId = BitConverter.ToInt16(identifierBytes, 0);
            if (!Enum.IsDefined(typeof(NetworkMessageIdentifier), enumId))
            {
                logger.Warn(string.Format("NetworkMessageIdentifier {0} does not exist", enumId));
                return;
            }
            NetworkMessageIdentifier networkMessageIdentifier = (NetworkMessageIdentifier)enumId;

            //fix remaining bytes to represent the actual message body
            System.Array.Copy(networkMessageBytes, 2, networkMessageBytes, 0, networkMessageBytes.Length - 2);
            if (!MessageRegister.ContainsKey(networkMessageIdentifier))
            {
                logger.Warn(string.Format("Received unregistered NetworkMessageIdentifier = {0}", networkMessageIdentifier));
                return;
            }
            INetworkMessage genericMessage = MessageRegister[networkMessageIdentifier].Deserialize(networkMessageBytes);

            logger.Info(string.Format("Received Message with {0} bytes. Relates to NetworkMessageIdentifier short={1} enum={2}", networkMessageBytes.Length, enumId, networkMessageIdentifier.ToString()));
            try
            {
                if (GetTree().Multiplayer.IsNetworkServer())
                {
                    genericMessage.ExecuteServer();
                }
                else
                {
                    genericMessage.ExecuteClient();
                }
            }
            catch (Exception e)
            {
                logger.Error(string.Format("Error executing received Message {0}", e));
            }
        }

        public void RegisterNetworkMessage(INetworkMessage networkMessage)
        {
            MessageRegister.Add(networkMessage.GetNetworkMessageIdentifier(), networkMessage);
        }
    }
}