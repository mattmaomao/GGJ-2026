using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class MapLoader : MonoBehaviour
{
    [SerializeField] GameObject playerObj;

    [Header("Colors")]
    [SerializeField] Color COLOR_BLACK = Color.black;
    [SerializeField] Color COLOR_RED = Color.red;
    [SerializeField] Color COLOR_BLUE = Color.blue;
    [SerializeField] Color COLOR_GREEN = Color.green;
    [SerializeField] Color COLOR_YELLOW = Color.yellow;
    [SerializeField] Color COLOR_CYAN = Color.cyan;
    [SerializeField] Color COLOR_MAGENTA = Color.magenta;
    // for empty space place holder
    [SerializeField] Color COLOR_NULL = Color.white;

    [Header("Settings")]
    [SerializeField] BackgroundSprite backgroundSprite;
    [SerializeField] GameObject tilePrefab;
    [SerializeField] Transform tileContainer;
    const string FILEPATH = "Maps/level one for real";
    const int BLOCK_SIZE = 8;
    List<LdtkLevel> gridTilesByLevel = new();

    void Start()
    {
        gridTilesByLevel.Clear();
        gridTilesByLevel = ExtractGridTiles(FILEPATH);

        // todo call init level with game manager
        InitLevel(1);
    }

    public List<LdtkLevel> ExtractGridTiles(string jsonPath)
    {
        string json = Resources.Load<TextAsset>(jsonPath).text;
        LdtkRoot root = JsonUtility.FromJson<LdtkRoot>(json);

        return root.levels.ToList();
    }

    // Implementation for initializing the level based on lvl parameter
    public void InitLevel(int lvl)
    {
        // Clear existing tiles
        for (int i = tileContainer.childCount - 1; i >= 0; i--)
            Destroy(tileContainer.GetChild(i).gameObject);

        // spawn tiles
        SpawnTiles(lvl);

        // put player at start position
        PutPlayerOnStartPt(lvl);

        backgroundSprite.ReLoadMaskableObj();
        tileContainer.gameObject.SetActive(true);
    }

    // Implementation for spawning tiles based on extracted data
    public void SpawnTiles(int lvl)
    {
        if (lvl < 0 || lvl >= gridTilesByLevel.Count)
        {
            Debug.LogError("Invalid level index");
            return;
        }

        LdtkLevel level = gridTilesByLevel[lvl];

        foreach (LdtkLayer layer in level.layerInstances)
        {
            foreach (LdtkGridTile tile in layer.gridTiles)
            {
                // todo check src to determine tile/ object, then spawn object (button)

                // spawn platform
                Vector2 position = new Vector2(tile.px[0] / BLOCK_SIZE, -tile.px[1] / BLOCK_SIZE);
                GameObject tileGO = Instantiate(tilePrefab, position, Quaternion.identity, tileContainer);

                // Additional setup for tileGO can be done here
                PlatformController platCon = tileGO.GetComponent<PlatformController>();
                platCon.Init(position, GetColorWithCode(tile.src), GetColorTypeWithCode(tile.src));
            }
        }
    }

    // hard code color code ***might need to be changed
    Color GetColorWithCode(int[] code)
    {
        switch (code[0])
        {
            case 21:
                return code[1] == 11 ? COLOR_BLACK : COLOR_YELLOW;
            case 31:
                return code[1] == 11 ? COLOR_RED : COLOR_CYAN;
            case 41:
                return code[1] == 11 ? COLOR_BLUE : COLOR_MAGENTA;
            case 51:
                return code[1] == 21 ? COLOR_GREEN : COLOR_NULL;
        }
        return COLOR_NULL;
    }

    // hard code color code ***might need to be changed
    PlatformColorType GetColorTypeWithCode(int[] code)
    {
        switch (code[0])
        {
            case 21:
                return code[1] == 11 ? PlatformColorType.Black : PlatformColorType.Yellow;
            case 31:
                return code[1] == 11 ? PlatformColorType.Red : PlatformColorType.Cyan;
            case 41:
                return code[1] == 11 ? PlatformColorType.Blue : PlatformColorType.Magenta;
            case 51:
                return code[1] == 21 ? PlatformColorType.Green : PlatformColorType.Null;
        }
        return PlatformColorType.Null;
    }

    public void PutPlayerOnStartPt(int lvl)
    {
        // put player at start position
        foreach (LdtkLayer layer in gridTilesByLevel[lvl].layerInstances)
        {
            if (layer.__identifier == "Entities")
            {
                Debug.Log(layer.entityInstances);
                if (layer.entityInstances != null && layer.entityInstances.Length > 0)
                {
                    var entity = layer.entityInstances[0];
                    Vector2 playerPos = new Vector2(entity.px[0] / BLOCK_SIZE, -entity.px[1] / BLOCK_SIZE);
                    // todo access player object and set position
                    if (playerObj != null)
                    {
                        playerObj.transform.position = playerPos;
                        return;
                    }
                }
            }
        }
    }

    [System.Serializable]
    public class LdtkRoot
    {
        public LdtkLevel[] levels;
    }

    [System.Serializable]
    public class LdtkLevel
    {
        public string identifier;
        public LdtkLayer[] layerInstances;
    }

    [System.Serializable]
    public class LdtkLayer
    {
        public string __identifier;
        public LdtkGridTile[] gridTiles;
        public LdtkGridEntity[] entityInstances;
    }

    [System.Serializable]
    public class LdtkGridTile
    {
        public int[] px;
        public int[] src;
        public int f;
        public int t;
    }

    [System.Serializable]
    public class LdtkGridEntity
    {
        public int[] px;
    }
}


