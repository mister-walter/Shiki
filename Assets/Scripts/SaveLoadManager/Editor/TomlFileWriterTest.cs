using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using NUnit.Framework;
using TomlReaderWriter;

namespace NUnit.TomlQuestReader.TomlQuestReaderTest {

	public class SetUpMyStuffPlz{
		
	}

	[TestFixture]
	public class TomlQuestReaderTest {
		//nunit-csharp-samples nunit on github MoneyTest.cs

		private SaveDataManager sdm;
		private TomlReaderWriter.TomlQuestReader tqr;
		private TomlQuestStateReader tqsr;
		private TomlQuestStateWriter tsw;

		public void SetUp(){
			tqr = new TomlReaderWriter.TomlQuestReader();
			tqsr = new TomlQuestStateReader();
			Debug.Log("breakpoint plz");
			tsw = new TomlQuestStateWriter();
			sdm = new SaveDataManager(tqr, tqsr, tsw);
		}

		[Test]
		public void something(){
			Assert.IsTrue(true);
		}

		[Test]
		public void testReadingToml(){
			SetUp(); // aslkdjfaklsjd why tho
			sdm.ReadSaves();
		}
	}
}

