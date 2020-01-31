﻿using Ntreev.Library.Threading;
using SharpFont;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace JSSoft.Font
{
    public sealed class FontDescriptor : IDisposable
    {
        private readonly Dictionary<uint, FontGlyph> glyphByID = new Dictionary<uint, FontGlyph>();
        private Library lib;
        private Face face;

        public FontDescriptor(string path, uint dpi, int height)
        {
            var pixelSize = (double)height * dpi / 72;
            this.lib = new Library();
            this.face = new Face(this.lib, Path.GetFullPath(path));
            this.face.SetCharSize(0, height, 0, dpi);
            this.ItemHeight = (int)Math.Round(this.face.Height * pixelSize / this.face.UnitsPerEM);
            var (min, max) = NamesList.Range;
            for (var i = min; i <= max; i++)
            {
                this.RegisterItem(i);
            }
            this.Name = this.face.FamilyName;
            this.DPI = dpi;
            this.Height = height;
        }

        public uint DPI { get; private set; }

        public int Height { get; private set; }

        public int ItemHeight { get; private set; }

        public string Name { get; private set; } = string.Empty;

        public IReadOnlyDictionary<uint, FontGlyph> Glyphs => this.glyphByID;

        private void RegisterItem(uint charCode)
        {
            var glyph = this.CreateGlyph(charCode);
            if (glyph == null)
                return;

            var ftbmp = glyph.Bitmap;
            var metrics = glyph.Metrics;
            var height = (double)Math.Round((double)glyph.LinearVerticalAdvance);
            _ = (double)Math.Round((double)glyph.LinearHorizontalAdvance);
            var baseLine = height + (height * glyph.Face.Descender / glyph.Face.Height);
            var glyphMetrics = new GlyphMetrics()
            {
                ID = charCode,
                Width = (int)metrics.Width,
                Height = (int)metrics.Height,
                HorizontalBearingX = (int)metrics.HorizontalBearingX,
                HorizontalBearingY = (int)metrics.HorizontalBearingY,
                HorizontalAdvance = (int)metrics.HorizontalAdvance,
                VerticalBearingX = (int)metrics.VerticalBearingX,
                VerticalBearingY = (int)metrics.VerticalBearingY,
                VerticalAdvance = (int)metrics.VerticalAdvance,
                BaseLine = (int)Math.Round(baseLine),
            };
            var charItem = new FontGlyph()
            {
                ID = charCode,
                Bitmap = this.CreateBitmap(ftbmp),
                Metrics = glyphMetrics,
            };
            this.glyphByID.Add(charCode, charItem);
            //var bitmap = this.CreateBitmap(ftbmp);
            //this.metricsByID.Add(charCode, glyphMetrics);
            //if (bitmap != null)
            //{
            //    this.bitmapByID.Add(charCode, bitmap);
            //}
        }

        private Bitmap CreateBitmap(FTBitmap ftbmp)
        {
            if (ftbmp.Rows > 0 && ftbmp.Width > 0)
            {
                var bitmap = ftbmp.ToGdipBitmap(System.Drawing.Color.White);
                return bitmap;
                //using (var stream = new MemoryStream())
                //{
                //    var bitmapImage = new BitmapImage();
                //    bitmap.Save(stream, ImageFormat.Png);
                //    bitmapImage.BeginInit();
                //    bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                //    bitmapImage.UriSource = null;
                //    bitmapImage.StreamSource = stream;
                //    bitmapImage.EndInit();
                //    bitmapImage.Freeze();
                //    return bitmapImage;
                //}
            }
            return null;
        }

        private GlyphSlot CreateGlyph(uint charCode)
        {
            var index = this.face.GetCharIndex(charCode);
            if (index == 0)
                return null;
            this.face.LoadGlyph(index, LoadFlags.Default, LoadTarget.Normal);
            this.face.Glyph.RenderGlyph(RenderMode.Normal);
            return this.face.Glyph;
        }

        public void Dispose()
        {
            this.glyphByID.Clear();
            this.ItemHeight = 0;
            this.face?.Dispose();
            this.face = null;
            this.lib?.Dispose();
            this.lib = null;
        }
    }
}
