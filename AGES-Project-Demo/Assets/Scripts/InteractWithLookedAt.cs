using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Detects when a player presses the Interact button when looking at an IInteractable, and calls the IInteractable's InteractWith function.
/// </summary>

public class InteractWithLookedAt : MonoBehaviour
{
    private IInteractive lookedAtInteractive;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Interact") && lookedAtInteractive != null)
        {
            Debug.Log("player did the shit");
            lookedAtInteractive.interactWith();
        }
    }

    private void OnLookedAtInteractiveChanged(IInteractive newLookedAtInteractive)
    {
        lookedAtInteractive = newLookedAtInteractive;
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
