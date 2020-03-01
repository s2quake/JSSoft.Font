﻿using Ntreev.Library;
using Ntreev.ModernUI.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSSoft.Font.ApplicationHost.MenuItems
{
    [Export(typeof(IMenuItem))]
    [ParentType(typeof(IShell))]
    [Order(2)]
    class ToolMenuItem : MenuItemBase
    {
        [ImportingConstructor]
        public ToolMenuItem()
        {
            this.DisplayName = "_Tool";
        }
    }
}
