using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;

namespace Shiki.Test {
    [TestFixture]
    public class TomlVariantDatabaseTest {
        private VariantDatabase LoadTestFile() {
            var db = new VariantDatabase();
            TextAsset testFile = Resources.Load<TextAsset>("TestAssets/TomlVariantDatabaseTest");
            db.LoadFromString(testFile.text);
            return db;
        }

        [Test]
        public void CheckItemCount() {
            var db = LoadTestFile();
            Assert.AreEqual(db.ItemCount(), 2);
        }

        [Test]
        public void CheckDefault() {
            var db = LoadTestFile();
            Assert.AreEqual(db.GetPrefabName("Shiroyama", "Winter"), "DefaultPrefab");
        }

        [Test]
        public void CheckDefaultWithNoGiven() {
            var db = LoadTestFile();
            Assert.AreEqual(db.GetPrefabName("Test", "Winter"), "TestPrefab");
        }

        [Test]
        public void CheckGivenSeason() {
            var db = LoadTestFile();
            Assert.AreEqual(db.GetPrefabName("Shiroyama", "Fall"), "FallPrefab");
        }
    }
}