﻿using UnityEngine;

namespace Assets.Scripts.Input.Api
{
    public interface IKeyboardControls
    {
        KeyCode QuickSlot1 { get; }

        KeyCode QuickSlot2 { get; }

        KeyCode QuickSlot3 { get; }

        KeyCode MoveLeft { get; }

        KeyCode MoveRight { get; }

        KeyCode MoveUp { get; }

        KeyCode MoveDown { get; }

        KeyCode Interact { get; }

        KeyCode ToggleInventory { get; }

        KeyCode ToggleCharacterSheet { get; }

        KeyCode GuiClose { get; }
    }
}