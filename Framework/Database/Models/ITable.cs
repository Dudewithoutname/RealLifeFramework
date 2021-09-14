using MySql.Data.MySqlClient;

namespace RealLifeFramework.Database
{
    public interface ITable
    {
        string Name { get; }
        MySqlCommand StartCommand { get; }
    }
}
