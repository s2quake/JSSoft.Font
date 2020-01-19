﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSSoft.Font
{
    public interface IShell
    {
        Task OpenAsync(string fontPath);

        Task CloseAsync();

        bool IsOpened { get; }

        string DisplayName { get; }

        IEnumerable<ICharacterGroup> Groups { get; }

        ICharacterGroup SelectedGroup { get; set; }


    }
}
