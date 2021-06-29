using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealLifeFramework.API.Models
{
    public class Stats
    {
        public bool online { get; set; }
        public int players { get; set; }
        public int ems { get; set; }
        public int pd { get; set; }
        public string serverIP { get; set; }
        public string serverPort { get; set; }
    }
}
