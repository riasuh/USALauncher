using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace USALauncher;

public class ProfilePathSelection : Form
{
	private string defaultprofilepath = Environment.GetEnvironmentVariable("userprofile") + "\\Documents\\Arma 3 - Other Profiles";

	private bool success;

	private IContainer components;

	private TextBox txtPath;

	private Button btnSelectPath;

	private Label lblPathText;

	private PictureBox pictureBox1;

	private FolderBrowserDialog fbdprofilePath;

	private Label lblErkannt;

	private Button btnWeiter;

	private Label label1;

	public ProfilePathSelection()
	{
		InitializeComponent();
		base.Icon = USALauncher.Properties.Resources.LOGO01_snowwhite_PNG;
		base.Closing += DataWindow_Closing;
		CenterToScreen();
		if (!string.IsNullOrEmpty((string)Settings.Default["profilePath"]))
		{
			fbdprofilePath.SelectedPath = (string)Settings.Default["profilePath"];
			txtPath.Text = (string)Settings.Default["profilePath"];
		}
		else if (Directory.Exists(defaultprofilepath))
		{
			fbdprofilePath.SelectedPath = defaultprofilepath;
			txtPath.Text = defaultprofilepath;
			lblErkannt.Visible = true;
		}
		else
		{
			fbdprofilePath.SelectedPath = "C:\\";
			lblErkannt.Visible = false;
		}
	}

	private void btnSelectPath_Click(object sender, EventArgs e)
	{
		fbdprofilePath.SelectedPath = txtPath.Text;
		if (fbdprofilePath.ShowDialog() == DialogResult.OK)
		{
			txtPath.Text = fbdprofilePath.SelectedPath;
		}
	}

	private void DataWindow_Closing(object sender, CancelEventArgs e)
	{
		if (!success)
		{
			Application.Exit();
		}
	}

	private bool isValidProfileDirectory(string directory)
	{
		if (txtPath.ToString().EndsWith("Arma 3 - Other Profiles"))
		{
			txtPath.Text = fbdprofilePath.SelectedPath;
			return true;
		}
		return false;
	}

	private void btnWeiter_Click(object sender, EventArgs e)
	{
		success = true;
		Settings.Default.profilePath = fbdprofilePath.SelectedPath;
		Settings.Default.Save();
		Settings.Default.Reload();
		Close();
	}

	private void btnWeiter_Click_1(object sender, EventArgs e)
	{
		txtPath.ToString();
		success = true;
		Settings.Default.profilePath = fbdprofilePath.SelectedPath;
		Settings.Default.Save();
		Settings.Default.Reload();
		Close();
	}

	private void btnSelectPath_Click_1(object sender, EventArgs e)
	{
		fbdprofilePath.SelectedPath = txtPath.Text;
		if (fbdprofilePath.ShowDialog() == DialogResult.OK)
		{
			txtPath.Text = fbdprofilePath.SelectedPath;
		}
	}

