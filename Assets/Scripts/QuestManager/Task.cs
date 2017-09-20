/// @author Larisa Motova
using System;
using System.Collections.Generic;
using System.Linq;
using Shiki.EventSystem;

namespace Shiki.Quests {

	/// <summary>
	/// Represents actions player must take in order to complete, or partially complete, a quest
	/// </summary>
	public class Task {

		/// <summary>
		/// Name of the task
		/// </summary>
		public string name { get; set; }

		/// <summary>
		/// The tasks that must be completed before this task can be completed
		/// </summary>
		public string[] subTasks { get; set; }

		/// <summary>
		/// Whether this task has been completed or not
		/// </summary>
		public bool isComplete;

		/// <summary>
		/// The trigger function to check whether this task is complete
		/// </summary>
		public Predicate<InteractionEvent> trigger { get; set; }

		/// <summary>
		/// The on complete function that runs once the task has been completed
		/// </summary>
		public Action onComplete { get; set; }
	}

	/// <summary>
	/// The tree form of the Task class.
	/// Heavily referenced: https://stackoverflow.com/questions/444296/how-to-efficiently-build-a-tree-from-a-flat-structure
	/// </summary>
	public class TaskNode {
		/// <summary>
		/// Children of the Task (if there are any)
		/// </summary>
		public List<TaskNode> Children = new List<TaskNode>();

		/// <summary>
		/// Parent of the Task (if there is one)
		/// </summary>
		public TaskNode Parent { get; set; }

		/// <summary>
		/// The actual Task object associated with this node
		/// </summary>
		public Task AssociatedTask { get; set; }
	}

	/// <summary>
	/// A temporary task class that serves as an intermediary between the save files and the Task class. All fields are strings
	/// </summary>
	public class TemporaryTask {
		/// <summary>
		/// Name of the task
		/// </summary>
		public string Name { get; set; }            // required

		/// <summary>
		/// List of child/subtasks separated by spaces
		/// </summary>
		public string SubTask { get; set; }         // empty

		/// <summary>
		/// The trigger function in string form
		/// </summary>
		public string Trigger { get; set; }         // empty	// could be empty

		/// <summary>
		/// The OnComplete function in string form
		/// </summary>
		public string OnComplete { get; set; }      // empty

		public TemporaryTask(string n, string st, string t, string oc) {
			Name = n;
			SubTask = st;
			Trigger = t;
			OnComplete = oc;
		}
	}

	/// <summary>
	/// A class to convert Temporary Tasks to Tasks and to Task Nodes
	/// </summary>
	public static class TemporaryTaskConverter {

		/// <summary>
		/// Converts a list of temporary tasks to a Task Node tree
		/// </summary>
		/// <returns>A tree of task nodes</returns>
		/// <param name="tempTasks">A list of temporary tasks</param>
		/// <param name="questStates">(Incomplete) Dictionary of task names to its complete status</param>
		public static IEnumerable<TaskNode> TempTaskToTaskTree(List<TemporaryTask> tempTasks, Dictionary<string, bool> questStates) {
			bool exists, isTempComplete;
			List<Task> taskList = new List<Task>();

			foreach(TemporaryTask tt in tempTasks) {    // turns each temporary task into a task
				exists = questStates.TryGetValue(tt.Name, out isTempComplete);
				taskList.Add(TempTaskToTask(tt, exists && isTempComplete));
			}

			IEnumerable<TaskNode> tNodes = TaskListsToTaskTrees(taskList); // converts task list to task tree
			return tNodes;
		}

		/// <summary>
		/// Converts a temporary task to a task
		/// </summary>
		/// <returns>The equivalent task</returns>
		/// <param name="tempTask">Temporary task to be converted</param>
		/// <param name="isComplete">If set to <c>true</c> the task has been completed.</param>
		public static Task TempTaskToTask(TemporaryTask tempTask, bool isComplete) {
			Task task = new Task();

			task.name = tempTask.Name;
			task.isComplete = isComplete;

			task.trigger = null;
			task.onComplete = null;

			task.subTasks = tempTask.SubTask.Split(' ');
			task.trigger = CreateTriggerFunction(tempTask.Trigger);
			task.onComplete = CreateOnCompleteFunction(tempTask.OnComplete);
			return task;
		}

