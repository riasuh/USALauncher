using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace USALauncher;

public class TexturemodForm : Form
{
    private IContainer components;

    private Timer timer1;

    public TexturemodForm()
    {
        InitializeComponent();
        base.Icon = USALauncher.Properties.Resources.Icon_1_USA_128;
    }

    private void timer1_Tick_1(object sender, EventArgs e)
    {
        Close();
    }

    private void Form2_Load(object sender, EventArgs e)
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
        timer1.Interval = 1500;
        timer1.Tick += timer1_Tick_1;
        // 
        // TexturemodForm
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        BackgroundImage = Properties.Resources.msgboxTexturemod_Image;
        ClientSize = new Size(348, 87);
        FormBorderStyle = FormBorderStyle.None;
        Margin = new Padding(4, 3, 4, 3);
        Name = "TexturemodForm";
        Text = "Form2";
        Load += Form2_Load;
        ResumeLayout(false);
    }
}
