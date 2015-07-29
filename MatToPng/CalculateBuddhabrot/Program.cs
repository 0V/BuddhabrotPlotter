using Buddhabrot.Models;
using Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace CalculateBuddhabrot
{
    class Program
    {
        static void Main(string[] args)
        {
            const byte ColorR = 255;
            const byte ColorG = 255;
            const byte ColorB = 255;
            const int ImagePixelWidth = 3000;
            const int ImagePixelHeight = 3000;

            //            int PlotCount = 10000;
            //            int Iteration = 1000000;
            //            int count = 10000;

            Console.Write("Plot Count:");
            int PlotCount = Convert.ToInt32(Console.ReadLine());
            Console.Write("Iteratoin:");
            int Iteration = Convert.ToInt32(Console.ReadLine());
            Console.Write("All Count:");
            int count = Convert.ToInt32(Console.ReadLine());


            //            int PlotCount = 100;
            //          int Iteration = 1000;

            var date = DateTime.Now.ToString("yyyyMMddHHmmss");
            var fileSafix = "result/" + date + "_" + Iteration + "_" + PlotCount;
            int ColorAlpha = 1;

            var color = Color.FromRgb(ColorR, ColorG, ColorB);



            for (int i = 0; i < count; i++)
            {
                try
                {
                    var p = new BuddhabrotPlotter(ImagePixelWidth, ImagePixelHeight, color);
                    p.AlphaMagnification = ColorAlpha;

                    Console.Write(i + ":");

                    long[,] pixelMatrix;
//                    WriteableBitmap MainImage;

                    pixelMatrix = p.ExcuteRandom(Iteration, PlotCount).Result;
                    FileSaver.SaveMatrix(pixelMatrix, fileSafix + i + ".csv").Wait();
                    Console.Write("Mat Saved   ");
                    //                    MainImage = p.MatrixToImage(pixelMatrix).Result;
                    //                   FileSaver.SaveImagePng(MainImage, fileSafix + i + ".png").Wait();
                    //                    Console.WriteLine("Image Saved");
                }
                catch (AggregateException ae)
                {
                    var sb = new StringBuilder(DateTime.Now.ToString());
                    sb.AppendLine();
                    foreach (var item in ae.InnerExceptions)
                    {
                        sb.AppendLine(Utils.GetExceptionInfoString(item));
                    }
                    var message = sb.ToString();
                    Utils.WriteNormalLog(message);
                    Console.WriteLine(message);
                    continue;
                }
            }
        }
    }
}
