using System;
using UnityEngine;
using NUnit.Framework;

namespace NUnit.SeasonScripts
{
    [TestFixture]
    public class SeasonCoordinateManagerTest
    {
        [Test]
        public void AngleInQuadrant()
        {
            // Quadrant 1
            Assert.AreEqual(0f, SeasonCoordinateManager.AngleInQuadrant(1f, 0f));
            Assert.AreEqual(45f, SeasonCoordinateManager.AngleInQuadrant(1f, 1f));
            Assert.AreEqual(30f, SeasonCoordinateManager.AngleInQuadrant(Mathf.Sqrt(3f), 1f), 0.1f);
            Assert.AreEqual(60f, SeasonCoordinateManager.AngleInQuadrant(1f, Mathf.Sqrt(3f)), 0.1f);
            
            // Quadrant 2
            Assert.AreEqual(0f, SeasonCoordinateManager.AngleInQuadrant(0f, 1f));
            Assert.AreEqual(45f, SeasonCoordinateManager.AngleInQuadrant(-1f, 1f));
            Assert.AreEqual(30f, SeasonCoordinateManager.AngleInQuadrant(-1, Mathf.Sqrt(3f)), 0.1f);
            Assert.AreEqual(60f, SeasonCoordinateManager.AngleInQuadrant(-Mathf.Sqrt(3f), 1f), 0.1f);

            // Quadrant 3
            Assert.AreEqual(0f, SeasonCoordinateManager.AngleInQuadrant(-1f, 0f));
            Assert.AreEqual(45f, SeasonCoordinateManager.AngleInQuadrant(-1f, -1f));
            Assert.AreEqual(30f, SeasonCoordinateManager.AngleInQuadrant(-Mathf.Sqrt(3f), -1f), 0.1f);
            Assert.AreEqual(60f, SeasonCoordinateManager.AngleInQuadrant(-1f, -Mathf.Sqrt(3f)), 0.1f);

            // Quadrant 4
            Assert.AreEqual(0f, SeasonCoordinateManager.AngleInQuadrant(0f, -1f));
            Assert.AreEqual(45f, SeasonCoordinateManager.AngleInQuadrant(1f, -1f));
            Assert.AreEqual(30f, SeasonCoordinateManager.AngleInQuadrant(1f, -Mathf.Sqrt(3f)), 0.1f);
            Assert.AreEqual(60f, SeasonCoordinateManager.AngleInQuadrant(Mathf.Sqrt(3f), -1f), 0.1f);
        }

        [Test]
        public void SeasonToGlobalCoordinateAngleTest()
        {
            var startAngle = 0f;

            SeasonCoordinate sc;
            Vector3 gc;

            sc = new SeasonCoordinate(1, 0f, 0);
            gc = SeasonCoordinateManager.SeasonToGlobalCoordinate(startAngle, sc);
            Assert.AreEqual(1, gc.x, 0.1);
            Assert.AreEqual(0, gc.z, 0.1);

            sc = new SeasonCoordinate(2, 45f, 0);
            gc = SeasonCoordinateManager.SeasonToGlobalCoordinate(startAngle, sc);
            Assert.AreEqual(Mathf.Sqrt(2f), gc.x, 0.1);
            Assert.AreEqual(Mathf.Sqrt(2f), gc.z, 0.1);

            sc = new SeasonCoordinate(2, 30f, 0);
            gc = SeasonCoordinateManager.SeasonToGlobalCoordinate(startAngle, sc);
            Assert.AreEqual(Mathf.Sqrt(3f), gc.x, 0.01);
            Assert.AreEqual(1f, gc.z, 0.01);

            sc = new SeasonCoordinate(2, 60f, 0);
            gc = SeasonCoordinateManager.SeasonToGlobalCoordinate(startAngle, sc);
            Assert.AreEqual(1f, gc.x, 0.01);
            Assert.AreEqual(Mathf.Sqrt(3f), gc.z, 0.01);
        }
    }
}
