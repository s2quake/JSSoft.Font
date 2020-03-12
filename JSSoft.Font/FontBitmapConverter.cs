﻿using SharpFont;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace JSSoft.Font
{
    static class FontBitmapConverter
    {
        public static Bitmap Convert(this FTBitmap ftbmp, Color color, uint charCode)
        {
            switch (ftbmp.PixelMode)
            {
                case PixelMode.Mono:
                    return FromMono(ftbmp, color);
                case PixelMode.Gray4:
                    throw new NotImplementedException();
                case PixelMode.Gray:
                    return FromGray(ftbmp, color);
                case PixelMode.Lcd:
                    throw new NotImplementedException();
                default:
                    return ftbmp.ToGdipBitmap();
            }
        }

        private static Bitmap FromMono(FTBitmap ftbmp, Color color)
        {
            var data = new byte[ftbmp.Rows * ftbmp.Width];
            var bitmap = new Bitmap(ftbmp.Width, ftbmp.Rows, PixelFormat.Format32bppArgb);
            var rect = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
            var data2 = bitmap.LockBits(rect, ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
            var bytes2 = new byte[Math.Abs(data2.Stride) * data2.Height];

            for (var y = 0; y < ftbmp.Rows; y++)
            {
                for (var x = 0; x < ftbmp.Pitch; x++)
                {
                    var v = ftbmp.BufferData[y * ftbmp.Pitch + x];
                    var num = x * 8;
                    var row = y * ftbmp.Width + x * 8;
                    var bits = 8;
                    if ((ftbmp.Width - num) < 8)
                    {
                        bits = ftbmp.Width - num;
                    }
                    for (var i = 0; i < bits; i++)
                    {
                        var bit = v & (1 << (7 - i));
                        data[row + i] = (byte)bit;
                    }
                }
            }

            for (var y = 0; y < bitmap.Height; y++)
            {
                for (var x = 0; x < bitmap.Width; x++)
                {
                    var v = data[(y * bitmap.Width) + x];
                    if (v != 0)
                    {
                        bytes2[x * 4 + y * data2.Stride + 0] = color.B;
                        bytes2[x * 4 + y * data2.Stride + 1] = color.G;
                        bytes2[x * 4 + y * data2.Stride + 2] = color.R;
                        bytes2[x * 4 + y * data2.Stride + 3] = color.A;
                    }
                }
            }
            Marshal.Copy(bytes2, 0, data2.Scan0, bytes2.Length);
            bitmap.UnlockBits(data2);
            return bitmap;
        }

        private static Bitmap FromGray(FTBitmap ftbmp, Color color)
        {
            Bitmap bmp = new Bitmap(ftbmp.Width, ftbmp.Rows, PixelFormat.Format32bppArgb);

            for (var y = 0; y < bmp.Height; y++)
            {
                for (var x = 0; x < bmp.Width; x++)
                {
                    var d = ftbmp.BufferData[y * bmp.Width + x];
                    if (d != 0)
                    {
                        bmp.SetPixel(x, y, Color.FromArgb(d, color.R, color.G, color.B));
                    }
                }
            }
            return bmp;
        }
    }
}
