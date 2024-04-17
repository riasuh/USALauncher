using System;
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

namespace USALauncher;

public class Mainframe : Form
{
	private string missionDownloadUri = "https://download.usa-life.net/mission.txt";

	private string modDownloadUri = "https://download.usa-life.net/mod.txt";

	public const int WM_NCLBUTTONDOWN = 161;

	public const int HT_CAPTION = 2;

	public string armaPath;

	public string localPath;

	public string profilePath;

	public string defaultprofilepath;

	private IPAddress[] addresslist = Dns.GetHostAddresses("51.195.61.187");

	public ArmA3ServerInfo info = new ArmA3ServerInfo(Dns.GetHostAddresses("51.195.61.187").FirstOrDefault().ToString(), 2303);

    private System.Threading.Timer timer;

	private bool getclick;

	private IContainer components;

	private PictureBox picBanner;

	private PictureBoxOpacity picTeamspeak;

	private PictureBoxOpacity picRegeln;

	private PictureBoxOpacity picHomepage;

	private PictureBoxOpacity picForum;

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

	private Label label1;

	private NumericUpDown nudVram;

	private TextBox txtParams;

	private Label label2;

	private Label lblProfileDescription;

	private PictureBoxOpacity pictureBoxOpacity1;

	private Label lblprofilePath;

	private Button btnProfilePath;

	private CheckBox cbHyper;

	private CheckBox cbIntro;

	private CheckBox cbnologs;

	private FolderBrowserDialog fbdprofilePath;

	private PictureBoxOpacity pictureBoxOpacity2;

	private Button buttontexture;

	private CheckBox cbRadioMod;

	private Label label5;

	private Label label6;

	private PictureBoxOpacity pictureBoxOpacity5;

	//private PictureBoxOpacity pictureBoxOpacity6;

	private ToolTip toolTip1;

	private ToolTip toolTip2;

	private ToolTip toolTipLaunch;

	private PictureBoxOpacity pictureBoxOpacity7;

	private PictureBoxOpacity pictureBoxOpacity8;

	private PictureBox pictureBox1;

	private PictureBox pictureBox2;

	private PictureBox pictureBox3;

	public object MainFrame { get; private set; }

	public object MainForm { get; private set; }

