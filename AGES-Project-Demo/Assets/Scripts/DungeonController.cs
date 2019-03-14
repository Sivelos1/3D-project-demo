using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonController : MonoBehaviour
{

    [SerializeField]
    [Tooltip("Provides layouts for the Dungeon Controller to create.")]
    private List<DungeonLayoutInformation> layouts = new List<DungeonLayoutInformation>();

    [SerializeField]
    [Tooltip("Creates a square area for the dungeon to be built inside. This number affects how many tiles long and wide the area will be.")]
    private int dungeonAreaSize = 25;

    [SerializeField]
    [Tooltip("Determines which floor the player is currently at. If it exceeds the maximumFloors value, the dungeon is considered complete.")]
    private int currentFloor;

    [SerializeField]
    [Tooltip("The number of times the player must reach the end of a dungeon floor to complete the dungeon.")]
    private int maximumFloors;

    //A 2-Dimensional list that stores values that correspond to specific tiles.
    private List<List<int>> dungeonInfo = new List<List<int>>();

    private void Start()
    {
        for (int i = 0; i < dungeonAreaSize; i++)
        {
            dungeonInfo[i] = new List<int>(dungeonAreaSize);
        }

        for (int x = 0; x < dungeonAreaSize; x++)
        {
            for (int y = 0; y < dungeonAreaSize; y++)
            {
                dungeonInfo[x][y] = 0;
            }
        }
    }

    private void BuildDungeon()
    {
        GameObject dungeon = new GameObject();
        for (int x = 0; x < dungeonAreaSize; x++)
        {
            for (int y = 0; y < dungeonAreaSize; y++)
            {
                dungeon.AddComponent<Rigidbody>();

            }
        }
    }

    //Fetches a tile corresponding to a value in dungeonInfo. 0 = Floors, 1 = Walls, etc.
    //private GameObject FindTileFromValue(int arrayValue)
    //{
    //    switch (arrayValue)
    //    {
    //        case 0:


    //    }
    //}

    //private DungeonLayoutInformation GetCurrentLayout()
    //{
    //    foreach (DungeonLayoutInformation layout in layouts)
    //    {
    //        if(currentFloor >= )
    //    }
    //}
}
