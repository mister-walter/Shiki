using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


public class SaveDataManager {

	private IQuestReader QuestReader;
	private IQuestStateReader QuestStateReader;
	private IQuestStateWriter QuestStateWriter;

//	private List<Task> tasks;

	private string questStatusFilePath = "Assets/StateFiles/quest_status.tml";
	private string questFilePath = "Assets/StateFiles/quest_list.tml";


	public SaveDataManager(IQuestReader qr, IQuestStateReader qsr, IQuestStateWriter qsw){
		QuestReader = qr;
		QuestStateReader = qsr;
		QuestStateWriter = qsw;
	}

	// Reads from save files
	public void ReadSaves() {
		// quest related variables
		Stream questStatusFileStream;
		Stream questFileStream;
		Dictionary<string, bool> currentState;

		// find the quest status file
		if(File.Exists(questStatusFilePath)){
			questStatusFileStream = File.OpenRead(questStatusFilePath); // TODO: multiple save files somehow
			currentState = QuestStateReader.LoadQuestState(questStatusFileStream);
			questStatusFileStream.Close();
		} else {
			currentState = null;			
		}
			
		// find the quest lists and tasks file
		if(File.Exists(questFilePath)){
			questFileStream = File.OpenRead(questFilePath);
			/*tasks = */
//			Debug.Log("Survived up to here");
			QuestReader.LoadQuestTasks(questFileStream, currentState);
			questFileStream.Close();
		} else {
			System.Console.WriteLine("File " + questFilePath + " was not found. Cannot run game properly.");
			return;
		}

		// load some other stuff here probably
		// TODO: items in hand, items set in the world (original item's location), I don't think player position matters

	}

	public void WriteSaves(List<Task> tasks){
		// quest related varibales
		Stream questStatusFileStream;
		if(!File.Exists(questStatusFilePath)){
			File.Create(questStatusFilePath);
		}
		questStatusFileStream = File.OpenWrite(questStatusFilePath);
		QuestStateWriter.SaveQuestState(questStatusFileStream, tasks);
	}




}
	
public interface IQuestReader {
	// so this comes 2nd
	// it uses the dictionary received from VVVV so we can make normal tasks like, immediately
	List<Task> LoadQuestTasks(Stream fileStream, Dictionary<string, bool> questStates);
}

public interface IQuestStateReader {
	// this comes first
	// it reads the saved state file
	// if it doesn't exist (then that problem is solved earlier on)
	Dictionary<string, bool> LoadQuestState(Stream fileStream);
}

public interface IQuestStateWriter {
	// this happens when you save
	void SaveQuestState(Stream fileStream, List<Task> tasks);
}