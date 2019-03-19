using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleSetActive : InteractiveObject
{
    [SerializeField]
    [Tooltip("The GameObject to toggle.")]
    private GameObject objectToToggle;

    [SerializeField]
    [Tooltip("Can the player interact with this more than once?")]
    private bool IsResuable = true;

    [SerializeField]
    [Tooltip("After being interacted with, the ToggleSetActive's display text will be changed to this value.")]
    private string toggledDisplayText = "ToggledInteractiveObject";

    //Remembers the ToggleSetInteractive's initial display text so that when toggled back to its initial state, it will recall the original display text.
    private string originalDisplayText;

    private bool hasBeenUsed = false;

    /// <summary>
    /// Toggles the active self value for the objectToToggle when the player interacts with this object.
    /// </summary>
    /// 
    private void Start()
    {
        if (!IsResuable) toggledDisplayText = string.Empty;
        originalDisplayText = displayText;
    }
    public override void InteractWith()
    {
        if (IsResuable || !hasBeenUsed)
        {
            base.InteractWith();
            objectToToggle.SetActive(!objectToToggle.activeSelf);
            if (!objectToToggle.activeSelf)
                displayText = toggledDisplayText;
            else
                displayText = originalDisplayText;
            hasBeenUsed = true;

        }
    }
}