	private void txtPath_TextChanged(object sender, EventArgs e)
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
		this.txtPath = new System.Windows.Forms.TextBox();
		this.btnSelectPath = new System.Windows.Forms.Button();
		this.lblPathText = new System.Windows.Forms.Label();
		this.pictureBox1 = new System.Windows.Forms.PictureBox();
		this.fbdprofilePath = new System.Windows.Forms.FolderBrowserDialog();
		this.lblErkannt = new System.Windows.Forms.Label();
		this.btnWeiter = new System.Windows.Forms.Button();
		this.label1 = new System.Windows.Forms.Label();
		((System.ComponentModel.ISupportInitialize)this.pictureBox1).BeginInit();
		base.SuspendLayout();
		this.txtPath.BackColor = System.Drawing.Color.FromArgb(27, 28, 33);
		this.txtPath.Font = new System.Drawing.Font("Bahnschrift SemiLight", 8f);
		this.txtPath.ForeColor = System.Drawing.Color.White;
		this.txtPath.Location = new System.Drawing.Point(8, 318);
		this.txtPath.Name = "txtPath";
		this.txtPath.Size = new System.Drawing.Size(266, 20);
		this.txtPath.TabIndex = 8;
		this.txtPath.TextChanged += new System.EventHandler(txtPath_TextChanged);
		this.btnSelectPath.Font = new System.Drawing.Font("Bahnschrift SemiLight", 9f);
		this.btnSelectPath.Location = new System.Drawing.Point(279, 318);
		this.btnSelectPath.Name = "btnSelectPath";
		this.btnSelectPath.Size = new System.Drawing.Size(28, 20);
		this.btnSelectPath.TabIndex = 7;
		this.btnSelectPath.Text = "...";
		this.btnSelectPath.UseVisualStyleBackColor = true;
		this.btnSelectPath.Click += new System.EventHandler(btnSelectPath_Click_1);
		this.lblPathText.AutoSize = true;
		this.lblPathText.BackColor = System.Drawing.Color.Transparent;
		this.lblPathText.Font = new System.Drawing.Font("Bahnschrift SemiLight", 10f);
		this.lblPathText.ForeColor = System.Drawing.Color.White;
		this.lblPathText.Location = new System.Drawing.Point(30, 263);
		this.lblPathText.Name = "lblPathText";
		this.lblPathText.Size = new System.Drawing.Size(258, 17);
		this.lblPathText.TabIndex = 6;
		this.lblPathText.Text = "Wähle bitte deinen Arma 3 Profilpfad aus";
		this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
		this.pictureBox1.BackgroundImage = USALauncher.Properties.Resources.LOGO01_snowwhite_PNG___Kopie;
		this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
		this.pictureBox1.Location = new System.Drawing.Point(29, 12);
		this.pictureBox1.Name = "pictureBox1";
		this.pictureBox1.Size = new System.Drawing.Size(262, 268);
		this.pictureBox1.TabIndex = 10;
		this.pictureBox1.TabStop = false;
		this.lblErkannt.AutoSize = true;
		this.lblErkannt.BackColor = System.Drawing.Color.Transparent;
		this.lblErkannt.Font = new System.Drawing.Font("Bahnschrift SemiLight", 10f);
		this.lblErkannt.ForeColor = System.Drawing.Color.Lime;
		this.lblErkannt.Location = new System.Drawing.Point(6, 301);
		this.lblErkannt.Name = "lblErkannt";
		this.lblErkannt.Size = new System.Drawing.Size(204, 17);
		this.lblErkannt.TabIndex = 11;
		this.lblErkannt.Text = "Profilpfad automatisch erkannt!";
		this.btnWeiter.Font = new System.Drawing.Font("Bahnschrift SemiLight", 8.5f);
		this.btnWeiter.Location = new System.Drawing.Point(232, 343);
		this.btnWeiter.Name = "btnWeiter";
		this.btnWeiter.Size = new System.Drawing.Size(75, 23);
		this.btnWeiter.TabIndex = 9;
		this.btnWeiter.Text = "Speichern";
		this.btnWeiter.UseVisualStyleBackColor = true;
		this.btnWeiter.Click += new System.EventHandler(btnWeiter_Click_1);
		this.label1.AutoSize = true;
		this.label1.BackColor = System.Drawing.Color.Transparent;
		this.label1.Font = new System.Drawing.Font("Bahnschrift SemiLight", 10f);
		this.label1.ForeColor = System.Drawing.Color.White;
		this.label1.Location = new System.Drawing.Point(20, 280);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(278, 17);
		this.label1.TabIndex = 12;
		this.label1.Text = "(Wird normalerweise automatisch erkannt)";
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.BackgroundImage = USALauncher.Properties.Resources.background_14_5;
		base.ClientSize = new System.Drawing.Size(321, 372);
		base.Controls.Add(this.label1);
		base.Controls.Add(this.txtPath);
		base.Controls.Add(this.btnSelectPath);
		base.Controls.Add(this.lblPathText);
		base.Controls.Add(this.pictureBox1);
		base.Controls.Add(this.lblErkannt);
		base.Controls.Add(this.btnWeiter);
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
		base.MaximizeBox = false;
		base.Name = "ProfilePathSelection";
		this.Text = "USA-LIFE Launcher - Profilpfad wählen";
		((System.ComponentModel.ISupportInitialize)this.pictureBox1).EndInit();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
