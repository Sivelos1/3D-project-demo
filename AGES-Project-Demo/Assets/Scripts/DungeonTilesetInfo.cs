using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// This class stores gameObjects that will be used to build a dungeon. Can only store one type of tile - for example, one instance of DungeonTilesetInfo could only store tiles for the floor of a dungeon. 
/// A separate DungeonTilesetInfo would be needed for things like walls. For each list, a tile will be picked randomly from the list if there is more than one item in the list.
/// Adjacent tiles of the same type will connect together.
/// </summary>
public class DungeonTilesetInfo : MonoBehaviour
{
    //These tiles are used for large groups of adjacent tiles - to create an outline, and to fill said outline.
    #region GroupBorderTiles
    [SerializeField]
    [Tooltip("Style Map Index 1. These tiles are used in the following situation:" +
        "\n X = Blank Space or tiles that are of a different type, O = Adjacent Tiles of the same type, and T = the tile that will be placed." +
        "\n XXX" +
        "\n OTO" +
        "\n OOO")]
    private List<GameObject> border = new List<GameObject>(); //1

    [SerializeField]
    [Tooltip("Style Map Index 2. These tiles are used in the following situation:" +
        "\n X = Blank Space or tiles that are of a different type, O = Adjacent Tiles of the same type, and T = the tile that will be placed." +
        "\n XXX" +
        "\n XOO" +
        "\n XOT")]
    private List<GameObject> borderCorner = new List<GameObject>(); //2

    [SerializeField]
    [Tooltip("Style Map Index 3, though also referenced as the default tile. These tiles are used in the following situation:" +
        "\n X = Blank Space or tiles that are of a different type, O = Adjacent Tiles of the same type, and T = the tile that will be placed." +
        "\n OOO" +
        "\n OTO" +
        "\n OOO")]
    private List<GameObject> surrounded = new List<GameObject>(); //3
    #endregion

    //These tiles are used for lengths of tiles that are typically one-tile thick. 
    #region IslandTiles
    [SerializeField]
    [Tooltip("Style Map Index 4. These tiles are used in the following situation:" +
        "\n X = Blank Space or tiles that are of a different type, O = Adjacent Tiles of the same type, and T = the tile that will be placed." +
        "\n XXX" +
        "\n OTO" +
        "\n XXX")]
    private List<GameObject> iFormation = new List<GameObject>(); //4

    [SerializeField]
    [Tooltip("Style Map Index 5. These tiles are used in the following situation:" +
        "\n X = Blank Space or tiles that are of a different type, O = Adjacent Tiles of the same type, and T = the tile that will be placed." +
        "\n XOX" +
        "\n XTO" +
        "\n XXX")]
    private List<GameObject> lFormation = new List<GameObject>(); //5

    [SerializeField]
    [Tooltip("Style Map Index 6. These tiles are used in the following situation:" +
        "\n X = Blank Space or tiles that are of a different type, O = Adjacent Tiles of the same type, and T = the tile that will be placed." +
        "\n XXX" +
        "\n XTX" +
        "\n XXX")]
    private List<GameObject> island = new List<GameObject>(); //6
    #endregion

    //These tiles are used for intersections of tiles, where tiles are placed on each side of a tile and continue in that direction - similar to the shape of a +.
    #region plusTiles
    [SerializeField]
    [Tooltip("Style Map Index 7. These tiles are used in the following situation:" +
        "\n X = Blank Space or tiles that are of a different type, O = Adjacent Tiles of the same type, and T = the tile that will be placed." +
        "\n XOX" +
        "\n OTO" +
        "\n XOX")]
    private List<GameObject> plusFormation = new List<GameObject>(); //7

    [SerializeField]
    [Tooltip("Style Map Index 8. These tiles are used in the following situation:" +
        "\n X = Blank Space or tiles that are of a different type, O = Adjacent Tiles of the same type, and T = the tile that will be placed." +
        "\n XXX" +
        "\n XTO" +
        "\n XXX")]
    private List<GameObject> plusEdge = new List<GameObject>(); //8
    #endregion

