﻿using UnityEngine.UI;

namespace Assets.Scripts.Plugins.Features.Hud.Inventory.Api
{
    public interface IDragItemFactory
    {
        IDragItemPrefab Create(Image sourceIcon);
    }
}