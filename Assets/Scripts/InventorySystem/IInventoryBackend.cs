/// @author Andrew Walter
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shiki.Inventory
{
    /// <summary>
    /// Interface for a backend for the inventory
    /// </summary>
    /// <typeparam name="T">The type that the inventory can store</typeparam>
    public interface IInventoryBackend<T> : IEnumerable<T>
    {
        int AddToEnd(T item);
        void Add(T item, int position);
        T Get(int position);
        T Pop(int position);
        int Remove(T item);
        int Capacity();
        int NumItems();
        void SetItems(List<GameObject> items);
    }
}