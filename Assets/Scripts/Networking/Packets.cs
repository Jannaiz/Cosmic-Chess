public enum GameServerPackets
{
    ErrorMessage = -1,
    Handshake = 1,
    ReceivePublicLobbys = 2,
    ServerInfo = 3,
    StartGame = 4,
    Movement = 5,
    GetLobbyCode = 6,
    ServerMessage = 7
}

public enum GameClientPackets
{
    Handshake = 1,
    GetPublicLobbys = 2,
    JoinRequest = 3,
    ReadyUp = 4,
    Movement = 5,
    CreateLobby = 6,
    PlayerMessage = 7
}