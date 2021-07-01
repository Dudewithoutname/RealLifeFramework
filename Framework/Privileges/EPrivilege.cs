using System.ComponentModel;

namespace RealLifeFramework.Privileges
{
    public enum EPrivilege : byte
    {
        PLAYER = 0,
        VETERAN,
        EPIC = 10,
        LEGEND,
        MYTHICAL,
        ZKHELPER = 99,
        HELPER,
        MOD,
        ADMIN,
        OWNER,
    }
}