		/// <summary>
		/// Creates the trigger function from a trigger string
		/// </summary>
		/// <returns>Trigger function in the form of a Predicate</returns>
		/// <param name="tr">Trigger function in string form</param>
		public static Predicate<InteractionEvent> CreateTriggerFunction(string tr) {
			ParsingResult pR = ParseString(tr);

			// TODO: implement location
			Predicate<InteractionEvent> pred = (InteractionEvent evt) => {
				// have some sort of dictionary
				if(pR.InteractionKind == evt.InteractionKind &&
				   pR.Obj1.Equals(evt.SourceObject.name) && // source == tool
				   pR.Obj2.Equals(evt.TargetObject.name) //&& 
														 //pR.Location == evt.Location
				  ) {
					return true;
				} else {
					return false;
				}
			};

			return pred;
		}

		/// <summary>
		/// Creates the on complete function from an oncomplete string.
		/// </summary>
		/// <returns>The on complete function in the form of an Action.</returns>
		/// <param name="oc">OnComplete function in string form.</param>
		public static Action CreateOnCompleteFunction(string oc) {
			ParsingResult pR = ParseString(oc);

			Action ac = () => {
				IGameEvent evt;

				if(pR.InteractionKind == InteractionKind.Become) {
					evt = new OnCompleteChangeEvent(pR.Obj1, pR.Obj2);
				} else if(pR.InteractionKind == InteractionKind.Play) {
					evt = new OnCompleteUIEvent(pR.UIEventKind, pR.Obj1);
				} else {
					evt = new OnCompleteGetEvent(pR.Obj1);
				}
				GameEventSystem.FireEvent(evt);
			};
			return ac;
		}

		/// <summary>
		/// Parses function strings according to the function grammar.
		/// At the moment, these include Trigger functions and OnComplete functions.
		/// 
		/// </summary>
		/// <returns>The parsed result.</returns>
		/// <param name="s">Function string to be parsed</param>
		public static ParsingResult ParseString(string s) {
			
			// Better documentation is in a text file
			// EXAMPLES OF LANGUAGE
			// target WITH source		ex: Cut Tree With Axe
			// source ON target			ex: Drop Water On ActiveFire
			// ENTER location			ex: Enter Winter

			// the source object is the tool
			// 		this is also object 1
			// the target object is the interacted object
			//		this is object 2

			// types of actions: Get, Enter, Drop, Cut, Hit, Dig, Merge, Open, None


			// Declares variables:
			string[] toParse = s.Split(' ');
			int length = toParse.Length;
			string obj1 = String.Empty;	// tool or source
			string obj2 = String.Empty;
			int tempQuantity = 1;
			int obj1Quantity = 0;
			int obj2Quantity = 0;
			string location = String.Empty;
			string objToObjIntrcType = String.Empty;
			InteractionKind action = InteractionKind.None;
			OnCompleteUIEventKind uiEventKind = OnCompleteUIEventKind.None;
			ParsingResult parsingResult = new ParsingResult();

			// Goes through string word by word
			for(int i = 0; i < length; i++) {   
				if(toParse[i].Equals("Player") || toParse[i].Equals("Object")) { // ignores these for now
					//
				} else if(toParse[i].Equals("Item") && ++i < length) {
					// next expected word might be a quantity
					if(Int32.TryParse(toParse[i], out tempQuantity)){
						i++;
					}

					// if next expected word is an item, get the item
					if(!String.IsNullOrEmpty(objToObjIntrcType)) {
						// figure out which item is being referred to
						if(objToObjIntrcType.Equals("With")) {
							obj2 = obj1;        // target item
							obj2Quantity = obj1Quantity;
							obj1 = toParse[i];  // source item = this current item
							obj1Quantity = tempQuantity;
						} else if(objToObjIntrcType.Equals("On") || objToObjIntrcType.Equals("And")) {
							obj2 = toParse[i];  // target item = this current item
							obj2Quantity = tempQuantity;
						}
					} else if(action == InteractionKind.Become && ++i < length) {
						// in the case of an OnComplete being parsed:
						obj2 = toParse[i];
						obj2Quantity = tempQuantity;
					} else {
						// if this is the first reference to an item:
						obj1 = toParse[i];
						obj1Quantity = tempQuantity;
					}
				} else if(toParse[i].Equals("With") || toParse[i].Equals("On") || toParse[i].Equals("And")) {
					objToObjIntrcType = toParse[i]; //if objects interact, set the interaction type

				} else if(toParse[i].Equals("Location") && ++i < length){
					location = toParse[i];
				} else if(Enum.TryParse<InteractionKind>(toParse[i], out action)) {
					if(toParse[i].Equals("Becomes")){ // again if an objects interact, set interaction type
						objToObjIntrcType = toParse[i]; 
					} else if(toParse[i].Equals("Play") && ++i + 1 < length){
						Enum.TryParse<OnCompleteUIEventKind>(toParse[i], out uiEventKind);
						obj1 = toParse[++i];
					}
				}
			}

			parsingResult.InteractionKind = action;
			parsingResult.UIEventKind = uiEventKind;
			parsingResult.Obj1 = obj1; // tool
			parsingResult.Obj2 = obj2; // other
			parsingResult.Obj1Quantity = obj1Quantity;
			parsingResult.Obj2Quantity = obj2Quantity;
			parsingResult.Location = location;
			parsingResult.objToObjInteractionType = objToObjIntrcType;

			return parsingResult;
		}

