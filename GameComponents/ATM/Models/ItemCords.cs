
namespace RealLifeFramework.ATM
{
    public struct ItemCords
    {
        public byte Index;
        public byte Page;

        public ItemCords(byte index, byte page)
        {
            Index = index;
            Page = page;
        }
    }
}
