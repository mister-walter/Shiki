using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class QuestEventManager {


	public delegate void QuestEventHandler();
	public static event QuestEventHandler QuestEventTEMPNAME;




	public void UpdateTasks(/* some info about task is put in here */){
//		foreach(var t in tasks){
////			t.update();
//		}
	}

	public void SaveQuestStatus(){
		// check if QuestSave.TOML exists
		// if not create it
		// then take the tasks array (that is made in this QuestEventManager file) and write it to the .TOML file
	}



	// and then here would be my test for when do I check if this event needs to be called


	// ok so the way this system would work is it would have to constantly check for a state


	//event handler: On_____()

	// each quest
//
//	public void CallEvent(){
//		QuestEventTEMPNAME();
//	}
}

public class Task{

	// TODO: change this up later

	public string name { get; set; }
	public List<Task> subTasks { get; set; }
	public bool isComplete;

	// TODO: these 2 turn into functions
	public string trigger { get; set; }
	public string onComplete { get; set; }

}
