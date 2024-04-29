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
        Process.Start(new ProcessStartInfo("https://usa-life.net/anleitung?o") { UseShellExecute = true });
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
        this.components = new System.ComponentModel.Container();
        this.btnOK = new System.Windows.Forms.Button();
        this.btnFAQ = new System.Windows.Forms.Button();
        this.label1 = new System.Windows.Forms.Label();
        this.label2 = new System.Windows.Forms.Label();
        this.label3 = new System.Windows.Forms.Label();
        this.label4 = new System.Windows.Forms.Label();
        this.picInfoUsaLogo = new System.Windows.Forms.PictureBox();
        this.lblUsalifeLink = new System.Windows.Forms.Label();
        this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
        this.lblGitLab = new System.Windows.Forms.Label();
        ((System.ComponentModel.ISupportInitialize)(this.picInfoUsaLogo)).BeginInit();
        this.SuspendLayout();
        // 
        // btnOK
        // 
        this.btnOK.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(39)))), ((int)(((byte)(39)))), ((int)(((byte)(42)))));
        this.btnOK.Cursor = System.Windows.Forms.Cursors.Hand;
        this.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
        this.btnOK.ForeColor = System.Drawing.Color.White;
        this.btnOK.Location = new System.Drawing.Point(15, 128);
        this.btnOK.Name = "btnOK";
        this.btnOK.Size = new System.Drawing.Size(75, 23);
        this.btnOK.TabIndex = 0;
        this.btnOK.Text = "OK";
        this.btnOK.UseVisualStyleBackColor = false;
        this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
        // 
        // btnFAQ
        // 
        this.btnFAQ.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(39)))), ((int)(((byte)(39)))), ((int)(((byte)(42)))));
        this.btnFAQ.Cursor = System.Windows.Forms.Cursors.Hand;
        this.btnFAQ.FlatStyle = System.Windows.Forms.FlatStyle.System;
        this.btnFAQ.ForeColor = System.Drawing.Color.White;
        this.btnFAQ.Location = new System.Drawing.Point(267, 128);
        this.btnFAQ.Name = "btnFAQ";
        this.btnFAQ.Size = new System.Drawing.Size(75, 23);
        this.btnFAQ.TabIndex = 1;
        this.btnFAQ.Text = "FAQ";
        this.btnFAQ.UseVisualStyleBackColor = false;
        this.btnFAQ.Click += new System.EventHandler(this.btnFAQ_Click);
        // 
        // label1
        // 
        this.label1.AutoSize = true;
        this.label1.BackColor = System.Drawing.Color.Transparent;
        this.label1.Font = new System.Drawing.Font("Bahnschrift SemiLight", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.label1.ForeColor = System.Drawing.Color.White;
        this.label1.Location = new System.Drawing.Point(112, 25);
        this.label1.Name = "label1";
        this.label1.Size = new System.Drawing.Size(118, 16);
        this.label1.TabIndex = 2;
        this.label1.Text = "USA LIFE Launcher";
        // 
        // label2
        // 
        this.label2.AutoSize = true;
        this.label2.BackColor = System.Drawing.Color.Transparent;
        this.label2.Font = new System.Drawing.Font("Bahnschrift SemiLight", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.label2.ForeColor = System.Drawing.Color.White;
        this.label2.Location = new System.Drawing.Point(112, 41);
        this.label2.Name = "label2";
        this.label2.Size = new System.Drawing.Size(96, 16);
        this.label2.TabIndex = 3;
        this.label2.Text = "Made with ❤️ by";
        // 
        // label3
        // 
        this.label3.AutoSize = true;
        this.label3.BackColor = System.Drawing.Color.Transparent;
        this.label3.Font = new System.Drawing.Font("Bahnschrift SemiLight", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.label3.ForeColor = System.Drawing.Color.White;
        this.label3.Location = new System.Drawing.Point(112, 82);
        this.label3.Name = "label3";
        this.label3.Size = new System.Drawing.Size(162, 16);
        this.label3.TabIndex = 4;
        this.label3.Text = "Copyright © 2024 USA LIFE";
        // 
        // label4
        // 
        this.label4.AutoSize = true;
        this.label4.BackColor = System.Drawing.Color.Transparent;
        this.label4.Font = new System.Drawing.Font("Bahnschrift SemiLight", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.label4.ForeColor = System.Drawing.Color.White;
        this.label4.Location = new System.Drawing.Point(112, 98);
        this.label4.Name = "label4";
        this.label4.Size = new System.Drawing.Size(148, 16);
        this.label4.TabIndex = 5;
        this.label4.Text = "Alle Rechte vorbehalten.";
        // 
        // picInfoUsaLogo
        // 
        this.picInfoUsaLogo.BackColor = System.Drawing.Color.Transparent;
        this.picInfoUsaLogo.Image = global::USALauncher.Properties.Resources.Logo_3_USAFull;
        this.picInfoUsaLogo.Location = new System.Drawing.Point(7, 28);
        this.picInfoUsaLogo.Name = "picInfoUsaLogo";
        this.picInfoUsaLogo.Size = new System.Drawing.Size(100, 80);
        this.picInfoUsaLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
        this.picInfoUsaLogo.TabIndex = 6;
        this.picInfoUsaLogo.TabStop = false;
        // 
        // lblUsalifeLink
        // 
        this.lblUsalifeLink.AutoSize = true;
        this.lblUsalifeLink.BackColor = System.Drawing.Color.Transparent;
        this.lblUsalifeLink.Cursor = System.Windows.Forms.Cursors.Hand;
        this.lblUsalifeLink.Font = new System.Drawing.Font("Bahnschrift SemiLight", 9.75F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.lblUsalifeLink.ForeColor = System.Drawing.SystemColors.Highlight;
        this.lblUsalifeLink.Location = new System.Drawing.Point(205, 43);
        this.lblUsalifeLink.Name = "lblUsalifeLink";
        this.lblUsalifeLink.Size = new System.Drawing.Size(73, 16);
        this.lblUsalifeLink.TabIndex = 8;
        this.lblUsalifeLink.Text = "usa-life.net";
        this.toolTip1.SetToolTip(this.lblUsalifeLink, "https://usa-life.net");
        this.lblUsalifeLink.Click += new System.EventHandler(this.lblUsalifeLink_Click);
        // 
        // lblGitLab
        // 
        this.lblGitLab.AutoSize = true;
        this.lblGitLab.BackColor = System.Drawing.Color.Transparent;
        this.lblGitLab.Cursor = System.Windows.Forms.Cursors.Hand;
        this.lblGitLab.Font = new System.Drawing.Font("Bahnschrift SemiLight", 9.75F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.lblGitLab.ForeColor = System.Drawing.SystemColors.Highlight;
        this.lblGitLab.Location = new System.Drawing.Point(112, 62);
        this.lblGitLab.Name = "lblGitLab";
        this.lblGitLab.Size = new System.Drawing.Size(87, 16);
        this.lblGitLab.TabIndex = 9;
        this.lblGitLab.Text = "GitLab Project";
        this.toolTip1.SetToolTip(this.lblGitLab, "https://gitlab.usa-life.net/larry/launcher");
        this.lblGitLab.Click += new System.EventHandler(this.lblGitLab_Click);
        // 
        // InfoFenster
        // 
        this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(56)))), ((int)(((byte)(73)))));
        this.ClientSize = new System.Drawing.Size(357, 166);
        this.Controls.Add(this.lblGitLab);
        this.Controls.Add(this.lblUsalifeLink);
        this.Controls.Add(this.picInfoUsaLogo);
        this.Controls.Add(this.label4);
        this.Controls.Add(this.label3);
        this.Controls.Add(this.label2);
        this.Controls.Add(this.label1);
        this.Controls.Add(this.btnFAQ);
        this.Controls.Add(this.btnOK);
        this.Icon = global::USALauncher.Properties.Resources.Icon_1_USA_128;
        this.MaximizeBox = false;
        this.Name = "InfoFenster";
        this.Text = "Informationen";
        this.Load += new System.EventHandler(this.InfoFenster_Load);
        ((System.ComponentModel.ISupportInitialize)(this.picInfoUsaLogo)).EndInit();
        this.ResumeLayout(false);
        this.PerformLayout();

    }
}
