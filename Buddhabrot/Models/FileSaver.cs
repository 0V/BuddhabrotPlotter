using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Buddhabrot.Models
{
    public class FileSaver
    {
        public static Task SaveImagePng(WriteableBitmap bitmap, string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName)) return null;

            return Task.Run(() =>
            {
                using (FileStream stream = new FileStream(fileName,
                                                     FileMode.Create, FileAccess.Write))
                {
                    PngBitmapEncoder encoder = new PngBitmapEncoder();
                    encoder.Frames.Add(BitmapFrame.Create(bitmap));
                    encoder.Save(stream);
                }
            });
        }

        public static Task SaveTransposedMatrix(long[,] pixel, string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName)) return null;

            return Task.Run(() =>
            {
                var sb = new StringBuilder();
                sb.AppendLine(pixel.GetLength(0) + "," + pixel.GetLength(1));

                //  pixel[x,y]  pixel[列,行]
                //  出力も画像行列の転置行列
                var width = pixel.GetLength(0);
                var height = pixel.GetLength(1);
                for (int i = 0; i < width; i++)
                {
                    sb.Append(pixel[i, 0]);
                    for (int j = 1; j < height; j++)
                    {
                        sb.Append(",");
                        sb.Append(pixel[i, j]);
                    }
                    sb.AppendLine();
                }
                File.WriteAllText(fileName, sb.ToString());
            });
        }

        public static Task SaveMatrix(long[,] pixel, string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName)) return null;

            return Task.Run(() =>
            {
                var sb = new StringBuilder();
                sb.AppendLine(pixel.GetLength(0) + "," + pixel.GetLength(1));

                //  pixel[x,y]  pixel[列,行]
                //  出力は画像行列
                var width = pixel.GetLength(0);
                var height = pixel.GetLength(1);
                for (int y = 0; y < height; y++)
                {
                    sb.Append(pixel[y, 0]);
                    for (int x = 1; x < width; x++)
                    {
                        sb.Append(",");
                        sb.Append(pixel[x, y]);
                    }
                    sb.AppendLine();
                }
                File.WriteAllText(fileName, sb.ToString());
            });
        }

        public static Task<long[,]> ReadMatrix(string fileName)
        {
            if (!File.Exists(fileName)) return null;

            return Task.Run<long[,]>(() =>
            {
                var data = File.ReadAllLines(fileName);

                int width = Convert.ToInt32(data[0].Split(',')[0]);
                int height = Convert.ToInt32(data[0].Split(',')[1]);

                long[,] pixels = new long[width, height];

                int x = 0;
                int y = 0;
                foreach (var line in data.Skip(1))
                {
                    x = 0;
                    var points = line.Split(',');
                    foreach (var p in points.Where(_ => !string.IsNullOrWhiteSpace(_)))
                    {
                        pixels[x, y] = Convert.ToInt64(p);
                        x++;
                    }
                    y++;
                }
                return pixels;
            });
        }


        public static long[,] AddMatrix(long[,] matA, long[,] matB)
        {
            var width = matA.GetLength(0);
            var height = matA.GetLength(1);
            if (width != matB.GetLength(0) || height != matB.GetLength(1)) throw new ArgumentException("引数の行列は、行と列の数が同じである必要があります。");

            long[,] pixels = new long[width, height];
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    pixels[x, y] = matA[x, y] + matB[x, y];
                }
            }
            return pixels;
        }
    }
}
