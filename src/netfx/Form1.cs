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
             
            BitmapW a = new BitmapW(openFileDialog1.FileName);
            pictureBox1.Image = a.GetBitmap();

            
            BitmapW res = a.Clone(); //this will hold the final result

            res = effects.vignette(res, 0, 0, 0);

            pictureBox2.Image = res.GetBitmap(); //show the resulting image
             
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.FileName == "")
            { MessageBox.Show("Select an image first"); return; }

            
            BitmapW a = new BitmapW(openFileDialog1.FileName);
            pictureBox1.Image = a.GetBitmap();


            BitmapW res = a.Clone(); //this will hold the final result

            res = effects.blur(res, "gaussian");

            pictureBox2.Image = res.GetBitmap(); //show the resulting image
             
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void Form1_Shown(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.FileName == "")
            { MessageBox.Show("Select an image first"); return; }


            BitmapW a = new BitmapW(openFileDialog1.FileName);
            pictureBox1.Image = a.GetBitmap(); //show the original image

            //apply effects

            BitmapW res = a.Clone(); //this will hold the final result

            //do some adjustments
            res = effects.brighten(res, 20);
            res = effects.saturate(res, -90);
            
            //add a purplish color
            BitmapW purple = a.Clone();
            purple = effects.fill(purple, 34, 43, 109);
            res = layers.merge("softLight", res, purple);

            //do some more adjustments
            res = effects.gamma(res, -5);
            res = effects.contrast(res, 50);
            

            pictureBox2.Image = res.GetBitmap(); //show the resulting image
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.FileName == "")
            { MessageBox.Show("Select an image first"); return; }


            BitmapW a = new BitmapW(openFileDialog1.FileName);
            pictureBox1.Image = a.GetBitmap(); //show the original image

            //apply effects

            BitmapW layer1 = a.Clone(); //working layer 1
            BitmapW res = a.Clone(); //this will hold the final result

            //do some adjustments
            res = effects.brighten(res, 10);
            res = effects.contrast(res, 30);
            
            //add a sepia softlight to the image
            layer1 = res.Clone();
            layer1 = effects.sepia(layer1);
            layer1 = effects.vignette(layer1, 0, 0, 0); //some vignette too
            res = layers.merge("softLight", res, layer1); //add this layer to our image as softlight

            

            //desaturate a little
            res = effects.saturate(res, -30);

            //tadaaa
            pictureBox2.Image = res.GetBitmap(); //show the resulting image
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.FileName == "")
            { MessageBox.Show("Select an image first"); return; }


            BitmapW a = new BitmapW(openFileDialog1.FileName);
            pictureBox1.Image = a.GetBitmap(); //show the original image

            //apply effect
            BitmapW res = a.Clone();

            res = effects.saturate(res, -100); //grayscale it
            res = effects.contrast(res, 125);
            res = effects.noise(res, 3);
            res = effects.sepia(res);

            res = effects.adjust(res, 8, 2, 4);
            

            //tadaaa
            pictureBox2.Image = res.GetBitmap(); //show the resulting image
        }
    }
}
