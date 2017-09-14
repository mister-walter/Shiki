/// @author Larisa Motova
using System.Collections.Generic;
using System.IO;
using Shiki.Quests;
using Nett;

namespace Shiki.ReaderWriter.TomlImplementation {

	/// <summary>
	/// Writes the player's progress through quests to a filestream in the TOML file format
	/// </summary>
	public class TomlQuestStateWriter : IQuestStateWriter {

		public void SaveQuestState(Stream fileStream, IEnumerable<TaskNode> tn) {
			List<Task> tasks = TemporaryTaskConverter.TaskTreesToTaskList(tn);

			string completedList = "";
			foreach(Task t in tasks) {
				if(t.isComplete) {
					completedList += t.name + " ";
				}
			}
			var tS = new TaskState { CompletedTasks = completedList };
			Toml.WriteStream(tS, fileStream);
		}
	}

	/// <summary>
	/// Contains the list of all tasks completed, separated by spaces
	/// </summary>
	public class TaskState{
		public string CompletedTasks { get; set; }
	}
		
}
	