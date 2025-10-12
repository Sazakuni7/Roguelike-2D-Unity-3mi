using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GeneracionProcedural : MonoBehaviour
{
    [SerializeField] int width;
    [SerializeField] int height;
    [SerializeField] float smoothness;
    [SerializeField] float seed;
    [SerializeField] public TileBase groundTile, caveTile, spawnerTile;
    [SerializeField] Tilemap groundTilemap, caveTilemap;

    [Header("Caves")]
    [Range(0, 1)]
    [SerializeField] float modifier;

    int[,] map;

    void Start()
    {
        Generation();
    }

    void Generation()
    {
        seed = Random.Range(-10000f, 10000f);
        clearMap();
        groundTilemap.ClearAllTiles();
        map = GenerateArray(width, height, true);
        map = TerrainGeneration(map);
        RenderMap(map, groundTilemap, caveTilemap, groundTile, caveTile, spawnerTile);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Generation();
        }
    }

    public int[,] GenerateArray(int width, int height, bool empty)
    {
        int[,] map = new int[width, height];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                map[x, y] = (empty) ? 0 : 1;
            }
        }
        return map;
    }

    public int[,] TerrainGeneration(int[,] map)
    {
        int perlinHeight;
        for (int x = 0; x < width; x++)
        {
            perlinHeight = Mathf.RoundToInt(Mathf.PerlinNoise(x / smoothness, seed) * height / 2);
            perlinHeight += height / 2;
            for (int y = 0; y < perlinHeight; y++)
            {
                int caveValue = Mathf.RoundToInt(Mathf.PerlinNoise((x * modifier) + seed, (y * modifier) + seed));
                if (caveValue == 1)
                {
                    map[x, y] = 2; // Cueva
                }
                else
                {
                    map[x, y] = 1; // Suelo
                }
            }
        }

        // Generar paredes gruesas en los bordes
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (x < 4 || x >= width - 4 || y < 4) // 4 tiles de grosor
                {
                    map[x, y] = 1; // Suelo (pared)
                }
            }
        }

        return map;
    }

    public void RenderMap(int[,] map, Tilemap groundTileMap, Tilemap caveTilemap, TileBase groundTilebase, TileBase caveTilebase, TileBase spawnerTilebase)
    {
        List<Vector3Int> posiblesSpawners = new List<Vector3Int>();

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (map[x, y] == 1)
                {
                    groundTileMap.SetTile(new Vector3Int(x, y, 0), groundTilebase);

                    // Verificar si es un posible lugar para un SpawnerTile
                    if (y + 1 < height && map[x, y + 1] == 0) // Espacio libre arriba
                    {
                        posiblesSpawners.Add(new Vector3Int(x, y, 0));
                    }
                }
                else if (map[x, y] == 2)
                {
                    caveTilemap.SetTile(new Vector3Int(x, y, 0), caveTilebase);
                }
            }
        }

        // Colocar SpawnerTiles en posiciones aleatorias
        for (int i = 0; i < 6; i++) // Generar 6 SpawnerTiles
        {
            if (posiblesSpawners.Count == 0) break;

            int randomIndex = Random.Range(0, posiblesSpawners.Count);
            Vector3Int spawnerPos = posiblesSpawners[randomIndex];
            groundTileMap.SetTile(spawnerPos, spawnerTilebase);
            GameObject spawnerObject = new GameObject("SpawnerTile");
            Vector3 worldPos = groundTileMap.CellToWorld(spawnerPos) + new Vector3(0.5f, 1f, 0f);
            spawnerObject.transform.position = worldPos;
        //    spawnerObject.transform.position = spawnerPos;
            spawnerObject.tag = "SpawnerTile";

            posiblesSpawners.RemoveAt(randomIndex); // Evitar repetir posiciones
        }
        if (Spawner.Instance != null)
        {
            Spawner.Instance.InicializarPosiciones();
        }
    }

    public int[,] GetMap()
    {
        return map;
    }

    void clearMap()
    {
        groundTilemap.ClearAllTiles();
        caveTilemap.ClearAllTiles();
    }
}