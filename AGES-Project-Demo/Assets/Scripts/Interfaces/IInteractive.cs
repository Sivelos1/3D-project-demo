﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Interface for elements that the player can interact with by pressing the interact button.
/// </summary>

public interface IInteractive
{
    void InteractWith();
    string DisplayText { get; }

}
