/// @author Larisa Motova
using System.Collections.Generic;
using System.IO;
using Nett;
using Shiki.Quests;

namespace Shiki.ReaderWriter.TomlImplementation {
    internal class QuestFileRoot {
        public List<TemporaryTask> task { get; set; }
    }
    /// <summary>
    /// Reads quest information from a filestream where information is written in the TOML file format.
    /// </summary>
    public class TomlQuestReader : IQuestReader {

        public IEnumerable<TaskNode> LoadQuestTasks(Stream fileStream, Dictionary<string, bool> questStates) {
            List<TemporaryTask> tempTaskList = ReadInTasks(fileStream);
            return TemporaryTaskConverter.TempTaskToTaskTree(tempTaskList, questStates);
        }

        /// <summary>
        /// Reads in quest tasks from a filestream where tasks are written in the TOML file format
        /// </summary>
        /// <returns>Tasks read in from file in the form of a list of Temporary Tasks</returns>
        /// <param name="fileStream">File stream to be read from.</param>
        private List<TemporaryTask> ReadInTasks(Stream fileStream) {
            QuestFileRoot root = Toml.ReadStream<QuestFileRoot>(fileStream);
            foreach(var task in root.task) {
                if(!task.IsValid()) {
                    throw new System.Exception("Found invalid task when reading file. Ensure that each task has a nonempty name.");
                }
            }
            return root.task;
        }
    }
}