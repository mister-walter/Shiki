/// @author Larisa Motova
using System.Collections.Generic;
using System.IO;
using Nett;

namespace Shiki.ReaderWriter.TomlImplementation {

    /// <summary>
    /// Reads in the player's state through quests in the TOML file format
    /// </summary>
    public class TomlQuestStateReader : IQuestStateReader {

        /// <summary>
        /// Loads in player's state through quests in TOML file format
        /// </summary>
        /// <returns>(Incomplete) dictionary of task names to completion status</returns>
        /// <param name="fileStream">File stream to read from.</param>
        public Dictionary<string, bool> LoadQuestState(Stream fileStream) {
            Dictionary<string, bool> taskDictionary = new Dictionary<string, bool>();

            TomlTable tomlTable = Toml.ReadStream(fileStream);
            //			string completedTasks = tomlTable.Get<string>("CompletedTask");	// completedTasks contains "PoundRice"

            var cT = tomlTable.TryGetValue("CompletedTask");
            if(cT != null) {
                string[] completedTasks = cT.Get<string>().Split(' ');
                foreach(string task in completedTasks) {
                    taskDictionary.Add(task, true);
                }
            }
            return taskDictionary;

        }
    }

}

