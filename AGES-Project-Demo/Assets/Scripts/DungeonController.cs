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

    //Generates a series of data-maps and applies them all to the dungeonMap for use in generating the dungeon.
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

    //Locates the layout being currently used by the Dungeon, and assigns it to the currentLayout variable.
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

    //Creates a series of boxes to be placed in the dungeon.
    private void BuildRooms()
    {
        int[,] newMap = new int[Get2DArraySizeX(dungeonMap), Get2DArraySizeY(dungeonMap)];
        newMap = ResetMap(newMap);
        int roomsToBuild = Random.Range(currentLayout.MinimumRoomCount, currentLayout.MaxRoomCount);
        List<int[,]> rooms = new List<int[,]>();
        List<int[]> roomCoordinates = new List<int[]>();
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
            rooms.Add(newRoom);
            roomCoordinates.Add(roomPos);
            //newMap = CombineTwoDimensionalArrays(newMap, newRoom, roomPos[0], roomPos[1]);
        }
        for (int i = 0; i < rooms.Count; i++)
        {
            int connectingRoom = Random.Range(0, rooms.Count);
            while (connectingRoom == i)
            {
                connectingRoom = Random.Range(0, rooms.Count);
            }
            newMap = CreateHallway(rooms[i], roomCoordinates[i], rooms[connectingRoom], roomCoordinates[connectingRoom]);
        }

        dungeonMap = CombineTwoDimensionalArrays(dungeonMap, newMap);
    }

    //Returns the size of the room's width.
    private int Get2DArraySizeX(int[,] array)
    {
        return array.GetLength(0);
    }

    //Returns the size of the room's depth.
    private int Get2DArraySizeY(int[,] array)
    {
        return array.GetLength(1);
    }

    private int[,] CreateHallway(int[,] startingRoom, int[] startingRoomCoordinates, int[,] connectingRoom, int[] connectingRoomCoordinates)
    {
        int[,] newMap = new int[Get2DArraySizeX(dungeonMap), Get2DArraySizeY(dungeonMap)];

        newMap = CombineTwoDimensionalArrays(newMap, startingRoom, startingRoomCoordinates[0], startingRoomCoordinates[1]);
        newMap = CombineTwoDimensionalArrays(newMap, connectingRoom, connectingRoomCoordinates[0], connectingRoomCoordinates[1]);

        int[] startingAnchor = { 0, 0 }, connectingAnchor = { 0, 0 };
        List<int[]> anchors = new List<int[]>();

        for (int i = 0; i < startingRoom.Length; i++)
        {
            if (CheckIfCanPutHallwayAnchor(startingAnchor[0], startingAnchor[1]))
            {
                startingAnchor = GetRandomCoordinateAtEdgeOfArray(startingRoom);
                startingAnchor[0] += startingRoomCoordinates[0];
                startingAnchor[1] += startingRoomCoordinates[1];
            }
        }
        anchors.Add(startingAnchor);
        for (int i = 0; i < connectingRoom.Length; i++)
        {
            if (CheckIfCanPutHallwayAnchor(connectingAnchor[0], connectingAnchor[1]))
            {
                connectingAnchor = GetRandomCoordinateAtEdgeOfArray(startingRoom);
                connectingAnchor[0] += connectingRoomCoordinates[0];
                connectingAnchor[1] += connectingRoomCoordinates[1];
                break;
            }
        }

        newMap[startingAnchor[0], startingAnchor[1]] = 2;
        newMap[connectingAnchor[0], connectingAnchor[1]] = 2;

        int[] newAnchor = startingAnchor;

        while (newAnchor != connectingAnchor)
        {
            newAnchor[0] = Random.Range((anchors[anchors.Count][0] - currentLayout.RoomSpacing), (anchors[anchors.Count][0] + currentLayout.RoomSpacing));
            newAnchor[1] = Random.Range((anchors[anchors.Count][1] - currentLayout.RoomSpacing), (anchors[anchors.Count][1] + currentLayout.RoomSpacing));
            try
            {
                if (CheckIfCanPutHallwayAnchor(newAnchor[0], newAnchor[1]))
                {
                    anchors.Add(newAnchor);
                    newMap[newAnchor[0], newAnchor[1]] = 2;
                    break;
                }
            }
            catch (System.IndexOutOfRangeException)
            {
                continue;
            }
            
        }



        #region slobmyknowb
        //int[] hallCoords = GetRandomCoordinateAtEdgeOfArray(startingRoom);
        //hallCoords[0] += startingRoomCoordinates[0];
        //hallCoords[1] += startingRoomCoordinates[1];

        //newMap[hallCoords[0], hallCoords[1]] = 2;

        //hallCoords = GetRandomCoordinateAtEdgeOfArray(connectingRoom);
        //hallCoords[0] += connectingRoomCoordinates[0];
        //hallCoords[1] += connectingRoomCoordinates[1];

        //newMap[hallCoords[0], hallCoords[1]] = 2;
        #endregion

        return newMap;

    }

    private int[] GetRandomCoordinateAtEdgeOfArray(int[,] array)
    {
        List<int[]> edgeCoordinates = new List<int[]>();
        for (int x = 0; x < Get2DArraySizeX(array); x++)
        {
            for (int y = 0; y < Get2DArraySizeY(array); y++)
            {
                if(IsCoordinateAtEdgeOfArray(x,y, array))
                {
                    int[] newEdgeCoordinate = { x, y };
                    edgeCoordinates.Add(newEdgeCoordinate);
                }
            }
        }
        return edgeCoordinates[Random.Range(0, edgeCoordinates.Count)];
    }

    private bool IsCoordinateAtEdgeOfArray(int x, int y, int[,] array)
    {
        if ((x > 0 && x < Get2DArraySizeX(array)) && (y > 0 && y < Get2DArraySizeY(array)))
            return false;
        else
        {
            Debug.Log($"x:{x}, y:{y} is at the edge of the Array size x:{Get2DArraySizeX(array)}, y{Get2DArraySizeY(array)}!");
            return true;
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
        int overLappingTiles = 0;
        Debug.Log($"Checking to see if Room of size x:{sizeX}, y:{sizeY} can be positioned at x:{coordinates[0]}, y:{coordinates[1]}.");
        for (int x = (coordinates[0] - currentLayout.RoomSpacing); x < (sizeX + coordinates[0] + (currentLayout.RoomSpacing*2)); x++)
        {
            for (int y = (coordinates[1] - currentLayout.RoomSpacing); y < (sizeY + coordinates[1] + (currentLayout.RoomSpacing * 2)); y++)
            {
                try
                {
                    if (dungeonMap[x, y] == 1)
                    {
                        overLappingTiles++;
                    }
                }
                catch (System.IndexOutOfRangeException)
                {
                    continue;
                }
            }
        }
        if(overLappingTiles == 0)
        {
            Debug.Log("The room can be placed!");
            return true;
        }
        else
        {
            Debug.Log("The room cannot be placed.");
            return false;
        }
    }

    private bool CheckIfCanPutHallwayAnchor(int targetX, int targetY)
    {
        int tilesThatHallwayAnchorsCannotOccupy = 0;
        Debug.Log($"Checking to see if Hallway Anchor of x:{targetX}, y:{targetY} can be placed.");
        for (int x = (targetX - currentLayout.RoomSpacing); x < (targetX + currentLayout.RoomSpacing); x++)
        {
            for (int y = (targetY - currentLayout.RoomSpacing); y < (targetY + currentLayout.RoomSpacing) ; y++)
            {
                try
                {
                    if (dungeonMap[x, y] == 1)
                    {
                        tilesThatHallwayAnchorsCannotOccupy++;
                    }
                }
                catch (System.IndexOutOfRangeException)
                {
                    continue;
                }
            }
        }
        if (tilesThatHallwayAnchorsCannotOccupy == 0)
        {
            Debug.Log("The anchor can be placed!");
            return true;
        }
        else
        {
            Debug.Log("The anchor cannot be placed.");
            return false;
        }
    }

    private int[,] CombineTwoDimensionalArrays(int[,] baseArray, int[,] arrayToAdd, int xOffset = 0, int yOffset = 0,  int ignoredValue = -1)
    {
        int[,] newArray = baseArray;
        for (int x = 0; x < Get2DArraySizeX(baseArray); x++)
        {
            for (int y = 0; y < Get2DArraySizeY(baseArray); y++)
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

        return newArray;
    }

}
