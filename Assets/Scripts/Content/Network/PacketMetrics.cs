using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Unity.VisualScripting;

#nullable enable

class TimeoutSignal
{
    public enum SignalType
    {
        CANCELED = 0,
        TIMEOUT = 1,
    }

    public PacketHandler.PacketType pakcetType;
    public SignalType signalType;
    public long sequence;
    //TODO
}

class IDGenerator
{
    long _id;

    public IDGenerator()
    {
        _id = 0;
    }

    public long Generate()
    {
        return Interlocked.Increment(ref _id);
    }
}


class PacketTimeoutSignalPort
{
    
    public class PacketPortHandler
    {
        Action? _callback1;
        Action<TimeoutSignal>? _callback2;
        public PacketPortHandler(Action? callback)
        {
            _callback1 = callback;
        }
        public PacketPortHandler(Action<TimeoutSignal>? callback)
        {
            _callback2 = callback;
        }

        public PacketPortHandler(Action? callback1, Action<TimeoutSignal>? callback2)
        {
            _callback1 = callback1;
            _callback2 = callback2;
        }

        public void Callback(TimeoutSignal signal)
        {
            _callback1?.Invoke();

#if UNITY_EDITOR
            if (_callback2 != null)
            {

                if (signal.signalType == TimeoutSignal.SignalType.TIMEOUT)
                    Utils.Log($"timeout!, seq: {signal.sequence}, signal type: {signal.signalType.ToString()}, packet type: {signal.pakcetType.ToString()}");

                _callback2.Invoke(signal);
            }
#else
             _callback2.Invoke(signal);
#endif

        }

        public static PacketPortHandler operator +(PacketPortHandler left, PacketPortHandler right)
        {
            left._callback1 += right._callback1;
            left._callback2 += right._callback2;

            return left;
        }
    }
    object _lock = new object();
    Dictionary<PacketHandler.PacketType, PacketPortHandler> _ports = new Dictionary<PacketHandler.PacketType, PacketPortHandler>();

    public void Docking(TimeoutSignal info)
    {
        lock (_lock)
        {
            if (!_ports.ContainsKey(info.pakcetType))
                return;


            // 여기부턴 삭제되어도 무조건 실행
            var handler = _ports[info.pakcetType];
            GameManager.ThreadPool.EnqueueJob(() =>
            {
                handler.Callback(info);
            });
        }
    }


    public void AddUnityHandler(PacketHandler.PacketType type, Action? callback)
    {
        if (callback == null)
            return;

        Action unity_callback = () =>
        {
            GameManager.ThreadPool.UniAsyncJob(callback);
        };

        lock (_lock)
        {
            if(_ports.TryGetValue(type, out var handler))
                handler += new PacketPortHandler(unity_callback);
            else
                _ports.Add(type, new PacketPortHandler(unity_callback));
        }
    }

    public void AddUnityHandler(PacketHandler.PacketType type, Action<TimeoutSignal>? callback)
    {
        if (callback == null)
            return;

        Action<TimeoutSignal> unity_callback = (signal) =>
        {
            GameManager.ThreadPool.UniAsyncJob(() => { callback(signal); });
        };

        lock (_lock)
        {
            if (_ports.TryGetValue(type, out var handler))
                handler += new PacketPortHandler(unity_callback);
            else
                _ports.Add(type, new PacketPortHandler(unity_callback));
        }
    }

    public void AddHandler(PacketHandler.PacketType type, Action? callback)
    {
        if (callback == null)
            return;

        lock (_lock)
        {
            if (_ports.TryGetValue(type, out var handler))
                handler += new PacketPortHandler(callback);
            else
                _ports.Add(type, new PacketPortHandler(callback));
        }
    }

    public void AddHandler(PacketHandler.PacketType type, Action<TimeoutSignal>? callback)
    {
        if (callback == null)
            return;

        lock (_lock)
        {
            if (_ports.TryGetValue(type, out var handler))
                handler += new PacketPortHandler(callback);
            else
                _ports.Add(type, new PacketPortHandler(callback));
        }
    }

