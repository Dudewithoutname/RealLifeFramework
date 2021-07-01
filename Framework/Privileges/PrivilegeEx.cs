using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealLifeFramework.Privileges
{
    public static class PrivilegeEx
    {
        public static string ToPermission(this EPrivilege privilege) => $"privilege.{privilege}";

    }
}