		/// <summary>
		/// Converts a list of Tasks to a set of Task Node trees
		/// Heavily referenced: https://stackoverflow.com/questions/444296/how-to-efficiently-build-a-tree-from-a-flat-structure
		/// </summary>
		/// <returns>The equivalent Task Node tree</returns>
		/// <param name="tasks">List of tasks to be converted.</param>
		public static IEnumerable<TaskNode> TaskListsToTaskTrees(List<Task> tasks) {
			Dictionary<string, TaskNode> lookup = new Dictionary<string, TaskNode>();
			tasks.ForEach(t => lookup.Add(t.name, new TaskNode { AssociatedTask = t }));

			foreach(var item in lookup.Values) {
				TaskNode proposedChild;
				foreach(string st in item.AssociatedTask.subTasks) {
					if(lookup.TryGetValue(st, out proposedChild)) {
						proposedChild.Parent = item;
						item.Children.Add(proposedChild);
					}
				}

			}
			return lookup.Values.Where(x => x.Parent == null);
		}

		/// <summary>
		/// Converts a set of Task Node trees to a list of Tasks recursively
		/// </summary>
		/// <returns>The equivalent list of tasks</returns>
		/// <param name="nodes">The Task Tree nodes to be converted</param>
		public static List<Task> TaskTreesToTaskList(IEnumerable<TaskNode> nodes) {
			List<Task> tasks = new List<Task>();
			foreach(TaskNode tn in nodes) {
				tasks = TaskTreeSingleToTaskList(tn, tasks);
				tasks.Add(tn.AssociatedTask);
			}
			return tasks;
		}

		/// <summary>
		/// Converts a single Task Node Tree to a list of Tasks recursively
		/// </summary>
		/// <returns>A single tree in list form</returns>
		/// <param name="tn">The starting task node</param>
		/// <param name="tasks">The list to build off of</param>
		public static List<Task> TaskTreeSingleToTaskList(TaskNode tn, List<Task> tasks) {
			foreach(TaskNode n in tn.Children) {
				tasks = TaskTreeSingleToTaskList(n, tasks);
			}
			tasks.Add(tn.AssociatedTask);
			return tasks;
		}
	}

}