    public void DeleteHandler(PacketHandler.PacketType type)
    {
        lock(_lock)
        {
            if (_ports.ContainsKey(type))
                _ports.Remove(type);
        }
    }
}

class PacketTimer
{
    static readonly int TIMEOUT = 5000; //ms
    PacketTimeoutSignalPort? _port;
    PacketHandler.PacketType _type;
    long _sequence;
    int _canceled;

    public PacketTimer(PacketHandler.PacketType type, long seq, PacketTimeoutSignalPort? port = null)
    {
        _type = type;
        _sequence = seq;
        _port = port;
        _canceled = 0;

        // 작업 등록
        GameManager.ThreadPool.EnqueueJob(() => { Watch(); });
    }

    public async void Watch(Action? callback = null)
    {
        await Task.Delay(TIMEOUT);

        if (Interlocked.Increment(ref _canceled) > 1)
            return;

        if(callback != null)
        {
            callback();
            return;
        }

        if(_port != null)
        {
            _port?.Docking(new TimeoutSignal() { pakcetType = _type, sequence = _sequence, signalType = TimeoutSignal.SignalType.TIMEOUT });
        }
    }

    public void Cancel()
    {
        if(Interlocked.Increment(ref _canceled) == 1)
        {
            _port?.Docking(new TimeoutSignal() { pakcetType = _type, sequence = _sequence, signalType = TimeoutSignal.SignalType.CANCELED });
        }
    }
}

class PacketMetrics
{
    public class Entry_Action
    {
        public enum EntryType
        {
            Unity = 0,
            Common = 1,
        }

        public EntryType ET { get; private set; }
        public Action Callback { get; private set; }
        public Entry_Action(EntryType type, Action callback)
        {
            ET = type;
            Callback = callback;
        }
    }

    public PacketTimeoutSignalPort Port { get; private set; } = new PacketTimeoutSignalPort();

    Dictionary<PacketHandler.PacketType, PacketHandler.PacketType>  _pair = new Dictionary<PacketHandler.PacketType, PacketHandler.PacketType>();
    Dictionary<PacketHandler.PacketType, Entry_Action>              _entry_queue = new Dictionary<PacketHandler.PacketType, Entry_Action>();
    Dictionary<PacketHandler.PacketType, Queue<PacketTimer>>        _metry_queue = new Dictionary<PacketHandler.PacketType, Queue<PacketTimer>>();
    Dictionary<PacketHandler.PacketType, IDGenerator>               _idgenerator = new Dictionary<PacketHandler.PacketType, IDGenerator>();

    object _lock_metry = new object();
    
    //시작시 반드시 모두 초기화
    //object _lock = new object();
    public void Init()
    {
        /*==========================================
        * 여기에 초기화 해야하는 모든 pair들을 명시합니다.
        ============================================*/
        _pair = new Dictionary<PacketHandler.PacketType, PacketHandler.PacketType>() {
            { PacketHandler.PacketType.PKT_C_SIGNUP, PacketHandler.PacketType.PKT_S_SIGNUP },
            { PacketHandler.PacketType.PKT_C_LOGIN, PacketHandler.PacketType.PKT_S_LOGIN },
            { PacketHandler.PacketType.PKT_C_VERIFYING, PacketHandler.PacketType.PKT_S_VERIFYING },
            { PacketHandler.PacketType.PKT_C_NICKNAME, PacketHandler.PacketType.PKT_S_NICKNAME },
            { PacketHandler.PacketType.PKT_C_NEW_CHARACTER, PacketHandler.PacketType.PKT_S_NEW_CHARACTER },
            { PacketHandler.PacketType.PKT_C_DELETE_CHARACTER, PacketHandler.PacketType.PKT_S_DELETE_CHARACTER },
        };


        foreach(var pair in _pair)
        {
            _metry_queue.Add(pair.Value, new Queue<PacketTimer>());
            _idgenerator.Add(pair.Value, new IDGenerator());
        }
    }

