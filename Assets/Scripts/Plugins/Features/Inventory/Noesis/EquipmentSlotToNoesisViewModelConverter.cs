﻿#if UNITY_5_3_OR_NEWER
#define NOESIS

#else

#endif

using System;
using Macerus.Plugins.Features.Inventory.Api;
using Assets.Scripts.Gui.Noesis;
using System.Collections.Generic;

namespace Assets.Scripts.Plugins.Features.Inventory.Noesis
{
    public sealed class EquipmentSlotToNoesisViewModelConverter : IEquipmentSlotToNoesisViewModelConverter
    {
        private readonly IResourceImageSourceFactory _resourceImageSourceFactory;

        public EquipmentSlotToNoesisViewModelConverter(IResourceImageSourceFactory resourceImageSourceFactory)
        {
            _resourceImageSourceFactory = resourceImageSourceFactory;
        }

        public IItemSlotNoesisViewModel Convert(IItemSlotViewModel input)
        {
            if (input == null)
            {
                return null;
            }

            var imageSource = input.IconResourceId == null
                ? null
                : _resourceImageSourceFactory.CreateForResourceId(input.IconResourceId);
            var gridPosition = GridPositionForSlot(input.Id);
            var viewModel = new EquipmentSlotNoesisViewModel(
                input,
                imageSource,
                gridPosition.Row,
                gridPosition.RowSpan,
                gridPosition.Column,
                gridPosition.ColumnSpan);
            return viewModel;
        }

        public IItemSlotViewModel ConvertBack(IItemSlotNoesisViewModel input)
        {
            if (input == null)
            {
                return null;
            }

            // FIXME: i am sorry for this filth.
            if (!(input is EquipmentSlotNoesisViewModel))
            {
                throw new NotSupportedException(
                    $"Currently there is only support for converting back from '{typeof(EquipmentSlotNoesisViewModel)}'.");
            }

            return ((EquipmentSlotNoesisViewModel)input).ViewModelToWrap;
        }

        // FIXME: find a way to some day refactor this out to make it dynamic
        private GridPosition GridPositionForSlot(object slotId)
        {
            var slot = (string)slotId;
            var lookup = new Dictionary<string, GridPosition>()
            {
                ["amulet"] = new GridPosition()     { Row = 0, Column = 0, RowSpan = 1, ColumnSpan = 1 },
                ["ring1"] = new GridPosition()      { Row = 1, Column = 0, RowSpan = 1, ColumnSpan = 1 },
                ["ring2"] = new GridPosition()      { Row = 1, Column = 1, RowSpan = 1, ColumnSpan = 1 },
                ["head"] = new GridPosition()       { Row = 0, Column = 2, RowSpan = 2, ColumnSpan = 2 },
                ["shoulders"] = new GridPosition()  { Row = 0, Column = 4, RowSpan = 2, ColumnSpan = 2 },
                ["left hand"] = new GridPosition()  { Row = 2, Column = 0, RowSpan = 2, ColumnSpan = 2 },
                ["body"] = new GridPosition()       { Row = 2, Column = 2, RowSpan = 2, ColumnSpan = 2 },
                ["right hand"] = new GridPosition() { Row = 2, Column = 4, RowSpan = 2, ColumnSpan = 2 },
                ["waist"] = new GridPosition()      { Row = 4, Column = 0, RowSpan = 2, ColumnSpan = 2 },
                ["legs"] = new GridPosition()       { Row = 4, Column = 2, RowSpan = 2, ColumnSpan = 2 },
                ["back"] = new GridPosition()       { Row = 4, Column = 4, RowSpan = 2, ColumnSpan = 2 },
                ["hands"] = new GridPosition()      { Row = 6, Column = 0, RowSpan = 2, ColumnSpan = 2 },
                ["feet"] = new GridPosition()       { Row = 6, Column = 2, RowSpan = 2, ColumnSpan = 2 },
            };

            return lookup[slot];
        }

        private struct GridPosition
        {
            public int Row;

            public int RowSpan;

            public int Column;

            public int ColumnSpan;
        }
    }
}