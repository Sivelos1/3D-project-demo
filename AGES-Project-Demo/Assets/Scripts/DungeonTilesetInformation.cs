using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script holds tileset information for randomly-generated dungeons. For example, what tiles to use for specific values.
/// </summary>
public class DungeonTilesetInformation : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The tileset's name.")]
    private string Name;
    
    [Tooltip("Tiles that will be used for the dungeon's floor.")]
    List<GameObject> floorTiles = new List<GameObject>();
    
    [Tooltip("Tiles that will be used at a wall.")]
    List<GameObject> wallTiles = new List<GameObject>();
}
