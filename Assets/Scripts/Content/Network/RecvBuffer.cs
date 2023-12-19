using System;

//세선에 같이 만들어도 되지만 가독성이 떨어지고 깔끔하지 않기때문에 따로 한다고 한다.
public class RecvBuffer
{
    ArraySegment<byte> _buffer;

    int _readPos;
    int _writePos;

    public int DataSize { get { return _writePos - _readPos; } }
    public int FreeSize { get { return _buffer.Count - _writePos; } }

    public RecvBuffer(int bufsize)
    {
        _buffer = new ArraySegment<byte>(new byte[bufsize],0,bufsize);
    }

    //지금은 어차피 _buffer.Offset이 0이라 안넣어도 되지만 다르게 할라면 꼭 넣어야함.
    public ArraySegment<byte> DataSegment
    {
        get { return new ArraySegment<byte>(_buffer.Array, _buffer.Offset + _readPos, DataSize); }
    }
    public ArraySegment<byte> RecvSegment
    {
        get { return new ArraySegment<byte>(_buffer.Array, _buffer.Offset + _writePos, FreeSize); }
    }

    public void Clear()
    {
        int datasize = DataSize;
        if(datasize == 0)
        {
            _readPos = _writePos = 0;   //남은데이터가 없으면 두 포인트를 0으로 위치시켜줌
        }
        else
        {
            Array.Copy(_buffer.Array, _buffer.Offset + _readPos, _buffer.Array, _buffer.Offset, datasize);
            _readPos = _buffer.Offset;
            _writePos = datasize;
        }
    }
    public bool OnRead(int numOfBytes)
    {
        if (numOfBytes > DataSize)   //현재 데이터 수보다 더 많이 읽었으면 오류
            return false;

        _readPos += numOfBytes;
        return true;
    }
    public bool OnWrite(int numOfBytes)
    {
        if (numOfBytes > FreeSize)   //현재 남은 빈공간보다 더 많이 읽었으면
            return false;

        _writePos += numOfBytes;
        return true;
    }
}