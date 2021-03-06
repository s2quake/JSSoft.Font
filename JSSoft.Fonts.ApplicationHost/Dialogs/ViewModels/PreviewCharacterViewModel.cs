﻿// MIT License
// 
// Copyright (c) 2020 Jeesu Choi
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

using JSSoft.Fonts.ApplicationHost.Properties;
using JSSoft.ModernUI.Framework;
using System;
using System.Windows;
using System.Windows.Media;

namespace JSSoft.Fonts.ApplicationHost.Dialogs.ViewModels
{
    public class PreviewCharacterViewModel : ModalDialogBase
    {
        public PreviewCharacterViewModel(ICharacter character)
        {
            this.Character = character ?? throw new ArgumentNullException(nameof(character));
            this.DisplayName = Resources.Title_Preview;
        }

        public ICharacter Character { get; }

        public ImageSource ImageSource => this.Character.Source;

        public GlyphMetrics GlyphMetrics => this.Character.GlyphMetrics;

        public Thickness Thickness
        {
            get
            {
                var left = this.GlyphMetrics.HorizontalBearingX;
                var top = this.GlyphMetrics.BaseLine - this.GlyphMetrics.HorizontalBearingY;
                var right = this.GlyphMetrics.HorizontalAdvance - (left + this.GlyphMetrics.Width);
                var bottom = this.GlyphMetrics.VerticalAdvance - (top + this.GlyphMetrics.Height);
                return new Thickness(left, top, right, bottom);
            }
        }
    }
}
