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
using JSSoft.Library;
using JSSoft.ModernUI.Framework;
using System;
using System.ComponentModel.Composition;

namespace JSSoft.Fonts.ApplicationHost.MenuItems
{
    [Export(typeof(IMenuItem))]
    [ParentType(typeof(IShell))]
    [Order(0)]
    [CategoryDefinition(Settings)]
    [CategoryDefinition(Recent)]
    [CategoryDefinition(Quit)]
    class FileMenuItem : MenuItemBase
    {
        public const string Settings = nameof(Settings);
        public const string Recent = nameof(Recent);
        public const string Quit = nameof(Quit);

        [ImportingConstructor]
        public FileMenuItem(IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
            this.DisplayName = Resources.MenuItem_File;
        }
    }
}
