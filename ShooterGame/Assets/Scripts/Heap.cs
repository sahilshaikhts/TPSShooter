using System;

public class Heap<T> where T:IHeapItem<T>
{
    T[] m_items;
    int m_currentItemCount;

    public Heap(int aSize)
    {
        m_items = new T[aSize];
    }
    
    public void Update(T aItem)
    {
        SortUp(aItem);
    }

    public bool Contains(T aItem) { return Equals(m_items[aItem.index], aItem); }
    public int Count { get { return m_items.Length; } }

    public void Add(T aItem)
    { 
        aItem.index=m_currentItemCount;
        m_items [m_currentItemCount]= aItem;
        SortUp(aItem);
        m_currentItemCount++;
    }
    public T RemoveFirst()
    {
        T firstItem = m_items[0];

        m_currentItemCount--;
        m_items[0] = m_items[m_currentItemCount];
        m_items[0].index=0;
        SortDown(m_items[0]);
        return firstItem;
    }

    private void SortDown(T aItem)
    {
        while (true)
        {
            int leftChildIndex = (aItem.index * 2) + 1;
            int rightChildIndex = (aItem.index * 2) + 2;
            int swapIndex = 0;

            if (leftChildIndex < m_currentItemCount)
            {
                swapIndex = leftChildIndex;

                if (rightChildIndex < m_currentItemCount)
                {
                    if (m_items[leftChildIndex].CompareTo(m_items[rightChildIndex]) < 0)
                    {
                        swapIndex = rightChildIndex;
                    }
                }
                if (aItem.CompareTo(m_items[swapIndex]) < 0)
                {
                    Swap(aItem, m_items[swapIndex]);
                }
                else
                    break;
            }
            else
                break;
        }
    }

    void SortUp(T aItem)
    {
        int parentIndex = (aItem.index - 1) / 2;
        while (true)
        {
            if (aItem.CompareTo(m_items[parentIndex]) > 0)
            {
                Swap(aItem, m_items[parentIndex]);
            }
            else
                break;
            parentIndex = (aItem.index - 1) / 2;
        }
    }

    void Swap(T aItemA,T aItemB)
    {
        m_items[aItemA.index]=aItemB;
        m_items[aItemB.index]=aItemA;

        int Index_a = aItemA.index;
        aItemA.index=aItemB.index;
        aItemB.index= Index_a;
    }
}

public interface IHeapItem<T>: IComparable<T>
{
   int index { get; set; }
}
