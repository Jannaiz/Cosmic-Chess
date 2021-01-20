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
    public int id;
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