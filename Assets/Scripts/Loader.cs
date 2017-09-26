using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shiki {
    public static class Loader {
        public static AudioClip LoadSound(string soundName) {
            var audioClip = Resources.Load<AudioClip>("Sounds/" + soundName);
            return audioClip;
        }

        public static GameObject LoadPrefabInstance(string prefabName) {
            Debug.Log(string.Format("Trying to load prefab {0}", prefabName));
            var obj = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/" + prefabName));
            if(obj.name.EndsWith("(Clone)")) {
                obj.name = obj.name.Substring(0, obj.name.Length - "(Clone)".Length);
            }
            return obj;
        }
    }
}
