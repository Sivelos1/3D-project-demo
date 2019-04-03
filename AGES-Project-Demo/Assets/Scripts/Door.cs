using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Door : InteractiveObject
{
    [SerializeField]
    [Tooltip("Check this box to lock the door.")]
    private bool isLocked;

    [SerializeField]
    [Tooltip("If the door is locked, this display text will be used instead.")]
    private string lockedDisplayText = "locked";

    [SerializeField]
    [Tooltip("Played when the player interacts with the door without a key.")]
    private AudioClip lockedAudioClip;

    [SerializeField]
    [Tooltip("Played when the door is open.")]
    private AudioClip openAudioClip;

    public override string DisplayText => isLocked ? lockedDisplayText : base.DisplayText;

    private Animator animator;
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
        animator = GetComponent<Animator>();
    }

    public override void InteractWith()
    {
        if (!isLocked) //if the door is open, then...
        {
            audioSource.clip = openAudioClip;
            animator.SetBool("shouldOpen", true);
            displayText = string.Empty;
            isOpen = true;
        }
        else //if the door is locked, then...
        {
            audioSource.clip = lockedAudioClip;
        }
        base.InteractWith(); //this plays a sound effect
    }
}
