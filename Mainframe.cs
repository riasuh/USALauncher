using SteamQuery;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using USALauncher.Properties;
using USALauncher.Resources;

namespace USALauncher;

public class Mainframe : Form
{
    private Font myFont;
    private Font myFontlblArmaSpieler;
    private string missionDownloadUri = "https://download.usa-life.net/mission.txt";

    private string modDownloadUri = "https://download.usa-life.net/mod.txt";

    public const int WM_NCLBUTTONDOWN = 161;

    public const int HT_CAPTION = 2;

    public string armaPath;

    public string localPath;

    public string profilePath;

    public string defaultprofilepath;

    private IPAddress[] addresslist = Dns.GetHostAddresses("s.usa-life.net");

    private System.Threading.Timer timer;

    private IContainer components;

    private Label lblPathDescription;

    private Label lblInstallationPath;

    private Button btnChangePath;

    private Label lblVersion;

    private ComboBox cbProfile;

    private Label lblProfil;

    private CheckBox cbSplash;

    private CheckBox cbWindow;

    private Label lblMaxVram;

    private NumericUpDown nudVram;

    private TextBox txtParams;

    private Label lblParams;

    private Label lblProfileDescription;

    private Label lblprofilePath;

    private Button btnProfilePath;

    private CheckBox cbHyper;

    private CheckBox cbIntro;

    private CheckBox cbnologs;

    private FolderBrowserDialog fbdprofilePath;

    private Button buttontexture;

    private ToolTip toolTip1;

    private ToolTip toolTip2;

    private ToolTip toolTipLaunch;

    private PictureBox picStatsImage;

    private PictureBox picInfo;
    private PictureBox picBanner;
    private PictureBox picInstagram;
    private PictureBox picDiscord;
    private PictureBox picUpdates;
    private PictureBox picHomepage;
    private PictureBox picRegeln;
    private PictureBox picTeamspeak;
    private PictureBox picSpielerOnline;
    private PictureBox picSteam;
    private PictureBox picYoutube;
    private PictureBox picClose;
    private Label lblMaxVram2;
    private PictureBox picReload;
    private PictureBox picMinimize;
    private System.Windows.Forms.Timer updateTimer;
    private Label playersOnlineLabel;
    private PictureBox picGetServerInfo;
    private PictureBox picPlay;

    public object MainFrame { get; private set; }

    public object MainForm { get; private set; }
    private Dictionary<PictureBox, Image> originalImages = new Dictionary<PictureBox, Image>();
    private void MainFrame_Load(object sender, EventArgs e)
    {
        AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
        AppDomain.CurrentDomain.FirstChanceException += CurrentDomain_FirstChanceException;
        // Füge deine PictureBox-Instanzen zum originalImages-Dictionary hinzu
        originalImages.Add(picSteam, picSteam.Image);
        originalImages.Add(picRegeln, picRegeln.Image);
        originalImages.Add(picHomepage, picHomepage.Image);
        originalImages.Add(picTeamspeak, picTeamspeak.Image);
        originalImages.Add(picUpdates, picUpdates.Image);
        originalImages.Add(picDiscord, picDiscord.Image);
        originalImages.Add(picInstagram, picInstagram.Image);
        originalImages.Add(picYoutube, picYoutube.Image);
        originalImages.Add(picClose, picClose.Image);
        originalImages.Add(picReload, picReload.Image);
        originalImages.Add(picMinimize, picMinimize.Image);
        originalImages.Add(picInfo, picInfo.Image);
        originalImages.Add(picGetServerInfo, picGetServerInfo.Image);
        originalImages.Add(picPlay, picPlay.Image);
        // Initialisiere den Timer
        updateTimer = new System.Windows.Forms.Timer();
        updateTimer.Interval = 60000; // 60 Sekunden
        updateTimer.Tick += UpdateTimer_Tick;
        updateTimer.Start();

        // Spielerinformationen initial laden
        LoadPlayerInformation();

    }
    private async void LoadPlayerInformation()
    {
        // IP-Adresse und Port des Servers
        string serverAddress = "s.usa-life.net:2303";

        // Verbindung zum Server herstellen und Abfrage durchführen
        using (var server = new GameServer(serverAddress))
        {
            await server.PerformQueryAsync();

            var information = server.Information;

            // Spielerinformationen in das Label einfügen
            playersOnlineLabel.Text = $"Spieler online: {information.OnlinePlayers}/{information.MaxPlayers}\n";

            // Weitere Informationen anzeigen oder Aktionen ausführen, falls benötigt
            // ...
        }
    }

    private void UpdateTimer_Tick(object sender, EventArgs e)
    {
        // Spielerinformationen aktualisieren
        LoadPlayerInformation();
    }
    private void CurrentDomain_FirstChanceException(object sender, FirstChanceExceptionEventArgs e)
    {
        WriteDownToLog(DateTime.Now.ToString("HH:mm:ss") + " - First Chance EXC -> " + e.Exception.ToString());
    }

    private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
    {
        WriteDownToLog(DateTime.Now.ToString("HH:mm:ss") + " - Unhandled EXC -> " + e.ExceptionObject.ToString());
    }

    private static Mutex logMutex = new Mutex(); // Globale Mutex-Variable

