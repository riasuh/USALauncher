using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Threading;
using System.Windows.Forms;
using ByteSizeLib;
using USALauncher.Properties;

namespace USALauncher;

public class Download : Form
{
	public string Fehlercode4;

	public string Fehlercode5;

	public string Fehlercode6;

	public string Fehlercode7;

	private bool showDownloadSuccess;

	private string localArma;

	private string armaPath;

	private string downloadPath;

	private string downloadPathTexture;

	private string Administrationtxt = "Bitte wenden Sie sich an die Administration in unserem Teamspeak3.";

	private string newestVersion;

	private Mainframe parent;

	private IContainer components;

	private Label label1;

	private Label lblVersion;

	private ProgressBar pgbDownload;

	private Label lblStatus;

	private PictureBox pictureBox1;

	private PictureBox pictureBox2;

	public Download()
	{
	}

	public Download(string newestVersion, string localArmaPath, string armaPath, Mainframe parent)
	{
		localArma = localArmaPath;
		this.newestVersion = newestVersion.Split('/').Last();
		this.armaPath = armaPath;
		this.parent = parent;
		InitializeComponent();
		base.Icon = USALauncher.Properties.Resources.LOGO01_snowwhite_PNG;
		CenterToScreen();
		lblVersion.Text = this.newestVersion;
		startDownload(newestVersion);
	}

	public void DownloadUpdater(string newestVersion, string localArmaPath, string armaPath, Mainframe parent)
	{
		localArma = localArmaPath;
		this.newestVersion = newestVersion.Split('/').Last();
		this.armaPath = armaPath;
		this.parent = parent;
		InitializeComponent();
		base.Icon = USALauncher.Properties.Resources.LOGO01_snowwhite_PNG;
		CenterToScreen();
		lblVersion.Text = this.newestVersion;
		startDownloadUpdater(newestVersion);
	}

