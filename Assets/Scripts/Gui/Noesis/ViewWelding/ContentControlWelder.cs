﻿#if UNITY_5_3_OR_NEWER
#define NOESIS
using Noesis;
#else
using System.Windows.Controls;
#endif

using ProjectXyz.Framework.ViewWelding.Api.Welders;

namespace Assets.Scripts.Gui.Noesis.ViewWelding
{
    public sealed class ContentControlWelder : ISimpleWelder
    {
        private readonly ContentControl _parent;
        private readonly object _child;

        public ContentControlWelder(
            ContentControl parent,
            object child)
        {
            _parent = parent;
            _child = child;
        }

        public IWeldResult Weld()
        {
            _parent.Content = _child;
            return new WeldResult(_parent, _parent.Content);
        }
    }
}
