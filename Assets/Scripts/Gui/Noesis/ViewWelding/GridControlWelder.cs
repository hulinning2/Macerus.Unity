﻿#if UNITY_5_3_OR_NEWER
#define NOESIS
using Noesis;
#else
using System.Windows;
using System.Windows.Controls;
#endif

using ProjectXyz.Framework.ViewWelding.Api.Welders;

namespace Assets.Scripts.Gui.Noesis.ViewWelding
{
    public sealed class GridControlWelder : ISimpleWelder
    {
        private readonly Grid _parent;
        private readonly UIElement _child;

        public GridControlWelder(
            Grid parent,
            UIElement child)
        {
            _parent = parent;
            _child = child;
        }

        public IWeldResult Weld()
        {
            _parent.Children.Add(_child);
            return new WeldResult(_parent, _child);
        }
    }
}
