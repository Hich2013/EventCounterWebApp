using Newtonsoft.Json;
using NUnit.Framework;
using SearchPatternWebApp.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchPattern.Tests
{
    [TestFixture]
    public class EventCounterTests
    {
        private StreamReader EventLog1;
        private StreamReader EventLog2;
        private StreamReader EventLog3;
        private StreamReader EventLog4;

        [SetUp]
        public void Init()
        {
            string fileName1 = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestData", "testData1.csv");
            string fileName2 = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestData", "testData2.csv");
            string fileName3 = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestData", "testData3.csv");
            string fileName4 = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestData", "testData4.csv");

            EventLog1 = new StreamReader(fileName1);
            EventLog2 = new StreamReader(fileName2);
            EventLog3 = new StreamReader(fileName3);
            EventLog4 = new StreamReader(fileName4);
        }

        [Test]
        public void eventCounter_Returns1_whenFileContainsAsingleFaultWithoutRedundantStages()
        {
            // Arrange 
            var eventCounter = new EventCounter();

            // Act
            eventCounter.ParseEvents("1", EventLog1);

            // Verify / Assert
            var eventCounts = JsonConvert.DeserializeObject<List<EventCount>>(eventCounter.GetEventCounts());
            Assert.AreEqual(eventCounts.First().eventCount, 1);
        }

        [Test]
        public void eventCounter_Returns1_whenFileContainsSingleFaultWithRedundantStages()
        {
            // Arrange 
            var eventCounter = new EventCounter();

            // Act
            eventCounter.ParseEvents("2", EventLog2);

            // Verify / Assert
            var eventCounts = JsonConvert.DeserializeObject<List<EventCount>>(eventCounter.GetEventCounts());
            Assert.AreEqual(eventCounts.First().eventCount, 1);
        }

        [Test]
        public void eventCounter_Returns2_whenFileContainsTwoFaults()
        {
            // Arrange 
            var eventCounter = new EventCounter();

            // Act
            eventCounter.ParseEvents("3", EventLog3);

            // Verify / Assert
            var eventCounts = JsonConvert.DeserializeObject<List<EventCount>>(eventCounter.GetEventCounts());
            Assert.AreEqual(eventCounts.First().eventCount, 2);
        }

        [Test]
        public void eventCounter_Returns0_whenFileDoesNotContainAnyFault()
        {
            // Arrange 
            var eventCounter = new EventCounter();

            // Act
            eventCounter.ParseEvents("4", EventLog4);

            // Verify / Assert
            var eventCounts = JsonConvert.DeserializeObject<List<EventCount>>(eventCounter.GetEventCounts());
            Assert.AreEqual(eventCounts.First().eventCount, 0);
        }
    }
}
