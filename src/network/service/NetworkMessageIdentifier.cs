namespace FeudalMP.src.network.service
{
    public enum NetworkMessageIdentifier : short
    {
        CONNECT,
        INITIAL_SYNC,
        SYNC,
        ERROR_MESSAGE,

        POSITION_ROTATION_UPDATE,
        DISCONNECT
    }
}