namespace FeudalMP.src.network.service
{
    public enum NetworkMessageIdentifier : short
    {
        CONNECT,
        INITIAL_SYNC,
        SYNC,
        ERROR_MESSAGE,
        POSITION_ROTATION_UPDATE,
        DISCONNECT,
        REQUEST_PLAYER_LIST,
        SPAWN_OBJECT_MESSAGE
    }
}