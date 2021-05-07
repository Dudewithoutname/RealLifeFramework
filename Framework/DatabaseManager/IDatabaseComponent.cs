using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace RealLifeFramework
{
    public interface IDatabaseComponent
    {
        MySqlCommand
        void Load();
    }
}
