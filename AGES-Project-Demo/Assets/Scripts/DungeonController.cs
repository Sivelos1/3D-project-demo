using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonController : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Stores DungeonLayoutInformation.")]
    private List<DungeonLayoutInformation> layouts = new List<DungeonLayoutInformation>();

    private DungeonLayoutInformation currentLayout;

    [SerializeField]
    [Tooltip("If, for some reason, a dungeon layout cannot be accessed, this layout will be used.")]
    private DungeonLayoutInformation backupLayout;

    [SerializeField]
    [Tooltip("The Dungeon's name. Displayed when moving to a new floor.")]
    private string dungeonName = "DUMMY";

    [SerializeField]
    [Tooltip("If this value exceeds Maximum Floors, the dungeon is considered complete.")]
    private int currentFloor = 0;

    [SerializeField]
    [Tooltip("The amount of floors that must be traversed to complete the dungeon.")]
    private int maximumFloors = 5;

    [SerializeField]
    [Tooltip("When going to a new floor, the game will display [DUNGEON NAME] BF[Floor] rather than F[Floor].")]
    private bool stairsLeadDown = false;

    #region Data Maps
    //Stores gameObjects as tiles that form the dungeon.
    private GameObject[,] dungeon;

    //Stores ints to determine what kind of tiles will be placed. For example, 0 = Wall. 1 = Floor. 2 = Terrain.
    private int[,] dungeonMap;

    //Analyzes the dungeonMap and applies the appropriate tile variants so everything looks nice.
    private int[,] styleMap;

    //Analyzes the styleMap and rotates the new tiles so they face the correct direction. Stores variables from 0 to 3 - the tile's direction will be set to X*90 - in other words, a value of 1 corresponds to a rotation of 90 degrees on their Y axis, 2 = 180 degrees, and so on.
    private int[,] rotateMap;
    #endregion

    private void Awake()
    {
        currentLayout = getCurrentLayout();
        PrepareDungeon();
    }

    private void Start()
    {

        PrepareDungeonMap();
        BuildDungeon();
    }

    private void BuildDungeon()
    {
        for (int x = 0; x < Mathf.Sqrt(dungeonMap.Length); x++)
        {
            for (int y = 0; y < Mathf.Sqrt(dungeonMap.Length); y++)
            {
                switch (dungeonMap[x, y])
                {
                    case 0:
                        dungeon[x, y] = Instantiate(currentLayout.WallTiles.GetTile(dungeonMap[x, y]), new Vector3(x, 0, y), new Quaternion(0, 0, 0, 0), GameObject.Find(gameObject.name).transform);
                        break;
                    case 1:
                        dungeon[x, y] = Instantiate(currentLayout.FloorTiles.GetTile(dungeonMap[x, y]), new Vector3(x, 0, y), new Quaternion(0, 0, 0, 0), GameObject.Find(gameObject.name).transform);
                        break;
                    case 2:
                        dungeon[x, y] = Instantiate(currentLayout.TerrainTiles.GetTile(dungeonMap[x, y]), new Vector3(x, 0, y), new Quaternion(0, 0, 0, 0), GameObject.Find(gameObject.name).transform);
                        break;
                    default:
                        dungeon[x, y] = Instantiate(currentLayout.FloorTiles.GetTile(dungeonMap[x, y]), new Vector3(x, 0, y), new Quaternion(0, 0, 0, 0), GameObject.Find(gameObject.name).transform);
                        break;
                }
                dungeon[x, y].name = $"x:{x+1}, y:{y+1}";

            }
        }
    }

    //Sets up the 'dungeon' matrix for use.
    private void PrepareDungeon()
    {
        dungeon = new GameObject[currentLayout.FloorSize +(currentLayout.FloorOutlineThickness*2), currentLayout.FloorSize + (currentLayout.FloorOutlineThickness * 2)];
    }

    //Resets the passed in map to all zeroes.
    private int[,] ResetMap(int[,] map)
    {
        map = new int[currentLayout.FloorSize, currentLayout.FloorSize];
        for (int x = 0; x < Mathf.Sqrt(map.Length); x++)
        {
            for (int y = 0; y < Mathf.Sqrt(map.Length); y++)
            {
                map[x, y] = 0;
            }
        }
        return map;

    }

    private void PrepareDungeonMap()
    {
        dungeonMap = ResetMap(dungeonMap);
        BuildRooms();
        SurroundDungeonMapWithWalls();
        
    }

    //Surrounds the dungeonMap with a wall currentFloorLayout.floorOutlineThickness wide. This wall cannot be interacted with.
    private void SurroundDungeonMapWithWalls()
    {
        int[,] newMap = new int[currentLayout.FloorSize + (currentLayout.FloorOutlineThickness*2), currentLayout.FloorSize + (currentLayout.FloorOutlineThickness*2)];
        Debug.Log(Mathf.Sqrt(newMap.Length));
        for (int x = 0; x < (currentLayout.FloorSize + (currentLayout.FloorOutlineThickness * 2)); x++)
        {
            for (int y = 0; y < (currentLayout.FloorSize + (currentLayout.FloorOutlineThickness * 2)); y++)
            {
                newMap[x, y] = 0;
            }
        }
        dungeonMap = CombineTwoDimensionalArrays(newMap, dungeonMap, currentLayout.FloorOutlineThickness, currentLayout.FloorOutlineThickness);
    }

    private DungeonLayoutInformation getCurrentLayout()
    {
        List<DungeonLayoutInformation> tempList = new List<DungeonLayoutInformation>();
        foreach (DungeonLayoutInformation layout in layouts)
        {
            if(currentFloor >= layout.StartingFloor && currentFloor <= layout.FinalFloor)
            {
                tempList.Add(layout);
            }
        }
        if (tempList.Count > 0)
            return tempList[Random.Range(0, tempList.Count)];
        else
            return backupLayout;
    }

    private void BuildRooms()
    {
        int roomsToBuild = Random.Range(currentLayout.MinimumRoomCount, currentLayout.MaxRoomCount);
        for (int i = 0; i < roomsToBuild; i++)
        {
            int roomSizeX = Random.Range(currentLayout.MinimumRoomSizeX, currentLayout.MaxRoomSizeX);
            int roomSizeY = Random.Range(currentLayout.MinimumRoomSizeY, currentLayout.MaxRoomSizeY);
            Debug.Log($"Preparing to create Room of x:{roomSizeX}, y:{roomSizeY}.");
            int[,] newRoom = new int[roomSizeX, roomSizeY];
            for (int x = 0; x < roomSizeX; x++)
            {
                for (int y = 0; y < roomSizeY; y++)
                {
                    newRoom[x, y] = 1;
                    //Debug.Log($"Made x:{x}, y:{y} of new Room.");
                }
            }
            Debug.Log($"Room of x:{roomSizeX}, y:{roomSizeY} complete.");
            int[] roomPos = DetermineRoomCoordinates(roomSizeX, roomSizeY);
            for (int z = 0; z < currentLayout.RoomGenerationFailureLimit || CheckIfCanPutRoom(roomSizeX, roomSizeY, roomPos); z++)
            {
                if (CheckIfCanPutRoom(roomSizeX, roomSizeY, roomPos))
                    break;
                roomPos = DetermineRoomCoordinates(roomSizeX, roomSizeY);
            }
            if (!CheckIfCanPutRoom(roomSizeX, roomSizeY, roomPos))
                continue;
            dungeonMap = CombineTwoDimensionalArrays(dungeonMap, newRoom, roomPos[0], roomPos[1], true, roomSizeX, roomSizeY);
        }
    }

    private int[] DetermineRoomCoordinates(int sizeX, int sizeY)
    {
        int[] coordinates = { 0, 0 };
        coordinates[0] = Mathf.Clamp(Random.Range(0, currentLayout.FloorSize), 0, (currentLayout.FloorSize - sizeX));
        coordinates[1] = Mathf.Clamp(Random.Range(0, currentLayout.FloorSize), 0, (currentLayout.FloorSize - sizeY));

        return coordinates;
    }

    private bool CheckIfCanPutRoom(int sizeX, int sizeY, int[] coordinates)
    {
        Debug.Log("Checking to see if Room can be positioned.");
        for (int x = 0; x < sizeX; x++)
        {
            for (int y = 0; y < sizeY; y++)
            {
                try
                {
                    if (dungeonMap[x, y] == 1)
                        return false;
                    if (dungeonMap[x + (currentLayout.RoomSpacing + 1), y] == 1)
                        return false;
                    if (dungeonMap[x, y + (currentLayout.RoomSpacing + 1)] == 1)
                        return false;
                    if (dungeonMap[x + (currentLayout.RoomSpacing + 1), y + (currentLayout.RoomSpacing + 1)] == 1)
                        return false;
                    if (dungeonMap[x - (currentLayout.RoomSpacing + 1), y] == 1)
                        return false;
                    if (dungeonMap[x, y - (currentLayout.RoomSpacing + 1)] == 1)
                        return false;
                    if (dungeonMap[x - (currentLayout.RoomSpacing + 1), y - (currentLayout.RoomSpacing + 1)] == 1)
                        return false;
                    if (dungeonMap[x + (currentLayout.RoomSpacing + 1), y - (currentLayout.RoomSpacing + 1)] == 1)
                        return false;
                    if (dungeonMap[x - (currentLayout.RoomSpacing + 1), y + (currentLayout.RoomSpacing + 1)] == 1)
                        return false;
                }
                catch (System.IndexOutOfRangeException)
                {
                    continue;
                }
                
            }
        }
        Debug.Log("The room can be placed!");
        return true;
    }

    private int[,] CombineTwoDimensionalArrays(int[,] baseArray, int[,] arrayToAdd, int xOffset = 0, int yOffset = 0, bool addedArrayNotSquare = false, int addedArrayX = 0, int addedArrayY = 0,  int ignoredValue = -1)
    {
        int[,] newArray = baseArray;
        if (addedArrayNotSquare == false)
        {
            for (int x = 0; x < Mathf.Sqrt(baseArray.Length); x++)
            {
                for (int y = 0; y < Mathf.Sqrt(baseArray.Length); y++)
                {
                    try
                    {
                        if (newArray[x + xOffset, y + yOffset] != ignoredValue)
                        {
                            //Debug.Log($"Changing value {baseArray[x + xOffset, y + yOffset]} at x:{x + xOffset}, y:{y + yOffset} to {arrayToAdd[x, y]}");
                            newArray[x + xOffset, y + yOffset] = arrayToAdd[x, y];

                        }
                        else
                            continue;
                    }
                    catch (System.IndexOutOfRangeException)
                    {

                        continue;
                    }
                }
            }
        }
        else
        {
            for (int x = 0; x < addedArrayX; x++)
            {
                for (int y = 0; y < addedArrayY; y++)
                {
                    if (newArray[x + xOffset, y + yOffset] != ignoredValue)
                    {
                        try
                        {
                            //Debug.Log($"Changing value {baseArray[x + xOffset, y + yOffset]} at x:{x + xOffset}, y:{y + yOffset} to {arrayToAdd[x, y]}");
                            newArray[x + xOffset, y + yOffset] = arrayToAdd[x, y];
                        }
                        catch (System.IndexOutOfRangeException)
                        {
                            continue;
                        }

                    }
                    else
                        continue;
                }
            }
        }

        return newArray;
    }
    
}
