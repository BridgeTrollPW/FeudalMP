using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using FeudalMP.src.network.entity;

namespace FeudalMP.src.network.service
{
    public interface INetworkMessageInvocation
    {
        INetworkMessageData Data { get; set; }
        ConcurrentQueue<INetworkMessage> MessageQueue { get; }
        NetworkMessageIdentifier GetNetworkMessageIdentifier();

        void ExecuteServer(int senderPeer);
        void ExecuteClient(int senderPeer);
    }
}