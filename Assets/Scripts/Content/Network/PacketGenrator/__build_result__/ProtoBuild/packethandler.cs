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
        PKT_C_REQUEST_SETTINGS_OPTIONS = 5,
        PKT_S_REQUEST_SETTINGS_OPTIONS = 6,
        PKT_C_SAVE_VOL_OPTIONS = 7,
        PKT_S_SAVE_VOL_OPTIONS = 8,
        PKT_S_ASK_VERF = 9,
        PKT_C_VERIFYING = 10,
        PKT_S_VERIFYING = 11,
        PKT_C_NICKNAME = 12,
        PKT_S_NICKNAME = 13,
        PKT_C_CHARACTERS = 14,
        PKT_S_CHARACTERS = 15,
        PKT_C_NEW_CHARACTER = 16,
        PKT_S_NEW_CHARACTER = 17,
        PKT_C_DELETE_CHARACTER = 18,
        PKT_S_DELETE_CHARACTER = 19,
        PKT_C_INGAME = 20,
        PKT_S_INGAME = 21,
        PKT_C_ITEMINFO = 22,
        PKT_S_ITEMINFO = 23,
    };
    Dictionary<ushort, Func<Session, ArraySegment<byte>, bool>> _handler = new Dictionary<ushort, Func<Session, ArraySegment<byte>, bool>>();
    public void Init()
    {
        _handler.Add((ushort)PacketType.PKT_S_SIGNUP, (session, buffer) => PacketHandlerImpl.Handle_S_SIGNUP(session, _HandlePacket<S_SIGNUP>(buffer)));
        _handler.Add((ushort)PacketType.PKT_S_LOGIN, (session, buffer) => PacketHandlerImpl.Handle_S_LOGIN(session, _HandlePacket<S_LOGIN>(buffer)));
        _handler.Add((ushort)PacketType.PKT_S_REQUEST_SETTINGS_OPTIONS, (session, buffer) => PacketHandlerImpl.Handle_S_REQUEST_SETTINGS_OPTIONS(session, _HandlePacket<S_REQUEST_SETTINGS_OPTIONS>(buffer)));
        _handler.Add((ushort)PacketType.PKT_S_SAVE_VOL_OPTIONS, (session, buffer) => PacketHandlerImpl.Handle_S_SAVE_VOL_OPTIONS(session, _HandlePacket<S_SAVE_VOL_OPTIONS>(buffer)));
        _handler.Add((ushort)PacketType.PKT_S_ASK_VERF, (session, buffer) => PacketHandlerImpl.Handle_S_ASK_VERF(session, _HandlePacket<S_ASK_VERF>(buffer)));
        _handler.Add((ushort)PacketType.PKT_S_VERIFYING, (session, buffer) => PacketHandlerImpl.Handle_S_VERIFYING(session, _HandlePacket<S_VERIFYING>(buffer)));
        _handler.Add((ushort)PacketType.PKT_S_NICKNAME, (session, buffer) => PacketHandlerImpl.Handle_S_NICKNAME(session, _HandlePacket<S_NICKNAME>(buffer)));
        _handler.Add((ushort)PacketType.PKT_S_CHARACTERS, (session, buffer) => PacketHandlerImpl.Handle_S_CHARACTERS(session, _HandlePacket<S_CHARACTERS>(buffer)));
        _handler.Add((ushort)PacketType.PKT_S_NEW_CHARACTER, (session, buffer) => PacketHandlerImpl.Handle_S_NEW_CHARACTER(session, _HandlePacket<S_NEW_CHARACTER>(buffer)));
        _handler.Add((ushort)PacketType.PKT_S_DELETE_CHARACTER, (session, buffer) => PacketHandlerImpl.Handle_S_DELETE_CHARACTER(session, _HandlePacket<S_DELETE_CHARACTER>(buffer)));
        _handler.Add((ushort)PacketType.PKT_S_INGAME, (session, buffer) => PacketHandlerImpl.Handle_S_INGAME(session, _HandlePacket<S_INGAME>(buffer)));
        _handler.Add((ushort)PacketType.PKT_S_ITEMINFO, (session, buffer) => PacketHandlerImpl.Handle_S_ITEMINFO(session, _HandlePacket<S_ITEMINFO>(buffer)));
    }
    public ArraySegment<byte> SerializePacket(C_SIGNUP pkt) { return _serializePacket(pkt, PacketType.PKT_C_SIGNUP); }
    public ArraySegment<byte> SerializePacket(C_LOGIN pkt) { return _serializePacket(pkt, PacketType.PKT_C_LOGIN); }
    public ArraySegment<byte> SerializePacket(C_REQUEST_SETTINGS_OPTIONS pkt) { return _serializePacket(pkt, PacketType.PKT_C_REQUEST_SETTINGS_OPTIONS); }
    public ArraySegment<byte> SerializePacket(C_SAVE_VOL_OPTIONS pkt) { return _serializePacket(pkt, PacketType.PKT_C_SAVE_VOL_OPTIONS); }
    public ArraySegment<byte> SerializePacket(C_VERIFYING pkt) { return _serializePacket(pkt, PacketType.PKT_C_VERIFYING); }
    public ArraySegment<byte> SerializePacket(C_NICKNAME pkt) { return _serializePacket(pkt, PacketType.PKT_C_NICKNAME); }
    public ArraySegment<byte> SerializePacket(C_CHARACTERS pkt) { return _serializePacket(pkt, PacketType.PKT_C_CHARACTERS); }
    public ArraySegment<byte> SerializePacket(C_NEW_CHARACTER pkt) { return _serializePacket(pkt, PacketType.PKT_C_NEW_CHARACTER); }
    public ArraySegment<byte> SerializePacket(C_DELETE_CHARACTER pkt) { return _serializePacket(pkt, PacketType.PKT_C_DELETE_CHARACTER); }
    public ArraySegment<byte> SerializePacket(C_INGAME pkt) { return _serializePacket(pkt, PacketType.PKT_C_INGAME); }
    public ArraySegment<byte> SerializePacket(C_ITEMINFO pkt) { return _serializePacket(pkt, PacketType.PKT_C_ITEMINFO); }
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