using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace USALauncher;

public class PathSelection : Form
{
	private string defaultPath = "C:\\Program Files (x86)\\Steam\\steamapps\\common\\Arma 3";

	private string localPath = Environment.GetEnvironmentVariable("localappdata") + "\\Arma 3\\MPMissionsCache";

	private bool success;

	private IContainer components;

	private Label lblPathText;

	private Button btnSelectPath;

	private TextBox txtPath;

	private FolderBrowserDialog fbdSelectPath;

	private Button btnWeiter;

	private PictureBox pictureBox1;

	private Label lblErkannt;

	private Label labelpathtext2;

	private Label label1;

	private Label label2;

	public PathSelection()
	{
		InitializeComponent();
		base.Icon = USALauncher.Properties.Resources.LOGO01_snowwhite_PNG;
		base.Closing += DataWindow_Closing;
		CenterToScreen();
		if (!string.IsNullOrEmpty((string)Settings.Default["armaPath"]))
		{
			fbdSelectPath.SelectedPath = (string)Settings.Default["armaPath"];
			txtPath.Text = (string)Settings.Default["armaPath"];
		}
		else if (Directory.Exists(defaultPath))
		{
			fbdSelectPath.SelectedPath = defaultPath;
			txtPath.Text = defaultPath;
			lblErkannt.Visible = true;
		}
		else
		{
			fbdSelectPath.SelectedPath = "C:\\";
			lblErkannt.Visible = false;
		}
	}

	private void btnSelectPath_Click(object sender, EventArgs e)
	{
		fbdSelectPath.SelectedPath = txtPath.Text;
		if (fbdSelectPath.ShowDialog() == DialogResult.OK)
		{
			txtPath.Text = fbdSelectPath.SelectedPath;
		}
	}

	private void DataWindow_Closing(object sender, CancelEventArgs e)
	{
		if (!success)
		{
			Application.Exit();
		}
	}

	private bool isValidArmaDirectory(string directory)
	{
		if (!Directory.Exists(localPath))
		{
			Directory.CreateDirectory(localPath);
		}
		if (Directory.Exists(directory) && File.Exists(directory + "\\arma3.exe") && Directory.Exists(localPath))
		{
			return true;
		}
		return false;
	}

