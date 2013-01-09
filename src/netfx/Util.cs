using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/*
 * This class provides common toolkit amongst modules. Exports 3 methods and 1 class. 
// ``` clamp(val, min, max) ```
// 
// Ensures a value is between min and max.
// 
// ``` dist(x1, x2) ```
//
// Calculates the absolute distance between two values.
//
// ``` normalize(val, dmin, dmax, smin, smax) ```
//
// Projects a value in the source range into the corresponding
// value in the destination range.
//
// ``` Bezier(C1, C2, C3, C4) ```
//
// A Bezier curve implementation.
 */

namespace netfx
{
    class Util
    {
        public static float clamp(float val, float min, float max)
        {
            return Math.Min(max, Math.Max(min, val));
        }

        public static float dist(float x1, float x2)
        {
            return (float)Math.Sqrt(Math.Pow(x2 - x1, 2));
        }

        public static float normalize(float val, float dmin, float dmax, float smin, float smax)
        {
            float sdist = dist(smin, smax);
            float ddist = dist(dmin, dmax);
            float ratio = ddist / sdist;
            val = clamp(val, smin, smax);
            return dmin + (val-smin) * ratio;
        }


        // **Adapted from (with special thanks)** <br>
        // 13thParallel.org Beziér Curve Code <br>
        // *by Dan Pupius (www.pupius.net)*
        public class Bezier
        {
            Point C1,C2,C3,C4;

            public Bezier(Point c1, Point c2, Point c3, Point c4)
            {
                C1 = c1; C2 = c2; C3 = c3; C4 = c4;
                
            }

            float B1(float t) { return t * t * t; }
            float B2(float t) { return 3 * t * t * (1 - t); }
            float B3(float t) { return 3 * t * (1 - t) * (1 - t); }
            float B4(float t) { return (1 - t) * (1 - t) * (1 - t); }

            Point getPoint(float t)
            {
                return new Point(
                    (int)(C1.X * B1(t) + C2.X * B2(t) + C3.X * B3(t) + C4.X * B4(t)),
                    (int)(C1.Y * B1(t) + C2.Y * B2(t) + C3.Y * B3(t) + C4.Y * B4(t))
                    );
            }

            // Creates a color table for 1024 points. To create the table
            // 1024 bezier points are calculated with t = i/1024 in every
            // loop iteration and map is created for [x] = y. This is then
            // used to project a color RGB value (x) to another color RGB
            // value (y).

            public int[] genColorTable()
            {
                int[] points = new int[1024];

                for (int i = 0; i < 1024; i++)
                {
                    Point point = getPoint(i / 1024.0f);
                    points[point.X] = point.Y;
                }

                return points;
            }
        }



    }
}
