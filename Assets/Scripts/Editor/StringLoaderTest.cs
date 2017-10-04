/// @author Larisa Motova
using System.Collections.Generic;
using NUnit.Framework;
using System.IO;
using System.Text;
using System;

namespace Shiki.Test {
    [TestFixture]
    public class StringLoaderTest {
        [Test]
        public void TestReadingStrings() {
            var testStream = new MemoryStream(Encoding.ASCII.GetBytes("FooString = \"AString\""));
            var loader = StringLoader.Instance();
            loader.LoadStrings(testStream);
            Assert.AreEqual(loader.GetString("FooString"), "AString");
        }

        [Test]
        public void TestGetStringException() {
            var loader = StringLoader.Instance();
            Action action = () => loader.GetString("foo");
            Assert.AreEqual(action, Throws.TypeOf<Exception>());
        }
    }
}

