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
    public partial class Form2 : Form
    {
        Form1 parent;
        public Form2(Form1 p)
        {
            InitializeComponent();
            
            parent = p;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            parent.pictureBox1.Size = new System.Drawing.Size((int)numericUpDown1.Value,(int)numericUpDown2.Value);
            Bitmap bmpd = new Bitmap((int)numericUpDown1.Value, (int)numericUpDown2.Value);
            using (Graphics g = Graphics.FromImage(bmpd))
            {
                g.Clear(Color.White);
            }
            parent.pictureBox1.Image = bmpd;
            this.Close();
        }
    }
}
