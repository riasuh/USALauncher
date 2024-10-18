using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace USALauncher
{
    internal static class Program
    {
        [STAThread]
        private static async Task Main() // Changed from void to Task
        {
            // Ensure the application is started in a single-threaded apartment state
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Ping the server
            bool isServerReachable = await IsServerReachableAsync("usa-life.net");
            if (!isServerReachable)
            {
                MessageBox.Show("usa-life.net konnte nicht erreicht werden", "Fehler beim Starten", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return; // Exit if the server is unreachable
            }

            // Check for updates
            await CheckForUpdatesAsync();

            // Run the main form
            Application.Run(new Mainframe());
        }

        private static async Task<bool> IsServerReachableAsync(string host)
        {
            try
            {
                using (var ping = new Ping())
                {
                    var reply = await ping.SendPingAsync(host, 1000);
                    return reply.Status == IPStatus.Success;
                }
            }
            catch
            {
                return false; // Return false if there's an exception
            }
        }
        private static async Task CheckForUpdatesAsync()
        {
            using HttpClient httpClient = new();
            try
            {
                httpClient.DefaultRequestHeaders.Add("user-agent", "Only a test!");
                string url = "https://download.usa-life.net/launcherversion.txt?rand=" + Guid.NewGuid();
                string text = await httpClient.GetStringAsync(url);

                // Überprüfe, ob die heruntergeladene Version von der aktuellen Version abweicht
                if (text.Trim() != Assembly.GetExecutingAssembly().GetName().Version.ToString())
                {
                    DialogResult result = MessageBox.Show("Es ist eine neue Version des USA LIFE Launchers verfügbar.\n" +
                        "Aktuell installiert: " + Assembly.GetExecutingAssembly().GetName().Version.ToString() + "\n" +
                        "Aktuellste Version: " + text.Trim() + "\n" +
                        "Möchten Sie jetzt aktualisieren?", "Neue Version verfügbar!", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk);

                    if (result == DialogResult.Yes)
                    {
                        // Lade und starte die neueste Version
                        await DownloadAndRunLatestVersionAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fehler beim Updaten des Launchers (Fehlercode: #2)\nBitte wende dich an die Administration.\n\nFehlerdetails: " + ex.Message, "Fehler beim Starten", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        private static async Task DownloadAndRunLatestVersionAsync()
        {
            try
            {
                // Implementiere das herunterladen und starten der neuesten Version
                using HttpClient httpClient = new();
                httpClient.DefaultRequestHeaders.Add("User-Agent", "Only a test!");
                string latestVersionFileUrl = "https://download.usa-life.net/USALauncher.exe?raw=true";
                string tempFileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads", "USALauncher.exe");

                // Lade die Datei herunter und speichere sie im angegebenen Pfad
                using HttpResponseMessage response = await httpClient.GetAsync(latestVersionFileUrl, HttpCompletionOption.ResponseHeadersRead);
                response.EnsureSuccessStatusCode();

                using var fileStream = new FileStream(tempFileName, FileMode.Create, FileAccess.Write, FileShare.None);
                using var httpStream = await response.Content.ReadAsStreamAsync();
                await httpStream.CopyToAsync(fileStream);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fehler beim Aktualisieren der Anwendung: " + ex.Message, "Fehler beim Starten", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }




    }
}
