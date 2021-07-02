using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealLifeFramework
{
    public static class PrivilegeEx
    {
        public static string ToPermission(this EPrivilege privilege) => $"privilege.{privilege.ToString().ToLower()}";
        public static string ToPermission(this string privilege) => $"privilege.{privilege.ToLower()}";
    }
}