    private void WriteDownToLog(string message)
    {
        string logdirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), Application.CompanyName, Application.ProductName + "\\Logs");
        // Versuche, den Mutex zu sperren
        if (logMutex.WaitOne())
        {
            try
            {
                if (!Directory.Exists(logdirectory))
                {
                    Directory.CreateDirectory(logdirectory);
                }
                using (StreamWriter streamWriter = new StreamWriter(logdirectory + "\\ErrorLog-" + DateTime.Now.ToString("dd-MM-yyyy"), append: true))
                {
                    streamWriter.WriteLine(message);
                }
            }
            finally
            {
                // Gib den Mutex frei, wenn das Schreiben abgeschlossen ist
                logMutex.ReleaseMutex();
            }
        }
    }
    // Deklaration der benutzerdefinierten Schriftart
    [System.Runtime.InteropServices.DllImport("gdi32.dll")]
    private static extern IntPtr AddFontMemResourceEx(IntPtr pbFont, uint cbFont,
            IntPtr pdv, [System.Runtime.InteropServices.In] ref uint pcFonts);

    private PrivateFontCollection fonts = new PrivateFontCollection();

    public Mainframe()
    {
        InitializeComponent();
        // Lade die benutzerdefinierte Schriftart beim Initialisieren des Formulars
        LoadCustomFont();
        base.Icon = USALauncher.Properties.Resources.Icon_1_USA_128;
        CenterToScreen();
        lblVersion.Text = "USA LIFE Launcher v." + Assembly.GetExecutingAssembly().GetName().Version.ToString();

        armaPath = Settings.Default.armaPath.ToString();
        if (string.IsNullOrEmpty(armaPath))
        {
            new PathSelection().ShowDialog();
        }
        profilePath = Settings.Default.profilePath.ToString();
        if (string.IsNullOrEmpty(profilePath))
        {
            new ProfilePathSelection().ShowDialog();
        }
        lblprofilePath.Text = profilePath;
        armaPath = Settings.Default["armaPath"].ToString();
        localPath = Environment.GetEnvironmentVariable("localappdata") + "\\Arma 3\\MPMissionsCache";
        defaultprofilepath = Environment.GetEnvironmentVariable("userprofile") + "\\Documents\\Arma 3 - Other Profiles";
        profilePath = Settings.Default["profilePath"].ToString();
        loadProfiles();
        lblprofilePath.Text = profilePath;
        lblInstallationPath.Text = armaPath;
        cbSplash.Checked = Settings.Default.noSplash;
        cbWindow.Checked = Settings.Default.windowed;
        nudVram.Value = Settings.Default.maxVram;
        txtParams.Text = Settings.Default.customParams;
        cbHyper.Checked = Settings.Default.enableHT;
        cbIntro.Checked = Settings.Default.skipIntro;
        cbnologs.Checked = Settings.Default.noLogs;


    }

    [DllImport("user32.dll")]
    public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

    [DllImport("user32.dll")]
    public static extern bool ReleaseCapture();

    private void picBanner_MouseDown(object sender, MouseEventArgs e)
    {
        if (e.Button == MouseButtons.Left)
        {
            ReleaseCapture();
            SendMessage(base.Handle, 161, 2, 0);
        }
    }

    private void picLogo_MouseDown(object sender, MouseEventArgs e)
    {
        if (e.Button == MouseButtons.Left)
        {
            ReleaseCapture();
            SendMessage(base.Handle, 161, 2, 0);
        }
    }

    private void picTeamspeak_Click(object sender, EventArgs e)
    {
        Process.Start(new ProcessStartInfo("ts3server://ts.usa-life.net") { UseShellExecute = true });
    }

    private void picRegeln_Click(object sender, EventArgs e)
    {
        Process.Start(new ProcessStartInfo("https://wiki.usa-life.net/de/Regelwerk/Server") { UseShellExecute = true });
    }

    private void picHomepage_Click(object sender, EventArgs e)
    {
        Process.Start(new ProcessStartInfo("https://usa-life.net/") { UseShellExecute = true });
    }

    private void picForum_Click(object sender, EventArgs e)
    {
        Process.Start(new ProcessStartInfo("https://forum.usa-life.net/") { UseShellExecute = true });
    }

    private void serverUpdatesButton_Click(object sender, EventArgs e)
    {
        Process.Start(new ProcessStartInfo("https://wiki.usa-life.net/de/Changelog") { UseShellExecute = true });
    }

    private void picSteam_Click(object sender, EventArgs e)
    {
        Process.Start(new ProcessStartInfo("https://steamcommunity.com/groups/usaliferpg") { UseShellExecute = true });
    }

    private void facebookBtn_Click(object sender, EventArgs e)
    {
        Process.Start(new ProcessStartInfo("https://www.facebook.com/usaliferpg/") { UseShellExecute = true });
    }

    private void btnChangePath_Click(object sender, EventArgs e)
    {
        new PathSelection().ShowDialog();
        armaPath = Settings.Default["armaPath"].ToString();
        lblInstallationPath.Text = armaPath;
    }




    private void btnLaunch_Click(object sender, EventArgs e)
    {
        Updaterplaybutton();
        using (WebClient webClient = new WebClient())
        {
            webClient.Headers.Add("user-agent", "Only a test!");
            string text = webClient.DownloadString(missionDownloadUri);
            text = text.Split(new string[1] { "\n" }, StringSplitOptions.None)[0];
            string text2 = text.Split('/').Last();
            _ = armaPath + "\\@USALifeMod";
            if (!File.Exists(localPath + "\\" + text2))
            {
                new DownloadForm(text, localPath, armaPath, this).ShowDialog();
                return;
            }
            toolTipLaunch.Show("Arma 3 wird gestartet...", this, Cursor.Position.X - base.Location.X, Cursor.Position.Y - base.Location.Y, 100000);
            ProcessStartInfo processStartInfo = new ProcessStartInfo();
            processStartInfo.FileName = Path.GetFileName(armaPath + "\\arma3launcher.exe");
            processStartInfo.WorkingDirectory = Path.GetDirectoryName(armaPath + "\\arma3launcher.exe");
            processStartInfo.Arguments = " -noLauncher -connect=s.usa-life.net -useBE -mod=@USALifeMod";
            if (Settings.Default.profile != null && Settings.Default.profile != "")
            {
                UserProfile userProfile = new UserProfile(Settings.Default.profile);
                processStartInfo.Arguments += $" \"-name={userProfile}\"";
            }
            if (cbSplash.Checked)
            {
                processStartInfo.Arguments += " -nosplash";
            }
            if (cbWindow.Checked)
            {
                processStartInfo.Arguments += " -window";
            }
            if (cbHyper.Checked)
            {
                processStartInfo.Arguments += " -enableHT";
            }
            if (cbIntro.Checked)
            {
                processStartInfo.Arguments += " -skipIntro";
            }
            if (cbnologs.Checked)
            {
                processStartInfo.Arguments += " -noLogs";
            }
            if (nudVram.Value != 0m)
            {
                processStartInfo.Arguments += $" -maxVRAM={nudVram.Value}";
            }
            if (txtParams.Text.Length != 0)
            {
                processStartInfo.Arguments += $" {txtParams.Text}";
            }
            processStartInfo.UseShellExecute = true; // Setze UseShellExecute auf true
            try
            {
                Process.Start(processStartInfo);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fehler beim Starten von Arma 3: " + ex.Message);
            }
        }
        disableLaunchButton();
    }


    private void btnLaunch_Clickfake()
    {
        Updaterplaybutton();
        using WebClient webClient = new WebClient();
        webClient.Headers.Add("user-agent", "Only a test!");
        string text = webClient.DownloadString(missionDownloadUri);
        text = text.Split(new string[1] { "\n" }, StringSplitOptions.None)[0];
        string text2 = text.Split('/').Last();
        _ = armaPath + "\\@USALifeMod";
        if (!File.Exists(localPath + "\\" + text2))
        {
            new DownloadForm(text, localPath, armaPath, this).ShowDialog();
        }
    }

    public void disableLaunchButton()
    {
        this.InvokeEx(delegate (Mainframe f)
        {
            f.picPlay.Enabled = false;
        });
        this.InvokeEx(delegate (Mainframe f)
        {
            f.picPlay.Text = "Starte ArmA...";
        });
        timer = new System.Threading.Timer(delegate
        {
            this.InvokeEx(delegate (Mainframe f)
            {
                f.picPlay.Enabled = true;
            });
            this.InvokeEx(delegate (Mainframe f)
            {
                f.picPlay.Text = "USA LIFE Spielen";
            });
            timer.Dispose();
        }, null, 10000, -1);
    }

    private void MouseHoverEnter(object sender, EventArgs e)
    {
        if (sender is PictureBox)
        {
            PictureBox pictureBox = (PictureBox)sender;

            // Überprüfe, ob die PictureBox die spezielle PictureBox ist, für die der Hover-Effekt implementiert werden soll (picClose)
            if (pictureBox == picClose)
            {
                // Setze das rote Bild für den Close-Button
                pictureBox.Image = Properties.Resources.picCloseRed_Image;
            }
            else if (pictureBox == picMinimize)
            {
                pictureBox.Image = Properties.Resources.picMinimizeGreen_Image;
            }
            else if (pictureBox == picInfo)
            {
                pictureBox.Image = Properties.Resources.picInfoBlue_Image;
            }
            else
            {
                // Bild in der PictureBox heller machen
                pictureBox.Image = AdjustBrightness((Bitmap)originalImages[pictureBox], 1.5f); // Helligkeit um 50% erhöhen
            }
        }
    }
    private void MouseHoverLeave(object sender, EventArgs e)
    {
        if (sender is PictureBox)
        {
            PictureBox pictureBox = (PictureBox)sender;

            // Überprüfe, ob die PictureBox die spezielle PictureBox ist, für die der Hover-Effekt implementiert wurde (picClose)
            if (pictureBox == picClose)
            {
                // Setze das normale Bild für den Close-Button zurück
                pictureBox.Image = Properties.Resources.picCloseWhite_Image;
            }
            else
            {
                // Originalbild wiederherstellen
                pictureBox.Image = originalImages[pictureBox];
            }
        }
    }

    private void AdaptChangelog(object sender, EventArgs e)
    {
    }

    private void cbProfile_SelectedIndexChanged(object sender, EventArgs e)
    {
        Settings.Default.profile = ((UserProfile)cbProfile.SelectedItem).path;
        Settings.Default.Save();
        Settings.Default.Reload();
    }

    private void cbSplash_Click(object sender, EventArgs e)
    {
        Settings.Default.noSplash = ((CheckBox)sender).Checked;
        Settings.Default.Save();
        Settings.Default.Reload();
    }

    private void cbWindow_Click(object sender, EventArgs e)
    {
        Settings.Default.windowed = ((CheckBox)sender).Checked;
        Settings.Default.Save();
        Settings.Default.Reload();
    }

    private void cbHyper_Click(object sender, EventArgs e)
    {
        Settings.Default.enableHT = ((CheckBox)sender).Checked;
        Settings.Default.Save();
        Settings.Default.Reload();
    }

    private void cbIntro_Click(object sender, EventArgs e)
    {
        Settings.Default.skipIntro = ((CheckBox)sender).Checked;
        Settings.Default.Save();
        Settings.Default.Reload();
    }

    private void cbnologs_Click(object sender, EventArgs e)
    {
        Settings.Default.noLogs = ((CheckBox)sender).Checked;
        Settings.Default.Save();
        Settings.Default.Reload();
    }

    private void nudVram_ValueChanged(object sender, EventArgs e)
    {
        Settings.Default.maxVram = Convert.ToInt32(((NumericUpDown)sender).Value);
        Settings.Default.Save();
        Settings.Default.Reload();
    }

    private void txtParams_TextChanged(object sender, EventArgs e)
    {
        Settings.Default.customParams = ((TextBox)sender).Text;
        Settings.Default.Save();
        Settings.Default.Reload();
    }

    private void selectProfilePath()
    {
        if (Directory.Exists(defaultprofilepath))
        {
            lblprofilePath.Text = defaultprofilepath;
            Settings.Default.profilePath = defaultprofilepath;
            Settings.Default.Save();
            Settings.Default.Reload();
            MessageBox.Show("yeah BOIIIIIIIII");
            return;
        }
        fbdprofilePath.SelectedPath = lblprofilePath.Text;
        if (fbdprofilePath.ShowDialog() == DialogResult.OK)
        {
            lblprofilePath.Text = fbdprofilePath.SelectedPath;
            loadProfiles();
            Settings.Default.profilePath = fbdprofilePath.SelectedPath;
            Settings.Default.Save();
            Settings.Default.Reload();
        }
    }

    private void btnProfilePath_Click(object sender, EventArgs e)
    {
        new ProfilePathSelection().ShowDialog();
        profilePath = Settings.Default["profilePath"].ToString();
        lblprofilePath.Text = profilePath;
        loadProfiles();
    }

    private void loadProfiles()
    {
        cbProfile.Items.Clear();
        lblprofilePath.Text = profilePath;
        // Überprüfen Sie, ob lblprofilePath.Text leer oder null ist
        if (!string.IsNullOrEmpty(lblprofilePath.Text))
        {
            string[] directories = Directory.GetDirectories(lblprofilePath.Text);

            foreach (string text in directories)
            {
                UserProfile userProfile = new UserProfile(text);
                cbProfile.Items.Add(userProfile);
                if (Settings.Default.profile != null && Settings.Default.profile == text)
                {
                    cbProfile.SelectedItem = userProfile;
                }
            }
        }
        else
        {
            // Behandeln Sie den Fall, wenn der Pfad leer oder null ist
            MessageBox.Show("Der Profilpfad wurde nicht richtig geladen.\nBitte die Anwendung nochmals starten.", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void picReload_Click(object sender2, EventArgs e2)
    {
        Updater();
    }

    private void Updater()
    {
        new DownloadForm();
        using (WebClient webClient = new WebClient())
        {
            webClient.Headers.Add("user-agent", "Only a test!");
            string text = webClient.DownloadString(missionDownloadUri).Split(new string[1] { "\n" }, StringSplitOptions.None)[0].Split('/').Last();
            if (File.Exists(localPath + "\\" + text))
            {
                MissionForm form = new MissionForm();
                form.StartPosition = FormStartPosition.Manual;
                form.Location = new Point(base.Location.X, base.Location.Y + base.Height - form.Height);
                form.Show();
                Thread.Sleep(2000);
                form.Close();
            }
            else
            {
                btnLaunch_Clickfake();
            }
        }
        DownloadForm download = new DownloadForm();
        using WebClient webClient2 = new WebClient();
        webClient2.Headers.Add("user-agent", "Only a test!");
        string text2 = webClient2.DownloadString(modDownloadUri);
        text2 = text2.Split(new string[1] { "\n" }, StringSplitOptions.None)[0];
        string text3 = text2.Split('_').Last();
        string text4 = text3.Split('.')[0] + "." + text3.Split('.')[1] + "." + text3.Split('.')[2] + "." + text3.Split('.')[3];
        string path = armaPath + "\\@USALifeMod\\version.txt";
        if (File.Exists(path))
        {
            string text5;
            using (StreamReader streamReader = File.OpenText(path))
            {
                text5 = streamReader.ReadLine();
            }
            if (text4 != text5)
            {
                if (!File.Exists(armaPath + "\\" + text3))
                {
                    Updaterplaybutton();
                }
            }
            else
            {
                Thread.Sleep(200);
                TexturemodForm form2 = new TexturemodForm();
                form2.StartPosition = FormStartPosition.Manual;
                form2.Location = new Point(base.Location.X, base.Location.Y + base.Height - form2.Height);
                form2.Show();
            }
        }
        else
        {
            download.Downloadtexture(text2, localPath, armaPath, this);
            download.ShowDialog();
        }
    }

    private void Updaterplaybutton()
    {
        DownloadForm download = new DownloadForm();
        using (WebClient webClient = new WebClient())
        {
            webClient.Headers.Add("user-agent", "Only a test!");
            string text = webClient.DownloadString(missionDownloadUri);
            text = text.Split(new string[1] { "\n" }, StringSplitOptions.None)[0];
            string text2 = text.Split('/').Last();
            if (!File.Exists(localPath + "\\" + text2))
            {
                download.DownloadUpdater(text, localPath, armaPath, this);
                download.ShowDialog();
            }
        }
        DownloadForm download2 = new DownloadForm();
        using WebClient webClient2 = new WebClient();
        webClient2.Headers.Add("user-agent", "Only a test!");
        string text3 = webClient2.DownloadString(modDownloadUri);
        text3 = text3.Split(new string[1] { "\n" }, StringSplitOptions.None)[0];
        string text4 = text3.Split('_').Last();
        string text5 = text4.Split('.')[0] + "." + text4.Split('.')[1] + "." + text4.Split('.')[2] + "." + text4.Split('.')[3];
        string path = armaPath + "\\@USALifeMod\\version.txt";
        if (File.Exists(path))
        {
            string text6;
            using (StreamReader streamReader = File.OpenText(path))
            {
                text6 = streamReader.ReadLine();
            }
            if (text5 != text6 && !File.Exists(armaPath + "\\" + text4))
            {
                download2.Downloadtexture(text3, localPath, armaPath, this, ShowDownloadSuccess: false);
                download2.ShowDialog();
            }
        }
        else
        {
            download2.Downloadtexture(text3, localPath, armaPath, this);
            download2.ShowDialog();
        }
    }
    private void cbHyper_CheckedChanged(object sender, EventArgs e)
    {
    }

    private void cbSplash_CheckedChanged(object sender, EventArgs e)
    {
    }

    private void buttontexture_Click(object sender, EventArgs e)
    {
        DownloadForm download = new DownloadForm();
        using WebClient webClient = new WebClient();
        webClient.Headers.Add("user-agent", "Only a test!");
        string text = webClient.DownloadString(modDownloadUri);
        string text2 = text.Split('_').Last();
        string text3 = text2.Split('.')[0] + "." + text2.Split('.')[1] + "." + text2.Split('.')[2] + "." + text2.Split('.')[3];
        string path = armaPath + "\\@USALifeMod\\version.txt";
        if (File.Exists(path))
        {
            string text4;
            using (StreamReader streamReader = File.OpenText(path))
            {
                text4 = streamReader.ReadLine();
            }
            if (text3 != text4)
            {
                if (!File.Exists(armaPath + "\\" + text2))
                {
                    download.Downloadtexture(text, localPath, armaPath, this);
                    download.ShowDialog();
                }
            }
            else
            {
                MessageBox.Show("Du hast bereits die neueste Version!");
            }
        }
        else
        {
            download.Downloadtexture(text, localPath, armaPath, this);
        }
    }

    private void button1_Click(object sender, EventArgs e)
    {
        new MissionForm().Show(this);
    }

    private void picMinimize_Click(object sender, EventArgs e)
    {
        base.WindowState = FormWindowState.Minimized;
    }
    private void picInfo_Click(object sender, EventArgs e)
    {
        InfoForm infoFenster = new InfoForm();
        infoFenster.StartPosition = FormStartPosition.CenterParent;
        infoFenster.ShowDialog();
    }

    private void picInstagram_Click(object sender, EventArgs e)
    {
        Process.Start(new ProcessStartInfo("https://www.instagram.com/usaliferpg/") { UseShellExecute = true });
    }
    private void picYoutube_Click(object sender, EventArgs e)
    {
        Process.Start(new ProcessStartInfo("https://www.youtube.com/@usaliferpg") { UseShellExecute = true });
    }
    private void picStatsImage_Click(object sender, EventArgs e)
    {
        Process.Start(new ProcessStartInfo("https://www.battlemetrics.com/servers/arma3") { UseShellExecute = true });
    }
    private void picDiscord_Click(object sender, EventArgs e)
    {
        Process.Start(new ProcessStartInfo("https://discord.gg/usaliferpg") { UseShellExecute = true });
    }
    private void picClose_Click(object sender, EventArgs e)
    {
        Application.Exit();
    }
    private async void picGetServerInfo_Click(object sender, EventArgs e)
    {
        LoadPlayerInformation();
    }
    protected override void Dispose(bool disposing)
    {
        if (disposing && components != null)
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }
    private Bitmap AdjustBrightness(Bitmap bitmap, float factor)
    {
        Bitmap adjustedBitmap = new Bitmap(bitmap.Width, bitmap.Height);

        // Durch jedes Pixel des Bildes iterieren
        for (int x = 0; x < bitmap.Width; x++)
        {
            for (int y = 0; y < bitmap.Height; y++)
            {
                Color pixelColor = bitmap.GetPixel(x, y);

                // Überprüfen, ob der Pixel undurchsichtig ist
                if (pixelColor.A > 0)
                {
                    // Neue Farbwerte berechnen, indem der Helligkeitsfaktor multipliziert wird
                    int newRed = (int)Math.Min(pixelColor.R * factor, 255);
                    int newGreen = (int)Math.Min(pixelColor.G * factor, 255);
                    int newBlue = (int)Math.Min(pixelColor.B * factor, 255);

                    // Neue Farbe setzen
                    adjustedBitmap.SetPixel(x, y, Color.FromArgb(pixelColor.A, newRed, newGreen, newBlue));
                }
                else
                {
                    // Transparenten Pixel unverändert setzen
                    adjustedBitmap.SetPixel(x, y, Color.FromArgb(0, 0, 0, 0));
                }
            }
        }

        return adjustedBitmap;
    }

    private void LoadCustomFont()
    {
        // Laden der benutzerdefinierten Schriftart aus den eingebetteten Ressourcen
        byte[] fontData = Properties.Resources.JosefinSans_Regular;
        IntPtr fontPtr = System.Runtime.InteropServices.Marshal.AllocCoTaskMem(fontData.Length);
        System.Runtime.InteropServices.Marshal.Copy(fontData, 0, fontPtr, fontData.Length);
        uint dummy = 0;
        fonts.AddMemoryFont(fontPtr, Properties.Resources.JosefinSans_Regular.Length);
        AddFontMemResourceEx(fontPtr, (uint)Properties.Resources.JosefinSans_Regular.Length, IntPtr.Zero, ref dummy);
        System.Runtime.InteropServices.Marshal.FreeCoTaskMem(fontPtr);

        // Erstelle die Font-Instanz mit der benutzerdefinierten Schriftart und -größe
        myFont = new Font(fonts.Families[0], 11.0F);
        myFontlblArmaSpieler = new Font(fonts.Families[0], 19.0F);

        // Setze die Schriftart für das Label
        lblPathDescription.Font = myFont;
        lblProfileDescription.Font = myFont;
        lblProfil.Font = myFont;
        lblMaxVram.Font = myFont;
        lblParams.Font = myFont;
        lblMaxVram2.Font = myFont;
        // Führe diese Zeile für jedes Label oder jedes Steuerelement aus, das die benutzerdefinierte Schriftart verwenden soll
        cbSplash.Font = myFont;
        cbWindow.Font = myFont;
        cbHyper.Font = myFont;
        cbIntro.Font = myFont;
        cbnologs.Font = myFont;

        playersOnlineLabel.Font = myFontlblArmaSpieler;
    }


    private void InitializeComponent()
    {
        components = new Container();
        ComponentResourceManager resources = new ComponentResourceManager(typeof(Mainframe));
        lblPathDescription = new Label();
        lblInstallationPath = new Label();
        btnChangePath = new Button();
        lblVersion = new Label();
        cbProfile = new ComboBox();
        lblProfil = new Label();
        cbSplash = new CheckBox();
        cbWindow = new CheckBox();
        lblMaxVram = new Label();
        nudVram = new NumericUpDown();
        txtParams = new TextBox();
        lblParams = new Label();
        lblProfileDescription = new Label();
        lblprofilePath = new Label();
        btnProfilePath = new Button();
        cbHyper = new CheckBox();
        cbIntro = new CheckBox();
        cbnologs = new CheckBox();
        fbdprofilePath = new FolderBrowserDialog();
        buttontexture = new Button();
        toolTip1 = new ToolTip(components);
        picInfo = new PictureBox();
        picInstagram = new PictureBox();
        picStatsImage = new PictureBox();
        picSteam = new PictureBox();
        picYoutube = new PictureBox();
        picClose = new PictureBox();
        picReload = new PictureBox();
        picMinimize = new PictureBox();
        picGetServerInfo = new PictureBox();
        toolTip2 = new ToolTip(components);
        toolTipLaunch = new ToolTip(components);
        picBanner = new PictureBox();
        picDiscord = new PictureBox();
        picUpdates = new PictureBox();
        picHomepage = new PictureBox();
        picRegeln = new PictureBox();
        picTeamspeak = new PictureBox();
        picSpielerOnline = new PictureBox();
        lblMaxVram2 = new Label();
        playersOnlineLabel = new Label();
        picPlay = new PictureBox();
        ((ISupportInitialize)nudVram).BeginInit();
        ((ISupportInitialize)picInfo).BeginInit();
        ((ISupportInitialize)picInstagram).BeginInit();
        ((ISupportInitialize)picStatsImage).BeginInit();
        ((ISupportInitialize)picSteam).BeginInit();
        ((ISupportInitialize)picYoutube).BeginInit();
        ((ISupportInitialize)picClose).BeginInit();
        ((ISupportInitialize)picReload).BeginInit();
        ((ISupportInitialize)picMinimize).BeginInit();
        ((ISupportInitialize)picGetServerInfo).BeginInit();
        ((ISupportInitialize)picBanner).BeginInit();
        ((ISupportInitialize)picDiscord).BeginInit();
        ((ISupportInitialize)picUpdates).BeginInit();
        ((ISupportInitialize)picHomepage).BeginInit();
        ((ISupportInitialize)picRegeln).BeginInit();
        ((ISupportInitialize)picTeamspeak).BeginInit();
        ((ISupportInitialize)picSpielerOnline).BeginInit();
        ((ISupportInitialize)picPlay).BeginInit();
        SuspendLayout();
        // 
        // lblPathDescription
        // 
        lblPathDescription.AutoSize = true;
        lblPathDescription.BackColor = Color.Transparent;
        lblPathDescription.Font = new Font("Bahnschrift SemiLight", 10F);
        lblPathDescription.ForeColor = Color.White;
        lblPathDescription.Location = new Point(14, 105);
        lblPathDescription.Margin = new Padding(4, 0, 4, 0);
        lblPathDescription.Name = "lblPathDescription";
        lblPathDescription.Size = new Size(131, 17);
        lblPathDescription.TabIndex = 8;
        lblPathDescription.Text = "Arma 3 Installation:";
        // 
        // lblInstallationPath
        // 
        lblInstallationPath.AutoSize = true;
        lblInstallationPath.BackColor = Color.Transparent;
        lblInstallationPath.Font = new Font("Bahnschrift SemiLight", 10F);
        lblInstallationPath.ForeColor = Color.FromArgb(34, 197, 94);
        lblInstallationPath.Location = new Point(225, 112);
        lblInstallationPath.Margin = new Padding(4, 0, 4, 0);
        lblInstallationPath.Name = "lblInstallationPath";
        lblInstallationPath.Size = new Size(77, 17);
        lblInstallationPath.TabIndex = 9;
        lblInstallationPath.Text = "UNKNOWN";
        // 
        // btnChangePath
        // 
        btnChangePath.BackColor = Color.FromArgb(39, 39, 42);
        btnChangePath.Cursor = Cursors.Hand;
        btnChangePath.FlatAppearance.BorderColor = Color.DimGray;
        btnChangePath.FlatAppearance.BorderSize = 2;
        btnChangePath.FlatStyle = FlatStyle.Popup;
        btnChangePath.Font = new Font("Bahnschrift SemiLight", 8.5F);
        btnChangePath.ForeColor = Color.White;
        btnChangePath.Location = new Point(706, 110);
        btnChangePath.Margin = new Padding(4, 3, 4, 3);
        btnChangePath.Name = "btnChangePath";
        btnChangePath.Size = new Size(128, 27);
        btnChangePath.TabIndex = 10;
        btnChangePath.Text = "Pfad ändern";
        btnChangePath.UseVisualStyleBackColor = false;
        btnChangePath.Click += btnChangePath_Click;
        // 
        // lblVersion
        // 
        lblVersion.AutoSize = true;
        lblVersion.BackColor = Color.Transparent;
        lblVersion.Font = new Font("Bahnschrift SemiLight", 8.7F);
        lblVersion.ForeColor = Color.White;
        lblVersion.Location = new Point(874, 558);
        lblVersion.Margin = new Padding(4, 0, 4, 0);
        lblVersion.Name = "lblVersion";
        lblVersion.Size = new Size(154, 14);
        lblVersion.TabIndex = 12;
        lblVersion.Text = "USA LIFE Launcher v1.0.0.0";
        // 
        // cbProfile
        // 
        cbProfile.BackColor = Color.FromArgb(39, 39, 42);
        cbProfile.Cursor = Cursors.Hand;
        cbProfile.DropDownStyle = ComboBoxStyle.DropDownList;
        cbProfile.FlatStyle = FlatStyle.Flat;
        cbProfile.Font = new Font("Bahnschrift SemiLight", 9F);
        cbProfile.ForeColor = SystemColors.Window;
        cbProfile.FormattingEnabled = true;
        cbProfile.Location = new Point(229, 195);
        cbProfile.Margin = new Padding(4, 3, 4, 3);
        cbProfile.Name = "cbProfile";
        cbProfile.Size = new Size(325, 22);
        cbProfile.TabIndex = 16;
        cbProfile.SelectedIndexChanged += cbProfile_SelectedIndexChanged;
        // 
        // lblProfil
        // 
        lblProfil.AutoSize = true;
        lblProfil.BackColor = Color.Transparent;
        lblProfil.Font = new Font("Bahnschrift SemiLight", 10F);
        lblProfil.ForeColor = Color.White;
        lblProfil.Location = new Point(14, 192);
        lblProfil.Margin = new Padding(4, 0, 4, 0);
        lblProfil.Name = "lblProfil";
        lblProfil.Size = new Size(46, 17);
        lblProfil.TabIndex = 17;
        lblProfil.Text = "Profil:";
        // 
        // cbSplash
        // 
        cbSplash.AutoSize = true;
        cbSplash.BackColor = Color.Transparent;
        cbSplash.Cursor = Cursors.Hand;
        cbSplash.Font = new Font("Bahnschrift SemiLight", 10F);
        cbSplash.ForeColor = Color.White;
        cbSplash.Location = new Point(682, 210);
        cbSplash.Margin = new Padding(4, 3, 4, 3);
        cbSplash.Name = "cbSplash";
        cbSplash.Size = new Size(145, 21);
        cbSplash.TabIndex = 18;
        cbSplash.Text = "kein Splashscreen";
        cbSplash.UseVisualStyleBackColor = false;
        cbSplash.CheckedChanged += cbSplash_CheckedChanged;
        cbSplash.Click += cbSplash_Click;
        // 
        // cbWindow
        // 
        cbWindow.AutoSize = true;
        cbWindow.BackColor = Color.Transparent;
        cbWindow.Cursor = Cursors.Hand;
        cbWindow.Font = new Font("Bahnschrift SemiLight", 10F);
        cbWindow.ForeColor = Color.White;
        cbWindow.Location = new Point(682, 240);
        cbWindow.Margin = new Padding(4, 3, 4, 3);
        cbWindow.Name = "cbWindow";
        cbWindow.Size = new Size(120, 21);
        cbWindow.TabIndex = 19;
        cbWindow.Text = "Fenstermodus";
        cbWindow.UseVisualStyleBackColor = false;
        cbWindow.Click += cbWindow_Click;
        // 
        // lblMaxVram
        // 
        lblMaxVram.AutoSize = true;
        lblMaxVram.BackColor = Color.Transparent;
        lblMaxVram.Font = new Font("Bahnschrift SemiLight", 10F);
        lblMaxVram.ForeColor = Color.White;
        lblMaxVram.Location = new Point(14, 226);
        lblMaxVram.Margin = new Padding(4, 0, 4, 0);
        lblMaxVram.Name = "lblMaxVram";
        lblMaxVram.Size = new Size(127, 17);
        lblMaxVram.TabIndex = 21;
        lblMaxVram.Text = "Max. VRAM (in MB)";
        // 
        // nudVram
        // 
        nudVram.BackColor = Color.FromArgb(39, 39, 42);
        nudVram.Font = new Font("Bahnschrift SemiLight", 9F);
        nudVram.ForeColor = Color.White;
        nudVram.Location = new Point(229, 231);
        nudVram.Margin = new Padding(4, 3, 4, 3);
        nudVram.Maximum = new decimal(new int[] { 100000, 0, 0, 0 });
        nudVram.Name = "nudVram";
        nudVram.Size = new Size(106, 22);
        nudVram.TabIndex = 22;
        nudVram.ValueChanged += nudVram_ValueChanged;
        // 
        // txtParams
        // 
        txtParams.BackColor = Color.FromArgb(39, 39, 42);
        txtParams.Font = new Font("Bahnschrift SemiLight", 10F);
        txtParams.ForeColor = SystemColors.Window;
        txtParams.Location = new Point(18, 298);
        txtParams.Margin = new Padding(4, 3, 4, 3);
        txtParams.Multiline = true;
        txtParams.Name = "txtParams";
        txtParams.Size = new Size(416, 81);
        txtParams.TabIndex = 23;
        txtParams.TextChanged += txtParams_TextChanged;
        // 
        // lblParams
        // 
        lblParams.AutoSize = true;
        lblParams.BackColor = Color.Transparent;
        lblParams.Font = new Font("Bahnschrift SemiLight", 10F);
        lblParams.ForeColor = Color.White;
        lblParams.Location = new Point(14, 268);
        lblParams.Margin = new Padding(4, 0, 4, 0);
        lblParams.Name = "lblParams";
        lblParams.Size = new Size(338, 17);
        lblParams.TabIndex = 24;
        lblParams.Text = "Andere Parameter (z.B. -cpuCount=<Anzahl Cores>)";
        // 
        // lblProfileDescription
        // 
        lblProfileDescription.AutoSize = true;
        lblProfileDescription.BackColor = Color.Transparent;
        lblProfileDescription.Font = new Font("Bahnschrift SemiLight", 10F);
        lblProfileDescription.ForeColor = Color.White;
        lblProfileDescription.Location = new Point(14, 143);
        lblProfileDescription.Margin = new Padding(4, 0, 4, 0);
        lblProfileDescription.Name = "lblProfileDescription";
        lblProfileDescription.Size = new Size(122, 17);
        lblProfileDescription.TabIndex = 25;
        lblProfileDescription.Text = "Arma 3 Profilpfad:";
        // 
        // lblprofilePath
        // 
        lblprofilePath.BackColor = Color.Transparent;
        lblprofilePath.Font = new Font("Bahnschrift SemiLight", 10F);
        lblprofilePath.ForeColor = Color.FromArgb(34, 197, 94);
        lblprofilePath.Location = new Point(225, 150);
        lblprofilePath.Margin = new Padding(4, 0, 4, 0);
        lblprofilePath.Name = "lblprofilePath";
        lblprofilePath.Size = new Size(474, 42);
        lblprofilePath.TabIndex = 27;
        lblprofilePath.Text = "- coming soon -";
        // 
        // btnProfilePath
        // 
        btnProfilePath.BackColor = Color.FromArgb(39, 39, 42);
        btnProfilePath.Cursor = Cursors.Hand;
        btnProfilePath.FlatStyle = FlatStyle.Popup;
        btnProfilePath.Font = new Font("Bahnschrift SemiLight", 8.5F);
        btnProfilePath.ForeColor = Color.White;
        btnProfilePath.Location = new Point(706, 148);
        btnProfilePath.Margin = new Padding(4, 3, 4, 3);
        btnProfilePath.Name = "btnProfilePath";
        btnProfilePath.Size = new Size(128, 27);
        btnProfilePath.TabIndex = 28;
        btnProfilePath.Text = "Pfad ändern";
        btnProfilePath.UseVisualStyleBackColor = false;
        btnProfilePath.Click += btnProfilePath_Click;
        // 
        // cbHyper
        // 
        cbHyper.AutoSize = true;
        cbHyper.BackColor = Color.Transparent;
        cbHyper.Cursor = Cursors.Hand;
        cbHyper.Font = new Font("Bahnschrift SemiLight", 10F);
        cbHyper.ForeColor = Color.White;
        cbHyper.Location = new Point(682, 270);
        cbHyper.Margin = new Padding(4, 3, 4, 3);
        cbHyper.Name = "cbHyper";
        cbHyper.Size = new Size(136, 21);
        cbHyper.TabIndex = 29;
        cbHyper.Text = "Hyper-Threading";
        cbHyper.UseVisualStyleBackColor = false;
        cbHyper.CheckedChanged += cbHyper_CheckedChanged;
        cbHyper.Click += cbHyper_Click;
        // 
        // cbIntro
        // 
        cbIntro.AutoSize = true;
        cbIntro.BackColor = Color.Transparent;
        cbIntro.Cursor = Cursors.Hand;
        cbIntro.Font = new Font("Bahnschrift SemiLight", 10F);
        cbIntro.ForeColor = Color.White;
        cbIntro.Location = new Point(682, 300);
        cbIntro.Margin = new Padding(4, 3, 4, 3);
        cbIntro.Name = "cbIntro";
        cbIntro.Size = new Size(149, 21);
        cbIntro.TabIndex = 30;
        cbIntro.Text = "Intro überspringen";
        cbIntro.UseVisualStyleBackColor = false;
        cbIntro.Click += cbIntro_Click;
        // 
        // cbnologs
        // 
        cbnologs.AutoSize = true;
        cbnologs.BackColor = Color.Transparent;
        cbnologs.Cursor = Cursors.Hand;
        cbnologs.Font = new Font("Bahnschrift SemiLight", 10F);
        cbnologs.ForeColor = Color.White;
        cbnologs.Location = new Point(682, 329);
        cbnologs.Margin = new Padding(4, 3, 4, 3);
        cbnologs.Name = "cbnologs";
        cbnologs.Size = new Size(97, 21);
        cbnologs.TabIndex = 31;
        cbnologs.Text = "keine Logs";
        cbnologs.UseVisualStyleBackColor = false;
        cbnologs.Click += cbnologs_Click;
        // 
        // buttontexture
        // 
        buttontexture.Location = new Point(334, 398);
        buttontexture.Margin = new Padding(4, 3, 4, 3);
        buttontexture.Name = "buttontexture";
        buttontexture.Size = new Size(51, 27);
        buttontexture.TabIndex = 38;
        buttontexture.Text = "Textureupdate";
        buttontexture.UseVisualStyleBackColor = true;
        buttontexture.Visible = false;
        buttontexture.Click += buttontexture_Click;
        // 
        // picInfo
        // 
        picInfo.BackColor = Color.Transparent;
        picInfo.Cursor = Cursors.Hand;
        picInfo.Image = Properties.Resources.picInfo_Image;
        picInfo.Location = new Point(955, 14);
        picInfo.Margin = new Padding(4, 3, 4, 3);
        picInfo.Name = "picInfo";
        picInfo.Size = new Size(19, 18);
        picInfo.SizeMode = PictureBoxSizeMode.Zoom;
        picInfo.TabIndex = 48;
        picInfo.TabStop = false;
        toolTip1.SetToolTip(picInfo, "Info´s");
        picInfo.Click += picInfo_Click;
        picInfo.MouseEnter += MouseHoverEnter;
        picInfo.MouseLeave += MouseHoverLeave;
        // 
        // picInstagram
        // 
        picInstagram.BackColor = Color.Transparent;
        picInstagram.Cursor = Cursors.Hand;
        picInstagram.Image = Properties.Resources.picInstagramGrey_Image;
        picInstagram.Location = new Point(63, 450);
        picInstagram.Margin = new Padding(4, 3, 4, 3);
        picInstagram.Name = "picInstagram";
        picInstagram.Size = new Size(38, 38);
        picInstagram.SizeMode = PictureBoxSizeMode.Zoom;
        picInstagram.TabIndex = 49;
        picInstagram.TabStop = false;
        toolTip1.SetToolTip(picInstagram, "Folge uns auf Instagram!");
        picInstagram.Click += picInstagram_Click;
        picInstagram.MouseEnter += MouseHoverEnter;
        picInstagram.MouseLeave += MouseHoverLeave;
        // 
        // picStatsImage
        // 
        picStatsImage.BackColor = Color.FromArgb(39, 39, 42);
        picStatsImage.Cursor = Cursors.Hand;
        picStatsImage.Image = Properties.Resources.picStatistic_Image;
        picStatsImage.Location = new Point(24, 507);
        picStatsImage.Margin = new Padding(4, 3, 4, 3);
        picStatsImage.Name = "picStatsImage";
        picStatsImage.Size = new Size(35, 35);
        picStatsImage.TabIndex = 47;
        picStatsImage.TabStop = false;
        toolTip1.SetToolTip(picStatsImage, "Server Statistik");
        picStatsImage.Click += picStatsImage_Click;
        // 
        // picSteam
        // 
        picSteam.BackColor = Color.Transparent;
        picSteam.Cursor = Cursors.Hand;
        picSteam.Image = Properties.Resources.picSteamGrey_Image;
        picSteam.Location = new Point(18, 450);
        picSteam.Margin = new Padding(4, 3, 4, 3);
        picSteam.Name = "picSteam";
        picSteam.Size = new Size(38, 38);
        picSteam.SizeMode = PictureBoxSizeMode.Zoom;
        picSteam.TabIndex = 57;
        picSteam.TabStop = false;
        toolTip1.SetToolTip(picSteam, "Trete unserer Steam Gruppe bei!");
        picSteam.Click += picSteam_Click;
        picSteam.MouseEnter += MouseHoverEnter;
        picSteam.MouseLeave += MouseHoverLeave;
        // 
        // picYoutube
        // 
        picYoutube.BackColor = Color.Transparent;
        picYoutube.Cursor = Cursors.Hand;
        picYoutube.Image = Properties.Resources.picYoutubeGrey_Image;
        picYoutube.Location = new Point(108, 450);
        picYoutube.Margin = new Padding(4, 3, 4, 3);
        picYoutube.Name = "picYoutube";
        picYoutube.Size = new Size(38, 38);
        picYoutube.SizeMode = PictureBoxSizeMode.Zoom;
        picYoutube.TabIndex = 58;
        picYoutube.TabStop = false;
        toolTip1.SetToolTip(picYoutube, "Folge uns auf YouTube!");
        picYoutube.Click += picYoutube_Click;
        picYoutube.MouseEnter += MouseHoverEnter;
        picYoutube.MouseLeave += MouseHoverLeave;
        // 
        // picClose
        // 
        picClose.BackColor = Color.Transparent;
        picClose.Cursor = Cursors.Hand;
        picClose.Image = Properties.Resources.picCloseWhite_Image;
        picClose.Location = new Point(1016, 14);
        picClose.Margin = new Padding(4, 3, 4, 3);
        picClose.Name = "picClose";
        picClose.Size = new Size(21, 21);
        picClose.SizeMode = PictureBoxSizeMode.Zoom;
        picClose.TabIndex = 59;
        picClose.TabStop = false;
        toolTip1.SetToolTip(picClose, "Close");
        picClose.Click += picClose_Click;
        picClose.MouseEnter += MouseHoverEnter;
        picClose.MouseLeave += MouseHoverLeave;
        // 
        // picReload
        // 
        picReload.BackColor = Color.Transparent;
        picReload.Cursor = Cursors.Hand;
        picReload.Image = Properties.Resources.picReload_Image;
        picReload.Location = new Point(582, 460);
        picReload.Margin = new Padding(1);
        picReload.Name = "picReload";
        picReload.Size = new Size(93, 92);
        picReload.SizeMode = PictureBoxSizeMode.Zoom;
        picReload.TabIndex = 62;
        picReload.TabStop = false;
        toolTip1.SetToolTip(picReload, "Nach Mod- und Missionsupdates suchen");
        picReload.Click += picReload_Click;
        picReload.MouseEnter += MouseHoverEnter;
        picReload.MouseLeave += MouseHoverLeave;
        // 
        // picMinimize
        // 
        picMinimize.BackColor = Color.Transparent;
        picMinimize.Cursor = Cursors.Hand;
        picMinimize.Image = Properties.Resources.picMinimize_Image;
        picMinimize.Location = new Point(985, 9);
        picMinimize.Margin = new Padding(4, 3, 4, 3);
        picMinimize.Name = "picMinimize";
        picMinimize.Size = new Size(18, 18);
        picMinimize.SizeMode = PictureBoxSizeMode.Zoom;
        picMinimize.TabIndex = 63;
        picMinimize.TabStop = false;
        toolTip1.SetToolTip(picMinimize, "Minimize");
        picMinimize.Click += picMinimize_Click;
        picMinimize.MouseEnter += MouseHoverEnter;
        picMinimize.MouseLeave += MouseHoverLeave;
        // 
        // picGetServerInfo
        // 
        picGetServerInfo.BackColor = Color.Transparent;
        picGetServerInfo.Cursor = Cursors.Hand;
        picGetServerInfo.Image = Properties.Resources.picReload_Image;
        picGetServerInfo.Location = new Point(344, 495);
        picGetServerInfo.Margin = new Padding(4, 3, 4, 3);
        picGetServerInfo.Name = "picGetServerInfo";
        picGetServerInfo.Size = new Size(58, 58);
        picGetServerInfo.SizeMode = PictureBoxSizeMode.Zoom;
        picGetServerInfo.TabIndex = 68;
        picGetServerInfo.TabStop = false;
        toolTip1.SetToolTip(picGetServerInfo, "Serverinfos aktualisieren");
        picGetServerInfo.Click += picGetServerInfo_Click;
        picGetServerInfo.MouseEnter += MouseHoverEnter;
        picGetServerInfo.MouseLeave += MouseHoverLeave;
        // 
        // toolTipLaunch
        // 
        toolTipLaunch.BackColor = Color.FromArgb(64, 64, 64);
        toolTipLaunch.ForeColor = Color.Black;
        // 
        // picBanner
        // 
        picBanner.BackColor = Color.FromArgb(45, 47, 49);
        picBanner.Image = (Image)resources.GetObject("picBanner.Image");
        picBanner.Location = new Point(0, -1);
        picBanner.Margin = new Padding(4, 3, 4, 3);
        picBanner.Name = "picBanner";
        picBanner.Size = new Size(1051, 69);
        picBanner.SizeMode = PictureBoxSizeMode.StretchImage;
        picBanner.TabIndex = 3;
        picBanner.TabStop = false;
        picBanner.MouseDown += picBanner_MouseDown;
        // 
        // picDiscord
        // 
        picDiscord.BackColor = Color.Transparent;
        picDiscord.Cursor = Cursors.Hand;
        picDiscord.Image = Properties.Resources.picDiscord_Image;
        picDiscord.Location = new Point(874, 103);
        picDiscord.Margin = new Padding(4, 3, 4, 3);
        picDiscord.Name = "picDiscord";
        picDiscord.Size = new Size(156, 46);
        picDiscord.SizeMode = PictureBoxSizeMode.Zoom;
        picDiscord.TabIndex = 51;
        picDiscord.TabStop = false;
        picDiscord.Click += picDiscord_Click;
        picDiscord.MouseEnter += MouseHoverEnter;
        picDiscord.MouseLeave += MouseHoverLeave;
        // 
        // picUpdates
        // 
        picUpdates.BackColor = Color.Transparent;
        picUpdates.Cursor = Cursors.Hand;
        picUpdates.Image = Properties.Resources.picUpdates_Image;
        picUpdates.Location = new Point(874, 315);
        picUpdates.Margin = new Padding(4, 3, 4, 3);
        picUpdates.Name = "picUpdates";
        picUpdates.Size = new Size(156, 46);
        picUpdates.SizeMode = PictureBoxSizeMode.Zoom;
        picUpdates.TabIndex = 52;
        picUpdates.TabStop = false;
        picUpdates.Click += serverUpdatesButton_Click;
        picUpdates.MouseEnter += MouseHoverEnter;
        picUpdates.MouseLeave += MouseHoverLeave;
        // 
        // picHomepage
        // 
        picHomepage.BackColor = Color.Transparent;
        picHomepage.Cursor = Cursors.Hand;
        picHomepage.Image = Properties.Resources.picHomepage_Image;
        picHomepage.Location = new Point(874, 262);
        picHomepage.Margin = new Padding(4, 3, 4, 3);
        picHomepage.Name = "picHomepage";
        picHomepage.Size = new Size(156, 46);
        picHomepage.SizeMode = PictureBoxSizeMode.Zoom;
        picHomepage.TabIndex = 53;
        picHomepage.TabStop = false;
        picHomepage.Click += picHomepage_Click;
        picHomepage.MouseEnter += MouseHoverEnter;
        picHomepage.MouseLeave += MouseHoverLeave;
        // 
        // picRegeln
        // 
        picRegeln.BackColor = Color.Transparent;
        picRegeln.Cursor = Cursors.Hand;
        picRegeln.Image = Properties.Resources.picRegeln_Image;
        picRegeln.Location = new Point(874, 209);
        picRegeln.Margin = new Padding(4, 3, 4, 3);
        picRegeln.Name = "picRegeln";
        picRegeln.Size = new Size(156, 46);
        picRegeln.SizeMode = PictureBoxSizeMode.Zoom;
        picRegeln.TabIndex = 54;
        picRegeln.TabStop = false;
        picRegeln.Click += picRegeln_Click;
        picRegeln.MouseEnter += MouseHoverEnter;
        picRegeln.MouseLeave += MouseHoverLeave;
        // 
        // picTeamspeak
        // 
        picTeamspeak.BackColor = Color.Transparent;
        picTeamspeak.Cursor = Cursors.Hand;
        picTeamspeak.Image = Properties.Resources.picTeamspeak_Image;
        picTeamspeak.Location = new Point(874, 156);
        picTeamspeak.Margin = new Padding(4, 3, 4, 3);
        picTeamspeak.Name = "picTeamspeak";
        picTeamspeak.Size = new Size(156, 46);
        picTeamspeak.SizeMode = PictureBoxSizeMode.Zoom;
        picTeamspeak.TabIndex = 55;
        picTeamspeak.TabStop = false;
        picTeamspeak.Click += picTeamspeak_Click;
        picTeamspeak.MouseEnter += MouseHoverEnter;
        picTeamspeak.MouseLeave += MouseHoverLeave;
        // 
        // picSpielerOnline
        // 
        picSpielerOnline.BackColor = Color.Transparent;
        picSpielerOnline.Image = Properties.Resources.picPlayersonline_Image;
        picSpielerOnline.Location = new Point(18, 495);
        picSpielerOnline.Margin = new Padding(4, 3, 4, 3);
        picSpielerOnline.Name = "picSpielerOnline";
        picSpielerOnline.Size = new Size(385, 58);
        picSpielerOnline.SizeMode = PictureBoxSizeMode.Zoom;
        picSpielerOnline.TabIndex = 56;
        picSpielerOnline.TabStop = false;
        // 
        // lblMaxVram2
        // 
        lblMaxVram2.AutoSize = true;
        lblMaxVram2.BackColor = Color.Transparent;
        lblMaxVram2.Font = new Font("Bahnschrift SemiLight", 10F);
        lblMaxVram2.ForeColor = Color.White;
        lblMaxVram2.Location = new Point(342, 228);
        lblMaxVram2.Margin = new Padding(4, 0, 4, 0);
        lblMaxVram2.Name = "lblMaxVram2";
        lblMaxVram2.Size = new Size(90, 17);
        lblMaxVram2.TabIndex = 61;
        lblMaxVram2.Text = "➜ 0 = default";
        // 
        // playersOnlineLabel
        // 
        playersOnlineLabel.AutoSize = true;
        playersOnlineLabel.BackColor = Color.FromArgb(39, 39, 42);
        playersOnlineLabel.Font = new Font("Bahnschrift Light", 18F, FontStyle.Regular, GraphicsUnit.Point, 0);
        playersOnlineLabel.ForeColor = Color.White;
        playersOnlineLabel.Location = new Point(57, 498);
        playersOnlineLabel.Margin = new Padding(4, 0, 4, 0);
        playersOnlineLabel.Name = "playersOnlineLabel";
        playersOnlineLabel.Size = new Size(201, 29);
        playersOnlineLabel.TabIndex = 67;
        playersOnlineLabel.Text = "Spieler online: ?/?";
        // 
        // picPlay
        // 
        picPlay.BackColor = Color.Transparent;
        picPlay.Cursor = Cursors.Hand;
        picPlay.Image = Properties.Resources.picLaunch_Image;
        picPlay.Location = new Point(691, 460);
        picPlay.Margin = new Padding(4, 3, 4, 3);
        picPlay.Name = "picPlay";
        picPlay.Size = new Size(340, 92);
        picPlay.SizeMode = PictureBoxSizeMode.Zoom;
        picPlay.TabIndex = 69;
        picPlay.TabStop = false;
        picPlay.Click += btnLaunch_Click;
        picPlay.MouseEnter += MouseHoverEnter;
        picPlay.MouseLeave += MouseHoverLeave;
        // 
        // Mainframe
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        BackgroundImage = (Image)resources.GetObject("$this.BackgroundImage");
        BackgroundImageLayout = ImageLayout.Stretch;
        ClientSize = new Size(1050, 584);
        ControlBox = false;
        Controls.Add(picPlay);
        Controls.Add(picGetServerInfo);
        Controls.Add(playersOnlineLabel);
        Controls.Add(picMinimize);
        Controls.Add(picReload);
        Controls.Add(lblMaxVram2);
        Controls.Add(picClose);
        Controls.Add(picYoutube);
        Controls.Add(picSteam);
        Controls.Add(picTeamspeak);
        Controls.Add(picRegeln);
        Controls.Add(picHomepage);
        Controls.Add(picUpdates);
        Controls.Add(picDiscord);
        Controls.Add(picInstagram);
        Controls.Add(picInfo);
        Controls.Add(picStatsImage);
        Controls.Add(buttontexture);
        Controls.Add(cbnologs);
        Controls.Add(cbIntro);
        Controls.Add(cbHyper);
        Controls.Add(btnProfilePath);
        Controls.Add(lblprofilePath);
        Controls.Add(lblProfileDescription);
        Controls.Add(lblParams);
        Controls.Add(txtParams);
        Controls.Add(nudVram);
        Controls.Add(lblMaxVram);
        Controls.Add(cbWindow);
        Controls.Add(cbSplash);
        Controls.Add(lblProfil);
        Controls.Add(cbProfile);
        Controls.Add(lblVersion);
        Controls.Add(btnChangePath);
        Controls.Add(lblInstallationPath);
        Controls.Add(lblPathDescription);
        Controls.Add(picBanner);
        Controls.Add(picSpielerOnline);
        DoubleBuffered = true;
        FormBorderStyle = FormBorderStyle.None;
        Icon = (Icon)resources.GetObject("$this.Icon");
        Margin = new Padding(4, 3, 4, 3);
        MaximizeBox = false;
        MaximumSize = new Size(1050, 584);
        MinimizeBox = false;
        MinimumSize = new Size(1050, 584);
        Name = "Mainframe";
        Load += MainFrame_Load;
        ((ISupportInitialize)nudVram).EndInit();
        ((ISupportInitialize)picInfo).EndInit();
        ((ISupportInitialize)picInstagram).EndInit();
        ((ISupportInitialize)picStatsImage).EndInit();
        ((ISupportInitialize)picSteam).EndInit();
        ((ISupportInitialize)picYoutube).EndInit();
        ((ISupportInitialize)picClose).EndInit();
        ((ISupportInitialize)picReload).EndInit();
        ((ISupportInitialize)picMinimize).EndInit();
        ((ISupportInitialize)picGetServerInfo).EndInit();
        ((ISupportInitialize)picBanner).EndInit();
        ((ISupportInitialize)picDiscord).EndInit();
        ((ISupportInitialize)picUpdates).EndInit();
        ((ISupportInitialize)picHomepage).EndInit();
        ((ISupportInitialize)picRegeln).EndInit();
        ((ISupportInitialize)picTeamspeak).EndInit();
        ((ISupportInitialize)picSpielerOnline).EndInit();
        ((ISupportInitialize)picPlay).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }
}