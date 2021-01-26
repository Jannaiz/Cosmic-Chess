using System;

[Serializable]
public class Location
{
    public int packetType;
    public string username;
    public Position startPos;
    public Position endPos;
}

[Serializable]
public class Position
{
    public int x;
    public int y;
}

[Serializable]
public class PacketChecker
{
    public int packetType;
}

[Serializable]
public class GetPublicLobbys
{
    public int packetType;
    public string username;
}

[Serializable]
public class LobbyAnswer
{
    public int packetType;
    public string[] lobbyCodes;
}

[Serializable]
public class LobbyRequest
{
    public int packetType;
    public string username;
    public int isPublic;
}

[Serializable]
public class JoinRequest
{
    public int packetType;
    public string username;
    public string lobbyCode;
}

[Serializable]
public class ClientHandshake
{
    public int packetType;
    public string username;
}

[Serializable]
public class ServerHandshake
{
    public int packetType;
    public int sessionId;
    public string welcomeMessage;
}

[Serializable]
public class ReadyUp
{
    public int packetType;
    public string username;
    public string lobbyCode;
}

[Serializable]
public class StartGame
{
    public int packetType;
    public int color;
}

[Serializable]
public class ServerData
{
    public int packetType;
    public int succes;
}

[Serializable]
public class CreateLobby
{
    public int packetType;
    public string lobbyCode;
}

[Serializable]
public class ServerMessage
{
    public int packetType;
    public string message;
}

[Serializable]
public class PlayerMessage
{
    public int packetType;
    public string username;
    public string message;
}