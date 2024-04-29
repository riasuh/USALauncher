using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Cache;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Windows.Forms;

namespace USALauncher
{
    internal static class Program
    {
        [STAThread]
        private static void Main()
        {
            try
            {
                new Ping().Send("usa-life.net", 1000);
            }
            catch
            {
                MessageBox.Show("usa-life.net konnte nicht erreicht werden", "Fehler beim Starten", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }

            // Überprüfe auf Updates
            CheckForUpdates();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(defaultValue: false);
            Application.Run(new Mainframe());
        }

        private static void CheckForUpdates()
        {
            using (WebClient webClient = new WebClient())
            {
                try
                {
                    webClient.Headers.Add("user-agent", "Only a test!");
                    string url = "https://download.usa-life.net/launcherversion.txt?rand=" + Guid.NewGuid();
                    string text = webClient.DownloadString(url);

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
                            DownloadAndRunLatestVersion();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Fehler beim Updaten des Launchers (Fehlercode: #2)\nBitte wende dich an die Administration.\n\nFehlerdetails: " + ex.Message, "Fehler beim Starten", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
            }
        }

        private static void DownloadAndRunLatestVersion()
        {
            try
            {
                // Implementiere das Herunterladen und Starten der neuesten Version
                using (WebClient webClient = new WebClient())
                {
                    webClient.Headers.Add("user-agent", "Only a test!");
                    string latestVersionFileUrl = "https://download.usa-life.net/USALauncher.exe?raw=true";
                    string tempFileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads", "USALauncher.exe");
                    // Setze den Pfad, wo die Datei gespeichert wird
                    webClient.DownloadFile(latestVersionFileUrl, tempFileName);

                    Process.Start(tempFileName);

                    System.Diagnostics.Debug.WriteLine("Die neueste Version wird heruntergeladen und gestartet...");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fehler beim Aktualisieren der Anwendung: " + ex.Message, "Fehler beim Starten", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }




    }
}
