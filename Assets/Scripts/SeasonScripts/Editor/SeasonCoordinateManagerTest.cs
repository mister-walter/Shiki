using System.Collections;
using System.Collections.Generic;
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
            Assert.AreEqual(45f, SeasonCoordinateManager.AngleInQuadrant(0f, 0f));
            Assert.AreEqual(0f, SeasonCoordinateManager.AngleInQuadrant(-1f, -1f));
            Assert.AreEqual(30f, SeasonCoordinateManager.AngleInQuadrant(0.966f, -0.259f), 0.1f);
            Assert.AreEqual(60f, SeasonCoordinateManager.AngleInQuadrant(0.966f, 0.259f), 0.1f);
            
            // Quadrant 2
            Assert.AreEqual(0f, SeasonCoordinateManager.AngleInQuadrant(1f, 1f));
            Assert.AreEqual(45f, SeasonCoordinateManager.AngleInQuadrant(0f, 1f));
            Assert.AreEqual(30f, SeasonCoordinateManager.AngleInQuadrant(0.259f, 0.966f), 0.1f);
            Assert.AreEqual(60f, SeasonCoordinateManager.AngleInQuadrant(-0.259f, 0.966f), 0.1f);

            // Quadrant 3
            Assert.AreEqual(0f, SeasonCoordinateManager.AngleInQuadrant(-1f, 1f));
            Assert.AreEqual(45f, SeasonCoordinateManager.AngleInQuadrant(-1f, 0f));
            Assert.AreEqual(30f, SeasonCoordinateManager.AngleInQuadrant(-0.966f, 0.259f), 0.1f);
            Assert.AreEqual(60f, SeasonCoordinateManager.AngleInQuadrant(-0.966f, -0.259f), 0.1f);

            // Quadrant 4
            Assert.AreEqual(0f, SeasonCoordinateManager.AngleInQuadrant(-1f, -1f));
            Assert.AreEqual(45f, SeasonCoordinateManager.AngleInQuadrant(0f, -1f));
            Assert.AreEqual(30f, SeasonCoordinateManager.AngleInQuadrant(-0.259f, -0.966f), 0.1f);
            Assert.AreEqual(60f, SeasonCoordinateManager.AngleInQuadrant(0.259f, -0.966f), 0.1f);
        }

        [Test]
        public void SeasonToGlobalCoordinateAngleTest()
        {
            var startAngle = -45;

            SeasonCoordinate sc;
            Vector3 gc;

            sc = new SeasonCoordinate(1, 45f, 0);
            gc = SeasonCoordinateManager.SeasonToGlobalCoordinate(startAngle, sc);
            Assert.AreEqual(1, gc.x, 0.1);
            Assert.AreEqual(0, gc.z, 0.1);

            sc = new SeasonCoordinate(Mathf.Sqrt(2), 0f, 0);
            gc = SeasonCoordinateManager.SeasonToGlobalCoordinate(startAngle, sc);
            Assert.AreEqual(1, gc.x, 0.1);
            Assert.AreEqual(-1, gc.z, 0.1);

            sc = new SeasonCoordinate(1, 30f, 0);
            gc = SeasonCoordinateManager.SeasonToGlobalCoordinate(startAngle, sc);
            Assert.AreEqual(0.966, gc.x, 0.01);
            Assert.AreEqual(-0.259, gc.z, 0.01);

            sc = new SeasonCoordinate(1, 60f, 0);
            gc = SeasonCoordinateManager.SeasonToGlobalCoordinate(startAngle, sc);
            Assert.AreEqual(0.966, gc.x, 0.01);
            Assert.AreEqual(0.259, gc.z, 0.01);
        }
    }
}
