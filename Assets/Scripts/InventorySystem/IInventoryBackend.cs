using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shiki.Inventory
{
    public interface IInventoryBackend<T> : IEnumerable<T>
    {
        int AddToEnd(T item);
        void Add(T item, int position);
        T Get(int position);
        T Pop(int position);
        int Remove(T item);
        int Capacity();
        int NumItems();
    }
}