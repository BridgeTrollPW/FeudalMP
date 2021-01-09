using System;
using Godot;

namespace FeudalMP.src.network.server.entity
{
    [Serializable]
    public class GameClient
    {
        private int id;
        private string name;
        private Vector3 rotation;
        private Vector3 translation;

        [NonSerialized]
        private GameClientState state;

        public int Id { get => id; set => id = value; }
        public GameClientState State { get => state; set => state = value; }
        public string Name { get => name; set => name = value; }
        public Vector3 Translation { get => translation; set => translation = value; }
        public Vector3 Rotation { get => rotation; set => rotation = value; }

        public GameClient()
        {
            state = GameClientState.CONNECTING;
        }
    }

    public enum GameClientState
    {
        //Client peer now connected to the server, no further logic done
        CONNECTING,
        //syncing meta data between client <-> server like server data, authentication, map data
        INITIAL_SYNC,
        //Starting to sync already connected players, push this client to all other clients
        SYNC,
        //client is active with pysical player representation
        ACTIVE,
        //timeout/package transmission is inconsistent
        TIMEOUT,
        //client requests a disconnect
        DISCONNECTING,
        //Client has no connection to the server anymore
        GONE
    }
}