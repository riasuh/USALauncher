using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using USALauncher.Properties;
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
        txtPath = new TextBox();
        btnSelectPath = new Button();
        lblPathText = new Label();
        picLogo = new PictureBox();
        fbdprofilePath = new FolderBrowserDialog();
        lblErkannt = new Label();
        btnWeiter = new Button();
        label1 = new Label();
        ((ISupportInitialize)picLogo).BeginInit();
        SuspendLayout();
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
        txtPath.TabIndex = 8;
        txtPath.TextChanged += txtPath_TextChanged;
        // 
        // btnSelectPath
        // 
        btnSelectPath.FlatStyle = FlatStyle.System;
        btnSelectPath.Font = new Font("Bahnschrift SemiLight", 9F);
        btnSelectPath.Location = new Point(326, 367);
        btnSelectPath.Margin = new Padding(4, 3, 4, 3);
        btnSelectPath.Name = "btnSelectPath";
        btnSelectPath.Size = new Size(33, 23);
        btnSelectPath.TabIndex = 7;
        btnSelectPath.Text = "...";
        btnSelectPath.UseVisualStyleBackColor = true;
        btnSelectPath.Click += btnSelectPath_Click_1;
        // 
        // lblPathText
        // 
        lblPathText.AutoSize = true;
        lblPathText.BackColor = Color.Transparent;
        lblPathText.Font = new Font("Bahnschrift SemiLight", 10F);
        lblPathText.ForeColor = Color.White;
        lblPathText.Location = new Point(35, 300);
        lblPathText.Margin = new Padding(4, 0, 4, 0);
        lblPathText.Name = "lblPathText";
        lblPathText.Size = new Size(270, 17);
        lblPathText.TabIndex = 6;
        lblPathText.Text = "Wähle bitte deinen Arma 3 Profilpfad aus";
        // 
        // picLogo
        // 
        picLogo.BackColor = Color.Transparent;
        picLogo.BackgroundImage = Properties.Resources.Logo_3_USAFull;
        picLogo.BackgroundImageLayout = ImageLayout.Stretch;
        picLogo.Location = new Point(41, 15);
        picLogo.Margin = new Padding(4, 3, 4, 3);
        picLogo.Name = "picLogo";
        picLogo.Size = new Size(292, 232);
        picLogo.TabIndex = 10;
        picLogo.TabStop = false;
        // 
        // lblErkannt
        // 
        lblErkannt.AutoSize = true;
        lblErkannt.BackColor = Color.Transparent;
        lblErkannt.Font = new Font("Bahnschrift SemiLight", 10F);
        lblErkannt.ForeColor = Color.Lime;
        lblErkannt.Location = new Point(7, 347);
        lblErkannt.Margin = new Padding(4, 0, 4, 0);
        lblErkannt.Name = "lblErkannt";
        lblErkannt.Size = new Size(209, 17);
        lblErkannt.TabIndex = 11;
        lblErkannt.Text = "Profilpfad automatisch erkannt!";
        // 
        // btnWeiter
        // 
        btnWeiter.FlatStyle = FlatStyle.System;
        btnWeiter.Font = new Font("Bahnschrift SemiLight", 8.5F);
        btnWeiter.Location = new Point(271, 396);
        btnWeiter.Margin = new Padding(4, 3, 4, 3);
        btnWeiter.Name = "btnWeiter";
        btnWeiter.Size = new Size(88, 27);
        btnWeiter.TabIndex = 9;
        btnWeiter.Text = "Speichern";
        btnWeiter.UseVisualStyleBackColor = true;
        btnWeiter.Click += btnWeiter_Click_1;
        // 
        // label1
        // 
        label1.AutoSize = true;
        label1.BackColor = Color.Transparent;
        label1.Font = new Font("Bahnschrift SemiLight", 10F);
        label1.ForeColor = Color.White;
        label1.Location = new Point(23, 320);
        label1.Margin = new Padding(4, 0, 4, 0);
        label1.Name = "label1";
        label1.Size = new Size(284, 17);
        label1.TabIndex = 12;
        label1.Text = "(Wird normalerweise automatisch erkannt)";
        // 
        // ProfilePathSelection
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = Color.FromArgb(49, 56, 73);
        ClientSize = new Size(374, 435);
        Controls.Add(label1);
        Controls.Add(txtPath);
        Controls.Add(btnSelectPath);
        Controls.Add(lblPathText);
        Controls.Add(picLogo);
        Controls.Add(lblErkannt);
        Controls.Add(btnWeiter);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        Margin = new Padding(4, 3, 4, 3);
        MaximizeBox = false;
        Name = "ProfilePathSelection";
        Text = "USA-LIFE Launcher - Profilpfad wählen";
        ((ISupportInitialize)picLogo).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }
}
