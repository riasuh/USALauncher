using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Forms;

namespace USALauncher.Resources;

public class InfoForm : Form
{
    private IContainer components;

    private Button btnOK;

    private Button btnFAQ;

    private Label label1;

    private Label label2;

    private Label label3;

    private Label label4;
    private Label lblUsalifeLink;
    private ToolTip toolTip1;
    private Label lblGitLab;
    private PictureBox picInfoUsaLogo;

    public InfoForm()
    {
        InitializeComponent();
    }

    private void InfoFenster_Load(object sender, EventArgs e)
    {
    }

    private void btnFAQ_Click(object sender, EventArgs e)
    {
        Process.Start(new ProcessStartInfo("https://usa-life.net/connect/download") { UseShellExecute = true });
    }

    private void btnOK_Click(object sender, EventArgs e)
    {
        Close();
    }
    private void lblUsalifeLink_Click(object sender, EventArgs e)
    {
        Process.Start(new ProcessStartInfo("https://usa-life.net/") { UseShellExecute = true });
    }
    private void lblGitLab_Click(object sender, EventArgs e)
    {
        Process.Start(new ProcessStartInfo("https://gitlab.usa-life.net/larry/launcher") { UseShellExecute = true });
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
        components = new Container();
        btnOK = new Button();
        btnFAQ = new Button();
        label1 = new Label();
        label2 = new Label();
        label3 = new Label();
        label4 = new Label();
        picInfoUsaLogo = new PictureBox();
        lblUsalifeLink = new Label();
        toolTip1 = new ToolTip(components);
        lblGitLab = new Label();
        ((ISupportInitialize)picInfoUsaLogo).BeginInit();
        SuspendLayout();
        // 
        // btnOK
        // 
        btnOK.BackColor = System.Drawing.Color.FromArgb(39, 39, 42);
        btnOK.Cursor = Cursors.Hand;
        btnOK.FlatStyle = FlatStyle.System;
        btnOK.ForeColor = System.Drawing.Color.White;
        btnOK.Location = new System.Drawing.Point(18, 148);
        btnOK.Margin = new Padding(4, 3, 4, 3);
        btnOK.Name = "btnOK";
        btnOK.Size = new System.Drawing.Size(88, 27);
        btnOK.TabIndex = 0;
        btnOK.Text = "OK";
        btnOK.UseVisualStyleBackColor = false;
        btnOK.Click += btnOK_Click;
        // 
        // btnFAQ
        // 
        btnFAQ.BackColor = System.Drawing.Color.FromArgb(39, 39, 42);
        btnFAQ.Cursor = Cursors.Hand;
        btnFAQ.FlatStyle = FlatStyle.System;
        btnFAQ.ForeColor = System.Drawing.Color.White;
        btnFAQ.Location = new System.Drawing.Point(312, 148);
        btnFAQ.Margin = new Padding(4, 3, 4, 3);
        btnFAQ.Name = "btnFAQ";
        btnFAQ.Size = new System.Drawing.Size(88, 27);
        btnFAQ.TabIndex = 1;
        btnFAQ.Text = "FAQ";
        btnFAQ.UseVisualStyleBackColor = false;
        btnFAQ.Click += btnFAQ_Click;
        // 
        // label1
        // 
        label1.AutoSize = true;
        label1.BackColor = System.Drawing.Color.Transparent;
        label1.Font = new System.Drawing.Font("Bahnschrift SemiLight", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
        label1.ForeColor = System.Drawing.Color.White;
        label1.Location = new System.Drawing.Point(131, 29);
        label1.Margin = new Padding(4, 0, 4, 0);
        label1.Name = "label1";
        label1.Size = new System.Drawing.Size(118, 16);
        label1.TabIndex = 2;
        label1.Text = "USA LIFE Launcher";
        // 
        // label2
        // 
        label2.AutoSize = true;
        label2.BackColor = System.Drawing.Color.Transparent;
        label2.Font = new System.Drawing.Font("Bahnschrift SemiLight", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
        label2.ForeColor = System.Drawing.Color.White;
        label2.Location = new System.Drawing.Point(131, 47);
        label2.Margin = new Padding(4, 0, 4, 0);
        label2.Name = "label2";
        label2.Size = new System.Drawing.Size(96, 16);
        label2.TabIndex = 3;
        label2.Text = "Made with ❤️ by";
        // 
        // label3
        // 
        label3.AutoSize = true;
        label3.BackColor = System.Drawing.Color.Transparent;
        label3.Font = new System.Drawing.Font("Bahnschrift SemiLight", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
        label3.ForeColor = System.Drawing.Color.White;
        label3.Location = new System.Drawing.Point(131, 95);
        label3.Margin = new Padding(4, 0, 4, 0);
        label3.Name = "label3";
        label3.Size = new System.Drawing.Size(162, 16);
        label3.TabIndex = 4;
        label3.Text = "Copyright © 2024 USA LIFE";
        // 
        // label4
        // 
        label4.AutoSize = true;
        label4.BackColor = System.Drawing.Color.Transparent;
        label4.Font = new System.Drawing.Font("Bahnschrift SemiLight", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
        label4.ForeColor = System.Drawing.Color.White;
        label4.Location = new System.Drawing.Point(131, 113);
        label4.Margin = new Padding(4, 0, 4, 0);
        label4.Name = "label4";
        label4.Size = new System.Drawing.Size(148, 16);
        label4.TabIndex = 5;
        label4.Text = "Alle Rechte vorbehalten.";
        // 
        // picInfoUsaLogo
        // 
        picInfoUsaLogo.BackColor = System.Drawing.Color.Transparent;
        picInfoUsaLogo.Image = Properties.Resources.Logo_3_USAFull;
        picInfoUsaLogo.Location = new System.Drawing.Point(8, 32);
        picInfoUsaLogo.Margin = new Padding(4, 3, 4, 3);
        picInfoUsaLogo.Name = "picInfoUsaLogo";
        picInfoUsaLogo.Size = new System.Drawing.Size(117, 92);
        picInfoUsaLogo.SizeMode = PictureBoxSizeMode.Zoom;
        picInfoUsaLogo.TabIndex = 6;
        picInfoUsaLogo.TabStop = false;
        // 
        // lblUsalifeLink
        // 
        lblUsalifeLink.AutoSize = true;
        lblUsalifeLink.BackColor = System.Drawing.Color.Transparent;
        lblUsalifeLink.Cursor = Cursors.Hand;
        lblUsalifeLink.Font = new System.Drawing.Font("Bahnschrift SemiLight", 9.75F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, 0);
        lblUsalifeLink.ForeColor = System.Drawing.SystemColors.Highlight;
        lblUsalifeLink.Location = new System.Drawing.Point(225, 48);
        lblUsalifeLink.Margin = new Padding(4, 0, 4, 0);
        lblUsalifeLink.Name = "lblUsalifeLink";
        lblUsalifeLink.Size = new System.Drawing.Size(73, 16);
        lblUsalifeLink.TabIndex = 8;
        lblUsalifeLink.Text = "usa-life.net";
        toolTip1.SetToolTip(lblUsalifeLink, "https://usa-life.net");
        lblUsalifeLink.Click += lblUsalifeLink_Click;
        // 
        // lblGitLab
        // 
        lblGitLab.AutoSize = true;
        lblGitLab.BackColor = System.Drawing.Color.Transparent;
        lblGitLab.Cursor = Cursors.Hand;
        lblGitLab.Font = new System.Drawing.Font("Bahnschrift SemiLight", 9.75F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, 0);
        lblGitLab.ForeColor = System.Drawing.SystemColors.Highlight;
        lblGitLab.Location = new System.Drawing.Point(131, 72);
        lblGitLab.Margin = new Padding(4, 0, 4, 0);
        lblGitLab.Name = "lblGitLab";
        lblGitLab.Size = new System.Drawing.Size(87, 16);
        lblGitLab.TabIndex = 9;
        lblGitLab.Text = "GitLab Project";
        toolTip1.SetToolTip(lblGitLab, "https://gitlab.usa-life.net/larry/launcher");
        lblGitLab.Click += lblGitLab_Click;
        // 
        // InfoForm
        // 
        AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = System.Drawing.Color.FromArgb(49, 56, 73);
        ClientSize = new System.Drawing.Size(416, 192);
        Controls.Add(lblGitLab);
        Controls.Add(lblUsalifeLink);
        Controls.Add(picInfoUsaLogo);
        Controls.Add(label4);
        Controls.Add(label3);
        Controls.Add(label2);
        Controls.Add(label1);
        Controls.Add(btnFAQ);
        Controls.Add(btnOK);
        Icon = Properties.Resources.Icon_1_USA_128;
        Margin = new Padding(4, 3, 4, 3);
        MaximizeBox = false;
        Name = "InfoForm";
        Text = "Informationen";
        Load += InfoFenster_Load;
        ((ISupportInitialize)picInfoUsaLogo).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }
}
