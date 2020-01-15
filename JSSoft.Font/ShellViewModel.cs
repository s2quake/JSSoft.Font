﻿using Caliburn.Micro;
using Ntreev.ModernUI.Framework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSSoft.Font
{
    [Export(typeof(IShell))]
    class ShellViewModel : ScreenBase, IShell
    {
        private readonly IFontService fontService;
        private readonly IEnumerable<IMenuItem> menuItems;
        private ObservableCollection<CharactersListBoxItemViewModel> itemList = new ObservableCollection<CharactersListBoxItemViewModel>();
        private ObservableCollection<CharactersListBoxItemViewModel> visibleList = new ObservableCollection<CharactersListBoxItemViewModel>();
        private CharactersListBoxItemViewModel selectedItem;

        [ImportingConstructor]
        public ShellViewModel(IFontService fontService, [ImportMany]IEnumerable<IMenuItem> menuItems)
        {
            this.fontService = fontService;
            this.menuItems = menuItems;
        }

        public async void Open(string fontPath)
        {
            await this.fontService.OpenAsync(fontPath);
            foreach (var (name, min, max) in NamesList.Items)
            {
                var item = new CharactersListBoxItemViewModel(this.fontService, name, min, max);
                this.SatisfyImportsOnce(item);
                this.itemList.Add(item);
            }
            foreach (var item in this.itemList)
            {
                if (item.IsVisible == true)
                    this.visibleList.Add(item);
            }
        }

        public void Open()
        {

        }

        public ObservableCollection<CharactersListBoxItemViewModel> ItemsSource => this.visibleList;

        public CharactersListBoxItemViewModel SelectedItem
        {
            get => this.selectedItem;
            set
            {
                this.selectedItem = value;
                this.NotifyOfPropertyChange(nameof(SelectedItem));
                this.NotifyOfPropertyChange(nameof(CharacterItems));
            }
        }

        public CharacterRowItem[] CharacterItems => this.selectedItem != null ? this.selectedItem.Items : new CharacterRowItem[] { };

        public IEnumerable<IMenuItem> MenuItems => MenuItemUtility.GetMenuItems(this, this.menuItems);

        protected override async void OnDeactivate(bool close)
        {
            if (close == true)
            {
                await this.fontService.CloseAsync();
            }
            base.OnDeactivate(close);
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();
            this.Open(@"SF-Mono-Semibold.otf");
        }
    }
}
