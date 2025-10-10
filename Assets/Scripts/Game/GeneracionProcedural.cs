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
    [SerializeField] TileBase groundTile, caveTile;
    [SerializeField] Tilemap groundTilemap, caveTilemap;

    [Header("Caves")]
    [Range(0,1)]
    [SerializeField] float modifier;

    int[,] map;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Generation();
    }

    // Update is called once per frame
    void Generation()
    {
        seed = Random.Range(-10000f, 10000f);
        clearMap();
        groundTilemap.ClearAllTiles();
        map = GenerateArray(width, height, true);
        map = TerrainGeneration(map);
        RenderMap(map, groundTilemap, caveTilemap, groundTile, caveTile);

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
                if (x < 3 || x >= width - 3 || y < 3) // 3 tiles de grosor
                {
                    map[x, y] = 1; // Suelo (pared)
                }
            }
        }

        return map;
    }

    public void RenderMap(int[,] map, Tilemap groundTileMap, Tilemap caveTilemap, TileBase groundTilebase, TileBase caveTilebase)
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (map[x, y] == 1)
                {
                    groundTileMap.SetTile(new Vector3Int(x, y, 0), groundTilebase);
                } else if (map[x, y] == 2)
                {
                    caveTilemap.SetTile(new Vector3Int(x, y, 0), caveTilebase);
                }
            }
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
