using System.Collections.Generic;

public class LockQueue<T>
{
    Queue<T> _packetQueue = new Queue<T>();
    object _lock = new object();

    public int Count { get { lock (_lock) { return _packetQueue.Count; } } }

    public void Push(T entity)
    {
        lock (_lock)
        {
            _packetQueue.Enqueue(entity);
        }
    }

    public T Pop()
    {
        lock (_lock)
        {
            //return => bool = false, numeric = 0, othre = null
            if (_packetQueue.Count == 0)
                return default(T);

            return _packetQueue.Dequeue();
        }
    }

    public List<T> PopAll()
    {
        var list = new List<T>();

        lock (_lock)
        {
            while (_packetQueue.Count > 0)
                list.Add(_packetQueue.Dequeue());
        }

        return list;
    }
}