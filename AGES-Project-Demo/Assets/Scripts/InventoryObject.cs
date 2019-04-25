using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryObject : InteractiveObject
{
    //TODO: Add long description field
    //TODO: add icon field

    [SerializeField]
    [Tooltip("The name of the object as displayed in the inventory menu.")]
    private string objectName = nameof(InventoryObject);

    [Tooltip("The text that will display when the player selects this object in the inventory menu.")]
    [TextArea(3, 8)]
    [SerializeField]
    private string description;

    [Tooltip("Icon to display for this item in the inventory menu.")]
    [SerializeField]
    private Sprite icon;

    private Renderer _renderer;
    private Collider _collider;

    private void Start()
    {
        _renderer = GetComponent<Renderer>();
        _collider = GetComponent<Collider>();
    }

    public InventoryObject()
    {
        displayText = $"Take {objectName}";
    }

    /// <summary>
    /// When the player interacts with an inventory object, we need to do 2 things:
    /// 1. add the object to the Player Inventory List.
    /// 2. Remove the object from the game world / scene.
    ///     Cannot use destroy, cause I need to keep gameobject in inventory list. Disable collider and mesh renderer
    /// </summary>
    /// 
    public override void InteractWith()
    {
        base.InteractWith();
        PlayInventory.InventoryObjects.Add(this);
        _collider.enabled = false;
        _renderer.enabled = false;

    }


}
