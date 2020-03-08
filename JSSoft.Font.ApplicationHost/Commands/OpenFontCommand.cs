﻿using JSSoft.Font.ApplicationHost.Dialogs.ViewModels;
using JSSoft.Font.ApplicationHost.Properties;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSSoft.Font.ApplicationHost.Commands
{
    public static class OpenFontCommand
    {
        public static bool CanExecute(IShell shell)
        {
            return shell.IsProgressing == false;
        }

        public static async Task ExecuteAsync(IShell shell)
        {
            var dialog = new OpenFileDialog()
            {
                Filter = Resources.FontFilter,
                FilterIndex = 1,
                RestoreDirectory = true,
            };

            if (dialog.ShowDialog() == true)
            {
                await ExecuteAsync(shell, dialog.FileName);
            }
        }

        public static async Task ExecuteAsync(IShell shell, string fontPath)
        {
            var settings = GetSettings(fontPath);
            if (settings != null)
            {
                if (shell.IsOpened == true)
                    await shell.CloseAsync();
                await shell.OpenAsync(fontPath, settings.Size, settings.DPI, settings.FaceIndex);
            }
        }

        private static FontLoadSettingsViewModel GetSettings(string path)
        {
            var dialog = new FontLoadSettingsViewModel(path);
            if (dialog.ShowDialog() == true)
            {
                return dialog;
            }
            return null;
        }
    }
}
