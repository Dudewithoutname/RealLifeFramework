using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealLifeFramework.API.Models
{
    public class Ban
    {
        public string steamId { get; set; }
        public string characterName { get; set; }
        public string provider { get; set; }
        public string reason { get; set; }
        public string time { get; set; }
    }
}
