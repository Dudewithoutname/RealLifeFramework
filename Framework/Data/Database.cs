using MySql.Data.MySqlClient;
using RealLifeFramework.SecondThread;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealLifeFramework.Framework.Data
{
    public class Database
    {
        public string Server { get; set; }
        public string DatabaseName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Port { get; set; }

        public MySqlConnection Connection { get; set; }

        public static Database Instance = null;

        public static Database Load()
        {
            if (Instance == null)
                Instance = new Database();

            return Instance;
        }

        public void Connect()
        {
            string connstring = string.Format("Server={0}; database={1}; UID={2}; password={3}; port={4}", Server, DatabaseName, UserName, Password, Port);
            Connection = new MySqlConnection(connstring);
            Connection.Open();
        }

        public bool IsConnect()
        {
            if (Connection == null)
            {
                if (String.IsNullOrEmpty(DatabaseName))
                    return false;

                string connstring = string.Format("Server={0}; database={1}; UID={2}; password={3}; port={4}", Server, DatabaseName, UserName, Password, Port);
                Connection = new MySqlConnection(connstring);
                Connection.Open();
                Logger.Log("[Database Manager] : Connected");
            }

            return true;
        }

        public void Close()
        {
            Logger.Log("[Database Manager] : Connection Closed");
            Connection.Close();
        }

        public void Setup()
        {
            if (IsConnect())
            {
                List<MySqlCommand> tables = new List<MySqlCommand>() 
                {
                    new MySqlCommand($"CREATE TABLE IF NOT EXISTS security ( " +
                        "hwid VARCHAR(40) PRIMARY KEY," +
                        "steamid VARCHAR(17)," +
                        "ban BIT DEFAULT 0" +
                        ")", Connection)
                };

                tables.ForEach((table) => table.ExecuteNonQuery());
            }
        }

        #region Default
        public void set(string table, string param, string paramValue, string key, string newValue)
        {
            SecondaryThread.Execute(() =>
            {
                if (IsConnect())
                {
                    string query = $" UPDATE {table} SET {key} = '{newValue}' WHERE {param} = {paramValue} ";
                    var cmd = new MySqlCommand(query, Connection);
                    cmd.ExecuteNonQuery();
                }
            });
        }

        public string get(string table, int pos, string name, string value)
        {
            string x = null;

            if (IsConnect())
            {

                string query = $" SELECT * FROM {table} WHERE {name} = '{value}' ";
                var cmd = new MySqlCommand(query, this.Connection);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    x = reader[pos].ToString();
                }
                reader.Close();
                return x;
            }
            else
            {
                return null;
            }
        }

        #endregion
    }
}
