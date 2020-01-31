﻿using Ntreev.ModernUI.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace JSSoft.Font.ApplicationHost
{
    class Character : PropertyChangedBase, ICharacter
    {
        private readonly FontDescriptor fontDescriptor;
        private bool isEnabled;
        private bool isChecked;
        private ImageSource source;
        private GlyphMetrics glyphMetrics;
        private FontGlyph glyph;

        internal Character(uint id)
        {
            this.ID = id;
        }

        public Character(FontDescriptor fontDescriptor, uint id)
        {
            this.fontDescriptor = fontDescriptor ?? throw new ArgumentNullException(nameof(fontDescriptor));
            this.ID = id;
            this.isEnabled = this.fontDescriptor.Glyphs.ContainsKey(id);
            if (this.fontDescriptor.Glyphs.ContainsKey(id))
            {
                this.glyph = this.fontDescriptor.Glyphs[id];
                this.glyphMetrics = this.fontDescriptor.Glyphs[id].Metrics;
            }
            else
            {
                this.glyphMetrics.VerticalAdvance = this.fontDescriptor.ItemHeight;
            }
            if (this.glyph != null && this.glyph.Bitmap != null)
            {
                var bitmap = this.glyph.Bitmap;
                using (var stream = new MemoryStream())
                {
                    var bitmapImage = new BitmapImage();
                    bitmap.Save(stream, ImageFormat.Png);
                    bitmapImage.BeginInit();
                    bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                    bitmapImage.UriSource = null;
                    bitmapImage.StreamSource = stream;
                    bitmapImage.EndInit();
                    bitmapImage.Freeze();
                    this.source = bitmapImage;
                }
            }
        }

        public override string ToString()
        {
            return $"{(char)this.ID}";
        }

        public uint ID { get; }

        public char Text => (char)this.ID;

        public bool IsEnabled
        {
            get => this.isEnabled;
            set
            {
                this.isEnabled = value;
                this.NotifyOfPropertyChange(nameof(IsEnabled));
            }
        }

        public bool IsChecked
        {
            get => this.isChecked;
            set
            {
                if (this.isChecked != value)
                {
                    this.isChecked = value;
                    this.NotifyOfPropertyChange(nameof(IsChecked));
                }
            }
        }

        public ImageSource Source
        {
            get => this.source;
            set
            {
                this.source = value;
                this.NotifyOfPropertyChange(nameof(Source));
            }
        }

        public GlyphMetrics GlyphMetrics
        {
            get => this.glyphMetrics;
            set
            {
                this.glyphMetrics = value;
                this.NotifyOfPropertyChange(nameof(GlyphMetrics));
            }
        }

        public void SetChecked(bool value)
        {
            if (this.isChecked != value)
            {
                this.isChecked = value;
                this.NotifyOfPropertyChange(nameof(IsChecked));
            }
        }
    }
}
