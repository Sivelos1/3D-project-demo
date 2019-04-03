using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class InteractiveObject : MonoBehaviour, IInteractive
{
    public virtual string DisplayText => displayText;

    [SerializeField]
    protected string displayText = nameof(InteractiveObject);

    protected AudioSource audioSource;

    protected virtual void Awake()
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
