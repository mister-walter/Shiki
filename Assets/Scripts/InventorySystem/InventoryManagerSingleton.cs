using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shiki.Inventory {
    public static class InventoryManagerSingleton {
        private static GameObject manager;

        public static GameObject GetInventoryManager()
        {
            return manager;
        }

        public static void SetInventoryManager(GameObject aManager)
        {
            manager = aManager;
        }
    }
}
