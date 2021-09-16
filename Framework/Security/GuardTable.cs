/*using MySql.Data.MySqlClient;
using RealLifeFramework.Database;
using RealLifeFramework.Threadding;
using System;
using System.Threading.Tasks;

namespace RealLifeFramework.Security
{
    // trying new syntax and "vychytavky in C#"
    // actually singleton is better name than instance :D
    [DatabaseTable]
    public class GuardTable : ITable
    {
        public string Name => "guard";
        public MySqlCommand StartCommand()
        {
            return new MySqlCommand( $"CREATE TABLE IF NOT EXISTS {Name} ( " +
                        "steamid VARCHAR(17) PRIMARY KEY," +
                        "hwid VARCHAR(40)," +
                        "ban BIT DEFAULT 0," +
                        "dateBanned INT DEFAULT 0," +
                        "time INT DEFAULT 0" +
                        ")", DatabaseManager.Singleton.Connection);
        }

        public static GuardTable Singleton => m_Singleton = m_Singleton ?? new GuardTable();
        private static GuardTable m_Singleton;

        public async Task<EGuardBanResult> CheckHWIDBan(string steamid, string hwid)
        {
            var isBanned = await DatabaseManager.Singleton.GetAsync(Name, "ban", "hwid", hwid);

            switch (isBanned)
            {
                case "0":
                    var isReallyBanned = await DatabaseManager.Singleton.GetAsync(Name, "ban", $"WHERE hwid = '{hwid}' AND ban = 1");

                    if (isReallyBanned == null)
                    {
                        return EGuardBanResult.False;
                    }
                    else
                    {
                        var dBR = await DatabaseManager.Singleton.GetAsync(Name, "dateBanned", "hwid", hwid);
                        var tR = await DatabaseManager.Singleton.GetAsync(Name, "time", "hwid", hwid);

                        if (int.TryParse(dBR, out var dB) && int.TryParse(tR, out var t))
                        {
                            if ((dB + t) <= DateTimeOffset.Now.ToUnixTimeSeconds())
                            {
                                await new MySqlCommand($"UPDATE {Name} SET ban = 0, dateBanned = 0, time = 0 WHERE hwid = '{hwid}' AND steamid = '{steamid}'", DatabaseManager.Singleton.Connection).ExecuteNonQueryAsync();
                                return EGuardBanResult.ToUnban;
                            }
                        }
                        return EGuardBanResult.True;
                    }

                case "1":
                    var dateBannedRaw = await DatabaseManager.Singleton.GetAsync(Name, "dateBanned", "hwid", hwid);
                    var timeRaw = await DatabaseManager.Singleton.GetAsync(Name, "time", "hwid", hwid);

                    if (int.TryParse(dateBannedRaw, out var dateBanned) && int.TryParse(timeRaw, out var time))  
                    {
                        if ((dateBanned + time) <= DateTimeOffset.Now.ToUnixTimeSeconds())
                        {
                            await new MySqlCommand($"UPDATE {Name} SET ban = 0, dateBanned = 0, time = 0 WHERE hwid = '{hwid}' AND steamid = '{steamid}'", DatabaseManager.Singleton.Connection).ExecuteNonQueryAsync();
                            return EGuardBanResult.ToUnban;
                        }
                    }
                    return EGuardBanResult.True;
    
                case null:
                    await new MySqlCommand($"INSERT INTO {Name} (hwid, steamid) VALUES ('{hwid}','{steamid}')", DatabaseManager.Singleton.Connection).ExecuteNonQueryAsync();
                    return EGuardBanResult.False;
            }

            return EGuardBanResult.False;
        }

        public void AddHWIDBan(string steamid, int time)
        {
            Helper.ExecuteAsync( async () =>
            {
                if (DatabaseManager.Singleton.IsConnect())
                {
                    await new MySqlCommand($"UPDATE {Name} SET ban = 1, dateBanned = {DateTimeOffset.Now.ToUnixTimeSeconds()}, time = {time} WHERE steamid = '{steamid}'", DatabaseManager.Singleton.Connection).ExecuteNonQueryAsync();
                }
            });
        }

        public void RemoveHWIDBan(string steamid)
        {
            Helper.ExecuteAsync( async () =>
            {
                if (DatabaseManager.Singleton.IsConnect())
                { 
                    await new MySqlCommand($"UPDATE {Name} SET ban = 0, dateBanned = 0, time = 0 WHERE steamid = '{steamid}'", DatabaseManager.Singleton.Connection).ExecuteNonQueryAsync();
                }
            });
        }

        public GuardTable () { }
    }
}
*/