using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Shiki.ReaderWriter;

namespace Shiki.Inventory {


	public class ObjectManager {

		SaveDataManager saveDataManager;

		public ObjectManager(SaveDataManager sdm){
			saveDataManager = sdm;
		}

		// this shall read the saves
		// maybe should make 2; one for inventory one for over-world
		public void ReadInventorySaves() {

		}

		public void SaveInventorySaves() {
			
		}

		public void ReadOverWorldSaves() {

		}

		public void SaveOverWorldSaves() {
			
		}
	}

}
