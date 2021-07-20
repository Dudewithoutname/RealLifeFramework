using System;
using System.Collections.Generic;
using Steamworks;
using MySql.Data.MySqlClient;
using RealLifeFramework.Skills;
using RealLifeFramework.RealPlayers;
using System.Reflection;

namespace RealLifeFramework
{
    public class DatabaseManager
    {
        public string Server { get; set; }
        public string DatabaseName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Port { get; set; }

        public MySqlConnection Connection { get; set; }

        public bool IsConnected()
        {
            if (Connection == null)
            {
                if (String.IsNullOrEmpty(DatabaseName))
                    return false;

                string connstring = string.Format("Server={0}; database={1}; UID={2}; password={3}; port={4}; charset=utf8; pooling=false", Server, DatabaseName, UserName, Password, Port);
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
            if (IsConnected())
            {
                List<MySqlCommand> tables = new List<MySqlCommand>();

                foreach (Type type in Assembly.GetExecutingAssembly().GetTypes()) 
                    if (type.GetCustomAttributes(typeof(Table), true).Length > 0)
                        tables.Add( ((ITable)Activator.CreateInstance(type)).Create() );

                tables.ForEach((table) => table.ExecuteNonQuery());
            }
        }

        #region Default
        public void set(string table, string playerID, string key, string newValue)
        {
            if (IsConnected())
            {
                string query = $" UPDATE {table} SET {key} = '{newValue}' WHERE steamid = {playerID} ";
                var cmd = new MySqlCommand(query, this.Connection);
                cmd.ExecuteNonQuery();
            }
        }

        public string get(string table, int pos, string name, string value)
        {
            string x = null;

            if (IsConnected())
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
