using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealLifeFramework.Chatting
{
    public class ChatProfile
    {
        public string NameColor { get; set; }
        public string Avatar { get; set; }

        public ChatProfile(string ncolor, string avatar)
        {
            NameColor = ncolor;
            Avatar = avatar;
        }
    }
}
