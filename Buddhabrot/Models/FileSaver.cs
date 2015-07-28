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

        public static Task SaveMatrix(long[,] pixel, string fileName)
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
    }
}
