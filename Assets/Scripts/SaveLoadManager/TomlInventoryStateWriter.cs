using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Shiki.Inventory;
using Nett;

namespace Shiki.ReaderWriter.TomlImplementation {
	public class TomlInventoryStateWriter : IInventoryStateWriter {
	
		// InventoryManagerSingleton.GetInventoryManager.inventoryObjs
		public void SaveInventoryState(Stream fileStream, List<GameObject> gameObjs){

			List<InventoryObject> writeableInventory = InventorySerialization.GameObjectToInventoryObjectList(gameObjs);
			foreach(InventoryObject io in writeableInventory){
				Toml.WriteStream(io, fileStream);
			}
		}
	
	}
}