	public void Downloadtexture(string newestVersion, string localArmaPath, string armaPath, Mainframe parent, bool ShowDownloadSuccess = true)
	{
		showDownloadSuccess = ShowDownloadSuccess;
		localArma = localArmaPath;
		this.newestVersion = newestVersion.Split('_').Last();
		this.armaPath = armaPath;
		this.parent = parent;
		InitializeComponent();
		base.Icon = USALauncher.Properties.Resources.LOGO01_snowwhite_PNG;
		CenterToScreen();
		lblVersion.Text = this.newestVersion;
		if (Directory.Exists(armaPath + "\\@USALifeMod"))
		{
			try
			{
				Directory.Delete(armaPath + "\\@USALifeMod", recursive: true);
			}
			catch (Exception)
			{
				lblStatus.Text = "FEHLER";
				MessageBox.Show("Keine Berechtigung beim Updaten der Modfile (Fehlercode: #4)\nSie müssen ihr Arma3 geschlossen haben, um die Mod vollständig upzudaten.\n" + Administrationtxt + "\n(URL:" + downloadPath + "/" + newestVersion + ")", "Kann die Datei nicht herunterladen", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				Close();
			}
			startDownloadTexture(newestVersion);
		}
		else
		{
			startDownloadTexture(newestVersion);
			lblVersion.Text = this.newestVersion;
		}
	}

	private void startDownload(string downloadLink)
	{
		string text = downloadLink.Split('/').Last();
		string fileName = localArma + "\\" + text;
		lblStatus.Text = "Starte Download...";
		WebClient webClient = new WebClient();
		webClient.Headers.Add("user-agent", "Only a test!");
		webClient.DownloadFileCompleted += Completed;
		webClient.DownloadProgressChanged += ProgressChanged;
		webClient.DownloadFileAsync(new Uri(downloadLink), fileName);
	}

	private void startDownloadUpdater(string downloadLink)
	{
		string text = downloadLink.Split('/').Last();
		string fileName = localArma + "\\" + text;
		lblStatus.Text = "Starte Download...";
		WebClient webClient = new WebClient();
		webClient.Headers.Add("user-agent", "Only a test!");
		webClient.DownloadFileCompleted += CompletedUpdater;
		webClient.DownloadProgressChanged += ProgressChanged;
		webClient.DownloadFileAsync(new Uri(downloadLink), fileName);
	}

	private void startDownloadTexture(string downloadLink)
	{
		string text = downloadLink.Split('/').Last();
		downloadPathTexture = armaPath + "\\" + text;
		lblStatus.Text = "Starte Download...";
		WebClient webClient = new WebClient();
		webClient.Headers.Add("user-agent", "Only a test!");
		webClient.DownloadFileCompleted += CompletedTexture;
		webClient.DownloadProgressChanged += ProgressChanged;
		webClient.DownloadFileAsync(new Uri(downloadLink), downloadPathTexture);
	}

	private void ProgressChanged(object sender, DownloadProgressChangedEventArgs e)
	{
		pgbDownload.Value = e.ProgressPercentage;
		lblStatus.Text = "Download: " + ByteSize.FromBytes(e.BytesReceived).ToString() + "/" + ByteSize.FromBytes(e.TotalBytesToReceive).ToString();
	}

	private void Completed(object sender, AsyncCompletedEventArgs e)
	{
		pgbDownload.Value = 100;
		if (e.Error == null)
		{
			lblStatus.Text = "Download abgeschlossen. Starte ArmA...";
			Process.Start(new ProcessStartInfo
			{
				FileName = Path.GetFileName(armaPath + "\\arma3launcher.exe"),
				WorkingDirectory = Path.GetDirectoryName(armaPath + "\\arma3launcher.exe"),
				Arguments = " -noLauncher -connect=s.usa-life.net -useBE"
			});
			parent.disableLaunchButton();
			Close();
			return;
		}
		string path = localArma + "\\" + newestVersion;
		try
		{
			File.Delete(path);
		}
		catch (Exception)
		{
		}
		lblStatus.Text = "Download fehlgeschlagen.";
		MessageBox.Show("Die Datei konnte nicht geladen werden (Fehlercode: #5)\nSchließe bitte dein Arma3\n" + Administrationtxt + "\n(URL:" + downloadPath + "/" + newestVersion + ")", "Kann die Datei nicht herunterladen", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		Close();
	}

	private void CompletedUpdater(object sender, AsyncCompletedEventArgs e)
	{
		pgbDownload.Value = 100;
		if (e.Error == null)
		{
			Thread.Sleep(1000);
			Close();
			return;
		}
		string path = localArma + "\\" + newestVersion;
		try
		{
			File.Delete(path);
		}
		catch (Exception)
		{
		}
		lblStatus.Text = "Download fehlgeschlagen.";
		MessageBox.Show("Die Dateien konnte nicht heruntergeladen werden (Fehlercode: #6)\n" + Administrationtxt + "\n(URL:" + downloadPath + "/" + newestVersion + ")", "Kann die Datei nicht herunterladen :C", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		Close();
	}

	private void CompletedTexture(object sender, AsyncCompletedEventArgs e)
	{
		pgbDownload.Value = 100;
		if (e.Error == null)
		{
			string sourceArchiveFileName = downloadPathTexture;
			string destinationDirectoryName = armaPath;
			ZipFile.ExtractToDirectory(sourceArchiveFileName, destinationDirectoryName);
			if (showDownloadSuccess)
			{
				Thread.Sleep(1000);
				Close();
			}
			return;
		}
		downloadPathTexture = localArma + "\\" + newestVersion;
		try
		{
			File.Delete(downloadPathTexture);
		}
		catch (Exception)
		{
		}
		lblStatus.Text = "Download fehlgeschlagen.";
		MessageBox.Show(Fehlercode7 + Administrationtxt + "(URL:" + downloadPath + "/" + newestVersion + ")", "Kann die Datei nicht herunterladen", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		Close();
	}

	private void Download_Load(object sender, EventArgs e)
	{
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
		this.label1 = new System.Windows.Forms.Label();
		this.lblVersion = new System.Windows.Forms.Label();
		this.pgbDownload = new System.Windows.Forms.ProgressBar();
		this.lblStatus = new System.Windows.Forms.Label();
		this.pictureBox1 = new System.Windows.Forms.PictureBox();
		this.pictureBox2 = new System.Windows.Forms.PictureBox();
		((System.ComponentModel.ISupportInitialize)this.pictureBox1).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.pictureBox2).BeginInit();
		base.SuspendLayout();
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
		this.label1.Location = new System.Drawing.Point(38, 9);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(236, 15);
		this.label1.TabIndex = 0;
		this.label1.Text = "Eine neue Version wird heruntergeladen...";
		this.lblVersion.AutoSize = true;
		this.lblVersion.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
		this.lblVersion.Location = new System.Drawing.Point(10, 36);
		this.lblVersion.Name = "lblVersion";
		this.lblVersion.Size = new System.Drawing.Size(55, 13);
		this.lblVersion.TabIndex = 1;
		this.lblVersion.Text = "VERSION";
		this.pgbDownload.Location = new System.Drawing.Point(12, 53);
		this.pgbDownload.Name = "pgbDownload";
		this.pgbDownload.Size = new System.Drawing.Size(283, 22);
		this.pgbDownload.TabIndex = 2;
		this.lblStatus.AutoSize = true;
		this.lblStatus.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
		this.lblStatus.Location = new System.Drawing.Point(10, 82);
		this.lblStatus.Name = "lblStatus";
		this.lblStatus.Size = new System.Drawing.Size(50, 13);
		this.lblStatus.TabIndex = 3;
		this.lblStatus.Text = "STATUS";
		this.pictureBox1.Image = USALauncher.Properties.Resources._4501_200;
		this.pictureBox1.Location = new System.Drawing.Point(16, 10);
		this.pictureBox1.Name = "pictureBox1";
		this.pictureBox1.Size = new System.Drawing.Size(16, 16);
		this.pictureBox1.TabIndex = 4;
		this.pictureBox1.TabStop = false;
		this.pictureBox2.Image = USALauncher.Properties.Resources._4501_200;
		this.pictureBox2.Location = new System.Drawing.Point(278, 10);
		this.pictureBox2.Name = "pictureBox2";
		this.pictureBox2.Size = new System.Drawing.Size(16, 16);
		this.pictureBox2.TabIndex = 5;
		this.pictureBox2.TabStop = false;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.BackColor = System.Drawing.Color.FromArgb(64, 64, 64);
		base.ClientSize = new System.Drawing.Size(307, 100);
		base.ControlBox = false;
		base.Controls.Add(this.pictureBox2);
		base.Controls.Add(this.pictureBox1);
		base.Controls.Add(this.lblStatus);
		base.Controls.Add(this.pgbDownload);
		base.Controls.Add(this.lblVersion);
		base.Controls.Add(this.label1);
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
		base.MaximizeBox = false;
		base.MinimizeBox = false;
		base.Name = "Download";
		this.Text = "Download";
		base.Load += new System.EventHandler(Download_Load);
		((System.ComponentModel.ISupportInitialize)this.pictureBox1).EndInit();
		((System.ComponentModel.ISupportInitialize)this.pictureBox2).EndInit();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
