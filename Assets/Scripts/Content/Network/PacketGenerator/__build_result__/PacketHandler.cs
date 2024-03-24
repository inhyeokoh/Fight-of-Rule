using Google.Protobuf;
using System;
using System.Collections.Generic;
using static PacketHandler;
class PacketHandler
{
    //singleton
    public static PacketHandler Instance { get; private set; } = new PacketHandler();

    PacketHandler()
    {
        Init();
    }
    public enum PacketType : ushort
    {
        PKT_C_SIGNUP = 1,
        PKT_S_SIGNUP = 2,
        PKT_C_LOGIN = 3,
        PKT_S_LOGIN = 4,
        PKT_C_OPTION = 5,
        PKT_S_OPTION = 6,
        PKT_S_ASK_VERF = 7,
        PKT_C_VERIFYING = 8,
        PKT_S_VERIFYING = 9,
        PKT_C_NICKNAME = 10,
        PKT_S_NICKNAME = 11,
        PKT_C_CHARACTERS = 12,
        PKT_S_CHARACTERS = 13,
        PKT_C_NEW_CHARACTER = 14,
        PKT_S_NEW_CHARACTER = 15,
    };
    Dictionary<ushort, Func<Session, ArraySegment<byte>, bool>> _handler = new Dictionary<ushort, Func<Session, ArraySegment<byte>, bool>>();
    public void Init()
    {
        _handler.Add((ushort)PacketType.PKT_S_SIGNUP, (session, buffer) => PacketHandlerImpl.Handle_S_SIGNUP(session, _HandlePacket<S_SIGNUP>(buffer)));
        _handler.Add((ushort)PacketType.PKT_S_LOGIN, (session, buffer) => PacketHandlerImpl.Handle_S_LOGIN(session, _HandlePacket<S_LOGIN>(buffer)));
        _handler.Add((ushort)PacketType.PKT_S_OPTION, (session, buffer) => PacketHandlerImpl.Handle_S_OPTION(session, _HandlePacket<S_OPTION>(buffer)));
        _handler.Add((ushort)PacketType.PKT_S_ASK_VERF, (session, buffer) => PacketHandlerImpl.Handle_S_ASK_VERF(session, _HandlePacket<S_ASK_VERF>(buffer)));
        _handler.Add((ushort)PacketType.PKT_S_VERIFYING, (session, buffer) => PacketHandlerImpl.Handle_S_VERIFYING(session, _HandlePacket<S_VERIFYING>(buffer)));
        _handler.Add((ushort)PacketType.PKT_S_NICKNAME, (session, buffer) => PacketHandlerImpl.Handle_S_NICKNAME(session, _HandlePacket<S_NICKNAME>(buffer)));
        _handler.Add((ushort)PacketType.PKT_S_CHARACTERS, (session, buffer) => PacketHandlerImpl.Handle_S_CHARACTERS(session, _HandlePacket<S_CHARACTERS>(buffer)));
        _handler.Add((ushort)PacketType.PKT_S_NEW_CHARACTER, (session, buffer) => PacketHandlerImpl.Handle_S_NEW_CHARACTER(session, _HandlePacket<S_NEW_CHARACTER>(buffer)));
    }
    public ArraySegment<byte> SerializePacket(C_SIGNUP pkt) { return _serializePacket(pkt, PacketType.PKT_C_SIGNUP); }
    public ArraySegment<byte> SerializePacket(C_LOGIN pkt) { return _serializePacket(pkt, PacketType.PKT_C_LOGIN); }
    public ArraySegment<byte> SerializePacket(C_OPTION pkt) { return _serializePacket(pkt, PacketType.PKT_C_OPTION); }
    public ArraySegment<byte> SerializePacket(C_VERIFYING pkt) { return _serializePacket(pkt, PacketType.PKT_C_VERIFYING); }
    public ArraySegment<byte> SerializePacket(C_NICKNAME pkt) { return _serializePacket(pkt, PacketType.PKT_C_NICKNAME); }
    public ArraySegment<byte> SerializePacket(C_CHARACTERS pkt) { return _serializePacket(pkt, PacketType.PKT_C_CHARACTERS); }
    public ArraySegment<byte> SerializePacket(C_NEW_CHARACTER pkt) { return _serializePacket(pkt, PacketType.PKT_C_NEW_CHARACTER); }
    static ArraySegment<byte> _serializePacket<T>(T pkt, PacketType protocol) where T : IMessage
    {
        //TODO send buffer & obj pool
        byte[] data = pkt.ToByteArray();
        ushort size = (ushort)(4 + data.Length);
        ushort id = (ushort)protocol;
        byte[] ser_packet = new byte[size];
        int count = 0;
        Array.Copy(BitConverter.GetBytes(size), 0, ser_packet, count, sizeof(ushort));
        count += sizeof(ushort);
        Array.Copy(BitConverter.GetBytes(id), 0, ser_packet, count, sizeof(ushort));
        count += sizeof(ushort);
        Array.Copy(data, 0, ser_packet, count, data.Length);
        return new ArraySegment<byte>(ser_packet);
    }

    public bool HandlePacket(Session session, ArraySegment<byte> buffer)
    {
        ushort count = 0;
        ushort size = BitConverter.ToUInt16(buffer.Array, buffer.Offset);
        count += 2;
        ushort id = BitConverter.ToUInt16(buffer.Array, buffer.Offset + count);
        count += 2;
        Func<Session, ArraySegment<byte>, bool> action = null;
        if (_handler.TryGetValue(id, out action))
            return action.Invoke(session, buffer);

        return false;
    }

    T _HandlePacket<T>(ArraySegment<byte> buffer) where T : IMessage, new()
    {
        T pkt = new T();
        pkt.MergeFrom(buffer.Array, buffer.Offset + 4, buffer.Count - 4);

        return pkt;
    }
}