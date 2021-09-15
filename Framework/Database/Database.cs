using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System.Reflection;
using System.Threading.Tasks;

namespace RealLifeFramework.Database
{
    public class DatabaseManager
    {
        public ConnectionString ConString;
        public MySqlConnection Connection;

        public static DatabaseManager Singleton;
        private List<ITable> tables;

        public static void Create()
        {
            if (Singleton == null)
                Singleton = new DatabaseManager();

            Singleton.connect();
        }

        private void connect()
        {
            var connstring = $"Server={ConString.Server}; database={ConString.Database}; UID={ConString.Username}; password={3}; port={4}";
            Connection = new MySqlConnection(connstring);
            Connection.Open();
        }

        public bool IsConnect()
        {
            if (Connection == null)
            {
                var connstring = $"Server={ConString.Server}; database={ConString.Database}; UID={ConString.Username}; password={3}; port={4}";
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

        public void Clear()
        {
            for (int i = 0; i < tables.Count; i++)
            {
                var cmd = new MySqlCommand($"DELETE FROM {tables[i].Name}", this.Connection);
                cmd.ExecuteNonQuery();
            }
        }

        public void Setup()
        {
            if (IsConnect())
            {
                tables = new List<ITable>();

                foreach (Type type in Assembly.GetExecutingAssembly().GetTypes())
                {
                    if (type.IsAssignableFrom(typeof(ITable)))
                    {
                        var table = (ITable)Activator.CreateInstance(type);
                        tables.Add(table);
                        table.StartCommand.ExecuteNonQuery();
                    }
                }
            }
        }

        #region Default
        public void Set(string table, string playerId, string key, string value)
        {
            if (IsConnect())
            {
                string query = $" UPDATE {table} SET {key} = '{value}' WHERE steamid = {playerId} ";
                var cmd = new MySqlCommand(query, this.Connection);
                cmd.ExecuteNonQuery();
            }
        }

        public string Get(string table, string what, string name, string value)
        {
            string x = null;

            if (IsConnect())
            {

                string query = $" SELECT {what} FROM {table} WHERE {name} = '{value}' ";
                var cmd = new MySqlCommand(query, this.Connection);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    x = reader[0].ToString();
                }

                reader.Close();
                return x;
            }
            else
            {
                return null;
            }
        }

        public async Task<string> GetAsync(string table, string what, string name, string value)
        {
            string x = null;

            if (IsConnect())
            {

                string query = $" SELECT {what} FROM {table} WHERE {name} = '{value}' ";
                var cmd = new MySqlCommand(query, this.Connection);
                var reader = await cmd.ExecuteReaderAsync();

                while (reader.Read())
                {
                    x = reader[0].ToString();
                }

                reader.Close();
                return x;
            }
            else
            {
                return null;
            }
        }

        public async Task SetAsync(string table, string playerId, string key, string value)
        {
            if (IsConnect())
            {
                string query = $" UPDATE {table} SET {key} = '{value}' WHERE steamid = {playerId} ";
                var cmd = new MySqlCommand(query, this.Connection);
                await cmd.ExecuteNonQueryAsync();
            }
        }
        #endregion
    }
}
