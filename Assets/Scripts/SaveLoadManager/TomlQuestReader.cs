/// @author Larisa Motova
using System.Collections.Generic;
using System.IO;
using Nett;
using Shiki.Quests;

namespace Shiki.ReaderWriter.TomlImplementation {

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
            List<TemporaryTask> tempTaskList = new List<TemporaryTask>();
            string name, subTasks, trigger, onComplete;     // for the sake of making temporary tasks
            TomlObject st, t, oc; // subtasks, trigger, on complete
            TomlTable task;

            TomlTable tomlTable = Toml.ReadStream(fileStream);
            TomlTableArray tasksList = tomlTable.Get<TomlTableArray>("TemporaryTask");  // list of temporary tasks
                                                                                        //			var tt = tasksList[0];	// get individual tasks (for reference)

            for(int i = 0; i < tasksList.Count; i++) {
                task = tasksList[i];
                name = task.Get<string>("Name");

                st = task.TryGetValue("SubTask");
                if(st != null) {
                    subTasks = st.Get<string>();
                } else { subTasks = string.Empty; }

                t = task.TryGetValue("Trigger");
                if(t != null) {
                    trigger = t.Get<string>();
                } else { trigger = string.Empty; }

                oc = task.TryGetValue("OnComplete");
                if(oc != null) {
                    onComplete = oc.Get<string>();
                } else { onComplete = string.Empty; }

                tempTaskList.Add(new TemporaryTask(name, subTasks, trigger, onComplete));
            }
            return tempTaskList;
        }
    }
}