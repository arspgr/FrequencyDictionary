using System;
using FrequencyDictionary.Settings;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class ApplicationSettingsTest
    {
        [Test]
        public void OutputFileNameIsNull_DefaultPathReturned()
        {
            var settings = new ApplicationSettings("test", null);
            Assert.AreEqual($"{AppDomain.CurrentDomain.BaseDirectory}/result.txt", settings.OutputFileName);
        }
    }
}