public enum GameServerPackets
{
    Movement = 1,
    ServerInfo = 2,
    Handshake = 3,
    StartServer = 4
}

public enum GameClientPackets
{
    Movement = 1,
    LobbyRequest = 2,
    JoinRequest = 3,
    Handshake = 4,
    ReadyUp = 5
}