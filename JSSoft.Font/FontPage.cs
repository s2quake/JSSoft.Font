﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSSoft.Font
{
    public sealed class FontPage
    {
        private readonly List<FontGlyphData> glyphList = new List<FontGlyphData>();
        private readonly (byte x, byte y)[,] pixels;
        private readonly FontDataSettings settings;
        private readonly FontNode node;

        public FontPage(int index, string name, FontDataSettings settings)
        {
            this.Index = index;
            this.Name = name ?? throw new ArgumentNullException(nameof(name));
            this.settings = settings;
            this.Width = settings.Width;
            this.Height = settings.Height;
            this.pixels = new (byte, byte)[settings.Width, settings.Height];
            this.node = FontNode.Create(settings);
        }

        public Rectangle Verify(FontGlyph glyph)
        {
            if (glyph.Bitmap == null)
                return Rectangle.Empty;

            var metrics = glyph.Metrics;
            var width = metrics.Width;
            var height = metrics.Height;



            //    var right = location.X + width + padding.Left + padding.Right;
            //    var bottom = location.Y + height + padding.Top + padding.Bottom;
            //    var spacingRight = Math.Min(right + spacing.Horizontal, this.Width);
            //    var spacingBottom = Math.Min(bottom + spacing.Vertical, this.Height);
            //    var spacingRectangle = new Rectangle(location.X, location.Y, spacingRight - location.X, spacingBottom - location.Y);
            //    var rectangle = new Rectangle(location.X + padding.Left, location.Y + padding.Top, width, height);

            return this.node.ReserveRegion(width, height);

            //if (this.HitTest(width, height) is Point)
            //{
            //    return true;
            //}

            //return false;
        }

        public void Add(FontGlyph glyph, Rectangle rectangle)
        {
            if (glyph.Bitmap == null)
                throw new InvalidOperationException();
            if (rectangle == Rectangle.Empty)
                throw new ArgumentException("empty rectangle does not allowed.", nameof(rectangle));

            var padding = this.settings.Padding;
            var spacing = this.settings.Spacing;
            var metrics = glyph.Metrics;
            var width = metrics.Width;
            var height = metrics.Height;

            //this.node.Add(glyph, rectangle);

            //if (this.HitTest(width, height) is Point location)
            //{
            //    var right = location.X + width + padding.Left + padding.Right;
            //    var bottom = location.Y + height + padding.Top + padding.Bottom;
            //    var spacingRight = Math.Min(right + spacing.Horizontal, this.Width);
            //    var spacingBottom = Math.Min(bottom + spacing.Vertical, this.Height);
            //    var spacingRectangle = new Rectangle(location.X, location.Y, spacingRight - location.X, spacingBottom - location.Y);
            //    var rectangle = new Rectangle(location.X + padding.Left, location.Y + padding.Top, width, height);
            //    this.FillRectangle(spacingRectangle);
            this.glyphList.Add(new FontGlyphData(this, glyph, rectangle));
            //}
        }

        public void Save(string filename)
        {
            this.Save(bitmap => bitmap.Save(filename, ImageFormat.Png));
        }

        public void Save(Stream stream)
        {
            this.Save(bitmap => bitmap.Save(stream, ImageFormat.Png));
        }

        public static Color DefaultBackgroundColor { get; } = Color.Transparent;

        public static Color DefaultForegroundColor { get; } = Color.White;

        public static Color DefaultPaddingColor { get; } = Color.Transparent;

        public int Index { get; }

        public string Name { get; }

        public int Width { get; }

        public int Height { get; }

        public Color BackgroundColor { get; set; } = DefaultBackgroundColor;

        public Color ForegroundColor { get; set; } = DefaultForegroundColor;

        public Color PaddingColor { get; set; } = DefaultPaddingColor;

        public IEnumerable<FontGlyphData> Glyphs
        {
            get
            {
                foreach (var item in this.glyphList)
                {
                    yield return item;
                }
            }
        }

        private Point? HitTest(int width, int height)
        {
            var padding = this.settings.Padding;
            var spacing = this.settings.Spacing;

            for (var y = 0; y < this.Height - height; y++)
            {
                for (var x = 0; x < this.Width - width; x++)
                {
                    var right = x + width + padding.Left + padding.Right;
                    var bottom = y + height + padding.Top + padding.Bottom;
                    if (right + spacing.Horizontal < this.Width)
                        right += spacing.Horizontal;
                    if (bottom + spacing.Vertical < this.Height)
                        bottom += spacing.Vertical;
                    var rect = Rectangle.FromLTRB(x, y, right, bottom);
                    if (this.IsEmpty(ref x, ref y, rect) == true)
                    {
                        return rect.Location;
                    }
                }
            }
            return null;
        }

        private bool IsEmpty(ref int x1, ref int y1, Rectangle rectangle)
        {
            if (rectangle.Right >= this.Width || rectangle.Bottom >= this.Height)
                return false;
            for (var x = rectangle.Left; x < rectangle.Right; x++)
            {
                for (var y = rectangle.Top; y < rectangle.Bottom; y++)
                {
                    if (this.pixels[x, y].x != 0)
                    {
                        x1 = this.pixels[x, y].x;
                        return false;
                    }
                    if (this.pixels[x, y].y != 0)
                    {
                        y1 = this.pixels[x, y].y;
                        return false;
                    }
                }
            }
            return true;
        }

        private void FillRectangle(Rectangle rectangle)
        {
            for (var x = rectangle.Left; x < rectangle.Right; x++)
            {
                for (var y = rectangle.Top; y < rectangle.Bottom; y++)
                {
                    this.pixels[x, y] = ((byte)rectangle.Right, (byte)rectangle.Bottom);
                }
            }
        }

        private void Save(Action<Bitmap> action)
        {
            var backgroundBrush = new SolidBrush(this.BackgroundColor);
            var paddingBrush = new SolidBrush(this.PaddingColor);
            var bitmap = new Bitmap(this.Width, this.Height);
            var graphics = Graphics.FromImage(bitmap);
            var padding = this.settings.Padding;

            graphics.CompositingMode = CompositingMode.SourceCopy;
            foreach (var item in this.glyphList)
            {
                var metrics = item.Metrics;
                var glyphBitmap = this.CloneBitmap(item.Bitmap, this.ForegroundColor);
                var rect = new Rectangle(item.Rectangle.Left, item.Rectangle.Top, metrics.Width, metrics.Height);
                var paddingRect = Rectangle.FromLTRB(item.Rectangle.Left - padding.Left, item.Rectangle.Top - padding.Top, item.Rectangle.Right + padding.Right, item.Rectangle.Bottom + padding.Bottom);
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.FillRectangle(paddingBrush, paddingRect);
                graphics.FillRectangle(backgroundBrush, rect);
                graphics.CompositingMode = CompositingMode.SourceOver;
                graphics.DrawImage(glyphBitmap, rect);
                glyphBitmap.Dispose();
            }
            action(bitmap);
        }

        private Bitmap CloneBitmap(Bitmap bitmap, Color color)
        {
            var rect = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
            var newBitmap = bitmap.Clone(rect, PixelFormat.Format32bppArgb) as Bitmap;
            var data = newBitmap.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            var ptr = data.Scan0;
            var length = Math.Abs(data.Stride) * data.Height;
            var bytes = new byte[length];

            System.Runtime.InteropServices.Marshal.Copy(ptr, bytes, 0, length);
            for (var y = 0; y < bitmap.Height; y++)
            {
                for (var x = 0; x < bitmap.Width; x++)
                {
                    byte b = bytes[x * 4 + y * data.Stride + 0];
                    byte g = bytes[x * 4 + y * data.Stride + 1];
                    byte r = bytes[x * 4 + y * data.Stride + 2];
                    byte a = bytes[x * 4 + y * data.Stride + 3];

                    float bf = ((float)b / 255) * ((float)color.B / 255);
                    float gf = ((float)g / 255) * ((float)color.G / 255);
                    float rf = ((float)r / 255) * ((float)color.R / 255);
                    float af = ((float)a / 255) * ((float)color.A / 255);

                    bytes[x * 4 + y * data.Stride + 0] = (byte)(bf * 255.0f);
                    bytes[x * 4 + y * data.Stride + 1] = (byte)(gf * 255.0f);
                    bytes[x * 4 + y * data.Stride + 2] = (byte)(rf * 255.0f);
                    bytes[x * 4 + y * data.Stride + 3] = (byte)(af * 255.0f);
                }
            }
            System.Runtime.InteropServices.Marshal.Copy(bytes, 0, ptr, length);
            newBitmap.UnlockBits(data);
            return newBitmap;
        }
    }
}

