using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
/*
 This class is a wrap around the original bitmap so the rest of the code is independant of the C# Bitmap
 This is a simple class in this C# implementation but if the code is ported then this would become an important class.
 * 
 */
namespace filtrr2_port
{
    class BitmapW
    {
        LockBitmap holder;
        bool bitmaplock=false;

        public BitmapW(String filename)
        {
            holder = new LockBitmap(new Bitmap(filename));
            if (!bitmaplock) { holder.LockBits(); bitmaplock = true; }
        }

        BitmapW(Bitmap a)
        {
            holder = new LockBitmap(a);
            if (!bitmaplock) { holder.LockBits(); bitmaplock = true; }
        }

        public void SetPixel(int x, int y, Color color)
        {
            if (!bitmaplock) { holder.LockBits(); bitmaplock = true; }
            holder.SetPixel(x, y, color);
        }

        public Color GetPixel(int x, int y)
        {
            return holder.GetPixel(x, y);
        }

        public int Width()
        {
            return holder.Width;
        }

        public int Height()
        {
            return holder.Height;
        }
        public Bitmap GetBitmap()
        {
            if (bitmaplock) { holder.UnlockBits(); bitmaplock = false; }
            return holder.GetBitmap();
        }
        public BitmapW Clone()
        {
            if (bitmaplock) { holder.UnlockBits(); bitmaplock = false; }
            return new BitmapW((Bitmap)holder.GetBitmap().Clone());
        }

    }
}
