using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using USALauncher.Properties;

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
		this.components = new System.ComponentModel.Container();
		this.timer1 = new System.Windows.Forms.Timer(this.components);
		base.SuspendLayout();
		this.timer1.Interval = 1500;
		this.timer1.Tick += new System.EventHandler(timer1_Tick_1);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.BackgroundImage = USALauncher.Properties.Resources.msgboxTexturemod_Image;
		base.ClientSize = new System.Drawing.Size(350, 75);
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
		base.Name = "Form2";
		this.Text = "Form2";
		base.Load += new System.EventHandler(Form2_Load);
		base.ResumeLayout(false);
	}
}
