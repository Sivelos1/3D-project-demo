using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This UI text displays info about the currently looked at interactive IInteractive.
/// The text should be hidden if the player is not currently looking at an interactive element.
/// </summary>

public class LookedAtInteractiveDisplayText : MonoBehaviour
{
    private IInteractive lookedAtInteractive;
    private Text displayText;

    private void Awake()
    {
        displayText = GetComponent<Text>();
    }

    private void UpdateDisplayText()
    {
        if(lookedAtInteractive != null)
        {
            displayText.text = lookedAtInteractive.DisplayText;
        }
        else
        {
            displayText.text = string.Empty;
        }
    }
    /// <summary>
    /// Event handler for VisionBasedDetection.LookedAtInteractiveChanged
    /// </summary>
    /// <param name="newLookedAtInteractive"> Reference to the new IINteractive the player is looking at</param>
    private void OnLookedAtInteractiveChanged(IInteractive newLookedAtInteractive)
    {
        UpdateDisplayText();
    }

    #region Event Subscription / Unsubscription
    private void OnEnable()
    {
        VisionBasedDetection.LookedAtInteractiveChanged += OnLookedAtInteractiveChanged;
    }
    private void OnDisable()
    {
        VisionBasedDetection.LookedAtInteractiveChanged -= OnLookedAtInteractiveChanged;
    }
    #endregion
}
