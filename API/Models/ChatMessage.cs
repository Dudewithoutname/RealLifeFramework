using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealLifeFramework.API.Models
{
    public class ChatMessage
    {
        public string steamId { get; set; }
        public string color { get; set; }
        public string name { get; set; }
        public string avatar { get; set; }
        public string message { get; set; }
    }
}
