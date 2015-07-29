using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Buddhabrot.Models
{
    public class BuddhabrotPlotter
    {
        public int PixelHeight { get; set; }
        public int PixelWidth { get; set; }
        public double DpiX { get; set; }
        public double DpiY { get; set; }

        public double AlphaMagnification { get; set; }

        public long AlphaThreshold { get; set; }

        public Color PointColor { get; set; }

        private void Initialize()
        {
            DpiX = 640;
            DpiY = 640;
            AlphaMagnification = 1;
            AlphaThreshold = 0;
        }


        public BuddhabrotPlotter(int width, int height)
        {
            PixelWidth = width;
            PixelHeight = height;
            PointColor = Color.FromRgb(255, 255, 255);
        }

        public BuddhabrotPlotter(int width, int height, Color color)
        {
            PixelWidth = width;
            PixelHeight = height;
            PointColor = color;
        }

        public Task<long[,]> Excute(int iteration, double rangeX = 4.0, double rangeY = 4.0)
        {
            long[,] pixel = new long[PixelWidth, PixelHeight];

            int width = PixelWidth;
            int height = PixelHeight;

            var reUnit = rangeX / width;
            var imUnit = rangeY / height;

            var startX = -(rangeX / 2);
            var startY = -(rangeY / 2);
            var endX = (rangeX / 2);
            var endY = (rangeY / 2);

            var task = Task.Run<long[,]>(() =>
            {
                var calc = new BuddhabrotCalculator(iteration);

                var p = Parallel.For(0, width, x =>
                {
                    for (int y = 0; y < height; y++)
                    {
                        var re = startX + (reUnit * x);
                        var im = startY + (imUnit * y);

                        var c = calc.Calculate(re, im);

                        if (c.Divergence == -1) continue;


                        foreach (var item in c.Reasult)
                        {
                            var resultX = (int)Math.Round((item.Real + endX) / reUnit);
                            var resultY = (int)Math.Round((item.Imag + endY) / imUnit);
                            if (resultX < width && resultY < height)
                            {
                                pixel[resultX, resultY]++;
                            }
                        }
                    }
                });
                return pixel;

            });

            return task;
        }

        public Task<long[,]> ExcuteRandom(int iteration, int count, double rangeX = 4.0, double rangeY = 4.0)
        {
            long[,] pixel = new long[PixelWidth, PixelHeight];

            int width = PixelWidth;
            int height = PixelHeight;

            var startX = -(rangeX / 2);
            var startY = -(rangeY / 2);
            var endX = (rangeX / 2);
            var endY = (rangeY / 2);


            var pointX = width / rangeX;
            var pointY = height / rangeY;


            var task = Task.Run(() =>
            {


                var calc = new BuddhabrotCalculator(iteration);
                //                var rand = new Mt19937();
                var rand = new Random();

                var p = Parallel.For(0, count, i =>
                {
                    var re = (rand.NextDouble() * rangeX) - 2.0;
                    var im = (rand.NextDouble() * rangeY) - 2.0;

                    var c = calc.Calculate(re, im);

                    if (c.Divergence != -1)
                    {
                        foreach (var item in c.Reasult)
                        {
                            var resultX = (int)Math.Round((item.Real + endX) * pointX);
                            var resultY = (int)Math.Round((item.Imag + endY) * pointY);
                            if (resultX < width && resultY < height)
                            {
                                pixel[resultX, resultY]++;
                            }
                        }
                    }
                });
                return pixel;
            });
            return task;
        }

        public Task<WriteableBitmap> MatrixToImage(long[,] pixel)
        {
            var width = pixel.GetLength(0);
            var height = pixel.GetLength(1);
            return Task.Run<WriteableBitmap>(() =>
            {
                var tmpImage = new WriteableBitmap(width, height, DpiX, DpiY, PixelFormats.Bgra32, null);

                unsafe
                {
                    tmpImage.Lock();

                    byte* ptr = (byte*)tmpImage.BackBuffer;

                    for (int y = 0; y < height; y++)
                    {
                        for (int x = 0; x < width; x++)
                        {
                            byte* p = ptr + (y * tmpImage.BackBufferStride) + (x * 4);
                            p[0] = PointColor.B;
                            p[1] = PointColor.G;
                            p[2] = PointColor.R;
                            p[3] = 0;
                            long alpha = (long)(pixel[x, y] * AlphaMagnification) - AlphaThreshold;

                            if (alpha > 255) alpha = 255;
                            if (alpha < 0) alpha = 0;

                            p[3] = Convert.ToByte(alpha);
                        }

                    }
                    tmpImage.AddDirtyRect(new Int32Rect(0, 0, width, height));


                    tmpImage.Unlock();
                    tmpImage.Freeze();
                    return tmpImage;
                }
            });

        }

    }
}