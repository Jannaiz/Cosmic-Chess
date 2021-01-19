using System;

[Serializable]
public class Location
{
    public int packetType;
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