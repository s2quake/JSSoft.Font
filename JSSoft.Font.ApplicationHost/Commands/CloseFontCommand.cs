﻿using JSSoft.Font.ApplicationHost.Dialogs.ViewModels;
using JSSoft.Font.ApplicationHost.Properties;
using Microsoft.Win32;
using Ntreev.ModernUI.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSSoft.Font.ApplicationHost.Commands
{
    public static class CloseFontCommand
    {
        public static bool CanExecute(IShell shell)
        {
            return shell.IsProgressing == false && shell.IsOpened == true;
        }

        public static async Task<bool> ExecuteAsync(IShell shell)
        {
            if (shell.IsModified == true)
            {
                var result = AppMessageBox.ConfirmSaveOnClosing();
                if (result == true)
                {
                    if (shell.SettingsPath != string.Empty)
                    {
                        await shell.SaveSettingsAsync();
                    }
                    else if (SaveSettingsCommand.CanExecute(shell) == true)
                    {
                        if (await SaveSettingsCommand.ExecuteAsync(shell) == false)
                            return false;
                    }
                }
                else if (result == null)
                {
                    return false;
                }
            }
            await shell.CloseAsync();
            return true;
        }
    }
}
