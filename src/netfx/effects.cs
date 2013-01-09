using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace filtrr2_port
{
    class effects
    {
        //Convolve
        // Performs a kerner convolution manipulation on the data
        // buffer. This is mostly used in masks i.e blurring or 
        // sharpening. It is a *very* intensive operation and will
        // be slow on big images! 
        // It creates a temporary data buffer where it writes the
        // new data. We can't modify the original buffer in-place 
        // because each new pixel value depends on the original
        // neighbouring values of that pixel (i.e the values residing)
        // inside the kernel.
        public BitmapW convolve(BitmapW image, float[,] kernel,int kw,int kh)
        {
            BitmapW temp = image.Clone();

           // int kh = kernel;
            //int kw = kh; //kernel[0].Length / 2;
            int i = 0, j = 0, n = 0, m = 0, cr, cg, cb,
            h = image.Height(),
            w = image.Width();

            for (i = 0; i < h; i++) 
            {
                for (j = 0; j < w; j++) 
                {
                    //kernel loop
                    float r = 0, g = 0, b = 0;
                    for (n = -kh; n <= kh; n++) 
                    {
                        for (m = -kw; m <= kw; m++) 
                        {
                            if (i + n >= 0 && i + n < h) 
                            {
                                if (j + m >= 0 && j + m < w) 
                                {
                                    float f = kernel[m + kw, n + kh];
                                    if (f == 0) {continue;}
                                    Color colortemp = image.GetPixel(j+m, i+n);
                                    cr = colortemp.R; cg = colortemp.G; cb = colortemp.B;

                                    r += cr * f;
                                    g += cg * f;
                                    b += cb * f;
                                }
                            }
                        }
                    }
                    //kernel loop end

                    temp.SetPixel(j, i, Color.FromArgb(255,(int)Util.clamp(r, 0, 255), (int)Util.clamp(g, 0, 255), (int)Util.clamp(b, 0, 255)));
                }
            }
            return temp;

        }

        // #### Adjust [No Range]
        public BitmapW adjust(BitmapW image, float pr, float pg, float pb)
        {
            float cr=0, cg=0, cb=0;
            int i = 0, j = 0,
            h = image.Height(),
            w = image.Width();

            for (i = 0; i < w; i++)
            {
                for (j = 0; j < h; j++)
                {
                    Color temp = image.GetPixel(i,j);
                    cr=temp.R; cg= temp.G; cb = temp.B;

                    cr *= (1.0f + pr);
                    cg *= (1.0f + pg);
                    cb *= (1.0f + pb);

                    image.SetPixel(i, j, Color.FromArgb((int)Util.clamp(cr, 0, 255), (int)Util.clamp(cg, 0, 255), (int)Util.clamp(cb, 0, 255)));
                }
            }
            return image;
        }

        // #### Brighten [-100, 100]
        public BitmapW brighten(BitmapW image, float p)
        {
            p = Util.normalize(p, -255, 255, -100, 100);
            float cr = 0, cg = 0, cb = 0;
            int i = 0, j = 0,
            h = image.Height(),
            w = image.Width();

            for (i = 0; i < w; i++)
            {
                for (j = 0; j < h; j++)
                {
                    Color temp = image.GetPixel(i, j);
                    cr = temp.R; cg = temp.G; cb = temp.B;

                    cr += (p);
                    cg += (p);
                    cb += (p);

                    image.SetPixel(i, j, Color.FromArgb((int)Util.clamp(cr, 0, 255), (int)Util.clamp(cg, 0, 255), (int)Util.clamp(cb, 0, 255)));
                }
            }
            return image;
        }

        // #### Alpha [-100, 100]
        public BitmapW alpha(BitmapW image, float p)
        {
            p = Util.normalize(p, 0, 255, -100, 100);
            float cr = 0, cg = 0, cb = 0, ca;
            int i = 0, j = 0,
            h = image.Height(),
            w = image.Width();

            for (i = 0; i < w; i++)
            {
                for (j = 0; j < h; j++)
                {
                    Color temp = image.GetPixel(i, j);
                    cr = temp.R; cg = temp.G; cb = temp.B; ca = temp.A;

                    ca += (p);
                    
                    image.SetPixel(i, j, Color.FromArgb((int)Util.clamp(ca, 0, 255),(int)Util.clamp(cr, 0, 255),
                        (int)Util.clamp(cg, 0, 255), (int)Util.clamp(cb, 0, 255)));
                }
            }
            return image;
        }

        // #### Saturate [-100, 100]
        public BitmapW saturate(BitmapW image, float p)
        {
            p = Util.normalize(p, 0, 2, -100, 100);
            float cr = 0, cg = 0, cb = 0, ca;
            int i = 0, j = 0,
            h = image.Height(),
            w = image.Width();

            for (i = 0; i < w; i++)
            {
                for (j = 0; j < h; j++)
                {
                    Color temp = image.GetPixel(i, j);
                    cr = temp.R; cg = temp.G; cb = temp.B; ca = temp.A;
                    float avg = (cr + cg + cb) / 3;

                    cr = avg + p * (cr - avg);
                    cg = avg + p * (cg - avg);
                    cb = avg + p * (cb - avg);

                    image.SetPixel(i, j, Color.FromArgb((int)Util.clamp(ca, 0, 255), (int)Util.clamp(cr, 0, 255),
                        (int)Util.clamp(cg, 0, 255), (int)Util.clamp(cb, 0, 255)));
                }
            }
            return image;
        }

        // #### Invert
        public BitmapW invert(BitmapW image)
        {
            float cr = 0, cg = 0, cb = 0, ca;
            int i = 0, j = 0,
            h = image.Height(),
            w = image.Width();

            for (i = 0; i < w; i++)
            {
                for (j = 0; j < h; j++)
                {
                    Color temp = image.GetPixel(i, j);
                    cr = temp.R; cg = temp.G; cb = temp.B; ca = temp.A;

                    cr = 255-  cr;
                    cg = 255 - cg;
                    cb = 255 - cb;

                    image.SetPixel(i, j, Color.FromArgb((int)Util.clamp(ca, 0, 255), (int)Util.clamp(cr, 0, 255),
                        (int)Util.clamp(cg, 0, 255), (int)Util.clamp(cb, 0, 255)));
                }
            }
            return image;
        }

        // #### Posterize [1, 255]
        public BitmapW posterize(BitmapW image, float p)
        {
            p = Util.clamp(p, 1, 255);
            int step = (int)Math.Floor(255 / p);
            float cr = 0, cg = 0, cb = 0, ca;
            int i = 0, j = 0,
            h = image.Height(),
            w = image.Width();

            for (i = 0; i < w; i++)
            {
                for (j = 0; j < h; j++)
                {
                    Color temp = image.GetPixel(i, j);
                    cr = temp.R; cg = temp.G; cb = temp.B; ca = temp.A;


                    cr = (float)Math.Floor(cr / step) * step;
                    cg = (float)Math.Floor(cg / step) * step;
                    cb = (float)Math.Floor(cb / step) * step;

                    image.SetPixel(i, j, Color.FromArgb((int)Util.clamp(ca, 0, 255), (int)Util.clamp(cr, 0, 255),
                        (int)Util.clamp(cg, 0, 255), (int)Util.clamp(cb, 0, 255)));
                }
            }
            return image;
        }

        // #### Gamma [-100, 100]
        public BitmapW gamma(BitmapW image, float p)
        {
            p = Util.normalize(p, 0, 2, -100, 100);

            float cr = 0, cg = 0, cb = 0, ca;
            int i = 0, j = 0,
            h = image.Height(),
            w = image.Width();

            for (i = 0; i < w; i++)
            {
                for (j = 0; j < h; j++)
                {
                    Color temp = image.GetPixel(i, j);
                    cr = temp.R; cg = temp.G; cb = temp.B; ca = temp.A;

                    cr = (float)Math.Pow(cr, p);
                    cg = (float)Math.Pow(cg, p);
                    cb = (float)Math.Pow(cb, p);

                    image.SetPixel(i, j, Color.FromArgb((int)Util.clamp(ca, 0, 255), (int)Util.clamp(cr, 0, 255),
                        (int)Util.clamp(cg, 0, 255), (int)Util.clamp(cb, 0, 255)));
                }
            }
            return image;
        }

        // #### Constrast [-100, 100]
        float contrastc(float f, float c){return (f - 0.5f) * c + 0.5f;}
        public BitmapW contrast(BitmapW image, float p)
        {
            p = Util.normalize(p, 0, 2, -100, 100);

            float cr = 0, cg = 0, cb = 0, ca;
            int i = 0, j = 0,
            h = image.Height(),
            w = image.Width();

            for (i = 0; i < w; i++)
            {
                for (j = 0; j < h; j++)
                {
                    Color temp = image.GetPixel(i, j);
                    cr = temp.R; cg = temp.G; cb = temp.B; ca = temp.A;

                    cr = 255 * contrastc(cr / 255, p);
                    cg = 255 * contrastc(cg / 255, p);
                    cb = 255 * contrastc(cb / 255, p);

                    image.SetPixel(i, j, Color.FromArgb((int)Util.clamp(ca, 0, 255), (int)Util.clamp(cr, 0, 255),
                        (int)Util.clamp(cg, 0, 255), (int)Util.clamp(cb, 0, 255)));
                }
            }
            return image;
        }

        // #### Sepia
        public BitmapW sepia(BitmapW image)
        {
            float cr = 0, cg = 0, cb = 0, ca;
            int i = 0, j = 0,
            h = image.Height(),
            w = image.Width();

            for (i = 0; i < w; i++)
            {
                for (j = 0; j < h; j++)
                {
                    Color temp = image.GetPixel(i, j);
                    cr = temp.R; cg = temp.G; cb = temp.B; ca = temp.A;
                    float tcr=cr, tcg=cg, tcb=cb;
                    cr = (tcr * 0.393f) + (tcg * 0.769f) + (tcb * 0.189f);
                    cg = (tcr * 0.349f) + (tcg * 0.686f) + (tcb * 0.168f);
                    cb = (tcr * 0.272f) + (tcg * 0.534f) + (tcb * 0.131f);

                    image.SetPixel(i, j, Color.FromArgb((int)Util.clamp(ca, 0, 255), (int)Util.clamp(cr, 0, 255),
                        (int)Util.clamp(cg, 0, 255), (int)Util.clamp(cb, 0, 255)));
                }
            }
            return image;
        }

        // #### Subtract [No Range]
        public BitmapW subtract(BitmapW image)
        {
            float cr = 0, cg = 0, cb = 0, ca;
            int i = 0, j = 0,
            h = image.Height(),
            w = image.Width();

            for (i = 0; i < w; i++)
            {
                for (j = 0; j < h; j++)
                {
                    Color temp = image.GetPixel(i, j);
                    cr = temp.R; cg = temp.G; cb = temp.B; ca = temp.A;

                    cr -= cr;
                    cg -= cg;
                    cb -= cb;

                    image.SetPixel(i, j, Color.FromArgb((int)Util.clamp(ca, 0, 255), (int)Util.clamp(cr, 0, 255),
                        (int)Util.clamp(cg, 0, 255), (int)Util.clamp(cb, 0, 255)));
                }
            }
            return image;
        }

        // #### Fill [No Range]
        public BitmapW fill(BitmapW image,int r,int g,int b)
        {
            float cr = 0, cg = 0, cb = 0, ca;
            int i = 0, j = 0,
            h = image.Height(),
            w = image.Width();

            for (i = 0; i < w; i++)
            {
                for (j = 0; j < h; j++)
                {
                    Color temp = image.GetPixel(i, j);
                    cr = temp.R; cg = temp.G; cb = temp.B; ca = temp.A;

                    cr = r;
                    cg = g;
                    cb = b;

                    image.SetPixel(i, j, Color.FromArgb((int)Util.clamp(ca, 0, 255), (int)Util.clamp(cr, 0, 255),
                        (int)Util.clamp(cg, 0, 255), (int)Util.clamp(cb, 0, 255)));
                }
            }
            return image;
        }



        // #### Blur ['simple', 'gaussian']
        public BitmapW blur(BitmapW image, string p)
        {
            BitmapW result;
            if (p == "simple")
            {
                result = convolve(image, new float[,]{
                {1.0f/9, 1.0f/9, 1.0f/9},
                {1.0f/9, 1.0f/9, 1.0f/9},
                {1.0f/9, 1.0f/9, 1.0f/9}
                },1,1
                );
            }
            else //gaussian
            {
                result = convolve(image, new float[,]{
                {1.0f/273, 4.0f/273, 7.0f/273, 4.0f/273, 1.0f/273},
                {4.0f/273, 16.0f/273, 26.0f/273, 16.0f/273, 4.0f/273},
                {7.0f/273, 26.0f/273, 41.0f/273, 26.0f/273, 7.0f/273},
                {4.0f/273, 16.0f/273, 26.0f/273, 16.0f/273, 4.0f/273},             
                {1.0f/273, 4.0f/273, 7.0f/273, 4.0f/273, 1.0f/273}
                },2,2);
            }

            return result;
        }

        // #### Sharpen
        public BitmapW sharpen(BitmapW image)
        {
            BitmapW result;
            
            result = convolve(image, new float[,]{
            {0.0f, -0.2f,  0.0f},
            {-0.2f, 1.8f, -0.2f},
            {0.0f, -0.2f,  0.0f}
            }, 1, 1
            );
            

            return result;
        }

        public BitmapW curves(BitmapW image, Point s, Point c1, Point c2, Point e)
        {
            Util.Bezier bezier = new Util.Bezier(s,c1,c2,e);
            int[] points = bezier.genColorTable();

            float cr = 0, cg = 0, cb = 0, ca;
            int i = 0, j = 0,
            h = image.Height(),
            w = image.Width();

            for (i = 0; i < w; i++)
            {
                for (j = 0; j < h; j++)
                {
                    Color temp = image.GetPixel(i, j);
                    cr = temp.R; cg = temp.G; cb = temp.B; ca = temp.A;

                    cr = points[(int)cr];
                    cg = points[(int)cg];
                    cb = points[(int)cb];

                    image.SetPixel(i, j, Color.FromArgb((int)Util.clamp(ca, 0, 255), (int)Util.clamp(cr, 0, 255),
                        (int)Util.clamp(cg, 0, 255), (int)Util.clamp(cb, 0, 255)));
                }
            }
            return image;

        }

        // #### Expose [-100, 100]
        public BitmapW expose(BitmapW image, float p)
        {
            p = Util.normalize(p, -1, 1, -100, 100);
            Point c1 = new Point(0, (int)(255 * p));
            Point c2 = new Point((int)(255 - (255 * p)), 255);
            return curves(image,new Point(0,0),c1,c2,new Point(255,255));
        }


    }
}
