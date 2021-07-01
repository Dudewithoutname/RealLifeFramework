using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealLifeFramework.Chatting
{
    public class ProfileData
    {
        public string NameColor { get; set; }
        public string Avatar { get; set; }

        public static explicit operator ProfileData(ChatProfile profile)
        {
            return new ProfileData()
            {
                Avatar = profile.Avatar,
                NameColor = profile.NameColor
            };
        }
    }
}
