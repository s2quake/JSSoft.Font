﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace JSSoft.Font
{
    public interface IFontService
    {
        Task OpenAsync(string path, CancellationToken cancellation);

        Task CloseAsync();

        IReadOnlyDictionary<uint, BitmapSource> Bitmaps { get; }

        IReadOnlyDictionary<uint, GlyphMetrics> Metrics { get; }

        int VerticalAdvance { get; }

        bool IsOpened { get; }

        string Name { get; }

        event EventHandler Opened;

        event EventHandler Closed;
    }
}
