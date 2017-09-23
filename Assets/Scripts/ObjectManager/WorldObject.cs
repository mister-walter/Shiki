

using UnityEngine;

namespace Shiki.Inventory {
	public class WorldObject {

		public string name { get; set; }
		public string position { get; set; }
		public string rotation { get; set; }
		public string scale { get; set; }
		public string currentScene { get; set; }
	}

	public static class WorldObjectSerialization {

		public static WorldObject GameObjectToWorldObjectSerialize(GameObject go) {
			WorldObject wo = new WorldObject();
			wo.name = go.name;
			wo.position = Vector3ToString(go.transform.position);
			wo.rotation = Vector3ToString(go.transform.rotation.eulerAngles);
			wo.scale = Vector3ToString(go.transform.lossyScale);
			return wo;
		}

		// WARNING: unfortunate side effect: will probably also load in the game objects too
		public static GameObject WorldObjectSerializeToGameObject(WorldObject wo) {
			GameObject go = Object.Instantiate(Resources.Load(wo.name)) as GameObject;
			go.transform.position = StringToVector3(wo.position);
			go.transform.rotation = Quaternion.Euler(StringToVector3(wo.rotation));
			go.transform.localScale = StringToVector3(wo.scale);
			return go;
		}

		public static string Vector3ToString(Vector3 v){
			string s = "";
			s += v.x + " " + v.y + " " + v.z;
			return s;
		}

		public static Vector3 StringToVector3(string s){
			string[] split = s.Split(' ');
			Vector3 v3 = new Vector3();			// TODO: probably should do some checkings here...
			v3.x = float.Parse(split[0]);
			v3.y = float.Parse(split[1]);
			v3.z = float.Parse(split[2]);
			return v3;
		}

	}
}
