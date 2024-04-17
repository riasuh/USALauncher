using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using USALauncher.Properties;

namespace USALauncher;

public class Form1 : Form
{
	private IContainer components;

	private Timer timer1;

	public Form1()
	{
		InitializeComponent();
		base.Icon = USALauncher.Properties.Resources.LOGO01_snowwhite_PNG;
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
		this.components = new System.ComponentModel.Container();
		this.timer1 = new System.Windows.Forms.Timer(this.components);
		base.SuspendLayout();
		this.timer1.Interval = 1000;
		this.timer1.Tick += new System.EventHandler(timer1_Tick);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.BackgroundImage = USALauncher.Properties.Resources.msgbox2;
		base.ClientSize = new System.Drawing.Size(350, 75);
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
		base.Name = "Form1";
		this.Text = "Form1";
		base.Load += new System.EventHandler(Form1_Load);
		base.ResumeLayout(false);
	}
}
