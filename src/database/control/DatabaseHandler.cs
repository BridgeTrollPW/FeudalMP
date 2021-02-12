using FeudalMP.src.foundation;
using FeudalMP.src.util;
using Godot;
using MySql.Data.MySqlClient;

public class DatabaseHandler
{
    public DatabaseHandler()
    {
        string cs = $"server={Settings.Instance.GetValue(CFG.Server.SECTION, CFG.Server.DATABASE_HOST)};userid={Settings.Instance.GetValue(CFG.Server.SECTION, CFG.Server.DATABASE_USER)};password={Settings.Instance.GetValue(CFG.Server.SECTION, CFG.Server.DATABASE_USER_PASSWORD)};database={Settings.Instance.GetValue(CFG.Server.SECTION, CFG.Server.DATABASE_NAME)}";
        using (var con = new MySqlConnection(cs))
        {
            con.Open();
            GD.Print($"MySQL version : {con.ServerVersion}");
        }
    }
}