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

        public static explicit operator ProfileData(ChatProfile profile)
        {
            return new ProfileData()
            {
                NameColor = profile.NameColor
            };
        }
    }
}
