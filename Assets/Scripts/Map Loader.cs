using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class MapLoader : MonoBehaviour
{
    public static MapLoader Instance { get; private set; }

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
    [SerializeField] Transform tileContainer;
    [SerializeField] GameObject tilePrefab;
    [SerializeField] GameObject flagPrefab;
    [SerializeField] GameObject buttonPrefab;
    [SerializeField] List<ColorSwapItem> buttonList;
    [SerializeField] Transform deathZone;
    Vector2 startingPlayerCoor = Vector2.zero;
    const string FILEPATH = "Maps/level one for real";
    const int BLOCK_SIZE = 8;
    List<LdtkLevel> gridTilesByLevel = new();

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        gridTilesByLevel.Clear();
        gridTilesByLevel = ExtractGridTiles(FILEPATH);
    }

    void Start()
    {
        playerObj = GameManager.Instance.player.gameObject;
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

        // put player at start position
        startingPlayerCoor = CalPlayerOnStartPt(lvl);
        GameManager.Instance.StoreInitialState(Vector2.zero);
        playerObj.transform.position = Vector2.zero;

        // spawn tiles
        SpawnTiles(lvl, startingPlayerCoor);

        backgroundSprite.ReLoadMaskableObj();
        tileContainer.gameObject.SetActive(true);
    }

    // Implementation for spawning tiles based on extracted data
    public void SpawnTiles(int lvl, Vector2 startCoor)
    {
        if (lvl < 0 || lvl >= gridTilesByLevel.Count)
        {
            Debug.LogError("Invalid level index");
            return;
        }

        LdtkLevel level = gridTilesByLevel[lvl];
        buttonList.Clear();

        float lowestY = float.MaxValue;
        foreach (LdtkLayer layer in level.layerInstances)
        {
            foreach (LdtkGridTile tile in layer.gridTiles)
            {
                // todo check src to determine tile/ object, then spawn object (button)
                Vector2 position = new Vector2(tile.px[0] / BLOCK_SIZE - startCoor.x, -tile.px[1] / BLOCK_SIZE - startCoor.y);
                if (position.y < lowestY)
                    lowestY = position.y;

                if (isPlatform(tile.src))
                {
                    // spawn platform
                    GameObject tileGO = Instantiate(tilePrefab, position, Quaternion.identity, tileContainer);

                    // Additional setup for tileGO can be done here
                    PlatformController platCon = tileGO.GetComponent<PlatformController>();
                    platCon.Init(position, GetColorWithCode(tile.src), GetColorTypeWithCode(tile.src));
                }
                else if (isButton(tile.src))
                {
                    // spawn buttons
                    GameObject buttonGO = Instantiate(buttonPrefab, position, Quaternion.identity, tileContainer);
                    ColorSwapItem colorSwapItem = buttonGO.GetComponent<ColorSwapItem>();
                    buttonList.Add(colorSwapItem);
                    colorSwapItem.Init(GetBtnColorIndexWithCode(tile.src), GetBtnDestroyAfterCollectionWithCode(tile.src));
                }
                else if (isFlag(tile.src))
                {
                    // spawn flag
                    Instantiate(flagPrefab, position, Quaternion.identity, tileContainer);
                }
            }
        }

        // put death zone under lowest platform
        deathZone.position = new Vector2(0, lowestY - 10f);
    }

    // todo find flag code
    bool isFlag(int[] code)
    {
        return code[0] == 41 && code[1] == 41;
    }

    bool isPlatform(int[] code)
    {
        if (code[1] == 11)
            if (code[0] == 21 || code[0] == 31 || code[0] == 41)
                return true;
        if (code[1] == 21)
            if (code[0] == 21 || code[0] == 31 || code[0] == 41 || code[0] == 51)
                return true;
        if (code[1] == 31)
            return true;
        return false;
    }

    bool isButton(int[] code)
    {
        if (code[1] == 1)
            return true;
        if (code[1] == 11)
            if (code[0] == 1 || code[0] == 11)
                return true;
        return false;
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

    // hard code color code ***might need to be changed
    int GetBtnColorIndexWithCode(int[] code)
    {
        // cycle button
        if (code[1] == 11)
        {
            switch (code[0])
            {
                case 1:
                    return -1;
                case 11:
                    return -1;
            }
        }

        // color button
        switch (code[0])
        {
            case 1:
                return 0;
            case 11:
                return 1;
            case 21:
                return 2;
            case 31:
                return 3;
            case 41:
                return 4;
            case 51:
                return 5;
        }
        return -1;
    }

    // hard code color code ***might need to be changed
    bool GetBtnDestroyAfterCollectionWithCode(int[] code)
    {
        if (code[0] == 1 && code[1] == 11)
        {
            return true;
        }
        return false;
    }

    public Vector2 CalPlayerOnStartPt(int lvl)
    {
        // put player at start position
        foreach (LdtkLayer layer in gridTilesByLevel[lvl].layerInstances)
        {
            if (layer.__identifier == "Entities")
            {
                if (layer.entityInstances != null && layer.entityInstances.Length > 0)
                {
                    var entity = layer.entityInstances[0];
                    return new Vector2(entity.px[0] / BLOCK_SIZE, -entity.px[1] / BLOCK_SIZE);
                }
            }
        }
        return Vector2.zero;
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


