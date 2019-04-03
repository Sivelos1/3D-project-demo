using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Door : InteractiveObject
{

    [SerializeField]
    [Tooltip("Assigning a key here will lock the door. If the player has the key in their inventory, they can open the locked door.")]
    private InventoryObject key;

    [SerializeField]
    [Tooltip("If checked, the Key Inventory Object will be removed from the player's inventory after unlocking the door.")]
    private bool consumeKey;

    [SerializeField]
    [Tooltip("If the door is locked, this display text will be used instead.")]
    private string lockedDisplayText = "locked";

    [SerializeField]
    [Tooltip("Played when the player interacts with the door without a key.")]
    private AudioClip lockedAudioClip;

    [SerializeField]
    [Tooltip("Played when the door is open.")]
    private AudioClip openAudioClip;

    public override string DisplayText => (isLocked && !HasKey) ? lockedDisplayText : base.DisplayText;
    private bool HasKey => PlayInventory.InventoryObjects.Contains(key);
    

    private Animator animator;
    private bool isLocked;
    private bool isOpen = false;
    private int shouldOpenAnimParameter = Animator.StringToHash(nameof(shouldOpenAnimParameter));

    /// <summary>
    /// Using a constructor to initialize Display Text in editor.
    /// </summary>
    public Door()
    {
        displayText = nameof(Door);
    }

    protected override void Awake()
    {
        base.Awake();
        InitializeKey();
        animator = GetComponent<Animator>();
    }

    private void InitializeKey()
    {
        if (key != null)
            isLocked = true;
        else isLocked = false;
    }

    public override void InteractWith()
    {
        if (!isOpen)
        {
            if(isLocked && !HasKey)
            {
                audioSource.clip = lockedAudioClip;
            }
            else //if its not locked, or its locked and we have the key
            {
                audioSource.clip = openAudioClip;
                animator.SetBool("shouldOpen", true);
                displayText = string.Empty;
                isOpen = true;
                UnlockDoor();
            }
            base.InteractWith(); //this plays a sound effect
        }
    }

    private void UnlockDoor()
    {
        isLocked = false;
        if (consumeKey && key != null) PlayInventory.InventoryObjects.Remove(key);
    }
}
