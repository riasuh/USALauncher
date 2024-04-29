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

	private PictureBox picLogo;

	private Label lblErkannt;

	private Label labelpathtext2;

	private Label label1;

	private Label label2;

	public PathSelection()
	{
		InitializeComponent();
		base.Icon = USALauncher.Properties.Resources.Icon_1_USA_128;
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
			MessageBox.Show("Du hast keinen validen Pfad ausgewählt, oder hast Arma nicht richtig installiert. Melde dich bei weiteren Problemen im Discord unter https://discord.gg/usaliferpg.", "Pfad nicht gefunden", MessageBoxButtons.OK, MessageBoxIcon.Hand);
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
            this.picLogo = new System.Windows.Forms.PictureBox();
            this.lblErkannt = new System.Windows.Forms.Label();
            this.labelpathtext2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.picLogo)).BeginInit();
            this.SuspendLayout();
            // 
            // lblPathText
            // 
            this.lblPathText.AutoSize = true;
            this.lblPathText.BackColor = System.Drawing.Color.Transparent;
            this.lblPathText.Font = new System.Drawing.Font("Bahnschrift SemiLight", 9.5F);
            this.lblPathText.ForeColor = System.Drawing.Color.White;
            this.lblPathText.Location = new System.Drawing.Point(6, 222);
            this.lblPathText.Name = "lblPathText";
            this.lblPathText.Size = new System.Drawing.Size(275, 32);
            this.lblPathText.TabIndex = 0;
            this.lblPathText.Text = "Schön das du den USA-LIFE Launcher benutzt. \r\nDu hilfst uns damit, den Server zu " +
    "schonen.\r\n";
            // 
            // btnSelectPath
            // 
            this.btnSelectPath.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnSelectPath.Font = new System.Drawing.Font("Bahnschrift SemiLight", 9F);
            this.btnSelectPath.Location = new System.Drawing.Point(279, 318);
            this.btnSelectPath.Name = "btnSelectPath";
            this.btnSelectPath.Size = new System.Drawing.Size(28, 20);
            this.btnSelectPath.TabIndex = 1;
            this.btnSelectPath.Text = "...";
            this.btnSelectPath.UseVisualStyleBackColor = true;
            this.btnSelectPath.Click += new System.EventHandler(this.btnSelectPath_Click);
            // 
            // txtPath
            // 
            this.txtPath.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(27)))), ((int)(((byte)(28)))), ((int)(((byte)(33)))));
            this.txtPath.Font = new System.Drawing.Font("Bahnschrift SemiLight", 8F);
            this.txtPath.ForeColor = System.Drawing.Color.White;
            this.txtPath.Location = new System.Drawing.Point(8, 318);
            this.txtPath.Name = "txtPath";
            this.txtPath.Size = new System.Drawing.Size(266, 20);
            this.txtPath.TabIndex = 2;
            // 
            // fbdSelectPath
            // 
            this.fbdSelectPath.HelpRequest += new System.EventHandler(this.fbdSelectPath_HelpRequest);
            // 
            // btnWeiter
            // 
            this.btnWeiter.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnWeiter.FlatAppearance.BorderSize = 0;
            this.btnWeiter.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnWeiter.Font = new System.Drawing.Font("Bahnschrift SemiLight", 8.5F);
            this.btnWeiter.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnWeiter.Location = new System.Drawing.Point(232, 343);
            this.btnWeiter.Name = "btnWeiter";
            this.btnWeiter.Size = new System.Drawing.Size(75, 23);
            this.btnWeiter.TabIndex = 3;
            this.btnWeiter.Text = "Speichern";
            this.btnWeiter.UseVisualStyleBackColor = true;
            this.btnWeiter.Click += new System.EventHandler(this.btnWeiter_Click);
            // 
            // picLogo
            // 
            this.picLogo.BackColor = System.Drawing.Color.Transparent;
            this.picLogo.BackgroundImage = global::USALauncher.Properties.Resources.Logo_3_USAFull;
            this.picLogo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.picLogo.Location = new System.Drawing.Point(35, 13);
            this.picLogo.Name = "picLogo";
            this.picLogo.Size = new System.Drawing.Size(250, 201);
            this.picLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picLogo.TabIndex = 4;
            this.picLogo.TabStop = false;
            // 
            // lblErkannt
            // 
            this.lblErkannt.AutoSize = true;
            this.lblErkannt.BackColor = System.Drawing.Color.Transparent;
            this.lblErkannt.Font = new System.Drawing.Font("Bahnschrift SemiLight", 9.7F);
            this.lblErkannt.ForeColor = System.Drawing.Color.Lime;
            this.lblErkannt.Location = new System.Drawing.Point(6, 301);
            this.lblErkannt.Name = "lblErkannt";
            this.lblErkannt.Size = new System.Drawing.Size(293, 16);
            this.lblErkannt.TabIndex = 5;
            this.lblErkannt.Text = "Arma3 Standardverzeichnis automatisch erkannt!";
            // 
            // labelpathtext2
            // 
            this.labelpathtext2.AutoSize = true;
            this.labelpathtext2.BackColor = System.Drawing.Color.Transparent;
            this.labelpathtext2.Font = new System.Drawing.Font("Bahnschrift SemiLight", 9.5F);
            this.labelpathtext2.ForeColor = System.Drawing.Color.White;
            this.labelpathtext2.Location = new System.Drawing.Point(6, 205);
            this.labelpathtext2.Name = "labelpathtext2";
            this.labelpathtext2.Size = new System.Drawing.Size(42, 16);
            this.labelpathtext2.TabIndex = 6;
            this.labelpathtext2.Text = "Hallo!";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Bahnschrift SemiLight", 9.5F);
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(6, 268);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(311, 16);
            this.label1.TabIndex = 7;
            this.label1.Text = "Wähle bitte dein Arma3-Installationsverzeichnis aus.";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Bahnschrift SemiLight", 9.5F);
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(5, 283);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(243, 16);
            this.label2.TabIndex = 8;
            this.label2.Text = "(Wenn es nicht automatisch erkannt wird)";
            // 
            // PathSelection
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(56)))), ((int)(((byte)(73)))));
            this.ClientSize = new System.Drawing.Size(321, 372);
            this.ControlBox = false;
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.labelpathtext2);
            this.Controls.Add(this.lblErkannt);
            this.Controls.Add(this.btnWeiter);
            this.Controls.Add(this.txtPath);
            this.Controls.Add(this.btnSelectPath);
            this.Controls.Add(this.lblPathText);
            this.Controls.Add(this.picLogo);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PathSelection";
            this.Text = "USA-LIFE Launcher - Pfad wählen";
            ((System.ComponentModel.ISupportInitialize)(this.picLogo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

	}
}
