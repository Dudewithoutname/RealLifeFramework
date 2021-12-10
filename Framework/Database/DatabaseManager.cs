using System;
using System.Collections.Generic;
using System.Linq;
using MySql.Data.MySqlClient;
using System.Reflection;
using System.Threading.Tasks;
using SDG.Unturned;
using HarmonyLib;

namespace RealLifeFramework.Database
{
    public class DatabaseManager
    {
        public static ConnectionString ConString => RealLife.Instance.Configuration.Instance.ConString;
        public MySqlConnection Connection;

        public static DatabaseManager Singleton;
        private List<ITable> tables;

        public static void Create()
        {
            if (Singleton == null)
            {
                Singleton = new DatabaseManager();
                
                Singleton.IsConnect();
                Singleton.Setup();
            }
        }

        public bool IsConnect()
        {
            if (Connection == null)
            {
                var connstring = $"server={ConString.Server}; database={ConString.Database}; UID={ConString.Username}; password={ConString.Password}; port={ConString.Port}";
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
            foreach (var cmd in tables.Select(t => new MySqlCommand($"DELETE FROM {t.Name}", this.Connection)))
            {
                cmd.ExecuteNonQuery();
            }
        }

        public void Setup()
        {
            if (IsConnect())
            {
                tables = new List<ITable>();

                var types = Assembly.GetExecutingAssembly().GetTypes();
                
                foreach (var type in types)
                {
                    if (type.GetCustomAttributes(typeof(DatabaseTable), true).Length > 0)
                    {
                        var table = (ITable) Activator.CreateInstance(type);
                        tables.Add(table);

                        table.StartCommand().ExecuteNonQuery();
                    }
                }
            }
        }

        #region Default
        public void Set(string table, string playerId, string key, string value)
        {
            if (IsConnect())
            {
                var query = $" UPDATE {table} SET {key} = '{value}' WHERE steamid = {playerId} ";
                var cmd = new MySqlCommand(query, this.Connection);
                cmd.ExecuteNonQuery();
            }
        }

        public string Get(string table, string what, string name, string value)
        {
            string x = null;

            if (IsConnect())
            {

                var query = $" SELECT {what} FROM {table} WHERE {name} = '{value}' ";
                var cmd = new MySqlCommand(query, this.Connection);
                var reader = cmd.ExecuteReader();

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

                var query = $" SELECT {what} FROM {table} WHERE {name} = '{value}' ";
                var cmd = new MySqlCommand(query, this.Connection);
                var reader = await cmd.ExecuteReaderAsync();

                while (await reader.ReadAsync())
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

        public async Task<string> GetAsync(string table, string what, string condition)
        {
            string x = null;

            if (IsConnect())
            {

                string query = $" SELECT {what} FROM {table} {condition}";
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
        /*public async Task<List<string>> GetListAsync(string table, string what, string name, string value)
        {
            List<string> x = new List<string>();

            if (IsConnect())
            {
                string query = $" SELECT {what} FROM {table} WHERE {name} = '{value}' ";
                var cmd = new MySqlCommand(query, this.Connection);
                var reader = await cmd.ExecuteReaderAsync();

                while (reader.Read())
                {
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        x.Add(reader[i].ToString());
                    }
                }
                reader.Close();

                return (x.Count > 0)? x : null;
            }
            else
            {
                return null;
            }
        }*/

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
