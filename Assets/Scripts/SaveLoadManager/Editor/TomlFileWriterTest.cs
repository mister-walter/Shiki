/// @author Larisa Motova
using System.Collections.Generic;
using NUnit.Framework;
using Shiki.ReaderWriter.TomlImplementation;
using Shiki.ReaderWriter;
using Shiki.Quests;

namespace NUnit.TomlQuestReaderTest {

	[TestFixture]
	public class TomlQuestReaderTest {
		//nunit-csharp-samples nunit on github MoneyTest.cs

		private SaveDataManager sdm;
		private TomlQuestReader tqr;
		private TomlQuestStateReader tqsr;
		private TomlQuestStateWriter tsw;

		public void SetUp(){
			tqr = new TomlQuestReader();
			tqsr = new TomlQuestStateReader();
			tsw = new TomlQuestStateWriter();
			sdm = new SaveDataManager(tqr, tqsr, tsw);
		}

		[Test]
		public void testReadingToml(){
			SetUp(); 
			sdm.ReadSaves();
		}

		[Test]
		public void testWritingToml(){
			SetUp();
			List<Task> tasks = new List<Task>();
			Task t;
			for(int i = 0; i < 5; i++){
				t = new Task();
				t.name = "test" + i;
				t.isComplete = (i % 2) == 0;
				tasks.Add(t);
			}

			sdm.taskTree = TemporaryTaskConverter.TaskListsToTaskTrees(tasks);
			sdm.WriteSaves();
			// "Assets/StateFiles/quest_status.tml"
		}
	}
}

