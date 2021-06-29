using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealLifeFramework.API.Models
{
    public class Character
    {
        public string steamId { get; set; }
        public string name { get; set; }
        public int age { get; set; }
        public string gender { get; set; }
        public string label { get; set; }
    }
}
