using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/*
// This object mimics the functionality of Photoshop layers. 
// It provides a single method: ```merge()```. This method takes
// a top and a bottom layer to merge together. *The top layer is 
// merged ontop of the bottom layer*.
//
// There are 7 pre-defined blending modes with which you can
// blend layers.
*/
namespace netfx
{
    class layers
    {
        BitmapW apply(BitmapW bottom, BitmapW top, string fn)
        {
            int i = 0, j = 0,
            h = Math.Min(bottom.Height(), top.Height()),
            w = Math.Min(bottom.Width(), top.Width());

            for (i = 0; i < w; i++)
            {
                for (j = 0; j < h; j++)
                {
                    // Execute blend on each pixel.
                    bottom.SetPixel(i, j, func(fn, bottom.GetPixel(i, j), top.GetPixel(i, j)));
                }
            }
            return bottom;
        }

        Color func(string fn, Color b, Color t)
        {
            int cr = 0, cg = 0, cb = 0, ca = 0;
            //check which function to apply and apply it
            if (fn == "multiply")
            {
                cr = (t.R * b.R) / 255;
                cg = (t.G * b.G) / 255;
                cb = (t.B * b.B) / 255;
                ca = (t.A  * b.A) / 255;
            }
            if (fn == "screen")
            {
                cr = 255 - (((255 - t.R) * (255 - b.R)) / 255);
                cg = 255 - (((255 - t.G) * (255 - b.G)) / 255);
                cb = 255 - (((255 - t.B) * (255 - b.B)) / 255);
                ca = 255 - (((255 - t.A) * (255 - b.A)) / 255);
            }

            if (fn == "overlay")
            {
                cr = coverlay(b.R, t.R);
                cg = coverlay(b.G, t.G);
                cb = coverlay(b.B, t.B);
                ca = coverlay(b.A, t.A);
            }

            // Thanks to @olivierlesnicki for suggesting a better algoritm.
            if (fn == "softLight")
            {
                cr = csoftLight(b.R, t.R);
                cg = csoftLight(b.G, t.G);
                cb = csoftLight(b.B, t.B);
                ca = csoftLight(b.A, t.A);
            }
            if (fn == "addition")
            {
                cr = b.R + t.R;
                cg = b.G + t.G;
                cb = b.B + t.B;
                ca = b.A + t.A;
            }
            if (fn == "exclusion")
            {
                cr = 128 - 2 * (b.R - 128) * (t.R - 128) / 255;
                cg = 128 - 2 * (b.G - 128) * (t.G - 128) / 255;
                cb = 128 - 2 * (b.B - 128) * (t.B - 128) / 255;
                ca = 128 - 2 * (b.A - 128) * (t.A - 128) / 255;
            }
            if (fn == "difference")
            {
                cr = Math.Abs(b.R - t.R);
                cg = Math.Abs(b.G - t.G);
                cb = Math.Abs(b.B - t.B);
                ca = Math.Abs(b.A - t.A);
            }
           if (fn == "colordodge")
            {
               
                double cr_d, cg_d, cb_d, ca_d;
                if ((t.R) != 255)
                    cr_d = ((double)b.R /(255 - t.R))*255 ;
                else cr_d = 0;

                if ((t.G) != 255)
                    cg_d = ((double)b.G/(255 - t.G))*255;
                else cg_d = 0;

                if ((t.B) != 255)
                    cb_d = ((double)b.B/(255 - t.B))*255;
                else cb_d = 0;

                if ((t.A) != 255)
                    ca_d = ((double)b.A/(255 - t.A))*255;
                else ca_d = 0;

                cr = (int)cr_d;
                cg = (int)cg_d;
                cb = (int)cb_d;
                ca = (int)ca_d;

            }
            return Color.FromArgb((int)Util.clamp(ca, 0, 255), (int)Util.clamp(cr, 0, 255), (int)Util.clamp(cg, 0, 255), (int)Util.clamp(cb, 0, 255));

        }

        int coverlay(int b, int t) { return (b > 128) ? 255 - 2 * (255 - t) * (255 - b) / 255 : (b * t * 2) / 255; }
        int csoftLight(float b, float t)
        {
            b /= 255;
            t /= 255;
            return (int)((t < 0.5) ? 255 * ((1 - 2 * t) * b * b + 2 * t * b) : 255 * ((1 - (2 * t - 1)) * b + (2 * t - 1) * (Math.Pow(b, 0.5))));
        }


        /// <summary>
        ///  Merges two layers. Takes a 'type' parameter and 
        /// a bottom and top layer. The 'type' parameter specifies
        /// the blending mode. type options are:
        ///- multiply
        ///- screen
        ///- overlay
        ///- softLight
        ///- addition
        ///- exclusion
        ///- difference
        /// </summary>
        /// <param name="type"></param>
        /// <param name="bottom"></param>
        /// <param name="top"></param>
        /// <returns></returns>
        public BitmapW merge(string type, BitmapW bottom, BitmapW top)
        {

            return apply(bottom, top, type);
        }

    }
}
