﻿using Microsoft.Win32;
using Ntreev.ModernUI.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSSoft.Font.ApplicationHost.ToolBarItems
{
    [Export(typeof(IToolBarItem))]
    [ParentType(typeof(IShell))]
    class ZoomOutToolBarItem : ToolBarItemBase
    {
        private readonly Lazy<IShell> shell;

        [ImportingConstructor]
        public ZoomOutToolBarItem(Lazy<IShell> shell)
        {
            this.shell = shell;
            this.Icon = "Images/zoom-out.png";
            this.DisplayName = "Zoom Out";
        }

        protected override bool OnCanExecute(object parameter) => this.Shell.IsProgressing == false;

        protected override void OnExecute(object parameter) => this.Shell.ZoomLevel /= 2.0;

        private IShell Shell => this.shell.Value;
    }
}
