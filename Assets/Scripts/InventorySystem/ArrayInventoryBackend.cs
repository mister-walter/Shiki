/// @author Andrew Walter

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shiki.Inventory.Backend {
    /// <summary>
    /// IInventoryBackend implementation that is backed by an array
    /// </summary>
    public class ArrayInventoryBackend : IInventoryBackend<GameObject> {
        private GameObject[] backingArray;
        private int capacity;

        public ArrayInventoryBackend(uint capacity) {
            this.backingArray = new GameObject[capacity];
            this.capacity = (int)capacity;
        }

        public void Add(GameObject item, int position) {
            this.backingArray[position] = item;
        }

        public int AddToEnd(GameObject item) {
            int pos = this.backingArray.Length - 1;
            this.backingArray[pos] = item;
            return pos;
        }

        public int Capacity() {
            return this.capacity;
        }

        public IEnumerator<GameObject> GetEnumerator() {
            return this.backingArray.GetEnumerator() as IEnumerator<GameObject>;
        }

        public GameObject Get(int position) {
            return this.backingArray[position];
        }

        public int NumItems() {
            return this.backingArray.Length;
        }

        public GameObject Pop(int position) {
            var go = this.backingArray[position];
            this.backingArray[position] = null;
            return go;
        }

        public int Remove(GameObject item) {
            for(int i = 0; i < capacity; i++) {
                if(this.backingArray[i] == item) {
                    this.backingArray[i] = null;
                    return i;
                }
            }
            return -1;
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return this.backingArray.GetEnumerator();
        }

        public void SetItems(List<GameObject> items) {
            if(items.Count > this.Capacity()) {
                throw new IndexOutOfRangeException("Too many items given for inventory backend capacity!");
            }
            for(var i = 0; i < this.Capacity(); i++) {
                if(i < items.Count) {
                    this.backingArray[i] = items[i];
                } else {
                    this.backingArray[i] = null;
                }
            }
        }
    }
}
