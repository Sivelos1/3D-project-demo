using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldInteractiveObjects : MonoBehaviour
{
    [SerializeField]
    private IInteractive objectBeingHeld;

    [SerializeField]
    private Transform heldPosition;
}
