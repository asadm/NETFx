NETFx
=====

NetFx is a C# image manipulation library. This library makes it possible to clone Instagram effect in your own Apps.
The library provides basic building blocks for advanced effects. Think of it as Instagram filters for .NET Developers.
The library is heavily inspired by filtrr2 javascript library by Alex Michael.

Following effects have been implemented:
Brighten
Saturate
Gamma
Adjust
Expose
Curves
Sharpen
Blur
Fill
Subtract
Sepia
Contrast
Posterize
Invert
Alpha

The library also supports photoshop like layer blending modes:
Multiply
Screen
Overlay
Soft Light
Addition
Exclusion
Difference

=====================================

Sample code in C#:
-------------------
effects effects = new effects(); //contains all the basic effects 
layers layers = new layers(); //photoshop like layer implementation

BitmapW a = new BitmapW("D:\megan.jpg"); //load an image
a = effects.sepia(a); //apply the effect
pictureBox1.Image = a.GetBitmap(); //show the resulting image


=====================================
