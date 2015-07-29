using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Buddhabrot.Models;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.IO;

namespace MatToPng
{
    class Program
    {
        static void Main(string[] args)
        {

            /*          var pp = new BuddhabrotPlotter(3000, 3000);

                      var m = FileSaver.ReadMatrix("1_6/1_6.csv").Result;

                      FileSaver.SaveMatrix(m, "aa_n" + ".csv").Wait();
                      var ppp = pp.MatrixToImage(m).Result;
                      FileSaver.SaveImagePng(ppp, "aa_n" + ".png").Wait();

                      m = FileSaver.ReadMatrix("aa_n.csv").Result;

                      FileSaver.SaveMatrix(m, "aa_n2" + ".csv").Wait();
                      ppp = pp.MatrixToImage(m).Result;
                      FileSaver.SaveImagePng(ppp, "aa_n2" + ".png").Wait();


                      m = FileSaver.ReadMatrix("aa_n2.csv").Result;

                      FileSaver.SaveMatrix(m, "aa_n3" + ".csv").Wait();
                      ppp = pp.MatrixToImage(m).Result;
                      FileSaver.SaveImagePng(ppp, "aa_n3" + ".png").Wait();

                      return;

          //            var ppp = pp.MatrixToImage(m).Result;
                      FileSaver.SaveTransposedMatrix(m, "aaa_t" + ".csv").Wait();
                      FileSaver.SaveImagePng(ppp, "aaa" + ".png").Wait();

                      var m_t = FileSaver.ReadMatrix("aaa_t.csv").Result;
                      var ppp_t = pp.MatrixToImage(m_t).Result;
                      FileSaver.SaveMatrix(m_t, "aaa_t2" + ".csv").Wait();
                      FileSaver.SaveImagePng(ppp_t, "aaa_t" + ".png").Wait();

                      var m_t2 = FileSaver.ReadMatrix("aaa_t2.csv").Result;
                      var ppp_t2 = pp.MatrixToImage(m_t2).Result;
                      FileSaver.SaveMatrix(m_t2, "aaa_t22" + ".csv").Wait();
                      FileSaver.SaveImagePng(ppp_t2, "aaa_t2" + ".png").Wait();



                      return;
          */



            byte ColorR = 255;
            byte ColorG = 255;
            byte ColorB = 255;
            int ColorAlpha = 1;

            Console.Write("Folder Name:");
            var folderName = Console.ReadLine();

            if (!Directory.Exists(folderName))
            {
                Console.WriteLine("Folder not exists.");
                return;
            }

            Console.Write("Color Alpha:");
            var tmpColorA = Console.ReadLine();
            if (!int.TryParse(tmpColorA, out ColorAlpha))
            {
                ColorAlpha = 1;
            }

            Console.Write("R:");
            var tmpR = Console.ReadLine();
            Console.Write("G:");
            var tmpG = Console.ReadLine();
            Console.Write("B:");
            var tmpB = Console.ReadLine();

            if (!byte.TryParse(tmpR, out ColorR))
            {
                ColorR = 255;
            }
            if (!byte.TryParse(tmpG, out ColorG))
            {
                ColorG = 255;
            }
            if (!byte.TryParse(tmpB, out ColorB))
            {
                ColorB = 255;
            }


            var color = Color.FromRgb(ColorR, ColorG, ColorB);


            Console.WriteLine("Computing ");

            var date = DateTime.Now.ToString("yyyyMMddHHmmss");
            var files = Directory.GetFiles(folderName, "*.csv");


            long[,] pixelMatrix = FileSaver.ReadMatrix(files.First()).Result;

            int ImagePixelWidth = pixelMatrix.GetLength(0);
            int ImagePixelHeight = pixelMatrix.GetLength(1);

            foreach (var item in files.Skip(1))
            {
                var ps = FileSaver.ReadMatrix(item).Result;
                pixelMatrix = FileSaver.AddMatrix(pixelMatrix, ps);
                Console.WriteLine("Add: " + item);
            }

            var p = new BuddhabrotPlotter(ImagePixelWidth, ImagePixelHeight, color);
            p.AlphaMagnification = ColorAlpha;

            FileSaver.SaveMatrix(pixelMatrix, folderName + "/" + folderName + ".csv").Wait();
            Console.Write("Mat Saved   ");
            var img = p.MatrixToImage(pixelMatrix).Result;
            FileSaver.SaveImagePng(img, folderName + "/" + folderName + ".png").Wait();
            Console.WriteLine("Image Saved");
        }
    }
}
