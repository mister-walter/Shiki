using System.Collections.Generic;
using UnityEngine;

namespace Shiki.Inventory{
	public class InventoryObject {

		public string name { get; set; }

	}

	public static class InventorySerialization {

		public static List<InventoryObject> GameObjectToInventoryObjectList(List<GameObject> gos){
			List<InventoryObject> ios = new List<InventoryObject>();
			foreach(GameObject go in gos){
				ios.Add(GameObjectToInventoryObjectSerialize(go));
			}
			return ios;
		}

		public static List<GameObject> InventoryObjectToGameObjectList(List<InventoryObject> ios){
			List<GameObject> gos = new List<GameObject>();
			foreach(InventoryObject io in ios){
				gos.Add(InventoryObjectSerializeToGameObject(io));
			}
			return gos;
		}

		private static InventoryObject GameObjectToInventoryObjectSerialize(GameObject go){
			InventoryObject io = new InventoryObject();
			io.name = go.name;
			return io;
		}

		// WARNING: Unfortunate side effect: will also probably load in the game objects
		private static GameObject InventoryObjectSerializeToGameObject(InventoryObject io){
			GameObject go = Object.Instantiate(Resources.Load(io.name)) as GameObject;
			return go;
		}
	}
}
