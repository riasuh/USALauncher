using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace USALauncher;

internal class PictureBoxOpacity : PictureBox
{
	private float opacity = 1f;

	public float Opacity
	{
		get
		{
			return opacity;
		}
		set
		{
			opacity = value;
			Invalidate();
		}
	}

	protected override void OnPaint(PaintEventArgs pe)
	{
		if (base.Image != null)
		{
			Bitmap bitmap = new Bitmap(base.Image, new Size(base.Width, base.Height));
			ImageAttributes imageAttributes = new ImageAttributes();
			ColorMatrix colorMatrix = new ColorMatrix();
			colorMatrix.Matrix33 = opacity;
			imageAttributes.SetColorMatrix(colorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
			pe.Graphics.Clear(Color.FromKnownColor(KnownColor.ButtonFace));
			pe.Graphics.DrawImage(base.Image, new Rectangle(0, 0, bitmap.Width, bitmap.Height), 0, 0, bitmap.Width, bitmap.Height, GraphicsUnit.Pixel, imageAttributes);
		}
		else
		{
			base.OnPaint(pe);
		}
	}
}