	private void btnWeiter_Click(object sender, EventArgs e)
	{
		if (isValidArmaDirectory(txtPath.Text))
		{
			success = true;
			Settings.Default.armaPath = fbdSelectPath.SelectedPath;
			Settings.Default.Save();
			Settings.Default.Reload();
			Close();
		}
		else
		{
			MessageBox.Show("Du hast keinen validen Pfad ausgewählt, oder hast Arma nicht richtig installiert. Melde dich bei weiteren Problemen im Forum unter https://forum.usa-life.net.", "Pfad nicht gefunden", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void fbdSelectPath_HelpRequest(object sender, EventArgs e)
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
		this.lblPathText = new System.Windows.Forms.Label();
		this.btnSelectPath = new System.Windows.Forms.Button();
		this.txtPath = new System.Windows.Forms.TextBox();
		this.fbdSelectPath = new System.Windows.Forms.FolderBrowserDialog();
		this.btnWeiter = new System.Windows.Forms.Button();
		this.pictureBox1 = new System.Windows.Forms.PictureBox();
		this.lblErkannt = new System.Windows.Forms.Label();
		this.labelpathtext2 = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		((System.ComponentModel.ISupportInitialize)this.pictureBox1).BeginInit();
		base.SuspendLayout();
		this.lblPathText.AutoSize = true;
		this.lblPathText.BackColor = System.Drawing.Color.Transparent;
		this.lblPathText.Font = new System.Drawing.Font("Bahnschrift SemiLight", 9.5f);
		this.lblPathText.ForeColor = System.Drawing.Color.White;
		this.lblPathText.Location = new System.Drawing.Point(6, 222);
		this.lblPathText.Name = "lblPathText";
		this.lblPathText.Size = new System.Drawing.Size(269, 32);
		this.lblPathText.TabIndex = 0;
		this.lblPathText.Text = "Schön das du den USA-LIFE Launcher benutzt. \r\nDu hilfst uns damit, den Server zu schonen.\r\n";
		this.btnSelectPath.Font = new System.Drawing.Font("Bahnschrift SemiLight", 9f);
		this.btnSelectPath.Location = new System.Drawing.Point(279, 318);
		this.btnSelectPath.Name = "btnSelectPath";
		this.btnSelectPath.Size = new System.Drawing.Size(28, 20);
		this.btnSelectPath.TabIndex = 1;
		this.btnSelectPath.Text = "...";
		this.btnSelectPath.UseVisualStyleBackColor = true;
		this.btnSelectPath.Click += new System.EventHandler(btnSelectPath_Click);
		this.txtPath.BackColor = System.Drawing.Color.FromArgb(27, 28, 33);
		this.txtPath.Font = new System.Drawing.Font("Bahnschrift SemiLight", 8f);
		this.txtPath.ForeColor = System.Drawing.Color.White;
		this.txtPath.Location = new System.Drawing.Point(8, 318);
		this.txtPath.Name = "txtPath";
		this.txtPath.Size = new System.Drawing.Size(266, 20);
		this.txtPath.TabIndex = 2;
		this.fbdSelectPath.HelpRequest += new System.EventHandler(fbdSelectPath_HelpRequest);
		this.btnWeiter.Font = new System.Drawing.Font("Bahnschrift SemiLight", 8.5f);
		this.btnWeiter.Location = new System.Drawing.Point(232, 343);
		this.btnWeiter.Name = "btnWeiter";
		this.btnWeiter.Size = new System.Drawing.Size(75, 23);
		this.btnWeiter.TabIndex = 3;
		this.btnWeiter.Text = "Speichern";
		this.btnWeiter.UseVisualStyleBackColor = true;
		this.btnWeiter.Click += new System.EventHandler(btnWeiter_Click);
		this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
		this.pictureBox1.BackgroundImage = USALauncher.Properties.Resources.LOGO01_snowwhite_PNG___Kopie;
		this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
		this.pictureBox1.Location = new System.Drawing.Point(27, -29);
		this.pictureBox1.Name = "pictureBox1";
		this.pictureBox1.Size = new System.Drawing.Size(262, 268);
		this.pictureBox1.TabIndex = 4;
		this.pictureBox1.TabStop = false;
		this.lblErkannt.AutoSize = true;
		this.lblErkannt.BackColor = System.Drawing.Color.Transparent;
		this.lblErkannt.Font = new System.Drawing.Font("Bahnschrift SemiLight", 9.7f);
		this.lblErkannt.ForeColor = System.Drawing.Color.Lime;
		this.lblErkannt.Location = new System.Drawing.Point(6, 301);
		this.lblErkannt.Name = "lblErkannt";
		this.lblErkannt.Size = new System.Drawing.Size(290, 16);
		this.lblErkannt.TabIndex = 5;
		this.lblErkannt.Text = "Arma3 Standardverzeichnis automatisch erkannt!";
		this.labelpathtext2.AutoSize = true;
		this.labelpathtext2.BackColor = System.Drawing.Color.Transparent;
		this.labelpathtext2.Font = new System.Drawing.Font("Bahnschrift SemiLight", 9.5f);
		this.labelpathtext2.ForeColor = System.Drawing.Color.White;
		this.labelpathtext2.Location = new System.Drawing.Point(6, 205);
		this.labelpathtext2.Name = "labelpathtext2";
		this.labelpathtext2.Size = new System.Drawing.Size(42, 16);
		this.labelpathtext2.TabIndex = 6;
		this.labelpathtext2.Text = "Hallo!";
		this.label1.AutoSize = true;
		this.label1.BackColor = System.Drawing.Color.Transparent;
		this.label1.Font = new System.Drawing.Font("Bahnschrift SemiLight", 9.5f);
		this.label1.ForeColor = System.Drawing.Color.White;
		this.label1.Location = new System.Drawing.Point(6, 268);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(308, 16);
		this.label1.TabIndex = 7;
		this.label1.Text = "Wähle bitte dein Arma3-Installationsverzeichnis aus.";
		this.label2.AutoSize = true;
		this.label2.BackColor = System.Drawing.Color.Transparent;
		this.label2.Font = new System.Drawing.Font("Bahnschrift SemiLight", 9.5f);
		this.label2.ForeColor = System.Drawing.Color.White;
		this.label2.Location = new System.Drawing.Point(5, 283);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(239, 16);
		this.label2.TabIndex = 8;
		this.label2.Text = "(Wenn es nicht automatisch erkannt wird)";
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.BackgroundImage = USALauncher.Properties.Resources.background_14_5;
		base.ClientSize = new System.Drawing.Size(321, 372);
		base.ControlBox = false;
		base.Controls.Add(this.label2);
		base.Controls.Add(this.label1);
		base.Controls.Add(this.labelpathtext2);
		base.Controls.Add(this.lblErkannt);
		base.Controls.Add(this.btnWeiter);
		base.Controls.Add(this.txtPath);
		base.Controls.Add(this.btnSelectPath);
		base.Controls.Add(this.lblPathText);
		base.Controls.Add(this.pictureBox1);
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
		base.MaximizeBox = false;
		base.MinimizeBox = false;
		base.Name = "PathSelection";
		this.Text = "USA-LIFE Launcher - Pfad wählen";
		((System.ComponentModel.ISupportInitialize)this.pictureBox1).EndInit();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
