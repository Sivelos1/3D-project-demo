﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class InteractiveObject : MonoBehaviour, IInteractive
{
    public string DisplayText => displayText;

    [SerializeField]
    protected string displayText = nameof(InteractiveObject);
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }



    public virtual void InteractWith()
    {
        if(audioSource != null)
            audioSource.Play();
        Debug.Log($"Player just interacted with: {gameObject.name}.");
    }
}
