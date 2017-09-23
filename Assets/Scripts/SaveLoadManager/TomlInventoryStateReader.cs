using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Shiki.Inventory;
using Nett;

namespace Shiki.ReaderWriter.TomlImplementation {
	public class TomlInventoryStateReader : IInventoryStateReader {

		public List<GameObject> LoadInventoryState(Stream fileStream){
			List<InventoryObject> ios = new List<InventoryObject>();
			TomlTable table = Toml.ReadStream(fileStream);
			TomlTable invObj;
			InventoryObject io;
			string name;
			// any other fields that we may want to add will go here

			TomlTableArray objectsList = table.Get<TomlTableArray>("InventoryObject");  // list of temporary tasks
			for(int i = 0; i < objectsList.Count; i++){
				io = new InventoryObject();
				invObj = objectsList[i];
				name = invObj.Get<string>("Name");

				io.name = name;
				ios.Add(io);
			}

			return InventorySerialization.InventoryObjectToGameObjectList(ios);
		}

	}
}
