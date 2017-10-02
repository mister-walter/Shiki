using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;
using Shiki.Constants;

namespace Shiki.Test {
    [TestFixture]
    public class ConstantsTest {
        [Test]
        public void CheckDistanceSimple() {
            var distance = SeasonName.Distance(SeasonName.Winter, SeasonName.Spring);
            Assert.AreEqual(distance, 1);
        }

        [Test]
        public void CheckDistanceWrapAround() {
            var distance = SeasonName.Distance(SeasonName.Fall, SeasonName.Winter);
            Assert.AreEqual(distance, 1);
        }
    }
}