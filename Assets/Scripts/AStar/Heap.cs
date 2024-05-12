using System;

public class Heap<T> where T : IHeapItem<T>
{
    private int currentIndex;
    private T[] items;

    public Heap(int gridMax)
    {
        currentIndex = 0;
        items = new T[gridMax];
    }

    public void Add(T item)
    {
        item.HeapIndex = currentIndex;
        items[currentIndex] = item;

        SortUp(item);

        currentIndex++;
    }

    public T RemoveFirst()
    {
        T removeFirst = items[0];
        currentIndex--;

        items[0] = items[currentIndex];
        items[0].HeapIndex = 0;

        SortDown(items[0]);

        return removeFirst;
    }

    public void UpdateItem(T item)
    {
        SortUp(item);
    }

    private void SortUp(T item)
    {
        int parantIndex = (item.HeapIndex - 1) / 2;

        while (true)
        {
            T parantNode = items[parantIndex];

            if (item.CompareTo(parantNode) > 0)
            {
                Swap(item, parantNode);
            }
            else
            {
                break;
            }

            parantIndex = (item.HeapIndex - 1) / 2;

        }
    }

    private void SortDown(T item)
    {
        while (true)
        {
            int leftChildIndex = (item.HeapIndex * 2) + 1;
            int rightChildIndex = (item.HeapIndex * 2) + 2;
            int swapIndex = 0;

            if (leftChildIndex < currentIndex)
            {
                swapIndex = leftChildIndex;

                if (rightChildIndex < currentIndex)
                {
                    if (items[leftChildIndex].CompareTo(items[rightChildIndex]) < 0)
                    {
                        swapIndex = rightChildIndex;
                    }
                }

                if (item.CompareTo(items[swapIndex]) < 0)
                {
                    Swap(item, items[swapIndex]);
                }
                else
                {
                    return;
                }
            }
            else
            {
                return;
            }

        }
    }

    public int Count
    {
        get { return currentIndex; }
    }

    public bool Contains(T item)
    {
        return Equals(items[item.HeapIndex], item);
    }

    public void Swap(T itemA, T itemB)
    {
        items[itemB.HeapIndex] = itemA;
        items[itemA.HeapIndex] = itemB;

        int tempIndex = itemA.HeapIndex;
        itemA.HeapIndex = itemB.HeapIndex;
        itemB.HeapIndex = tempIndex;
    }


}


public interface IHeapItem<T> : IComparable<T>
{
    public int HeapIndex
    {
        get;
        set;
    }
}