    //thread unsafe
    public void AddPacketPair(PacketHandler.PacketType send, PacketHandler.PacketType recv)
    {
        _pair.Add(send, recv);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void AddUnityWatcher(Action? enter = null, Action? callback = null)
    {
        if (enter == null && callback == null)
            return;

        foreach (var pair in _pair)
        {
            if(enter != null)
                _entry_queue.Add(pair.Key, new Entry_Action(Entry_Action.EntryType.Unity, enter));
            if(callback != null)
                Port.AddUnityHandler(pair.Value, callback);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void AddUnityWatcher(Action? enter = null, Action<TimeoutSignal>? callback = null)
    {
        if (enter == null && callback == null)
            return;

        foreach (var pair in _pair)
        {
            if (enter != null)
                _entry_queue.Add(pair.Key, new Entry_Action(Entry_Action.EntryType.Unity, enter));
            if (callback != null)
                Port.AddUnityHandler(pair.Value, callback);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void AddWatcher(Action? enter = null, Action? callback = null)
    {
        if (enter == null && callback == null)
            return;

        foreach (var pair in _pair)
        {
            if (enter != null)
                _entry_queue.Add(pair.Key, new Entry_Action(Entry_Action.EntryType.Common, enter));
            if (callback != null)
                Port.AddUnityHandler(pair.Value, callback);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void AddWatcher(Action? enter = null, Action<TimeoutSignal>? callback = null)
    {
        if (enter == null && callback == null)
            return;

        foreach (var pair in _pair)
        {
            if (enter != null)
                _entry_queue.Add(pair.Key, new Entry_Action(Entry_Action.EntryType.Common, enter));
            if (callback != null)
                Port.AddUnityHandler(pair.Value, callback);
        }
    }

    // thread unsafe
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    void _Metry(PacketHandler.PacketType type)
    {
        if (!_pair.TryGetValue(type, out var type_pair))
            return;

        lock(_lock_metry)
        {
            if (!_metry_queue.TryGetValue(type_pair, out var mqueue))
                return;

            mqueue.Enqueue(new PacketTimer(type_pair, _idgenerator[type_pair].Generate(), Port));
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Metry(ArraySegment<byte> buffer, Action? OnMetry = null)
    {
        var proto_id = BitConverter.ToUInt16(buffer.Array, buffer.Offset + 2);

        if (!Enum.IsDefined(typeof(PacketHandler.PacketType), proto_id))
            return;

        if (_entry_queue.TryGetValue((PacketHandler.PacketType)proto_id, out var entry))
        {
            switch (entry.ET)
            {
                case Entry_Action.EntryType.Unity:
                    GameManager.ThreadPool.UniAsyncJob(() =>
                    {
                        //unity main thread에서 호출하고
                        entry.Callback?.Invoke();

                        //예약
                        GameManager.ThreadPool.EnqueueJob(() =>
                        {
                            _Metry((PacketHandler.PacketType)proto_id);
                            OnMetry?.Invoke();
                        });
                    });
                    break;
                case Entry_Action.EntryType.Common:
                    //전부 예약
                    GameManager.ThreadPool.EnqueueJob(() =>
                    {
                        entry.Callback?.Invoke();
                        _Metry((PacketHandler.PacketType)proto_id);
                        OnMetry?.Invoke();
                    });
                    break;
                default:
                    break;
            }

            return;
        }

        GameManager.ThreadPool.EnqueueJob(() =>
        {
            OnMetry?.Invoke();
        });
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Stop(ArraySegment<byte> buffer)
    {
        var proto_id = BitConverter.ToUInt16(buffer.Array, buffer.Offset + 2);

        if (!Enum.IsDefined(typeof(PacketHandler.PacketType), proto_id))
            return;

        Stop((PacketHandler.PacketType)proto_id);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Stop(PacketHandler.PacketType type)
    {
        lock(_lock_metry)
        {
            try
            {
                if (_metry_queue.TryGetValue(type, out var mqueue))
                {
                    var timer = mqueue.Dequeue();
                    timer.Cancel();
                }
            }
            catch(Exception ex)
            {
                Utils.Log($"Packet Recived after timeout! packet : {type.ToString()}");
                Utils.Log($"OnRecvCompleteRecursive {ex.Message}" + "\n" + ex.StackTrace);
            }
        }
    }
}