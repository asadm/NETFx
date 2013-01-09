using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace filtrr2_port
{
    public partial class Form1 : Form
    {
        effects effects = new effects(); //contains all the basic effects 
        layers layers = new layers(); //photoshop like layer implementation

        public Form1()
        {
            InitializeComponent();
            openFileDialog1.FileName = "";

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.FileName == "") 
            { MessageBox.Show("Select an image first");  return; }

            label1.Visible = true; this.Invalidate();

            BitmapW a = new BitmapW(openFileDialog1.FileName);
            pictureBox1.Image = a.GetBitmap();

            BitmapW layer1 = a.Clone(), layer2 = a.Clone(), layer3 = a.Clone(); //make three copies of the image l1,l2,l3
            BitmapW res = a.Clone(); //this will hold the final result

            layer3 = effects.fill(layer3, 167, 118, 12); //layer 3 is a solid fill of this rgb color

            layer2 = effects.blur(layer2, "gaussian"); //layer2 is a blurred version of original image
            layer1 = effects.saturate(layer1, -50); //layer1 is a saturated version


            res = layers.merge("overlay", res, layer1); //merge layer1 onto original with "overlay" layer blending
            res = layers.merge("softLight", res, layer2); //now merge layer2 onto this, with "softlight" layer blending
            res = layers.merge("softLight", res, layer3); //now merge layer3 onto this, with "softlight" layer blending
            res = effects.saturate(res,-40); //apply -40 saturate effect on this now
            res = effects.contrast(res, 10); //apply 10 contrast 

            pictureBox2.Image = res.GetBitmap(); //show the resulting image
            label1.Visible = false;

        }

        private void button2_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
            if (openFileDialog1.FileName == "") return;
            BitmapW a = new BitmapW(openFileDialog1.FileName);
            pictureBox1.Image = a.GetBitmap();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.FileName == "")
            { MessageBox.Show("Select an image first"); return; }
            label1.Visible = true; this.Invalidate();
            BitmapW a = new BitmapW(openFileDialog1.FileName);
            pictureBox1.Image = a.GetBitmap();

            
            BitmapW res = a.Clone(); //this will hold the final result

            res = effects.sepia(res);

            pictureBox2.Image = res.GetBitmap(); //show the resulting image
            label1.Visible = false;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.FileName == "")
            { MessageBox.Show("Select an image first"); return; }

            label1.Visible = true; this.Invalidate();
            BitmapW a = new BitmapW(openFileDialog1.FileName);
            pictureBox1.Image = a.GetBitmap();


            BitmapW res = a.Clone(); //this will hold the final result

            res = effects.blur(res, "gaussian");

            pictureBox2.Image = res.GetBitmap(); //show the resulting image
            label1.Visible = false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            label1.Visible = false;
            this.Invalidate();
            //a.Activate();
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            info a = new info();
            a.Show();
        }
    }
}
