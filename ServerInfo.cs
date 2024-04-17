using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace USALauncher;

public class ServerInfo
{
	public string GameVersion { get; set; }

	public string HostName { get; set; }

	public string MapName { get; set; }

	public string GameType { get; set; }

	public int NumPlayers { get; set; }

	public int NumTeam { get; set; }

	public int MaxPlayers { get; set; }

	public string GameMode { get; set; }

	public string TimeLimit { get; set; }

	public bool Password { get; set; }

	public string CurrentVersion { get; set; }

	public string RequiredVersion { get; set; }

	public string Mod { get; set; }

	public bool BattleEye { get; set; }

	public double Longitude { get; set; }

	public double Latitude { get; set; }

	public List<string> Players { get; set; }

	public string Mission { get; set; }

	public static ServerInfo Parse(string data)
	{
        ServerInfo serverInfo = new ServerInfo();
        string[] array = data.Split(default(char));
        Dictionary<string, string> dictionary = new Dictionary<string, string>();

        // Überprüfe, ob das Array die erwartete Länge hat
        if (array.Length % 2 == 0)
        {
            for (int i = 0; i < array.Length; i++)
            {
                if ((i & 1) == 0 && !dictionary.ContainsKey(array[i]) && i + 1 < array.Length)
                {
                    dictionary.Add(array[i], array[i + 1]);
                }
            }
        }
        else
        {
            // Handle den Fall, wenn das Array keine gerade Anzahl von Elementen hat
            // Hier könntest du eine Fehlermeldung ausgeben oder eine geeignete Maßnahme ergreifen.
            // Zum Beispiel:
            Console.WriteLine("Fehler beim Analysieren der Daten. Ungültige Anzahl von Elementen im Array.");
            // Oder:
            // throw new ArgumentException("Ungültige Anzahl von Elementen im Array.");
        }

        serverInfo.GameVersion = GetValueByKey("gamever", dictionary);
		serverInfo.HostName = GetValueByKey("hostname", dictionary);
		serverInfo.MapName = GetValueByKey("mapname", dictionary);
		serverInfo.GameType = GetValueByKey("gametype", dictionary);
		serverInfo.NumPlayers = ParseInt(GetValueByKey("numplayers", dictionary));
		serverInfo.NumTeam = ParseInt(GetValueByKey("numteams", dictionary));
		serverInfo.MaxPlayers = ParseInt(GetValueByKey("maxplayers", dictionary));
		serverInfo.GameMode = GetValueByKey("gamemode", dictionary);
		serverInfo.TimeLimit = GetValueByKey("timelimit", dictionary);
		serverInfo.Password = ParseBoolean(GetValueByKey("password", dictionary));
		serverInfo.CurrentVersion = GetValueByKey("currentVersion", dictionary);
		serverInfo.RequiredVersion = GetValueByKey("requiredVersion", dictionary);
		serverInfo.Mod = GetValueByKey("mod", dictionary);
		serverInfo.BattleEye = ParseBoolean(GetValueByKey("sv_battleye", dictionary));
		serverInfo.Longitude = ParseDouble(GetValueByKey("lng", dictionary));
		serverInfo.Latitude = ParseDouble(GetValueByKey("lat", dictionary));
		serverInfo.Mission = GetValueByKey("mission", dictionary);
		return serverInfo;
	}

    internal static ServerInfo Parse(byte[] response)
    {
        ServerInfo serverInfo = new ServerInfo();
        int pos = 5;

        // Stelle sicher, dass pos innerhalb des gültigen Bereichs liegt
        if (pos >= response.Length)
        {
            Logger.WriteDownToLog("Fehler beim Parsen: 'pos' liegt außerhalb des gültigen Bereichs.");
            return serverInfo; // Gib ein leeres ServerInfo-Objekt zurück
        }

        serverInfo.HostName = GetNextPartString(ref pos, response);

        // Stelle sicher, dass pos innerhalb des gültigen Bereichs liegt
        if (pos >= response.Length)
        {
            Logger.WriteDownToLog("Fehler beim Parsen: 'pos' liegt außerhalb des gültigen Bereichs.");
            return serverInfo; // Gib ein leeres ServerInfo-Objekt zurück
        }

        serverInfo.MapName = GetNextPartString(ref pos, response);

        // Führe ähnliche Überprüfungen für andere Eigenschaften durch
        // ...

        // Überprüfe, ob es genug Daten gibt, um NumPlayers und MaxPlayers zu parsen
        if (pos + 1 < response.Length)
        {
            serverInfo.NumPlayers = response[pos++];
            serverInfo.MaxPlayers = response[pos++];
        }
        else
        {
            Logger.WriteDownToLog("Fehler beim Parsen: Nicht genügend Daten für NumPlayers und MaxPlayers.");
            return serverInfo; // Gib ein ServerInfo-Objekt mit den bisher geparsten Daten zurück
        }

        // Führe weitere Überprüfungen durch
        // ...

        return serverInfo;
    }



    public static class Logger
    {
        public static void WriteDownToLog(string message)
        {
            if (!Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + "Logs"))
            {
                Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "Logs");
            }
            using (StreamWriter streamWriter = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "Logs\\ErrorLog-" + DateTime.Now.ToString("dd-MM-yyyy"), append: true))
            {
                streamWriter.WriteLine(message);
            }
        }
    }


    private static byte[] GetNextPart(ref int pos, byte[] response)
    {
        byte[] array = new byte[0];
        for (int i = 0; i < response.Length; i++)
        {
            if (response[i + pos] == 0)
            {
                pos = i + pos + 1;
                return array;
            }
            Array.Resize(ref array, array.Length + 1);
            array[i] = response[i + pos];
        }
        return new byte[0];
    }


    private static string GetNextPartString(ref int pos, byte[] response)
	{
		byte[] nextPart = GetNextPart(ref pos, response);
		return Encoding.ASCII.GetString(nextPart);
	}

	private static int ParseInt(string value)
	{
		if (string.IsNullOrWhiteSpace(value))
		{
			return 0;
		}
		int result = 0;
		int.TryParse(value, out result);
		return result;
	}

	private static double ParseDouble(string value)
	{
		if (string.IsNullOrWhiteSpace(value))
		{
			return 0.0;
		}
		double result = 0.0;
		double.TryParse(value, out result);
		return result;
	}

	private static bool ParseBoolean(string value)
	{
		if (string.IsNullOrWhiteSpace(value))
		{
			return false;
		}
		if (value == "1" || value.ToLowerInvariant() == "true")
		{
			return true;
		}
		return false;
	}

	private static string GetValueByKey(string key, Dictionary<string, string> values)
	{
		if (values.ContainsKey(key))
		{
			return values[key];
		}
		return null;
	}
}
