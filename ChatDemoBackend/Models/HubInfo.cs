using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatDemoBackend.Models
{
    public class HubInfo
    {
        public Dictionary<string, string> list { get; set; }
        public int numberOfConnected { get; set; }
    }
}
