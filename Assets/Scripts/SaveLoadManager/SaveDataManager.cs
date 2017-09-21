/// @author Larisa Motova
using System.Collections.Generic;
using System.IO;
using Shiki.Quests;


namespace Shiki.ReaderWriter {
    /// <summary>
    /// The manager for reading and writing from save data and other state-related files
    /// </summary>
    public class SaveDataManager {

        /// <summary>
        /// The file path to the list of quests
        /// </summary>
        private string questFilePath = "Assets/StateFiles/quest_list.tml";

        /// <summary>
        /// The file path to the player's quest state
        /// </summary>
        private string questStatusFilePath = "Assets/StateFiles/quest_status.tml";
        //private string questStatusFilePathTest = "Assets/StateFiles/quest_status_tes2t2222.tml";


        /// <summary>
        /// The reader for quest information
        /// </summary>
        private IQuestReader QuestReader;

        /// <summary>
        /// The reader for the player's state in quests
        /// </summary>
        private IQuestStateReader QuestStateReader;

        /// <summary>
        /// The writer for the player's state in quests
        /// </summary>
        private IQuestStateWriter QuestStateWriter;

        /// <summary>
        /// The tree of tasks for the quests
        /// </summary>
        public IEnumerable<TaskNode> taskTree { get; set; }

        public SaveDataManager(IQuestReader qr, IQuestStateReader qsr, IQuestStateWriter qsw) {
            QuestReader = qr;
            QuestStateReader = qsr;
            QuestStateWriter = qsw;
        }

        /// <summary>
        /// This reads and saves into the SaveDataManager the saved state of the player
        /// At the moment this includes: the status of the quest tasks
        /// </summary>
        public void ReadSaves() {
            // quest related variables
            Stream questStatusFileStream;
            Stream questFileStream;
            Dictionary<string, bool> currentState;

            // find the quest status file
            if(File.Exists(questStatusFilePath)) {
                questStatusFileStream = File.OpenRead(questStatusFilePath); // TODO: multiple save files somehow
                currentState = QuestStateReader.LoadQuestState(questStatusFileStream);
                questStatusFileStream.Close();
            } else {
                currentState = null;
            }

            // find the quest lists and tasks file
            if(File.Exists(questFilePath)) {
                questFileStream = File.OpenRead(questFilePath);
                taskTree = QuestReader.LoadQuestTasks(questFileStream, currentState);
                questFileStream.Close();
            } else {
                System.Console.WriteLine("File " + questFilePath + " was not found. Cannot run game properly.");
                return;
            }

            // TODO load some other stuff eventually
            // ex: items in hand, items set in the world (original item's location), I don't think player position matters
        }

        /// <summary>
        /// Writes the player's current status into the appropriate save files
        /// </summary>
        public void WriteSaves() {
            // TODO: when there's more information, maybe some sort of GetInfoFunction
            // at the moment I'm just resetting the taskTree from the outside

            Stream questStatusFileStream;
            if(!File.Exists(questStatusFilePath)) {
                questStatusFileStream = File.Create(questStatusFilePath);
            } else {
                questStatusFileStream = File.OpenWrite(questStatusFilePath);
            }
            QuestStateWriter.SaveQuestState(questStatusFileStream, taskTree);
            questStatusFileStream.Close();
        }
    }

    /// <summary>
    /// Reads quest information (tasks and subtasks) from a file stream
    /// </summary>
    public interface IQuestReader {

        /// <summary>
        /// Reads and loads in quest information. Is called after reading in quest states
        /// </summary>
        /// <returns>The tasks in tree form</returns>
        /// <param name="fileStream">File stream to read from.</param>
        /// <param name="questStates">Dictionary of task names to their current completion state</param>
        IEnumerable<TaskNode> LoadQuestTasks(Stream fileStream, Dictionary<string, bool> questStates);
    }

    /// <summary>
    /// Reads the player's state of progress through quests from a filestream
    /// </summary>
    public interface IQuestStateReader {

        /// <summary>
        /// Reads and loads the player's current progress through the quests
        /// Is called before reading and loading the quest information itself
        /// </summary>
        /// <returns>An incomplete dictionary of task names to the player's completion state through</returns>
        /// <param name="fileStream">File stream to read from.</param>
        Dictionary<string, bool> LoadQuestState(Stream fileStream);
    }

    /// <summary>
    /// Writes the player's state of progress through quests into a filestream
    /// </summary>
    public interface IQuestStateWriter {

        /// <summary>
        /// Saves the player's current progress through the quests.
        /// </summary>
        /// <param name="fileStream">File stream to write into.</param>
        /// <param name="tn">Task Node tree to save.</param>
        void SaveQuestState(Stream fileStream, IEnumerable<TaskNode> tn);

    }
}
