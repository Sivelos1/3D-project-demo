    using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Stores information that is used to build the dungeon floor.
/// </summary>
/// 
public class DungeonLayoutInformation : MonoBehaviour
{
    //Public variables for the purpose of communicating with other classes
    #region ProxyVariables
    public DungeonTilesetInfo WallTiles
    {
        get { return wallTiles; }
        private set { }
    }
    public DungeonTilesetInfo FloorTiles
    {
        get { return floorTiles; }
        private set { }
    }
    public DungeonTilesetInfo TerrainTiles
    {
        get { return terrainTiles; }
        private set { }
    }
    //-------------------------------------//
    public int StartingFloor
    {
        get { return startingFloor; }
        private set { }
    }
    public int FinalFloor
    {
        get { return finalFloor; }
        private set { }
    }
    //-------------------------------------//
    public int FloorSize
    {
        get { return floorSize; }
        private set { }
    }
    public int FloorOutlineThickness
    {
        get { return floorOutlineThickness; }
        private set { }
    }
    public int WallTerrainFrequency
    {
        get { return wallTerrainFrequency; }
        private set { }
    }
    public int FloorTerrainFrequency
    {
        get { return floorTerrainFrequency; }
        private set { }
    }
    public int RoomGenerationFailureLimit
    {
        get { return roomGenerationFailureLimit; }
        private set { }
    }
    public int MinimumRoomSizeX
    {
        get { return minimumRoomSizeX; }
        private set { }
    }
    public int MinimumRoomSizeY
    {
        get { return minimumRoomSizeY; }
        private set { }
    }
    public int MaxRoomSizeX
    {
        get { return maxRoomSizeX; }
        private set { }
    }
    public int MaxRoomSizeY
    {
        get { return maxRoomSizeY; }
        private set { }
    }
    public int MinimumRoomCount
    {
        get { return minimumRoomCount; }
        private set { }
    }
    public int MaxRoomCount
    {
        get { return maxRoomCount; }
        private set { }
    }
    public int RoomSpacing
    {
        get { return roomSpacing; }
        private set { }
    }
    public float ChanceOfHallwayTwistingAway
    {
        get { return chanceOfHallwayTwistingAway; }
        private set { }
    }
    #endregion

    #region tilesetInfo
    [SerializeField]
    [Tooltip("Used to provide barriers that give structure to the dungeon.")]
    private DungeonTilesetInfo wallTiles;

    [SerializeField]
    [Tooltip("Used to create a platform for the player to stand on.")]
    private DungeonTilesetInfo floorTiles;
    
    [SerializeField]
    [Tooltip("Used for non-floor tiles. May be a puddle of water, or a chasm.")]
    private DungeonTilesetInfo terrainTiles;
    #endregion

    #region floorToFloorInformation
    [SerializeField]
    [Tooltip("The floor that this layout will start being used by the DungeonController. If there are multiple Layouts that can be used during the same range of floors, one will be chosen randomly.")]
    private int startingFloor;

    [SerializeField]
    [Tooltip("The floor that, upon reaching the end of a floor, the DungeonController will proceed to a new DungeonLayout.")]
    private int finalFloor;
    #endregion

    #region floorStructureInformation
    [SerializeField]
    [Tooltip("Creates a square of size X in tiles for the dungeon floor to be built in.")]
    private int floorSize = 50;

    [SerializeField]
    [Tooltip("Surrounds the square created by floorSize with an outline of tiles that encompasses the dungeon. The player cannot interact with these tiles.")]
    private int floorOutlineThickness = 5;

    [SerializeField]
    [Tooltip("Determines how often Terrain tiles will replace Wall Tiles. If it is less than or equal to 0, Wall tiles will not be replaced by Terrain tiles.")]
    private int wallTerrainFrequency = 5;
    [SerializeField]
    [Tooltip("Determines how often Terrain tiles will replace Floor Tiles. If it is less than or equal to 0, Floor tiles will not be replaced by Terrain Tiles.")]
    private int floorTerrainFrequency = 0;

    [SerializeField]
    [Tooltip("How many times the dungeon is allowed to fail positioning a room before trying over from stratch.")]
    [Min(1)]
    private int roomGenerationFailureLimit = 10;

    [SerializeField]
    [Tooltip("The amount of tiles that a room's width cannot be lower than.")]
    [Min(1)]
    private int minimumRoomSizeX = 5;
    [SerializeField]
    [Tooltip("The amount of tiles that a room's depth cannot be lower than.")]
    [Min(1)]
    private int minimumRoomSizeY = 5;


    [SerializeField]
    [Tooltip("The amount of tiles that a room's width cannot exceed.")]
    private int maxRoomSizeX = 10;
    [SerializeField]
    [Tooltip("The amount of tiles that a room's width cannot exceed.")]
    private int maxRoomSizeY = 10;

    [SerializeField]
    [Tooltip("The minimum amount of Rooms that must be generated per Dungeon.")]
    private int minimumRoomCount = 2;
    [SerializeField]
    [Tooltip("The highest amount of Rooms that will be generated for the floor.")]
    private int maxRoomCount = 6;

    [SerializeField]
    [Tooltip("Two Rooms must have at least X tiles between each other.")]
    private int roomSpacing = 1;

    [SerializeField]
    [Tooltip("The amount of tiles a new hallway anchor will be placed when generating hallways.")]
    [Min(1)]
    private int hallwayAnchorMaxDistance = 2;

    [SerializeField]
    [Tooltip("The closer this value is to 0, the higher the chance of a hallway making a turn away from its goal. Makes hallways twistier. Does not affect tiles connecting hallways and rooms.")]
    [Min(0)]
    private float chanceOfHallwayTwistingAway = 1;
    #endregion

}
