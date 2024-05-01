using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace USALauncher;

public class MissionForm : Form
{
    private IContainer components;

    private Timer timer1;

    public MissionForm()
    {
        InitializeComponent();
        base.Icon = USALauncher.Properties.Resources.Icon_1_USA_128;
    }

    private void timer1_Tick(object sender, EventArgs e)
    {
        Close();
    }

    private void Form1_Load(object sender, EventArgs e)
    {
        timer1.Start();
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
        timer1 = new Timer(components);
        SuspendLayout();
        // 
        // timer1
        // 
        timer1.Interval = 1000;
        timer1.Tick += timer1_Tick;
        // 
        // MissionForm
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        BackgroundImage = Properties.Resources.msgboxMission_Image;
        ClientSize = new Size(347, 87);
        FormBorderStyle = FormBorderStyle.None;
        Margin = new Padding(4, 3, 4, 3);
        Name = "MissionForm";
        Text = "Form1";
        Load += Form1_Load;
        ResumeLayout(false);
    }
}
