using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchPatternWebApp.Helpers
{
    public interface IEventCounter
    {
        void ParseEvents(string deviceID, StreamReader eventLog);
        string GetEventCounts();
    }
}
