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

	private PictureBox picLogo;

	private FolderBrowserDialog fbdprofilePath;

	private Label lblErkannt;

	private Button btnWeiter;

	private Label label1;

	public ProfilePathSelection()
	{
		InitializeComponent();
		base.Icon = USALauncher.Properties.Resources.Icon_1_USA_128;
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
            this.picLogo = new System.Windows.Forms.PictureBox();
            this.fbdprofilePath = new System.Windows.Forms.FolderBrowserDialog();
            this.lblErkannt = new System.Windows.Forms.Label();
            this.btnWeiter = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.picLogo)).BeginInit();
            this.SuspendLayout();
            // 
            // txtPath
            // 
            this.txtPath.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(27)))), ((int)(((byte)(28)))), ((int)(((byte)(33)))));
            this.txtPath.Font = new System.Drawing.Font("Bahnschrift SemiLight", 8F);
            this.txtPath.ForeColor = System.Drawing.Color.White;
            this.txtPath.Location = new System.Drawing.Point(8, 318);
            this.txtPath.Name = "txtPath";
            this.txtPath.Size = new System.Drawing.Size(266, 20);
            this.txtPath.TabIndex = 8;
            this.txtPath.TextChanged += new System.EventHandler(this.txtPath_TextChanged);
            // 
            // btnSelectPath
            // 
            this.btnSelectPath.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnSelectPath.Font = new System.Drawing.Font("Bahnschrift SemiLight", 9F);
            this.btnSelectPath.Location = new System.Drawing.Point(279, 318);
            this.btnSelectPath.Name = "btnSelectPath";
            this.btnSelectPath.Size = new System.Drawing.Size(28, 20);
            this.btnSelectPath.TabIndex = 7;
            this.btnSelectPath.Text = "...";
            this.btnSelectPath.UseVisualStyleBackColor = true;
            this.btnSelectPath.Click += new System.EventHandler(this.btnSelectPath_Click_1);
            // 
            // lblPathText
            // 
            this.lblPathText.AutoSize = true;
            this.lblPathText.BackColor = System.Drawing.Color.Transparent;
            this.lblPathText.Font = new System.Drawing.Font("Bahnschrift SemiLight", 10F);
            this.lblPathText.ForeColor = System.Drawing.Color.White;
            this.lblPathText.Location = new System.Drawing.Point(30, 260);
            this.lblPathText.Name = "lblPathText";
            this.lblPathText.Size = new System.Drawing.Size(270, 17);
            this.lblPathText.TabIndex = 6;
            this.lblPathText.Text = "Wähle bitte deinen Arma 3 Profilpfad aus";
            // 
            // picLogo
            // 
            this.picLogo.BackColor = System.Drawing.Color.Transparent;
            this.picLogo.BackgroundImage = global::USALauncher.Properties.Resources.Logo_3_USAFull;
            this.picLogo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.picLogo.Location = new System.Drawing.Point(35, 13);
            this.picLogo.Name = "picLogo";
            this.picLogo.Size = new System.Drawing.Size(250, 201);
            this.picLogo.TabIndex = 10;
            this.picLogo.TabStop = false;
            // 
            // lblErkannt
            // 
            this.lblErkannt.AutoSize = true;
            this.lblErkannt.BackColor = System.Drawing.Color.Transparent;
            this.lblErkannt.Font = new System.Drawing.Font("Bahnschrift SemiLight", 10F);
            this.lblErkannt.ForeColor = System.Drawing.Color.Lime;
            this.lblErkannt.Location = new System.Drawing.Point(6, 301);
            this.lblErkannt.Name = "lblErkannt";
            this.lblErkannt.Size = new System.Drawing.Size(209, 17);
            this.lblErkannt.TabIndex = 11;
            this.lblErkannt.Text = "Profilpfad automatisch erkannt!";
            // 
            // btnWeiter
            // 
            this.btnWeiter.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnWeiter.Font = new System.Drawing.Font("Bahnschrift SemiLight", 8.5F);
            this.btnWeiter.Location = new System.Drawing.Point(232, 343);
            this.btnWeiter.Name = "btnWeiter";
            this.btnWeiter.Size = new System.Drawing.Size(75, 23);
            this.btnWeiter.TabIndex = 9;
            this.btnWeiter.Text = "Speichern";
            this.btnWeiter.UseVisualStyleBackColor = true;
            this.btnWeiter.Click += new System.EventHandler(this.btnWeiter_Click_1);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Bahnschrift SemiLight", 10F);
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(20, 277);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(284, 17);
            this.label1.TabIndex = 12;
            this.label1.Text = "(Wird normalerweise automatisch erkannt)";
            // 
            // ProfilePathSelection
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(56)))), ((int)(((byte)(73)))));
            this.ClientSize = new System.Drawing.Size(321, 372);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtPath);
            this.Controls.Add(this.btnSelectPath);
            this.Controls.Add(this.lblPathText);
            this.Controls.Add(this.picLogo);
            this.Controls.Add(this.lblErkannt);
            this.Controls.Add(this.btnWeiter);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "ProfilePathSelection";
            this.Text = "USA-LIFE Launcher - Profilpfad wählen";
            ((System.ComponentModel.ISupportInitialize)(this.picLogo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

	}
}
