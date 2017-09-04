using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Nett;

namespace TomlReaderWriter {

	public class TomlQuestStateReader : IQuestStateReader {

		public TomlQuestStateReader(){}

		public Dictionary<string, bool> LoadQuestState(Stream fileStream){
			Dictionary<string, bool> taskDictionary = new Dictionary<string, bool>();

			TomlTable tomlTable = Toml.ReadStream(fileStream);
//			string completedTasks = tomlTable.Get<string>("CompletedTask");	// completedTasks contains "PoundRice"

			var cT = tomlTable.TryGetValue("CompletedTask"); 
			if(cT != null){
				string[] completedTasks = cT.Get<string>().Split(' ');
				foreach(string task in completedTasks){
					taskDictionary.Add(task, true);
				}
			} 
			return taskDictionary;

		}
	}



}

