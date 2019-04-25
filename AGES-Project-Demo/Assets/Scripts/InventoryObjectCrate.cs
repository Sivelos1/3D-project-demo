using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class InventoryObjectCrate : InteractiveObject
{
    [SerializeField]
    [Tooltip("The inventory object that will be spawned upon opening the chest.")]
    private InventoryObject loot;

    [SerializeField]
    [Tooltip("Upon opening the chest, the loot will be spawned at the assigned Transform.")]
    private Transform lootSpawnsAtTransform;

    [SerializeField]
    [Tooltip("Assigning a key here will lock the chest. If the player has the key in their inventory, they can open the locked chest.")]
    private InventoryObject key;

    [SerializeField]
    [Tooltip("If checked, the Key Inventory Object will be removed from the player's inventory after unlocking the chest.")]
    private bool consumeKey;

    [SerializeField]
    [Tooltip("If the door is locked, this display text will be used instead.")]
    private string lockedDisplayText = "locked";

    [SerializeField]
    [Tooltip("Played when the player interacts with the chest without a key.")]
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
    public InventoryObjectCrate()
    {
        displayText = nameof(InventoryObjectCrate);
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
            if (isLocked && !HasKey)
            {
                audioSource.clip = lockedAudioClip;
            }
            else //if its not locked, or its locked and we have the key
            {
                audioSource.clip = openAudioClip;
                animator.SetBool("isOpen", true);
                displayText = string.Empty;
                isOpen = true;
                UnlockChest();
            }
            base.InteractWith(); //this plays a sound effect
        }
    }

    private void InsantiateLoot()
    {
        if(loot != null)
        {
            Instantiate(loot, lootSpawnsAtTransform, false);
            loot.transform.position = Vector3.zero;
        }
        else
        {
            Debug.Log("No loot to spawn.");
        }
    }

    private void UnlockChest()
    {
        isLocked = false;
        if (consumeKey && key != null) PlayInventory.InventoryObjects.Remove(key);
    }
}
