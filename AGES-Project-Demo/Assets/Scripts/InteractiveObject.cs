using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveObject : MonoBehaviour, IInteractive
{
    public string DisplayText => displayText;

    [SerializeField]
    private string displayText;
    
    

    public void interactWith()
    {
        Debug.Log($"Player just interacted with: {gameObject.name}.");
    }
}
