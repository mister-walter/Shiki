/// @author Larisa Motova
using System.Collections.Generic;
using UnityEngine;
using Shiki.EventSystem;
using Shiki.ReaderWriter;

namespace Shiki.Quests {

	/// <summary>
	/// Manager for all events that happen related to quests and tasks. 
	/// Handles communication between the UI files where quest-related data is saved.
	/// </summary>
	public class QuestEventManager {

		/// <summary>
		/// The associated save data manager, which handles saving and writing game data to the filesystem.
		/// </summary>
		SaveDataManager saveDataManager;

		/// <summary>
		/// The task trees that hold the individual tasks that must be completed in each quest
		/// </summary>
		IEnumerable<TaskNode> taskTree;

		public QuestEventManager(SaveDataManager sdm){
			saveDataManager = sdm;
			taskTree = saveDataManager.taskTree;
		}

		/// <summary>
		/// Whenever an Interaction Event is fired, this runs the event through all task trees recursively to see if a task is now completed
		/// </summary>
		/// <param name="evt">The event fired</param>
		public void UpdateTasks(InteractionEvent evt) {
			foreach(TaskNode tn in taskTree){
				if(!tn.AssociatedTask.isComplete){
	                UpdateTaskBranch(evt, tn);
				}
			}
		}

		/// <summary>
		/// For each individual tree, this runs the Interaction Event through all tasks and their children recursively to check whether a task has been completed.
		/// This first checks if a task's child nodes have all been completed, and only then runs the event through the tasks trigger function. 
		/// </summary>
		/// <param name="evt">Evt.</param>
		/// <param name="tn">Tn.</param>
		public bool UpdateTaskBranch(InteractionEvent evt, TaskNode tn){
			bool allChildrenComplete = true;

			foreach(TaskNode tchild in tn.Children) {
				allChildrenComplete &= (tchild.AssociatedTask.isComplete || UpdateTaskBranch(evt, tchild));
				// if child not complete, and updating child doesn't complete the child, allChildrenComplete = false
			}
			if(allChildrenComplete && (tn.AssociatedTask.trigger == null || tn.AssociatedTask.trigger(evt))){
				tn.AssociatedTask.isComplete = true;
				tn.AssociatedTask.onComplete();
				return true;
			}
			return false;
		}

		/// <summary>
		/// Writes the task tree to a file using the save data manager
		/// </summary>
		public void SaveQuestStatus() {
			saveDataManager.taskTree = taskTree;
			saveDataManager.WriteSaves();
		}

		/// <summary>
		/// Reads task tree from file using save data manager
		/// </summary>
		public void ReadSaves(){
			saveDataManager.ReadSaves();
			taskTree = saveDataManager.taskTree;
		}
	}

	/// <summary>
	/// Interaction kind.
	/// Tasks contain trigger and oncomplete functions in a language where most keywords are associated with one of the following enum values.
	/// </summary>
	public enum InteractionKind {
		Get, Enter, Drop, Cut, Hit, Weave, Dig, Grind, Merge, Become, None
	};

	/// <summary>
	/// Interaction event. Gets fired when a player interacts with an object, enters a location, interacts two objects with each other, or any other quest-related interaction occurs.
	/// </summary>
	public class InteractionEvent : IGameEvent {
		/// <summary>
		/// The original object involved in the interaction
		/// </summary>
		public GameObject SourceObject { get; set; }

		/// <summary>
		/// The object receiving interaction/being targeted in the interaction
		/// </summary>
		public GameObject TargetObject { get; set; }

		/// <summary>
		/// Current location of the player when event occurs
		/// </summary>
		/// <value>The location.</value>
		public Vector3 Location { get; set; }

		/// <summary>
		/// Type of interaction between objects/player
		/// </summary>
		public InteractionKind InteractionKind { get; set; }
	}

	/// <summary>
	/// Parsing result. When a task's trigger or oncomplete functions are read in, they are filled into this Parsing Result class
	/// </summary>
	public class ParsingResult {
		/// <summary>
		/// First object found in parsing. This is typically the tool the player is using.
		/// </summary>
		public string Obj1 { get; set; }

		/// <summary>
		/// Second object found in parsing
		/// </summary>
		public string Obj2 { get; set; }

		/// <summary>
		/// Location listed in parsing
		/// </summary>
		public string Location { get; set; }

		/// <summary>
		/// Interaction required from player (enters, hits, etc)
		/// </summary>
		public InteractionKind InteractionKind { get; set; }

		/// <summary>
		/// Describes relationship the 2 objects, if applicable. (With, on, becomes, etc)
		/// </summary>
		public string objToObjInteractionType { get; set; }
	}

	/// <summary>
	/// On complete get event. Gets fired when a task's oncomplete function is called.
	/// Results in the player receiving an item.
	/// </summary>
	public class OnCompleteGetEvent : IGameEvent {
		/// <summary>
		/// Object the player receives
		/// </summary>
		public string ObjectToReceive { get; set; }

		public OnCompleteGetEvent(string otr){
			ObjectToReceive = otr;
		}
	}

	/// <summary>
	/// On complete change event. Gets fired when a task's oncomplete function is called
	/// Results in one item transforming into another.
	/// </summary>
	public class OnCompleteChangeEvent : IGameEvent {

		/// <summary>
		/// Object to be changed
		/// </summary>
		public string OrigObject { get; set; }

		/// <summary>
		/// New object (what the original object turns into)
		/// </summary>
		public string ObjectChangedTo { get; set; }


		public OnCompleteChangeEvent(string oo, string oct){
			OrigObject = oo;
			ObjectChangedTo = oct;
		}
	}

}
