using System;
using FeudalMP.src.network.entity;
using FeudalMP.src.network.service;
using FeudalMP.src.util;

[System.Serializable]
public class Connect : INetworkMessage
{
    public INetworkMessage Deserialize(byte[] byteArray)
    {
        return NetworkMessageSerializer.Deserialize<Connect>(byteArray);
    }

    public void ExecuteClient()
    {
        Logger logger = new Logger(nameof(Connect));
        logger.Info("ExecuteClient() called");
    }

    public void ExecuteServer()
    {
        Logger logger = new Logger(nameof(Connect));
        logger.Info("ExecuteServer() called");
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