using System;

[Serializable]
public class HandshakeHeader
{
    public int packetType;
    public string username;
}

[Serializable] 
public class Header
{
    public int packetType;
    public string username;
    public string sessionId;
}

[Serializable]
public class Location
{
    public Header header;
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
public class HeaderChecker
{
    public Header header;
}

//[Serializable]
//public class PacketChecker
//{
//    public int packetType;
//}

[Serializable]
public class GetPublicLobbys
{
    public Header header;
}

[Serializable]
public class LobbyAnswer
{
    public Header header;
    public string[] lobbyCodes;
}

[Serializable]
public class LobbyRequest
{
    public Header header;
    public int isPublic;
    public int map;
    public int playerAmount;
    public int dimensionAmount;
}

[Serializable]
public class JoinRequest
{
    public Header header;
    public string lobbyCode;
}

[Serializable]
public class ClientHandshake
{
    public HandshakeHeader header;
}

[Serializable]
public class ServerHandshake
{
    public Header header;
    public string welcomeMessage;
}

[Serializable]
public class ReadyUp
{
    public Header header;
    public string lobbyCode;
}

[Serializable]
public class StartGame
{
    public Header header;
    public int color;
}

[Serializable]
public class ServerData
{
    public Header header;
    public int succes;
}

[Serializable]
public class CreateLobby
{
    public Header header;
    public string lobbyCode;
}

[Serializable]
public class ServerMessage
{
    public Header header;
    public string message;
}

[Serializable]
public class PlayerMessage
{
    public Header header;
    public string message;
}

[Serializable] 
public class StayAlive
{
    public Header header;
}