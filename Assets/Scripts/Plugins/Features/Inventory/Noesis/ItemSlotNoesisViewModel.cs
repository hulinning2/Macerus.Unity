﻿#if UNITY_5_3_OR_NEWER
#define NOESIS
using Noesis;
#else
using System.Windows.Media;
#endif

using System;
using System.ComponentModel;
using System.Collections.Generic;

using Assets.Scripts.Gui.Noesis.ViewModels;

using Macerus.Plugins.Features.Inventory.Default;
using Macerus.Plugins.Features.Inventory.Api;

namespace Assets.Scripts.Plugins.Features.Inventory.Noesis
{
    public sealed class ItemSlotNoesisViewModel :
        NotifierBase,
        IItemSlotNoesisViewModel
    {
        private static readonly Lazy<IReadOnlyDictionary<string, string>> _lazyNotifierMapping;

        static ItemSlotNoesisViewModel()
        {
            _lazyNotifierMapping = LazyNotifierMappingBuilder.BuildMapping<ItemSlotNoesisViewModel>();
        }

        public ItemSlotNoesisViewModel(
            IItemSlotViewModel viewModelToWrap,
            ImageSource iconImageSource)
        {
            ViewModelToWrap = viewModelToWrap;
            IconImageSource = iconImageSource;

            ViewModelToWrap.PropertyChanged += ViewModelToWrap_PropertyChanged;
        }

        public object Id => ViewModelToWrap.Id;

        public bool HasItem => ViewModelToWrap.HasItem;

        public ImageSource IconImageSource { get; }

        public string SlotLabel => ViewModelToWrap.SlotLabel;

        public bool ShowLabel => ViewModelToWrap.ShowLabel;

        [NotifyForWrappedProperty(nameof(IItemSlotViewModel.IsDragOver))]
        public bool IsDragOver
        {
            get => ViewModelToWrap.IsDragOver;
            set => ViewModelToWrap.IsDragOver = value;
        }

        [NotifyForWrappedProperty(nameof(IItemSlotViewModel.IsDropAllowed))]
        public bool IsDropAllowed
        {
            get => ViewModelToWrap.IsDropAllowed != false;
            set => ViewModelToWrap.IsDropAllowed = value;
        }

        [NotifyForWrappedProperty(nameof(IItemSlotViewModel.IsFocused))]
        public bool IsFocused
        {
            get => ViewModelToWrap.IsFocused;
            set => ViewModelToWrap.IsFocused = value;
        }

        internal IItemSlotViewModel ViewModelToWrap { get; }

        private void ViewModelToWrap_PropertyChanged(
            object sender,
            PropertyChangedEventArgs e)
        {
            if (!_lazyNotifierMapping.Value.TryGetValue(
                e.PropertyName,
                out var propertyName))
            {
                return;
            }

            OnPropertyChanged(propertyName);
        }
    }
}
