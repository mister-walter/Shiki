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
		private string[] triggerStringTests;
		private string[] onCompleteStringTests;


		public void SetUp(){
			tqr = new TomlQuestReader();
			tqsr = new TomlQuestStateReader();
			tsw = new TomlQuestStateWriter();
			sdm = new SaveDataManager(tqr, tqsr, tsw);

			triggerStringTests = new string[2];
			triggerStringTests[0] = "Player Hit Item SteamedRice With Item Hammer";
			triggerStringTests[1] = "Player Drop Item RiceInBowl On Item ActiveFire";

			onCompleteStringTests = new string[1];
			onCompleteStringTests[0] = "Player Get Item WhiteMochi";
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

		[Test]
		public void testParser(){
			SetUp();
			for(int i = 0; i < triggerStringTests.Length; i++){
				TemporaryTaskConverter.ParseString(triggerStringTests[i]);
			}
			for(int i = 0; i < onCompleteStringTests.Length; i++){
				TemporaryTaskConverter.ParseString(onCompleteStringTests[i]);
			}
		}

		[Test]
		public void testTriggerCreation(){
			SetUp();
			for(int i = 0; i<triggerStringTests.Length; i++){
				TemporaryTaskConverter.CreateTriggerFunction(triggerStringTests[i]);
			}
		}

		[Test]
		public void testOnCompleteCreation(){
			SetUp();
			for(int i = 0; i<onCompleteStringTests.Length; i++){
				TemporaryTaskConverter.CreateOnCompleteFunction(onCompleteStringTests[i]);
			}
		}
	}
}

