using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Nett;

namespace TomlReaderWriter {
		
	public class TomlQuestReader : IQuestReader {

		public TomlQuestReader(){}

		public List<Task> LoadQuestTasks(Stream fileStream, Dictionary<string, bool> questStates){
			List<TemporaryTask> tempTaskList = ReadInTasks(fileStream);
			return TempTaskToTaskList(tempTaskList, questStates);
		}

		public List<TemporaryTask> ReadInTasks(Stream fileStream){
			List<TemporaryTask> tempTaskList = new List<TemporaryTask>();
			string name, subTasks, trigger, onComplete;		// for the sake of making temporary tasks
			TomlObject st, t, oc;
			TomlTable task;
			bool isComplete, exists;

			TomlTable tomlTable = Toml.ReadStream(fileStream);
			TomlTableArray tasksList = tomlTable.Get<TomlTableArray>("TemporaryTask");	// list of temporary tasks
//			var tt = tasksList[0];	// get individual tasks
			for(int i = 0; i < tasksList.Count; i++){ // toml doesn't do foreach apparently
				task = tasksList[i];
				name = task.Get<string>("Name");

				st = task.TryGetValue("SubTask");
				if(st != null){
					subTasks = st.Get<string>();			// TODO: make sure this thing works... unity hates me
				} else { subTasks = string.Empty; }

				t = task.TryGetValue("Trigger");
				if(t != null){
					trigger = t.Get<string>(); 
				} else { trigger = string.Empty; }

				oc = task.TryGetValue("OnComplete");
				if(oc != null){
					onComplete = oc.Get<string>(); 
				} else { onComplete = string.Empty; }
					
				tempTaskList.Add(new TemporaryTask(name, subTasks, trigger, onComplete));
			}
			return tempTaskList;
		}
			
		public List<Task> TempTaskToTaskList(List<TemporaryTask> tempTasks, Dictionary<string, bool> questStates){
			bool exists, isComplete;
			List<Task> taskList = new List<Task>();

			foreach(TemporaryTask tt in tempTasks){
				exists = questStates.TryGetValue(tt.Name, out isComplete);
				taskList.Add(TempTaskToTask(tt, exists && isComplete));
			}
			return taskList;
		}	
	

		public Task TempTaskToTask(TemporaryTask tempTask, bool isComplete){
			Task task = new Task();

			task.name = tempTask.Name;
			task.isComplete = isComplete;


//			TODO: task.subTasks		// can just do task.subTasks.Add(subTask);
//			TODO: task.trigger = 	// predicate
//			TODO: task.onComplete = // action
		}
	}


	// this is the information that was in the 
	public class TemporaryTask{
		public string Name { get; set; }			// required
		public string SubTask { get; set; }			// empty
		public string Trigger { get; set; }			// empty	// could be empty
		public string OnComplete { get; set; }		// empty	// TODO: discuss how you end quests

		public TemporaryTask(string n, string st, string t, string oc){
			Name = n;
			SubTask = st;
			Trigger = t;
			OnComplete = oc;
		}
	}
}