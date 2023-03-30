using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

public enum TileType
{
    Empty,
    White,
    Green,
    Red
}
public class GridBuildingSystem : MonoBehaviour
{
    public static GridBuildingSystem current;


    public GridLayout gridLayout;
    public Tilemap mainTilemap;
    public Tilemap tempTilemap;

    private static Dictionary<TileType, TileBase> tileBases = new Dictionary<TileType, TileBase>();

    private Building temp;
    private Vector3 prevPos;
    private BoundsInt prevArea;
    #region Unity Methods

    private void Awake()
    {
        current = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        string tilePath = @"Tiles\";
        tileBases.Add(TileType.Empty, null);
        tileBases.Add(TileType.White, Resources.Load<TileBase>(path:tilePath+"white"));
        tileBases.Add(TileType.Green, Resources.Load<TileBase>(path: tilePath + "green"));
        tileBases.Add(TileType.Red, Resources.Load<TileBase>(path: tilePath + "red"));
    }

    // Update is called once per frame
    void Update()
    {
        if (!temp)
        {
            return;
        }
        if (Input.GetMouseButtonDown(0))
        {
            if (temp.CanBePlaced())
            {
                temp.Place();
            }
        }else if (Input.GetMouseButtonDown(1))
        {
            ClearArea();
            Destroy(temp.gameObject);
        }
        if (temp!=null &&!temp.Placed)
        {
            Vector2 touchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int cellPos = gridLayout.LocalToCell(touchPos);
            {
                if (prevPos != cellPos)
                {
                    temp.transform.localPosition = gridLayout.CellToLocalInterpolated(cellPos + new Vector3(.5f, .5f, 0f));
                    prevPos = cellPos;
                    FollowBuilding();
                }
            }
        }
    }
    #endregion
    #region Tilemap Management
    private static TileBase[] GetTilesBlock(BoundsInt area, Tilemap tilemap)
    {
        TileBase[] array = new TileBase[area.size.x * area.size.y * area.size.z];
        int counter = 0;
        foreach(var v in area.allPositionsWithin){
            Vector3Int pos = new Vector3Int(v.x, v.y, 0);
            array[counter] = tilemap.GetTile(pos);
            counter++;
        }
        return array;
    }

    private static void SetTileBlock(BoundsInt area, TileType type, Tilemap tilemap)
    {
        int size = area.size.x * area.size.y * area.size.z;
        TileBase[] tileArray = new TileBase[size];
        FillTiles(tileArray, type);
        tilemap.SetTilesBlock(area, tileArray);
    }
    //Fill tiles with the given type
    private static void FillTiles(TileBase[] arr, TileType type)
    {
        for(int i = 0; i < arr.Length; i++)
        {
            arr[i] = tileBases[type];
        }
    }

    #endregion

    #region Building Placement
    //Selected building from the production panel gets instantiated here to build
    public void InitializeWithBuilding(GameObject building)
    {
        temp = Instantiate(building, Vector3.zero, Quaternion.identity).GetComponent<Building>();
        temp.GetComponent<Collider2D>().enabled = false;
        InformationPanel.Instance.ClearInfo();
        FollowBuilding();
    }

    private void ClearArea()
    {
        TileBase[] toClear = new TileBase[prevArea.size.x * prevArea.size.y * prevArea.size.z];
        FillTiles(toClear, TileType.Empty);
        tempTilemap.SetTilesBlock(prevArea, toClear);
    }

    //Grid system follows the building
    private void FollowBuilding()
    {
        ClearArea();

        temp.area.position = gridLayout.WorldToCell(temp.gameObject.transform.position);
        BoundsInt buildingArea = temp.area;
        TileBase[] baseArray = GetTilesBlock(buildingArea, mainTilemap);

        int size = baseArray.Length;
        TileBase[] tileArray = new TileBase[size];

        for(int i = 0; i < baseArray.Length; i++)
        {
            if(baseArray[i]== tileBases[TileType.White])
            {
                tileArray[i] = tileBases[TileType.Green];
            }
            else
            {
                FillTiles(tileArray, TileType.Red);
                break;

            }
        }
        tempTilemap.SetTilesBlock(buildingArea, tileArray);
        prevArea = buildingArea;
    }

    //Gets the value if this area is clear to build
    public bool CanTakeArea(BoundsInt area)
    {
        TileBase[] baseArray = GetTilesBlock(area, mainTilemap);
        foreach(var b in baseArray)
        {
            if (b != tileBases[TileType.White])
            {
                return false;
            }
        }
        return true;
    }
    //Changed the placed buildings tile colors and ReScan A* area
    public void TakeArea(BoundsInt area)
    {
        SetTileBlock(area, TileType.Empty, tempTilemap);
        SetTileBlock(area, TileType.Green, mainTilemap);
        temp = null;

        FindObjectOfType<AstarPath>().Scan();
    }

    //Change areas color as selected
    public void SelectArea(BoundsInt area)
    {
        SetTileBlock(area, TileType.Green, tempTilemap);
    }
    //Change areas color as Deselected
    public void DeselectArea(BoundsInt area)
    {
        SetTileBlock(area, TileType.Empty, tempTilemap);
    }
    //Change the area color to default value
    public void DestroyArea(BoundsInt area)
    {
        SetTileBlock(area, TileType.White, mainTilemap);
    }
    #endregion

    public bool IsHoldingBuilding()
    {
        if (temp == null)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
}
