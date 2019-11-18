using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsLab2
{
    public partial class Form1 : Form
    {
        private Bitmap bmpd;
        private Color CurColor = Color.Black,OldColor= Color.White;
        private bool drawing = false;
        private Point? mlocation = null;
        private Pen pen = new Pen(Color.Black, 2);
        public Form1()
        {
            InitializeComponent();
            if (pictureBox1.Image == null)
            {
                bmpd = new Bitmap(pictureBox1.Width, pictureBox1.Height);
                using (Graphics g = Graphics.FromImage(bmpd))
                {
                    g.Clear(Color.White);
                }
                pictureBox1.Image = bmpd;
            }
            KnownColor[] values = (KnownColor[])Enum.GetValues(typeof(KnownColor));
            toolStripButton4.BackColor = CurColor;
        }

        private void toolStripLabel1_Click(object sender, EventArgs e)
        {

        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            var Form_2 = new Form2(this);
            Form_2.StartPosition = FormStartPosition.CenterParent;
            Form_2.ShowDialog();

        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            ColorDialog MyDialog = new ColorDialog();
            MyDialog.AllowFullOpen = false;
            MyDialog.ShowHelp = true;
            MyDialog.Color = CurColor;
            OldColor = CurColor;
            // Update the text box color if the user clicks OK 
            if (MyDialog.ShowDialog() == DialogResult.OK)
                CurColor = MyDialog.Color;

            toolStripButton4.BackColor = CurColor;
           
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            if (!this.toolStripButton2.Checked)
            {
                this.toolStripButton2.Checked = true;
                this.toolStripButton3.Checked = false;

            }
            else { this.toolStripButton2.Checked = false; }
      
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            if (!this.toolStripButton3.Checked)
            {
                this.toolStripButton2.Checked = false;
                this.toolStripButton3.Checked = true;
            }
            else { this.toolStripButton3.Checked = false; }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (mlocation != null && drawing)
            {
                using (Graphics g = Graphics.FromImage(pictureBox1.Image))
                {
                    g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                    g.DrawLine(pen, mlocation.Value, e.Location);
                }
                pictureBox1.Invalidate();
                mlocation = e.Location;
            }
            System.GC.Collect();
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            mlocation= e.Location;
            if (this.toolStripButton2.Checked && !drawing)
            {
                pen = new Pen(CurColor, 2);
                drawing = true;
            }
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (this.toolStripButton3.Checked)
            {
                //bmpd = new Bitmap(pictureBox1.Width, pictureBox1.Height);
                //using (Graphics g = Graphics.FromImage(bmpd))
                //{
                //    g.Clear(CurColor);
                //}
                //pictureBox1.Image = bmpd;
                Bitmap tmp = pictureBox1.Image as Bitmap;
                Bitmap b = pictureBox1.Image as Bitmap;
                mlocation = e.Location;
                OldColor = tmp.GetPixel(mlocation.Value.X, mlocation.Value.Y);
                floodFill(0, 0, CurColor, OldColor);
                pictureBox1.Image = tmp;
                System.GC.Collect();

            }
            if (this.toolStripButton2.Checked)
            {
                drawing = false;
            }
        }

        void floodFill(int x, int y, Color fill, Color old)
        { 
            if (x < 0 || x >= pictureBox1.Size.Width) return;
            if (y < 0 || y >= pictureBox1.Size.Width) return;
            Bitmap b = pictureBox1.Image as Bitmap;
            if (b.GetPixel(x, y) == old)
            {
                b.SetPixel(x, y, fill);
                floodFill(x+1, y, fill, old);
                floodFill(x-1, y, fill, old);
                floodFill(x, y+1, fill, old);
                floodFill(x, y+1, fill, old);
            }
            //pictureBox1.Image = b;
        }
    }
}
