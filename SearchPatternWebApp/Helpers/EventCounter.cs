using Newtonsoft.Json;
using SearchPatternWebApp.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace SearchPatternWebApp.Helpers
{
    public class EventCounter: IEventCounter
    {
        public List<EventCount> EventCounts;

        public EventCounter()
        {
            EventCounts = new List<EventCount>();
        }

        static readonly object _object = new object();

        public void ParseEvents(string deviceID, StreamReader eventLog)
        {
            int count = 0;
            int prevStage = -1;
            int currentStage = -1;
            State currentState = State.StateA;
            DateTime prevTime = new DateTime();

            string[] _values = new string[2];

            // Ignore First line 
            eventLog.ReadLine();

            // Read CSV file one line at a time 
            while (!eventLog.EndOfStream)
            {
                _values = eventLog.ReadLine().Split(',');
                var currentTime = DateTime.Parse(_values[0]);
                currentStage = Int32.Parse(_values[1]);

                // Mark Time Stamp if stage has changed to 3
                if (currentState == State.StateA && currentStage == 3 && prevStage != 3)
                {
                    prevTime = currentTime;
                }

                // Transition to State B if time is greater than 5 and previous State was A
                else if (currentState == State.StateA && currentStage == 2 && prevStage == 3)
                {
                    double timeDiff = (currentTime - prevTime).TotalSeconds;
                    if (timeDiff > 300)
                        currentState = State.StateB;
                }

               // Reset to State A if I am in State B and moved to stage 1
                else if (currentState == State.StateB && currentStage == 1)
                {
                    currentState = State.StateA;
                }

                // Fault found if we were in State B and moved to stage 0
                else if (currentState == State.StateB && currentStage == 0)
                {
                    count++;

                    // Reset State
                    currentState = State.StateA;

                }

                prevStage = currentStage;
            }

            // Add to EventCounts and lock to prevent concurrent threads from writing to EventCounts 
            lock(_object)
            {
                EventCounts.Add(new EventCount { deviceID = deviceID, eventCount = count});
            }            
        }

        public string GetEventCounts()
        {
            return JsonConvert.SerializeObject(EventCounts);
        }

        private enum State
        {
            StateA,
            StateB
        }      
    }
}