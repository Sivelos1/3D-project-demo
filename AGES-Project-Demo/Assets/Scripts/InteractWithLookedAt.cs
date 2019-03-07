using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Detects when a player presses the Interact button when looking at an IInteractable, and calls the IInteractable's InteractWith function.
/// </summary>

public class InteractWithLookedAt : MonoBehaviour
{

    [SerializeField]
    private VisionBasedDetection detectLookedAtInteractive;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Interact") && detectLookedAtInteractive.LookedAtInteractive != null)
        {
            Debug.Log("player did the shit");
            detectLookedAtInteractive.LookedAtInteractive.interactWith();
        }
    }
}
