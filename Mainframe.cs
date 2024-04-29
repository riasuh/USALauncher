using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using USALauncher.Resources;

using System.Data;
using System.Text;
using System.Threading.Tasks;
using System.Drawing.Text;
using SteamQuery;

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

    public ArmA3ServerInfo info = new ArmA3ServerInfo(Dns.GetHostAddresses("s.usa-life.net").FirstOrDefault().ToString(), 2303);

    private System.Threading.Timer timer;

    private IContainer components;

    private Label lblPathDescription;

    private Label lblInstallationPath;

    private Button btnChangePath;

    private WebBrowser wbChangelog;

    private Label lblVersion;

    private Label lblArmaSpieler;

    private System.Windows.Forms.Timer tmrUpdateStats;

    private PictureBoxOpacity picLaunch;

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

    private CheckBox cbRadioMod;

    private Label label5;

    private Label label6;

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

    /*private void WriteDownToLog(string message)
	{
		if (!Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + "Logs"))
		{
			Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "Logs");
		}
		StreamWriter streamWriter = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "Logs\\ErrorLog-" + DateTime.Now.ToString("dd-MM-yyyy"), append: true);
		streamWriter.WriteLine(message);
		streamWriter.Close();
	}*/

    private static Mutex logMutex = new Mutex(); // Globale Mutex-Variable

    private void WriteDownToLog(string message)
    {
        // Versuche, den Mutex zu sperren
        if (logMutex.WaitOne())
        {
            try
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
        lblInstallationPath.Text = armaPath;
        cbSplash.Checked = Settings.Default.noSplash;
        cbWindow.Checked = Settings.Default.windowed;
        nudVram.Value = Settings.Default.maxVram;
        txtParams.Text = Settings.Default.customParams;
        cbHyper.Checked = Settings.Default.enableHT;
        cbIntro.Checked = Settings.Default.skipIntro;
        cbnologs.Checked = Settings.Default.noLogs;
        /* old Arma3ServerInfo code 
          new Thread((ThreadStart)delegate
        {
            updateStats(null, null);
        }).Start();
        tmrUpdateStats.Start();
        */
        wbChangelog.DocumentTitleChanged += AdaptChangelog;

        
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
        Process.Start("t" +
            "s3server://ts.usa-life.net");
    }

    private void picRegeln_Click(object sender, EventArgs e)
    {
        Process.Start("https://usa-life.net/regeln");
    }

    private void picHomepage_Click(object sender, EventArgs e)
    {
        Process.Start("https://usa-life.net/");
    }

    private void picForum_Click(object sender, EventArgs e)
    {
        Process.Start("https://forum.usa-life.net/");
    }

    private void serverUpdatesButton_Click(object sender, EventArgs e)
    {
        Process.Start("https://discord.gg/usaliferpg");
    }

    private void picSteam_Click(object sender, EventArgs e)
    {
        Process.Start("https://steamcommunity.com/groups/usaliferpg");
    }

    private void facebookBtn_Click(object sender, EventArgs e)
    {
        Process.Start("https://www.facebook.com/usaliferpg/");
    }

    private void btnChangePath_Click(object sender, EventArgs e)
    {
        new PathSelection().ShowDialog();
        armaPath = Settings.Default["armaPath"].ToString();
        lblInstallationPath.Text = armaPath;
    }

    private void updateStats(object sender, EventArgs e)
    {
        info.Update();

        this.InvokeEx(delegate (Mainframe f)
        {
            if (info.ServerInfo == null)
            {
                f.lblArmaSpieler.Text = "Spieler Online: 0/0";
            }
            else
            {
                f.lblArmaSpieler.Text = "Spieler Online: " + info.ServerInfo.NumPlayers + "/" + info.ServerInfo.MaxPlayers;
            }
        });
    }




    private void btnLaunch_Click(object sender, EventArgs e)
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
            ProcessStartInfo processStartInfo2 = processStartInfo;
            processStartInfo2.Arguments = string.Concat(processStartInfo2.Arguments, " \"-name=", userProfile, "\"");
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
            processStartInfo.Arguments = processStartInfo.Arguments + " -maxVRAM=" + nudVram.Value;
        }
        if (txtParams.Text.Length != 0)
        {
            processStartInfo.Arguments = processStartInfo.Arguments + " " + txtParams.Text;
        }
        Process.Start(processStartInfo);
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
            f.picLaunch.Enabled = false;
        });
        this.InvokeEx(delegate (Mainframe f)
        {
            f.picLaunch.Text = "Starte ArmA...";
        });
        timer = new System.Threading.Timer(delegate
        {
            this.InvokeEx(delegate (Mainframe f)
            {
                f.picLaunch.Enabled = true;
            });
            this.InvokeEx(delegate (Mainframe f)
            {
                f.picLaunch.Text = "USA LIFE Spielen";
            });
            timer.Dispose();
        }, null, 10000, -1);
    }

    private void MouseHoverEnter(object sender, EventArgs e)
    {
        if (sender is PictureBoxOpacity)
        {
            ((PictureBoxOpacity)sender).Opacity = 0.9f;
        }
        else if (sender is PictureBox)
        {
            PictureBox pictureBox = (PictureBox)sender;

            // Überprüfe, ob die PictureBox die spezielle PictureBox ist, für die der Hover-Effekt implementiert werden soll (picClose)
            if (pictureBox == picClose)
            {
                // Setze das rote Bild für den Close-Button
                pictureBox.Image = Properties.Resources.picCloseRed_Image;
            } else if (pictureBox == picMinimize)
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
        if (sender is PictureBoxOpacity)
        {
            ((PictureBoxOpacity)sender).Opacity = 1f;
        }
        else if (sender is PictureBox)
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

    private void AdaptChangelog(object sender, CancelEventArgs e)
    {
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
        Process.Start("https://www.instagram.com/usaliferpg/");
    }
    private void picYoutube_Click(object sender, EventArgs e)
    {
        Process.Start("https://www.youtube.com/@usaliferpg");
    }
    private void picStatsImage_Click(object sender, EventArgs e)
    {
        Process.Start("https://www.battlemetrics.com/servers/arma3");
    }
    private void picDiscord_Click(object sender, EventArgs e)
    {
        Process.Start("https://discord.gg/usaliferpg");
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
        myFontlblArmaSpieler = new Font(fonts.Families[0], 17.0F);

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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Mainframe));
            this.lblPathDescription = new System.Windows.Forms.Label();
            this.lblInstallationPath = new System.Windows.Forms.Label();
            this.btnChangePath = new System.Windows.Forms.Button();
            this.wbChangelog = new System.Windows.Forms.WebBrowser();
            this.lblVersion = new System.Windows.Forms.Label();
            this.lblArmaSpieler = new System.Windows.Forms.Label();
            this.tmrUpdateStats = new System.Windows.Forms.Timer(this.components);
            this.cbProfile = new System.Windows.Forms.ComboBox();
            this.lblProfil = new System.Windows.Forms.Label();
            this.cbSplash = new System.Windows.Forms.CheckBox();
            this.cbWindow = new System.Windows.Forms.CheckBox();
            this.lblMaxVram = new System.Windows.Forms.Label();
            this.nudVram = new System.Windows.Forms.NumericUpDown();
            this.txtParams = new System.Windows.Forms.TextBox();
            this.lblParams = new System.Windows.Forms.Label();
            this.lblProfileDescription = new System.Windows.Forms.Label();
            this.lblprofilePath = new System.Windows.Forms.Label();
            this.btnProfilePath = new System.Windows.Forms.Button();
            this.cbHyper = new System.Windows.Forms.CheckBox();
            this.cbIntro = new System.Windows.Forms.CheckBox();
            this.cbnologs = new System.Windows.Forms.CheckBox();
            this.fbdprofilePath = new System.Windows.Forms.FolderBrowserDialog();
            this.buttontexture = new System.Windows.Forms.Button();
            this.cbRadioMod = new System.Windows.Forms.CheckBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.picInfo = new System.Windows.Forms.PictureBox();
            this.picInstagram = new System.Windows.Forms.PictureBox();
            this.picStatsImage = new System.Windows.Forms.PictureBox();
            this.picSteam = new System.Windows.Forms.PictureBox();
            this.picYoutube = new System.Windows.Forms.PictureBox();
            this.picClose = new System.Windows.Forms.PictureBox();
            this.picReload = new System.Windows.Forms.PictureBox();
            this.picMinimize = new System.Windows.Forms.PictureBox();
            this.picGetServerInfo = new System.Windows.Forms.PictureBox();
            this.toolTip2 = new System.Windows.Forms.ToolTip(this.components);
            this.toolTipLaunch = new System.Windows.Forms.ToolTip(this.components);
            this.picBanner = new System.Windows.Forms.PictureBox();
            this.picDiscord = new System.Windows.Forms.PictureBox();
            this.picUpdates = new System.Windows.Forms.PictureBox();
            this.picHomepage = new System.Windows.Forms.PictureBox();
            this.picRegeln = new System.Windows.Forms.PictureBox();
            this.picTeamspeak = new System.Windows.Forms.PictureBox();
            this.picSpielerOnline = new System.Windows.Forms.PictureBox();
            this.lblMaxVram2 = new System.Windows.Forms.Label();
            this.playersOnlineLabel = new System.Windows.Forms.Label();
            this.picLaunch = new USALauncher.PictureBoxOpacity();
            ((System.ComponentModel.ISupportInitialize)(this.nudVram)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picInfo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picInstagram)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picStatsImage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picSteam)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picYoutube)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picClose)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picReload)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picMinimize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picGetServerInfo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBanner)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picDiscord)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picUpdates)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picHomepage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picRegeln)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picTeamspeak)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picSpielerOnline)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picLaunch)).BeginInit();
            this.SuspendLayout();
            // 
            // lblPathDescription
            // 
            this.lblPathDescription.AutoSize = true;
            this.lblPathDescription.BackColor = System.Drawing.Color.Transparent;
            this.lblPathDescription.Font = new System.Drawing.Font("Bahnschrift SemiLight", 10F);
            this.lblPathDescription.ForeColor = System.Drawing.Color.White;
            this.lblPathDescription.Location = new System.Drawing.Point(12, 91);
            this.lblPathDescription.Name = "lblPathDescription";
            this.lblPathDescription.Size = new System.Drawing.Size(131, 17);
            this.lblPathDescription.TabIndex = 8;
            this.lblPathDescription.Text = "Arma 3 Installation:";
            // 
            // lblInstallationPath
            // 
            this.lblInstallationPath.AutoSize = true;
            this.lblInstallationPath.BackColor = System.Drawing.Color.Transparent;
            this.lblInstallationPath.Font = new System.Drawing.Font("Bahnschrift SemiLight", 10F);
            this.lblInstallationPath.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(197)))), ((int)(((byte)(94)))));
            this.lblInstallationPath.Location = new System.Drawing.Point(193, 97);
            this.lblInstallationPath.Name = "lblInstallationPath";
            this.lblInstallationPath.Size = new System.Drawing.Size(77, 17);
            this.lblInstallationPath.TabIndex = 9;
            this.lblInstallationPath.Text = "UNKNOWN";
            // 
            // btnChangePath
            // 
            this.btnChangePath.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(39)))), ((int)(((byte)(39)))), ((int)(((byte)(42)))));
            this.btnChangePath.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnChangePath.FlatAppearance.BorderColor = System.Drawing.Color.DimGray;
            this.btnChangePath.FlatAppearance.BorderSize = 2;
            this.btnChangePath.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnChangePath.Font = new System.Drawing.Font("Bahnschrift SemiLight", 8.5F);
            this.btnChangePath.ForeColor = System.Drawing.Color.White;
            this.btnChangePath.Location = new System.Drawing.Point(605, 95);
            this.btnChangePath.Name = "btnChangePath";
            this.btnChangePath.Size = new System.Drawing.Size(110, 23);
            this.btnChangePath.TabIndex = 10;
            this.btnChangePath.Text = "Pfad ändern";
            this.btnChangePath.UseVisualStyleBackColor = false;
            this.btnChangePath.Click += new System.EventHandler(this.btnChangePath_Click);
            // 
            // wbChangelog
            // 
            this.wbChangelog.Location = new System.Drawing.Point(286, 374);
            this.wbChangelog.MinimumSize = new System.Drawing.Size(20, 20);
            this.wbChangelog.Name = "wbChangelog";
            this.wbChangelog.Size = new System.Drawing.Size(44, 20);
            this.wbChangelog.TabIndex = 11;
            this.wbChangelog.Url = new System.Uri("https://forum.usa-life.net/forum/index.php?board/9-server-updates/", System.UriKind.Absolute);
            this.wbChangelog.Visible = false;
            this.wbChangelog.NewWindow += new System.ComponentModel.CancelEventHandler(this.AdaptChangelog);
            // 
            // lblVersion
            // 
            this.lblVersion.AutoSize = true;
            this.lblVersion.BackColor = System.Drawing.Color.Transparent;
            this.lblVersion.Font = new System.Drawing.Font("Bahnschrift SemiLight", 8.7F);
            this.lblVersion.ForeColor = System.Drawing.Color.White;
            this.lblVersion.Location = new System.Drawing.Point(733, 484);
            this.lblVersion.Name = "lblVersion";
            this.lblVersion.Size = new System.Drawing.Size(153, 14);
            this.lblVersion.TabIndex = 12;
            this.lblVersion.Text = "USA LIFE Launcher v1.3.0.0";
            // 
            // lblArmaSpieler
            // 
            this.lblArmaSpieler.AutoSize = true;
            this.lblArmaSpieler.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(39)))), ((int)(((byte)(39)))), ((int)(((byte)(42)))));
            this.lblArmaSpieler.Font = new System.Drawing.Font("Bahnschrift Light", 11F);
            this.lblArmaSpieler.ForeColor = System.Drawing.Color.White;
            this.lblArmaSpieler.Location = new System.Drawing.Point(591, 332);
            this.lblArmaSpieler.Name = "lblArmaSpieler";
            this.lblArmaSpieler.Size = new System.Drawing.Size(203, 18);
            this.lblArmaSpieler.TabIndex = 13;
            this.lblArmaSpieler.Text = "Spieler Online:    /? (obsolete)";
            this.lblArmaSpieler.Visible = false;
            // 
            // cbProfile
            // 
            this.cbProfile.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(39)))), ((int)(((byte)(39)))), ((int)(((byte)(42)))));
            this.cbProfile.Cursor = System.Windows.Forms.Cursors.Hand;
            this.cbProfile.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbProfile.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbProfile.Font = new System.Drawing.Font("Bahnschrift SemiLight", 9F);
            this.cbProfile.ForeColor = System.Drawing.SystemColors.Window;
            this.cbProfile.FormattingEnabled = true;
            this.cbProfile.Location = new System.Drawing.Point(196, 169);
            this.cbProfile.Name = "cbProfile";
            this.cbProfile.Size = new System.Drawing.Size(279, 22);
            this.cbProfile.TabIndex = 16;
            this.cbProfile.SelectedIndexChanged += new System.EventHandler(this.cbProfile_SelectedIndexChanged);
            // 
            // lblProfil
            // 
            this.lblProfil.AutoSize = true;
            this.lblProfil.BackColor = System.Drawing.Color.Transparent;
            this.lblProfil.Font = new System.Drawing.Font("Bahnschrift SemiLight", 10F);
            this.lblProfil.ForeColor = System.Drawing.Color.White;
            this.lblProfil.Location = new System.Drawing.Point(12, 166);
            this.lblProfil.Name = "lblProfil";
            this.lblProfil.Size = new System.Drawing.Size(46, 17);
            this.lblProfil.TabIndex = 17;
            this.lblProfil.Text = "Profil:";
            // 
            // cbSplash
            // 
            this.cbSplash.AutoSize = true;
            this.cbSplash.BackColor = System.Drawing.Color.Transparent;
            this.cbSplash.Cursor = System.Windows.Forms.Cursors.Hand;
            this.cbSplash.Font = new System.Drawing.Font("Bahnschrift SemiLight", 10F);
            this.cbSplash.ForeColor = System.Drawing.Color.White;
            this.cbSplash.Location = new System.Drawing.Point(585, 182);
            this.cbSplash.Name = "cbSplash";
            this.cbSplash.Size = new System.Drawing.Size(145, 21);
            this.cbSplash.TabIndex = 18;
            this.cbSplash.Text = "kein Splashscreen";
            this.cbSplash.UseVisualStyleBackColor = false;
            this.cbSplash.CheckedChanged += new System.EventHandler(this.cbSplash_CheckedChanged);
            this.cbSplash.Click += new System.EventHandler(this.cbSplash_Click);
            // 
            // cbWindow
            // 
            this.cbWindow.AutoSize = true;
            this.cbWindow.BackColor = System.Drawing.Color.Transparent;
            this.cbWindow.Cursor = System.Windows.Forms.Cursors.Hand;
            this.cbWindow.Font = new System.Drawing.Font("Bahnschrift SemiLight", 10F);
            this.cbWindow.ForeColor = System.Drawing.Color.White;
            this.cbWindow.Location = new System.Drawing.Point(585, 208);
            this.cbWindow.Name = "cbWindow";
            this.cbWindow.Size = new System.Drawing.Size(120, 21);
            this.cbWindow.TabIndex = 19;
            this.cbWindow.Text = "Fenstermodus";
            this.cbWindow.UseVisualStyleBackColor = false;
            this.cbWindow.Click += new System.EventHandler(this.cbWindow_Click);
            // 
            // lblMaxVram
            // 
            this.lblMaxVram.AutoSize = true;
            this.lblMaxVram.BackColor = System.Drawing.Color.Transparent;
            this.lblMaxVram.Font = new System.Drawing.Font("Bahnschrift SemiLight", 10F);
            this.lblMaxVram.ForeColor = System.Drawing.Color.White;
            this.lblMaxVram.Location = new System.Drawing.Point(12, 196);
            this.lblMaxVram.Name = "lblMaxVram";
            this.lblMaxVram.Size = new System.Drawing.Size(127, 17);
            this.lblMaxVram.TabIndex = 21;
            this.lblMaxVram.Text = "Max. VRAM (in MB)";
            // 
            // nudVram
            // 
            this.nudVram.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(39)))), ((int)(((byte)(39)))), ((int)(((byte)(42)))));
            this.nudVram.Font = new System.Drawing.Font("Bahnschrift SemiLight", 9F);
            this.nudVram.ForeColor = System.Drawing.Color.White;
            this.nudVram.Location = new System.Drawing.Point(196, 200);
            this.nudVram.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.nudVram.Name = "nudVram";
            this.nudVram.Size = new System.Drawing.Size(91, 22);
            this.nudVram.TabIndex = 22;
            this.nudVram.ValueChanged += new System.EventHandler(this.nudVram_ValueChanged);
            // 
            // txtParams
            // 
            this.txtParams.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(39)))), ((int)(((byte)(39)))), ((int)(((byte)(42)))));
            this.txtParams.Font = new System.Drawing.Font("Bahnschrift SemiLight", 10F);
            this.txtParams.ForeColor = System.Drawing.SystemColors.Window;
            this.txtParams.Location = new System.Drawing.Point(15, 258);
            this.txtParams.Multiline = true;
            this.txtParams.Name = "txtParams";
            this.txtParams.Size = new System.Drawing.Size(357, 71);
            this.txtParams.TabIndex = 23;
            this.txtParams.TextChanged += new System.EventHandler(this.txtParams_TextChanged);
            // 
            // lblParams
            // 
            this.lblParams.AutoSize = true;
            this.lblParams.BackColor = System.Drawing.Color.Transparent;
            this.lblParams.Font = new System.Drawing.Font("Bahnschrift SemiLight", 10F);
            this.lblParams.ForeColor = System.Drawing.Color.White;
            this.lblParams.Location = new System.Drawing.Point(12, 232);
            this.lblParams.Name = "lblParams";
            this.lblParams.Size = new System.Drawing.Size(338, 17);
            this.lblParams.TabIndex = 24;
            this.lblParams.Text = "Andere Parameter (z.B. -cpuCount=<Anzahl Cores>)";
            // 
            // lblProfileDescription
            // 
            this.lblProfileDescription.AutoSize = true;
            this.lblProfileDescription.BackColor = System.Drawing.Color.Transparent;
            this.lblProfileDescription.Font = new System.Drawing.Font("Bahnschrift SemiLight", 10F);
            this.lblProfileDescription.ForeColor = System.Drawing.Color.White;
            this.lblProfileDescription.Location = new System.Drawing.Point(12, 124);
            this.lblProfileDescription.Name = "lblProfileDescription";
            this.lblProfileDescription.Size = new System.Drawing.Size(122, 17);
            this.lblProfileDescription.TabIndex = 25;
            this.lblProfileDescription.Text = "Arma 3 Profilpfad:";
            // 
            // lblprofilePath
            // 
            this.lblprofilePath.BackColor = System.Drawing.Color.Transparent;
            this.lblprofilePath.Font = new System.Drawing.Font("Bahnschrift SemiLight", 10F);
            this.lblprofilePath.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(197)))), ((int)(((byte)(94)))));
            this.lblprofilePath.Location = new System.Drawing.Point(193, 130);
            this.lblprofilePath.Name = "lblprofilePath";
            this.lblprofilePath.Size = new System.Drawing.Size(406, 36);
            this.lblprofilePath.TabIndex = 27;
            this.lblprofilePath.Text = "- coming soon -";
            // 
            // btnProfilePath
            // 
            this.btnProfilePath.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(39)))), ((int)(((byte)(39)))), ((int)(((byte)(42)))));
            this.btnProfilePath.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnProfilePath.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnProfilePath.Font = new System.Drawing.Font("Bahnschrift SemiLight", 8.5F);
            this.btnProfilePath.ForeColor = System.Drawing.Color.White;
            this.btnProfilePath.Location = new System.Drawing.Point(605, 128);
            this.btnProfilePath.Name = "btnProfilePath";
            this.btnProfilePath.Size = new System.Drawing.Size(110, 23);
            this.btnProfilePath.TabIndex = 28;
            this.btnProfilePath.Text = "Pfad ändern";
            this.btnProfilePath.UseVisualStyleBackColor = false;
            this.btnProfilePath.Click += new System.EventHandler(this.btnProfilePath_Click);
            // 
            // cbHyper
            // 
            this.cbHyper.AutoSize = true;
            this.cbHyper.BackColor = System.Drawing.Color.Transparent;
            this.cbHyper.Cursor = System.Windows.Forms.Cursors.Hand;
            this.cbHyper.Font = new System.Drawing.Font("Bahnschrift SemiLight", 10F);
            this.cbHyper.ForeColor = System.Drawing.Color.White;
            this.cbHyper.Location = new System.Drawing.Point(585, 234);
            this.cbHyper.Name = "cbHyper";
            this.cbHyper.Size = new System.Drawing.Size(136, 21);
            this.cbHyper.TabIndex = 29;
            this.cbHyper.Text = "Hyper-Threading";
            this.cbHyper.UseVisualStyleBackColor = false;
            this.cbHyper.CheckedChanged += new System.EventHandler(this.cbHyper_CheckedChanged);
            this.cbHyper.Click += new System.EventHandler(this.cbHyper_Click);
            // 
            // cbIntro
            // 
            this.cbIntro.AutoSize = true;
            this.cbIntro.BackColor = System.Drawing.Color.Transparent;
            this.cbIntro.Cursor = System.Windows.Forms.Cursors.Hand;
            this.cbIntro.Font = new System.Drawing.Font("Bahnschrift SemiLight", 10F);
            this.cbIntro.ForeColor = System.Drawing.Color.White;
            this.cbIntro.Location = new System.Drawing.Point(585, 260);
            this.cbIntro.Name = "cbIntro";
            this.cbIntro.Size = new System.Drawing.Size(149, 21);
            this.cbIntro.TabIndex = 30;
            this.cbIntro.Text = "Intro überspringen";
            this.cbIntro.UseVisualStyleBackColor = false;
            this.cbIntro.Click += new System.EventHandler(this.cbIntro_Click);
            // 
            // cbnologs
            // 
            this.cbnologs.AutoSize = true;
            this.cbnologs.BackColor = System.Drawing.Color.Transparent;
            this.cbnologs.Cursor = System.Windows.Forms.Cursors.Hand;
            this.cbnologs.Font = new System.Drawing.Font("Bahnschrift SemiLight", 10F);
            this.cbnologs.ForeColor = System.Drawing.Color.White;
            this.cbnologs.Location = new System.Drawing.Point(585, 285);
            this.cbnologs.Name = "cbnologs";
            this.cbnologs.Size = new System.Drawing.Size(97, 21);
            this.cbnologs.TabIndex = 31;
            this.cbnologs.Text = "keine Logs";
            this.cbnologs.UseVisualStyleBackColor = false;
            this.cbnologs.Click += new System.EventHandler(this.cbnologs_Click);
            // 
            // buttontexture
            // 
            this.buttontexture.Location = new System.Drawing.Point(286, 345);
            this.buttontexture.Name = "buttontexture";
            this.buttontexture.Size = new System.Drawing.Size(44, 23);
            this.buttontexture.TabIndex = 38;
            this.buttontexture.Text = "Textureupdate";
            this.buttontexture.UseVisualStyleBackColor = true;
            this.buttontexture.Visible = false;
            this.buttontexture.Click += new System.EventHandler(this.buttontexture_Click);
            // 
            // cbRadioMod
            // 
            this.cbRadioMod.AutoSize = true;
            this.cbRadioMod.BackColor = System.Drawing.Color.Transparent;
            this.cbRadioMod.Cursor = System.Windows.Forms.Cursors.Hand;
            this.cbRadioMod.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbRadioMod.ForeColor = System.Drawing.Color.White;
            this.cbRadioMod.Location = new System.Drawing.Point(594, 353);
            this.cbRadioMod.Name = "cbRadioMod";
            this.cbRadioMod.Size = new System.Drawing.Size(15, 14);
            this.cbRadioMod.TabIndex = 39;
            this.cbRadioMod.UseVisualStyleBackColor = false;
            this.cbRadioMod.Visible = false;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Font = new System.Drawing.Font("Bahnschrift SemiLight", 10F);
            this.label5.ForeColor = System.Drawing.Color.White;
            this.label5.Location = new System.Drawing.Point(615, 351);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(106, 17);
            this.label5.TabIndex = 40;
            this.label5.Text = "USA Radio Mod";
            this.label5.Visible = false;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.Font = new System.Drawing.Font("Bahnschrift SemiLight", 10F);
            this.label6.ForeColor = System.Drawing.Color.White;
            this.label6.Location = new System.Drawing.Point(635, 368);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(86, 17);
            this.label6.TabIndex = 41;
            this.label6.Text = "verwenden?";
            this.label6.Visible = false;
            // 
            // picInfo
            // 
            this.picInfo.BackColor = System.Drawing.Color.Transparent;
            this.picInfo.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picInfo.Image = global::USALauncher.Properties.Resources.picInfo_Image;
            this.picInfo.Location = new System.Drawing.Point(819, 12);
            this.picInfo.Name = "picInfo";
            this.picInfo.Size = new System.Drawing.Size(16, 16);
            this.picInfo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picInfo.TabIndex = 48;
            this.picInfo.TabStop = false;
            this.toolTip1.SetToolTip(this.picInfo, "Info´s");
            this.picInfo.Click += new System.EventHandler(this.picInfo_Click);
            this.picInfo.MouseEnter += new System.EventHandler(this.MouseHoverEnter);
            this.picInfo.MouseLeave += new System.EventHandler(this.MouseHoverLeave);
            // 
            // picInstagram
            // 
            this.picInstagram.BackColor = System.Drawing.Color.Transparent;
            this.picInstagram.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picInstagram.Image = global::USALauncher.Properties.Resources.picInstagramGrey_Image;
            this.picInstagram.Location = new System.Drawing.Point(54, 390);
            this.picInstagram.Name = "picInstagram";
            this.picInstagram.Size = new System.Drawing.Size(33, 33);
            this.picInstagram.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picInstagram.TabIndex = 49;
            this.picInstagram.TabStop = false;
            this.toolTip1.SetToolTip(this.picInstagram, "Folge uns auf Instagram!");
            this.picInstagram.Click += new System.EventHandler(this.picInstagram_Click);
            this.picInstagram.MouseEnter += new System.EventHandler(this.MouseHoverEnter);
            this.picInstagram.MouseLeave += new System.EventHandler(this.MouseHoverLeave);
            // 
            // picStatsImage
            // 
            this.picStatsImage.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(39)))), ((int)(((byte)(39)))), ((int)(((byte)(42)))));
            this.picStatsImage.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picStatsImage.Image = global::USALauncher.Properties.Resources.picStatistic_Image;
            this.picStatsImage.Location = new System.Drawing.Point(21, 439);
            this.picStatsImage.Name = "picStatsImage";
            this.picStatsImage.Size = new System.Drawing.Size(30, 30);
            this.picStatsImage.TabIndex = 47;
            this.picStatsImage.TabStop = false;
            this.toolTip1.SetToolTip(this.picStatsImage, "Server Statistik");
            this.picStatsImage.Click += new System.EventHandler(this.picStatsImage_Click);
            // 
            // picSteam
            // 
            this.picSteam.BackColor = System.Drawing.Color.Transparent;
            this.picSteam.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picSteam.Image = global::USALauncher.Properties.Resources.picSteamGrey_Image;
            this.picSteam.Location = new System.Drawing.Point(15, 390);
            this.picSteam.Name = "picSteam";
            this.picSteam.Size = new System.Drawing.Size(33, 33);
            this.picSteam.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picSteam.TabIndex = 57;
            this.picSteam.TabStop = false;
            this.toolTip1.SetToolTip(this.picSteam, "Trete unserer Steam Gruppe bei!");
            this.picSteam.Click += new System.EventHandler(this.picSteam_Click);
            this.picSteam.MouseEnter += new System.EventHandler(this.MouseHoverEnter);
            this.picSteam.MouseLeave += new System.EventHandler(this.MouseHoverLeave);
            // 
            // picYoutube
            // 
            this.picYoutube.BackColor = System.Drawing.Color.Transparent;
            this.picYoutube.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picYoutube.Image = global::USALauncher.Properties.Resources.picYoutubeGrey_Image;
            this.picYoutube.Location = new System.Drawing.Point(93, 390);
            this.picYoutube.Name = "picYoutube";
            this.picYoutube.Size = new System.Drawing.Size(33, 33);
            this.picYoutube.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picYoutube.TabIndex = 58;
            this.picYoutube.TabStop = false;
            this.toolTip1.SetToolTip(this.picYoutube, "Folge uns auf YouTube!");
            this.picYoutube.Click += new System.EventHandler(this.picYoutube_Click);
            this.picYoutube.MouseEnter += new System.EventHandler(this.MouseHoverEnter);
            this.picYoutube.MouseLeave += new System.EventHandler(this.MouseHoverLeave);
            // 
            // picClose
            // 
            this.picClose.BackColor = System.Drawing.Color.Transparent;
            this.picClose.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picClose.Image = global::USALauncher.Properties.Resources.picCloseWhite_Image;
            this.picClose.Location = new System.Drawing.Point(871, 12);
            this.picClose.Name = "picClose";
            this.picClose.Size = new System.Drawing.Size(18, 18);
            this.picClose.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picClose.TabIndex = 59;
            this.picClose.TabStop = false;
            this.toolTip1.SetToolTip(this.picClose, "Close");
            this.picClose.Click += new System.EventHandler(this.picClose_Click);
            this.picClose.MouseEnter += new System.EventHandler(this.MouseHoverEnter);
            this.picClose.MouseLeave += new System.EventHandler(this.MouseHoverLeave);
            // 
            // picReload
            // 
            this.picReload.BackColor = System.Drawing.Color.Transparent;
            this.picReload.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picReload.Image = global::USALauncher.Properties.Resources.picReload_Image;
            this.picReload.Location = new System.Drawing.Point(499, 399);
            this.picReload.Name = "picReload";
            this.picReload.Size = new System.Drawing.Size(80, 80);
            this.picReload.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picReload.TabIndex = 62;
            this.picReload.TabStop = false;
            this.toolTip1.SetToolTip(this.picReload, "Nach Mod- und Missionsupdates suchen");
            this.picReload.Click += new System.EventHandler(this.picReload_Click);
            this.picReload.MouseEnter += new System.EventHandler(this.MouseHoverEnter);
            this.picReload.MouseLeave += new System.EventHandler(this.MouseHoverLeave);
            // 
            // picMinimize
            // 
            this.picMinimize.BackColor = System.Drawing.Color.Transparent;
            this.picMinimize.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picMinimize.Image = global::USALauncher.Properties.Resources.picMinimize_Image;
            this.picMinimize.Location = new System.Drawing.Point(844, 8);
            this.picMinimize.Name = "picMinimize";
            this.picMinimize.Size = new System.Drawing.Size(15, 16);
            this.picMinimize.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picMinimize.TabIndex = 63;
            this.picMinimize.TabStop = false;
            this.toolTip1.SetToolTip(this.picMinimize, "Minimize");
            this.picMinimize.Click += new System.EventHandler(this.picMinimize_Click);
            this.picMinimize.MouseEnter += new System.EventHandler(this.MouseHoverEnter);
            this.picMinimize.MouseLeave += new System.EventHandler(this.MouseHoverLeave);
            // 
            // picGetServerInfo
            // 
            this.picGetServerInfo.BackColor = System.Drawing.Color.Transparent;
            this.picGetServerInfo.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picGetServerInfo.Image = global::USALauncher.Properties.Resources.picReload_Image;
            this.picGetServerInfo.Location = new System.Drawing.Point(295, 429);
            this.picGetServerInfo.Name = "picGetServerInfo";
            this.picGetServerInfo.Size = new System.Drawing.Size(50, 50);
            this.picGetServerInfo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picGetServerInfo.TabIndex = 68;
            this.picGetServerInfo.TabStop = false;
            this.toolTip1.SetToolTip(this.picGetServerInfo, "Serverinfos aktualisieren");
            this.picGetServerInfo.Click += new System.EventHandler(this.picGetServerInfo_Click);
            this.picGetServerInfo.MouseEnter += new System.EventHandler(this.MouseHoverEnter);
            this.picGetServerInfo.MouseLeave += new System.EventHandler(this.MouseHoverLeave);
            // 
            // toolTipLaunch
            // 
            this.toolTipLaunch.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.toolTipLaunch.ForeColor = System.Drawing.Color.Black;
            // 
            // picBanner
            // 
            this.picBanner.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(47)))), ((int)(((byte)(49)))));
            this.picBanner.Image = ((System.Drawing.Image)(resources.GetObject("picBanner.Image")));
            this.picBanner.Location = new System.Drawing.Point(0, -1);
            this.picBanner.Name = "picBanner";
            this.picBanner.Size = new System.Drawing.Size(901, 60);
            this.picBanner.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picBanner.TabIndex = 3;
            this.picBanner.TabStop = false;
            this.picBanner.MouseDown += new System.Windows.Forms.MouseEventHandler(this.picBanner_MouseDown);
            // 
            // picDiscord
            // 
            this.picDiscord.BackColor = System.Drawing.Color.Transparent;
            this.picDiscord.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picDiscord.Image = global::USALauncher.Properties.Resources.picDiscord_Image;
            this.picDiscord.Location = new System.Drawing.Point(749, 89);
            this.picDiscord.Name = "picDiscord";
            this.picDiscord.Size = new System.Drawing.Size(134, 40);
            this.picDiscord.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picDiscord.TabIndex = 51;
            this.picDiscord.TabStop = false;
            this.picDiscord.Click += new System.EventHandler(this.picDiscord_Click);
            this.picDiscord.MouseEnter += new System.EventHandler(this.MouseHoverEnter);
            this.picDiscord.MouseLeave += new System.EventHandler(this.MouseHoverLeave);
            // 
            // picUpdates
            // 
            this.picUpdates.BackColor = System.Drawing.Color.Transparent;
            this.picUpdates.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picUpdates.Image = global::USALauncher.Properties.Resources.picUpdates_Image;
            this.picUpdates.Location = new System.Drawing.Point(749, 273);
            this.picUpdates.Name = "picUpdates";
            this.picUpdates.Size = new System.Drawing.Size(134, 40);
            this.picUpdates.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picUpdates.TabIndex = 52;
            this.picUpdates.TabStop = false;
            this.picUpdates.Click += new System.EventHandler(this.serverUpdatesButton_Click);
            this.picUpdates.MouseEnter += new System.EventHandler(this.MouseHoverEnter);
            this.picUpdates.MouseLeave += new System.EventHandler(this.MouseHoverLeave);
            // 
            // picHomepage
            // 
            this.picHomepage.BackColor = System.Drawing.Color.Transparent;
            this.picHomepage.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picHomepage.Image = global::USALauncher.Properties.Resources.picHomepage_Image;
            this.picHomepage.Location = new System.Drawing.Point(749, 227);
            this.picHomepage.Name = "picHomepage";
            this.picHomepage.Size = new System.Drawing.Size(134, 40);
            this.picHomepage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picHomepage.TabIndex = 53;
            this.picHomepage.TabStop = false;
            this.picHomepage.Click += new System.EventHandler(this.picHomepage_Click);
            this.picHomepage.MouseEnter += new System.EventHandler(this.MouseHoverEnter);
            this.picHomepage.MouseLeave += new System.EventHandler(this.MouseHoverLeave);
            // 
            // picRegeln
            // 
            this.picRegeln.BackColor = System.Drawing.Color.Transparent;
            this.picRegeln.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picRegeln.Image = global::USALauncher.Properties.Resources.picRegeln_Image;
            this.picRegeln.Location = new System.Drawing.Point(749, 181);
            this.picRegeln.Name = "picRegeln";
            this.picRegeln.Size = new System.Drawing.Size(134, 40);
            this.picRegeln.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picRegeln.TabIndex = 54;
            this.picRegeln.TabStop = false;
            this.picRegeln.Click += new System.EventHandler(this.picRegeln_Click);
            this.picRegeln.MouseEnter += new System.EventHandler(this.MouseHoverEnter);
            this.picRegeln.MouseLeave += new System.EventHandler(this.MouseHoverLeave);
            // 
            // picTeamspeak
            // 
            this.picTeamspeak.BackColor = System.Drawing.Color.Transparent;
            this.picTeamspeak.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picTeamspeak.Image = global::USALauncher.Properties.Resources.picTeamspeak_Image;
            this.picTeamspeak.Location = new System.Drawing.Point(749, 135);
            this.picTeamspeak.Name = "picTeamspeak";
            this.picTeamspeak.Size = new System.Drawing.Size(134, 40);
            this.picTeamspeak.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picTeamspeak.TabIndex = 55;
            this.picTeamspeak.TabStop = false;
            this.picTeamspeak.Click += new System.EventHandler(this.picTeamspeak_Click);
            this.picTeamspeak.MouseEnter += new System.EventHandler(this.MouseHoverEnter);
            this.picTeamspeak.MouseLeave += new System.EventHandler(this.MouseHoverLeave);
            // 
            // picSpielerOnline
            // 
            this.picSpielerOnline.BackColor = System.Drawing.Color.Transparent;
            this.picSpielerOnline.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.picSpielerOnline.Image = global::USALauncher.Properties.Resources.picPlayersonline_Image;
            this.picSpielerOnline.Location = new System.Drawing.Point(15, 429);
            this.picSpielerOnline.Name = "picSpielerOnline";
            this.picSpielerOnline.Size = new System.Drawing.Size(330, 50);
            this.picSpielerOnline.TabIndex = 56;
            this.picSpielerOnline.TabStop = false;
            // 
            // lblMaxVram2
            // 
            this.lblMaxVram2.AutoSize = true;
            this.lblMaxVram2.BackColor = System.Drawing.Color.Transparent;
            this.lblMaxVram2.Font = new System.Drawing.Font("Bahnschrift SemiLight", 10F);
            this.lblMaxVram2.ForeColor = System.Drawing.Color.White;
            this.lblMaxVram2.Location = new System.Drawing.Point(293, 198);
            this.lblMaxVram2.Name = "lblMaxVram2";
            this.lblMaxVram2.Size = new System.Drawing.Size(90, 17);
            this.lblMaxVram2.TabIndex = 61;
            this.lblMaxVram2.Text = "➜ 0 = default";
            // 
            // playersOnlineLabel
            // 
            this.playersOnlineLabel.AutoSize = true;
            this.playersOnlineLabel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(39)))), ((int)(((byte)(39)))), ((int)(((byte)(42)))));
            this.playersOnlineLabel.Font = new System.Drawing.Font("Bahnschrift Light", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.playersOnlineLabel.ForeColor = System.Drawing.Color.White;
            this.playersOnlineLabel.Location = new System.Drawing.Point(49, 432);
            this.playersOnlineLabel.Name = "playersOnlineLabel";
            this.playersOnlineLabel.Size = new System.Drawing.Size(201, 29);
            this.playersOnlineLabel.TabIndex = 67;
            this.playersOnlineLabel.Text = "Spieler online: ?/?";
            // 
            // picLaunch
            // 
            this.picLaunch.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picLaunch.Image = global::USALauncher.Properties.Resources.picLaunch_Image;
            this.picLaunch.Location = new System.Drawing.Point(592, 399);
            this.picLaunch.Margin = new System.Windows.Forms.Padding(0);
            this.picLaunch.Name = "picLaunch";
            this.picLaunch.Opacity = 1F;
            this.picLaunch.Size = new System.Drawing.Size(291, 80);
            this.picLaunch.TabIndex = 15;
            this.picLaunch.TabStop = false;
            this.picLaunch.Click += new System.EventHandler(this.btnLaunch_Click);
            this.picLaunch.MouseEnter += new System.EventHandler(this.MouseHoverEnter);
            this.picLaunch.MouseLeave += new System.EventHandler(this.MouseHoverLeave);
            // 
            // Mainframe
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(900, 506);
            this.ControlBox = false;
            this.Controls.Add(this.picGetServerInfo);
            this.Controls.Add(this.playersOnlineLabel);
            this.Controls.Add(this.picMinimize);
            this.Controls.Add(this.picReload);
            this.Controls.Add(this.lblMaxVram2);
            this.Controls.Add(this.picClose);
            this.Controls.Add(this.picYoutube);
            this.Controls.Add(this.picSteam);
            this.Controls.Add(this.picTeamspeak);
            this.Controls.Add(this.picRegeln);
            this.Controls.Add(this.picHomepage);
            this.Controls.Add(this.picUpdates);
            this.Controls.Add(this.picDiscord);
            this.Controls.Add(this.picInstagram);
            this.Controls.Add(this.picInfo);
            this.Controls.Add(this.picStatsImage);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.cbRadioMod);
            this.Controls.Add(this.buttontexture);
            this.Controls.Add(this.cbnologs);
            this.Controls.Add(this.cbIntro);
            this.Controls.Add(this.cbHyper);
            this.Controls.Add(this.btnProfilePath);
            this.Controls.Add(this.lblprofilePath);
            this.Controls.Add(this.lblProfileDescription);
            this.Controls.Add(this.lblParams);
            this.Controls.Add(this.txtParams);
            this.Controls.Add(this.nudVram);
            this.Controls.Add(this.lblMaxVram);
            this.Controls.Add(this.cbWindow);
            this.Controls.Add(this.cbSplash);
            this.Controls.Add(this.lblProfil);
            this.Controls.Add(this.cbProfile);
            this.Controls.Add(this.picLaunch);
            this.Controls.Add(this.lblArmaSpieler);
            this.Controls.Add(this.lblVersion);
            this.Controls.Add(this.wbChangelog);
            this.Controls.Add(this.btnChangePath);
            this.Controls.Add(this.lblInstallationPath);
            this.Controls.Add(this.lblPathDescription);
            this.Controls.Add(this.picBanner);
            this.Controls.Add(this.picSpielerOnline);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(900, 506);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(900, 506);
            this.Name = "Mainframe";
            this.Load += new System.EventHandler(this.MainFrame_Load);
            ((System.ComponentModel.ISupportInitialize)(this.nudVram)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picInfo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picInstagram)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picStatsImage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picSteam)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picYoutube)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picClose)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picReload)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picMinimize)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picGetServerInfo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBanner)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picDiscord)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picUpdates)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picHomepage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picRegeln)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picTeamspeak)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picSpielerOnline)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picLaunch)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

    }
}