    //These tiles are used at intersections shaped like the letter T.
    #region tTiles
    [SerializeField]
    [Tooltip("Style Map Index 9. These tiles are used in the following situation:" +
        "\n X = Blank Space or tiles that are of a different type, O = Adjacent Tiles of the same type, and T = the tile that will be placed." +
        "\n XXX" +
        "\n OTO" +
        "\n XOX")]
    private List<GameObject> tFormation = new List<GameObject>(); //9

    [SerializeField]
    [Tooltip("Style Map Index 10. These tiles are used in the following situation:" +
        "\n X = Blank Space or tiles that are of a different type, O = Adjacent Tiles of the same type, and T = the tile that will be placed." +
        "\n OOO" +
        "\n OTO" +
        "\n XOX")]
    private List<GameObject> boldTFormation = new List<GameObject>(); //10
    #endregion

    //Other tile formations that don't belong in other groups.
    #region miscellaneousTiles
    [SerializeField]
    [Tooltip("Style Map Index 11. These tiles are used in the following situation:" +
        "\n X = Blank Space or tiles that are of a different type, O = Adjacent Tiles of the same type, and T = the tile that will be placed." +
        "\n OOO" +
        "\n OTO" +
        "\n OOX")]
    private List<GameObject> bulkyCorner = new List<GameObject>(); //11

    [SerializeField]
    [Tooltip("Style Map Index 12. These tiles are used in the following situation:" +
        "\n X = Blank Space or tiles that are of a different type, O = Adjacent Tiles of the same type, and T = the tile that will be placed." +
        "\n OOO" +
        "\n OTO" +
        "\n OOX")]
    private List<GameObject> flagStyle = new List<GameObject>(); //12

    [SerializeField]
    [Tooltip("Style Map Index 13. These tiles are used in the following situation:" +
        "\n X = Blank Space or tiles that are of a different type, O = Adjacent Tiles of the same type, and T = the tile that will be placed." +
        "\n OOO" +
        "\n OTO" +
        "\n XOO")]
    private List<GameObject> reverseFlag = new List<GameObject>(); //13

    [SerializeField]
    [Tooltip("Style Map Index 14. These tiles are used in the following situation:" +
        "\n X = Blank Space or tiles that are of a different type, O = Adjacent Tiles of the same type, and T = the tile that will be placed." +
        "\n XOX" +
        "\n OTO" +
        "\n XOO")]
    private List<GameObject> leotardFormation = new List<GameObject>(); //14

    [SerializeField]
    [Tooltip("Style Map Index 15. These tiles are used in the following situation:" +
        "\n X = Blank Space or tiles that are of a different type, O = Adjacent Tiles of the same type, and T = the tile that will be placed." +
        "\n OOX" +
        "\n OTO" +
        "\n XOO")]
    private List<GameObject> diamondFormation = new List<GameObject>(); //15
    #endregion

    public GameObject GetTile(int tileType)
    {
        Random RNG = new Random();
        switch (tileType)
        {
            case 1:
                return border[Random.Range(0, border.Count)];
            case 2:
                return borderCorner[Random.Range(0, borderCorner.Count)];
            case 3:
                return surrounded[Random.Range(0, borderCorner.Count)];
            case 4:
                return iFormation[Random.Range(0, iFormation.Count)];
            case 5:
                return lFormation[Random.Range(0, lFormation.Count)];
            case 6:
                return island[Random.Range(0, island.Count)];
            case 7:
                return plusFormation[Random.Range(0, plusFormation.Count)];
            case 8:
                return plusEdge[Random.Range(0, plusEdge.Count)];
            case 9:
                return tFormation[Random.Range(0, tFormation.Count)];
            case 10:
                return boldTFormation[Random.Range(0, boldTFormation.Count)];
            case 11:
                return bulkyCorner[Random.Range(0, bulkyCorner.Count)];
            case 12:
                return flagStyle[Random.Range(0, flagStyle.Count)];
            case 13:
                return reverseFlag[Random.Range(0, reverseFlag.Count)];
            case 14:
                return leotardFormation[Random.Range(0, leotardFormation.Count)];
            case 15:
                return diamondFormation[Random.Range(0, diamondFormation.Count)];
            default:
                return surrounded[Random.Range(0, surrounded.Count)];
        }
    }
}
