using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Information that is referenced when a randomly generated dungeon is being built.
/// </summary>

public class DungeonLayoutInformation : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Provides objects to build the dungeon floor with.")]
    private DungeonTilesetInformation tilesetInformation;

    [SerializeField]
    [Tooltip("The lowest floor value for this dungeon layout to begin generating at.")]
    int minimumFloorRange = 1;

    [SerializeField]
    [Tooltip("The highest floor value for this dungeon layout to begin generating at. After proceeding to the next floor, the dungeon fetches the next layout.")]
    int maximumFloorRange = 2;

    [SerializeField]
    [Tooltip("The minimum amount of tiles a dungeon chamber's width will be. ")]
    int minimumRoomSizeX = 3;

    [SerializeField]
    [Tooltip("The maximum amount of tiles a dungeon chamber's width will be. ")]
    int maximumRoomSizeX = 5;

    [SerializeField]
    [Tooltip("The minimum amount of tiles a dungeon chamber's depth will be. ")]
    int minimumRoomSizeY = 3;

    [SerializeField]
    [Tooltip("The maximum amount of tiles a dungeon chamber's depth will be. ")]
    int maximumRoomSizeY = 5;

    //Determines how many rooms will be generated. The lower it is, the more less generated. CANNOT be zero.
    public int roomFrequency { get; private set; }
}
