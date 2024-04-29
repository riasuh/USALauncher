using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace USALauncher;

public class ArmA3ServerInfo
{
	public string host;

	public int port;

	public long ping;

	public Stopwatch Stopwatch = new Stopwatch();

	public ServerInfo ServerInfo { get; protected set; }

	public PlayerCollection Players { get; protected set; }

	public ArmA3ServerInfo(string host, int port)
	{
		this.host = host;
		this.port = port;
	}

    public void Update()
    {
        try
        {
            using UdpClient udpClient = new UdpClient(56800);
            Stopwatch.Reset();
            IPEndPoint remoteEP = new IPEndPoint(IPAddress.Parse(host), port);
            udpClient.Client.ReceiveTimeout = 100;
            udpClient.Connect(remoteEP);
            try
            {
                byte[] array = new byte[25]
                {
                255, 255, 255, 255, 84, 83, 111, 117, 114, 99,
                101, 32, 69, 110, 103, 105, 110, 101, 32, 81,
                117, 101, 114, 121, 0
                };
                udpClient.Send(array, array.Length);
                Stopwatch.Start();
                byte[] response = udpClient.Receive(ref remoteEP);

                // Protokollieren Sie den Inhalt der response-Variable
                System.Diagnostics.Debug.WriteLine("Response from server: " + BitConverter.ToString(response));

                Stopwatch.Stop();
                ping = Stopwatch.ElapsedMilliseconds;
                ServerInfo = ServerInfo.Parse(response);
            }
            catch (SocketException ex)
            {
                LogException("SocketException", ex);
            }
            catch (IndexOutOfRangeException ex)
            {
                LogException("IndexOutOfRangeException", ex);
            }
            catch (Exception ex)
            {
                LogException("Exception", ex);
            }
        }
        catch (SocketException value)
        {
            System.Diagnostics.Debug.WriteLine(value);
        }
        catch (Exception value2)
        {
            System.Diagnostics.Debug.WriteLine(value2);
        }
    }


    private void LogException(string exceptionType, Exception ex)
    {
        // Hier protokollierst du die Ausnahme in einer Datei oder Konsole.
        string logMessage = $"{DateTime.Now.ToString("HH:mm:ss")} - {exceptionType} -> {ex.Message}\nStackTrace: {ex.StackTrace}";
        WriteDownToLog(logMessage);
        System.Diagnostics.Debug.WriteLine(logMessage);
    }



    private void WriteDownToLog(string message)
    {
        string logDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");
        if (!Directory.Exists(logDirectory))
        {
            Directory.CreateDirectory(logDirectory);
        }
        string logFilePath = Path.Combine(logDirectory, $"ErrorLog-{DateTime.Now.ToString("dd-MM-yyyy")}.txt");

        using (StreamWriter streamWriter = new StreamWriter(logFilePath, append: true))
        {
            streamWriter.WriteLine(message);
        }
    }

}
