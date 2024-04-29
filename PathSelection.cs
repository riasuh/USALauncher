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
        lblPathText = new Label();
        btnSelectPath = new Button();
        txtPath = new TextBox();
        fbdSelectPath = new FolderBrowserDialog();
        btnWeiter = new Button();
        picLogo = new PictureBox();
        lblErkannt = new Label();
        labelpathtext2 = new Label();
        label1 = new Label();
        label2 = new Label();
        ((ISupportInitialize)picLogo).BeginInit();
        SuspendLayout();
        // 
        // lblPathText
        // 
        lblPathText.AutoSize = true;
        lblPathText.BackColor = Color.Transparent;
        lblPathText.Font = new Font("Bahnschrift SemiLight", 9.5F);
        lblPathText.ForeColor = Color.White;
        lblPathText.Location = new Point(7, 256);
        lblPathText.Margin = new Padding(4, 0, 4, 0);
        lblPathText.Name = "lblPathText";
        lblPathText.Size = new Size(275, 32);
        lblPathText.TabIndex = 0;
        lblPathText.Text = "Schön das du den USA-LIFE Launcher benutzt. \r\nDu hilfst uns damit, den Server zu schonen.\r\n";
        // 
        // btnSelectPath
        // 
        btnSelectPath.FlatStyle = FlatStyle.System;
        btnSelectPath.Font = new Font("Bahnschrift SemiLight", 9F);
        btnSelectPath.Location = new Point(326, 367);
        btnSelectPath.Margin = new Padding(4, 3, 4, 3);
        btnSelectPath.Name = "btnSelectPath";
        btnSelectPath.Size = new Size(33, 23);
        btnSelectPath.TabIndex = 1;
        btnSelectPath.Text = "...";
        btnSelectPath.UseVisualStyleBackColor = true;
        btnSelectPath.Click += btnSelectPath_Click;
        // 
        // txtPath
        // 
        txtPath.BackColor = Color.FromArgb(27, 28, 33);
        txtPath.Font = new Font("Bahnschrift SemiLight", 8F);
        txtPath.ForeColor = Color.White;
        txtPath.Location = new Point(9, 367);
        txtPath.Margin = new Padding(4, 3, 4, 3);
        txtPath.Name = "txtPath";
        txtPath.Size = new Size(310, 20);
        txtPath.TabIndex = 2;
        // 
        // fbdSelectPath
        // 
        fbdSelectPath.HelpRequest += fbdSelectPath_HelpRequest;
        // 
        // btnWeiter
        // 
        btnWeiter.Cursor = Cursors.Hand;
        btnWeiter.FlatAppearance.BorderSize = 0;
        btnWeiter.FlatStyle = FlatStyle.System;
        btnWeiter.Font = new Font("Bahnschrift SemiLight", 8.5F);
        btnWeiter.ForeColor = SystemColors.ControlText;
        btnWeiter.Location = new Point(271, 396);
        btnWeiter.Margin = new Padding(4, 3, 4, 3);
        btnWeiter.Name = "btnWeiter";
        btnWeiter.Size = new Size(88, 27);
        btnWeiter.TabIndex = 3;
        btnWeiter.Text = "Speichern";
        btnWeiter.UseVisualStyleBackColor = true;
        btnWeiter.Click += btnWeiter_Click;
        // 
        // picLogo
        // 
        picLogo.BackColor = Color.Transparent;
        picLogo.BackgroundImage = Properties.Resources.Logo_3_USAFull;
        picLogo.BackgroundImageLayout = ImageLayout.Zoom;
        picLogo.Location = new Point(41, 15);
        picLogo.Margin = new Padding(4, 3, 4, 3);
        picLogo.Name = "picLogo";
        picLogo.Size = new Size(292, 232);
        picLogo.SizeMode = PictureBoxSizeMode.Zoom;
        picLogo.TabIndex = 4;
        picLogo.TabStop = false;
        // 
        // lblErkannt
        // 
        lblErkannt.AutoSize = true;
        lblErkannt.BackColor = Color.Transparent;
        lblErkannt.Font = new Font("Bahnschrift SemiLight", 9.7F);
        lblErkannt.ForeColor = Color.Lime;
        lblErkannt.Location = new Point(7, 347);
        lblErkannt.Margin = new Padding(4, 0, 4, 0);
        lblErkannt.Name = "lblErkannt";
        lblErkannt.Size = new Size(293, 16);
        lblErkannt.TabIndex = 5;
        lblErkannt.Text = "Arma3 Standardverzeichnis automatisch erkannt!";
        // 
        // labelpathtext2
        // 
        labelpathtext2.AutoSize = true;
        labelpathtext2.BackColor = Color.Transparent;
        labelpathtext2.Font = new Font("Bahnschrift SemiLight", 9.5F);
        labelpathtext2.ForeColor = Color.White;
        labelpathtext2.Location = new Point(7, 237);
        labelpathtext2.Margin = new Padding(4, 0, 4, 0);
        labelpathtext2.Name = "labelpathtext2";
        labelpathtext2.Size = new Size(42, 16);
        labelpathtext2.TabIndex = 6;
        labelpathtext2.Text = "Hallo!";
        // 
        // label1
        // 
        label1.AutoSize = true;
        label1.BackColor = Color.Transparent;
        label1.Font = new Font("Bahnschrift SemiLight", 9.5F);
        label1.ForeColor = Color.White;
        label1.Location = new Point(7, 309);
        label1.Margin = new Padding(4, 0, 4, 0);
        label1.Name = "label1";
        label1.Size = new Size(311, 16);
        label1.TabIndex = 7;
        label1.Text = "Wähle bitte dein Arma3-Installationsverzeichnis aus.";
        // 
        // label2
        // 
        label2.AutoSize = true;
        label2.BackColor = Color.Transparent;
        label2.Font = new Font("Bahnschrift SemiLight", 9.5F);
        label2.ForeColor = Color.White;
        label2.Location = new Point(6, 327);
        label2.Margin = new Padding(4, 0, 4, 0);
        label2.Name = "label2";
        label2.Size = new Size(243, 16);
        label2.TabIndex = 8;
        label2.Text = "(Wenn es nicht automatisch erkannt wird)";
        // 
        // PathSelection
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = Color.FromArgb(49, 56, 73);
        ClientSize = new Size(374, 433);
        ControlBox = false;
        Controls.Add(label2);
        Controls.Add(label1);
        Controls.Add(labelpathtext2);
        Controls.Add(lblErkannt);
        Controls.Add(btnWeiter);
        Controls.Add(txtPath);
        Controls.Add(btnSelectPath);
        Controls.Add(lblPathText);
        Controls.Add(picLogo);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        Margin = new Padding(4, 3, 4, 3);
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "PathSelection";
        Text = "USA-LIFE Launcher - Pfad wählen";
        ((ISupportInitialize)picLogo).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }
}
