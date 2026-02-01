using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapLoader : MonoBehaviour
{
    [SerializeField] GameObject tilePrefab;
    [SerializeField] Transform tileContainer;
    const int BLOCK_SIZE = 64;
    List<LdtkLevel> gridTilesByLevel = new();

    void Start()
    {
        gridTilesByLevel.Clear();
        gridTilesByLevel = ExtractGridTiles("Maps/Level one");

        foreach (LdtkLevel level in gridTilesByLevel)
        {
            Debug.Log($"Level: {level.identifier}, layer count: {level.layerInstances.Length}");
            foreach (LdtkLayer layer in level.layerInstances)
            {
                Debug.Log($"  Layer: {layer.__identifier}, tile count: {layer.gridTiles.Length}, entity count: {layer.entityInstances.Length}");
            }
        }
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

        PutPlayerOnStartPt(lvl);

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
                Vector3 position = new Vector3(tile.px[0] / BLOCK_SIZE, tile.px[1] / BLOCK_SIZE, 0);
                GameObject tileGO = Instantiate(tilePrefab, position, Quaternion.identity, tileContainer);
                // Additional setup for tileGO can be done here

                // set block color base on tile.src
                int colorCode1 = tile.src[0];
                int colorCode2 = tile.src[1];
                // todo
            }
        }
    }

    public void PutPlayerOnStartPt(int lvl)
    {
        // put player at start position
        foreach (LdtkLayer layer in gridTilesByLevel[lvl].layerInstances)
        {
            if (layer.__identifier == "Entities")
            {
                var playerEntity = layer.entityInstances.FirstOrDefault(e => e.px != null);
                if (playerEntity != null)
                {
                    Vector3 playerPos = new Vector3(playerEntity.px[0] / BLOCK_SIZE, playerEntity.px[1] / BLOCK_SIZE, 0);
                    // todo access player object and set position
                    GameObject player = GameObject.FindWithTag("Player");
                    if (player != null)
                    {
                        player.transform.position = playerPos;
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