	private void MainFrame_Load(object sender, EventArgs e)
	{
		AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
		AppDomain.CurrentDomain.FirstChanceException += CurrentDomain_FirstChanceException;
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


    public Mainframe()
	{
		InitializeComponent();
		base.Icon = USALauncher.Properties.Resources.LOGO01_snowwhite_PNG;
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
		new Thread((ThreadStart)delegate
		{
			updateStats(null, null);
		}).Start();
		tmrUpdateStats.Start();
		wbChangelog.DocumentTitleChanged += AdaptChangelog;
	}

	[DllImport("user32.dll")]
	public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

	[DllImport("user32.dll")]
	public static extern bool ReleaseCapture();

	private void picClose_Click(object sender, EventArgs e)
	{
		Application.Exit();
	}

	private void MainFrame_MouseDown(object sender, MouseEventArgs e)
	{
	}

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

	private void steamBtn_Click(object sender, EventArgs e)
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
			new Download(text, localPath, armaPath, this).ShowDialog();
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
			new Download(text, localPath, armaPath, this).ShowDialog();
		}
	}

	public void disableLaunchButton()
	{
		this.InvokeEx(delegate(Mainframe f)
		{
			f.picLaunch.Enabled = false;
		});
		this.InvokeEx(delegate(Mainframe f)
		{
			f.picLaunch.Text = "Starte ArmA...";
		});
		timer = new System.Threading.Timer(delegate
		{
			this.InvokeEx(delegate(Mainframe f)
			{
				f.picLaunch.Enabled = true;
			});
			this.InvokeEx(delegate(Mainframe f)
			{
				f.picLaunch.Text = "USA LIFE Spielen";
			});
			timer.Dispose();
		}, null, 10000, -1);
	}

	private void MouseHoverEnter(object sender, EventArgs e)
	{
		((PictureBoxOpacity)sender).Opacity = 0.9f;
	}

	private void MouseHoverLeave(object sender, EventArgs e)
	{
		((PictureBoxOpacity)sender).Opacity = 1f;
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

	private void pictureBoxOpacity2_Click(object sender2, EventArgs e2)
	{
		Updater();
	}

	private void Updater()
	{
		new Download();
		using (WebClient webClient = new WebClient())
		{
			webClient.Headers.Add("user-agent", "Only a test!");
			string text = webClient.DownloadString(missionDownloadUri).Split(new string[1] { "\n" }, StringSplitOptions.None)[0].Split('/').Last();
			if (File.Exists(localPath + "\\" + text))
			{
				Form1 form = new Form1();
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
		Download download = new Download();
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
				Form2 form2 = new Form2();
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
		Download download = new Download();
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
		Download download2 = new Download();
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
		Download download = new Download();
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

	private void pictureBoxOpacity7_Click(object sender, EventArgs e)
	{
		base.WindowState = FormWindowState.Minimized;
	}

	private void pictureBoxOpacity7_Click_1(object sender, EventArgs e)
	{
		base.WindowState = FormWindowState.Minimized;
	}

	private void MouserHoverLeave(object sender, EventArgs e)
	{
	}

	private void pictureBoxOpacity8_Click(object sender, EventArgs e)
	{
		Application.Exit();
	}

	private void button1_Click(object sender, EventArgs e)
	{
		new Form1().Show(this);
	}

	private void lblArmaSpieler_Click(object sender, EventArgs e)
	{
	}

	private void pictureBox2_Click(object sender, EventArgs e)
	{
		InfoFenster infoFenster = new InfoFenster();
		infoFenster.StartPosition = FormStartPosition.CenterParent;
		infoFenster.ShowDialog();
	}

	private void pictureBox3_Click(object sender, EventArgs e)
	{
		Process.Start("https://www.instagram.com/usaliferpg/");
	}

	private void pictureBox1_Click(object sender, EventArgs e)
	{
		Process.Start("https://www.battlemetrics.com/servers/arma3/4373113");
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		base.Dispose(disposing);
	}

	private void InitializeComponent()
	{
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Mainframe));
            this.picBanner = new System.Windows.Forms.PictureBox();
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
            this.label1 = new System.Windows.Forms.Label();
            this.nudVram = new System.Windows.Forms.NumericUpDown();
            this.txtParams = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
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
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pictureBoxOpacity8 = new USALauncher.PictureBoxOpacity();
            this.pictureBoxOpacity7 = new USALauncher.PictureBoxOpacity();
            this.pictureBoxOpacity5 = new USALauncher.PictureBoxOpacity();
            this.pictureBoxOpacity2 = new USALauncher.PictureBoxOpacity();
            this.toolTip2 = new System.Windows.Forms.ToolTip(this.components);
            this.toolTipLaunch = new System.Windows.Forms.ToolTip(this.components);
            this.pictureBoxOpacity1 = new USALauncher.PictureBoxOpacity();
            this.picLaunch = new USALauncher.PictureBoxOpacity();
            this.picForum = new USALauncher.PictureBoxOpacity();
            this.picHomepage = new USALauncher.PictureBoxOpacity();
            this.picRegeln = new USALauncher.PictureBoxOpacity();
            this.picTeamspeak = new USALauncher.PictureBoxOpacity();
            ((System.ComponentModel.ISupportInitialize)(this.picBanner)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudVram)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxOpacity8)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxOpacity7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxOpacity5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxOpacity2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxOpacity1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picLaunch)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picForum)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picHomepage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picRegeln)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picTeamspeak)).BeginInit();
            this.SuspendLayout();
            // 
            // picBanner
            // 
            this.picBanner.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(47)))), ((int)(((byte)(49)))));
            this.picBanner.Image = ((System.Drawing.Image)(resources.GetObject("picBanner.Image")));
            this.picBanner.Location = new System.Drawing.Point(0, -1);
            this.picBanner.Name = "picBanner";
            this.picBanner.Size = new System.Drawing.Size(901, 60);
            this.picBanner.TabIndex = 3;
            this.picBanner.TabStop = false;
            this.picBanner.MouseDown += new System.Windows.Forms.MouseEventHandler(this.picBanner_MouseDown);
            // 
            // lblPathDescription
            // 
            this.lblPathDescription.AutoSize = true;
            this.lblPathDescription.BackColor = System.Drawing.Color.Transparent;
            this.lblPathDescription.Font = new System.Drawing.Font("Bahnschrift SemiLight", 10F);
            this.lblPathDescription.ForeColor = System.Drawing.Color.White;
            this.lblPathDescription.Location = new System.Drawing.Point(12, 124);
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
            this.lblInstallationPath.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.lblInstallationPath.Location = new System.Drawing.Point(193, 124);
            this.lblInstallationPath.Name = "lblInstallationPath";
            this.lblInstallationPath.Size = new System.Drawing.Size(77, 17);
            this.lblInstallationPath.TabIndex = 9;
            this.lblInstallationPath.Text = "UNKNOWN";
            // 
            // btnChangePath
            // 
            this.btnChangePath.BackColor = System.Drawing.Color.Transparent;
            this.btnChangePath.Font = new System.Drawing.Font("Bahnschrift SemiLight", 8.5F);
            this.btnChangePath.Location = new System.Drawing.Point(751, 121);
            this.btnChangePath.Name = "btnChangePath";
            this.btnChangePath.Size = new System.Drawing.Size(137, 23);
            this.btnChangePath.TabIndex = 10;
            this.btnChangePath.Text = "Pfad ändern";
            this.btnChangePath.UseVisualStyleBackColor = false;
            this.btnChangePath.Click += new System.EventHandler(this.btnChangePath_Click);
            // 
            // wbChangelog
            // 
            this.wbChangelog.Location = new System.Drawing.Point(290, 415);
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
            this.lblVersion.Location = new System.Drawing.Point(735, 482);
            this.lblVersion.Name = "lblVersion";
            this.lblVersion.Size = new System.Drawing.Size(157, 14);
            this.lblVersion.TabIndex = 12;
            this.lblVersion.Text = "USA LIFE Launcher v1.3.0.0";
            // 
            // lblArmaSpieler
            // 
            this.lblArmaSpieler.AutoSize = true;
            this.lblArmaSpieler.BackColor = System.Drawing.Color.Transparent;
            this.lblArmaSpieler.Font = new System.Drawing.Font("Bahnschrift Light", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblArmaSpieler.ForeColor = System.Drawing.Color.White;
            this.lblArmaSpieler.Location = new System.Drawing.Point(51, 466);
            this.lblArmaSpieler.Name = "lblArmaSpieler";
            this.lblArmaSpieler.Size = new System.Drawing.Size(211, 29);
            this.lblArmaSpieler.TabIndex = 13;
            this.lblArmaSpieler.Text = "Spieler Online:    /?";
            this.lblArmaSpieler.Click += new System.EventHandler(this.lblArmaSpieler_Click);
            // 
            // tmrUpdateStats
            // 
            this.tmrUpdateStats.Interval = 30000;
            this.tmrUpdateStats.Tick += new System.EventHandler(this.updateStats);
            // 
            // cbProfile
            // 
            this.cbProfile.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(27)))), ((int)(((byte)(28)))), ((int)(((byte)(33)))));
            this.cbProfile.Cursor = System.Windows.Forms.Cursors.Hand;
            this.cbProfile.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbProfile.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbProfile.Font = new System.Drawing.Font("Bahnschrift SemiLight", 9F);
            this.cbProfile.ForeColor = System.Drawing.SystemColors.Window;
            this.cbProfile.FormattingEnabled = true;
            this.cbProfile.Location = new System.Drawing.Point(196, 193);
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
            this.lblProfil.Location = new System.Drawing.Point(12, 194);
            this.lblProfil.Name = "lblProfil";
            this.lblProfil.Size = new System.Drawing.Size(46, 17);
            this.lblProfil.TabIndex = 17;
            this.lblProfil.Text = "Profil:";
            // 
            // cbSplash
            // 
            this.cbSplash.AutoSize = true;
            this.cbSplash.BackColor = System.Drawing.Color.Transparent;
            this.cbSplash.Font = new System.Drawing.Font("Bahnschrift SemiLight", 10F);
            this.cbSplash.ForeColor = System.Drawing.Color.White;
            this.cbSplash.Location = new System.Drawing.Point(750, 179);
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
            this.cbWindow.Font = new System.Drawing.Font("Bahnschrift SemiLight", 10F);
            this.cbWindow.ForeColor = System.Drawing.Color.White;
            this.cbWindow.Location = new System.Drawing.Point(750, 205);
            this.cbWindow.Name = "cbWindow";
            this.cbWindow.Size = new System.Drawing.Size(120, 21);
            this.cbWindow.TabIndex = 19;
            this.cbWindow.Text = "Fenstermodus";
            this.cbWindow.UseVisualStyleBackColor = false;
            this.cbWindow.Click += new System.EventHandler(this.cbWindow_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Bahnschrift SemiLight", 10F);
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(12, 224);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(177, 17);
            this.label1.TabIndex = 21;
            this.label1.Text = "Max. VRAM (MB), 0 default:";
            // 
            // nudVram
            // 
            this.nudVram.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(27)))), ((int)(((byte)(28)))), ((int)(((byte)(33)))));
            this.nudVram.Font = new System.Drawing.Font("Bahnschrift SemiLight", 9F);
            this.nudVram.ForeColor = System.Drawing.Color.White;
            this.nudVram.Location = new System.Drawing.Point(196, 224);
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
            this.txtParams.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(27)))), ((int)(((byte)(28)))), ((int)(((byte)(33)))));
            this.txtParams.Font = new System.Drawing.Font("Bahnschrift SemiLight", 10F);
            this.txtParams.ForeColor = System.Drawing.SystemColors.Window;
            this.txtParams.Location = new System.Drawing.Point(15, 281);
            this.txtParams.Multiline = true;
            this.txtParams.Name = "txtParams";
            this.txtParams.Size = new System.Drawing.Size(315, 83);
            this.txtParams.TabIndex = 23;
            this.txtParams.TextChanged += new System.EventHandler(this.txtParams_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Bahnschrift SemiLight", 10F);
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(12, 261);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(338, 17);
            this.label2.TabIndex = 24;
            this.label2.Text = "Andere Parameter (z.B. -cpuCount=<Anzahl Cores>)";
            // 
            // lblProfileDescription
            // 
            this.lblProfileDescription.AutoSize = true;
            this.lblProfileDescription.BackColor = System.Drawing.Color.Transparent;
            this.lblProfileDescription.Font = new System.Drawing.Font("Bahnschrift SemiLight", 10F);
            this.lblProfileDescription.ForeColor = System.Drawing.Color.White;
            this.lblProfileDescription.Location = new System.Drawing.Point(12, 153);
            this.lblProfileDescription.Name = "lblProfileDescription";
            this.lblProfileDescription.Size = new System.Drawing.Size(122, 17);
            this.lblProfileDescription.TabIndex = 25;
            this.lblProfileDescription.Text = "Arma 3 Profilpfad:";
            // 
            // lblprofilePath
            // 
            this.lblprofilePath.AutoSize = true;
            this.lblprofilePath.BackColor = System.Drawing.Color.Transparent;
            this.lblprofilePath.Font = new System.Drawing.Font("Bahnschrift SemiLight", 10F);
            this.lblprofilePath.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.lblprofilePath.Location = new System.Drawing.Point(193, 153);
            this.lblprofilePath.Name = "lblprofilePath";
            this.lblprofilePath.Size = new System.Drawing.Size(112, 17);
            this.lblprofilePath.TabIndex = 27;
            this.lblprofilePath.Text = "- coming soon -";
            // 
            // btnProfilePath
            // 
            this.btnProfilePath.Font = new System.Drawing.Font("Bahnschrift SemiLight", 8.5F);
            this.btnProfilePath.Location = new System.Drawing.Point(751, 150);
            this.btnProfilePath.Name = "btnProfilePath";
            this.btnProfilePath.Size = new System.Drawing.Size(137, 23);
            this.btnProfilePath.TabIndex = 28;
            this.btnProfilePath.Text = "Pfad ändern";
            this.btnProfilePath.UseVisualStyleBackColor = true;
            this.btnProfilePath.Click += new System.EventHandler(this.btnProfilePath_Click);
            // 
            // cbHyper
            // 
            this.cbHyper.AutoSize = true;
            this.cbHyper.BackColor = System.Drawing.Color.Transparent;
            this.cbHyper.Font = new System.Drawing.Font("Bahnschrift SemiLight", 10F);
            this.cbHyper.ForeColor = System.Drawing.Color.White;
            this.cbHyper.Location = new System.Drawing.Point(750, 231);
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
            this.cbIntro.Font = new System.Drawing.Font("Bahnschrift SemiLight", 10F);
            this.cbIntro.ForeColor = System.Drawing.Color.White;
            this.cbIntro.Location = new System.Drawing.Point(750, 257);
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
            this.cbnologs.Font = new System.Drawing.Font("Bahnschrift SemiLight", 10F);
            this.cbnologs.ForeColor = System.Drawing.Color.White;
            this.cbnologs.Location = new System.Drawing.Point(750, 282);
            this.cbnologs.Name = "cbnologs";
            this.cbnologs.Size = new System.Drawing.Size(97, 21);
            this.cbnologs.TabIndex = 31;
            this.cbnologs.Text = "keine Logs";
            this.cbnologs.UseVisualStyleBackColor = false;
            this.cbnologs.Click += new System.EventHandler(this.cbnologs_Click);
            // 
            // buttontexture
            // 
            this.buttontexture.Location = new System.Drawing.Point(290, 386);
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
            this.cbRadioMod.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbRadioMod.ForeColor = System.Drawing.Color.White;
            this.cbRadioMod.Location = new System.Drawing.Point(750, 315);
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
            this.label5.Location = new System.Drawing.Point(767, 307);
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
            this.label6.Location = new System.Drawing.Point(767, 323);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(86, 17);
            this.label6.TabIndex = 41;
            this.label6.Text = "verwenden?";
            this.label6.Visible = false;
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(47)))), ((int)(((byte)(49)))));
            this.pictureBox2.ErrorImage = global::USALauncher.Properties.Resources.bg;
            this.pictureBox2.Image = global::USALauncher.Properties.Resources.infoicon;
            this.pictureBox2.Location = new System.Drawing.Point(809, 13);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(22, 22);
            this.pictureBox2.TabIndex = 48;
            this.pictureBox2.TabStop = false;
            this.toolTip1.SetToolTip(this.pictureBox2, "Info´s");
            this.pictureBox2.Click += new System.EventHandler(this.pictureBox2_Click);
            // 
            // pictureBox3
            // 
            this.pictureBox3.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox3.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pictureBox3.Image = global::USALauncher.Properties.Resources.instaicon;
            this.pictureBox3.Location = new System.Drawing.Point(854, 69);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(33, 33);
            this.pictureBox3.TabIndex = 49;
            this.pictureBox3.TabStop = false;
            this.toolTip1.SetToolTip(this.pictureBox3, "Folge uns auf Instagram!");
            this.pictureBox3.Click += new System.EventHandler(this.pictureBox3_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox1.Image = global::USALauncher.Properties.Resources.minimal_29_512_Kopie;
            this.pictureBox1.Location = new System.Drawing.Point(15, 467);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(30, 30);
            this.pictureBox1.TabIndex = 47;
            this.pictureBox1.TabStop = false;
            this.toolTip1.SetToolTip(this.pictureBox1, "Server Statistik");
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // pictureBoxOpacity8
            // 
            this.pictureBoxOpacity8.BackColor = System.Drawing.SystemColors.Control;
            this.pictureBoxOpacity8.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pictureBoxOpacity8.Image = global::USALauncher.Properties.Resources.picclose1;
            this.pictureBoxOpacity8.Location = new System.Drawing.Point(866, 12);
            this.pictureBoxOpacity8.Name = "pictureBoxOpacity8";
            this.pictureBoxOpacity8.Opacity = 1F;
            this.pictureBoxOpacity8.Size = new System.Drawing.Size(23, 23);
            this.pictureBoxOpacity8.TabIndex = 45;
            this.pictureBoxOpacity8.TabStop = false;
            this.toolTip1.SetToolTip(this.pictureBoxOpacity8, "Beenden");
            this.pictureBoxOpacity8.Click += new System.EventHandler(this.pictureBoxOpacity8_Click);
            this.pictureBoxOpacity8.MouseEnter += new System.EventHandler(this.MouseHoverEnter);
            this.pictureBoxOpacity8.MouseLeave += new System.EventHandler(this.MouseHoverLeave);
            // 
            // pictureBoxOpacity7
            // 
            this.pictureBoxOpacity7.BackColor = System.Drawing.SystemColors.Control;
            this.pictureBoxOpacity7.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pictureBoxOpacity7.Image = global::USALauncher.Properties.Resources.picmin;
            this.pictureBoxOpacity7.Location = new System.Drawing.Point(838, 12);
            this.pictureBoxOpacity7.Name = "pictureBoxOpacity7";
            this.pictureBoxOpacity7.Opacity = 1F;
            this.pictureBoxOpacity7.Size = new System.Drawing.Size(23, 23);
            this.pictureBoxOpacity7.TabIndex = 44;
            this.pictureBoxOpacity7.TabStop = false;
            this.toolTip1.SetToolTip(this.pictureBoxOpacity7, "Minimieren");
            this.pictureBoxOpacity7.Click += new System.EventHandler(this.pictureBoxOpacity7_Click_1);
            this.pictureBoxOpacity7.MouseEnter += new System.EventHandler(this.MouseHoverEnter);
            this.pictureBoxOpacity7.MouseLeave += new System.EventHandler(this.MouseHoverLeave);
            // 
            // pictureBoxOpacity5
            // 
            this.pictureBoxOpacity5.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pictureBoxOpacity5.Image = global::USALauncher.Properties.Resources.steam;
            this.pictureBoxOpacity5.Location = new System.Drawing.Point(815, 69);
            this.pictureBoxOpacity5.Name = "pictureBoxOpacity5";
            this.pictureBoxOpacity5.Opacity = 1F;
            this.pictureBoxOpacity5.Size = new System.Drawing.Size(33, 33);
            this.pictureBoxOpacity5.TabIndex = 42;
            this.pictureBoxOpacity5.TabStop = false;
            this.toolTip1.SetToolTip(this.pictureBoxOpacity5, "Folge uns auf Steam!");
            this.pictureBoxOpacity5.Click += new System.EventHandler(this.steamBtn_Click);
            // 
            // pictureBoxOpacity2
            // 
            this.pictureBoxOpacity2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pictureBoxOpacity2.Image = ((System.Drawing.Image)(resources.GetObject("pictureBoxOpacity2.Image")));
            this.pictureBoxOpacity2.Location = new System.Drawing.Point(491, 397);
            this.pictureBoxOpacity2.Name = "pictureBoxOpacity2";
            this.pictureBoxOpacity2.Opacity = 1F;
            this.pictureBoxOpacity2.Size = new System.Drawing.Size(80, 80);
            this.pictureBoxOpacity2.TabIndex = 32;
            this.pictureBoxOpacity2.TabStop = false;
            this.toolTip1.SetToolTip(this.pictureBoxOpacity2, "Nach Updates suchen");
            this.pictureBoxOpacity2.Click += new System.EventHandler(this.pictureBoxOpacity2_Click);
            this.pictureBoxOpacity2.MouseEnter += new System.EventHandler(this.MouseHoverEnter);
            this.pictureBoxOpacity2.MouseLeave += new System.EventHandler(this.MouseHoverLeave);
            // 
            // toolTipLaunch
            // 
            this.toolTipLaunch.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.toolTipLaunch.ForeColor = System.Drawing.Color.Black;
            // 
            // pictureBoxOpacity1
            // 
            this.pictureBoxOpacity1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pictureBoxOpacity1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBoxOpacity1.Image")));
            this.pictureBoxOpacity1.Location = new System.Drawing.Point(627, 69);
            this.pictureBoxOpacity1.Name = "pictureBoxOpacity1";
            this.pictureBoxOpacity1.Opacity = 1F;
            this.pictureBoxOpacity1.Size = new System.Drawing.Size(136, 33);
            this.pictureBoxOpacity1.TabIndex = 26;
            this.pictureBoxOpacity1.TabStop = false;
            this.pictureBoxOpacity1.Click += new System.EventHandler(this.serverUpdatesButton_Click);
            this.pictureBoxOpacity1.MouseEnter += new System.EventHandler(this.MouseHoverEnter);
            this.pictureBoxOpacity1.MouseLeave += new System.EventHandler(this.MouseHoverLeave);
            // 
            // picLaunch
            // 
            this.picLaunch.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.picLaunch.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picLaunch.Image = ((System.Drawing.Image)(resources.GetObject("picLaunch.Image")));
            this.picLaunch.Location = new System.Drawing.Point(572, 397);
            this.picLaunch.Margin = new System.Windows.Forms.Padding(0);
            this.picLaunch.Name = "picLaunch";
            this.picLaunch.Opacity = 1F;
            this.picLaunch.Size = new System.Drawing.Size(316, 80);
            this.picLaunch.TabIndex = 15;
            this.picLaunch.TabStop = false;
            this.picLaunch.Click += new System.EventHandler(this.btnLaunch_Click);
            this.picLaunch.MouseEnter += new System.EventHandler(this.MouseHoverEnter);
            this.picLaunch.MouseLeave += new System.EventHandler(this.MouseHoverLeave);
            // 
            // picForum
            // 
            this.picForum.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picForum.Image = ((System.Drawing.Image)(resources.GetObject("picForum.Image")));
            this.picForum.Location = new System.Drawing.Point(473, 69);
            this.picForum.Name = "picForum";
            this.picForum.Opacity = 1F;
            this.picForum.Size = new System.Drawing.Size(136, 33);
            this.picForum.TabIndex = 7;
            this.picForum.TabStop = false;
            this.picForum.Click += new System.EventHandler(this.picForum_Click);
            this.picForum.MouseEnter += new System.EventHandler(this.MouseHoverEnter);
            this.picForum.MouseLeave += new System.EventHandler(this.MouseHoverLeave);
            // 
            // picHomepage
            // 
            this.picHomepage.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picHomepage.Image = ((System.Drawing.Image)(resources.GetObject("picHomepage.Image")));
            this.picHomepage.Location = new System.Drawing.Point(318, 69);
            this.picHomepage.Name = "picHomepage";
            this.picHomepage.Opacity = 1F;
            this.picHomepage.Size = new System.Drawing.Size(136, 33);
            this.picHomepage.TabIndex = 6;
            this.picHomepage.TabStop = false;
            this.picHomepage.Click += new System.EventHandler(this.picHomepage_Click);
            this.picHomepage.MouseEnter += new System.EventHandler(this.MouseHoverEnter);
            this.picHomepage.MouseLeave += new System.EventHandler(this.MouseHoverLeave);
            // 
            // picRegeln
            // 
            this.picRegeln.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picRegeln.Image = ((System.Drawing.Image)(resources.GetObject("picRegeln.Image")));
            this.picRegeln.Location = new System.Drawing.Point(165, 69);
            this.picRegeln.Name = "picRegeln";
            this.picRegeln.Opacity = 1F;
            this.picRegeln.Size = new System.Drawing.Size(136, 33);
            this.picRegeln.TabIndex = 5;
            this.picRegeln.TabStop = false;
            this.picRegeln.Click += new System.EventHandler(this.picRegeln_Click);
            this.picRegeln.MouseEnter += new System.EventHandler(this.MouseHoverEnter);
            this.picRegeln.MouseLeave += new System.EventHandler(this.MouseHoverLeave);
            // 
            // picTeamspeak
            // 
            this.picTeamspeak.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picTeamspeak.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.picTeamspeak.Image = ((System.Drawing.Image)(resources.GetObject("picTeamspeak.Image")));
            this.picTeamspeak.Location = new System.Drawing.Point(12, 69);
            this.picTeamspeak.Name = "picTeamspeak";
            this.picTeamspeak.Opacity = 1F;
            this.picTeamspeak.Size = new System.Drawing.Size(136, 33);
            this.picTeamspeak.TabIndex = 4;
            this.picTeamspeak.TabStop = false;
            this.picTeamspeak.Click += new System.EventHandler(this.picTeamspeak_Click);
            this.picTeamspeak.MouseEnter += new System.EventHandler(this.MouseHoverEnter);
            this.picTeamspeak.MouseLeave += new System.EventHandler(this.MouseHoverLeave);
            // 
            // Mainframe
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(900, 506);
            this.ControlBox = false;
            this.Controls.Add(this.pictureBox3);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.pictureBoxOpacity8);
            this.Controls.Add(this.pictureBoxOpacity7);
            this.Controls.Add(this.pictureBoxOpacity5);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.cbRadioMod);
            this.Controls.Add(this.buttontexture);
            this.Controls.Add(this.pictureBoxOpacity2);
            this.Controls.Add(this.cbnologs);
            this.Controls.Add(this.cbIntro);
            this.Controls.Add(this.cbHyper);
            this.Controls.Add(this.btnProfilePath);
            this.Controls.Add(this.lblprofilePath);
            this.Controls.Add(this.pictureBoxOpacity1);
            this.Controls.Add(this.lblProfileDescription);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtParams);
            this.Controls.Add(this.nudVram);
            this.Controls.Add(this.label1);
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
            this.Controls.Add(this.picForum);
            this.Controls.Add(this.picHomepage);
            this.Controls.Add(this.picRegeln);
            this.Controls.Add(this.picTeamspeak);
            this.Controls.Add(this.picBanner);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(900, 506);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(900, 506);
            this.Name = "Mainframe";
            this.Load += new System.EventHandler(this.MainFrame_Load);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MainFrame_MouseDown);
            ((System.ComponentModel.ISupportInitialize)(this.picBanner)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudVram)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxOpacity8)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxOpacity7)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxOpacity5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxOpacity2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxOpacity1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picLaunch)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picForum)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picHomepage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picRegeln)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picTeamspeak)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

	}
}
